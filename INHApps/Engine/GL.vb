Option Explicit On
Option Strict On
Imports ATLib
Imports System.IO
Imports System.Runtime.Remoting
Imports InDesign
'***************************************************************************************
'*
'* GL - Engine global data
'*
'* AUDIT TRAIL
'* 
'* V1.000   25-OCT-2007  BA  Original
'*
'* front end driver for InDesign
'*
'*
'*
'*
'***************************************************************************************
Module GL
    Friend Engine As ATLib.Engine
    Friend Sys As ATSystem
    Friend INDService As EngineLib.INDService
End Module
