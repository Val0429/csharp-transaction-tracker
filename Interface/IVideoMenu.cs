using System;
using System.Drawing;
using Constant;

namespace Interface
{
    public interface IVideoMenu
    {
        event EventHandler<EventArgs<String>> OnButtonClick;

        IVideoWindow VideoWindow { get; set; }
        ToolMenuType MenuType { get; set; }
        Point PanelPoint { get; set; }
        System.Timers.Timer HideMenuTimer { get; }

        Boolean Visible { get; set; }
        Boolean IsPressButton { get; set; }

        void CheckPermission();
        void CheckStatus();
        void UpdateLocation();

        void GenerateInstantPlaybackToolMenu();
        void GeneratePlaybackToolMenu();
        void GenerateSmartSearchToolMenu();
        void GeneratePresetPointToolMenu();
        void GenerateLiveToolMenu();
        void GenerateInterrogationToolMenu();
    }
}
