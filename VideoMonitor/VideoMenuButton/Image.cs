using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;

namespace VideoMonitor
{
	public partial class VideoMenu
	{
		private Button _snapshotButton;
		private Button _saveImageButton;

		private static readonly Image _saveImage = Resources.GetResources(Properties.Resources.save_image, Properties.Resources.IMGSave_image);
		private static readonly Image _snapshot = Resources.GetResources(Properties.Resources.snapshot, Properties.Resources.IMGSnapshot);

		private void SnapshotButtonMouseClick(Object sender, MouseEventArgs e)
		{
			if (VideoWindow != null && Server != null)
			{
				VideoWindow.Snapshot(Server.Configure.ImageWithTimestamp);
				Server.WriteOperationLog("Device %1 Take Snapshot".Replace("%1", VideoWindow.Camera.Id.ToString()));
			}
		}

		protected virtual void SaveImageButtonMouseClick(Object sender, MouseEventArgs e)
		{
			if (VideoWindow != null && Server != null)
			{
				VideoWindow.SaveImage(Server.Configure.ImageWithTimestamp);
				Server.WriteOperationLog("Device %1 Save Image".Replace("%1", VideoWindow.Camera.Id.ToString()));
			}


			//XmlDocument xmlDoc = new XmlDocument();
			//HttpWebRequest request = Xml.GetHttpRequest( String.Format( "cgi-bin/userdef?channel=channel{0}&event=event2&data=", VideoWindow.Device.Id), Server.Credential, 5);
			//request.Method = "POST";
			//request.ContentType = "text/xml; encoding='utf-8'";
			//Xml.PostXmlToHttp(request, xmlDoc);
		}

		protected void UpdateSnapshotButton()
		{
			if (_snapshotButton == null || VideoWindow.Camera == null) return;

			//if (VideoWindow.Viewer.Visible && (VideoWindow.Camera.Model.Type != "AudioBox"))
			if (VideoWindow.Viewer.Visible)
			{
				if ((VideoWindow.Camera.Model == null) || (VideoWindow.Camera.Model.Type != "AudioBox"))
				{
					SetButtonPosition(_snapshotButton);
					Controls.Add(_snapshotButton);
					_count++;
				}
				else
				{
					Controls.Remove(_snapshotButton);
				}
			}
			else
			{
				Controls.Remove(_snapshotButton);
			}
		}

		protected void UpdateSaveImageButton()
		{
			if (_saveImageButton == null || VideoWindow.Camera == null) return;

			//if (VideoWindow.Viewer.Visible && (VideoWindow.Camera.Model.Type != "AudioBox"))
			if (VideoWindow.Viewer.Visible)
			{
				if ((VideoWindow.Camera.Model == null) || (VideoWindow.Camera.Model.Type != "AudioBox"))
				{
					SetButtonPosition(_saveImageButton);
					Controls.Add(_saveImageButton);
					_count++;
				}
				else
				{
					Controls.Remove(_saveImageButton);
				}
			}
			else
			{
				Controls.Remove(_saveImageButton);
			}
		}
	}
}