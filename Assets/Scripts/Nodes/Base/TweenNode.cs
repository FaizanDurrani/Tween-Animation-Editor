using System.Collections;
using DG.Tweening;
using Graphs;
using Types;
using UnityEngine;
using XNode;

namespace Base
{
    public abstract class TweenNode : ExtendedNode
    {
        [Input] public TweenData Tween;
        [Input] public RectTransform Target;
        [Input] public AnimationCurve EaseCurve;
        [Input] public float AnimationDuration;
        [Input] public float Delay;
        [Input] public bool PingPong;

        protected override void OnInputUpdate(NodePort port)
        {
            if (port.fieldName == nameof(Tween))
            {
                ExecuteTween(Target ?? GetGraph<AnimationTemplateGraph>().Target);
            }
        }

        protected abstract void ExecuteTween(RectTransform t);
    }
}