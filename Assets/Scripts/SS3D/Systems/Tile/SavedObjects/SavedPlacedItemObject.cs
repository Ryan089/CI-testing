using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS3D.Systems.Tile
{
    /// <summary>
    /// SaveObject that contains all information required to reconstruct a placed item object.
    /// </summary>
    [Serializable]
    public struct SavedPlacedItemObject
    {
        public string itemName;
        public Vector3 worldPosition;
        public Quaternion rotation;
    }
}