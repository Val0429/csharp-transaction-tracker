using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Constant;
using Interface;
using Rectangle = System.Drawing.Rectangle;

namespace SetupDevice
{
    /// <summary>
    /// Display Motion Regions in Layout
    /// </summary>
    public partial class MotionRegionControl
    {
        private ICamera _camera;
        private Image _image;
        private readonly Canvas _canvas;
        private Canvas _canvasImage;
        private Canvas _rectangle;

        private ToolTip _toolTipRectPoint;
        private static Double _ratio;
        private readonly SolidColorBrush _colorBrushSelected;
        private readonly SolidColorBrush _colorBrushUnselected;
        private Image _redPot;
        private TextBlock _defaultNote;
        public Dictionary<String, String> Localization;

        private Boolean _isLoadRectangles = false;
        private Border _border;
        private TextBlock _number;
        private const Int32 MiniRectangleSize = 50;
        private Point _currentMousePosition;
        private System.Drawing.Image _rectanglePointImage;
        private Canvas _coverage;
        private const UInt16 PadWidth = 720;
        private const UInt16 PadHeight = 540;
        public MotionRegionControl()
        {
            InitializeComponent();
            Localization = new Dictionary<String, String>
                               {
                                    {"SetupDevice_MotionRegionDefaultNote", "Connect fail."},
                                    {"SetupDevice_MotionRegionRectanglePointDescription", "Drag to adjust rectangle size."}
                               };
            Localizations.Update(Localization);

            _rectanglePointImage = Constant.Resources.GetResources(Properties.Resources.RedPoint, Properties.Resources.IMGRedPoint);

            _colorBrushSelected = new SolidColorBrush(Colors.Yellow)
            {
                Opacity = 0.1
            };
            _colorBrushUnselected = new SolidColorBrush(Colors.White)
            {
                Opacity = 0.3
            };

            _canvas = new Canvas
            {
                Width = Convert.ToDouble(PadWidth),
                Height = Convert.ToDouble(PadHeight),
                Background = Brushes.Black,
            };

            _coverage = new Canvas
            {
                Width = Convert.ToDouble(PadWidth),
                Height = Convert.ToDouble(PadHeight),
                Background = Brushes.Transparent,
            };

            Canvas.SetTop(_coverage, 0);
            Canvas.SetLeft(_coverage, 0);
            Canvas.SetZIndex(_coverage, 999);
            _defaultNote = new TextBlock
            {
                Text = Localization["SetupDevice_MotionRegionDefaultNote"],
                Foreground = Brushes.White,
                FontFamily = new FontFamily("Arial"),
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

            Canvas.SetTop(_defaultNote, _canvas.Height / 2 - _defaultNote.Height / 2);
            Canvas.SetLeft(_defaultNote, _canvas.Width / 2 - _defaultNote.Width / 2);
            _canvas.Children.Add(_defaultNote);

            InitializeComponent();
            Background = Brushes.Black;

            var mainWindow = this;
            mainWindow.Content = null;
            mainWindow.Content = _canvas;

            _toolTipRectPoint = new ToolTip
            {
                Content = Localization["SetupDevice_MotionRegionRectanglePointDescription"]
            };
        }

        public void ArrangeSettingByDevice(ICamera camera, System.Drawing.Image snapshot)
        {
            _camera = camera;
            if (camera == null) return;

            CreatePhotoByImage(snapshot);
        }

        public void CreatePhotoByImage(System.Drawing.Image image)
        {
            _canvas.Children.Clear();

            if(image == null)
            {
                _canvas.Children.Add(_defaultNote);
                return;
            }

            _image = new Image
            {
                Name = "BasicPhoto"
            };

            var source = Converter.ImageToBitmapSource(image);

            if (source != null)
            {
                _image.Source = source;
                var newSize = CalculateImageSize(image.Width, image.Height);
                _canvasImage = new Canvas
                {
                    Width = newSize.Width,
                    Height = newSize.Height,
                    AllowDrop = true
                };
                _canvasImage.MouseLeftButtonDown -= CanvasImageMouseDown;
                _canvasImage.MouseLeftButtonDown += CanvasImageMouseDown;
                _canvasImage.DragOver -= CanvasImageDragOver;
                _canvasImage.DragOver += CanvasImageDragOver;

                _image.Width = newSize.Width;
                _image.Height = newSize.Height;
                Canvas.SetTop(_image, 0);
                Canvas.SetLeft(_image, 0);

                if (!_canvasImage.Children.Contains(_image))
                    _canvasImage.Children.Add(_image);

                Canvas.SetLeft(_canvasImage, _canvas.Width / 2 - (_canvasImage.Width / 2));
                Canvas.SetTop(_canvasImage, _canvas.Height / 2 - (_canvasImage.Height / 2));
                _canvas.Children.Add(_canvasImage);
                _rectangle = null;

                _redPot = CreateImageNode("RightBottom", _rectanglePointImage, 10, 10, 0, 0);
                _redPot.ToolTip = _toolTipRectPoint;
                Canvas.SetZIndex(_redPot, 100);
            }
        }

        private Canvas _bottommostRectangle = null;

        private void CanvasImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            _bottommostRectangle = null;
            var cvs = sender as Canvas;
            if (cvs != null)
            {
                VisualTreeHelper.HitTest(_canvasImage, null, RectangleHitTestResult, new PointHitTestParameters(e.GetPosition(_canvasImage)));
            }

            if (_bottommostRectangle != null && _bottommostRectangle != _rectangle)
            {
                _currentMousePosition = e.GetPosition(_canvasImage);
                RectangleMouseDownForFocus(_bottommostRectangle, null);
                var data = new DataObject(typeof(Canvas), _bottommostRectangle);
                DragDrop.DoDragDrop(_bottommostRectangle, data, DragDropEffects.All);
            }
        }

        public HitTestResultBehavior RectangleHitTestResult(HitTestResult result)
        {
            if (result != null)
            {
                if (result.VisualHit.GetType().Name != "Canvas") return HitTestResultBehavior.Continue;
                var node = result.VisualHit as Canvas;
                if (node != null)
                {
                    _bottommostRectangle = node;
                }
            }
            return HitTestResultBehavior.Continue;
        } 


        public void UpdateRectangles()
        {
            if (_isLoadRectangles || _rectangle == null) return;

            if (_rectangle.DataContext == null) return;

            var currentRect = _rectangle.DataContext is KeyValuePair<ushort, Rectangle> ? (KeyValuePair<ushort, Rectangle>) _rectangle.DataContext : new KeyValuePair<ushort, Rectangle>();

            _camera.MotionRectangles[currentRect.Key] = new Rectangle((int)((int)Canvas.GetLeft(_rectangle) / _ratio), (int)((int)Canvas.GetTop(_rectangle) / _ratio), (int)((int)_rectangle.Width / _ratio), (int)((int)_rectangle.Height / _ratio));

            if (_camera.ReadyState == ReadyState.Ready)
                _camera.ReadyState = ReadyState.Modify;
        }

        public void DrawSettingByMotionRectangles()
        {
            _isLoadRectangles = true;

            var check = false;
            if (_canvasImage == null)
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

            if (check)
            {
                _isLoadRectangles = false;
                return;
            }

            var children = _canvasImage.Children.OfType<Canvas>().ToList();

            foreach (var rect in children)
            {
                _canvasImage.Children.Remove(rect);
            }

            Canvas selectItem = null;
            foreach (KeyValuePair<ushort , Rectangle> rect in _camera.MotionRectangles)
            {
                _rectangle = new Canvas
                {
                    Name = "Rectangle" + rect.Key.ToString(),
                    Background = _colorBrushUnselected,
                    Width = rect.Value.Width * _ratio,
                    Height = rect.Value.Height * _ratio,
                    AllowDrop = true,
                    DataContext = rect
                };

                Canvas.SetTop(_rectangle, rect.Value.Y * _ratio);
                Canvas.SetLeft(_rectangle, rect.Value.X * _ratio);
                if (_currentPeopleCountingRectangle == rect.Value)
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

                var text = new TextBlock
                               {
                                   Text = rect.Key.ToString(),
                                   Foreground = Brushes.White,
                                   FontFamily = new FontFamily("Arial"),
                                   FontSize = 44,
                                   FontStretch = FontStretches.UltraExpanded,
                                   FontStyle = FontStyles.Normal,
                                   FontWeight = FontWeights.Regular,
                                   Padding = new Thickness(2, 2, 2, 2),
                                   TextAlignment = TextAlignment.Center,
                                   TextWrapping = TextWrapping.Wrap
                               };

                Canvas.SetTop(text, 0);
                Canvas.SetLeft(text, 10);
                text.IsHitTestVisible = false;
                _rectangle.Children.Add(text);

                _canvasImage.Children.Add(_rectangle);
            }

            MakeAllRegionDisabled();
            _isLoadRectangles = false;

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
        }

        private void DrawRedPot()
        {
            if (_rectangle == null || _redPot == null) return;
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

                if (_border != null)
                {
                    _border.BorderBrush = Brushes.WhiteSmoke;
                }

                if (_number != null)
                {
                    _number.Foreground = Brushes.White;
                }

                var zindex = 1;
                foreach (var child in _canvasImage.Children)
                {
                    var cvs = child as Canvas;
                    if (cvs == null) continue;
                    if (cvs != _rectangle)
                    {
                        Canvas.SetZIndex(cvs, zindex);
                        zindex++;
                    }
                }
                Canvas.SetZIndex(_rectangle, zindex);
            }
            _rectangle = null;

            _currentPeopleCountingRectangle = null;
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
                if (cvs != null)
                {
                    cvs.Background = _colorBrushUnselected;
                    cvs.MouseDown -= RectangleMouseDown;
                    cvs.MouseDown += RectangleMouseDownForFocus;
                    cvs.Cursor = Cursors.Hand;
                    Canvas.SetZIndex(cvs, 0);
                }
            }
        }

        private Rectangle? _currentPeopleCountingRectangle;

        private void RectangleMouseDownForFocus(object sender, MouseButtonEventArgs e)
        {
            var cvs = sender as Canvas;

            if (cvs != null)
            {
                if (cvs == _rectangle) return;
                
                MakeSelectedRegionDisabled();
                Canvas.SetZIndex(cvs, 99);

                cvs.Background = _colorBrushSelected;
                cvs.MouseDown -= RectangleMouseDownForFocus;
                cvs.MouseDown -= RectangleMouseDown;
                cvs.MouseDown += RectangleMouseDown;
                cvs.Cursor = Cursors.SizeAll;
                _rectangle = cvs;
                _currentPeopleCountingRectangle = cvs.DataContext is Rectangle ? (Rectangle) cvs.DataContext : new Rectangle();
                DrawRedPot();

                foreach (var child in cvs.Children)
                {
                    var border = child as Border;
                    if (border != null)
                    {
                        _border = border;
                        continue;
                    }

                    var text = child as TextBlock;
                    if(text != null)
                    {
                        _number = text;
                        continue;
                    }
                }

                _border.BorderBrush = Brushes.DarkOrange;
                _number.Foreground = Brushes.Red;
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
            var image = e.Source as Image;
            if (image != null)
            {
                _currentMousePosition = e.GetPosition(_canvasImage);
                var data = new DataObject(typeof(Image), image);
                DragDrop.DoDragDrop(image, data, DragDropEffects.Move);
            }

        }

        private void CanvasImageDragOver(object sender, DragEventArgs e)
        {
            var nodeImage = e.Data.GetData(typeof(Image)) as Image;
            if (nodeImage != null)
            {
                if (nodeImage.Name != "RightBottom") return;

                var diffX = e.GetPosition(_canvasImage).X - Canvas.GetLeft(_rectangle);
                var diffY = e.GetPosition(_canvasImage).Y - Canvas.GetTop(_rectangle);

                if (diffX >= MiniRectangleSize && e.GetPosition(_canvasImage).X <= _canvasImage.Width)
                {
                    Canvas.SetLeft(nodeImage, e.GetPosition(_canvasImage).X - nodeImage.Width / 2);
                    _rectangle.Width = _border.Width = diffX;
                }

                if (diffY >= MiniRectangleSize && e.GetPosition(_canvasImage).Y <= _canvasImage.Height)
                {
                    Canvas.SetTop(nodeImage, e.GetPosition(_canvasImage).Y - nodeImage.Height / 2);
                    _rectangle.Height = _border.Height = diffY;
                }
                UpdateRectangles();
                return;
            }

            var nodeRect = e.Data.GetData(typeof(Canvas)) as Canvas;
            if (nodeRect == null) return;

            var diffRectX = e.GetPosition(_canvasImage).X - _currentMousePosition.X;
            var diffRectY = e.GetPosition(_canvasImage).Y - _currentMousePosition.Y;
            _currentMousePosition = e.GetPosition(_canvasImage);

            var oldX = Canvas.GetLeft(nodeRect);
            var oldY = Canvas.GetTop(nodeRect);

            var finalX = oldX + diffRectX;
            var finalY = oldY + diffRectY;

            if (finalX <= 0 || finalX >= _canvasImage.Width - _rectangle.Width || finalY <= 0 || finalY >= _canvasImage.Height - _rectangle.Height)
            {
                if ((oldX >= 0 && oldX <= _canvasImage.Width - _rectangle.Width) && (oldY >= 0 && oldY <= _canvasImage.Height - _rectangle.Height))
                {
                    return;
                }
            }

            Canvas.SetLeft(nodeRect, Canvas.GetLeft(nodeRect) + diffRectX);
            Canvas.SetTop(nodeRect, Canvas.GetTop(nodeRect) + diffRectY);

            foreach (var child in _canvasImage.Children)
            {
                var point = child as Image;
                if (point != null)
                {
                    if (point.Name == "BasicPhoto") continue;
                    Canvas.SetLeft(point, Canvas.GetLeft(point) + diffRectX);
                    Canvas.SetTop(point, Canvas.GetTop(point) + diffRectY);
                }
            }
            UpdateRectangles();
        }

        private Image CreateImageNode(String name, System.Drawing.Image image, Int32 width, Int32 height, Double x, Double y)
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

            if (width <= PadWidth && height <= PadHeight)
            {
                if (width > height)
                {
                    returnSize.Width = PadWidth;
                    _ratio = (PadWidth / Double.Parse(width.ToString())) * _ratio;
                    returnSize.Height = height * _ratio;
                }
                else
                {
                    returnSize.Height = PadHeight;
                    _ratio = (PadHeight / Double.Parse(height.ToString())) * _ratio;
                    returnSize.Width = width * _ratio;
                }
            }
            else
            {
                if (width > PadWidth)
                {
                    returnSize.Width = PadWidth;
                    _ratio = (PadWidth / Double.Parse(width.ToString())) * _ratio;
                    returnSize.Height = height * (PadWidth / Double.Parse(width.ToString()));

                    if (returnSize.Height > PadHeight)
                    {
                        returnSize = CalculateImageSize(returnSize.Width, returnSize.Height);
                    }
                }
                else
                {
                    if (height > PadHeight)
                    {
                        returnSize.Height = PadHeight;
                        _ratio = (PadHeight / Double.Parse(height.ToString())) * _ratio;
                        returnSize.Width = width * (PadHeight / Double.Parse(height.ToString()));

                        if (returnSize.Width > PadWidth)
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
