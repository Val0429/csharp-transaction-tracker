using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Constant.Utility;

namespace PanelBase
{
    public partial class ValidatedTextBox : TextBox
    {
        public ValidatedTextBox()
        {
            InitializeComponent();
            
            ErrorColor = Color.FromArgb(223, 173, 183);
        }


        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (Validate(e.KeyChar))
            {
                base.OnKeyPress(e);
            }
            else
            {
                e.Handled = true;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            IsValid = Validate();

            BackColor = IsValid ? NormalColor : ErrorColor;

            base.OnTextChanged(e);
        }

        private bool Validate(char keyChar)
        {
            switch (Type)
            {
                case ValidationType.NumberAndAlpha:
                    return GenericUtility.IsNumberAndAlphaOnly(keyChar);
                case ValidationType.Number:
                    return GenericUtility.IsNumberOnly(keyChar);
                case ValidationType.ExceptXmlEscape:
                    return !IsXmlEscape(keyChar);
                case ValidationType.LetterOrDigit:
                    return char.IsLetterOrDigit(keyChar) || char.IsControl(keyChar);
                case ValidationType.Custom:
                    return CustomValidation != null && CustomValidation(keyChar);
                default:
                    return true;
            }
        }
        
        private static bool IsXmlEscape(char keyChar)
        {
            return keyChar == '>' || keyChar == '<' || keyChar == '\'' || keyChar == '\"' || keyChar == '&';
        }

        public bool Validate()
        {
            switch (Type)
            {
                case ValidationType.Email:
                    return GenericUtility.IsEmail(Text);
                case ValidationType.EmailAndNullable:
                    return string.IsNullOrEmpty(Text) || GenericUtility.IsEmail(Text);
                case ValidationType.IPAddress:
                    return GenericUtility.IsIPAddress(Text);
                default:
                    return true;
            }
        }


        // Properties
        public bool IsValid { get; private set; }

        public ValidationType Type { get; set; }

        public Color ErrorColor { get; set; }

        private Color _normalColor = Color.White;
        public Color NormalColor
        {
            get { return _normalColor; }
            set { _normalColor = BackColor = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<char, bool> CustomValidation { get; set; }
    }


    public enum ValidationType { None, Email, NumberAndAlpha, Number, ExceptXmlEscape, LetterOrDigit, EmailAndNullable, IPAddress, Custom }
}
