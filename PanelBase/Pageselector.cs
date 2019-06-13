using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;

namespace PanelBase
{
	public sealed partial class PageSelector : UserControl
	{
		public event EventHandler<EventArgs<Int32>> OnSelectionChange;

		private readonly PictureBox _firstPage;
		private readonly PictureBox _lastPage;
		private readonly PictureBox _nextPage;
		private readonly PictureBox _previousPage;

		public Dictionary<String, String> Localization;
		public Int32 SelectPage;
		public Int32 Pages;
		
		public FlowDirection FlowDirection
		{
			set
			{
				flowLayoutPanel.FlowDirection = value;
			}
		}

		public PageSelector()
		{
			Localization = new Dictionary<String, String>
							   {
								   {"PageSelector_Prev", "Previous Page"},
								   {"PageSelector_Next", "Next Page"},
								   {"PageSelector_First", "First Page"},
								   {"PageSelector_Last", "Last Page"},
							   };
			Localizations.Update(Localization);

			InitializeComponent();
			
			Dock = DockStyle.Fill;
			DoubleBuffered = true;

			_firstPage = new PictureBox
			{
				Size = new Size(25, 25),
				Cursor = Cursors.Hand,
				SizeMode = PictureBoxSizeMode.CenterImage,
				Image = Resources.GetResources(Properties.Resources.firstPage, Properties.Resources.IMGFirstPage)
			};
			_firstPage.MouseClick += FirstPageMouseClick;
			SharedToolTips.SharedToolTip.SetToolTip(_firstPage, Localization["PageSelector_First"]);

			_lastPage = new PictureBox
			{
				Size = new Size(25, 25),
				Cursor = Cursors.Hand,
				SizeMode = PictureBoxSizeMode.CenterImage,
				Image = Resources.GetResources(Properties.Resources.lastPage, Properties.Resources.IMGLastPage)
			};
			_lastPage.MouseClick += LastPageMouseClick;
			SharedToolTips.SharedToolTip.SetToolTip(_lastPage, Localization["PageSelector_Last"]);

			_nextPage = new PictureBox
			{
				Size = new Size(25, 25),
				Cursor = Cursors.Hand,
				SizeMode = PictureBoxSizeMode.CenterImage,
				Image = Resources.GetResources(Properties.Resources.nextPage, Properties.Resources.IMGNextPage)
			};
			_nextPage.MouseClick += NextPageMouseClick;
			SharedToolTips.SharedToolTip.SetToolTip(_nextPage, Localization["PageSelector_Next"]);

			_previousPage = new PictureBox
			{
				Size = new Size(25, 25),
				Cursor = Cursors.Hand,
				SizeMode = PictureBoxSizeMode.CenterImage,
				Image = Resources.GetResources(Properties.Resources.previousPage, Properties.Resources.IMGPreviousPage)
			};
			_previousPage.MouseClick += PreviousPageMouseClick;
			SharedToolTips.SharedToolTip.SetToolTip(_previousPage, Localization["PageSelector_Prev"]);
		}

		private Boolean _hidePageButon;
		public void HidePageButton()
		{
			_hidePageButon = true;
			_nextPage.Visible = _previousPage.Visible = _firstPage.Visible = _lastPage.Visible = false;
		}

		private void FirstPageMouseClick(Object sender, MouseEventArgs e)
		{
			SelectPage = 1;
			ShowPages();
			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<Int32>(SelectPage));
		}

		private void LastPageMouseClick(Object sender, MouseEventArgs e)
		{
			SelectPage = Pages;
			ShowPages();
			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<Int32>(SelectPage));
		}

		private void NextPageMouseClick(Object sender, MouseEventArgs e)
		{
			SelectPage++;
			ShowPages();
			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<Int32>(SelectPage));
		}

		private void PreviousPageMouseClick(Object sender, MouseEventArgs e)
		{
			SelectPage--;
			ShowPages();
			if (OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<Int32>(SelectPage));
		}

		public Color NormalColor = Color.Black;
		public Color SelectColor = Color.FromArgb(245, 125, 5);
		public Int32 PageLabelHeight = 0;
		private readonly Queue<PageNumberPanel> _recyclePagePanel = new Queue<PageNumberPanel>();
		private PageNumberPanel GetPagePanel()
		{
			if (_recyclePagePanel.Count > 0)
			{
				return _recyclePagePanel.Dequeue();
			}

			var pagePanel = new PageNumberPanel
			{
				NormalColor = NormalColor,
				SelectColor = SelectColor,
				IsSelected = false
			};
			if (PageLabelHeight != 0)
			{
				pagePanel.MinimumSize = new Size(pagePanel.MinimumSize.Width, PageLabelHeight);
			}

			pagePanel.MouseClick += PageNumberPanelOnMouseClick;

			return pagePanel;
		}

		private void PageNumberPanelOnMouseClick(Object sender, MouseEventArgs e)
		{
			var pagePanel = sender as PageNumberPanel;
			if (pagePanel == null || pagePanel.Index == 0) return;
			
			SelectPage = pagePanel.Index;
			ShowPages();

			if(OnSelectionChange != null)
				OnSelectionChange(this, new EventArgs<Int32>(SelectPage));
		}

		public UInt16 PagesDisplay = 20;
		public void ShowPages()
		{
			Visible = false;
			ClearViewModel();

			//only 1 page , no page
			if(Pages <= 1) return;

			if (flowLayoutPanel.FlowDirection == FlowDirection.LeftToRight)
				PageLeftToRight();
			else
				PageRightToLeft();

			Visible = true;
		}

		private List<Control> _controls = new List<Control>();
		private void PageSelectorVisibleChanged(Object sender, EventArgs e)
		{
			VisibleChanged -= PageSelectorVisibleChanged;

			foreach (var control in _controls)
			{
				flowLayoutPanel.Controls.Add(control);
			}
			_controls.Clear();
		}

		private void PageLeftToRight()
		{
			var waitToAppend = !Visible;
			if (waitToAppend)
			{
				_controls.Clear();
				VisibleChanged -= PageSelectorVisibleChanged;
				VisibleChanged += PageSelectorVisibleChanged;
			}

			if (!_hidePageButon)
			{
				if (SelectPage > 1)
				{
					if (waitToAppend)
					{
						_controls.Add(_firstPage);
						_controls.Add(_previousPage);
					}
					else
					{
						flowLayoutPanel.Controls.Add(_firstPage);
						flowLayoutPanel.Controls.Add(_previousPage);
					}
				}
				else
				{
					//add 2 blank space at left
					if (waitToAppend)
					{
						_controls.Add(GetPagePanel());
						_controls.Add(GetPagePanel());
					}
					else
					{
						flowLayoutPanel.Controls.Add(GetPagePanel());
						flowLayoutPanel.Controls.Add(GetPagePanel());
					}
				}
			}

			var start = SelectPage - PagesDisplay / 2;// 10;
			var end = SelectPage + ((PagesDisplay / 2) - 1); // 9;
			if (start < 1)
			{
				end += (1 - start);
				end = Math.Min(end, Pages);
				start = 1;
			}
			else if (end > Pages)
			{
				start -= (end - Pages);
				start = Math.Max(start, 1);
				end = Pages;
			}

			for (var i = start; i <= end; i++)
			{
				var panel = GetPagePanel();
				panel.Index = Convert.ToInt32(i);
				if (i == SelectPage)
				{
					panel.IsSelected = true;
				}

				if (waitToAppend)
				{
					_controls.Add(panel);
				}
				else
				{
					flowLayoutPanel.Controls.Add(panel);
				}
			}

			if (!_hidePageButon)
			{
				if (SelectPage < Pages)
				{

					if (waitToAppend)
					{
						_controls.Add(_nextPage);
						_controls.Add(_lastPage);
					}
					else
					{
						flowLayoutPanel.Controls.Add(_nextPage);
						flowLayoutPanel.Controls.Add(_lastPage);
					}
				}
			}
		}

		private void PageRightToLeft()
		{
			var waitToAppend = !Visible;
			if (waitToAppend)
			{
				_controls.Clear();
				VisibleChanged -= PageSelectorVisibleChanged;
				VisibleChanged += PageSelectorVisibleChanged;
			}

			if (!_hidePageButon)
			{
				if (SelectPage < Pages)
				{
					if (waitToAppend)
					{
						_controls.Add(_lastPage);
						_controls.Add(_nextPage);
					}
					else
					{
						flowLayoutPanel.Controls.Add(_lastPage);
						flowLayoutPanel.Controls.Add(_nextPage);
					}
				}
				else
				{
					//add 2 blank space at right
					if (waitToAppend)
					{
						_controls.Add(GetPagePanel());
						_controls.Add(GetPagePanel());
					}
					else
					{
						flowLayoutPanel.Controls.Add(GetPagePanel());
						flowLayoutPanel.Controls.Add(GetPagePanel());
					}
				}
			}

			var start = SelectPage - PagesDisplay / 2;// 10;
			var end = SelectPage + ((PagesDisplay / 2) - 1); // 9;
			if (start < 1)
			{
				end += (1 - start);
				end = Math.Min(end, Pages);
				start = 1;
			}
			else if (end > Pages)
			{
				start -= (end - Pages);
				start = Math.Max(start, 1);
				end = Pages;
			}

			for (var i = end; i >= start; i--)
			{
				var panel = GetPagePanel();
				panel.Index = Convert.ToInt32(i);
				if (i == SelectPage)
				{
					panel.IsSelected = true;
				}

				if (waitToAppend)
				{
					_controls.Add(panel);
				}
				else
				{
					flowLayoutPanel.Controls.Add(panel);
				}
			}

			if (!_hidePageButon)
			{
				if (SelectPage > 1)
				{
					if (waitToAppend)
					{
						_controls.Add(_previousPage);
						_controls.Add(_firstPage);
					}
					else
					{
						flowLayoutPanel.Controls.Add(_previousPage);
						flowLayoutPanel.Controls.Add(_firstPage);
					}
				}
			}
		}

		public void ClearViewModel()
		{
			flowLayoutPanel.Controls.Remove(_firstPage);
			flowLayoutPanel.Controls.Remove(_lastPage);
			flowLayoutPanel.Controls.Remove(_nextPage);
			flowLayoutPanel.Controls.Remove(_previousPage);

			foreach (PageNumberPanel panel in flowLayoutPanel.Controls)
			{
				panel.IsSelected = false;
				panel.Index = 0;
				if(!_recyclePagePanel.Contains(panel))
					_recyclePagePanel.Enqueue(panel);
			}

			flowLayoutPanel.Controls.Clear();
		}
	}

	public sealed class PageNumberPanel : Label
	{
		private Int32 _index;
		public Int32 Index
		{
			get { return _index; }
			set {
				_index = value;
				if (value == 0)
				{
					Text = "";
					Width = 25;
					Cursor = Cursors.Default;
				}
				else
				{
					Text = value.ToString();
					Cursor = Cursors.Hand;
					//if (value < 100)
					//    Width = 25;
					//else if (value < 1000)
					//    Width = 33;
					//else
					//    Width = 42;
				}
			}
		}

		public Color NormalColor = Color.Black;
		public Color SelectColor = Color.FromArgb(245, 125, 5);
		public Boolean IsSelected
		{
			set
			{
				ForeColor = (value) ? SelectColor : NormalColor;
			}
		}

		public PageNumberPanel()
		{
			Size = new Size(25, 25);
			MinimumSize = new Size(25, 25);
			AutoSize = true;
			Cursor = Cursors.Hand;
			Dock = DockStyle.Left;
		    DoubleBuffered = true;
			Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			TextAlign = ContentAlignment.MiddleCenter;
			ForeColor = NormalColor;
		}
	}
}
