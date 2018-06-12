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
    public class LayoutItemDrawer : Editor
    {
        private ReorderableList boundList;
        private SerializedProperty bound_prop;
        private List<string> ignoredProp = new List<string> {
            "bounds","parent"
        };

        private void OnEnable()
        {
            bound_prop = serializedObject.FindProperty("bounds");
            InitList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefults();
            boundList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDefults()
        {
           var iterator = serializedObject.GetIterator();
            var enterChild = true;
            while (iterator.NextVisible(enterChild))
            {
                enterChild = false;
                if (!ignoredProp.Contains(iterator.propertyPath))
                {
                    EditorGUILayout.PropertyField(iterator, true);
                }
            }
        }

        public void InitList()
        {
            boundList = new ReorderableList(serializedObject, bound_prop);
            boundList.drawHeaderCallback = (rect) => { EditorGUI.LabelField(rect, "Bounds"); };
            boundList.elementHeight = 3 * EditorGUIUtility.singleLineHeight + 10;
            boundList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) =>
            {
                if (bound_prop.arraySize > index && index >= 0)
                {
                    var boundProp = bound_prop.GetArrayElementAtIndex(index);
                    var centerProp = boundProp.FindPropertyRelative("center");
                    var sizeProp = boundProp.FindPropertyRelative("size");
                    var reverseProp = boundProp.FindPropertyRelative("reverse");
                    rect = GetBoxRect(rect, "");
                    var reversRect = new Rect(rect.x + 200, rect.y, rect.width - 200, EditorGUIUtility.singleLineHeight);
                    reverseProp.boolValue = EditorGUI.ToggleLeft(reversRect,new GUIContent("Reverse"), reverseProp.boolValue);
                    var labelRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, 60, EditorGUIUtility.singleLineHeight);
                    var contentRect = new Rect(rect.x + 60, rect.y + EditorGUIUtility.singleLineHeight, rect.width - 60, EditorGUIUtility.singleLineHeight);
                    EditorGUI.LabelField(labelRect, "[center]");
                    centerProp.vector3Value = EditorGUI.Vector3Field(contentRect, "", centerProp.vector3Value);
                    contentRect.y += EditorGUIUtility.singleLineHeight;
                    labelRect.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.LabelField(labelRect, "[s i z e]");
                    sizeProp.vector3Value = EditorGUI.Vector3Field(contentRect, "", sizeProp.vector3Value);
                }
            };
        }

        private Rect GetBoxRect(Rect rect, string label)
        {
            var paddingRect = GetPaddingRect(rect, 5);
            var boxRect = GetPaddingRect(rect, 2.5f);
            GUI.Box(boxRect, "");
            var labelRect = new Rect(boxRect.x, boxRect.y, 20, 20);
            EditorGUI.LabelField(labelRect, label);
            return paddingRect;
        }

        private Rect GetPaddingRect(Rect rect, float padding)
        {
            return new Rect(rect.x + padding, rect.y + padding, rect.width - 2 * padding, rect.height - 2 * padding);
        }

    }
}