using System.Collections;
using System.Collections.Generic;
using BasicNodes;
using MasterNodes;
using TweenEditor;
using UnityEngine;

public class TweenTemplate : MonoBehaviour
{
	[SerializeField] private bool _onStart;
	[SerializeField] private Transform _target;
	[SerializeField] private AnimationTemplateGraph _templateGraph;

	[HideInInspector] public object[] Values;
	
	private void Start()
	{
		_templateGraph.Start.Transform = _target;
		for (int i = 0; i < _templateGraph.nodes.Count; i++)
		{
			var node = _templateGraph.nodes[i] as ValueNode;
			
			if (node != null)
			{
				node.NodeValue = Values[i];
			}
		}
		_templateGraph.Start.ExecuteTemplate();
	}
}
