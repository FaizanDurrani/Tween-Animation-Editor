using Base;
using XNode;

namespace Graphs
{
    public abstract class ExtendedGraph : NodeGraph
    {
        public abstract void NodeRemoved(ExtendedNode node);
        public abstract void NodeInitialized(ExtendedNode node);
    }
}