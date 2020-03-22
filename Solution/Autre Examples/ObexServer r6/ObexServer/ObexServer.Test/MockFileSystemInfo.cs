using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.IO;
using System.Runtime.Remoting;
using System.Collections;
using System.Diagnostics;

namespace ObexServer.Tests
{

    public static class MockFileSystemInfo
    {
        public static FileInfo CreateFileInfo(String name, Int64 size,
            DateTime created, DateTime modified, DateTime accessed)
        {
            Hashtable values = new Hashtable();
            values.Add("get_Name", name);
            values.Add("get_Length", size);
            values.Add("get_LastWriteTime", modified);
            values.Add("get_LastAccessTime", accessed);
            values.Add("get_CreationTime", created);
            return PropertiesHashtableRealProxy.Create<FileInfo>(values);
        }

        public static DirectoryInfo CreatedDirectoryInfo(String name,
            DateTime created, DateTime modified, DateTime accessed)
        {
            Hashtable values = new Hashtable();
            values.Add("get_Name", name);
            values.Add("get_LastWriteTime", modified);
            values.Add("get_LastAccessTime", accessed);
            values.Add("get_CreationTime", created);
            return PropertiesHashtableRealProxy.Create<DirectoryInfo>(values);
        }

    }//class

    internal class PropertiesHashtableRealProxy : RealProxy
    {
        private IDictionary m_values;

        public static T Create<T>(IDictionary propertyValues)
            where T : MarshalByRefObject
        {
            RealProxy proxy = new PropertiesHashtableRealProxy(typeof(T), propertyValues);
            return (T)proxy.GetTransparentProxy();
        }

        private PropertiesHashtableRealProxy(Type targetType, IDictionary propertyValues)
            : base(targetType)
        {
            m_values = propertyValues;
        }

        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage callMsg = (IMethodCallMessage)msg;
            string memberName = callMsg.MethodName;
            IMethodReturnMessage ret;
            if (!m_values.Contains(memberName)) {
                ret = new ReturnMessage(new MissingMemberException(memberName), callMsg);
            } else {
                object result = m_values[memberName];
                ret = new ReturnMessage(result, null, 0, null, callMsg);
                Type returnType = ((System.Reflection.MethodInfo)callMsg.MethodBase).ReturnType;
                if (result == null) {
                    // Only can check not value type return type
                    Debug.Assert(!returnType.IsValueType,
                        String.Format(System.Globalization.CultureInfo.InvariantCulture,
                            "Return type incompatibility -- null value for ValueType: '{0}'.",
                                returnType));
                } else {
                    Debug.Assert(returnType.IsAssignableFrom(result.GetType()),
                        String.Format(System.Globalization.CultureInfo.InvariantCulture,
                            "Return type incompatibility -- wanted: '{0}', was '{1}'.",
                                returnType, result.GetType()));
                }
            }
            return ret;
        }
    }//class


    //---------------------------------------------------------------------
/*
    [TestFixture]
    public class PlayingProxy
    {
        [Test]
        public void View()
        {
            Stream orig = new MemoryStream();
            byte[]buf = {1,2,3,4,5,6,7};
            orig.Write(buf, 0, buf.Length);
            Stream proxy = PassThruRealProxy.Create(orig);
            Console.WriteLine("Length: " + proxy.Length);
            Console.WriteLine(proxy.ToString());
        }
    }

    class PassThruRealProxy : RealProxy
    {
        MarshalByRefObject m_target;

        public static T Create<T>(T target) 
            where T : MarshalByRefObject
        {
            RealProxy proxy = new PassThruRealProxy(target);
            return (T)proxy.GetTransparentProxy();
        }

        private PassThruRealProxy(MarshalByRefObject target)
            : base(target.GetType())
        {
            m_target = target;
        }

        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage callMsg = (IMethodCallMessage)msg;
            IMessage ret = RemotingServices.ExecuteMessage(m_target, callMsg);
            //
            Console.WriteLine("in : " + msg);
            Console.WriteLine("in : " + msg.GetType().AssemblyQualifiedName);
            // --> System.Runtime.Remoting.Messaging.Message
            Console.WriteLine("out: " + ret);
            Console.WriteLine("out: " + ret.GetType().AssemblyQualifiedName);   
            // -->System.Runtime.Remoting.Messaging.ReturnMessage
            //
            return ret;
        }
    }//class
*/

}//ns
