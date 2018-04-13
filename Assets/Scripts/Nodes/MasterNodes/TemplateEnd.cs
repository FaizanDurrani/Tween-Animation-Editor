using Base;
using TweenEditor;
using DG.Tweening;

namespace MasterNodes
{
    public class TemplateEnd : ExtendedNode
    {
        protected override string NodeName => "Final Node";
        [Output] public Sequence Tween;
    }
}