using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Constant;
using Interface;

namespace SmartSearch.UserDefined
{
	public partial class UserDefinedResult : SearchResult
	{
		public UserDefinedResult()
        {
			Localization.Add("Event_SaveImage", "Save Image");
            Localizations.Update(Localization);
        }

		public override void SmartSearchResult(Object sender, EventArgs<String> e)
		{
			if (InvokeRequired)
			{
				Invoke(new SmartSearchResultDelegate(SmartSearchResult), sender, e);
				return;
			}

			XmlNodeList times = Xml.LoadXml(e.Value).GetElementsByTagName("Time");

			//Console.WriteLine(@"Find " + times.Count);
			_found += times.Count;

			snapshotFlowLayoutPanel.Visible = false;

			foreach (XmlElement time in times)
			{
				SearchResultPanel searchResultPanel = GetSearchResultPanel();

				searchResultPanel.Camera = _camera;
				searchResultPanel.Timecode = Convert.ToUInt64(time.InnerText);
				searchResultPanel.Type = time.GetAttribute("type");

				switch (time.GetAttribute("type"))
				{
					case "UserDefine" :
						searchResultPanel.Description = Localization["Event_SaveImage"];
						break;
					default :
						searchResultPanel.Description = time.GetAttribute("desc");
						break;
				}
				

				if (snapshotFlowLayoutPanel.Controls.Count < NumPerPage)
				{
					switch (_size)
					{
						case "QQVGA":
							searchResultPanel.SmallSize();
							break;

						case "HQVGA":
							searchResultPanel.MediumSize();
							break;

						case "QVGA":
							searchResultPanel.LargeSize();
							break;
					}

					snapshotFlowLayoutPanel.Controls.Add(searchResultPanel);
				}

				_sortedList.Add(searchResultPanel);
			}

			_sortedList.Sort((x, y) => (String.Compare(x.Timecode.ToString(), y.Timecode.ToString())));

			Boolean needSort = false;

			foreach (SearchResultPanel panel in snapshotFlowLayoutPanel.Controls)
			{
				if (snapshotFlowLayoutPanel.Controls.IndexOf(panel) != _sortedList.IndexOf(panel))
				{
					needSort = true;
					break;
				}
			}

			if (needSort)
			{
				foreach (SearchResultPanel panel in _sortedList)
				{
					if (!snapshotFlowLayoutPanel.Controls.Contains(panel))
					{
						switch (_size)
						{
							case "QQVGA":
								panel.SmallSize();
								break;

							case "HQVGA":
								panel.MediumSize();
								break;

							case "QVGA":
								panel.LargeSize();
								break;
						}
						snapshotFlowLayoutPanel.Controls.Add(panel);
					}

					panel.SendToBack();
				}

				while (snapshotFlowLayoutPanel.Controls.Count > NumPerPage)
				{
					snapshotFlowLayoutPanel.Controls.Remove(snapshotFlowLayoutPanel.Controls[snapshotFlowLayoutPanel.Controls.Count - 1]);
				}
			}
			snapshotFlowLayoutPanel.Visible = true;
		}

		protected override SearchResultPanel GetSearchResultPanel()
		{
			UserDefinedResultPanel searchResultPanel = null;
			if (RecycleSearchResultPanel.Count > 0)
			{
				foreach (UserDefinedResultPanel resultPanel in RecycleSearchResultPanel)
				{
					if (resultPanel.IsLoadingImage) continue;

					searchResultPanel = resultPanel;
					break;
				}

				if (searchResultPanel != null)
					RecycleSearchResultPanel.Remove(searchResultPanel);
			}

			if (searchResultPanel == null)
			{
				searchResultPanel = new UserDefinedResultPanel
				{
					SearchResult = this,
					Server = Server,
				};
				searchResultPanel.OnSelectionChange += SearchResultPanelOnSelectionChange;
			}

			return searchResultPanel;
		}
	}
}
