using UnityEngine;
using XNode;

namespace ValueNodes
{
    public class TransformNode : ValueNode<Transform>
    {
        protected override string GetNodeName()=> "Transform Node";

        [Output] public Vector3 Position;
        [Output] public Vector3 LocalPosition;
        [Output] public Vector3 Scale;

        public override object GetValue(NodePort port)
        {
            switch (port.fieldName)
            {
                default:
                    return base.GetValue(port);
                case nameof(Position):
                    return GetExposedValue().position;
                case nameof(LocalPosition):
                    return GetExposedValue().localPosition;
                case nameof(Scale):
                    return GetExposedValue().localScale;
            }
        }
    }
}