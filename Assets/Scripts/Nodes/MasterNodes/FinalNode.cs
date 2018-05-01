using Base;
using Types;
using UnityEngine;

namespace MasterNodes
{
    public class FinalNode : ExtendedNode
    {
        protected override string GetNodeName()=> "Final Node";
        [Input(connectionType = ConnectionType.Override)] public TweenData TweenData;
        [HideInInspector] public ReorderableEventList OnComplete;
    }
}