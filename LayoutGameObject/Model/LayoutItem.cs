using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace LayoutGameObject
{
    [System.Serializable]
    public class LayoutItem:ScriptableObject
    {
        public LayoutStyle style;
        public Vector3 span;
        public Vector3Int count;
        public bool enableClip;
        public List<Bounds> bounds = new List<Bounds>();
        public Coordinate parent;
        public GameObject prefab;
    }

}