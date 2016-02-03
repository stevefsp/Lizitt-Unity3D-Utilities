# Lizitt Unity3D Utilities

## Overview

This project implements various useful utilities for [Unity3D](http://unity3d.com/unity) projects.

API Status: Pre-release Alpha  
Test Status: Beta  
Language: C#  
Target: Unity3D 5.0+ (Personal & Professional)  
Lastest release tested in: Unity3D 5.3.1 Professional

## Installation

Download the project and drop the contents of the `Source` directory into the Unity Assets folder.  No pre-built DLLs are planned for this project.

### Notes

The directory structure is organized to allow you to more easily pick and choose the 'modules' you want, with the directory names indicating dependancies.  If you don't want a module, just don't copy its directory to your project.  For example:  Core has no dependancies.  CoreComponents is dependant on Core. 

The default location for the code is in the `Plugins` folder.  This improves Unity Editor compile times and makes the utility features available to all Unity scripts.  The location in `Plugins` is not a requirement.

## Documentation

There is no online documentation at this point.  But all code is documented using standard C# XML comments.

## Miscellaneous

All members are located under the `com.lizitt` namespace.



