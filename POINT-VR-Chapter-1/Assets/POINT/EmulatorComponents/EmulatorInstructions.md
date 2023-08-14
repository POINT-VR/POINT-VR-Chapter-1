# Emulator
## Setting it up
Setting up the emulator is easy! Everything should come ready to use out of the box. You simply need to connect the PlayerEmulator prefab to the Player Spawner Script.
If something doesn't work then check that all the scripts are connected to the right objects. If you still have issues feel free to reach out to George Bayliss for help.

## Controls
Movement:
 * WASD for motion in the player's xz plane 
 * 1 and Q for motion in the true y-direction
 * Arrow keys for player rotation
 * R resets vertical rotation

Laser:
 * Laser direction is controlled by the mouse
 * Mouse click will interact with UI and toggle grab when hovered over a grabbable
 * Scroll wheel to push and pull

 * P opens the pause menu

These controls are the ones which I have found to be most convenient, however feel free to change any of the bindings for your own ease of use.

## General notes
The purpose of the emulator is foremost to be easy and comfortable to use. There are some unavoidable imperfections such as the perspective making it hard to see where the laser is, or that the controls (especially the mouse) can feel clunky at times, however for the most part it's should feel smooth and intuitive.
These controls are seperate from the XR Controller and don't interface with it at all so as new functionality is added to scenes it may not always function with the Emulator.
All the necessary scripts and most of the necessary assets are contained within the EmulatorComponents folder.
All the scripts are fully commented. 

Last updated 7/11/23