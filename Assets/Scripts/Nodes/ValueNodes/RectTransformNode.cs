using UnityEngine;
using XNode;

namespace ValueNodes
{
    public class RectTransformNode : ValueNode<RectTransform>
    {
        protected override string GetNodeName() => "Rect Transform Node";

        [Output] public Vector2 SizeDelta;
        [Output] public Vector3 AnchoredPosition3D;
        [Output] public Vector2 AnchorMin, AnchorMax;
        [Output] public Vector2 Scale;

        public override object GetValue(NodePort port)
        {
            switch (port.fieldName)
            {
                default:
                    return base.GetValue(port);
                case nameof(SizeDelta):
                    return GetExposedValue().sizeDelta;
                case nameof(AnchoredPosition3D):
                    return GetExposedValue().anchoredPosition3D;
                case nameof(AnchorMin):
                    return GetExposedValue().anchorMin;
                case nameof(AnchorMax):
                    return GetExposedValue().anchorMax;
                case nameof(Scale):
                    return GetExposedValue().localScale;
            }
        }
    }
}