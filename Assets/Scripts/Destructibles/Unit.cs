using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class for all objects that can have Parts.
/// </summary>
public class Unit : Destructible
{
    [Tooltip("Empty GameObject used to represent aiming direction. Should also be put into NetworkTransformChild component.")]
    public Transform aimTransform;

    protected List<PartData> partsData = new List<PartData>();
    protected List<Weapon> weapons = new List<Weapon>();
    protected Dictionary<Vector3Int, Part> parts = new Dictionary<Vector3Int, Part>();

    // Holds all children that won't be deleted when rebuilding parts
    private readonly List<string> _children = new List<string>();

    #region Properties

    /// <summary>
    /// The direction of aiming relative to Unit's local rotation. Weapons will try to match to this rotation.
    /// </summary>
    public Vector3 Target
    {
        get { return aimTransform.localEulerAngles; }
        set { aimTransform.localEulerAngles = value; }
    }

    /// <summary>
    /// List of PartData parts attached to this Unit.
    /// </summary>
    [Obsolete("Use Parts property instead.", false)]
    public ReadOnlyCollection<PartData> PartsData
    {
        get { return partsData.AsReadOnly(); }
    }
    
    /// <summary>
    /// All parts (position, Part) of this Unit.
    /// </summary>
    public Dictionary<Vector3Int, Part> Parts
    {
        get { return parts; }
    }

    #endregion

    /// <summary>
    /// Invoked when all or some parts changed.
    /// </summary>
    public event EventHandler<EventArgs> PartsChanged;

    // Use this for initialization
    public new void Start()
    {
        base.Start();

        // Making sure that there's an Aim GameObject
        if (!aimTransform)
        {
            if (!(aimTransform = transform.Find("Aim")))
            {
                aimTransform = Instantiate(new GameObject("Aim"), transform.position, transform.rotation, transform).transform;
            }
            GetComponent<NetworkTransformChild>().target = aimTransform;
        }

        // Adding all children to the List that will prevent them from being destroyed
        _children.AddRange(transform.GetComponentsInChildren<Transform>().Select(t => t.name));

        // Handling event
        PartsChanged += OnPartsChanged;

        // Only on server
        if (!isServer)
            return;

    }

    /// <summary>
    /// Tries to shoot from all weapons.
    /// </summary>
    public void Shoot()
    {
        CmdShoot();
    }

    /// <summary>
    /// Returns a List with all available nodes.
    /// </summary>
    /// <param name="onlyAvailable">True will return only available nodes.</param>
    /// <returns></returns>
    public ReadOnlyCollection<Node> GetNodes(bool onlyAvailable)
    {
        List<Node> nodes = new List<Node>();

        // Adding nodes from Unit
        foreach (Node node in transform.Find("Nodes").GetComponentsInChildren<Node>())
        {
            if(node.IsAvailable || !onlyAvailable)
                nodes.Add(node);
        }

        // Adding nodes from parts
        foreach (KeyValuePair<Vector3Int, Part> part in parts)
        {
            part.Value.Nodes.ForEach(n =>
            {
                if(n.IsAvailable || !onlyAvailable)
                    nodes.Add(n);
            });
        }

        return nodes.AsReadOnly();
    }

    /// <summary>
    /// Adds part to Unit.
    /// </summary>
    public void AddPart(PartData partData)
    {
        CmdAddPart(partData.ToString());
    }

    /// <summary>
    /// Removes part from Unit.
    /// </summary>
    public void RemovePart(Vector3 position)
    {
        CmdRemovePart(position);
    }

    /// <summary>
    /// Refreshes the parts List. Rebuilds parts when successful.
    /// </summary>
    public void RefreshParts()
    {
        // We can't just instantiate our parts, we need to load informations about them from the server first
        // Send command to server to refresh parts list
        CmdRefreshParts();
    }

    /// <summary>
    /// Rebuilds game objects from prefabs. Is called automatically when parts are refreshed.
    /// </summary>
    private void RebuildParts()
    {
        // Remove all children
        transform.DestroyChildren(_children.ToArray());
        // Clear parts Dictionary
        Parts.Clear();
        // Loop through parts and instantiate them
        // Note: Those parts are only created locally
        foreach (PartData part in partsData)
        {
            InstantiatePart(part);
        }
        // Invoking event
        if (PartsChanged != null)
            PartsChanged(this, EventArgs.Empty);
    }

    /// <summary>
    /// Instantiates one Part.
    /// </summary>
    private Part InstantiatePart(PartData partData)
    {
        GameObject partGameObject = Instantiate(PartManager.Instance.GetPartById(partData.Id).Prefab, transform.position + partData.Position,
            Quaternion.Euler(transform.localEulerAngles + partData.Rotation), gameObject.transform);
        // Taking Part component
        Part part = partGameObject.GetComponent<Part>();
        // Setting Part Id
        part.SetID(partData.Id);
        // Adding Part to parts Dictionary
        parts.Add(Vector3Int.RoundToInt(partData.Position), part);
        // Returning Part
        return part;
    }

    /// <summary>
    ///  Reattaches all connections for given nodes.
    /// </summary>
    /// <param name="nodes">Nodes to recalculate</param>
    /// <param name="partsChecked">Will add all parts that were checked to this List.</param>
    /// <param name="recursion">Execute function on all parts' nodes that are found.</param>
    /// <returns>True if at least one node is connected to the Unit.</returns>
    //todo should probably take Part as an argument (but Unit is problematic)
    private bool RecalculateAttachments(IEnumerable<Node> nodes, ref List<Part> partsChecked, bool recursion = false)
    {
        bool connection = false;

        foreach (Node node in nodes)
        {
            Part foundPart;
            if (parts.TryGetValue(Vector3Int.RoundToInt(node.AttachmentPosition), out foundPart))
            {
                node.ReattachPart(foundPart);
                if (recursion && !foundPart.Checked)
                {
                    foundPart.Checked = true;
                    partsChecked.Add(foundPart);
                    connection = RecalculateAttachments(foundPart.Nodes, ref partsChecked, true);
                }
            }
            // If node is pointing at Vector3Int.zero (Unit position)
            if (Vector3Int.RoundToInt(node.AttachmentPosition) == Vector3Int.zero)
            {
                node.IsAttachedToUnit = true;
                connection = true;
                if (node.AttachedPart != null)
                    node.DetachPart();
            }
            // Detach a Part if there isn't any now
            if(!foundPart && node.AttachedPart != null)
                node.DetachPart();
        }

        return connection;
    }

    private void RecalculateAllAttachments()
    {
        // Set all Parts as not yet checked and without connection to the Unit
        foreach (KeyValuePair<Vector3Int, Part> keyValuePair in Parts)
        {
            keyValuePair.Value.Checked = false;
        }

        // Recalculate attachments for Unit nodes
        List<Part> temp = new List<Part>();
        RecalculateAttachments(transform.Find("Nodes").GetComponentsInChildren<Node>(), ref temp);

        // Parts to remove
        List<Vector3Int> keys = new List<Vector3Int>();

        // Foreach Parts
        foreach (KeyValuePair<Vector3Int, Part> keyValuePair in Parts)
        {
            Part part = keyValuePair.Value;
            // Continue if already checked
            if(part.Checked)
                continue;
            // Mark part as checked
            part.Checked = true;
            // Hold all parts recursively checked
            List<Part> partsChecked = new List<Part> {part};
            // Recalculate attachments recursively
            if (!RecalculateAttachments(part.Nodes, ref partsChecked, true))
            {
                // If there was no connection to the Unit
                // Keys to remove
                foreach (Part partChecked in partsChecked)
                {
                    partChecked.transform.parent = part.transform;
                    keys.Add(Parts.First(p => p.Value == partChecked).Key);
                }
                
                part.transform.parent = null;
            }
        }
        foreach (Vector3Int key in keys)
        {
            // Removing from PartsData
            partsData.Remove(partsData.First(p => Vector3Int.RoundToInt(p.Position) == key));

            // Remove from Parts
            Parts.Remove(key);
        }

    }

    /// <summary>
    /// Event handler for PartsChanged.
    /// </summary>
    private void OnPartsChanged(object sender, EventArgs e)
    {
        // Revalculate all connections
        RecalculateAllAttachments();
        // Clear weapons List
        weapons.Clear();
        // Add weapons
        weapons.AddRange(GetComponentsInChildren<Weapon>());
    }

    #region Networking

    /// <summary>
    /// Request to add a Part.
    /// </summary>
    /// <param name="part">PartData</param>
    [Command]
    private void CmdAddPart(string part)
    {
        partsData.Add(new PartData(part));
        RpcAddPart(part);
        // Instantiate this part
        InstantiatePart(new PartData(part));
        // Invoking event
        if (PartsChanged != null)
            PartsChanged(this, EventArgs.Empty);
    }

    /// <summary>
    /// Request to remove a Part from position (relative to Unit).
    /// </summary>
    [Command]
    private void CmdRemovePart(Vector3 position)
    {
        // Searching for a Part in Parts
        KeyValuePair<Vector3Int, Part> keyValuePair = Parts.First(p => p.Key == Vector3Int.RoundToInt(position));
        // If a Part was found
        if (!keyValuePair.Equals(default(KeyValuePair<Vector3Int, Part>)))
        {
            // Removing from Parts
            Parts.Remove(keyValuePair.Key);

            // Removing from PartsData
            partsData.Remove(partsData.First(p => Vector3Int.RoundToInt(p.Position) == keyValuePair.Key));

            // Destroying gameObject
            Destroy(keyValuePair.Value.gameObject);

            RpcRemovePart(position);
            // Invoking event
            if (PartsChanged != null)
                PartsChanged(this, EventArgs.Empty);
        }
        else
        {
            // Display warning if there's no Part in this position (server only warning)
            Debug.LogWarning("Trying to remove a non-existent Part (check if removal position is correct).");
        }
    }

    /// <summary>
    /// Request for refreshing parts List.
    /// </summary>
    [Command]
    private void CmdRefreshParts()
    {
        // Converting PartData to string array
        string[] strings = new string[partsData.Count];
        for (int i = 0; i < partsData.Count; i++)
        {
            strings[i] = partsData[i].ToString();
        }
        // Sending parts to client
        RpcSendParts(strings);
    }

    /// <summary>
    /// Send parts to clients.
    /// </summary>
    [ClientRpc]
    private void RpcSendParts(string[] strings)
    {
        // Clearing parts List
        partsData.Clear();
        // Loading PartData from string array
        foreach (string s in strings)
        {
            partsData.Add(new PartData(s));
        }
        // Rebuild parts
        RebuildParts();
    }

    /// <summary>
    /// Requests client to add a new Part.
    /// </summary>
    [ClientRpc]
    private void RpcAddPart(string str)
    {
        // Add a part on clients (skip server, Part was already added in the CmdAddPart)
        if (isServer)
            return;

        partsData.Add(new PartData(str));

        // Instantiate this part
        InstantiatePart(new PartData(str));

        // Invoking event (for server already invoked in Command)
        if (PartsChanged != null)
            PartsChanged(this, EventArgs.Empty);
    }

    /// <summary>
    /// Request client to remove a Part
    /// </summary>
    [ClientRpc]
    private void RpcRemovePart(Vector3 position)
    {
        // Remove on clients
        if (isServer)
            return;

        // Searching for a Part in Parts
        KeyValuePair<Vector3Int, Part>  keyValuePair = Parts.First(p => p.Key == Vector3Int.RoundToInt(position));
        // If a Part was not found
        if (keyValuePair.Equals(default(KeyValuePair<Vector3Int, Part>)))
            return;

        // Removing from Parts
        Parts.Remove(keyValuePair.Key);

        // Destroying gameObject
        Destroy(keyValuePair.Value.gameObject);

        // Removing from PartsData
        partsData.Remove(partsData.First(p => Vector3Int.RoundToInt(p.Position) == keyValuePair.Key));

        // Invoking event (for server already invoked in Command)
        if (PartsChanged != null)
            PartsChanged(this, EventArgs.Empty);
    }

    /// <summary>
    /// Request server to shoot.
    /// </summary>
    [Command]
    private void CmdShoot()
    {
        foreach (Weapon weapon in weapons)
        {
            // Is the weapon ready to shoot
            if (!weapon.Ready())
                continue;
            // Instantiate GameObject
            GameObject shot = Instantiate(weapon.bulletPrefab, weapon.gunTransform.position, weapon.gunTransform.rotation);
            // Spawn it - so it appears for all clients
            NetworkServer.Spawn(shot);
            // Destroy it after some time
            Destroy(shot, 5f);
        }
    }

    #endregion

}