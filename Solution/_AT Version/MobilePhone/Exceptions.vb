
Imports System.Collections.Generic
Imports System.Text

Namespace SMSPDULib
	Class UnknownSMSTypeException
		Inherits Exception
		Public Sub New(pduType As Byte)
			MyBase.New(String.Format("Unknow SMS type. PDU type binary: {0}.", Convert.ToString(pduType, 2)))
		End Sub
	End Class
End Namespace