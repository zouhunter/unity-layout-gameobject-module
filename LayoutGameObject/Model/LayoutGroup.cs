using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace LayoutGameObject
{
    [System.Serializable]
    public class LayoutGroup
    {
        public string title;
        public List<LayoutItem> items = new List<LayoutItem>();
    }

}