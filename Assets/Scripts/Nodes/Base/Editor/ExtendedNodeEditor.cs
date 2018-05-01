using XNodeEditor;

namespace Base.Editor
{
    [CustomNodeEditor(typeof(ExtendedNode))]
    public class ExtendedNodeEditor : NodeEditor
    {
        public override int GetWidth() => 300;
    }
}