using System.Collections;
using Base;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;

namespace TweenNodes
{
    public class FadeNode : TweenNode
    {
        [Input] public float From, To;

        protected override string GetNodeName() => "Fade Node";

        protected override void ExecuteTween(RectTransform t)
        {
            CanvasGroup cg;
            bool d = false;
            if (t.gameObject.GetComponent<CanvasGroup>())
            {
                cg = t.gameObject.GetComponent<CanvasGroup>();
            }
            else
            {
                d = true;
                cg = t.gameObject.AddComponent<CanvasGroup>();
            }

            cg.interactable = false;
            cg.alpha = From;
            cg.DOFade(To, AnimationDuration).SetEase(EaseCurve).onComplete(() =>
            {
                cg.interactable = true;
                if (d)
                    Destroy(cg);
            });
        }
    }
}