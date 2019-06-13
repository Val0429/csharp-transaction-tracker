using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;
using SetupBase;

namespace SetupException
{
    public sealed partial class EditPanel : UserControl
    {
        public POS_Exception POSException;
        public IPTS PTS;
        public Dictionary<String, String> Localization;

        public Boolean IsEditing;

        public EditPanel()
        {
            Localization = new Dictionary<String, String>
                               {
                                   {"MessageBox_Information", "Information"},
                                   
                                   {"POSException_Name", "Name"},
                                   {"POSException_Manufacture", "Manufacture"},
                                   {"POSException_TransactionType", "Transaction Type"},
                                   {"POS_Exception", "Exception"},
                                   {"POS_Segment", "Segment"},
                                   {"POS_Tag", "Tag"},
                                   {"POS_FilterPOSId", "Filter POS Id"},
                                   {"POS_FilterPOSIdYes", "Yes"},
                                    {"POSException_Separator", "Separator"},
                                   {"EditPOSExceptionPanel_Information", "Information"},
                               };
            Localizations.Update(Localization);
            InitializeComponent();
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            informationLabel.Text = Localization["EditPOSExceptionPanel_Information"];
            exceptionLabel.Text = Localization["POS_Exception"];
            segmentLabel.Text = Localization["POS_Segment"];
            tagLabel.Text = Localization["POS_Tag"];
            filterPODIdCheckBox.Text = Localization["POS_FilterPOSIdYes"];
            //nameTextBox.KeyPress += KeyAccept.AcceptNumberAndAlphaOnly;

            BackgroundImage = Manager.BackgroundNoBorder;
        }

        public void Initialize()
        {
            manufacturePanel.Paint += PaintInputTop;
            namePanel.Paint += PaintInputMiddle;
            transactionTypePanel.Paint += PaintInputMiddle2;
            separatorDoubleBufferPanel.Paint += PaintInputMiddle3;
            FilterPOSIdPanel.Paint += PaintInputBottom;

            foreach (String manufacture in POS_Exception.Manufactures)
            {
                manufactureComboBox.Items.Add(POS_Exception.ToDisplay(manufacture));
            }
            manufactureComboBox.SelectedIndex = 0;
            manufactureComboBox.Enabled = (POS_Exception.Manufactures.Length > 1);

            transactionTypeComboBox.Items.Add("Single");
            transactionTypeComboBox.Items.Add("Multi");
            transactionTypeComboBox.SelectedIndex = 0;

            nameTextBox.TextChanged += NameTextBoxTextChanged;

            manufactureComboBox.SelectedIndexChanged += ManufactureComboBoxSelectedIndexChanged;
            transactionTypeComboBox.SelectedIndexChanged += TransactionTypeComboBoxSelectedIndexChanged;
            filterPODIdCheckBox.CheckedChanged += FilterPODIdCheckBoxCheckedChanged;
            separatorTextBox.TextChanged += SeparatorTextBoxTextChanged;
        }

        public void PaintInputTop(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintTop(g, manufacturePanel);

            Manager.PaintText(g, Localization["POSException_Manufacture"]);
        }

        public void PaintInputMiddle(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintBottom(g, namePanel);

            Manager.PaintText(g, Localization["POSException_Name"]);
        }

        public void PaintInputMiddle2(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintBottom(g, transactionTypePanel);

            Manager.PaintText(g, Localization["POSException_TransactionType"]);
        }

        public void PaintInputMiddle3(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintBottom(g, separatorDoubleBufferPanel);

            Manager.PaintText(g, Localization["POSException_Separator"]);
        }

        public void PaintInputBottom(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Manager.PaintBottom(g, FilterPOSIdPanel);

            Manager.PaintText(g, Localization["POS_FilterPOSId"]);
        }

        public void ParsePOSException()
        {
            if (POSException == null) return;

            IsEditing = false;

            nameTextBox.Text = POSException.Name;

            manufactureComboBox.SelectedItem = POS_Exception.ToDisplay(POSException.Manufacture);
            transactionTypeComboBox.SelectedItem = POSException.TransactionType == 1 ? "Single" : "Multi";
            filterPODIdCheckBox.Checked = POSException.IsSupportPOSId;
            separatorTextBox.Text = POSException.Separator;
            Visible = false;

            containerPanel.AutoScroll = false;
            ClearViewModel();
            UpdateDisplayPanel();
            //--------------------------------------------------------------------------------
            foreach (var exception in POSException.Exceptions)
            {
                ExceptionPanel exceptionPanel = GetExceptionPanel();

                exceptionPanel.Name = exception.Key;
                exceptionPanel.Exception = exception;

                exceptionContainerPanel.Controls.Add(exceptionPanel);
            }

            exceptionLabel.Visible = POSException.Exceptions.Count > 0;
            //ExceptionPanel exceptionTitlePanel = GetExceptionPanel();
            //exceptionTitlePanel.IsTitle = true;
            //exceptionTitlePanel.Cursor = Cursors.Default;
            //exceptionTitlePanel.EditVisible = false;
            //exceptionTitlePanel.OnSelectAll += ExceptionPanelOnSelectAll;
            //exceptionTitlePanel.OnSelectNone += ExceptionPanelOnSelectNone;
            //exceptionContainerPanel.Controls.Add(exceptionTitlePanel);
            //--------------------------------------------------------------------------------
            if (POSException.Segments.Count == 0)
            {
                segmentLabel.Visible = segmentContainerPanel.Visible = false;
            }
            else
            {
                segmentLabel.Visible = segmentContainerPanel.Visible = true;
            }

            POSException.Segments.Sort((x, y) => (y.Key.CompareTo(x.Key)));
            foreach (var segment in POSException.Segments)
            {
                SegmentPanel segmentPanel = GetSegmentPanel();

                segmentPanel.Name = segment.Key;
                segmentPanel.Segment = segment;

                segmentContainerPanel.Controls.Add(segmentPanel);
            }
            
            //SegmentPanel segmentsTitlePanel = GetSegmentPanel();
            //segmentsTitlePanel.IsTitle = true;
            //segmentsTitlePanel.Cursor = Cursors.Default;
            //exceptionTitlePanel.EditVisible = false;
            //exceptionTitlePanel.OnSelectAll += ExceptionPanelOnSelectAll;
            //exceptionTitlePanel.OnSelectNone += ExceptionPanelOnSelectNone;
            //segmentContainerPanel.Controls.Add(segmentsTitlePanel);
            //--------------------------------------------------------------------------------
            foreach (var tag in POSException.Tags)
            {
                TagPanel tagPanel = GetTagPanel();

                tagPanel.Name = tag.Key;
                tagPanel.Tag = tag;

                tagContainerPanel.Controls.Add(tagPanel);
            }

            //TagPanel tagTitlePanel = GetTagPanel();
            //tagTitlePanel.IsTitle = true;
            //tagTitlePanel.Cursor = Cursors.Default;
            //exceptionTitlePanel.EditVisible = false;
            //exceptionTitlePanel.OnSelectAll += ExceptionPanelOnSelectAll;
            //exceptionTitlePanel.OnSelectNone += ExceptionPanelOnSelectNone;
            //tagContainerPanel.Controls.Add(tagTitlePanel);
            //--------------------------------------------------------------------------------

            containerPanel.AutoScroll = true;
            Visible = true;
            //containerPanel.AutoScrollPosition = new Point(0, 0);
            containerPanel.Select();

            IsEditing = true;
        }

        private void UpdateDisplayPanel()
        {
            switch (POSException.Manufacture)
            {
                case "Generic":
                    transactionTypePanel.Visible =
                    separatorDoubleBufferPanel.Visible=
                    FilterPOSIdPanel.Visible = true;
                    break;

                default:
                    transactionTypePanel.Visible =
                    separatorDoubleBufferPanel.Visible =
                    FilterPOSIdPanel.Visible = false;
                    break;
            }
        }

        private readonly Queue<ExceptionPanel> _recycleException = new Queue<ExceptionPanel>();
        private ExceptionPanel GetExceptionPanel()
        {
            if (_recycleException.Count > 0)
            {
                return _recycleException.Dequeue();
            }

            var exceptionPanel = new ExceptionPanel
            {
                SelectedColor = Manager.DeleteTextColor,
            };

            //exceptionPanel.OnExceptionEditClick += ExceptionPanelOnExceptionEditClick;
            //exceptionPanel.OnSelectChange += ExceptionPanelOnSelectChange;

            return exceptionPanel;
        }

        private readonly Queue<SegmentPanel> _recycleSegment = new Queue<SegmentPanel>();
        private SegmentPanel GetSegmentPanel()
        {
            if (_recycleSegment.Count > 0)
            {
                return _recycleSegment.Dequeue();
            }

            var segmentPanel = new SegmentPanel
            {
                SelectedColor = Manager.DeleteTextColor,
            };

            //exceptionPanel.OnExceptionEditClick += ExceptionPanelOnExceptionEditClick;
            //exceptionPanel.OnSelectChange += ExceptionPanelOnSelectChange;

            return segmentPanel;
        }

        private readonly Queue<TagPanel> _recycleTag = new Queue<TagPanel>();
        private TagPanel GetTagPanel()
        {
            if (_recycleTag.Count > 0)
            {
                return _recycleTag.Dequeue();
            }

            var tagPanel = new TagPanel
            {
                SelectedColor = Manager.DeleteTextColor,
            };

            //exceptionPanel.OnExceptionEditClick += ExceptionPanelOnExceptionEditClick;
            //exceptionPanel.OnSelectChange += ExceptionPanelOnSelectChange;

            return tagPanel;
        }

        private void ClearViewModel()
        {
            foreach (ExceptionPanel exceptionPanel in exceptionContainerPanel.Controls)
            {
                exceptionPanel.Exception = null;
                //exceptionPanel.Cursor = Cursors.Hand;

                if (exceptionPanel.IsTitle)
                {
                    //exceptionPanel.OnSelectAll -= POSExceptionPanelOnSelectAll;
                    //exceptionPanel.OnSelectNone -= POSExceptionPanelOnSelectNone;
                    exceptionPanel.IsTitle = false;
                }

                if (!_recycleException.Contains(exceptionPanel))
                {
                    _recycleException.Enqueue(exceptionPanel);
                }
            }
            exceptionContainerPanel.Controls.Clear();

            foreach (SegmentPanel segmentPanel in segmentContainerPanel.Controls)
            {
                segmentPanel.Segment = null;
                //segmentPanel.Cursor = Cursors.Hand;

                if (segmentPanel.IsTitle)
                {
                    //exceptionPanel.OnSelectAll -= POSExceptionPanelOnSelectAll;
                    //exceptionPanel.OnSelectNone -= POSExceptionPanelOnSelectNone;
                    segmentPanel.IsTitle = false;
                }

                if (!_recycleSegment.Contains(segmentPanel))
                {
                    _recycleSegment.Enqueue(segmentPanel);
                }
            }
            segmentContainerPanel.Controls.Clear();

            foreach (TagPanel tagPanel in tagContainerPanel.Controls)
            {
                tagPanel.Tag = null;
                //tagPanel.Cursor = Cursors.Hand;

                if (tagPanel.IsTitle)
                {
                    //tagPanel.OnSelectAll -= POSExceptionPanelOnSelectAll;
                    //tagPanel.OnSelectNone -= POSExceptionPanelOnSelectNone;
                    tagPanel.IsTitle = false;
                }

                if (!_recycleTag.Contains(tagPanel))
                {
                    _recycleTag.Enqueue(tagPanel);
                }
            }
            tagContainerPanel.Controls.Clear();
        }

        private void NameTextBoxTextChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSException.Name = nameTextBox.Text;

            POSExceptionModify();
        }

        private void ManufactureComboBoxSelectedIndexChanged(Object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSException.Manufacture = POS_Exception.ToIndex(manufactureComboBox.SelectedItem.ToString());

            POS_Exception.SetDefaultExceptions(POSException);
            POS_Exception.SetDefaultSegments(POSException);
            POS_Exception.SetDefaultTags(POSException);

            foreach (var pos in PTS.POS.POSServer)
            {
                if (pos.Exception != POSException.Id) continue;
                if (pos.Manufacture == POSException.Manufacture) continue;

                pos.Exception = 0;
                if (pos.ReadyState == ReadyState.Ready)
                    pos.ReadyState = ReadyState.Modify;
            }
            ParsePOSException();

            switch (POSException.Manufacture)
            {
                case "Generic":
                    POSException.IsSupportPOSId = filterPODIdCheckBox.Checked = false;
                    break;

                default:
                    POSException.IsSupportPOSId = filterPODIdCheckBox.Checked = true;
                    break;
            }

            UpdateDisplayPanel();
            POSExceptionModify();
        }

        private void TransactionTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsEditing) return;

            if (transactionTypeComboBox.SelectedItem as String == "Single")
            {
                POSException.TransactionType = 1;
            }
            else
            {
                POSException.TransactionType = 2;
            }

            POSExceptionModify();
        }

        private void FilterPODIdCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSException.IsSupportPOSId = filterPODIdCheckBox.Checked;

            POSExceptionModify();
        }

        private void SeparatorTextBoxTextChanged(object sender, EventArgs e)
        {
            if (!IsEditing) return;

            POSException.Separator = separatorTextBox.Text;

            POSExceptionModify();
        }

        private void POSExceptionModify()
        {
            if (POSException.ReadyState != ReadyState.New)
                POSException.ReadyState = ReadyState.Modify;
        }
    }
}
