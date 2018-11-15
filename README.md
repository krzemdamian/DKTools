# DKTools
## [Wiki (link)](https://github.com/krzemdamian/DKTools/wiki)
Go to wiki pages for more information on:
* plugin installation
* used programming technologies
* command descriptions

## Description
DKTools is a Revit plug-in written in C# run by Revit API.
Autodesk Revit is a Building Information Modeling program for the construction industry.
The extension adds custom commands and utilities to Revit which ease designing.
Most of the commands execute python scripts interpreted by python script engines.

## Genesis
The idea for this plug in derived from Dynamo which is visual programming environment for Revit.
In Dynamo it is possible to create custom nodes which execute actions written in python.
Unfortunately, it's not as convenient as Revit commands.
Luckily it's possible to run IronPython scripts directly in C# code.
This mechanism is used in DKTools so that python scripts are invoked in Revit through script engine.
All Revit API objects used by add-in can be passed to python scripts.
