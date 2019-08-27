using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 RoundXAndYCoords(this Vector3 vec)
    {
        return new Vector3 (Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y), vec.z);
    }
} 