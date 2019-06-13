using System.Windows.Forms;
using System.Xml;

namespace Layout.Plugin
{
	public class LayoutManager : Layout.LayoutManager
	{
		protected override void CreateBlocks(XmlDocument configNode)
		{
			var blocks = configNode.GetElementsByTagName("Block");
			if (blocks.Count <= 0) return;

			foreach (XmlElement node in blocks)
			{
				var blockPanel = new BlockPanel
				                 	{
				                 		LayoutManager = this,
				                 		BlockNode = node,
				                 	};

				if (blockPanel.Dock != DockStyle.Fill)
				{
					BlockPanels.Add(blockPanel);
				}
				else
					BlockPanels.Insert(0, blockPanel);
			}
		}
	}
}
