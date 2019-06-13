
using Constant;

namespace PTSReports.Cashier
{
    public class CashierIcon : SetupBase.Icon
    {
        public override void Initialize()
        {
            Localization.Add("Control_ReportCashierIcon", "Cashier");
            base.Initialize();

            ActivateIcon = Resources.GetResources(Properties.Resources.cashierIcon_activate, Properties.Resources.IMGCashierIconActivate);
            InactivateIcon = Resources.GetResources(Properties.Resources.cashierIcon, Properties.Resources.IMGCashierIcon);

            Button.Text = TitleName = Localization["Control_ReportCashierIcon"];
            Button.Image = InactivateIcon;
            Button.Name = @"ReportCashier";
        }
    }
}
