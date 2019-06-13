using System;
using System.Windows.Forms;

namespace Interface
{
    public interface IMinimize
    {
        event EventHandler OnMinimizeChange;

        UInt16 MinimizeHeight{ get; }
        Boolean IsMinimize { get; }

        Button Icon { get; }

        void Minimize();
        void Maximize();
    }
}