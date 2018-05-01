using System;
using Base;
using Graphs;
using Types;
using UnityEngine;
using XNode;

namespace MasterNodes
{
    public class StartNode : ExtendedNode
    {
        [Output] public TweenData Sequence;
        protected override string GetNodeName() => "Start Node";

        protected override void Init()
        {
            base.Init();
            
            GetGraph<AnimationTemplateGraph>().TweenExecuted += OnTweenExecuted;
        }

        private void OnTweenExecuted(RectTransform rectTransform)
        {
            Sequence = new TweenData(GetGraph<AnimationTemplateGraph>().Sequence);
            UpdateOutputNode(nameof(Sequence));
        }
    }
}