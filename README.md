# Lizitt Unity3D Utilities

## Notice

**2016-03-18:** Released v0.2.0.  With this release the main branch will no longer be treated as a development branch and breaking changes will only occur in major versions (0.x).  For now, bug fixes and new features will still be added between releases with the changes aggregated periodically into minor releases (0.x.x).

## Overview

**API Status:** Pre-release Alpha  
**Test Status:** Beta  
**Language:** C#  
**Platform:** [Unity3D](http://unity3d.com/unity) 5.3+ (Personal & Professional)

## Installation

Download the project and drop the contents of the `Source` directory into the Unity Assets folder.

### Notes

The directory structure is organized to allow you to more easily pick and choose the 'modules' you want, with the directory names indicating dependancies.  If you don't want a module, just don't copy its directory to your project.  For example:  Core has no dependancies.  CoreComponents is dependant on Core. 

The default location for the code is in the `Plugins` folder.  This improves Unity Editor compile times and makes the utility features available to all Unity scripts.  The location in `Plugins` is not a requirement.

## Documentation

There is no online documentation at this point.  But all code is documented using standard C# XML comments.

## Miscellaneous

All members are located under the `com.lizitt` namespace.



