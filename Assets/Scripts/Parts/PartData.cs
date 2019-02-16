using UnityEngine;

/// <summary>
/// Holds information about single Part that is attached to an Unit.
/// </summary>
public class PartData
{
    #region Properties

    /// <summary>
    /// Id of a part.
    /// </summary>
    public int Id { get; private set; }
    
    /// <summary>
    /// Part position relative to local position of an Unit.
    /// </summary>
    public Vector3 Position { get; private set; }

    /// <summary>
    /// Part rotation relative to local rotation of an Unit.
    /// </summary>
    public Vector3 Rotation { get; private set; }

    #endregion

    /// <summary>
    /// Default constructor.
    /// </summary>
    public PartData(int id, Vector3 position, Vector3 rotation = new Vector3())
    {
        Id = id;
        Position = position;
        Rotation = rotation;
    }

    /// <summary>
    /// Constructs PartData from string. Used to synchronize PartData across clients.
    /// </summary>
    /// <param name="partData">id,x-pos,y-pos,z-pos,x-rot,y-rot,z-rot</param>
    public PartData(string partData)
    {
        if (string.IsNullOrEmpty(partData))
            return;
        string[] data = partData.Split(',');

        // Reading all data from splitted string
        Id = int.Parse(data[0]);
        Position = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
        Rotation = new Vector3(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]));
    }

    /// <summary>
    /// Encodes PartData into string. Used to synchronize PartData across clients.
    /// </summary>
    /// <returns>id,x-pos,y-pos,z-pos,x-rot,y-rot,z-rot</returns>
    public override string ToString()
    {
        string str = "";
        str += Id + ",";
        str += (int) Position.x + "," + (int) Position.y + "," + (int) Position.z + ",";
        str += (int) Rotation.x + "," + (int) Rotation.y + "," + (int) Rotation.z + ",";

        return str;
    }
}
