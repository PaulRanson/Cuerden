Simple RPG Camera
Austin Zimmer
phatrobit@gmail.com
http://unity.phatrobit.com/

ReadMe

Description:
The goal of the Simple RPG Camera package is to provide an easy to implement, no hassle camera control script that is
packed with all the features you'd expect it to have and more. It's designed to work very well with RPG styled games
(a WoW or Diablo style), and can be easily integrated into projects of any size without making a mess.

SimpleRpgCamera.cs Instructions:
- Place "PhatRobit/Simple RPG Camera/Scripts/SimpleRpgCamera.cs" onto a camera object.
- Place the transform of the object you want to orbit into the 'Target Settings/Target' field, or type in the target's tag in the 'Target Settings/Target Tag' field.
- To rotate objects, place their Transform into the 'Rotation Settings/Objects To Rotate' field ('Rotate Objects' must be enabled).
- To fade objects, place their Renderer into the 'Zoom Settings/Objects To Fade' field (only works on transparent shaders, and 'Zoom Settings/Fade Objects' must be enabled).
- Adjust the other fields as needed.

SrpgcKeyboardMovementController.cs / SrpgcMouseMovementController.cs Instructions:
- Place "PhatRobit/Demos/SRPGCSources/Scripts/SrpgcKeyboardMovementController.cs OR SrpgcMouseMovementController.cs" onto a player object that has
an Animator, Rigidbody and Collider component.
- Adjust the fields as needed to match your Animator component.

Note: Instructions are the same for mobile scripts
Warning: When using the target offset option, it is possible that the camera will go through objects depending on the offset

Contact:
- Feel free to send me / post on the forum (http://unity.phatrobit.com/forum/) your suggestions, comments, questions or report any
problems you may have. Thanks and enjoy!
- phatrobit@gmail.com