using System;

namespace Constant
{
	public class ComboxItem
	{
		public string Text = "";
        public string Value = "";
        public Object Object = null;

        public int Depth = 0;
        public Boolean Selectable = true;
        public Boolean Visiable = true;
        public ComboxItem ParentNode = null;

        public ComboxItem(string text, Object value)
		{
			Text = text;
            Value = value as String;
		    Object = value;
		}

        public ComboxItem() { }

        public override string ToString()
		{
			return Text;
		}
	}
}
