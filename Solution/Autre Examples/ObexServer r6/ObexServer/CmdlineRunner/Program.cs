using System;
using System.Collections.Generic;
using System.Text;
//
using System.Net.Sockets;

namespace CmdlineRunner
{
    class Program
    {
        /* TODO internal struct ProtoRow
        internal struct ProtoRow
        {
            public String name;
            public ListenProtocol protocolId;

            public ProtoRow(String name, ListenProtocol protocolId)
            {
                this.name = name;
                this.protocolId = protocolId;
            }
        }

        ProtoRow[] ProtocolOptionTable = new ProtoRow[]{
            new ProtoRow("irda", ListenProtocol.Irda),
        };
        */

        static void Main(string[] args)
        {
            //-CreateBigFiles();
            //-DumpTraceListeners();
            //
            //
            ListenProtocol proto;
            if (args.Length != 1) {
                usage();
                return;
            }
            bool bluetoothFtp = false;
            if ("-irda".Equals(args[0], StringComparison.OrdinalIgnoreCase)) {
                proto = ListenProtocol.Irda;
            } else if ("-bluetooth".Equals(args[0], StringComparison.OrdinalIgnoreCase)) {
                proto = ListenProtocol.Bluetooth;
                bluetoothFtp = true;
            } else if ("-bluetoothOPP".Equals(args[0], StringComparison.OrdinalIgnoreCase)) {
                proto = ListenProtocol.Bluetooth;
                bluetoothFtp = false;
            } else if ("-tcpip".Equals(args[0], StringComparison.OrdinalIgnoreCase)) {
                proto = ListenProtocol.Ipv4;
            } else if ("-tcpip6".Equals(args[0], StringComparison.OrdinalIgnoreCase)) {
                proto = ListenProtocol.Ipv6;
            } else if ("-tcpip64dual".Equals(args[0], StringComparison.OrdinalIgnoreCase)) {
                proto = ListenProtocol.Ipv6And4Dual;
            //} else if ("-tcpip64pair".Equals(args[0], StringComparison.OrdinalIgnoreCase)) {
            //    proto = ListenProtocol.Ipv6And4Pair;
            } else {
                usage();
                return;
            }
            //--------
            NetworkServer serverInstance = new NetworkServer();
            bool useSync = false;
            serverInstance.HackUseSync = useSync;
            serverInstance.NetworkProtocol = proto;
            serverInstance.BluetoothFtpProfile = bluetoothFtp;
            serverInstance.Start();
        }

        private static void usage()
        {
            Console.WriteLine("(-irda | -bluetooth | -bluetoothOPP | -tcpip | -tcpip6 | -tcpip64dual)");
        }


        //----------------------------------------------------------------------
        private static void DumpTraceListeners()
        {
            foreach (System.Diagnostics.TraceListener cur in System.Diagnostics.Trace.Listeners) {
                Console.WriteLine(cur.GetType().Name + " " + cur.Name + " " + cur.TraceOutputOptions);
            }
        }

        private static void CreateBigFiles()
        {
            long size;
            //
            size = 2 * 1024 * 1024;
            CreateFileOfLength("2MB.bin", size);
            //
            size = 2L * 1024 * 1024 * 1024;
            CreateFileOfLength("2GB.bin", size);
            //
            size = 2147483749;
            CreateFileOfLength("2blahblah.bin", size);
            //
            size = 0x80000000;
            CreateFileOfLength("0x80000000.bin", size);
            //
            size = 0xffffffff;
            CreateFileOfLength("0xffffffff.bin", size);
            //
            Console.WriteLine ("Done CreateBigFiles");
        }

        private static void CreateFileOfLength(string filename, long size)
        {
            using (System.IO.FileStream strm = System.IO.File.Create(filename)) {
                strm.SetLength(size);
            }
            Console.WriteLine("Created file '{0}' of length {1} (0x{1:X})", filename, size);
        }

    }//class
}
