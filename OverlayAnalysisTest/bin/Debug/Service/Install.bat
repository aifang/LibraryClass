%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe WindowsService.exe
Net Start FHService
sc config FHService start= auto
pause