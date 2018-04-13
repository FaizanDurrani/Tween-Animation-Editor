using System;
using System.Linq;
using System.Xml.Xsl.Runtime;
using MasterNodes;
using UnityEditor;
using UnityEngine;
using XNode;

namespace TweenEditor
{
    [Serializable, CreateAssetMenu(fileName = "NewTweenTemplate", menuName = "Tweens/Template")]
    public class AnimationTemplateGraph : NodeGraph
    {
        public enum Error
        {
            NoStart,
            NoEnd,
            ManyStart,
            ManyEnd,
            None
        }

        public Error CurrentError
        {
            get
            {
                bool start = false, end = false, manyStart = false, manyEnd = false;
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i] is TemplateStart && !start)
                    {
                        start = true;
                    }
                    else if (nodes[i] is TemplateStart)
                    {
                        manyStart = true;
                    }

                    if (nodes[i] is TemplateEnd && !end)
                    {
                        end = true;
                    }
                    else if (nodes[i] is TemplateEnd)
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
        }

        private TemplateStart _start;

        public TemplateStart Start
        {
            get
            {
                if (CurrentError != Error.None) return null;

                if (Application.isPlaying && _start != null)
                    return _start;

                return _start = nodes.Find(x => x is TemplateStart) as TemplateStart;
            }
        }

        private TemplateEnd _end;

        public TemplateEnd End
        {
            get
            {
                if (CurrentError != Error.None) return null;

                if (Application.isPlaying && _end != null)
                    return _end;

                return _end = nodes.Find(x => x is TemplateEnd) as TemplateEnd;
            }
        }   
    }
}