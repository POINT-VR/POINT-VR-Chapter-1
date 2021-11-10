# How to get a device

## The IDEA Lab
Any student can borrow a quest from the IDEA Lab. Here is the link to request equipment:  https://uiuc.libcal.com/equipment?lid=5488  
The IDEA Lab also has computers, Oculus Rifts, and Oculus Quests available.

# Building Guide for Unity and Oculus Devices. How to create the APK file.

## Oculus Quest 2
1.) Go to Build Settings
2.) Click Android
3.) Switch Platform
4.) After platform has switched, go to Player Settings.
5.) In Player Settings -> Under Android go to Other Settings.
6.) Make sure the minimum Android API is 7.0 and that the target API is the latest Android API
7.) Back in Build Settings, in Android switchs to be compatible with all available devices
8.) Build
9.) Build and Run if Quest is connect via USB?

To get it to run on the Oculus Device will most likely require sideloading the .apk file onto the Oculus Device. And with the Oculus device in developer mode, one should find the apk file in the unknown sources area and should be able to open it.

Side Notes:
1.) Make sure in Project Settings, under XR Management, that Oculus support is enabled.

# How to get the APK file working on an Oculus Device
This requires sideloading the APK file onto the Oculus Device
Other Guides: https://uploadvr.com/sideloading-quest-how-to/ , https://sidequestvr.com/setup-howto

## Oculus Quest 2
1.) Get Sidequest
2.) Connect to the Oculus Device either on your PC OR phone.
  For Windows PC: Download Oculus Developer Hub
  For Phone: Pair Quest headset, with the Oculus App on your phone (available in the AppStore or PlayStore)
             - Make sure that the Headset and the App are on the same Wi-Fi network so that they can find each other.
             - If the Pairing code is not appearing on the headset, the headset may need to be factor reset. To preform a factory reset, turn off the headset and press the power and volume down buttons. This should open a menu to factory reset the device.
3.) If on a Windows PC, download the Oculus Drivers: https://developer.oculus.com/downloads/package/oculus-go-adb-drivers/.
4.) Connect Quest headset to via USB cable
5.) Once the Quest is connected to your PC or phone, enable Developer Mode and reboot the device
