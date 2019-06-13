using System;

namespace App
{
    public partial class AppClient
    {
        protected virtual void InitializeToolPanel()
        {
            ToolPanel = ApplicationForms.ToolPanel();

            SwitchPagePanel = ApplicationForms.SwitchPagePanel();

            PageFunctionPanel = ApplicationForms.FunctionPanel();

            MiniFunctionPanel = ApplicationForms.MiniFunctionPanel();

            MiniFunctionPanel.Click += MiniFunctionPanelClick;
            ToolPanel.Controls.Add(PageFunctionPanel);
            ToolPanel.Controls.Add(MiniFunctionPanel);
            ToolPanel.Controls.Add(SwitchPagePanel);

            MainPanel.Controls.Add(ToolPanel);
        }
        
        protected virtual void InitializeToolPanelUI2()
        {
            ToolPanel = ApplicationForms.ToolPanelUI2();

            //dock fill, put on top
            PageDockIconPanel = ApplicationForms.PageDockIconPanelUI2();

            //fixed height, define one xml, put on buttom
            PageFunctionPanel = ApplicationForms.FunctionPanelUI2();

            ToolPanel.Controls.Add(PageDockIconPanel);
            ToolPanel.Controls.Add(PageFunctionPanel);

            MainPanel.Controls.Add(ToolPanel);
        }

        protected void MiniFunctionPanelClick(Object sender, EventArgs e)
        {
            _isFunctionPanelExpand = !_isFunctionPanelExpand;

            if (_isFunctionPanelExpand)
            {
                MiniFunctionPanel.BackgroundImage = CollapseImage;
                PageFunctionPanel.Visible = true;
            }
            else
            {
                MiniFunctionPanel.BackgroundImage = ExpandImage;
                PageFunctionPanel.Visible = false;
            }
        }
    }
}
