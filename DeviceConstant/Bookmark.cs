using System;
using System.Drawing;
using Constant;

namespace DeviceConstant
{
    public class Bookmark
    {
        public String Creator;
        public DateTime DateTime;
        public DateTime CreateDateTime;

        private Image _bookmark;
        private readonly Int32 _bookmarkWidth;
        private Rectangle _bookmarkRectangle;
        public Boolean Selected
        {
            set
            {
                _bookmark = (value) ? BookmarkImage : BookmarkImageUnSelected;
            }
        }

        public readonly Image BookmarkImage;
        public readonly Image BookmarkImageUnSelected;

        public Bookmark()
        {
            BookmarkImage = Resources.GetResources(Properties.Resources.bookmark, Properties.Resources.IMGBookmark);
            BookmarkImageUnSelected = Resources.GetResources(Properties.Resources.bookmark_unselected, Properties.Resources.IMGBookmarkUnselected);

            _bookmarkRectangle = new Rectangle(0, 0, BookmarkImage.Width, BookmarkImage.Height);
            _bookmarkWidth = Convert.ToInt32(Math.Round(BookmarkImage.Width / 2.0));
        }

        public void Paint(Graphics graphics, Int32 x, Int32 y)
        {
            _bookmarkRectangle.X = x - _bookmarkWidth;
            _bookmarkRectangle.Y = y;
            graphics.DrawImage(_bookmark, _bookmarkRectangle.X, _bookmarkRectangle.Y);
        }

        public Boolean Contains(Point point)
        {
            return _bookmarkRectangle.Contains(point);
        }
    }
}
