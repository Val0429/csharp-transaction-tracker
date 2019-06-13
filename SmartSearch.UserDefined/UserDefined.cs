using Constant;

namespace SmartSearch.UserDefined
{
	public partial class UserDefined : SmartSearch
	{
		public UserDefined()
		{
			Localization.Add("Event_SaveImage", "Save Image");
			Localizations.Update(Localization);

			InitializeComponent();
			userDefineCheckBox.Text = Localization["Event_SaveImage"];
			userDefineCheckBox.Enabled = true;
		}
	}
}
