using UnityEngine;

public class PartData
{
    public int id { get; private set; }
    public Vector3 position { get; private set; }
    public Vector3 rotation { get; private set; }

    public PartData(int id, Vector3 position, Vector3 rotation = new Vector3())
    {
        this.id = id;
        this.position = position;
        this.rotation = rotation;
    }

    public PartData(string partData = "")
    {
        if (string.IsNullOrEmpty(partData))
            return;
        string[] data = partData.Split(',');

        // Reading all data from splitted string
        id = int.Parse(data[0]);
        position = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
        rotation = new Vector3(float.Parse(data[4]), float.Parse(data[5]), float.Parse(data[6]));
    }

    public override string ToString()
    {
        string str = "";
        str += id + ",";
        str += (int) position.x + "," + (int) position.y + "," + (int) position.z + ",";
        str += (int) rotation.x + "," + (int) rotation.y + "," + (int) rotation.z + ",";

        return str;
    }
}
