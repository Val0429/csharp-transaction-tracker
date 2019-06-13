
using System.Xml;

namespace Interface
{
    public interface IBehaviorManager
    {
        ILayoutManager Layout { set; }
        XmlDocument ConfigNode { set; }
    }
}
