using Graphs;
using UnityEngine;
using ValueNodes;

namespace Components
{
	public class TweenTemplate : MonoBehaviour
	{
		[SerializeField] private bool _executeAtStart;
		[SerializeField] private RectTransform _target;
		[SerializeField] private AnimationTemplateGraph _templateGraph;

		public ReorderableEventList OnComplete;
	
		[HideInInspector] public object[] Values;
	
		private void Start()
		{
			if (_executeAtStart)
				Execute();
		}

		public void Execute()
		{
			for (int i = 0; i < _templateGraph.nodes.Count; i++)
			{
				var node = _templateGraph.nodes[i] as ValueNode;
			
				if (node != null)
				{
					node.NodeValue = Values[i];
				}
			}
			_templateGraph.ExecuteTween(_target, this);
		}
	}
}
