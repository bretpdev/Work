using System;

namespace INCIDENTRP
{
	partial class PhysicalDamageIncidentType : BaseIncidentType
	{
		private PhysicalDamageLossOrTheftIncident _physicalIncident;

		public PhysicalDamageIncidentType(PhysicalDamageLossOrTheftIncident physicalIncident)
		{
			InitializeComponent();
			_physicalIncident = physicalIncident;

			if (_physicalIncident.DesktopWasDamaged)
                chkDesktopDamaged.Checked = true;
			if (_physicalIncident.DesktopWasLost)
                chkDesktopLost.Checked = true;
			if (_physicalIncident.DesktopWasStolen)
                chkDesktopStolen.Checked = true;
			if (_physicalIncident.LaptopWasDamaged)
                chkLaptopDamaged.Checked = true;
			if (_physicalIncident.LaptopWasLost)
                chkLaptopLost.Checked = true;
			if (_physicalIncident.LaptopWasStolen)
                chkLaptopStolen.Checked = true;
			if (_physicalIncident.MobileComunicationDeviceWasLost)
                chkMobileLost.Checked = true;
			if (_physicalIncident.MobileComunicationDeviceWasStolen)
                chkMobileStolen.Checked = true;
			if (_physicalIncident.RemovableMediaWithPiiWasLost)
                chkRemovableMediaLost.Checked = true;
			if (_physicalIncident.RemovableMediaWithPiiWasStolen)
                chkRemovableMediaStolen.Checked = true;
			if (_physicalIncident.MicrofilmWithRecordsContainingPiiWasLost)
                chkMicrofilmLost.Checked = true;
			if (_physicalIncident.MicrofilmWithRecordsContainingPiiWasStolen)
                chkMicrofilmStolen.Checked = true;
			if (_physicalIncident.PaperRecordWithPiiWasLost)
                chkPaperLost.Checked = true;
			if (_physicalIncident.PaperRecordWithPiiWasStolen)
                chkPaperStolen.Checked = true;
			if (_physicalIncident.WindowOrDoorWasDamaged)
                chkWindowDoorDamage.Checked = true;
			if (_physicalIncident.DataWasEncrypted)
                radYes.Checked = true;
		}

		private void chkDesktopDamaged_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.DesktopWasDamaged = chkDesktopDamaged.Checked;
			if (chkDesktopDamaged.Checked)
			{
				chkDesktopLost.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopLost.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileLost.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperLost.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = true;
				radNo.Checked = true;
			}
		}

		private void chkDesktopLost_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.DesktopWasLost = chkDesktopLost.Checked;
			if (chkDesktopLost.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopLost.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileLost.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperLost.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = true;
				radNo.Checked = true;
			}
		}

		private void chkDesktopStolen_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.DesktopWasStolen = chkDesktopStolen.Checked;
			if (chkDesktopStolen.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopLost.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopLost.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileLost.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperLost.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = true;
				radNo.Checked = true;
			}
		}

		private void chkLaptopDamaged_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.LaptopWasDamaged = chkLaptopDamaged.Checked;
			if (chkLaptopDamaged.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopLost.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopLost.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileLost.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperLost.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = true;
				radNo.Checked = true;
			}
		}

		private void chkLaptopLost_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.LaptopWasLost = chkLaptopLost.Checked;
			if (chkLaptopLost.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopLost.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileLost.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperLost.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = true;
				radNo.Checked = true;
			}
		}

		private void chkLaptopStolen_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.LaptopWasStolen = chkLaptopStolen.Checked;
			if (chkLaptopStolen.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopLost.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopLost.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileLost.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperLost.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = true;
				radNo.Checked = true;
			}
		}

		private void chkMobileLost_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.MobileComunicationDeviceWasLost = chkMobileLost.Checked;
			if (chkMobileLost.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopLost.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopLost.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperLost.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = true;
				radNo.Checked = true;
			}
		}

		private void chkMobileStolen_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.MobileComunicationDeviceWasStolen = chkMobileStolen.Checked;
			if (chkMobileStolen.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopLost.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopLost.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileLost.Checked = false;
				chkPaperLost.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = true;
				radNo.Checked = true;
			}
		}

		private void chkRemovableMediaLost_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.RemovableMediaWithPiiWasLost = chkRemovableMediaLost.Checked;
			if (chkRemovableMediaLost.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopLost.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopLost.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileLost.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperLost.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = true;
				radNo.Checked = true;
			}
		}

		private void chkRemovableMediaStolen_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.RemovableMediaWithPiiWasStolen = chkRemovableMediaStolen.Checked;
			if (chkRemovableMediaStolen.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopLost.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopLost.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileLost.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperLost.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = true;
				radNo.Checked = true;
			}
		}

		private void chkMicrofilmLost_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.MicrofilmWithRecordsContainingPiiWasLost = chkMicrofilmLost.Checked;
			if (chkMicrofilmLost.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopLost.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopLost.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileLost.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperLost.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = false;
				radNo.Checked = true;
			}
		}

		private void chkMicrofilmStolen_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.MicrofilmWithRecordsContainingPiiWasStolen = chkMicrofilmStolen.Checked;
			if (chkMicrofilmStolen.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopLost.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopLost.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMobileLost.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperLost.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = false;
				radNo.Checked = true;
			}
		}

		private void chkPaperLost_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.PaperRecordWithPiiWasLost = chkPaperLost.Checked;
			if (chkPaperLost.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopLost.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopLost.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileLost.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = false;
				radNo.Checked = true;
			}
		}

		private void chkPaperStolen_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.PaperRecordWithPiiWasStolen = chkPaperStolen.Checked;
			if (chkPaperStolen.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopLost.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopLost.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileLost.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperLost.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				chkWindowDoorDamage.Checked = false;
				grpEncrypted.Visible = false;
				radNo.Checked = true;
			}
		}

		private void chkWindowDoorDamage_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.WindowOrDoorWasDamaged = chkWindowDoorDamage.Checked;
			if (chkWindowDoorDamage.Checked)
			{
				chkDesktopDamaged.Checked = false;
				chkDesktopLost.Checked = false;
				chkDesktopStolen.Checked = false;
				chkLaptopDamaged.Checked = false;
				chkLaptopLost.Checked = false;
				chkLaptopStolen.Checked = false;
				chkMicrofilmLost.Checked = false;
				chkMicrofilmStolen.Checked = false;
				chkMobileLost.Checked = false;
				chkMobileStolen.Checked = false;
				chkPaperLost.Checked = false;
				chkPaperStolen.Checked = false;
				chkRemovableMediaLost.Checked = false;
				chkRemovableMediaStolen.Checked = false;
				grpEncrypted.Visible = false;
				radNo.Checked = true;
			}
		}

		private void radYes_CheckedChanged(object sender, EventArgs e)
		{
			_physicalIncident.DataWasEncrypted = radYes.Checked;
		}
	}//class
}//namespace
