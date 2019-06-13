using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace App
{
	public partial class UnlockAppForm : Form
	{
		private const String CgiLogin = @"cgi-bin/login?action=login";
		
		public IApp App { get; set; }

		public event EventHandler OnCancel;
		public event EventHandler OnConfirm;
		public Dictionary<String, String> Localization;

		public UnlockAppForm()
		{
			Localization = new Dictionary<String, String>
				{
					{"Common_Cancel", "Cancel"},
					{"MessageBox_Error", "Error"},
					{"UnlockAppForm_Title", "Unlock Application"},
					{"UnlockAppForm_Unlock", "Unlock"},
					{"UnlockAppForm_EnterPassword", "Enter the \"%1\" password to unlock application"},
					{"UnlockAppForm_PasswordIncorrect", "Password is incorrect"},

					{"LoginForm_Account", "Account"},
					{"LoginForm_Password", "Password"},
					{"Menu_SignOut", "Sign Out"},

					{"LoginForm_SignOutFailed", "Sign out failure"},
					{"LoginForm_SignInFailedNoAccount", "Account can't be empty."},
					{"LoginForm_SignInFailedCantCreateHttpWebRequest", "Can not create HttpWebRequest object."},
					{"LoginForm_SignInFailedConnectFailure", "Can not connect to server. Please confirm host and port is correct."},
					{"LoginForm_SignOutTimeout", "Sign out timeout. Please check firewall setting."},
					{"LoginForm_SignOutTimeoutSSL", "Sign out timeout. Please check firewall setting and SSL port is correct."},
					{"LoginForm_SignOutFailedAuthFailure", "Sign out failure. Please confirm account and password is correct."},
					{"LoginForm_SignOutFailedPortOccupation", "Sign out failure. Please verify if port %1 is already used by another application."},
				};
			Localizations.Update(Localization);

			InitializeComponent();
			Text = Localization["UnlockAppForm_Title"];
			unlockButton.Text = Localization["UnlockAppForm_Unlock"];
			cancelButton.Text = Localization["Common_Cancel"];
			logoutButton.Text = Localization["Menu_SignOut"];
			accountLabel.Text = Localization["LoginForm_Account"];
			passwordLabel.Text = Localization["LoginForm_Password"];

			accountTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;

			unlockButton.BackgroundImage = Resources.GetResources(Properties.Resources.loginButton, Properties.Resources.IMGLoginButton);
			cancelButton.BackgroundImage = Resources.GetResources(Properties.Resources.cancelButotn, Properties.Resources.IMGCancelButotn);
			logoutButton.BackgroundImage = Resources.GetResources(Properties.Resources.cancelButotn, Properties.Resources.IMGCancelButotn);

			BackgroundImage = Resources.GetResources(Properties.Resources.controllerBG, Properties.Resources.BGControllerBG);
		}

		private void PasswordTextBoxKeyPress(Object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == Convert.ToChar(Keys.Enter))
			{
				CheckPassword();
			}
		}

		public new void Show()
		{
			base.Show();
			passwordTextBox.Focus();
		}
		
		private IUser _user;
		public IUser User
		{
			set
			{
				_user = value;
				unlockLabel.Text = Localization["UnlockAppForm_EnterPassword"].Replace("%1", _user.Credential.UserName);
			}
			get { return _user; }
		}

		private void UnlockButtonClick(Object sender, EventArgs e)
		{
			CheckPassword();
		}

		public void CheckPassword()
		{
			if(passwordTextBox.Text == _user.Credential.Password)
			{
				Hide();

				TopMost = false;

				passwordTextBox.Text = "";

				if (OnConfirm != null)
					OnConfirm(this, null);
			}
			else
			{
				TopMostMessageBox.Show(Localization["UnlockAppForm_PasswordIncorrect"], Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Error);
				passwordTextBox.Focus();
				passwordTextBox.SelectAll();
			}
		}

		private void CancelButtonClick(Object sender, EventArgs e)
		{
			TopMost = false;

			passwordTextBox.Text = "";

			if (OnCancel != null)
				OnCancel(this, null);

			Hide();
		}

		//==============================================================================================
		private void AccountTextBoxPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyData == Keys.Enter)
			{
				password2TextBox.Focus();
				e.IsInputKey = true;
			}

		}

		private void Password2TextBoxPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyData == Keys.Enter)
			{
				logoutButton.PerformClick();
				e.IsInputKey = true;
			}
		}

		private void LogoutButtonClick(object sender, EventArgs e)
		{
			logoutButton.Enabled = false;
			if (LogoutViaCGI())
			{
				//logoutButton.Enabled = true;
				App.RemoveProperties();
				App.OpenAnotherProcessAfterLogout = true;
				App.Logout();
				return;
			}

			logoutButton.Enabled = true;
		}

		protected virtual Boolean LogoutViaCGI()
		{
			HttpWebResponse response = null;

			try
			{
				var request = Xml.GetHttpRequest(CgiLogin, new ServerCredential
				{
					Port = App.Credential.Port ,
					Domain = App.Credential.Domain,
					UserName = accountTextBox.Text,
					Password = password2TextBox.Text,
					SSLEnable = App.Credential.SSLEnable,
				});

				if (request == null)
				{
					//URI Format Error
					MessageBox.Show(Localization["LoginForm_SignOutFailedCantCreateHttpWebRequest"], Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				response = (HttpWebResponse)request.GetResponse();

				if (response != null)
				{
					if (response.StatusCode == HttpStatusCode.OK)
					{
						response.Close();
						return true;
					}
					response.Close();
				}
			}
			catch (WebException exception)
			{
				if (response != null)
					response.Close();

				String message = String.Empty;
				if (exception.Status == WebExceptionStatus.ConnectFailure)
					message = Localization["LoginForm_SignInFailedConnectFailure"];
				else if (exception.Status == WebExceptionStatus.ProtocolError)
				{
					var httpWebResponse = ((HttpWebResponse)exception.Response);
					if (httpWebResponse != null)
					{
						switch (httpWebResponse.StatusCode)
						{
							case HttpStatusCode.Unauthorized:
								message = Localization["LoginForm_SignOutFailedAuthFailure"];
								break;

							case HttpStatusCode.NotFound:
								message = Localization["LoginForm_SignOutFailedPortOccupation"].Replace("%1", App.Credential.Port.ToString());
								break;
						}
					}
				}
				else if (exception.Status == WebExceptionStatus.Timeout)
				{
					if (App.Credential.SSLEnable)
						message = Localization["LoginForm_SignOutTimeoutSSL"];
					else
						message = Localization["LoginForm_SignOutTimeout"];
				}
				else //unknown reason
					message = Localization["LoginForm_SignOutFailed"];

				if (!String.IsNullOrEmpty(message))
				{
					MessageBox.Show(message, Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				return false;
			}

			MessageBox.Show(
				(accountTextBox.Text == ""
				? Localization["LoginForm_SignInFailedNoAccount"]
				: Localization["LoginForm_SignOutFailed"]), Localization["MessageBox_Error"], MessageBoxButtons.OK, MessageBoxIcon.Error);

			return false;
		}

	}
}
