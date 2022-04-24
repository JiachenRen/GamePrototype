using UnityEngine;

public abstract class Constants
{
    public abstract class Layers
    {
        public static LayerMask TerrainMask = LayerMask.NameToLayer("Terrain Mask");
    }

    public abstract class AreaType
    {
        public const int NotWalkable = 1;
    }

    public abstract class Tags
    {
        public const string TerrainSurface = "TerrainSurface";
        public const string Weapon = "Weapon";
    }
}