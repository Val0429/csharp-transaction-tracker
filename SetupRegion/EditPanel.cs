using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using Manager = SetupBase.Manager;

namespace SetupRegion
{
	public sealed partial class EditPanel : UserControl
	{
	    public IRegion Region;
        public IPTS PTS;
		public Dictionary<String, String> Localization;

		public Boolean IsEditing;

		private readonly Queue<SetupStore.ItemPanel> _recycleItem = new Queue<SetupStore.ItemPanel>();

        private readonly PageSelector _pageSelector;

        public EditPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},

                                   {"City_itemId", "Id"},

                                   {"City_Name", "Name"},
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

        private void StorePanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (SetupStore.ItemPanel control in containerPanel.Controls)
            {
                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void StorePanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (SetupStore.ItemPanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }

        private void StorePanelOnSelectChange(Object sender, EventArgs e)
        {
            var panel = sender as SetupStore.ItemPanel;
            if (panel == null) return;

            var selectAll = false;

            if (panel.Checked)
            {
                selectAll = true;
                foreach (SetupStore.ItemPanel control in containerPanel.Controls)
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
                if (!Region.Stores.ContainsKey(panel.Store.Id))
                    Region.Stores[panel.Store.Id] = panel.Store;
            }
            else
            {
                if (Region.Stores.ContainsKey(panel.Store.Id))
                    Region.Stores.Remove(panel.Store.Id);
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as SetupStore.ItemPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= StorePanelOnSelectAll;
                title.OnSelectNone -= StorePanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += StorePanelOnSelectAll;
                title.OnSelectNone += StorePanelOnSelectNone;
            }
        }

        private void ClearViewModel()
        {
            foreach (SetupStore.ItemPanel itemPanel in containerPanel.Controls)
            {
                itemPanel.OnSelectChange -= StorePanelOnSelectChange;

                itemPanel.SelectionVisible = false;

                itemPanel.Checked = false;
                itemPanel.Store = null;
                itemPanel.Cursor = Cursors.Hand;
                itemPanel.EditVisible = true;

                itemPanel.OnSelectChange += StorePanelOnSelectChange;

                if (itemPanel.IsTitle)
                {
                    itemPanel.OnSelectAll -= StorePanelOnSelectAll;
                    itemPanel.OnSelectNone -= StorePanelOnSelectNone;
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

            List<IStore> sortResult = null;
            if (PTS != null)
            {
                sortResult = new List<IStore>(PTS.POS.StoreManager.Values);
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


            List<IStore> pageResult = new List<IStore>();
            var start = (_page - 1) * CountPerPage;
            var end = start - 1 + CountPerPage;
            if (end > sortResult.Count) end = sortResult.Count - 1;

            var idx = -1;
            foreach (IStore store in sortResult)
            {
                idx++;
                if (idx < start) continue;
                if (idx > end) break;

                pageResult.Insert(0, store);
            }

            containerPanel.Visible = false;
            foreach (IStore store in pageResult)
            {
                SetupStore.ItemPanel storePanel = GetStorePanel();

                storePanel.Store = store;
                storePanel.EditVisible = false;
                storePanel.SelectionVisible = true;

                storePanel.OnSelectChange -= StorePanelOnSelectChange;
                if (Region.Stores.ContainsKey(store.Id))
                    storePanel.Checked = true;
                else
                {
                    storePanel.Checked = false;

                    foreach (IRegion value in PTS.POS.RegionManager.Values)
                    {
                        if (value.Stores.ContainsKey(store.Id))
                        { 
                            storePanel.SelectionVisible = false;
                            break;
                        }
                    }
                }

                storePanel.OnSelectChange += StorePanelOnSelectChange;

                containerPanel.Controls.Add(storePanel);
            }

            SetupStore.ItemPanel titlePanel = GetStorePanel();
            titlePanel.IsTitle = true;
            titlePanel.Cursor = Cursors.Default;
            titlePanel.EditVisible = false;
            titlePanel.OnSelectAll += StorePanelOnSelectAll;
            titlePanel.OnSelectNone += StorePanelOnSelectNone;
            containerPanel.Controls.Add(titlePanel);
            containerPanel.Visible = true;

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
        }

        private SetupStore.ItemPanel GetStorePanel()
        {
            if (_recycleItem.Count > 0)
            {
                return _recycleItem.Dequeue();
            }

            var posPanel = new SetupStore.ItemPanel
            {
                SelectedColor = Manager.DeleteTextColor,
                EditVisible = true,
                SelectionVisible = false,
            };

            posPanel.OnSelectChange -= StorePanelOnSelectChange;
            posPanel.OnSelectChange += StorePanelOnSelectChange;

            return posPanel;
        }

        public Brush SelectedColor
        {
            set
            {
                foreach (SetupStore.ItemPanel control in containerPanel.Controls)
                    control.SelectedColor = value;
            }
        }

		public void PaintInput(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.Paint(g, (Control)sender);

			if (Localization.ContainsKey("City_" + ((Control)sender).Tag))
				Manager.PaintText(g, Localization["City_" + ((Control)sender).Tag]);
			else
				Manager.PaintText(g, ((Control)sender).Tag.ToString());
		}

		public void ParseDivision()
		{
			if (Region == null) return;

			IsEditing = false;

			nameTextBox.Text = Region.Name;
			itemIDTextBox.Text = Region.Id.ToString();

		    GenerateViewModel();

            posPanel.Focus();
            //nameTextBox.Focus();

			IsEditing = true;
		}
        
		private void NameTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!IsEditing) return;

            Region.Name = nameTextBox.Text;
			POSIsModify();
		}

		public void POSIsModify()
		{
			if (Region.ReadyState == ReadyState.Ready)
                Region.ReadyState = ReadyState.Modify;

			//PTS.POSModify(POS);
		}
	}
}
