
using System;
using App;
using Constant;
using Interface;

namespace PTSReports.ThresholdDeviation
{
    public class ThresholdDeviationIcon : SetupBase.Icon, IAppUse
    {
        public IApp App { get; set; }
        public override void Initialize()
        {
            Localization.Add("Control_ReportThresholdDeviationIcon", "Threshold Deviation");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.thresholdIcon_activate, Properties.Resources.IMGThredholdActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.thresholdIcon, Properties.Resources.IMGThredholdIcon);

            Button.Text = TitleName = Localization["Control_ReportThresholdDeviationIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"ThresholdDeviation";
        }
    }
}
