using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 RoundXAndYCoords(this Vector3 vec)
    {
        return new Vector3 (Mathf.Round(vec.x), Mathf.Round(vec.y), vec.z);
    }
} 