# DeltaShare by DotCube

![banner](https://github.com/user-attachments/assets/27e5a64f-30f2-4428-9fbb-c885f2a726c5)

## Team Information
- Mentor:
	- Abdullah Al Amin | [alamin4265](https://github.com/alamin4265)
- Team Members:
	- Jahangir Alam | [jahangir1x](https://github.com/jahangir1x) (Team Leader)
	- Tamim Rahman | [Tamim-Rahman101](https://github.com/Tamim-Rahman101)
	- Rejuanul Huq Sanny | [Rejuanul463](https://github.com/Rejuanul463)

## About DeltaShare
DeltaShare is a cross-platform multi-user file sharing software to exchange files seamlessly between Android and Windows. The software includes offline pooling through a local network for efficient file sharing. 

## Why DeltaShare?
Have you ever faced a situation where your team needed to share files among yourselves and you guys ended up sharing the same file again and again?

DeltaShare was born out of this very challenge. With DeltaShare, multiple users can connect simultaneously and share files at the same time, across Android and Windows devices, in offline scenario. The software is built for students, professionals, travelers to ensure seamless collaboration.

## Key Features
- Seamless offline file sharing over local networks.
- Real-time multi-user support for simultaneous file transfers.
- Cross-platform compatibility across Android and Windows devices.

## Software Architecture
In our project, we followed MVVM (Model-View-ViewModel) architectural pattern that separates an application's data logic, presentation, and user interface into three distinct components: the Model, View, and ViewModel. This separation promotes code organization, maintainability, and testability. By binding the ViewModel to the UI, we achieved a dynamic and responsive user experience, which was crucial for a real-time file-sharing application. This structure greatly helped us maintain cleaner code and adapt quickly to changes throughout the development process.

## Tech Stack
By leveraging .NET MAUI (a cross-platform framework) we developed a native mobile and desktop applications using C# and XAML.



## Getting Started
### Prerequisites
- Install .NET SDK 8.0 from [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
- Download Visual Studio 2022 from [here](https://visualstudio.microsoft.com/downloads/).
- Clone this repository git clone https://github.com/Learnathon-By-Geeky-Solutions/dotcube.git
- Open client_app/DeltaShare.sln in Visual Studio 2022.

### Run on local android device
- Enable developer mode [guide](https://developer.android.com/studio/debug/dev-options)
- Connect USB cable.
- Open ADB prompt. ![image](https://github.com/user-attachments/assets/4442a2ea-10cb-4008-add2-98a5c0208746)
- type adb devices and check device is authorized or not. Press "Allow" if asked in android. ![image](https://github.com/user-attachments/assets/55e2356e-e680-48f2-b981-8cacaf947a1e)
- Select local device from devices. ![image](https://github.com/user-attachments/assets/af6bffde-5b02-4509-86b1-cf33ff62d59d)
- Run!

## Consistent Coding Style for This Project
install the following extensions in Visual Studio 2022
- [SonarQube for Visual Studio 2022](https://marketplace.visualstudio.com/items?itemName=SonarSource.SonarLintforVisualStudio2022)
- [XAML Styler for Visual Studio 2022](https://marketplace.visualstudio.com/items?itemName=TeamXavalon.XAMLStyler2022)

Enable the following settings in Visual Studio 2022
- Tools > Options > Text Editor > C# > Code Cleanup > Run code cleanup on save

![image](https://github.com/user-attachments/assets/88196d8e-b793-45cb-8bef-32fe283af59a)

- Add following settings for Configure Code Cleanup

![image](https://github.com/user-attachments/assets/187fd303-83d7-48e6-90f1-ad29cf8f1e54)

- Tools > Options > Text Editor > XAML > Formatting > Spacing
	- Set Position each attribute on a separate line

![image](https://github.com/user-attachments/assets/ab2a7812-a4e4-4554-a8f2-d09e74a5c3d0)

## Set Firewall Rules in Windows to Run

Windows/Linux/macOS firewalls might block incoming connections.
We need to allow port 8080 in the firewall settings.

On Windows, press Win+X

![image](https://github.com/user-attachments/assets/be831432-6a96-483a-9ffd-59834ec471a7)

Open Windows Terminal (Admin)
bash
netsh http add urlacl url="http://+:9898/" user=everyone


## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
