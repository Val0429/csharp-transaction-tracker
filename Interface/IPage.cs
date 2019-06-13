using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace Interface
{
    public interface IPage
    {
        IApp App { get; set; }
        IServer Server { get; set; }
        String Name { get; set; }
        String TitleName { get; set; }
        ILayoutManager Layout { get;}
        IBehaviorManager Behavior { get;}
        String Version { get; }

        void LoadConfig();

        Boolean IsExists { get; }
        Boolean IsInitialize { get; }
        Boolean IsActivate { get; }
        Boolean IsCoolDown { get; }
        Boolean IsPlugins { get; }

        Button Icon { get; }
        Panel Content { get; }
        Panel Function { get; }
        XmlElement PageNode { get;  set; }

        List<IMouseHandler> MouseHandler { get; }
        List<IFocus> FocusList { get; }
        List<IKeyPress> KeyPressList { get; }

        void Initialize();
        void Activate();
        void Deactivate();
        void HidePanel();
        void ShowPanel();
        void CheckDragDataType(List<IDrop> container, Object dragObj);
    }
}
