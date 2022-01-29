using System;
using System.Windows.Forms;
using Q;

namespace RefereneceTelephoneActivityContact
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			ReflectionInterface ri = ReflectionInterfaceFromOpenSession.Connect();
			if (ri == null)
			{
				MessageBox.Show("No Reflection sessions were found. Please open a session and try again.", "No Sessions Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else
			{
				while (new StartupDialog().ShowDialog() == DialogResult.OK)
				{
					try
					{
						new RtacCompass(ri).Main();
					}
					catch (EndDLLException)
					{
						return;
					}
				}//while
			}
		}//Main()
	}//class
}//namespace
