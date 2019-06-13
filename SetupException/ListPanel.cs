using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Constant;
using Interface;
using PanelBase;
using Manager = SetupBase.Manager;

namespace SetupException
{
	public sealed partial class ListPanel : UserControl
	{
		public event EventHandler<EventArgs<POS_Exception>> OnExceptionEdit;
		public event EventHandler OnThresholdEdit;
		public event EventHandler OnExceptionColorEdit;
		public event EventHandler OnExceptionAdd;
		
		public IPTS PTS;

		public Dictionary<String, String> Localization;

		public ListPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},
								   
								   {"SetupException_AddedException", "Added Exception"},
								   {"SetupException_AddNewException", "Add new Exception..."},
                                   {"SetupException_AddNewExceptionByFile", "Add new Exception by file"},
								   {"POS_Threshold", "Threshold"},
								   {"POS_ExceptionColor", "Exception Color"},

                                   {"POS_ExceptionLoadFail", "Invalid format - %1. Please check the file."},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			addedPOSExceptionLabel.Text = Localization["SetupException_AddedException"];

			BackgroundImage = Manager.BackgroundNoBorder;
		}

		public void Initialize()
		{
			addNewDoubleBufferPanel.Paint += AddNewPanelPaint;
            addExceptionByFileDoubleBufferPanel.Paint += AddExceptionByFileDoubleBufferPanelPaint;
			thresholdDoubleBufferPanel.Paint += ThresholdPanelPaint;
			exceptionColorDoubleBufferPanel.Paint += ExceptionColorPanelPaint;

			addNewDoubleBufferPanel.MouseClick += AddNewDoubleBufferPanelMouseClick;
            addExceptionByFileDoubleBufferPanel.MouseClick += AddExceptionByFileDoubleBufferPanelMouseClick;
			thresholdDoubleBufferPanel.MouseClick += ThresholdDoubleBufferPanelMouseClick;
			exceptionColorDoubleBufferPanel.MouseClick += ExceptionColorDoubleBufferPanelMouseClick;
		}

		private void AddNewPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintHighLightInput(g, addNewDoubleBufferPanel);
			Manager.PaintEdit(g, addNewDoubleBufferPanel);

			Manager.PaintText(g, Localization["SetupException_AddNewException"]);
		}

        private void AddExceptionByFileDoubleBufferPanelPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintHighLightInput(g, addExceptionByFileDoubleBufferPanel);
            //Manager.PaintEdit(g, addExceptionByFileDoubleBufferPanel);

            Manager.PaintText(g, Localization["SetupException_AddNewExceptionByFile"]);
        }

		private void ThresholdPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, thresholdDoubleBufferPanel);
			Manager.PaintEdit(g, thresholdDoubleBufferPanel);

			Manager.PaintText(g, Localization["POS_Threshold"]);
		}

		private void ExceptionColorPanelPaint(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.PaintSingleInput(g, exceptionColorDoubleBufferPanel);
			Manager.PaintEdit(g, exceptionColorDoubleBufferPanel);

			Manager.PaintText(g, Localization["POS_ExceptionColor"]);
		}

		private readonly Queue<POSExceptionPanel> _recycleException = new Queue<POSExceptionPanel>();

		public void GenerateViewModel()
		{
			ClearViewModel();
			addNewDoubleBufferPanel.Visible = 
			addNewLabel.Visible =
            addExceptionByFileDoubleBufferPanel.Visible =
            addExceptionByFileLabel.Visible =
			thresholdDoubleBufferPanel.Visible = thresholdLabel.Visible =
			exceptionColorDoubleBufferPanel.Visible =
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
                if(exception.IsCapture) continue;
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
            addExceptionByFileDoubleBufferPanel.Visible = addExceptionByFileLabel.Visible =
			thresholdDoubleBufferPanel.Visible = thresholdLabel.Visible = 
			exceptionColorDoubleBufferPanel.Visible = 
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

                    if(control.POSException.Manufacture == "Generic") continue;

					var keys = PTS.POS.Exceptions.Keys.ToList();
					keys.Sort();
					foreach (var key in keys)
					{
                        if (PTS.POS.Exceptions[key].IsCapture) continue;
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
			if (OnExceptionAdd != null)
				OnExceptionAdd(this, e);
		}

        private void AddExceptionByFileDoubleBufferPanelMouseClick(object sender, MouseEventArgs e)
        {
            DialogResult resault = openFileDialog.ShowDialog();
            if (resault == DialogResult.OK)
            {
                String file = openFileDialog.FileName;
                String text = String.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                MemoryStream ms = null;
                try
                {
                    text = File.ReadAllText(file);
                    text = text.Replace("\r", "\\r").Replace("\n", "\\n");
                    
                    // Encode the XML string in a UTF-8 byte array
                    //byte[] encodedString = Encoding.UTF8.GetBytes(text);

                    //// Put the byte array into a stream and rewind it to the beginning
                    //ms = new MemoryStream(encodedString);
  
                    //ms.Flush();
                    //ms.Position = 0;

                    //using (XmlTextReader xmlTextReader = new XmlTextReader(file) { WhitespaceHandling = WhitespaceHandling.All })
                    //{
                    //    xmlTextReader.MoveToContent();
                    //    xmlDoc.Load(xmlTextReader);
                    //}
                }
                catch (IOException)
                {
                }

                try
                {
                    xmlDoc.LoadXml(text);
                    //if (ms != null)
                    //{
                    //    xmlDoc.Load(ms);
                    //    ms.Close();
                    //    ms.Dispose();
                        var node = xmlDoc.GetElementsByTagName("ExceptionConfiguration");
                        if(node.Count > 0)
                        {
                            var newException = PTS.POS.ParserXmlToException(node[0] as XmlElement);
                            newException.Id = PTS.POS.GetNewExceptionId();
                            newException.TransactionType = 1;
                            if (!PTS.POS.Exceptions.ContainsKey(newException.Id))
                            {
                                PTS.POS.Exceptions.Add(newException.Id, newException);
                            }

                            if (OnExceptionEdit != null)
                                OnExceptionEdit(this, new EventArgs<POS_Exception>(newException));
                        }
                        else
                        {
                            TopMostMessageBox.Show(Localization["POS_ExceptionLoadFail"].Replace("%1", "1"),
                                Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                }
                catch (Exception exception)
                {
                    TopMostMessageBox.Show(Localization["POS_ExceptionLoadFail"].Replace("%1", exception.Message),
                                Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

		private void ThresholdDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnThresholdEdit != null)
				OnThresholdEdit(this, e);
		}

		private void ExceptionColorDoubleBufferPanelMouseClick(Object sender, MouseEventArgs e)
		{
			if (OnExceptionColorEdit != null)
				OnExceptionColorEdit(this, e);
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