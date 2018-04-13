using Base;
using TweenEditor;
using UnityEngine;
using XNode;

namespace MasterNodes
{
    public class TemplateStart : ExtendedNode
    {
        [Output] public Transform Transform;
        protected override string NodeName => "Start Node";

        public override object GetValue(NodePort port)
        {
            return Transform;
        }

        public void ExecuteTemplate()
        {
            UpdateOutputNode(nameof(Transform));
        }
    }
}