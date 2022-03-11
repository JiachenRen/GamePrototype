using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Constants
{
    public class Layers
    {
        public static LayerMask Terrain = LayerMask.NameToLayer("Terrain");
    }

    public class Tags
    {
        public static string Ground = "Ground";
    }
}