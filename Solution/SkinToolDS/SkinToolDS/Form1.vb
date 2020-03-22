Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Management


Namespace Temperature
	Public Partial Class MainForm
		Inherits Form
		Public Sub New()

			InitializeComponent()
		End Sub

		Private localtemp As New temperature()

		Private Function GetCpuTemp() As String
			Dim result As String = [String].Empty
			Dim rawdata As String = [String].Empty
			Dim temp As [Double]
			Dim searcher As New ManagementObjectSearcher("root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature")

			Dim enumerator As ManagementObjectCollection.ManagementObjectEnumerator = searcher.[Get]().GetEnumerator()

			While enumerator.MoveNext()
				Dim x As Integer
				Dim tempObject As ManagementBaseObject = enumerator.Current

				temp = Convert.ToDouble(tempObject("CurrentTemperature").ToString())
				temp = (temp - 2732) / 10.0

				result = " " + (1.8 * temp + 32).ToString() + " F"
			End While
			searcher.Dispose()
			Return result

		End Function

		Private Sub MainForm_Load(sender As Object, e As EventArgs)


			timer1.Interval = 1000
			timer1.Start()

		End Sub

		Private Sub MainForm_Click(sender As Object, e As EventArgs)
			Me.Close()
		End Sub

		Private Sub timer1_Tick(sender As Object, e As EventArgs)
			'label1.Text = localtemp.cputemperature;

			label1.Text = CPUTempFromOH()
			Me.Text = label1.Text
		End Sub
		' From The Web
		' using OHML.Dll
		Private Function CPUTempFromOH() As String

			Dim result As String = [String].Empty
			Dim computerHardware As New OpenHardwareMonitor.Hardware.Computer()
			computerHardware.Open()
			computerHardware.CPUEnabled = True
			Dim temps = New List(Of Decimal)()
			For Each hardware As var In computerHardware.Hardware
				If hardware.HardwareType <> OpenHardwareMonitor.Hardware.HardwareType.CPU Then
					Continue For
				End If
				hardware.Update()
				For Each sensor As var In hardware.Sensors
					If sensor.SensorType = OpenHardwareMonitor.Hardware.SensorType.Temperature Then
						If sensor.Value IsNot Nothing Then
							temps.Add(CDec(sensor.Value))
						End If
					End If
				Next
			Next
			Dim maxTemp = If(temps.Count = 0, 0, temps.Max())
			result = " " + ((Convert.ToDecimal(1.8) * maxTemp) + 32).ToString() + " F"
			'result = " " + ((maxTemp)).ToString() + " C";
			'result =  maxTemp.ToString();
			Return result


		End Function
		' temperature class
		' ver 1 01-29-2014
		' Reports 2 WMI Temps which do not appear to be current, compared to OH Monitor
		Public Class temperature
			Public Sub New()
			End Sub
			Public Property systemtemperature() As String
				Get
					GetTemperatures()
					Return _systemtemperature
				End Get
				Set
				End Set
			End Property
			Public Property cputemperature() As String
				Get
					GetTemperatures()
					Return _cputemperature
				End Get
				Set
				End Set
			End Property
			Public Property numberoftemperatures() As Integer
				Get
					Return _numberoftemperatures
				End Get
				Set
				End Set
			End Property

			Private _systemtemperature As String
			Private _cputemperature As String
			Private _numberoftemperatures As Integer
			Private Sub GetTemperatures()
				Dim result As String = [String].Empty
				Dim temp As [Double]
				Dim count As Integer = 0
				Dim searcher As New ManagementObjectSearcher("root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature")

				Dim enumerator As ManagementObjectCollection.ManagementObjectEnumerator = searcher.[Get]().GetEnumerator()

				While enumerator.MoveNext()
					Dim tempObject As ManagementBaseObject = enumerator.Current
					temp = Convert.ToDouble(tempObject("CurrentTemperature").ToString())
					temp = (temp - 2732) / 10.0
					If count = 0 Then
						_cputemperature = " " + (1.8 * temp + 32).ToString() + " F"
					End If
					If count = 1 Then
						_systemtemperature = " " + (1.8 * temp + 32).ToString() + " F"
					End If

					count += 1
				End While
				searcher.Dispose()
			End Sub

		End Class
	End Class
End Namespace
