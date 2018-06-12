using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace LayoutGameObject
{
    [System.Serializable]
    public class Bounds
    {
        public bool reverse;
        public Vector3 size;
        public Vector3 center;

        internal bool Contains(Vector3 point)
        {
            return Contains(point.x, center.x - size.x * 0.5f, center.x + size.x * 0.5f) &&
                Contains(point.y, center.y - size.y * 0.5f, center.y + size.y * 0.5f) &&
                Contains(point.z, center.z - size.z * 0.5f, center.z + size.z * 0.5f);

        }
        private bool Contains(float x,float a,float b)
        {
            if ((x > a && x > b )||(x < a && x < b))
            {
                return false;
            }
            return true;
        }
    }
}