using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace LayoutGameObject
{
    public class LayoutObj : ScriptableObject
    {
        public List<LayoutGroup> groups = new List<LayoutGroup>();
    }
}