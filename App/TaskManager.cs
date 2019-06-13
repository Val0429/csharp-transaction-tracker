using System;

namespace App
{
	public partial class AppClient
	{
		public void DisableTaskManager()
		{
			if (!IsAdministrator) return;

			try
			{
				//HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\HideFastUserSwitching
				string key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
				Microsoft.Win32.Registry.SetValue(key, "HideFastUserSwitching", 1, Microsoft.Win32.RegistryValueKind.DWord);

				//HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableLockWorkstation
				key = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System";
				Microsoft.Win32.Registry.SetValue(key, "DisableChangePassword", 1, Microsoft.Win32.RegistryValueKind.DWord);

				//HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer
				key = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
				Microsoft.Win32.Registry.SetValue(key, "NoClose", 1, Microsoft.Win32.RegistryValueKind.DWord);

				//HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableTaskMgr
				key = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System";
				Microsoft.Win32.Registry.SetValue(key, "DisableTaskMgr", 1, Microsoft.Win32.RegistryValueKind.DWord);

				//HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\NoLogoff
				key = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
				Microsoft.Win32.Registry.SetValue(key, "NoLogoff", 1, Microsoft.Win32.RegistryValueKind.DWord);
			}
			catch (Exception)
			{
			}
		}

		public void EnableTaskManager()
		{
			if (!IsAdministrator) return;

			try
			{
				//HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\HideFastUserSwitching
				string key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
				Microsoft.Win32.Registry.SetValue(key, "HideFastUserSwitching", 0, Microsoft.Win32.RegistryValueKind.DWord);

				//HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableLockWorkstation
				key = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System";
				Microsoft.Win32.Registry.SetValue(key, "DisableChangePassword", 0, Microsoft.Win32.RegistryValueKind.DWord);

				//HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer
				key = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
				Microsoft.Win32.Registry.SetValue(key, "NoClose", 0, Microsoft.Win32.RegistryValueKind.DWord);

				//HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableTaskMgr
				key = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System";
				Microsoft.Win32.Registry.SetValue(key, "DisableTaskMgr", 0, Microsoft.Win32.RegistryValueKind.DWord);

				//HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer\NoLogoff
				key = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";
				Microsoft.Win32.Registry.SetValue(key, "NoLogoff", 0, Microsoft.Win32.RegistryValueKind.DWord);
			}
			catch (Exception)
			{
			}
		}
	}
}
