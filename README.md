# Unity visualization for Emika Franka Panda

## Summary
Unity project with Emika Franka Panda and IK.

## Introduction
This is a repository for files needed to run example application for Emika Franka Panda with analytical inverse kinematics. Project includes DLL of the analytical inverse kinematics for Franka Panda Robot. DLL project is availible on https://github.com/jabolcni/franka_analytical_ik_DLL. Method for analytical inverse kinematics was developed by Yanhao He and Steven Liu, you can find their repository on https://github.com/ffall007/franka_analytical_ik/blob/main/README.md. 

Meshes for the Franka Panda robot have been obtained from Franka Emika Panda Simulator (https://github.com/FirefoxMetzger/panda_gazebo_sim). Kinematics of Franka Panda model follows the kinematic DH model in https://frankaemika.github.io/docs/control_parameters.html#denavithartenberg-parameters. 

## Use
You can either:
1. copy all files into Assests directory of new fresh Unity project or 
2. copy just Unity package file and import it.

In both cases you will get folders:
- `meshes`: contains meshes for Frankla Panda robot copied from https://github.com/FirefoxMetzger/panda_gazebo_sim.
- `Plugins`: Contains DLL of IK code. DLL contains two versions of IK function, one with float data types and other with double data types for joint variables and end-effector transformation matrix. 
- `Scenes`: Samples scene.
- `Scripts`: Contains two C# scripts, one for double data type and one for float data type versions of IK function. C# script is added to *Panda_Base* gameobject, which is the root object of the Franka Panda model.  

Open scene *SampleFrankaPandaScene.unity*. It contains robot and a target object. If you run the scene and move the target object robot will follow the target object.