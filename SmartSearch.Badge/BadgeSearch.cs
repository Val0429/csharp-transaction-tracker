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

namespace SmartSearch.Badge
{
	public partial class BadgeSearch : SmartSearch
	{
		public override event EventHandler<EventArgs<String>> OnSmartSearchResult;

		public BadgeSearch()
		{
			InitializeComponent();

			periodTimeCheckBox.Checked = false;
			userDefineCheckBox.Checked = true;

			staffComboBox.Items.Add(new ComboxItem("", ""));
			staffComboBox.Items.Add(new ComboxItem("Jacob AppelSmith", "802345672312344"));
			staffComboBox.Items.Add(new ComboxItem("Abigail Miller", "134546456334563"));
			staffComboBox.Items.Add(new ComboxItem("Buonanno Joseph", "465789767845676"));
			staffComboBox.SelectedIndex = 0;

			AccessComboBox.Items.Add(new ComboxItem("", ""));
			AccessComboBox.Items.Add(new ComboxItem("Front Door", "I-000001"));
			AccessComboBox.Items.Add(new ComboxItem("Metting Room", "I-000003"));
			AccessComboBox.Items.Add(new ComboxItem("Server Room", "I-000008"));
			AccessComboBox.SelectedIndex = 0;
		}

		//protected delegate void SmartSearchResultDelegate(Object sender, EventArgs<String> e);
		public override void SmartSearchResult(Object sender, EventArgs<string> e)
		{
			if (InvokeRequired)
			{
				Invoke(new SmartSearchResultDelegate(SmartSearchResult), sender, e);
				return;
			}

			var tmpDoc = Xml.LoadXml(e.Value);
			if ((staffComboBox.Text != "") || (AccessComboBox.Text != ""))
			{
				var nodes = tmpDoc.SelectNodes("SmartSearch/Time");

				foreach (XmlElement node in nodes)
				{
					//1334143534554,I-000003,134546456334563
					var atr = node.Attributes["status"].Value.Split(',');

					var bRemove = true;
					if (((ComboxItem)AccessComboBox.SelectedItem).Value != "")
					{
						if (atr[1] == ((ComboxItem) AccessComboBox.SelectedItem).Value) bRemove = false;
						if ((bRemove) && (node.ParentNode != null)) node.ParentNode.RemoveChild(node);
					}

					if (((ComboxItem)staffComboBox.SelectedItem).Value != "")
					{
						bRemove = true;
						if (atr[2] == ((ComboxItem) staffComboBox.SelectedItem).Value) bRemove = false;
						if ((bRemove) && (node.ParentNode != null)) node.ParentNode.RemoveChild(node);
					}
				}
			}

			_found += tmpDoc.GetElementsByTagName("Time").Count;

			if (OnSmartSearchResult != null)
				OnSmartSearchResult(this, new EventArgs<string>(tmpDoc.InnerXml));
		}
	}
}
