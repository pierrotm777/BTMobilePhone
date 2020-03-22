'Copyright (c) 2005,
'   HeSicong of UESTC, Dreamworld Site(http://www.hesicong.com), All rights reserved.

'Redistribution and use in source and binary forms, with or without modification, 
'are permitted provided that the following conditions are met:

'1.Redistributions of source code must retain the above copyright notice, this list
'  of conditions and the following disclaimer. 
'2.Redistributions in binary form must reproduce the above copyright notice, this
'  list of conditions and the following disclaimer in the documentation and/or other 
'  materials provided with the distribution. 
'3.Neither the name of the Dreamworld nor the names of its contributors may be 
'  used to endorse or promote products derived from this software without specific prior
'  written permission. 
'  
'THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS
'OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
'AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
'CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
'DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
'DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER
'IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY 
'OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

Imports System.IO
Imports System.Threading
Imports System.Reflection
Imports System.Collections

Namespace Dreamworld.FileTransfer.SDK
    Public Class PluginServices
        Public Structure AvailablePlugin
            Public AssemblyPath As String
            Public ClassName As String
        End Structure

        ''' <summary>
        ''' Find a plug-in with given interface name.
        ''' </summary>
        ''' <param name="strPath">Path to search. Subdirs will not search.</param>
        ''' <param name="strInterface">Interface name</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function FindPlugins(ByVal strPath As String, ByVal strInterface As String) As AvailablePlugin()
            Dim Plugins As ArrayList = New ArrayList()
            Dim strDLLs() As String, intIndex As Integer
            Dim objDLL As [Assembly]

            'Go through all DLLs in the directory, attempting to load them
            strDLLs = Directory.GetFileSystemEntries(strPath, "*.dll")
            For intIndex = 0 To strDLLs.Length - 1
                Try
                    objDLL = [Assembly].LoadFrom(strDLLs(intIndex))
                    ExamineAssembly(objDLL, strInterface, Plugins)
                Catch e As Exception
                    'Error loading DLL, we don't need to do anything special
                End Try
            Next

            'Return all plugins found
            Dim Results(Plugins.Count - 1) As AvailablePlugin

            If Plugins.Count <> 0 Then
                Plugins.CopyTo(Results)
                Return Results
            Else
                Return Nothing
            End If
        End Function

        Private Shared Sub ExamineAssembly(ByVal objDLL As [Assembly], ByVal strInterface As String, ByVal Plugins As ArrayList)
            Dim objType As Type
            Dim objInterface As Type
            Dim Plugin As AvailablePlugin

            'Loop through each type in the DLL
            For Each objType In objDLL.GetTypes
                'Only look at public types
                If objType.IsPublic = True Then
                    'Ignore abstract classes
                    If Not ((objType.Attributes And TypeAttributes.Abstract) = TypeAttributes.Abstract) Then

                        'See if this type implements our interface
                        objInterface = objType.GetInterface(strInterface, True)

                        If Not (objInterface Is Nothing) Then
                            'It does
                            Plugin = New AvailablePlugin()
                            Plugin.AssemblyPath = objDLL.Location
                            Plugin.ClassName = objType.FullName
                            Plugins.Add(Plugin)
                        End If

                    End If
                End If
            Next
        End Sub

        Public Shared Function CreateInstance(ByVal Plugin As AvailablePlugin) As Object
            Dim objDLL As [Assembly]
            Dim objPlugin As Object

            Try
                'Load dll
                objDLL = [Assembly].LoadFrom(Plugin.AssemblyPath)

                'Create and return class instance
                objPlugin = objDLL.CreateInstance(Plugin.ClassName)
            Catch e As Exception
                Return Nothing
            End Try

            Return objPlugin
        End Function

        ''' <summary>
        ''' Select plugin from Model ID
        ''' </summary>
        ''' <param name="ap">AvaliablePlugin</param>
        ''' <param name="baudrate">Speed</param>
        ''' <param name="port">Port name like "COM1:"</param>
        ''' <param name="CommPhoneClassName">The name of commphone class.</param>
        ''' <returns>Plugin number in AvialiablePlugin</returns>
        ''' <remarks></remarks>
        Public Shared Function SelectPlugin(ByVal modelID As String, ByVal ap As PluginServices.AvailablePlugin()) As Integer
            If ap Is Nothing Then
                Throw New Exception("Not plugin found")
            End If

            'Loop every plugin to find supported device.
            For i As Integer = 0 To ap.Length - 1
                Dim objPlugin As IPhonePlugIn
                objPlugin = DirectCast(PluginServices.CreateInstance(ap(i)), IPhonePlugIn)
                Dim supportedModel As String() = objPlugin.SupportModelID
                For j As Integer = 0 To supportedModel.Length - 1
                    If ModelID = supportedModel(j) Then
                        Return i
                    End If
                Next
            Next
            'If not found
            Throw New NotSupportedException("Your phone are not supported")
        End Function

        Private Shared Function SendAT(ByVal port As IO.Ports.SerialPort, ByVal cmd As String) As String
            Dim mATTimeOut As Integer = 500
            port.NewLine = Chr(13)
            port.WriteLine(cmd)
            Dim response As New Text.StringBuilder
            Console.ForegroundColor = ConsoleColor.White
            Dim start As Date = Now
            Do
                Thread.Sleep(10)
                Dim rsp As String = port.ReadExisting
                response.Append(rsp)
                If Now.Subtract(start).TotalMilliseconds > mATTimeOut Then Throw New TimeoutException("AT TimeOut")
                If response.ToString.Contains("OK") Or response.ToString.Contains("ERROR") Then Exit Do
            Loop
            Return response.ToString
        End Function

    End Class
End Namespace