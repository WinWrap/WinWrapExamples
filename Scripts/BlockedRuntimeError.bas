﻿'#Language "WWB.NET"

Imports System.Text

Dim WithEvents anincident1 As Incident = TheIncident

Private Sub anincident1_Started() Handles anincident1.Started
    anincident1.FilledInBy = ScriptName()
    Dim sb As New StringBuilder
    sb.Append("Hello")
    Dim o As Object = sb.GetType
    ' force an unsafe runtime error
    Dim a As Object = o.Assembly
End Sub
