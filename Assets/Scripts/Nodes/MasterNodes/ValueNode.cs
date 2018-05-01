using Base;
using XNode;

namespace ValueNodes
{
    public abstract class ValueNode : ExtendedNode
    {
        public string Name = "";
        public object NodeValue;
    }
    
    public abstract class ValueNode<T> : ValueNode
    {
        [Output] public T Value;

        public override object GetValue(NodePort port)
        {
            return NodeValue;
        }

        protected T GetExposedValue()
        {
            return (T) NodeValue;
        }
    }
}