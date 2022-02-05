using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.WinForms;

namespace SftpCoordinator
{
    public static class Builders
    {
        public static FormBuilder SettingsForm
        {
            get
            {
                FormBuilder builder = new FormBuilder("Settings");
                builder.InputWidth = 450;
                List<Setting> settings = Settings.GetAll();
                foreach (var setting in settings)
                {
                    if (setting.Value is bool)
                        builder.AddField<YesNoButton>(setting.Name, b =>
                        {
                            b.SelectedValue = (bool)setting.Value;
                            b.Tag = setting.SettingId;
                            b.Height = 25;
                        });
                    else if (setting.Value is int)
                        builder.AddField<NumericUpDown>(setting.Name, n =>
                        {
                            n.Value = (int)setting.Value;
                            n.Tag = setting.SettingId;
                        });
                    else if (Settings.ArchiveLocationSettings.Contains(setting.SettingId))
                        builder.AddField<BehaviorTextBox>(setting.Name, n =>
                        {
                            n.Text = setting.Value.ToString();
                            n.Tag = setting.SettingId;
                            n.InstalledBehaviors.Install(new IsValidDirectoryBehavior<BehaviorTextBox>(n));
                        });
                    else
                        builder.AddField<TextBox>(setting.Name, c =>
                        {
                            c.Text = setting.Value.ToString();
                            c.Tag = setting.SettingId;
                        });
                }
                builder.FormAccepted += (form) =>
                {
                    if (MessageBox.Show("Are you sure you want to save changes?", "Save changes?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        foreach (TextBox tb in form.InputControls.Filter<TextBox>())
                        {
                            SettingId settingId = (SettingId)tb.Tag;
                            var setting = settings.Where(o => o.SettingId == settingId).Single();
                            if (setting.Value.ToString() != tb.Text)
                                Settings.SaveSetting((int)settingId, tb.Text);
                        }
                        foreach (YesNoButton ynb in form.InputControls.Filter<YesNoButton>())
                        {
                            SettingId settingId = (SettingId)ynb.Tag;
                            var setting = settings.Where(o => o.SettingId == settingId).Single();
                            if (setting.Value.ToString() != ynb.SelectedValue.ToString())
                                Settings.SaveSetting((int)settingId, ynb.SelectedValue);
                        }
                        foreach (NumericUpDown nud in form.InputControls.Filter<NumericUpDown>())
                        {
                            SettingId settingId = (SettingId)nud.Tag;
                            var setting = settings.Where(o => o.SettingId == settingId).Single();
                            if (setting.Value.ToString() != nud.Value.ToString())
                                Settings.SaveSetting((int)settingId, (int)nud.Value);
                        }
                        Settings.LoadSettings();
                        Dashboard.Instance.SyncDisplay();
                        return true;
                    }
                    Dashboard.Instance.SyncDisplay();
                    return false;
                };
                builder.FormCancelled += (form) => { Dashboard.Instance.SyncDisplay(); return true; };
                return builder;
            }
        }
    }
}
