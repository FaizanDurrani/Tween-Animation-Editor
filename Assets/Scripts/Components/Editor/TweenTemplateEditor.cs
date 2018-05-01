using System;
using System.Reflection;
using Graphs;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace Components.Editor
{
    [CustomEditor(typeof(TweenTemplate))]
    public class TweenTemplateEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            
            // Draw the default inspector
            base.OnInspectorGUI();

            AnimationTemplateGraph template =
                (AnimationTemplateGraph) serializedObject.FindProperty("_templateGraph").objectReferenceValue;
            if (template.CurrentError != AnimationTemplateGraph.Error.None)
            {
                EditorGUILayout.HelpBox("Invalid Template, Please check the template for any errors",
                    MessageType.Error);
                return;
            }

            var tweenTemplate = ((TweenTemplate) target);
            if (tweenTemplate.Values == null)
            tweenTemplate.Values = new object[template.nodes.Count];
            for (var i = 0; i < template.nodes.Count; i++)
            {
                var editor = CreateEditor(template.nodes[i]);
                var value = editor.serializedObject.FindProperty("Value");

                if (value != null)
                {
                    if (value.propertyType == SerializedPropertyType.ObjectReference)
                        tweenTemplate.Values[i] = EditorGUILayout.ObjectField(
                            editor.serializedObject.FindProperty("Name").stringValue,
                            (Object) tweenTemplate.Values[i], GetType(value), true);
                    else
                    {
                        Debug.Log(value.propertyType);
                        DefaultProperty(value.propertyType,
                            editor.serializedObject.FindProperty("Name").stringValue, ref tweenTemplate.Values[i]);
                    }
                }
            }
            
            
        }

        public static System.Type GetType(SerializedProperty property)
        {
            System.Type parentType = property.serializedObject.targetObject.GetType();
            System.Reflection.FieldInfo fi = parentType.GetField(property.propertyPath);
            return fi.FieldType;
        }

        public void DefaultProperty(SerializedPropertyType propertyType, string label, [NotNull] ref object obj)
        {  switch (propertyType)
            {
                case SerializedPropertyType.Integer:
                    obj = EditorGUILayout.LongField(label, (long) obj);
                    break;
                case SerializedPropertyType.Boolean:
                    obj = EditorGUILayout.Toggle(label, (bool) obj);
                    break;
                case SerializedPropertyType.Float:
                    obj = EditorGUILayout.FloatField(label, (float) obj);
                    break;
                case SerializedPropertyType.String:
                    obj = EditorGUILayout.TextField(label, (string) obj);
                    break;
                case SerializedPropertyType.Color:
                    obj = EditorGUILayout.ColorField(label, (Color) obj);
                    break;
                case SerializedPropertyType.Vector2:
                    obj = EditorGUILayout.Vector2Field(label, (Vector2) obj);
                    break;
                case SerializedPropertyType.Vector3:
                    obj = EditorGUILayout.Vector3Field(label, (Vector3) obj);
                    break;
                default:
                    break;
            }
        }
    }
}