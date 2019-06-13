
using Constant;

namespace PTSReports.Top2Cashier
{
    public class Top2CashierIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportTop2CashierIcon", "Top 2 Cashier");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.top2Icon_activate, Properties.Resources.IMGTop2IconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.top2Icon, Properties.Resources.IMGTop2Icon);

            Button.Text = TitleName = Localization["Control_ReportTop2CashierIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"ReportTop2Cashier";
        }
    }
}
