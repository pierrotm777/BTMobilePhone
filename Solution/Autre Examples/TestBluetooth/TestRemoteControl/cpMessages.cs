using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace cPOSdotNet.Core.RemoteControl
{
    public class ServerMessages
    {
        public const string SM_ACKNOWLEDGEOK = "OK";
        public const string SM_ACKNOWLEDGECANCEL = "CANCEL";
        public const string SM_DISCONNECT = "BYE";
    }
    public class ClientMessages
    {
        public const string CM_REQUESTCONNECT = "HELLO";
        public const string CM_DISCONNECT = "BYE";        
    }

    public enum enuMessageType
    {
        emtUndefined = -1,
        emtRequestEvent = 0,
        emtRequestBinding = 1,
        emtResponceBinding = 2,
        emtResponceDefinition = 3
    }

    [Serializable]
    [XmlRoot("cposmessage")]
    [XmlInclude(typeof(cRequestExecuteEvent)),
     XmlInclude(typeof(cRequestBinding)),
     XmlInclude(typeof(cResponceBinding)),
     XmlInclude(typeof(cSkinDefinition))]
    public abstract class cPOSMessage
    {
        public abstract enuMessageType MessageType { get; }
    }

    [Serializable]
    public class cRequestExecuteEvent : cPOSMessage
    {
        public override enuMessageType MessageType { get { return enuMessageType.emtRequestEvent; } }
        public string Version;
        public string Function;
        public string Value;
    }

    [Serializable]
    public class cRequestBinding : cPOSMessage
    {
        public override enuMessageType MessageType { get { return enuMessageType.emtRequestBinding; } }
        public string Version;
        public string Function;
        public string Key;
        public string Element;
    }

    [Serializable]
    public class cResponceBinding : cPOSMessage
    {
        public override enuMessageType MessageType { get { return enuMessageType.emtResponceBinding; } }
        public string Version;
        public string Function;
        public string Key;
        public string Element;
        public string ValueType;
        public object Value;
    }

    [Serializable]
    public class cSkinDefinition : cPOSMessage
    {
        public override enuMessageType MessageType { get { return enuMessageType.emtResponceDefinition; } }
        public string Version;
        public object Value;
    }
}

