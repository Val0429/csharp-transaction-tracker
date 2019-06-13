using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using Manager = SetupBase.Manager;

namespace SetupDivision
{
	public sealed partial class EditPanel : UserControl
	{
		public IDivision Division;
		public IPTS PTS;
		public Dictionary<String, String> Localization;

		public Boolean IsEditing;

		private readonly Queue<SetupRegion.ItemPanel> _recycleItem = new Queue<SetupRegion.ItemPanel>();

        private readonly PageSelector _pageSelector;

        public EditPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},

                                   {"Region_itemId", "Id"},

                                   {"Region_Name", "Name"},
							   };
			Localizations.Update(Localization);
			InitializeComponent();
			DoubleBuffered = true;
			Dock = DockStyle.Fill;

			BackgroundImage = Manager.BackgroundNoBorder;

		    itemIDTextBox.ReadOnly = true;

            _pageSelector = new PageSelector
            {
                PagesDisplay = 10,
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Color.Transparent
            };
            _pageSelector.OnSelectionChange += PageSelectorOnSelectionChange;
            pageSelectorPanel.Controls.Add(_pageSelector);
            _pageSelector.BringToFront();

        }

        private Int32 _page = 1;
        private const UInt16 CountPerPage = 20;

        private void PageSelectorOnSelectionChange(Object sender, EventArgs<Int32> e)
        {
            _page = _pageSelector.SelectPage;

            containerPanel.Visible = false;

            GenerateViewModel();

            containerPanel.Visible = true;
        }

        public void Initialize()
		{
			namePanel.Paint += PaintInput;
			itemIDPanel.Paint += PaintInput;

            nameTextBox.TextChanged += NameTextBoxTextChanged;
        }

        private void RegionPanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (SetupRegion.ItemPanel control in containerPanel.Controls)
            {
                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void RegionPanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (SetupRegion.ItemPanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }

        private void RegionPanelOnSelectChange(Object sender, EventArgs e)
        {
            var panel = sender as SetupRegion.ItemPanel;
            if (panel == null) return;

            var selectAll = false;

            if (panel.Checked)
            {
                selectAll = true;
                foreach (SetupRegion.ItemPanel control in containerPanel.Controls)
                {
                    if (control.IsTitle) continue;
                    if (!control.Checked && control.Enabled)
                    {
                        selectAll = false;
                        break;
                    }
                }
            }


            if (panel.Checked)
            {
                if (!Division.Regions.ContainsKey(panel.Region.Id))
                    Division.Regions[panel.Region.Id] = panel.Region;
            }
            else
            {
                if (Division.Regions.ContainsKey(panel.Region.Id))
                    Division.Regions.Remove(panel.Region.Id);
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as SetupRegion.ItemPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= RegionPanelOnSelectAll;
                title.OnSelectNone -= RegionPanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += RegionPanelOnSelectAll;
                title.OnSelectNone += RegionPanelOnSelectNone;
            }
        }

        private void ClearViewModel()
        {
            foreach (SetupRegion.ItemPanel itemPanel in containerPanel.Controls)
            {
                itemPanel.OnSelectChange -= RegionPanelOnSelectChange;

                itemPanel.SelectionVisible = false;

                itemPanel.Checked = false;
                itemPanel.Region = null;
                itemPanel.Cursor = Cursors.Hand;
                itemPanel.EditVisible = true;

                itemPanel.OnSelectChange += RegionPanelOnSelectChange;

                if (itemPanel.IsTitle)
                {
                    itemPanel.OnSelectAll -= RegionPanelOnSelectAll;
                    itemPanel.OnSelectNone -= RegionPanelOnSelectNone;
                    itemPanel.IsTitle = false;
                }

                if (!_recycleItem.Contains(itemPanel))
                {
                    _recycleItem.Enqueue(itemPanel);
                }
            }
            containerPanel.Controls.Clear();
        }

        public void GenerateViewModel()
        {
            ClearViewModel();

            List<IRegion> sortResult = null;
            if (PTS != null)
            {
                sortResult = new List<IRegion>(PTS.POS.RegionManager.Values);
            }

            if (sortResult == null)
            {
                return;
            }

            sortResult.Sort((x, y) => (x.Id - y.Id));

            if (sortResult.Count == 0)
            {
                return;
            }


            _pageSelector.SelectPage = _page;
            _pageSelector.Pages = Convert.ToInt32(Math.Ceiling(sortResult.Count / (CountPerPage * 1.0)));
            _pageSelector.ShowPages();


            List<IRegion> pageResult = new List<IRegion>();
            var start = (_page - 1) * CountPerPage;
            var end = start - 1 + CountPerPage;
            if (end > sortResult.Count) end = sortResult.Count - 1;

            var idx = -1;
            foreach (IRegion region in sortResult)
            {
                idx++;
                if (idx < start) continue;
                if (idx > end) break;

                pageResult.Insert(0, region);
            }

            containerPanel.Visible = false;
            foreach (IRegion region in pageResult)
            {
                SetupRegion.ItemPanel regionPanel = GetRegionPanel();

                regionPanel.Region = region;
                regionPanel.EditVisible = false;
                regionPanel.SelectionVisible = true;

                regionPanel.OnSelectChange -= RegionPanelOnSelectChange;
                if (Division.Regions.ContainsKey(region.Id))
                    regionPanel.Checked = true;
                else
                {
                    regionPanel.Checked = false;

                    foreach (IDivision value in PTS.POS.DivisionManager.Values)
                    {
                        if (value.Regions.ContainsKey(region.Id))
                        {
                            regionPanel.SelectionVisible = false;
                            break;
                        }
                    }
                }

                regionPanel.OnSelectChange += RegionPanelOnSelectChange;

                containerPanel.Controls.Add(regionPanel);
            }

            SetupRegion.ItemPanel titlePanel = GetRegionPanel();
            titlePanel.IsTitle = true;
            titlePanel.Cursor = Cursors.Default;
            titlePanel.EditVisible = false;
            titlePanel.OnSelectAll += RegionPanelOnSelectAll;
            titlePanel.OnSelectNone += RegionPanelOnSelectNone;
            containerPanel.Controls.Add(titlePanel);
            containerPanel.Visible = true;

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
        }

        private SetupRegion.ItemPanel GetRegionPanel()
        {
            if (_recycleItem.Count > 0)
            {
                return _recycleItem.Dequeue();
            }

            var posPanel = new SetupRegion.ItemPanel
            {
                SelectedColor = Manager.DeleteTextColor,
                EditVisible = true,
                SelectionVisible = false,
            };

            posPanel.OnSelectChange -= RegionPanelOnSelectChange;
            posPanel.OnSelectChange += RegionPanelOnSelectChange;

            return posPanel;
        }

        public Brush SelectedColor
        {
            set
            {
                foreach (SetupRegion.ItemPanel control in containerPanel.Controls)
                    control.SelectedColor = value;
            }
        }

		public void PaintInput(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.Paint(g, (Control)sender);

			if (Localization.ContainsKey("Region_" + ((Control)sender).Tag))
				Manager.PaintText(g, Localization["Region_" + ((Control)sender).Tag]);
			else
				Manager.PaintText(g, ((Control)sender).Tag.ToString());
		}

		public void ParseDivision()
		{
			if (Division == null) return;

			IsEditing = false;

			nameTextBox.Text = Division.Name;
			itemIDTextBox.Text = Division.Id.ToString();

		    GenerateViewModel();

            posPanel.Focus();
            //nameTextBox.Focus();

			IsEditing = true;
		}
        
		private void NameTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!IsEditing) return;

            Division.Name = nameTextBox.Text;
			POSIsModify();
		}

		public void POSIsModify()
		{
			if (Division.ReadyState == ReadyState.Ready)
                Division.ReadyState = ReadyState.Modify;

			//PTS.POSModify(POS);
		}
	}
}
