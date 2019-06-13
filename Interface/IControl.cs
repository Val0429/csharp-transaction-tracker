using System;

namespace Interface
{
    public interface IControl
    {
        String TitleName { get; set; }
        String Name { get; set; }

        void Initialize();
        void Activate();
        void Deactivate();
    }
}
