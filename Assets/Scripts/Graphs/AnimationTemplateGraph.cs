using System;
using Base;
using DG.Tweening;
using MasterNodes;
using UnityEngine;
using XNode;

namespace Graphs
{
    [Serializable, CreateAssetMenu(fileName = "NewTweenTemplate", menuName = "Tweens/Template")]
    public class AnimationTemplateGraph : ExtendedGraph
    {
        public enum Error
        {
            NoStart,
            NoEnd,
            ManyStart,
            ManyEnd,
            None
        }

        private Sequence _sequence;

        public Sequence Sequence
        {
            get
            {
                if (_sequence == null) _sequence = DOTween.Sequence();
                return _sequence;
            }
        }

        public Error CurrentError { private set; get; }
        public StartNode StartNode { private set; get; }
        public FinalNode FinalNode { private set; get; }
        public RectTransform Target { private set; get; }
        public MonoBehaviour Executor { private set; get; }

        public event Action<RectTransform> TweenExecuted;

        private Error GetError()
        {
            bool start = false, end = false, manyStart = false, manyEnd = false;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] is StartNode && !start)
                {
                    start = true;
                }
                else if (nodes[i] is StartNode)
                {
                    manyStart = true;
                }

                if (nodes[i] is FinalNode && !end)
                {
                    end = true;
                }
                else if (nodes[i] is FinalNode)
                {
                    manyEnd = true;
                }
            }

            if (manyStart) return Error.ManyStart;
            if (manyEnd) return Error.ManyEnd;
            if (!start) return Error.NoStart;
            if (!end) return Error.NoEnd;
            return Error.None;
        }

        public override void NodeRemoved(ExtendedNode node)
        {
            var startNode = node as StartNode;
            var endNode = node as FinalNode;
            if (startNode != null)
            {
                StartNode = null;
            }
            else if (endNode != null)
            {
                FinalNode = null;
            }

            CurrentError = GetError();
        }

        public override void NodeInitialized(ExtendedNode node)
        {
            var startNode = node as StartNode;
            var endNode = node as FinalNode;
            if (startNode != null)
            {
                StartNode = startNode;
            }
            else if (endNode != null)
            {
                FinalNode = endNode;
            }

            CurrentError = GetError();
        }

        public void ExecuteTween(RectTransform t, MonoBehaviour c)
        {
            Executor = c;
            Target = t;
            TweenExecuted?.Invoke(t);   
        }
    }
}