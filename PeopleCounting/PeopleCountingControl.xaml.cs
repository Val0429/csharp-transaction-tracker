using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Constant;
using Interface;

namespace PeopleCounting
{
    public partial class PeopleCountingControl
    {
        public event EventHandler<EventArgs<Boolean>> OnEnabledSnapshotButtons;
        public event EventHandler<EventArgs<Boolean>> OnEnabledClearButtons;
        public event EventHandler<EventArgs<Boolean>> OnEnabledRegionControlButtons;
        public event EventHandler<EventArgs<Boolean>> OnEnabledDrawingButton;
        public event EventHandler<EventArgs<Boolean>> OnEnabledPeopleDescriptionLabel;
        public event EventHandler<EventArgs<Boolean>> OnEnabledVerifyButton;
        public event EventHandler<EventArgs<Boolean>> OnEnabledFeatureButton;
        public event EventHandler<EventArgs<System.Drawing.Rectangle>> OnSaveFeatureRetangle;
        public Int32 RectangleLimitCount = 1;
        public List<PeopleCountingRectangle> Rectangles { get; set; }
        private Image _image;
        private readonly Canvas _canvas;
        private Canvas _canvasImage;
        private Canvas _rectangle;
        private Line _line;
        private Line _arrowNE;
        private Line _arrowSW;
        private Line _lineNE;
        private Line _lineSW;
        private TextBlock _textCountNE;
        private TextBlock _textCountSW;
        private Int32 _margin = 5;
        private String _direction;
        private ToolTip _tooltipIn;
        private ToolTip _tooltipOut;
        private ToolTip _toolTipLine;
        private ToolTip _toolTipLinePoint1;
        private ToolTip _toolTipLinePoint2;
        private ToolTip _toolTipRectPoint;
        private static Double _ratio;
        private readonly SolidColorBrush _colorBrushSelected;
        private readonly SolidColorBrush _colorBrushUnselected;
        private Image _redPot;
        private Image _lineLeftPot;
        private Image _lineRightPot;
        private TextBlock _defaultNote;
        public Dictionary<String, String> Localization;

        private Boolean _isLoadRectangles = false;
        private Boolean _isDrawingRectangle;
        private Point _currentRectangleStartPoint;
        private Border _border;
        private const Int32 MiniRectangleSize = 50;
        private Point _currentMousePosition;
        private const Int32 LineMiniLength = 25;
        private const Int32 ArrowWidth = 20;
        private const Int32 ArrowDistanceLimit = 30;
        private Brush _arrowInRedColor;
        private Brush _arrowOutGreenColor;
        private Brush _textInColor;
        private Brush _textOutColor;
        private System.Drawing.Image _rectanglePointImage;
        private System.Drawing.Image _rectangleLinePointImage;
        private Canvas _coverage;
        public Boolean IsVerified;

        public PeopleCountingControl()
        {
            Localization = new Dictionary<String, String>
                               {
                                    {"PeopleCounting_DefaultNote", "Connect fail."},
                                    {"PeopleCounting_PeopleInDescription", "In (Click to switch)"},
                                    {"PeopleCounting_PeopleOutDescription", "Out (Click to switch)"},
                                    {"PeopleCounting_RectangleLineDescription", "Drag to adjust line."},
                                    {"PeopleCounting_RectanglePointDescription", "Drag to adjust rectangle size."},
                                    {"PeopleCounting_RectangleLinePointDescription", "Drag to adjust length of line. "},
                               };
            Localizations.Update(Localization);

            _rectanglePointImage = Constant.Resources.GetResources(Properties.Resources.RedPoint, Properties.Resources.IMGRedPoint);
            _rectangleLinePointImage = Constant.Resources.GetResources(Properties.Resources.GrayPoint, Properties.Resources.IMGGrayPoint);

            Rectangles = new List<PeopleCountingRectangle>();
            _colorBrushSelected = new SolidColorBrush(Colors.Yellow)
                              {
                                  Opacity = 0.1
                              };
            _colorBrushUnselected = new SolidColorBrush(Colors.White)
            {
                Opacity = 0.3
            };

            _textInColor =  new SolidColorBrush(Colors.Red)
            {
                Opacity = 0.5
            };

            _textOutColor = new SolidColorBrush(Colors.Green)
            {
                Opacity = 0.5
            };

            _canvas = new Canvas
            {
                Width = Convert.ToDouble(Properties.Resources.PadWidth),
                Height = Convert.ToDouble(Properties.Resources.PadHeight),
                Background = Brushes.Black
            };

            MouseUp += PeopleCountingControlMouseUp;

            _coverage = new Canvas
            {
                Width = Convert.ToDouble(Properties.Resources.PadWidth),
                Height = Convert.ToDouble(Properties.Resources.PadHeight),
                Background = Brushes.Transparent,
            };

            Canvas.SetTop(_coverage, 0);
            Canvas.SetLeft(_coverage, 0);
            Canvas.SetZIndex(_coverage, 999);
            _defaultNote = new TextBlock
                               {
                                   Text = Localization["PeopleCounting_DefaultNote"],
                                   Foreground = Brushes.White,
                                   FontFamily = new System.Windows.Media.FontFamily("Arial"),
                                   FontSize = 14,
                                   FontStretch = FontStretches.UltraExpanded,
                                   FontStyle = FontStyles.Normal,
                                   FontWeight = FontWeights.Regular,
                                   Padding = new Thickness(2, 2, 2, 2),
                                   TextAlignment = TextAlignment.Center,
                                   TextWrapping = TextWrapping.Wrap,
                                   Width = 200,
                                   Height = 100
                               };

            _defaultNote.Typography.NumeralStyle = FontNumeralStyle.Normal;
            _defaultNote.Typography.SlashedZero = true;

            Canvas.SetTop(_defaultNote,_canvas.Height/2 - _defaultNote.Height/2);
            Canvas.SetLeft(_defaultNote, _canvas.Width / 2 - _defaultNote.Width / 2);
            _canvas.Children.Add(_defaultNote);

            InitializeComponent();
            Background = Brushes.Black;

            var mainWindow = this;
            mainWindow.Content = null;
            mainWindow.Content = _canvas;

            _tooltipIn = new ToolTip
                             {
                                 Content = Localization["PeopleCounting_PeopleInDescription"]
                             };

            _tooltipOut = new ToolTip
                            {
                                Content = Localization["PeopleCounting_PeopleOutDescription"]
                            };

            _toolTipLine = new ToolTip
                               {
                                   Content = Localization["PeopleCounting_RectangleLineDescription"]
                               };

            _toolTipLinePoint1 = new ToolTip
                                {
                                    Content = Localization["PeopleCounting_RectangleLinePointDescription"]
                                };

            _toolTipLinePoint2 = new ToolTip
                                {
                                    Content = Localization["PeopleCounting_RectangleLinePointDescription"]
                                };

            _toolTipRectPoint = new ToolTip
                                    {
                                        Content = Localization["PeopleCounting_RectanglePointDescription"]
                                    };

            _arrowInRedColor = new SolidColorBrush(Color.FromArgb(255,238,60,50)); //Red
            _arrowOutGreenColor = new SolidColorBrush(Color.FromArgb(255,109, 162, 32)); //Green

        }

        private void PeopleCountingControlMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isDrawingRectangle) return;
            _isDrawingRectangle = false;
            RemoveRegion();
            if(_drawingMode == "MakeFeature")
            {
                StartDrawingFeatureRectangle();
            }
            else
            {
                StartDrawingRectangle();
            }
            
            e.Handled = true;
        }

        public void CreatePhotoByImage(System.Drawing.Image image)
        {
            _canvas.Children.Clear();

            _image = new Image
            {
                Name = "BasicPhoto"
            };

            var source = Converter.ImageToBitmapSource(image);

            if(source != null)
            {
                _image.Source = source;
                var newSize = CalculateImageSize(image.Width, image.Height);
                _canvasImage = new Canvas
                {
                    Width = newSize.Width,
                    Height = newSize.Height,
                    AllowDrop = true
                };
                if (_canvasImage.Children.Contains(_image))
                {
                    _canvasImage.Children.Remove(_image);
                }
                _canvasImage.DragOver -= CanvasImageDragOver;
                _canvasImage.DragOver += CanvasImageDragOver;

                _image.Width = newSize.Width;
                _image.Height = newSize.Height;
                Canvas.SetTop(_image, 0);
                Canvas.SetLeft(_image, 0);
               
                _canvasImage.Children.Add(_image);

                Canvas.SetLeft(_canvasImage, _canvas.Width / 2 - (_canvasImage.Width / 2));
                Canvas.SetTop(_canvasImage, _canvas.Height / 2 - (_canvasImage.Height / 2));
                _canvas.Children.Add(_canvasImage);
                _rectangle = null;

                _redPot = CreateImageNode("RightBottom", _rectanglePointImage, 10, 10,0, 0);
                _redPot.ToolTip = _toolTipRectPoint;
                Canvas.SetZIndex(_redPot, 100);
                _lineLeftPot = CreateImageNode("LeftPoint", _rectangleLinePointImage, 10, 10, 0, 0);
                _lineLeftPot.ToolTip = _toolTipLinePoint1;
                _lineRightPot = CreateImageNode("RightPoint", _rectangleLinePointImage, 10, 10, 0, 0);
                _lineRightPot.ToolTip = _toolTipLinePoint2;

                if (OnEnabledSnapshotButtons != null)
                {
                    OnEnabledSnapshotButtons(this,new EventArgs<Boolean>(IsVerified ? false : true));
                }

                if (OnEnabledFeatureButton != null)
                {
                    OnEnabledFeatureButton(this, new EventArgs<Boolean>(true));
                }
            }
        }

        public void RefreshByImage(System.Drawing.Image image)
        {
            if (image != null)
            {
                _image = new Image
                {
                    Name = "BasicPhoto"
                };

                _image.Source = Converter.ImageToBitmapSource(image);
                var newSize = CalculateImageSize(image.Width, image.Height);

                if (_canvasImage == null)
                {
                    _canvasImage = new Canvas
                    {
                        Width = newSize.Width,
                        Height = newSize.Height,
                        AllowDrop = true
                    };
                    _canvasImage.DragOver -= CanvasImageDragOver;
                    _canvasImage.DragOver += CanvasImageDragOver;

                    _redPot = CreateImageNode("RightBottom", _rectanglePointImage, 10, 10, 0, 0);
                    _redPot.ToolTip = _toolTipRectPoint;
                    Canvas.SetZIndex(_redPot, 100);
                    _lineLeftPot = CreateImageNode("LeftPoint", _rectangleLinePointImage, 10, 10, 0, 0);
                    _lineLeftPot.ToolTip = _toolTipLinePoint1;
                    _lineRightPot = CreateImageNode("RightPoint", _rectangleLinePointImage, 10, 10, 0, 0);
                    _lineRightPot.ToolTip = _toolTipLinePoint2;
                }
                else
                {
                    _canvasImage.Width = newSize.Width;
                    _canvasImage.Height = newSize.Height;
                }

                if (!_canvas.Children.Contains(_canvasImage))
                    _canvas.Children.Add(_canvasImage);

                if (!_canvasImage.Children.Contains(_image))
                    _canvasImage.Children.Add(_image);

                _image.Width = newSize.Width;
                _image.Height = newSize.Height;
                Canvas.SetTop(_image, 0);
                Canvas.SetLeft(_image, 0);
                Canvas.SetLeft(_canvasImage, _canvas.Width / 2 - (_canvasImage.Width / 2));
                Canvas.SetTop(_canvasImage, _canvas.Height / 2 - (_canvasImage.Height / 2));

                if(_drawingMode == "ROI")
                    DrawSettingByPeopleCountingList();
            }
        }

        public void ClearRegions()
        {
            var list = _canvasImage.Children.OfType<Canvas>().ToList();
            foreach (Canvas canvas in list)
            {
                _canvasImage.Children.Remove(canvas);
            }

            if (_canvasImage.Children.Contains(_redPot))
            {
                _canvasImage.Children.Remove(_redPot);
            }

            if (OnEnabledClearButtons != null)
            {
                OnEnabledClearButtons(this, new EventArgs<Boolean>(false));
            }

            if (OnEnabledRegionControlButtons != null)
            {
                OnEnabledRegionControlButtons(this, new EventArgs<Boolean>(false));
            }

            if (OnEnabledPeopleDescriptionLabel != null)
            {
                OnEnabledPeopleDescriptionLabel(this, new EventArgs<Boolean>(false));
            }
            if(_drawingMode == "ROI") CreateRectangles();
            CheckRectangleCountLimit();

        }
         
        public void RemoveRegion()
        {
            if(_rectangle != null)
            {
                _rectangle.Children.Clear();
                if(_canvasImage.Children.Contains(_rectangle))
                {
                    _canvasImage.Children.Remove(_rectangle);
                    _rectangle = null;
                    MakeAllRegionDisabled();

                    var list = _canvasImage.Children.OfType<Canvas>().ToList();
                    if (list.Count <2 && OnEnabledClearButtons != null)
                    {
                        OnEnabledClearButtons(this, new EventArgs<Boolean>(false));
                    }

                    if (OnEnabledPeopleDescriptionLabel != null)
                    {
                        var enabled = list.Count > 0 ? true : false;
                        OnEnabledPeopleDescriptionLabel(this, new EventArgs<Boolean>(enabled));
                    }

                    if (_drawingMode == "ROI") CreateRectangles();
                    CheckRectangleCountLimit();
                }
            }
        }

        public void DeleteSetting()
        {
            _canvas.Children.Clear();
            _canvas.Children.Add(_defaultNote);

            if (OnEnabledSnapshotButtons != null)
            {
                OnEnabledSnapshotButtons(this, new EventArgs<Boolean>(false));
            }

            if (OnEnabledClearButtons != null)
            {
                OnEnabledClearButtons(this, new EventArgs<Boolean>(false));
            }

            if (OnEnabledRegionControlButtons != null)
            {
                OnEnabledRegionControlButtons(this, new EventArgs<Boolean>(false));
            }

            if (OnEnabledPeopleDescriptionLabel != null)
            {
                OnEnabledPeopleDescriptionLabel(this, new EventArgs<Boolean>(false));
            }

            if(OnEnabledFeatureButton != null)
            {
                OnEnabledFeatureButton(this, new EventArgs<Boolean>(false));
            }
        }

        public void SwitchVerifyMode()
        {
            if(IsVerified)
            {
                _canvas.Children.Add(_coverage);
            }
            else
            {
                _canvas.Children.Remove(_coverage);
            }

            DrawSettingByPeopleCountingList();
        }

        public void CreateRectangles()
        {
            if (_isLoadRectangles || _drawingMode != "ROI") return;
            Rectangles.Clear();
            foreach (var child  in _canvasImage.Children  )
            {
                var cvs = child as Canvas;
                if(cvs != null)
                {
                    var rect = new PeopleCountingRectangle();
                    rect.Rectangle = new System.Drawing.Rectangle((int)((int)Canvas.GetLeft(cvs) / _ratio), (int)((int)Canvas.GetTop(cvs) / _ratio), (int)((int)cvs.Width / _ratio), (int)((int)cvs.Height / _ratio));
                    var direction = String.Empty;
                    var list = cvs.Children.OfType<Line>().Where(item=>item.Name == "Line").ToList();
                    if (list.Count > 0)
                    {
                       direction =  list[0].DataContext as String;
                    }

                    if (String.IsNullOrEmpty(direction))
                    {
                        return;
                    }

                    foreach (var node in cvs.Children)
                    {
                        var line = node as Line;
                        if(line != null)
                        {
                            var lineType = line.DataContext as String;
                            switch (line.Name)
                            {
                                case "Line":
                                    rect.StartPoint = new System.Drawing.Point((int) ((int)line.X1 / _ratio), (int) ((int)line.Y1 / _ratio));
                                    rect.EndPoint = new System.Drawing.Point((int) ((int)line.X2 / _ratio), (int) ((int)line.Y2 / _ratio));
                                    break;
                                case "LineNE":

                                    if (lineType == "In")
                                    {
                                        rect.In = direction == "Vertical" ? Direction.DownToUp : Direction.LeftToRight;
                                    }
                                    else
                                    {
                                        rect.Out = direction == "Vertical" ? Direction.DownToUp : Direction.LeftToRight;
                                    }
                                    break;
                                case "LineSW":

                                    if (lineType == "In")
                                    {
                                        rect.In = direction == "Vertical" ? Direction.UpToDown : Direction.RightToLeft;
                                    }
                                    else
                                    {
                                        rect.Out = direction == "Vertical" ? Direction.UpToDown : Direction.RightToLeft;
                                    }
                                    break;
                            }
                        }
                    }
                    Rectangles.Add(rect);
                }
            }

            if(OnEnabledVerifyButton != null)
            {
                OnEnabledVerifyButton(this, new EventArgs<Boolean>(Rectangles.Count > 0 ? true : false));
            }
        }

        public void SaveDrawingFeatureRectangle()
        {
            foreach (var child  in _canvasImage.Children  )
            {
                var cvs = child as Canvas;
                if (cvs != null)
                {
                    var rect = new System.Drawing.Rectangle((int)((int)Canvas.GetLeft(cvs) / _ratio), (int)((int)Canvas.GetTop(cvs) / _ratio), (int)((int)cvs.Width / _ratio), (int)((int)cvs.Height / _ratio));
                    if (OnSaveFeatureRetangle != null) OnSaveFeatureRetangle(this, new EventArgs<System.Drawing.Rectangle>(rect));
                }
            }
        }

        public void DrawSettingByPeopleCountingList()
        {
            _isLoadRectangles = true;

            var check = false;
            if(_canvasImage == null)
            {
                check = true;
            }
            else
            {
                if (_canvas.Children.Contains(_canvasImage) == false)
                {
                    check = true;
                }
            }

            if(check)
            {
                if (OnEnabledSnapshotButtons != null)
                {
                    OnEnabledSnapshotButtons(this, new EventArgs<Boolean>(true));
                }

                if(OnEnabledDrawingButton != null)
                {
                    OnEnabledDrawingButton(this, new EventArgs<Boolean>(false));
                }
                
                _isLoadRectangles = false;
                return;
            }

            var children = _canvasImage.Children.OfType<Canvas>().ToList();

            foreach (var rect in children)
            {
                _canvasImage.Children.Remove(rect);
            }
            
            Canvas selectItem = null;
            foreach (PeopleCountingRectangle rect in Rectangles)
            {
                _rectangle = new Canvas
                {
                    Name = "Rectangle",
                    Background = _colorBrushUnselected,
                    Width = rect.Rectangle.Width*_ratio,
                    Height = rect.Rectangle.Height * _ratio,
                    AllowDrop = true,
                    DataContext = rect
                };
                _rectangle.DragOver += RectangleDragOver;
                Canvas.SetTop(_rectangle, rect.Rectangle.Y * _ratio);
                Canvas.SetLeft(_rectangle, rect.Rectangle.X * _ratio);
                if (_currentPeopleCountingRectangle == rect)
                {
                    selectItem = _rectangle;
                }
                _border = new Border
                {
                    Width = _rectangle.Width,
                    Height = _rectangle.Height,
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.WhiteSmoke
                };

                Canvas.SetTop(_border, 0);
                Canvas.SetLeft(_border, 0);
                _rectangle.Children.Add(_border);

               _line = new Line
                {
                    Name = "Line",
                    StrokeThickness = 4,
                    X1 = rect.StartPoint.X * _ratio,
                    Y1 = rect.StartPoint.Y * _ratio,
                    X2 = rect.EndPoint.X * _ratio,
                    Y2 = rect.EndPoint.Y * _ratio,
                    Stroke = Brushes.DimGray,
                    Cursor = Cursors.ScrollAll,
                    ToolTip = _toolTipLine,
                    StrokeEndLineCap = PenLineCap.Round,
                    StrokeStartLineCap = PenLineCap.Round,
                    DataContext =  rect.In == Direction.LeftToRight || rect.In == Direction.RightToLeft ? "Horizontal" : "Vertical"
                };
            
                _arrowNE = new Line
                {
                    Name = "ArrowNE",
                    StrokeEndLineCap = PenLineCap.Triangle,
                    Stroke = (rect.In == Direction.DownToUp || rect.In == Direction.LeftToRight) ? _arrowInRedColor : _arrowOutGreenColor,
                    ToolTip = _tooltipIn,
                    Cursor = Cursors.Hand
                };
                _arrowNE.DataContext = _arrowNE.Stroke == _arrowInRedColor ? "In" : "Out";
                _rectangle.Children.Add(_arrowNE);

                _lineNE = new Line
                {
                    Name = "LineNE",
                    Stroke = (rect.In == Direction.DownToUp || rect.In == Direction.LeftToRight) ? _arrowInRedColor : _arrowOutGreenColor,
                    ToolTip = _tooltipIn,
                    Cursor = Cursors.Hand,
                };
                _lineNE.DataContext = _lineNE.Stroke == _arrowInRedColor ? "In" : "Out";
                _rectangle.Children.Add(_lineNE);

                _arrowSW = new Line
                {
                    Name = "ArrowSW",
                    StrokeEndLineCap = PenLineCap.Triangle,
                    Stroke = rect.In == Direction.UpToDown || rect.In == Direction.RightToLeft ? _arrowInRedColor : _arrowOutGreenColor,
                    ToolTip = _tooltipOut,
                    Cursor = Cursors.Hand,
                };
                _arrowSW.DataContext = _arrowSW.Stroke == _arrowInRedColor ? "In" : "Out";
                _rectangle.Children.Add(_arrowSW);

                _lineSW = new Line
                {
                    Name = "LineSW",
                    Stroke = rect.In == Direction.UpToDown || rect.In == Direction.RightToLeft ? _arrowInRedColor : _arrowOutGreenColor,
                    ToolTip = _tooltipOut,
                    Cursor = Cursors.Hand,
                };
                _lineSW.DataContext = _lineSW.Stroke == _arrowInRedColor ? "In" : "Out";
                _rectangle.Children.Add(_lineSW);
                _rectangle.Children.Add(_line);

                _textCountNE = new TextBlock
                {
                    Text = _arrowNE.Stroke == _arrowInRedColor ? rect.PeopleCountingIn.ToString() : rect.PeopleCountingOut.ToString(),
                    DataContext =  _lineNE.Stroke == _arrowInRedColor ? "In" : "Out",
                    Foreground =Brushes.Black,
                    Background = (rect.In == Direction.DownToUp || rect.In == Direction.LeftToRight) ? _textInColor : _textOutColor, 
                    Width = 40,
                    Height = 20,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    FontFamily = new System.Windows.Media.FontFamily("Arial"),
                    FontSize = 15,
                    FontStretch = FontStretches.UltraExpanded,
                    FontStyle = FontStyles.Normal,
                    FontWeight = FontWeights.Regular,
                    Padding = new Thickness(2, 2, 2, 2),
                };
                _textCountNE.Visibility = IsVerified ? Visibility.Visible : Visibility.Hidden;
                _rectangle.Children.Add(_textCountNE);

                _textCountSW = new TextBlock
                {
                    Text =
                        _arrowSW.Stroke == _arrowInRedColor
                            ? rect.PeopleCountingIn.ToString()
                            : rect.PeopleCountingOut.ToString(),
                    DataContext = _arrowSW.Stroke == _arrowInRedColor ? "In" : "Out",
                    Foreground = Brushes.Black,
                    Background =rect.In == Direction.UpToDown || rect.In == Direction.RightToLeft ? _textInColor  : _textOutColor,
                    Width = 40,
                    Height = 20,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    FontFamily = new System.Windows.Media.FontFamily("Arial"),
                    FontSize = 15,
                    FontStretch = FontStretches.UltraExpanded,
                    FontStyle = FontStyles.Normal,
                    FontWeight = FontWeights.Regular,
                    Padding = new Thickness(2, 2, 2, 2),
                };
                //node.LineHeight = Double.NaN;
                _textCountSW.Visibility = IsVerified ? Visibility.Visible : Visibility.Hidden;
                _rectangle.Children.Add(_textCountSW);

                AdjustArrowLineByDirection(_line.DataContext as String);

                _canvasImage.Children.Add(_rectangle);
            }

            MakeAllRegionDisabled();
            _isLoadRectangles = false;
            CheckRectangleCountLimit();

            if (OnEnabledSnapshotButtons != null)
            {
                OnEnabledSnapshotButtons(this, new EventArgs<Boolean>(IsVerified ? false: true));
            }

            if (OnEnabledClearButtons != null)
            {
                var enabled = Rectangles.Count > 1 ? true : false;
                if (IsVerified) enabled = false;
                OnEnabledClearButtons(this, new EventArgs<Boolean>(enabled));
            }

            if (selectItem != null)
            {
                RectangleMouseDownForFocus(selectItem, null);
            }
            else
            {
                var list = _canvasImage.Children.OfType<Canvas>().ToList();
                if (list.Count > 0)
                {
                    RectangleMouseDownForFocus(list[0], null);
                }
            }

            if (OnEnabledRegionControlButtons != null)
            {
                var enabled = Rectangles.Count > 0 ? true : false;
                if (IsVerified) enabled = false;
                OnEnabledRegionControlButtons(this, new EventArgs<Boolean>(enabled));
            }

            if (OnEnabledVerifyButton != null)
            {
                OnEnabledVerifyButton(this, new EventArgs<Boolean>(Rectangles.Count > 0 ? true : false));
            }

            if (OnEnabledDrawingButton != null)
            {
                var enabled = Rectangles.Count >= RectangleLimitCount ? false : true;
                if (IsVerified) enabled = false;
                OnEnabledDrawingButton(this, new EventArgs<Boolean>(enabled));
            }

            if (OnEnabledPeopleDescriptionLabel != null)
            {
                var enabled = Rectangles.Count >0 ? true : false;
                OnEnabledPeopleDescriptionLabel(this, new EventArgs<Boolean>(enabled));
            }

            if(IsVerified)
            {
                if(!_canvas.Children.Contains(_coverage))
                {
                    _canvas.Children.Add(_coverage);
                }
            }
        }

        private String _drawingMode = "ROI";

        public void StartDrawingRectangle()
        {
            if (_canvasImage == null) return;

            _drawingMode = "ROI";

            if (_isDrawingRectangle)
            {
                _canvasImage.Cursor = Cursors.Arrow;
                _isDrawingRectangle = false;
                _canvasImage.MouseDown -= CanvasImageMouseDownForStartDrawingRectangle;
                return;
            }
            _canvasImage.Cursor = Cursors.Cross;
            _isDrawingRectangle = true;
            _canvasImage.MouseDown -= CanvasImageMouseDownForStartDrawingRectangle;
            _canvasImage.MouseDown += CanvasImageMouseDownForStartDrawingRectangle;
            MakeAllRegionDisabled();
        }

        public void StartDrawingFeatureRectangle()
        {
            if (_canvasImage == null) return;
            _drawingMode = "MakeFeature";

            var list = _canvasImage.Children.OfType<Canvas>().ToList();
            foreach (Canvas canvas in list)
            {
                _canvasImage.Children.Remove(canvas);
            }

            if (_canvasImage.Children.Contains(_redPot))
            {
                _canvasImage.Children.Remove(_redPot);
            }

            if (_rectangle != null)
            {
                if (_canvasImage.Children.Contains(_rectangle))
                {
                    _canvasImage.Children.Remove(_rectangle);
                    _rectangle = null;
                }
            }

            RemoveRegion();
            _canvasImage.Cursor = Cursors.Cross;
            _isDrawingRectangle = true;
            _canvasImage.MouseDown -= CanvasImageMouseDownForStartDrawingRectangle;
            _canvasImage.MouseDown += CanvasImageMouseDownForStartDrawingRectangle;
        }

        public void StopDrawingFeatureRectangle()
        {
            _isDrawingRectangle = false;
            DrawSettingByPeopleCountingList();
            _drawingMode = "ROI";
            _canvasImage.Cursor = Cursors.Arrow;
            _canvasImage.MouseDown -= CanvasImageMouseDownForStartDrawingRectangle;
        }

        private void CanvasImageMouseDownForStartDrawingRectangle(object sender, MouseButtonEventArgs e)
        {
            if (_isDrawingRectangle)
            {
                _currentRectangleStartPoint = e.GetPosition(_canvasImage);
                _canvasImage.MouseDown -= CanvasImageMouseDownForStartDrawingRectangle;
                _rectangle = new Canvas
                                 {
                                     Name = "Rectangle",
                                     Background = _colorBrushSelected,
                                     Width = 0,
                                     Height = 0,
                                     AllowDrop = true
                                 };
                _rectangle.DragOver -= RectangleDragOver;
                _rectangle.DragOver += RectangleDragOver;
                _rectangle.Drop -= RectangleDrop;
                _rectangle.Drop += RectangleDrop;

                Canvas.SetTop(_rectangle, _currentRectangleStartPoint.Y);
                Canvas.SetLeft(_rectangle, _currentRectangleStartPoint.X);

                _border = new Border
                {
                    Width = _rectangle.Width,
                    Height = _rectangle.Height,
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.Orange
                };

                Canvas.SetTop(_border, 0);
                Canvas.SetLeft(_border,0);
                _rectangle.Children.Add(_border);
                _canvasImage.Children.Add(_rectangle);

                _canvasImage.MouseMove -= CanvasImageMouseMove;
                _canvasImage.MouseMove += CanvasImageMouseMove;

                _canvasImage.MouseUp -= CanvasImageMouseDownForCompleteDrawingRectangle;
                _canvasImage.MouseUp += CanvasImageMouseDownForCompleteDrawingRectangle;

            }
        }
        
        private void CanvasImageMouseDownForCompleteDrawingRectangle(object sender, MouseButtonEventArgs e)
        {
            if (_rectangle == null) return;
            if (_isDrawingRectangle)
            {
                _isDrawingRectangle = false;
                _canvasImage.Cursor = Cursors.Arrow;

                AdjustRectangleByCurrentPoint(e.GetPosition(_canvasImage));

                if (_rectangle.Width < MiniRectangleSize) _rectangle.Width = _border.Width = MiniRectangleSize;
                if (_rectangle.Height < MiniRectangleSize) _rectangle.Height = _border.Height = MiniRectangleSize;

                DrawRedPot();

                _line = new Line
                {
                    Name = "Line",
                    StrokeThickness = 4,
                    X1 = _drawingMode == "ROI" ? _margin : 0,
                    Y1 = _drawingMode == "ROI" ? _rectangle.Height / 2 : 0,
                    X2 = _drawingMode == "ROI" ? _rectangle.Width - _margin : 0,
                    Y2 = _drawingMode == "ROI" ? _rectangle.Height / 2: 0,
                    Stroke = Brushes.DarkOrange,
                    Cursor = Cursors.ScrollAll,
                    ToolTip = _toolTipLine,
                    StrokeEndLineCap = PenLineCap.Round,
                    StrokeStartLineCap = PenLineCap.Round,
                    Opacity = 1,
                    DataContext = "Vertical",
                    Visibility = _drawingMode == "ROI" ?  Visibility.Visible : Visibility.Hidden
                };
                _line.MouseDown -= LineMouseDown;
                _line.MouseDown += LineMouseDown;
                _direction = _line.DataContext as String;

                Canvas.SetTop(_lineLeftPot, _drawingMode == "ROI" ? _line.Y1 - 5: 0);
                Canvas.SetLeft(_lineLeftPot, _drawingMode == "ROI" ? _line.X1 - 5: 0);
                _lineLeftPot.Visibility = _drawingMode == "ROI" ? Visibility.Visible : Visibility.Hidden;
                _lineLeftPot.Cursor = Cursors.ScrollWE;
                _lineLeftPot.MouseDown += LinePointMouseDown;

                Canvas.SetTop(_lineRightPot, _drawingMode == "ROI" ? _line.Y2 - 5: 0);
                Canvas.SetLeft(_lineRightPot,_drawingMode == "ROI" ?  _line.X2 - 5: 0);
                _lineRightPot.Visibility = _drawingMode == "ROI" ? Visibility.Visible : Visibility.Hidden;
                _lineRightPot.Cursor = Cursors.ScrollWE;
                _lineRightPot.MouseDown += LinePointMouseDown;

                _arrowNE = new Line
                {
                    Name = "ArrowNE",
                    StrokeEndLineCap = PenLineCap.Triangle,
                    Stroke = _arrowInRedColor,
                    ToolTip = _tooltipIn,
                    Cursor = Cursors.Hand,
                    DataContext = "In",
                    Visibility = _drawingMode == "ROI" ? Visibility.Visible : Visibility.Hidden
                };
                _arrowNE.MouseDown -= ArrowMouseDown;
                _arrowNE.MouseDown += ArrowMouseDown;
                _rectangle.Children.Add(_arrowNE);

                _lineNE = new Line
                {
                    Name = "LineNE",
                    Stroke = _arrowInRedColor,
                    ToolTip = _tooltipIn,
                    Cursor = Cursors.Hand,
                    DataContext = "In",
                    Visibility = _drawingMode == "ROI" ? Visibility.Visible : Visibility.Hidden
                };
                _lineNE.MouseDown -= ArrowMouseDown;
                _lineNE.MouseDown += ArrowMouseDown;
                _rectangle.Children.Add(_lineNE);

                _arrowSW = new Line
                {
                    Name = "ArrowSW",
                    StrokeEndLineCap = PenLineCap.Triangle,
                    Stroke = _arrowOutGreenColor,
                    ToolTip = _tooltipOut,
                    Cursor = Cursors.Hand,
                    DataContext = "Out",
                    Visibility = _drawingMode == "ROI" ? Visibility.Visible : Visibility.Hidden
                };
                _arrowSW.MouseDown -= ArrowMouseDown;
                _arrowSW.MouseDown += ArrowMouseDown;
                _rectangle.Children.Add(_arrowSW);

                _lineSW = new Line
                {
                    Name = "LineSW",
                    Stroke = _arrowOutGreenColor,
                    ToolTip = _tooltipOut,
                    Cursor = Cursors.Hand,
                    DataContext = "Out",
                    Visibility = _drawingMode == "ROI" ? Visibility.Visible : Visibility.Hidden
                };
                _lineSW.MouseDown -= ArrowMouseDown;
                _lineSW.MouseDown += ArrowMouseDown;
                _rectangle.Children.Add(_lineSW);

                _rectangle.Children.Add(_line);
                _rectangle.Children.Add(_lineLeftPot);
                _rectangle.Children.Add(_lineRightPot);

                _textCountNE = new TextBlock
                {
                    Text = "0",
                    DataContext = "In",
                    Foreground = Brushes.Red,
                    Width = 20,
                    Height = 20,
                    Visibility = Visibility.Hidden
                };
                _rectangle.Children.Add(_textCountNE);

                _textCountSW = new TextBlock
                {
                    Text = "0",
                    DataContext = "Out",
                    Foreground = Brushes.Green,
                    Width = 20,
                    Height = 20,
                    Visibility = Visibility.Hidden
                };
                _rectangle.Children.Add(_textCountSW);

                AdjustArrowLineByDirection(_direction);
                

                _rectangle.Cursor = Cursors.SizeAll;
                _rectangle.MouseDown += RectangleMouseDown;

                _canvasImage.MouseMove -= CanvasImageMouseMove;
                _canvasImage.MouseDown -= CanvasImageMouseDownForCompleteDrawingRectangle;

                if (_drawingMode == "ROI") CreateRectangles();
                CheckRectangleCountLimit();

                if (_drawingMode != "ROI") return;
                if (Rectangles.Count>1 && OnEnabledClearButtons != null)
                {
                    OnEnabledClearButtons(this,new EventArgs<Boolean>(true));
                }

                if (OnEnabledRegionControlButtons != null)
                {
                    OnEnabledRegionControlButtons(this, new EventArgs<Boolean>(true));
                }

                if (OnEnabledPeopleDescriptionLabel != null)
                {
                    OnEnabledPeopleDescriptionLabel(this, new EventArgs<Boolean>(true));
                }

            }
        }

        private void CheckRectangleCountLimit()
        {
            var result = true;
            if (Rectangles.Count >= RectangleLimitCount)
            {
                result = false;
            }

            if (OnEnabledDrawingButton != null)
            {
                OnEnabledDrawingButton(this, new EventArgs<Boolean>(result));
            }
        }

        private void DrawRedPot()
        {
            if(_rectangle == null || _redPot==null) return;
            var recX = Canvas.GetLeft(_rectangle);
            var recY = Canvas.GetTop(_rectangle);

            if (_canvasImage.Children.Contains(_redPot))
            {
                _canvasImage.Children.Remove(_redPot);
            }
            Canvas.SetTop(_redPot, recY + _rectangle.Height - (_redPot.Height / 2));
            Canvas.SetLeft(_redPot, recX + _rectangle.Width - (_redPot.Width / 2));
            _redPot.MouseLeftButtonDown -= DragNode;
            _redPot.MouseLeftButtonDown += DragNode;

            _redPot.Cursor = Cursors.SizeNWSE;
            _canvasImage.Children.Add(_redPot);
        }

        public void MakeSelectedRegionDisabled()
        {
            if (_rectangle != null)
            {
                _rectangle.Background = _colorBrushUnselected;
                _rectangle.MouseDown -= RectangleMouseDown;
                _rectangle.MouseDown += RectangleMouseDownForFocus;
                _rectangle.Cursor = Cursors.Hand;

                if (_arrowNE != null) _arrowNE.MouseDown -= ArrowMouseDown;
                if (_lineNE != null) _lineNE.MouseDown -= ArrowMouseDown;
                if (_arrowSW != null) _arrowSW.MouseDown -= ArrowMouseDown;
                if (_lineSW != null) _lineSW.MouseDown -= ArrowMouseDown;
                if (_line != null) _line.MouseDown -= LineMouseDown;
                if (_lineLeftPot != null) _lineLeftPot.MouseDown -= LinePointMouseDown;
                if (_lineRightPot != null) _lineRightPot.MouseDown -= LinePointMouseDown;

                if (_rectangle.Children.Contains(_lineLeftPot))
                {
                    _rectangle.Children.Remove(_lineLeftPot);
                }

                if (_rectangle.Children.Contains(_lineRightPot))
                {
                    _rectangle.Children.Remove(_lineRightPot);
                }

                if (_line != null)
                {
                    _line.Stroke = Brushes.DimGray;
                    _line.Cursor = Cursors.Hand;
                }

                if(_border != null)
                {
                    _border.BorderBrush = Brushes.WhiteSmoke;
                }

                Canvas.SetZIndex(_rectangle, 0);
            }
            _rectangle = null;
            _line = null;
            _currentPeopleCountingRectangle = null;

            if (OnEnabledRegionControlButtons != null)
            {
                OnEnabledRegionControlButtons(this, new EventArgs<Boolean>(false));
            }
        }

        public void MakeAllRegionDisabled()
        {
            if (_canvasImage == null) return;
            MakeSelectedRegionDisabled();

            if (_canvasImage.Children.Contains(_redPot))
            {
                _canvasImage.Children.Remove(_redPot);
            }

            foreach (var child in _canvasImage.Children)
            {
                var cvs = child as Canvas;
                if(cvs != null)
                {
                    cvs.Background = _colorBrushUnselected;
                    cvs.MouseDown -= RectangleMouseDown;
                    cvs.MouseDown += RectangleMouseDownForFocus;
                    cvs.Cursor = Cursors.Hand;
                    Canvas.SetZIndex(cvs, 0);
                }
            }
        }

        private PeopleCountingRectangle _currentPeopleCountingRectangle;

        private void RectangleMouseDownForFocus(object sender, MouseButtonEventArgs e)
        {
            if (_isDrawingRectangle) return;
            var cvs = sender as Canvas;

            if(cvs != null)
            {
                if (cvs == _rectangle) return;
                Canvas.SetZIndex(cvs, 99);
                MakeSelectedRegionDisabled();

                cvs.Background = _colorBrushSelected;
                cvs.MouseDown -= RectangleMouseDownForFocus;
                cvs.MouseDown -= RectangleMouseDown;
                cvs.MouseDown += RectangleMouseDown;
                cvs.Cursor = Cursors.SizeAll;
                _rectangle = cvs;
                _currentPeopleCountingRectangle = cvs.DataContext as PeopleCountingRectangle;
                DrawRedPot();
                
                foreach (var child in cvs.Children)
                {
                    var line = child as Line;
                    if(line != null)
                    {
                        switch (line.Name)
                        {
                            case "ArrowNE":
                                _arrowNE = line;
                                break;
                            case "LineNE":
                                _lineNE = line;
                                break;
                            case "ArrowSW":
                                _arrowSW = line;
                                break;
                            case "LineSW":
                                _lineSW = line;
                                break;
                            case "Line":
                                _line = line;
                                break;
                        }
                    }
                    else
                    {
                        var point = child as System.Windows.Controls.Image;
                        if(point != null)
                        {
                            switch (point.Name)
                            {
                                case "LeftPoint":
                                    _lineLeftPot = point;
                                    break;
                                case "RightPoint":
                                    _lineRightPot = point;
                                    break;
                            }
                        }
                        else
                        {
                            var border = child as Border;
                            if(border != null)
                            {
                                _border = border;
                            }
                        }
                    }
                }
                _direction = _line.DataContext as String;
                _line.Stroke = Brushes.DarkOrange;
                _line.Cursor = Cursors.ScrollAll;
                _border.BorderBrush = Brushes.DarkOrange;

                if (_arrowNE != null) _arrowNE.MouseDown += ArrowMouseDown;
                if (_lineNE != null) _lineNE.MouseDown += ArrowMouseDown;
                if (_arrowSW != null) _arrowSW.MouseDown += ArrowMouseDown;
                if (_lineSW != null) _lineSW.MouseDown += ArrowMouseDown;
                if (_line != null) _line.MouseDown += LineMouseDown;
                if (_lineLeftPot != null) _lineLeftPot.MouseDown += LinePointMouseDown;
                if (_lineRightPot != null) _lineRightPot.MouseDown += LinePointMouseDown;

                _lineLeftPot = CreateImageNode("LeftPoint", Properties.Resources.GrayPoint, 10, 10, 0, 0);
                _lineLeftPot.ToolTip = _toolTipLinePoint1;
                _lineLeftPot.MouseDown += LinePointMouseDown;
                _rectangle.Children.Add(_lineLeftPot);

                _lineRightPot = CreateImageNode("RightPoint", Properties.Resources.GrayPoint, 10, 10, 0, 0);
                _lineRightPot.ToolTip = _toolTipLinePoint2;
                _lineRightPot.MouseDown += LinePointMouseDown;
                _rectangle.Children.Add(_lineRightPot);

                _lineLeftPot.Cursor = _lineRightPot.Cursor = _direction == "Horizontal" ? Cursors.ScrollNS : Cursors.ScrollWE;
                AdjustLinePointsPosition();

                if (OnEnabledRegionControlButtons != null)
                {
                    OnEnabledRegionControlButtons(this,new EventArgs<Boolean>(IsVerified ? false :  true));
                }
            }
        }

        private void ArrowMouseDown(object sender, MouseButtonEventArgs e)
        {
            SwitchArrowAttribute();
        }

        public void SwitchArrowAttribute()
        {
            if (_canvasImage == null || _rectangle == null) return;

            foreach (var child in _rectangle.Children)
            {
                var line = child as Line;
                if (line != null)
                {
                    if (line.Name == "Line") continue;
                    var type = line.DataContext as String;
                    if(type == null) continue;

                    line.DataContext = type == "In" ? "Out" : "In";
                    line.Stroke = type != "In" ? _arrowInRedColor : _arrowOutGreenColor;
                    line.ToolTip = type != "In" ? _tooltipIn : _tooltipOut;

                    CreateRectangles();
                    continue;
                }

                var text = child as TextBlock;
                if(text != null)
                {
                    var type = text.DataContext as String;
                    if(type == null) continue;
                    text.DataContext = type == "In" ? "Out" : "In";
                    text.Background = type != "In" ? _textInColor : _textOutColor;
                    //text.Text =  text.DataContext as String == "In" ? current 
                    CreateRectangles();
                }
            }

            //DrawSettingByPeopleCountingList();
        }

        private void LineMouseDown(object sender, MouseButtonEventArgs e)
        {
            var line = e.Source as Line;
            if (line != null)
            {
                _currentMousePosition = e.GetPosition(_rectangle);
                var data = new DataObject(typeof(Line), line);
                DragDrop.DoDragDrop(line, data, DragDropEffects.Move);
            }
        }

        private void LinePointMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.Source as Image;
            if (point != null)
            {
                var data = new DataObject(typeof(Image), point);
                DragDrop.DoDragDrop(point, data, DragDropEffects.Move);
            }
        }

        private void RectangleMouseDown(object sender, MouseButtonEventArgs e)
        {
            var rectangle = e.Source as Canvas;
            if (rectangle != null)
            {
                _currentMousePosition = e.GetPosition(_canvasImage);
                var data = new DataObject(typeof(Canvas), rectangle);
                DragDrop.DoDragDrop(rectangle, data, DragDropEffects.Move);
            }
        }
 
        private void DragNode(object sender, MouseButtonEventArgs e)
        {
            var image = e.Source as System.Windows.Controls.Image;
            if (image != null)
            {
                _currentMousePosition = e.GetPosition(_canvasImage);
                var data = new DataObject(typeof(Image), image);
                DragDrop.DoDragDrop(image, data, DragDropEffects.Move);
            }

        }

        private void CanvasImageDragOver(object sender, DragEventArgs e)
        {
            var nodeImage = e.Data.GetData(typeof(Image)) as System.Windows.Controls.Image;
            if(nodeImage != null)
            {
                if (nodeImage.Name != "RightBottom") return;
                var diffX = e.GetPosition(_canvasImage).X - Canvas.GetLeft(_rectangle);
                var diffY = e.GetPosition(_canvasImage).Y - Canvas.GetTop(_rectangle);
                var resizeCheck = false;

                if (diffX > MiniRectangleSize && e.GetPosition(_canvasImage).X <= _canvasImage.Width)
                {
                    if (diffX>=_line.X1 + 11)
                    {
                        if (diffX > _line.X2)
                        {
                            resizeCheck = true;
                            _rectangle.Width = _border.Width = diffX;
                            Canvas.SetLeft(nodeImage, e.GetPosition(_canvasImage).X - nodeImage.Width / 2);
                        }
                        else
                        {
                            if(_direction == "Vertical" && _line.X2-_line.X1-5 > LineMiniLength)
                            {
                                resizeCheck = true;
                                _line.X2 = diffX-5;
                                _rectangle.Width = _border.Width = diffX;
                                Canvas.SetLeft(nodeImage, e.GetPosition(_canvasImage).X - nodeImage.Width / 2);
                            }
                        }
                    }
                }

                if (diffY > MiniRectangleSize && e.GetPosition(_canvasImage).Y <= _canvasImage.Height)
                {
                    if (diffY >= _line.Y1 +11)
                    {
                        if (diffY > _line.Y2)
                        {
                            resizeCheck = true;
                            _rectangle.Height = _border.Height = diffY;
                            Canvas.SetTop(nodeImage, e.GetPosition(_canvasImage).Y - nodeImage.Height / 2);
                        }
                        else
                        {
                            if (_direction == "Horizontal" && _line.Y2 - _line.Y1-5 > LineMiniLength)
                            {
                                resizeCheck = true;
                                _line.Y2 = diffY-5;
                                _rectangle.Height = _border.Height = diffY;
                                Canvas.SetTop(nodeImage, e.GetPosition(_canvasImage).Y - nodeImage.Height / 2);
                            }
                        }
                    }
                }

                if ((diffX > 0 || diffY > 0) && resizeCheck)
                {
                    AdjustLineByDirection(_direction,false);
                    AdjustArrowLineByDirection(_direction);
                }
                if (_drawingMode == "ROI")
                {
                    CreateRectangles();
                }

                return;
            }

            var nodeRect = e.Data.GetData(typeof(Canvas)) as Canvas;
            if (nodeRect != null)
            {
                var diffX = e.GetPosition(_canvasImage).X -_currentMousePosition.X;
                var diffY = e.GetPosition(_canvasImage).Y - _currentMousePosition.Y;
                _currentMousePosition = e.GetPosition(_canvasImage);

                var finalX = Canvas.GetLeft(nodeRect) + diffX;
                var finalY = Canvas.GetTop(nodeRect) + diffY;

                if (finalX <= 0 || finalX >= _canvasImage.Width - _rectangle.Width || finalY <= 0 || finalY >= _canvasImage.Height - _rectangle.Height)
                {
                    return;
                }

                Canvas.SetLeft(nodeRect, Canvas.GetLeft(nodeRect) + diffX);
                Canvas.SetTop(nodeRect, Canvas.GetTop(nodeRect) + diffY);

                foreach (var child in _canvasImage.Children)
                {
                    var point = child as Image;
                    if (point != null)
                    {
                        if (point.Name == "BasicPhoto") continue;
                        Canvas.SetLeft(point, Canvas.GetLeft(point) + diffX);
                        Canvas.SetTop(point, Canvas.GetTop(point) + diffY);
                    }
                }
            }
            if (_drawingMode == "ROI")
            {
                CreateRectangles();
            }
        }

        private void RectangleDragOver(object sender, DragEventArgs e)
        {
            var newPoint = e.GetPosition(_rectangle);
            //drag line
            var nodeLine = e.Data.GetData(typeof(Line)) as Line;
            if (nodeLine != null)
            {
                e.Handled = true; 
                var diffX = newPoint.X - _currentMousePosition.X;
                var diffY = newPoint.Y - _currentMousePosition.Y;
                _currentMousePosition = newPoint;

                if (newPoint.X <= 0 || newPoint.X >= _rectangle.Width - 0 || newPoint.Y <= 0 || newPoint.Y >= _rectangle.Height - 0 || nodeLine.X2>=_rectangle.Width || nodeLine.Y2 >= _rectangle.Height)
                {
                    return;
                }

                var newX1 = nodeLine.X1 + diffX;
                var newX2 = nodeLine.X2 + diffX;
                var newY1 = nodeLine.Y1 + diffY;
                var newY2 = nodeLine.Y2 + diffY;

                if (newX1 <= 0 || newY1 <= 0 || newX2 >= _rectangle.Width || newY2 >= _rectangle.Height)
                {
                    return;
                }

                nodeLine.X1 += diffX;
                nodeLine.X2 += diffX;
                nodeLine.Y1 += diffY;
                nodeLine.Y2 += diffY;

                AdjustArrowLineByDirection(_direction);
                AdjustLinePointsPosition();
            }

            //drag points
            var nodePoint = e.Data.GetData(typeof(System.Windows.Controls.Image)) as System.Windows.Controls.Image;
            if (nodePoint != null && _line != null)
            {
                if(newPoint.X<=-5 || newPoint.Y<=-5 || newPoint.X > _rectangle.Width-10 || newPoint.Y >  _rectangle.Height-10)
                {
                    return;
                }
                
                if (nodePoint.Name == "LeftPoint")
                {
                    e.Handled = true; 
                    var rightPointPosition = new Point(Canvas.GetLeft(_lineRightPot), Canvas.GetTop(_lineRightPot));
                    if (_line.DataContext as String == "Vertical")
                    {
                        if (rightPointPosition.X - newPoint.X <= LineMiniLength)
                        {
                            return;
                        }
                        _line.X1 = newPoint.X + 5;
                        Canvas.SetLeft(nodePoint, newPoint.X);
                    }
                    else
                    {
                        if (rightPointPosition.Y - newPoint.Y <= LineMiniLength)
                        {
                            e.Handled = true; 
                            return;
                        }
                        _line.Y1 = newPoint.Y + 5;
                        Canvas.SetTop(nodePoint, newPoint.Y);
                    }
                }
                else if (nodePoint.Name == "RightPoint")
                {
                    e.Handled = true; 
                    var leftPointPosition = new Point(Canvas.GetLeft(_lineLeftPot), Canvas.GetTop(_lineLeftPot));
                    if (_line.DataContext as String == "Vertical")
                    {
                        if ( newPoint.X - leftPointPosition.X <= LineMiniLength)
                        {
                           return;
                        }
                        _line.X2 = newPoint.X + 5;
                        Canvas.SetLeft(nodePoint, newPoint.X);
                    }
                    else
                    {
                        if (newPoint.Y - leftPointPosition.Y <= LineMiniLength)
                        {
                            e.Handled = true; 
                            return;
                        }
                        _line.Y2 = newPoint.Y + 5;
                        Canvas.SetTop(nodePoint, newPoint.Y);
                    }
                }
                AdjustArrowLineByDirection(_line.DataContext as String);
            }
        }

        private void RectangleDrop(object sender, DragEventArgs e)
        {
            CreateRectangles();
        }

        private void AdjustLineByDirection(String direction,Boolean isDefault)
        {
            if (direction == "Horizontal")
            {
                if (isDefault)
                {
                    _line.X1 = _rectangle.Width / 2;
                    _line.X2 = _rectangle.Width / 2;
                    _line.Y1 = _margin;
                    _line.Y2 = _rectangle.Height - _margin;
                }
            }
            else
            {
                if (isDefault)
                {
                    _line.X1 = _margin;
                    _line.X2 = _rectangle.Width - _margin;
                    _line.Y1 = _rectangle.Height / 2;
                    _line.Y2 = _rectangle.Height / 2;
                }
            }
            _lineLeftPot.Cursor = _lineRightPot.Cursor = direction == "Horizontal" ? Cursors.ScrollNS : Cursors.ScrollWE;
            CreateRectangles();
        }

        private void AdjustArrowLineByDirection(String direction)
        {
            if (direction == "Horizontal")
            {
                //          ←|
                //              |→
                //East
                _arrowNE.StrokeThickness = ArrowWidth;
                _arrowNE.X1 = _line.X1 * 0.25 > ArrowDistanceLimit ? _line.X1 - ArrowDistanceLimit : _line.X1 - _line.X1 * 0.25;
                _arrowNE.Y1 = (_line.Y2 + _line.Y1) * 0.5;
                _arrowNE.X2 = _arrowNE.X1 - 3;
                _arrowNE.Y2 = (_line.Y2 + _line.Y1) * 0.5;

                _lineNE.StrokeThickness = ArrowWidth/2;
                _lineNE.X1 = _line.X2;
                _lineNE.Y1 = (_line.Y2 + _line.Y1) * 0.5;
                _lineNE.X2 = _line.X1 * 0.25 > ArrowDistanceLimit ? _line.X2 - ArrowDistanceLimit : _line.X2 - _line.X1 * 0.25; 
                _lineNE.Y2 = (_line.Y2+ _line.Y1) * 0.5;

                Canvas.SetTop(_textCountNE, _rectangle.Height/2 - _textCountNE.Height/2);
                Canvas.SetLeft(_textCountNE, 10 - _textCountNE.Width/2);

                //West
                _arrowSW.StrokeThickness = ArrowWidth;
                _arrowSW.X1 = (_rectangle.Width - _line.X1) * 0.25 > ArrowDistanceLimit ? _line.X1 + ArrowDistanceLimit : _line.X1 + (_rectangle.Width - _line.X1) * 0.25;
                _arrowSW.Y1 = (_line.Y2 + _line.Y1) * 0.5;
                _arrowSW.X2 = _arrowSW.X1 + 3;
                _arrowSW.Y2 = (_line.Y2 + _line.Y1) * 0.5;

                _lineSW.StrokeThickness = ArrowWidth/2;
                _lineSW.X1 = _line.X2;
                _lineSW.Y1 = (_line.Y2 + _line.Y1) * 0.5;
                _lineSW.X2 = (_rectangle.Width - _line.X1) * 0.25 > ArrowDistanceLimit ? _line.X2 + ArrowDistanceLimit : _line.X2 + (_rectangle.Width - _line.X1) * 0.25; 
                _lineSW.Y2 = (_line.Y2 + _line.Y1) * 0.5;

                Canvas.SetTop(_textCountSW, _rectangle.Height/2 - _textCountSW.Height / 2);
                Canvas.SetLeft(_textCountSW, _rectangle.Width - 10 - _textCountSW.Width / 2);
            }
            else
            {
                //       ↑
                //------------
                //                 ↓
                //North
                _arrowNE.StrokeThickness = ArrowWidth;
                _arrowNE.X1 = (_line.X2+ _line.X1) * 0.5;
                _arrowNE.Y1 = (_line.Y1) * 0.25 > ArrowDistanceLimit ? _line.Y1 - ArrowDistanceLimit : _line.Y1 - (_line.Y1) * 0.25;
                _arrowNE.X2 = (_line.X2+ _line.X1) * 0.5;
                _arrowNE.Y2 = _arrowNE.Y1 - 3;

                _lineNE.StrokeThickness = ArrowWidth/2;
                _lineNE.X1 = (_line.X2 + _line.X1) * 0.5;
                _lineNE.Y1 = _line.Y1;
                _lineNE.X2 = (_line.X2+ _line.X1) * 0.5;
                _lineNE.Y2 = (_line.Y1) * 0.25 > ArrowDistanceLimit ? _line.Y1 - ArrowDistanceLimit : _line.Y1 - (_line.Y1) * 0.25;

                Canvas.SetTop(_textCountNE, 10 - _textCountNE.Height/2);
                Canvas.SetLeft(_textCountNE, _rectangle.Width/2 - _textCountNE.Width/2);

                //South
                _arrowSW.StrokeThickness = ArrowWidth;
                _arrowSW.X1 = (_line.X2 + _line.X1) * 0.5;
                _arrowSW.Y1 = (_rectangle.Height - _line.Y1) * 0.25 > ArrowDistanceLimit ? _line.Y1 + ArrowDistanceLimit : _line.Y1 + (_rectangle.Height - _line.Y1) * 0.25;
                _arrowSW.X2 = (_line.X2+ _line.X1) * 0.5;
                _arrowSW.Y2 = _arrowSW.Y1 + 3;

                _lineSW.StrokeThickness = ArrowWidth/2;
                _lineSW.X1 = (_line.X2+ _line.X1) * 0.5;
                _lineSW.Y1 = _line.Y1;
                _lineSW.X2 = (_line.X2 + _line.X1) * 0.5;
                _lineSW.Y2 = (_rectangle.Height - _line.Y1) * 0.25 > ArrowDistanceLimit ? _line.Y1 + ArrowDistanceLimit : _line.Y1 + (_rectangle.Height - _line.Y1) * 0.25;

                Canvas.SetTop(_textCountSW, _rectangle.Height - 10 - _textCountSW.Height / 2);
                Canvas.SetLeft(_textCountSW, _rectangle.Width / 2 - _textCountSW.Width / 2);

            }
            AdjustLinePointsPosition();
            CreateRectangles();
        }

        private void AdjustLinePointsPosition()
        {
            if (_rectangle != null && _line != null && _lineLeftPot != null && _lineRightPot != null)
            {
                Canvas.SetTop(_lineLeftPot, _line.Y1 - 5);
                Canvas.SetLeft(_lineLeftPot, _line.X1 - 5);
                Canvas.SetTop(_lineRightPot, _line.Y2 - 5);
                Canvas.SetLeft(_lineRightPot, _line.X2 - 5);
            }
        }

        public void ChangeLineDirection()
        {
            if (_canvasImage == null || _rectangle == null) return;
           _line.DataContext = _direction = _direction == "Vertical" ? "Horizontal" : "Vertical";
            AdjustLineByDirection(_direction,true);
            AdjustArrowLineByDirection(_direction);
            CreateRectangles();
        }

        private void CanvasImageMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawingRectangle)
            {
                AdjustRectangleByCurrentPoint(e.GetPosition(_canvasImage));
            }
        }

        private void AdjustRectangleByCurrentPoint(Point point)
        {
            if (_rectangle == null) return;
            var diffX = point.X - _currentRectangleStartPoint.X;
            var diffY = point.Y - _currentRectangleStartPoint.Y;
            if (diffX > 0)
            {
               _rectangle.Width = _border.Width = diffX;
            }

            if (diffY > 0)
            {
                _rectangle.Height = _border.Height = diffY;
            }

            if (_drawingMode == "ROI")
            {
                CreateRectangles();
            }

        }

        private System.Windows.Controls.Image CreateImageNode(String name, System.Drawing.Image image, Int32 width, Int32 height, Double x, Double y)
        {
            var myImage = new Image();
            var source = Converter.ImageToBitmapSource(image);
            if (source != null)
            {
                myImage.Source = source;
                myImage.Name = name;
                myImage.Width = width;
                myImage.Height = height;
                Canvas.SetTop(myImage, y - (myImage.Height / 2));
                Canvas.SetLeft(myImage, x - (myImage.Width / 2));
                return myImage;
            }
            return null;
        }

        private static Size CalculateImageSize(Double width, Double height)
        {
            _ratio = 1;
            var returnSize = new Size
            {
                Width = width,
                Height = height
            };

            if (width <= Int32.Parse(Properties.Resources.PadWidth) && height <= Int32.Parse(Properties.Resources.PadHeight))
            {
                if(width > height)
                {
                    returnSize.Width = Int32.Parse(Properties.Resources.PadWidth);
                    _ratio = (Double.Parse(Properties.Resources.PadWidth) / Double.Parse(width.ToString())) * _ratio;
                    returnSize.Height = height * _ratio;
                }
                else
                {
                    returnSize.Height = Int32.Parse(Properties.Resources.PadHeight);
                    _ratio = (Double.Parse(Properties.Resources.PadHeight) / Double.Parse(height.ToString())) * _ratio;
                    returnSize.Width = width * _ratio;
                }
            }
            else
            {
                if (width > Int32.Parse(Properties.Resources.PadWidth))
                {
                    returnSize.Width = Int32.Parse(Properties.Resources.PadWidth);
                    _ratio = (Double.Parse(Properties.Resources.PadWidth)/Double.Parse(width.ToString())) * _ratio;
                    returnSize.Height = height * (Double.Parse(Properties.Resources.PadWidth) / Double.Parse(width.ToString()));

                    if (returnSize.Height > Double.Parse(Properties.Resources.PadHeight))
                    {
                        returnSize = CalculateImageSize(returnSize.Width, returnSize.Height);
                    }
                }
                else
                {
                    if (height > Int32.Parse(Properties.Resources.PadHeight))
                    {
                        returnSize.Height = Int32.Parse(Properties.Resources.PadHeight);
                        _ratio = (Double.Parse(Properties.Resources.PadHeight) / Double.Parse(height.ToString())) * _ratio;
                        returnSize.Width = width * (Double.Parse(Properties.Resources.PadHeight) / Double.Parse(height.ToString()));

                        if (returnSize.Width > Double.Parse(Properties.Resources.PadWidth))
                        {
                            returnSize = CalculateImageSize(returnSize.Width, returnSize.Height);
                        }
                    }
                }
            }
            return returnSize;
        }

    }
}
