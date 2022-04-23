# Instructions for building the APK file

## Building for the First Time

1) Open Unity Hub
2) Go to Installs
3) Go to version 2019.4.20f1, the version used for our project
4) Add modules
5) Make sure that Android Build Support is checked
6) Expand the dropdown under Android Build Support
7) Ensure that Android SDK & NDK Tools and OpenJDK are checked
8) Install these if necessary
9) Open the project in the editor
10) Open Build Settings (File->Build Settings...)
11) Under Platform, select Android
12) Click the button on the bottom right of the build window to Switch Platform
13) Click the button on the bottom left of the build window to open Player Settings
14) In the Android tab, go to Other Settings
15) Ensure the Minimum API Level is set to Android 7.0 'Nougat' (API Level 24)
16) Ensure the Target API Level is set to Automatic (highest installed)
17) Close Other Settings and open Publishing Settings
18) Ensure the custom keystore is set to the path: ProjectSettings/pointvr.keystore
19) Enter the password (it is pinned in the #group1 channel on our discord)
20) Ensure the project key alias is set to pointvr
21) Enter the password again (same one)
22) In the project settings (Edit->Project Settings...), open the XR Plug-In Management menu
23) In the Android tab, ensure that Oculus is enabled
24) Reopen the build settings
25) Set the Run Device to All compatible devices
26) Select Build
27) Ensure that you are placing your build in the Builds folder of the project folder

## Building for Every Time After

1) If this project was kept open in the editor since the last build, you may skip to Step 5
2) In the Build Settings, open Player Settings
3) Open Publishing Settings
4) Put in the keystore password (twice)
5) Open Build Settings
6) Select Build
7) Ensure that you are placing your build in the Builds folder of the project folder
