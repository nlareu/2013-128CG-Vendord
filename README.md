# 2013-128CG-Vendord

## Development Environment

- Visual Studio 2008
- [.NET Compact Framework 3.5 Redistributable]

## ClickOnce Deployment of Vendord.Desktop.App

### Publish Vendord.Desktop.App

- Open Vendord.sln in Visual Studio 2008.
- Right Click Vendord.Desktop.App > Publish
- Where do you want to publish the application?
	- C:\Users\Shaun\Documents\GitHub\BigFont\BigFont.MVC\Software\Vendord\Win7\
	- Click Next
- How will users install the application? 
	- From a website: 
	- http://bigfont.ca/software/vendord/win7/
	- Click Next
- Will the application be available offline?
	- Yes. This app is available online and offline.
	- Next   
	- Finish
- Close Visual Studio 2008, saving changes.

### Include the new files in BigFont.MVC

- Open BigFont.MVC in Visual Studio 2013
- In the solution explorer, expand to \Software\Vendord\Win7\ApplicationFiles
- Show all files in the solution explorer.
- Right click on the newly published version of Vendord (e.g. Vendord.Desktop.App_1_0_0_64) 
- Choose Include in Project
- (Also include any other files that are new in this version of Vendord.Desktop.App)
- Close Visual Studio 2013, saving changes.

### Push the new files to the Internet

- Open the BigFont website in Git
- Run the following commands

```
git add -A
git commit -m "Publish version __ of Vendord.Desktop.App."
git push
```

### Test the installation package.

- Go to http://manage.windowsazure.com	
- Websites > bigfont > deployments
- Check that the deployment succeeded.
- If it did, go to http://bigfont.ca/software/vendord/win7/publish.htm > Install
- Choose "Save File"
- Run setup.exe
- Accept the myriad security prompts
- Once it is downloaded, choose to Run setup.exe
- Accept the myriad security prompts
- Installation will eventually complete and Vendord will open.
- The whole process takes about five minutes on my computer.

## Uninstall Vendord.Desktop.App

### Uninstall Vendord

- Open Add/Remove Programs (appwiz.cpl)
- Uninstall Vendord

### Uninstall dependencies

First try with Add/Remove Programs then with [Revo].

1. Windows Mobile Device Center
1. MS SQL Server Compact 3.5 SP2 x64 (not always present)
1. MS SQL Server Compact 3.5 SP2 for Devices (not always present)
1. MS SQL Server Compact 3.5 SP2
1. MS SQL Server 2005 Compact Edition (not always present)
1. MS .NET Compact Framework 3.5 (uninstall sometimes isn't an option)
1. Microsoft Sync Framework 2.1 Core Components (x86)
1. Microsoft Sync Framework 2.1 Database Providers (x86)

Delete Documents/VENDORD (unless you want to keep user data).

## Install Vendord.SmartDevice.App

### Reset the machine to factory

- Turn of the Windows CE 6.0 Device
- Then do a cold boot; i.e. hold and release 1 + 9 + Power Button

### Install SQL Server Compact Editition 3.5

- Download the [SQL CE for Devices Installer]
- Then run the downloaded MSI
- Then go to "C:\Program Files (x86)\Microsoft SQL Server Compact Edition\v3.5\Devices\wce500\armv4i\"
- Copy sqlce.wce5.armv4i.CAB to the device
- Run it from the device to install.

### Install Vendord.Device.App

- Open Vendord.sln in Visual Studio 2008



[.NET Compact Framework 3.5 Redistributable]:
http://www.microsoft.com/en-ca/download/details.aspx?id=65

[Revo]:
http://www.revouninstaller.com/

[SQL CE for Devices Installer]:
http://www.microsoft.com/en-ca/download/details.aspx?id=12264
