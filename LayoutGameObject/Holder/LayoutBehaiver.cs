using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace LayoutGameObject
{
    public class LayoutBehaiver : MonoBehaviour
    {
        [HideInInspector]
        public LayoutItem layoutItem;
        [Range(0.1f,10)]
        public float gizmosSize = 1;
        public bool delyCreate;
        public int delyCreateCount_Once;
        private void Start()
        {
            delyCreateCount_Once = delyCreateCount_Once == 0 ? 1 : delyCreateCount_Once;

            var points = LayoutUtil.CalcutePoints(layoutItem);

            if (delyCreate)
            {
                StartCoroutine(DelyCreateItems(points));
            }
            else
            {
                DreateItems(points);
            }
        }

        private void DreateItems(Vector3[] points)
        {
            foreach (var item in points)
            {
                CreateOne(item);
            }
        }

        private IEnumerator DelyCreateItems(Vector3[] points)
        {
            int currentIndex = 0;

            if (delyCreateCount_Once == 0)
                delyCreateCount_Once = 1;

            while (currentIndex < points.Length)
            {
                yield return null;
                for (int i = 0; i < delyCreateCount_Once; i++)
                {
                    if(currentIndex < points.Length)
                    {
                        CreateOne(points[currentIndex]);
                        currentIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
           
        }
        
        private void CreateOne(Vector3 pos)
        {
            var item = Instantiate(layoutItem.prefab);
            item.transform.SetParent(transform);
            item.transform.localPosition = pos;
            item.gameObject.SetActive(true);
        }

        private void OnDrawGizmos()
        {
            if (layoutItem == null) return;

            if (layoutItem.bounds != null)
            {
                foreach (var bound in layoutItem.bounds)
                {
                    Gizmos.DrawWireCube(bound.center + transform.position, bound.size);
                }
            }

            if (layoutItem.count.x > 0 && layoutItem.count.y > 0 && layoutItem.count.z > 0)
            {
                var points = LayoutUtil.CalcutePoints(layoutItem);
                foreach (var item in points)
                {
                    Gizmos.DrawSphere(item + transform.position, gizmosSize);
                }
            }
        }
    }

}