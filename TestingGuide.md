# How to Test the APK Builds

## Setting Up Your Testing Environment

1) On your PC, Download SideQuest: https://sidequestvr.com/download
2) This will require you to make or sign into a SideQuest account
3) Windows users will be required to download Oculus drivers: https://developer.oculus.com/downloads/package/oculus-go-adb-drivers/
4) Get the Oculus App on your iOS and Android device or Oculus Developer Hub on your Windows PC
5) Create or sign into your Facebook account or Oculus account (these may soon be forcibly be migrated into Facebook accounts)
6) Register as a developer on this account (you will have to create a new organization, which can be anything at all)

## Borrowing an Oculus Quest from the IDEA Lab

1) Headsets from the IDEA Lab must be pre-booked with the following link: https://uiuc.libcal.com/equipment?lid=5488
2) Physically pick it up from the IDEA Lab at this location and between these hours: https://www.library.illinois.edu/enx/idea-lab/
3) Since the last person to use it probably didn't charge it, you should do so before attempting to use it
4) Factory reset it by holding the volume down and power buttons: https://www.youtube.com/watch?v=MRRDQxQ51NM
5) Perform the first-time setup with the headset and pair it with the software you chose in Step 4 of the environment setup above
6) Within that software, go to the settings and enable developer mode for the paired device
7) After rebooting, you can sideload the APK onto the headset with the next set of instructions below
8) When you are done with the headset, reset it and return it to the IDEA Lab during normal hours
9) If you forgot to reset it before turning it in, you can remove it from your Oculus account here: https://secure.oculus.com/my/devices/

## Sideloading the APK onto the Headset

1) Connect the headset to your PC with the USB cable
2) Open SideQuest
3) In the top-right corner, select "Install APK file from folder on computer"
4) Select the test APK to sideload
5) In the headset, navigate to your apps
6) Change from All apps to Unknown Source
7) Run the project from this folder

# How to test Unity project with Oculus Quest 2 and Virtual Display
For those who have already purchased Virtual Display and wanted to test a project wirelessly.  
This only works for Windows.  
Refer to this thread for more information: https://forum.unity.com/threads/unity-editor-and-virtual-desktop.881449/#post-6422274  

1.) Install SteamVR (if you have not)  
2.) Also install SideQuest and Virtual Display on your PC  
3.) Locate your Virtual Desktop folder (it should look similar to this: C:\Program Files\Virtual Desktop Streamer\VirtualDesktop.Streamer.exe)  
4.) Locate your Unity Editor folder (it should look similar to this: C:\Program Files\Unity\Hub\Editor\2019.4.20f1\Editor\Unity.exe)  
5.) Locate your POINT-VR Project folder. (it should look similar to this: C:\Users\POINT-VR-Chapter-1)  
**WARNING:** It is **VERY** important that your project folder location does not contain any whitespace. Otherwise it would not work. If your location happens to have whitespace in it (such as C:\Users\Your Name\POINT-VR-Chapter-1), I suggest moving POINT-VR-Chapter-1 folder to C:\Users.  
6.) Open Command Prompt and type in your command with the following syntax: "Virtual Dekstop Streamer Exe Path" "Unity Editor executable path" -projectpath "Unity project path -useHub -hubIPC -cloudEnvironment production"  
  So your command should look something like this: 
```
"C:\Program Files\Virtual Desktop Streamer\VirtualDesktop.Streamer.exe" "C:\Program Files\Unity\Hub\Editor\2019.4.20f1\Editor\Unity.exe" -projectpath "C:\Users\POINT-VR-Chapter-1 -useHub -hubIPC -cloudEnvironment production"
```
  It should open the Unity project directly. If it opens the Unity Hub, then you did something wrong.  
7.) Open Virtual Desktop in your Quest 2 and launch SteamVR.  
8.) Hit play in your Unity project and begin testing. 