using System.Linq;
using UnityEngine;

public static class TransformExtension {

    /// <summary>
    /// Destroys all children of this transform. Skips children listed in params skip.
    /// </summary>
    /// <param name="skip">Children to skip</param>
    public static void DestroyChildren(this Transform transform, params string[] skip)
    {
        foreach (Transform child in transform)
        {
            if(skip.Any(s => s == child.name))
                continue;
            GameObject.Destroy(child.gameObject);
        }
    }
}
