using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing building nodes
/// </summary>
/// 
/// Local position normalised (or *2) is 'direction' and can also be added to partent position 
/// to obtain the position of the node's 'cube'

public class Node : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Returns true if this Node is directly attached to Unit.
    /// </summary>
    public bool IsAttachedToUnit { get; set; }

    /// <summary>
    /// Is node attached to any Part or Unit.
    /// </summary>
    public bool IsAvailable
    {
        get
        {
            return AttachedPart == null && !IsAttachedToUnit;
        }
    }

    /// <summary>
    /// Part that is attached to this Node. Null when Node is not attached to a Part.
    /// </summary>
    public Part AttachedPart { get; private set; }

    /// <summary>
    /// Position relative to Unit where a Part attached to this node should be placed.
    /// </summary>
    public Vector3 AttachmentPosition
    {
        get
        {
            if(transform.parent.parent.parent == null)
            {
                return transform.localPosition.normalized;
            }

            //return transform.parent.parent.localToWorldMatrix * transform.localPosition.normalized;
            return transform.parent.parent.localPosition + transform.localPosition.normalized;
            //todo for future
            //Transform player = transform.parent.parent.parent;
            //Transform part = transform.parent.parent;
            //return player.worldToLocalMatrix * part.localToWorldMatrix * (transform.localPosition * 2f);
        }
    }

    #endregion

	// Use this for initialization
	void Awake ()
	{

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Force attaches a Part to this Node.
    /// </summary>
    public void ReattachPart(Part part)
    {
        AttachedPart = part;
    }

    /// <summary>
    /// Attaches a Part to this Node.
    /// </summary>
    /// <returns>True is successfuly attached.</returns>
    public bool AttachPart(Part part)
    {
        if (AttachedPart == null)
        {
            AttachedPart = part;
            return true;
        }

        Debug.LogWarning("Trying to attach a Part to a Node already in use.");
        return false;
    }

    /// <summary>
    /// Detaches a Part from this Node.
    /// </summary>
    public bool DetachPart()
    {
        if (AttachedPart != null)
        {
            AttachedPart = null;
            return true;
        }
        
        Debug.LogWarning("Trying to detach a Part from a Node, but there isn't a Part attached.");
        return false;
    }

}
