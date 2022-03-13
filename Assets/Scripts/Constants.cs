using UnityEngine;

public interface Constants
{
    public class Layers
    {
        public static LayerMask Terrain = LayerMask.NameToLayer("Terrain");
        public static LayerMask TerrainMask = LayerMask.NameToLayer("Terrain Mask");
    }

    public class AreaType
    {
        public static int NotWalkable = 1;
    }

    public class Tags
    {
        public static string Ground = "Ground";
    }
}