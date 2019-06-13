using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using PanelBase;
using SetupPOS;
using Manager = SetupBase.Manager;

namespace SetupStore
{
	public sealed partial class EditPanel : UserControl
	{
		public IStore Store;
		public IPTS PTS;
		public Dictionary<String, String> Localization;

		public Boolean IsEditing;

		private readonly Queue<POSPanel> _recycleItem = new Queue<POSPanel>();

        private readonly PageSelector _pageSelector;

        public EditPanel()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"MessageBox_Information", "Information"},

                                   {"Store_itemId", "Id"},

                                   {"Store_Name", "Name"},
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

        private void POSPanelOnSelectAll(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (POSPanel control in containerPanel.Controls)
            {
                control.Checked = true;
            }
            containerPanel.AutoScroll = true;
        }

        private void POSPanelOnSelectNone(Object sender, EventArgs e)
        {
            containerPanel.AutoScroll = false;
            foreach (POSPanel control in containerPanel.Controls)
            {
                control.Checked = false;
            }
            containerPanel.AutoScroll = true;
        }

        private void POSPanelOnSelectChange(Object sender, EventArgs e)
        {
            var panel = sender as POSPanel;
            if (panel == null) return;

            var selectAll = false;

            if (panel.Checked)
            {
                selectAll = true;
                foreach (POSPanel control in containerPanel.Controls)
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
                if (!Store.Pos.ContainsKey(panel.POS.Id))
                    Store.Pos[panel.POS.Id] = panel.POS;
            }
            else
            {
                if (Store.Pos.ContainsKey(panel.POS.Id))
                    Store.Pos.Remove(panel.POS.Id);
            }

            var title = containerPanel.Controls[containerPanel.Controls.Count - 1] as POSPanel;
            if (title != null && title.IsTitle && title.Checked != selectAll)
            {
                title.OnSelectAll -= POSPanelOnSelectAll;
                title.OnSelectNone -= POSPanelOnSelectNone;

                title.Checked = selectAll;

                title.OnSelectAll += POSPanelOnSelectAll;
                title.OnSelectNone += POSPanelOnSelectNone;
            }
        }

        private void ClearViewModel()
        {
            foreach (POSPanel itemPanel in containerPanel.Controls)
            {
                itemPanel.OnSelectChange -= POSPanelOnSelectChange;

                itemPanel.SelectionVisible = false;

                itemPanel.Checked = false;
                itemPanel.POS = null;
                itemPanel.Cursor = Cursors.Hand;
                itemPanel.EditVisible = true;

                itemPanel.OnSelectChange += POSPanelOnSelectChange;

                if (itemPanel.IsTitle)
                {
                    itemPanel.OnSelectAll -= POSPanelOnSelectAll;
                    itemPanel.OnSelectNone -= POSPanelOnSelectNone;
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

            List<IPOS> sortResult = null;
            if (PTS != null)
            {
                sortResult = new List<IPOS>(PTS.POS.POSServer);
            }

            if (sortResult == null)
            {
                return;
            }

            sortResult.Sort((x, y) => (x.Id.CompareTo(y.Id)));

            if (sortResult.Count == 0)
            {
                return;
            }

            _pageSelector.SelectPage = _page;
            _pageSelector.Pages = Convert.ToInt32(Math.Ceiling(sortResult.Count / (CountPerPage * 1.0)));
            _pageSelector.ShowPages();


            List<IPOS> pageResult = new List<IPOS>();
            var start = (_page - 1) * CountPerPage;
            var end = start - 1 + CountPerPage;
            if (end > sortResult.Count) end = sortResult.Count - 1;

            var idx = -1;
            foreach (IPOS pos in sortResult)
            {
                idx++;
                if (idx < start) continue;
                if (idx > end) break;

                pageResult.Insert(0, pos);
            }

            containerPanel.Visible = false;
            foreach (IPOS pos in pageResult)
            {
                if (pos.IsCapture) continue;
                POSPanel posPanel = GetPOSPanel();

                posPanel.POS = pos;
                posPanel.EditVisible = false;
                posPanel.SelectionVisible = true;

                posPanel.OnSelectChange -= POSPanelOnSelectChange;
                if (Store.Pos.ContainsKey(pos.Id))
                    posPanel.Checked = true;
                else
                {
                    posPanel.Checked = false;

                    foreach (IStore value in PTS.POS.StoreManager.Values)
                    {
                        if (value.Pos.ContainsKey(pos.Id))
                        {
                            posPanel.SelectionVisible = false;
                            break;
                        }
                    }
                }

                posPanel.OnSelectChange += POSPanelOnSelectChange;

                containerPanel.Controls.Add(posPanel);
            }

            POSPanel titlePanel = GetPOSPanel();
            titlePanel.IsTitle = true;
            titlePanel.Cursor = Cursors.Default;
            titlePanel.EditVisible = false;
            titlePanel.OnSelectAll += POSPanelOnSelectAll;
            titlePanel.OnSelectNone += POSPanelOnSelectNone;
            containerPanel.Controls.Add(titlePanel);
            containerPanel.Visible = true;

            containerPanel.AutoScroll = false;
            containerPanel.Focus();
            containerPanel.AutoScroll = true;
        }

        private POSPanel GetPOSPanel()
        {
            if (_recycleItem.Count > 0)
            {
                return _recycleItem.Dequeue();
            }

            var posPanel = new POSPanel
            {
                SelectedColor = Manager.DeleteTextColor,
                EditVisible = true,
                SelectionVisible = false,
            };

            posPanel.OnSelectChange -= POSPanelOnSelectChange;
            posPanel.OnSelectChange += POSPanelOnSelectChange;

            return posPanel;
        }

        public Brush SelectedColor
        {
            set
            {
                foreach (POSPanel control in containerPanel.Controls)
                    control.SelectedColor = value;
            }
        }

		public void PaintInput(Object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Manager.Paint(g, (Control)sender);

			if (Localization.ContainsKey("Store_" + ((Control)sender).Tag))
				Manager.PaintText(g, Localization["Store_" + ((Control)sender).Tag]);
			else
				Manager.PaintText(g, ((Control)sender).Tag.ToString());
		}

		public void ParseDivision()
		{
			if (Store == null) return;

			IsEditing = false;

			nameTextBox.Text = Store.Name;
			itemIDTextBox.Text = Store.Id.ToString();

		    GenerateViewModel();

            posPanel.Focus();
            //nameTextBox.Focus();

			IsEditing = true;
		}
        
		private void NameTextBoxTextChanged(Object sender, EventArgs e)
		{
			if (!IsEditing) return;

            Store.Name = nameTextBox.Text;
			POSIsModify();
		}

		public void POSIsModify()
		{
			if (Store.ReadyState == ReadyState.Ready)
                Store.ReadyState = ReadyState.Modify;

			//PTS.POSModify(POS);
		}
	}
}
