using System.Drawing;
using System.Windows.Forms;

namespace INCIDENTRP
{
	class SummaryItem : Label
	{
		private bool _isValid;

		/// <summary>
		/// Creates a blank SummaryItem. Useful for inserting a blank line in a FlowLayoutPanel.
		/// </summary>
		public SummaryItem()
		{
			_isValid = true;
			AutoSize = true;
		}

		/// <summary>
		/// Creates a SummaryItem.
		/// </summary>
		/// <param name="text">The text to display in the item.</param>
		/// <param name="isHeader">Not currently used.</param>
		/// <param name="isValid">If true, a red border is drawn around the item.</param>
		public SummaryItem(string text, bool isHeader, bool isValid)
		{
			_isValid = isValid;
			AutoSize = true;
			Text = text;
			TextAlign = ContentAlignment.MiddleLeft;
			//TODO: isHeader is not currently being used, but may come in handy if we want to change how things look.

			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (!_isValid)
			{
				//Draw a red border.
				const float BORDER_SIZE = 1f;
				Color BORDER_COLOR = Color.Red;
				Rectangle rectangle = new Rectangle(1, 1, Width - 2, Height - 2);
				using (Pen pen = new Pen(new SolidBrush(BORDER_COLOR), BORDER_SIZE))
				{
					e.Graphics.DrawRectangle(pen, rectangle);
				}
			}//if
		}//OnPaint()
	}//class
}//namespace
