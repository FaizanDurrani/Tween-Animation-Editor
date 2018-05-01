using System;
using DG.Tweening;
using UnityEngine;

namespace Types
{
    [Serializable]
    public struct TweenData
    {
        public Sequence Sequence { get; }
        
        public TweenData(Sequence sequence)
        {
            Sequence = sequence;
        }
    }
}