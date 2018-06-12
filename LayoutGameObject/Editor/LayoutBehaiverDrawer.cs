using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace LayoutGameObject
{
    [CustomEditor(typeof(LayoutBehaiver))]
    public class LayoutBehaiverDrawer : Editor
    {
        private LayoutBehaiver layoutBehaiver;
        private Editor layoutItemDrawer;
        private void OnEnable()
        {
            layoutBehaiver = target as LayoutBehaiver;
            InitDrawer(layoutBehaiver.layoutItem);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawItemContent();
            DrawLayoutItem();
        }
        public void InitDrawer(LayoutItem layoutItem)
        {
            if (layoutItem)
            {
                Editor.CreateCachedEditor(layoutItem, typeof(LayoutItemDrawer), ref layoutItemDrawer);
            }
        }

        private void DrawLayoutItem()
        {
            if (layoutBehaiver.layoutItem != null && layoutItemDrawer != null)
            {
                layoutItemDrawer.OnInspectorGUI();
                SaveLayoutItem();
            }
            else
            {
                EditorGUI.HelpBox(GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight), "请创建并关联一个layoutItem对象", MessageType.Warning);
            }
        }

        private void DrawItemContent()
        {
            var rect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, EditorGUIUtility.singleLineHeight);
            var objectRect = layoutBehaiver.layoutItem == null ? new Rect(rect.x, rect.y, rect.width - 60, rect.height):rect;

            EditorGUI.BeginChangeCheck();
            layoutBehaiver.layoutItem = EditorGUI.ObjectField(objectRect, new GUIContent("LayoutItem"), layoutBehaiver.layoutItem, typeof(LayoutItem), false) as LayoutItem;
            if (EditorGUI.EndChangeCheck())
            {
                if (layoutBehaiver.layoutItem != null) {
                    InstallLayoutItemInfo();
                }
                InitDrawer(layoutBehaiver.layoutItem);
            }
            var operateRect = new Rect(rect.x + rect.width - 60, rect.y, 60, rect.height);

            if (layoutBehaiver.layoutItem == null && GUI.Button(operateRect, "create", EditorStyles.miniButtonRight))
            {
                CreateAnLayoutItem();
            }
        }

        /// <summary>
        /// 创建一个layoutItem
        /// </summary>
        private void CreateAnLayoutItem()
        {
            var item = ScriptableObject.CreateInstance<LayoutItem>();
            ProjectWindowUtil.CreateAsset(item, "layoutItem.asset");
        }

        /// <summary>
        /// 保存layoutItem
        /// </summary>
        private void SaveLayoutItem()
        {
            if(layoutBehaiver.layoutItem)
            {
                layoutBehaiver.layoutItem.parent.localPosition = layoutBehaiver.transform.localPosition;
                layoutBehaiver.layoutItem.parent.localScale = layoutBehaiver.transform.localScale;
                layoutBehaiver.layoutItem.parent.localEulerAngles = layoutBehaiver.transform.localEulerAngles;
                EditorUtility.SetDirty(layoutBehaiver.layoutItem);
            }
        }

        /// <summary>
        /// 加载从数据来源的信息
        /// </summary>
        private void InstallLayoutItemInfo()
        {
            layoutBehaiver.transform.localPosition = layoutBehaiver.layoutItem.parent.localPosition;
            layoutBehaiver.transform.localScale = layoutBehaiver.layoutItem.parent.localScale;
            layoutBehaiver.transform.localEulerAngles = layoutBehaiver.layoutItem.parent.localEulerAngles;
        }
    }

}