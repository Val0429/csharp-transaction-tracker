using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using Constant;

namespace SmartSearch.UserDefined
{
	public class UserDefinedResultPanel : SearchResultPanel
	{
		protected new Image UserDefine = Resources.GetResources(Properties.Resources.userdefine, Properties.Resources.IMGUserdefine);

		public override String Type
		{
			set
			{
				_type = value;

				switch (_type)
				{
					case "ManualRecord":
						_image = ManualRecord;
						break;

					case "IVS":
						_image = IVS;
						break;

					case "RecordFailed":
						_image = RecordFailed;
						break;

					case "RecordRecovery":
						_image = RecordRecovery;
						break;

					case "NetworkLoss":
						_image = NetworkLoss;
						break;

					case "NetworkRecovery":
						_image = NetworkRecovery;
						break;

					case "VideoLoss":
						_image = VideoLoss;
						break;

					case "VideoRecovery":
						_image = VideoRecovery;
						break;

					case "UserDefine":
						_image = UserDefine;
						break;

					case "Motion":
						_image = Motion;
						break;

					case "DigitalInput":
						_image = DigitalInput;
						break;

					case "DigitalOutput":
						_image = DigitalOutput;
						break;

					default:
						_image = null;
						break;
				}
			}
		}
	}
}
