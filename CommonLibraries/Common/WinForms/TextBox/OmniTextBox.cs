using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public delegate void ValidationEventHandler(object sender, SimpleValidationEventArgs e);
    public class SimpleValidationEventArgs
    {
        public bool Valid { get; set; }
        public string ValidationMessage { get; set; }
        public SimpleValidationEventArgs()
        {
            Valid = false;
        }
    }
    public class OmniTextBox : TextBox
    {
        public OmniTextBox()
        {
            this.KeyPress += KeyPressHandler;
            this.KeyUp += KeyUpHandler;
            this.TextChanged += TextChangedHandler;
            this.Leave += LeaveHandler;
            Mask = string.Empty;
            AllowAllCharacters = true;
            AllowedAdditionalCharacters = "";
            AllowAlphaCharacters = true;
            AllowNumericCharacters = true;
            InvalidColor = Color.LightPink;
            ValidColor = Color.LightGreen;
            IsValid = null;
        }

        #region Text Masking

        private string mask;
        public string Mask
        {
            get { return mask; }
            set
            {
                mask = value;
                if (!string.IsNullOrEmpty(mask))
                    this.MaxLength = mask.Length;
            }
        }

        protected void ApplyMask()
        {
            int cursor = this.SelectionStart;
            if (!string.IsNullOrEmpty(Mask))
            {
                for (int i = 0; i < Text.Length; i++)
                {
                    if (i >= Mask.Length) break;
                    char maskChar = Mask[i];
                    if (maskChar == '_') continue;

                    char textChar = Text[i];
                    if (textChar != maskChar)
                    {
                        Text = Text.Insert(i, maskChar.ToString());
                    }
                }
                this.SelectionStart = cursor + 1;
            }
            
        }

        protected void AppendMaskChar()
        {
            int cursor = SelectionStart;
            if (cursor == Text.Length)
                if (Mask.Length > cursor)
                    if (Mask[cursor] != '_')
                    {
                        Text += Mask[cursor];
                        SelectionStart = Text.Length;
                    }
        }

        protected bool MaskMatch(string input, int index)
        {
            if (Mask.Length - 1 < index) return false;
            return Mask[index] == input[index];
        }

        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Back)
            {
                AppendMaskChar();
            }
        }

        #endregion

        #region General Validation

        public Color InvalidColor { get; set; }
        public Color ValidColor { get; set; }
        private Color? NormalColor { get; set; }
        public bool? IsValid { get; private set; }
        private ToolTip tt = new ToolTip();
        private string validationMessage;
        public string ValidationMessage
        {
            get { return validationMessage; }
            set
            {
                validationMessage = value;
                tt.SetToolTip(this, value);
            }
        }

        public event ValidationEventHandler ValidationOnLeave;
        public event ValidationEventHandler ValidationFinal;
        public event ValidationEventHandler ValidationOnKeyPress;

        public bool Validate()
        {
            if (ValidationFinal != null)
            {
                SimpleValidationEventArgs args = new SimpleValidationEventArgs();
                ValidationFinal(this, args);
                ValidationMessage = args.ValidationMessage;
                return args.Valid;
            }
            return true;
        }

        public void MarkValid()
        {
            NormalCheck();
            IsValid = true;
            this.BackColor = ValidColor;
        }
        public void MarkInvalid()
        {
            NormalCheck();
            IsValid = false;
            this.BackColor = InvalidColor;
        }
        public void MarkNormal()
        {
            NormalCheck();
            IsValid = null;
            this.BackColor = NormalColor.Value;
        }
        private void NormalCheck()
        {
            if (!NormalColor.HasValue)
                NormalColor = this.BackColor;
        }

        private void LeaveHandler(object sender, EventArgs e)
        {
            ValidationCheck(ValidationOnLeave);
        }

        private void ValidationCheck(ValidationEventHandler handler)
        {
            if (handler != null)
            {
                SimpleValidationEventArgs args = new SimpleValidationEventArgs();
                handler(this, args);
                if (!args.Valid)
                    MarkInvalid();
                else
                    MarkNormal();
                ValidationMessage = args.ValidationMessage;
            }
        }
        #endregion

        #region Text and Key Validation

        public bool AllowAllCharacters { get; set; }
        public bool AllowAlphaCharacters { get; set; }
        public bool AllowNumericCharacters { get; set; }
        public string AllowedAdditionalCharacters { get; set; }

        private void TextChangedHandler(object sender, EventArgs e)
        {
            ApplyMask();
            if (!ValidateInput(this.Text))
                this.Text = "";
        }

        private void KeyPressHandler(object sender, KeyPressEventArgs e)
        {
            bool isCommand = IsCommand(e);
            bool isValid = ValidateCharacter(e.KeyChar);

            if (!isCommand && !isValid)
                e.Handled = true;

            ValidationCheck(ValidationOnKeyPress);
        }

        public static bool IsCommand(KeyPressEventArgs e)
        {
            //so far backspace is the only key to generate a character.
            //arrows, copy, cut, paste do not.
            bool isBackspace = e.KeyChar == '\b';
            bool isControl = ModifierKeys.HasFlag(Keys.Control);
            bool isCommand = isBackspace || isControl;
            return isCommand;
        }

        public bool ValidateInput(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (MaskMatch(input, i))
                    continue;
                char c = input[i];
                if (!ValidateCharacter(c))
                    return false;
            }
            return true;
        }

        public bool ValidateCharacter(char c)
        {
            if (AllowAllCharacters)
                return true;
            if (AllowAlphaCharacters && ValidAlphaChar(c))
                return true;
            if (AllowNumericCharacters && ValidNumericChar(c))
                return true;
            if (AllowedAdditionalCharacters.Contains(c))
                return true;
            return false;
        }

        public static bool ValidNumericChar(char c)
        {
            int tryParse = 0;
            return int.TryParse(c.ToString(), out tryParse);
        }

        public static bool ValidAlphaChar(char c)
        {
            var isCapital = ((int)c).BetweenInc(65, 90); //A-Z
            var isLower = ((int)c).BetweenInc(97, 122); //a-z
            return isCapital || isLower;
        }
        #endregion
    }
}
