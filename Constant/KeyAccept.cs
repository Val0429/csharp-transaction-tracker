using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Constant
{
    public static class KeyAccept
    {
        private const string AlphaPattern = @"^[a-zA-Z]*$";
        public static void AcceptNumberAndAlphaOnly(Object sender, KeyPressEventArgs e)
        {
            //\r and backspace
            if (e.KeyChar == (Char)13 || e.KeyChar == (Char)8)
            {
                e.Handled = false;
                return;
            }

            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
                return;
            }

            if (char.IsLetter(e.KeyChar) && Regex.IsMatch(e.KeyChar.ToString(), AlphaPattern))
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;

            if (e.KeyChar == (Char)22 && sender is TextBox && ((TextBox)sender).ShortcutsEnabled)
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }

        public static void AcceptNumberOnly(Object sender, KeyPressEventArgs e)
        {
            //\r and backspace
            if (e.KeyChar == (Char)13 || e.KeyChar == (Char)8)
            {
                e.Handled = false;
                return;
            }

            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
                return;
            }
            e.Handled = true;
        }
    }
}