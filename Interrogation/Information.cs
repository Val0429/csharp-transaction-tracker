using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;

namespace Interrogation
{
    public partial class Information : UserControl, IControl, IAppUse, IServerUse
    {
        public event EventHandler<EventArgs<String, DateTime, String>> OnStartRecordClicked;
        public event EventHandler<EventArgs<String, DateTime, String>> OnStopRecordClicked;

        public IApp App { get; set; }
        public IServer Server { get; set; }

        public String TitleName { get; set; }

        public Dictionary<String, String> Localization;

        private Int32 _startTime = 0;

        public Information()
        {
            Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},

                                   {"Interrogation_Information", "Information"},
								   {"Interrogation_Name", "Name"},
                                   {"Interrogation_Date", "Date"},
                                   {"Interrogation_NoOfRecord", "No. of Record"},
                                   {"Interrogation_RecordingTime", "Recording Time"},
                                   {"Interrogation_StartRecord", "Start Record"},
                                   {"Interrogation_StopRecord", "Stop Record"},

								   {"Interrogation_NameCantEmpty", "Name can't be empty."},
								   {"Interrogation_NoOfRecordCantEmpty", "No. of Record can't be empty."},
                                };
            Localizations.Update(Localization);

            InitializeComponent();
            Dock = DockStyle.Fill;
            DoubleBuffered = true;
        }

        public virtual void Initialize()
        {
            informationLabel.Text = Localization["Interrogation_Information"];
            nameLabel.Text = Localization["Interrogation_Name"];
            dateLabel.Text = Localization["Interrogation_Date"];
            numberLabel.Text = Localization["Interrogation_NoOfRecord"];

        	timeLabel.Text = Localization["Interrogation_RecordingTime"];
            startButton.Text = Localization["Interrogation_StartRecord"];
            stopButton.Text = Localization["Interrogation_StopRecord"];
        }

        public virtual void Activate()
        {
            
        }

        public virtual void Deactivate()
        {
            if (recordTimer.Enabled)
				StopButtonClick(this, null);
        }

		public void ContentChange(Object sender, EventArgs<Object> e)
		{
			if (e.Value != null)
			{
				nameTextBox.Enabled = true;
				datePicker.Enabled = true;
				noofRecordTextBox.Enabled = true;
				startButton.Enabled = true;
			}
			else
			{
			    nameTextBox.Text = "";
			    noofRecordTextBox.Text = "";
			    timeLabel.Text = "";
				nameTextBox.Enabled = false;
				datePicker.Enabled = false;
				noofRecordTextBox.Enabled = false;
				startButton.Enabled = false;
			}
		}

    	private void StartButtonClick(object sender, EventArgs e)
        {
            if (nameTextBox.Text == "")
            {
				TopMostMessageBox.Show(Localization["Interrogation_NameCantEmpty"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
				nameTextBox.Focus();
                return;
            }

            if (noofRecordTextBox.Text == "")
            {
				TopMostMessageBox.Show(Localization["Interrogation_NoOfRecordCantEmpty"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
				noofRecordTextBox.Focus();
                return;
            }

            _startTime = 0;
            DisplayTimeLabel();

            recordTimer.Enabled = false;
            recordTimer.Enabled = true;

            if ( OnStartRecordClicked != null)
                OnStartRecordClicked(this, new EventArgs<string, DateTime, string>(nameTextBox.Text, datePicker.Value, noofRecordTextBox.Text));


            App.HeaderPanel.Enabled = false;
            App.PageDockIconPanel.Enabled = false;
            foreach (IBlockPanel panel in App.PageActivated.Layout.BlockPanels)
            {
                foreach (IControlPanel controlPanel in panel.ControlPanels)
                {
                    if (controlPanel.Control.Name == "Information")
                    {
                        nameTextBox.Enabled = false;
                        datePicker.Enabled = false;
                        noofRecordTextBox.Enabled = false;
                        startButton.Enabled = false;
                        stopButton.Enabled = true;
                    }
                    else
                    {
                        ((Control) controlPanel.Control).Enabled = false;
                    }
                }
           }

           App.PageFunctionPanel.Enabled = true;
        }

        private void StopButtonClick(object sender, EventArgs e)
        {
            recordTimer.Enabled = false;

            if (OnStopRecordClicked != null)
                OnStopRecordClicked(this, new EventArgs<string, DateTime, string>(nameTextBox.Text, datePicker.Value, noofRecordTextBox.Text));

            App.HeaderPanel.Enabled = true;
            App.PageDockIconPanel.Enabled = true;
            foreach (IBlockPanel panel in App.PageActivated.Layout.BlockPanels)
            {
                foreach (IControlPanel controlPanel in panel.ControlPanels)
                {
                    if (controlPanel.Control.Name == "Information")
                    {
                        nameTextBox.Enabled = true;
                        datePicker.Enabled = true;
                        noofRecordTextBox.Enabled = true;
                        startButton.Enabled = true;
                        stopButton.Enabled = false;

                        timeLabel.Text = "";
                    }
                    else
                    {
                        ((Control)controlPanel.Control).Enabled = true;
                    }
                }
            }

            App.PageFunctionPanel.Enabled = true;
        }

        private void RecordTimerTick(object sender, EventArgs e)
        {
            _startTime++;
            DisplayTimeLabel();
        }

        private void DisplayTimeLabel()
        {
            var h = _startTime / 3600;
            var s = _startTime % 3600;

            var m = s / 60;
            s = s % 60;

			timeLabel.Text = Localization["Interrogation_RecordingTime"] + " " + String.Format("{0:D2} : {1:D2} : {2:D2}", h, m, s);
        }
    }
}
