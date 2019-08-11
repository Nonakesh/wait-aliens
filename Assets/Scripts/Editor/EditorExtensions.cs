using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class EditorExtensions
{
    public static void FixedSizeArray(SerializedProperty property, int elements, string[] elementNames,
        bool inlineArray = false)
    {
        FixedSizeArray(property, elements, elementNames, inlineArray,
            (prop, label) => EditorGUILayout.PropertyField(prop, label, true));
    }

    public static void FixedSizeArray(SerializedProperty property, int elements, string[] elementNames,
        bool inlineArray, Action<SerializedProperty, GUIContent> propertyDrawer)
    {
        ResizeArrayProperty(property, elements);

        if (!inlineArray)
        {
            EditorGUILayout.PropertyField(property, false);
        }

        if (property.isExpanded || inlineArray)
        {
            using (new EditorGUI.IndentLevelScope(inlineArray ? 0 : 1))
            {
                var elemGUIContent = elementNames.Select(x => new GUIContent(x)).ToList();
                for (int i = 0; i < property.arraySize; i++)
                {
                    var elem = property.GetArrayElementAtIndex(i);
                    propertyDrawer(elem, elemGUIContent[i]);
                }
            }
        }
    }

    public static void CustomArrayProperty(SerializedProperty property, Action<SerializedProperty, int> propertyDrawer,
        bool inlineArray = false, bool hideArraySize = false)
    {
        if (!inlineArray)
        {
            EditorGUILayout.PropertyField(property, false);
        }

        if (!property.isExpanded)
        {
            return;
        }

        using (new EditorGUI.IndentLevelScope())
        {
            if (!hideArraySize)
            {
                var arrayLength = property.FindPropertyRelative("Array.size");
                EditorGUILayout.PropertyField(arrayLength);
            }

            if (!property.hasMultipleDifferentValues)
            {
                for (int i = 0; i < property.arraySize; i++)
                {
                    var elem = property.GetArrayElementAtIndex(i);
                    propertyDrawer(elem, i);
                }
            }
        }
    }

    public static void ResizeArrayProperty(SerializedProperty property, int size)
    {
        while (property.arraySize > size)
        {
            property.DeleteArrayElementAtIndex(property.arraySize - 1);
        }

        while (property.arraySize < size)
        {
            property.InsertArrayElementAtIndex(property.arraySize);
        }
    }
}