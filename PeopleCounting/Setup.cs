using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using App;
using Constant;
using Interface;
using PanelBase;
using SetupBase;

namespace PeopleCounting
{
	public sealed partial class Setup : UserControl, IControl, IServerUse, IBlockPanelUse
	{
		public event EventHandler<EventArgs<String>> OnSelectionChange;
		private const String CgiVerifyIVS= @"cgi-bin/ivs?channel=channel%1";

		public IBlockPanel BlockPanel { get; set; }

		public Dictionary<String, String> Localization;

		public IVAS VAS;
		private IServer _server;
		public IServer Server { 
			get { return _server; }
			set
			{
				_server = value;
				if (_server is IVAS)
					VAS = _server as IVAS;
			}
		}

		private ListPanel _listPanel;
		private PeopleCountingPanel _peopleCountingPanel;

		private Control _focusControl;
		private  BackgroundWorker _getSnapshotBackgroundWorker;
		private  BackgroundWorker _listenIVSBackgroundWorker;
		public Setup()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"Control_PeopleCounting", "People Counting"},

								   {"MessageBox_Information", "Information"},
								   
								   {"PeopleCounting_MaximumLicense", "Reached maximum license limit \"%1\""},
								   {"PeopleCounting_AddNVRFirst", "Add NVR first."},
								   {"PeopleCounting_NoAvailableDevice", "No Available Device."},
								   {"PeopleCounting_CloneDevice", "Clone Device"},
								   {"PeopleCounting_DeleteDevice", "Delete Device"},
							   };
			Localizations.Update(Localization);

			Name = "PeopleCounting";
			TitleName = Localization["Control_PeopleCounting"];

			InitializeComponent();

			Dock = DockStyle.Fill;

			_getSnapshotBackgroundWorker = new BackgroundWorker {WorkerSupportsCancellation = true};
			_getSnapshotBackgroundWorker.DoWork += GetSnapshot;
			_getSnapshotBackgroundWorker.RunWorkerCompleted += GetSnapshotCompleted;

			_listenIVSBackgroundWorker = new BackgroundWorker {WorkerSupportsCancellation = true};
			_listenIVSBackgroundWorker.DoWork += ListenIVS;
		}

		public String TitleName { get; set; }

		public void Initialize()
		{
			if (Parent is IControlPanel)
				BlockPanel.SyncDisplayControlList.Add((IControlPanel)Parent);

			_listPanel = new ListPanel
			{
				VAS = VAS,
			};
			_listPanel.Initialize();

			_peopleCountingPanel = new PeopleCountingPanel();

			_peopleCountingPanel.VAS = VAS;
			_listPanel.OnDeviceEdit += ListPanelOnDeviceEdit;
			_listPanel.OnDeviceAdd += ListPanelOnDeviceAdd;
			_peopleCountingPanel.OnVerify += PeopleCountingPanelOnVerify;
			_peopleCountingPanel.OnVerifyStop += PeopleCountingPanelOnVerifyStop;

			contentPanel.Controls.Contains(_listPanel);
		}

		private void ListPanelOnDeviceEdit(Object sender, EventArgs<IDevice> e)
		{
			EditDevice(e.Value);
		}
		
		private void ListPanelOnDeviceAdd(Object sender, EventArgs e)
		{
			if (VAS == null || VAS.NVR.NVRs.Count == 0)
			{
				TopMostMessageBox.Show(Localization["PeopleCounting_AddNVRFirst"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			Int32 count = 0;
			foreach (KeyValuePair<UInt16, INVR> obj in VAS.NVR.NVRs)
			{
				if (obj.Value.Device != null)
					count += obj.Value.Device.Devices.Count;
			}
			if (count == 0)
			{
				TopMostMessageBox.Show(Localization["PeopleCounting_NoAvailableDevice"], Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (VAS.Device.Devices.Count >= VAS.License.Amount)
			{
				TopMostMessageBox.Show(Localization["PeopleCounting_MaximumLicense"].Replace("%1", VAS.License.Amount.ToString()),
					Localization["MessageBox_Information"], MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			_listPanel.AddDevice();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					TitleName + " (" + Server.Device.Devices.Count + "/" + Server.License.Amount + ")", "", "Delete,Clone")));
		}

		private Boolean _startVerify;
		private Boolean _listenIVS;

		private void PeopleCountingPanelOnVerify(Object sender, EventArgs e)
		{
			_startVerify = true;
			if (_focusCamera == null) return;
			foreach (PeopleCountingRectangle rectangle in _focusCamera.Rectangles)
			{
				rectangle.PeopleCountingIn = 0;
				rectangle.PeopleCountingOut = 0;
			}

			_listenIVS = true;
			if (!_listenIVSBackgroundWorker.IsBusy)
				_listenIVSBackgroundWorker.RunWorkerAsync();

			if (!_getSnapshotBackgroundWorker.IsBusy)
				_getSnapshotBackgroundWorker.RunWorkerAsync();
		}

		private void PeopleCountingPanelOnVerifyStop(Object sender, EventArgs e)
		{
			_startVerify = _listenIVS = false;
			foreach (PeopleCountingRectangle rectangle in _focusCamera.Rectangles)
			{
				rectangle.PeopleCountingIn = 0;
				rectangle.PeopleCountingOut = 0;
			}

			//_listenIVSBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
			//_listenIVSBackgroundWorker.DoWork += ListenIVS;

			_listenIVSBackgroundWorker.CancelAsync();
			_listenIVSBackgroundWorker.Dispose();
			_listenIVSBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
			_listenIVSBackgroundWorker.DoWork += ListenIVS;

			//_getSnapshotBackgroundWorker.CancelAsync();
			//_getSnapshotBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
			//_getSnapshotBackgroundWorker.DoWork += GetSnapshot;
			//_getSnapshotBackgroundWorker.RunWorkerCompleted += GetSnapshotCompleted;
		}

		private Image _snapshot;
		private void GetSnapshot(Object sender, DoWorkEventArgs e)
		{
			if (!_startVerify) return;
			_snapshot = (_peopleCountingPanel.Camera != null) ? _peopleCountingPanel.Camera.GetSnapshot() : null;
		}

		private void ListenIVS(Object sender, DoWorkEventArgs e)
		{
			if (_focusCamera == null) return;
			if(!_listenIVS) return;

			var request = Xml.GetHttpRequest(CgiVerifyIVS.Replace("%1", _focusCamera.Id.ToString()), Server.Credential);
			request.Method = "GET";

			byte[] buffer = new byte[2000];
			Int32 read;

			ICamera camera = _peopleCountingPanel.Camera;
			if (camera == null) return;

			WebResponse response;
			Stream stream;
			try
			{
				response = request.GetResponse();
				stream = response.GetResponseStream();
			}
			catch (Exception exception)
			{
				return;
			}
			if (stream == null) return;

			String text;
			while (_listenIVS && stream.CanRead && (read = stream.Read(buffer, 0, buffer.Length)) != 0)
			{
				var reader = new StreamReader(new MemoryStream(buffer, 0, read));
				text = reader.ReadToEnd();
				if (!text.StartsWith("<XML>")) continue;

				try
				{
					var end = text.IndexOf("</XML>");
					text = text.Substring(0, end + "</XML>".Length);
					XmlDocument xmlDoc = Xml.LoadXml(text);
					XmlElement rectangleNode = Xml.GetFirstElementByTagName(xmlDoc, "Rectangle");
					if (rectangleNode != null)
					{
						Int32 x = Convert.ToInt32(rectangleNode.GetAttribute("x"));
						Int32 y = Convert.ToInt32(rectangleNode.GetAttribute("y"));
						Int32 width = Convert.ToInt32(rectangleNode.GetAttribute("width"));
						Int32 height = Convert.ToInt32(rectangleNode.GetAttribute("height"));

						foreach (PeopleCountingRectangle rectangle in _focusCamera.Rectangles)
						{
							if (rectangle.Rectangle.Width == width && rectangle.Rectangle.Height == height &&
								rectangle.Rectangle.X == x && rectangle.Rectangle.Y == y)
							{

								XmlElement resultNode = Xml.GetFirstElementByTagName(xmlDoc, "Result");
								if (resultNode != null)
								{
									rectangle.PeopleCountingIn += Convert.ToUInt16(Xml.GetFirstElementValueByTagName(resultNode, "In"));
									rectangle.PeopleCountingOut += Convert.ToUInt16(Xml.GetFirstElementValueByTagName(resultNode, "Out"));

									_peopleCountingPanel.UpdatePeopleCountingNumber();
								}
								break;
							}
						}
					}
				}
				catch (Exception)
				{
					stream.Close();
					response.Close();
					_listenIVS = false;
					_listenIVSBackgroundWorker.CancelAsync();
					_listenIVSBackgroundWorker.Dispose();
					_listenIVSBackgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
					_listenIVSBackgroundWorker.DoWork += ListenIVS;
					_listenIVSBackgroundWorker.RunWorkerAsync();
					return;
				}
			}

			stream.Close();
			response.Close();
		}

		private void GetSnapshotCompleted(Object sender, RunWorkerCompletedEventArgs e)
		{
			if (_snapshot != null)
				_peopleCountingPanel.UpdateSnapshot(_snapshot);

			if (_startVerify && !_getSnapshotBackgroundWorker.IsBusy)
				_getSnapshotBackgroundWorker.RunWorkerAsync();
		}

		private ICamera _focusCamera;
		private void EditDevice(IDevice device)
		{
			if (!(device is ICamera)) return;
			ICamera camera = device as ICamera;
			_focusControl = _peopleCountingPanel;
			_focusCamera = camera;
			
			ApplicationForms.ShowLoadingIcon(Server.Form);

			foreach (KeyValuePair<UInt16, IDevice> obj in camera.Server.Device.Devices)
			{
				if (!(obj.Value is ICamera)) continue;
				if (camera.Profile == ((ICamera)obj.Value).Profile)
				{
					_peopleCountingPanel.Camera = ((ICamera)obj.Value);
					_peopleCountingPanel.Rectangles = camera.Rectangles;
					break;
				}
			}

			//if (!_listenIVSBackgroundWorker.IsBusy)
			//    _listenIVSBackgroundWorker.RunWorkerAsync();

			Manager.ReplaceControl(_listPanel, _peopleCountingPanel, contentPanel, ManagerMoveToEditComplete);
		}

		private void ManagerMoveToEditComplete()
		{
			ApplicationForms.HideLoadingIcon();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
					 _peopleCountingPanel.Camera.ToString(), "Back", "")));
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
			_startVerify = false;
			_listenIVS = false;
			_peopleCountingPanel.Deactivate();
		}

		private void ShowDeviceList()
		{
			_startVerify = false;
			_listenIVS = false;
			_listPanel.Enabled = true;
			_peopleCountingPanel.Deactivate();
			_focusControl = _listPanel;
			if (!contentPanel.Controls.Contains(_listPanel))
			{
				contentPanel.Controls.Clear();
				contentPanel.Controls.Add(_listPanel);
			}

			_listPanel.GenerateViewModel();

			if (OnSelectionChange != null)
			{
				String buttons = "";

				if (Server.Device.Devices.Count > 0)
					buttons = "Delete,Clone";

				if (OnSelectionChange != null)
					OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
						 TitleName + " (" + Server.Device.Devices.Count + "/" + Server.License.Amount + ")", "", buttons)));
			}
		}

		public void ShowContent(Object sender, EventArgs<String> e)
		{
			BlockPanel.ShowThisControlPanel(this);

			ShowDeviceList();
		}

		private void DeleteDevice()
		{
			_listPanel.SelectedColor = Manager.DeleteTextColor;
			_listPanel.ShowCheckBox();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
						 Localization["PeopleCounting_DeleteDevice"], "Back", "Confirm")));
		}

		private void CloneDevice()
		{
			_listPanel.SelectedColor = Manager.SelectedTextColor;
			_listPanel.ShowCheckBox();

			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName,
						 Localization["PeopleCounting_CloneDevice"], "Back", "Duplicate")));
		}

		public void SelectionChange(Object sender, EventArgs<String> e)
		{
			String item;
			if (!Manager.ParseSelectionChange(e.Value, TitleName, out item))
				return;

			switch (item)
			{
				case "Confirm":
					_listPanel.RemoveSelectedDevices();

					ShowDeviceList();
					break;

				case "Duplicate":
					_listPanel.CloneSelectedDevices();

					ShowDeviceList();
					break;

				case "Delete":
					DeleteDevice();
					break;

				case "Clone":
					CloneDevice();
					break;

				default:
					if (item == TitleName || item == "Back")
					{
						Manager.ReplaceControl(_focusControl, _listPanel, contentPanel, ShowDeviceList);
					}
					break;
			}
		}
	}
}
