using System;
using System.Collections;
using System.Collections.Generic;
using Graphs;
using MasterNodes;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(AnimationTemplateGraph))]
public class AnimationTemplateGraphEditor : NodeGraphEditor
{
    private AnimationTemplateGraph _templateGraph;

    public override void OnGUI()
    {
        _templateGraph = (AnimationTemplateGraph) target;
        DrawErrors();
    }

    private void DrawErrors()
    {
        GUILayout.BeginArea(new Rect(0, 0, 300, position.height));
        GUILayout.FlexibleSpace();
        
        switch (_templateGraph.CurrentError)
        {
            case AnimationTemplateGraph.Error.ManyEnd:
                EditorGUILayout.HelpBox("Too many TemplateEnd nodes, must only have one", MessageType.Error);
                break;
            case AnimationTemplateGraph.Error.ManyStart:
                EditorGUILayout.HelpBox("Too many TemplateStart nodes, must only have one", MessageType.Error);
                break;
            case AnimationTemplateGraph.Error.NoStart:
                EditorGUILayout.HelpBox("There's no TemplateStart node, Please add one", MessageType.Error);
                break;
            case AnimationTemplateGraph.Error.NoEnd:
                EditorGUILayout.HelpBox("There's no TemplateEnd node, Please add one", MessageType.Error);
                break;
            case AnimationTemplateGraph.Error.None:
                break;
        }

        GUILayout.EndArea();
    }
}