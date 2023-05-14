# TSK First-Person Character Controller

TSK First-Person Character Controller is a powerful and versatile tool for creating immersive first-person experiences in your Unity projects.

This asset provides a customisable and easy-to-use character controller, built using the Cinemachine and Input System packages for Unity 2021.3+.

With support for various control schemes and actions, compatibility with URP/HDRP Render Pipelines, and presets available via the Samples section in PackageManager, TSK First Person Character Controller is a comprehensive solution for game developers and enthusiasts who want to create high-quality first-person gameplay.

In this documentation, we will explore the features of the TSK First Person Character Controller, how to use it in your projects, and how to troubleshoot common issues.

## PURPOSE

TSK First Person Character Controller aims to provide developers with a ready-to-use base for creating immersive first-person experiences in Unity. Using this package saves time and effort in creating a custom character controller from scratch, allowing you to focus on other aspects of your project, such as game mechanics, level design, and aesthetics.

## FEATURES

-   Control Schemes Supported:
    -   Keyboard and Mouse
-   Actions:
    -   Walk
    -   Sprint
    -   Jump
    -   Crouch
-   Compatibility with URP/HDRP Render Pipelines
-   Presets available via the Samples section in PackageManager
-   Settings configured with ScriptableObject for better management of the first-person controller

## IMPORTANT NOTES ON PACKAGE DEPENDENCIES

It's important to note that the TSK First Person Character Controller asset has several package dependencies that must be installed to function correctly. These dependencies include:

-   Cinemachine
-   Input System
-   Physics

Please install these packages in your Unity project before importing the TSK First Person Character Controller asset. If you encounter any issues with the asset, double-check that all required dependencies are installed and up to date.

## SWITCHING INPUT SYSTEMS

If you want to use the Input System and Input Manager (Old) together or switch your project back to the old Input Manager, you must go to Edit > Project Settings, then select Player.

Under Other settings, you will see that Input System Package (New) is selected. Here you can switch to your preferred setup. Please remember that the TSK First Person Character Controller does not work with the Input Manager (Old) setting.

## USING THE TSK FIRST PERSON CHARACTER CONTROLLER

-   To use the TSK First Person Character Controller in your game, import the asset into your Unity project.
-   After importing the package, you should see a new folder called "TSK First Person Character Controller" in your project hierarchy. This folder contains all the necessary scripts and assets to use the TSK First Person Character Controller in your game.
-   To add the character controller to your scene, drag and drop the "Player" prefab from the "Prefabs" folder in the TSK First Person Character Controller folder into your scene.
-   You can customise the character controller settings through the Inspector panel, with the FirstPersonControllerSettings located in the Settings folder providing access to a range of adjustable values. It is worth noting that the Ground Layer is set to Default by default. However, users may customise this layer to suit their specific needs.

## ADDING A NEW INPUT ACTION

You must modify the "FirstPersonInputs" script to add a new input action. Here are the steps you can follow:

1. Open the "FirstPersonInputs" script in your code editor.
2. Locate the "Input Values" region in the script. This is where all the input values for the character controller are defined.
3. Define a new input value by adding a new line of code in the "Input Values" region. For example, if you want to add a new input value for "Interact", you can add the following code:  
![Picture1](https://github.com/TheSleepyKoala/com.tsk.ess.fpcc/assets/59434446/66346b92-ef34-4025-a077-a43d7d80f576)
4. Create a new method to handle the input action. For example, to handle the "Interact" input action, you can add the following code:  
![Picture2](https://github.com/TheSleepyKoala/com.tsk.ess.fpcc/assets/59434446/71148c9b-d353-402d-b8fb-6349de0a0db4)
6. Open the “FirstPersonControls” in
putactions in the Scripts folder and add an action called “Interact” in the InputActions window and assign a key binding. (Make sure to click Save Asset)
6. Open the Player prefab in the “Prefabs” folder, click the “Events” dropdown, followed by the Player dropdown.
7. Here, we need to press the + icon on the Interact Event,  
![Picture3](https://github.com/TheSleepyKoala/com.tsk.ess.fpcc/assets/59434446/7573ed94-7267-444f-98e6-97f864fa4388)
8. Drag into the Object field our Player prefab.  
![Picture4](https://github.com/TheSleepyKoala/com.tsk.ess.fpcc/assets/59434446/09d81763-d90c-4419-bd14-207e68c83edb)
9. Click the “No Function” dropdown, and navigate to our “OnInteract” CallbackContext, and select "InteractInput" from the dropdown.  
![Picture5](https://github.com/TheSleepyKoala/com.tsk.ess.fpcc/assets/59434446/b876192e-ff04-4e46-9aa8-060f5369d21a)
 
10. That's it! You have successfully added a new input action for the TSK First Person Character Controller.

## URP/HDRP COMPATIBILITY

By default, the materials use the Built-in Render Pipeline. If you want to use the First-Person Character Controller in URP or HDRP, you must upgrade the materials.

Edit -> Rendering -> Materials -> Convert Selected Built-in materials to URP/HDRP

## TROUBLESHOOTING

Here are some common issues and their potential solutions when using the TSK First Person Character Controller asset:

### Character movements are not smooth or responsive:

-   Solution: Check the input settings in the project to ensure that the controller receives input correctly. Also, ensure the game’s framerate is consistent and high enough to provide smooth movement.

### The character falls through the ground or colliders:

-   Solution: Check the colliders on the ground and any other objects in the scene to ensure they are correctly configured and set up to interact with the character controller. Also, check the physics settings in the project to ensure that the character's gravity and collision detection are set up correctly.

### The camera view is distorted or not following the character properly:

-   Solution: Check the camera settings in the project to ensure that they are correctly set up and configured to follow the character. Also, ensure that camera or post-processing effects do not interfere with the camera view.

### The character is not jumping or performing other actions:

-   Solution: Check the input settings in the project to ensure that the correct input is being received for the desired action. Also, check the character controller settings that necessary actions are enabled and set up correctly.

### The asset is not working at all:

-   Solution: Check that the asset is installed correctly and imported into the project. Also, ensure that any necessary dependencies or plugins are correctly installed and configured. If all else fails, contact the asset developer for further assistance.

## TECHNICAL DOCUMENTATION

For further elucidation on the intricacies of the distinct scripts, we invite you to consult the self-contained annotations provided therein. Do bear in mind, however, that numerous methods are characterised by their succinct and descriptive names, rendering their functions readily apparent.
