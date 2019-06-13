using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Constant;
using PanelBase;

namespace App_POSTransactionServer
{
    public class LoginForm : App.LoginForm
    {
        public LoginForm()
        {
            DefaultPort = 7777;

            Localization.Add("LoginForm_PTS", "PTS");

            AppProperties = new AppClientProperties();
        }

        public override void ShowSplash()
        {
            //ExecutionFile = "PTS.exe";
            App = new POSTransactionServer
            {
                Credential = new ServerCredential
                {
                    Port = Port,
                    Domain = Host,
                    UserName = Account,
                    Password = Password,
                    SSLEnable = SSLEnable,
                },
                Language = AppProperties.DefaultLanguage
            };

            base.ShowSplash();

            IsLoading = false;
            Application.Idle -= LoginServer;
            Application.Idle += LoginServer;
        }

        private bool _isInitial;
        protected override void UpdateUI()
        {
            base.UpdateUI();

            var name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            switch (name)
            {
                case "TransactionTracker":
                    signInLabel.Text = @"TransactionTracker";
                    Icon = Properties.Resources.icon;

                    if (!_isInitial && File.Exists(Application.StartupPath + "\\images\\banner.png"))
                    {
                        Image banner = Image.FromFile(Application.StartupPath + "\\images\\banner.png");
                        Controls.Remove(titlePanel);
                        Controls.Remove(logoPictureBox);

                        var transactionTrackerLabel = new PictureBox();
                        transactionTrackerLabel.BackgroundImage = banner;
                        transactionTrackerLabel.BackgroundImageLayout = ImageLayout.Stretch;
                        transactionTrackerLabel.Size = new Size(banner.Width, 60);
                        transactionTrackerLabel.Dock = DockStyle.Top;
                        Controls.Add(transactionTrackerLabel);
                        signInLabel.Visible = false;
                        titlePanel.Visible = false;
                        
                    }

                    if (!_isInitial && File.Exists(Application.StartupPath + "\\images\\loginLogo.png"))
                    {
                        Image logo = Image.FromFile(Application.StartupPath + "\\images\\loginLogo.png");
                        var logoLabel = new PictureBox();
                        logoLabel.BackgroundImage = logo;
                        logoLabel.Size = new Size(logo.Width, logo.Height+20);
                        logoLabel.Dock = DockStyle.Bottom;
                        logoLabel.BackColor = Color.FromArgb(59, 152, 195);
                        Controls.Add(logoLabel);

                        logoLabel.BackgroundImageLayout = ImageLayout.Center;

                        var largeLoginPanel = Resources.GetResources(Properties.Resources.loginPanel, Properties.Resources.BGLoginPanelBG);
                        BackgroundImage = largeLoginPanel;
                        MinimumSize = MaximumSize = new Size(largeLoginPanel.Width, largeLoginPanel.Height + 53);

                        foreach (var control in Controls)
                        {
                            if(control is Label)
                            {
                                var item = control as Label;
                                item.Location = new Point(item.Location.X, item.Location.Y - 45);
                            }

                            if (control is ComboBox)
                            {
                                var item = control as ComboBox;
                                item.Location = new Point(item.Location.X, item.Location.Y - 45);
                            }

                            if (control is TextBox)
                            {
                                var item = control as TextBox;
                                item.Location = new Point(item.Location.X, item.Location.Y - 45);
                            }

                            if (control is Panel)
                            {
                                var item = control as Panel;
                                item.Location = new Point(item.Location.X, item.Location.Y - 45);
                            }

                            if (control is Button)
                            {
                                var item = control as Button;
                                item.Location = new Point(item.Location.X, item.Location.Y - 60);
                            }

                            if (control is CheckBox)
                            {
                                var item = control as CheckBox;
                                item.Location = new Point(item.Location.X, item.Location.Y - 45);
                            }
                        }

                    }
                    _isInitial = true;
                    break;

                default:
                    signInLabel.Text += @" " + Localization["LoginForm_PTS"];
                    Icon = Properties.Resources.icon2;
                    break;
            }

            sslCheckBox.Visible = false;
        }

        private void InitializeComponent()
        {
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.FlatAppearance.BorderSize = 0;
            this.loginButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.loginButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.loginButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // cancelButton
            // 
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            // 
            // sslCheckBox
            // 
            this.sslCheckBox.Visible = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(494, 418);
            this.Name = "LoginForm";
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
