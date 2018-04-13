using UnityEngine;
using XNode;

namespace BasicNodes
{
    public class TransformNode : ValueNode<Transform>
    {
        protected override string NodeName => "Transform Node";
    }
}