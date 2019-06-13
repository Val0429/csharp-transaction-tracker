using System;

namespace Constant
{
    public class Store
    {
        public String Id = "";
        public String Name = "";
        public String Address = "";
        public String Phone = "";

        public override String ToString()
        {
            return Id.PadLeft(2, '0') + " " + Name;
        }
    }
}
