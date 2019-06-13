using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupGenericPOSSetting
{
	public sealed partial class ListPanel : UserControl
	{
		public event EventHandler<EventArgs<POS_Exception>> OnExceptionEdit;
		public event EventHandler OnPOSSettingEdit;
		public event EventHandler OnCustomizationAdd;
		public event EventHandler OnPOSSettingAdd;
		
		public IPTS PTS;

		public Dictionary<String, String> Localization;

		public ListPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   
								   {"SetupGenericPOSSetting_AddedException", "Added Exception"},
								   {"SetupGenericPOSSetting_AddNewSetting", "Generic POS Setting and Capture Data..."},
								   {"SetupGenericPOSSetting_TransactionSetting", "POS Transaction Setting"},
								   {"SetupGenericPOSSetting_AddCustomizastionException", "Add Customization Exception"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
            addedPOSExceptionLabel.Text = Localization["SetupGenericPOSSetting_AddedException"];

			BackgroundImage = Manager.BackgroundNoBorder;
		}

		public void Initialize()
		{
			addNewDoubleBufferPanel.Paint += AddNewPanelPaint;
			transactionSettingDoubleBufferPanel.Paint += ThresholdPanelPaint;

			addNewDoubleBufferPanel.MouseClick += AddNewDoubleBufferPanelMouseClick;
			transactionSettingDoubleBufferPanel.MouseClick += TransactionEditDoubleBufferPanelMouseClick;

		    addedPOSExceptionLabel.Visible =
		    containerPanel.Visible = false;
		}

		private void AddNewPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintHighLightInput(g, addNewDoubleBufferPanel);
			Manager.PaintEdit(g, addNewDoubleBufferPanel);

            Manager.PaintText(g, Localization["SetupGenericPOSSetting_AddNewSetting"]);
		}

		private void ThresholdPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, transactionSettingDoubleBufferPanel);
			Manager.PaintEdit(g, transactionSettingDoubleBufferPanel);

            Manager.PaintText(g, Localization["SetupGenericPOSSetting_TransactionSetting"]);
		}

		private readonly Queue<POSExceptionPanel> _recycleException = new Queue<POSExceptionPanel>();

		public void GenerateViewModel()
		{
		    return;
			ClearViewModel();
			addNewDoubleBufferPanel.Visible = 
			addNewLabel.Visible =
			transactionSettingDoubleBufferPanel.Visible = thresholdLabel.Visible =
			addedPOSExceptionLabel.Visible = true;
			containerPanel.Visible = false;

			List<POS_Exception> sortResult = null;
			if (PTS != null)
			{
				sortResult = new List<POS_Exception>(PTS.POS.Exceptions.Values);
			}
			
			if (sortResult == null)
			{
				addedPOSExceptionLabel.Visible = false;
				return;
			}

			sortResult.Sort((x, y) => (y.Id - x.Id));

			if (sortResult.Count == 0)
			{
				addedPOSExceptionLabel.Visible = false;
				return;
			}
			
			foreach (POS_Exception exception in sortResult)
			{
				POSExceptionPanel posExceptionPanel = GetPOSExceptionPanel();

				posExceptionPanel.POSException = exception;

				containerPanel.Controls.Add(posExceptionPanel);
			}

			POSExceptionPanel posExceptionTitlePanel = GetPOSExceptionPanel();
			posExceptionTitlePanel.IsTitle = true;
			posExceptionTitlePanel.Cursor = Cursors.Default;
			posExceptionTitlePanel.EditVisible = false;
			//deviceTitleControl.OnSortChange += DeviceControlOnSortChange;
			posExceptionTitlePanel.OnSelectAll += POSExceptionPanelOnSelectAll;
			posExceptionTitlePanel.OnSelectNone += POSExceptionPanelOnSelectNone;
			containerPanel.Controls.Add(posExceptionTitlePanel);
			containerPanel.Visible = true;

			containerPanel.AutoScroll = false;
			containerPanel.Focus();
			containerPanel.AutoScroll = true;
		}

		private POSExceptionPanel GetPOSExceptionPanel()
		{
			if (_recycleException.Count > 0)
			{
				return _recycleException.Dequeue();
			}

			var posExceptionPanel = new POSExceptionPanel
			{
				SelectedColor = Manager.DeleteTextColor,
				EditVisible = true,
				SelectionVisible = false,
			};

			posExceptionPanel.OnPOSExceptionEditClick += POSExceptionPanelOnPOSExceptionEditClick;
			posExceptionPanel.OnSelectChange += POSExceptionPanelOnSelectChange;

			return posExceptionPanel;
		}

		public Brush SelectedColor{
			set
			{
				foreach (POSExceptionPanel control in containerPanel.Controls)
					control.SelectedColor = value;
			}
		}

		public void ShowCheckBox()
		{
			addNewDoubleBufferPanel.Visible = addNewLabel.Visible =
				transactionSettingDoubleBufferPanel.Visible = thresholdLabel.Visible = 
				addedPOSExceptionLabel.Visible = false;

			foreach (POSExceptionPanel control in containerPanel.Controls)
			{
				control.SelectionVisible = true;
				control.EditVisible = false;
			}

			containerPanel.AutoScroll = false;
			containerPanel.Focus();
			containerPanel.AutoScroll = true;
		}

		public void RemoveSelectedException()
		{
			foreach (POSExceptionPanel control in containerPanel.Controls)
			{
				if (!control.Checked || control.POSException == null) continue;

				if (PTS == null) continue;

				PTS.POS.Exceptions.Remove(control.POSException.Id);
				foreach (var pos in PTS.POS.POSServer)
				{
					if (pos.Exception != control.POSException.Id) continue;

					//clera exception
					pos.Exception = 0;

					//clear exception report
					if (pos.ExceptionReports.ReadyState == ReadyState.Ready)
						pos.ExceptionReports.ReadyState = ReadyState.Modify;

					pos.ExceptionReports.Clear();

					if (pos.ReadyState == ReadyState.Ready)
						pos.ReadyState = ReadyState.Modify;
					
					//set default to another exception if manufacture is the same.
					if (PTS.POS.Exceptions.Count <= 0) continue;

					var keys = PTS.POS.Exceptions.Keys.ToList();
					keys.Sort();
					foreach (var key in keys)
					{
						if (PTS.POS.Exceptions[key].Manufacture == pos.Manufacture)
						{
							pos.Exception = key;
							break;
						}
					}
				}
			}
		}

		private void AddNewDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnPOSSettingAdd != null)
				OnPOSSettingAdd(this, e);
		}

		private void TransactionEditDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnPOSSettingEdit != null)
				OnPOSSettingEdit(this, e);
		}

		private void POSExceptionPanelOnPOSExceptionEditClick(Object sender, EventArgs e)
		{
			if (((POSExceptionPanel)sender).POSException != null)
			{
				if (OnExceptionEdit != null)
					OnExceptionEdit(this, new EventArgs<POS_Exception>(((POSExceptionPanel)sender).POSException));
			}
		}

		private void POSExceptionPanelOnSelectChange(Object sender, EventArgs e)
		{
			var panel = sender as POSExceptionPanel;
			if (panel == null) return;

			var selectAll = false;
			if (panel.Checked)
			{
				selectAll = true;
				foreach (POSExceptionPanel control in containerPanel.Controls)
				{
					if (control.IsTitle) continue;
					if (!control.Checked && control.Enabled)
					{
						selectAll = false;
						break;
					}
				}
			}

			var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as POSExceptionPanel;
			if (title != null && title.IsTitle && title.Checked != selectAll)
			{
				title.OnSelectAll -= POSExceptionPanelOnSelectAll;
				title.OnSelectNone -= POSExceptionPanelOnSelectNone;

				title.Checked = selectAll;

				title.OnSelectAll += POSExceptionPanelOnSelectAll;
				title.OnSelectNone += POSExceptionPanelOnSelectNone;
			}
		}

		private void ClearViewModel()
		{
			foreach (POSExceptionPanel posExceptionPanel in containerPanel.Controls)
			{
				posExceptionPanel.SelectionVisible = false;

				posExceptionPanel.Checked = false;
				posExceptionPanel.POSException = null;
				posExceptionPanel.Cursor = Cursors.Hand;
				posExceptionPanel.EditVisible = true;

				if (posExceptionPanel.IsTitle)
				{
					posExceptionPanel.OnSelectAll -= POSExceptionPanelOnSelectAll;
					posExceptionPanel.OnSelectNone -= POSExceptionPanelOnSelectNone;
					posExceptionPanel.IsTitle = false;
				}

				if (!_recycleException.Contains(posExceptionPanel))
				{
					_recycleException.Enqueue(posExceptionPanel);
				}
			}
			containerPanel.Controls.Clear();
		}

		private void POSExceptionPanelOnSelectAll(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (POSExceptionPanel control in containerPanel.Controls)
			{
				control.Checked = true;
			}
			containerPanel.AutoScroll = true;
		}

		private void POSExceptionPanelOnSelectNone(Object sender, EventArgs e)
		{
			containerPanel.AutoScroll = false;
			foreach (POSExceptionPanel control in containerPanel.Controls)
			{
				control.Checked = false;
			}
			containerPanel.AutoScroll = true;
		}
	} 
}