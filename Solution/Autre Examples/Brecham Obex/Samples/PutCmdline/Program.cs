using System;
using System.IO;
using InTheHand.Net.Sockets;
using Brecham.Obex;
using InTheHand.Net;    //e.g. BluetoothEndPoint

namespace PutCmdline
{
    class Program
    {
        static void PutFile(ObexClientSession sess, String filepath)
        {
            using (FileStream src = File.OpenRead(filepath)) {
                long lengthInBits = src.Length * 8;
                String name = Path.GetFileName(filepath);
                Console.WriteLine("Filename: {0}", filepath);
                Console.WriteLine("  Length = {0}", src.Length);
                //
                DateTime start = DateTime.UtcNow;
                sess.PutFrom(src, name, null, src.Length);
                TimeSpan ts = DateTime.UtcNow - start;
            double bps = ((lengthInBits / ts.TotalMilliseconds) * 1000);
            Console.WriteLine("  PutFrom took {0}, at an average {1} bps.",
                ts, bps);
        }//using
    }//fn


        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Command line syntax: [-bluetooth <device name>] filename [filename ...]");
                return;
            }
            //----------------------------------------------------------
            // Network Connect
            //----------------------------------------------------------
            int curArg = 0;
            IrDAClient irdaCli = null;
            BluetoothClient btCli = null;
            System.Net.Sockets.NetworkStream peerStream;
            //
            if (args.Length > 2 && args[0].Equals("-bluetooth"))
            {
                // Bluetooth
                String givenDeviceName = args[1];
                curArg += 2;
                btCli = new BluetoothClient();
                Console.WriteLine("Searching for Bluetooth device with name: "+ givenDeviceName);
                BluetoothDeviceInfo[] devices = btCli.DiscoverDevices();
                BluetoothAddress addr=null;
                foreach (BluetoothDeviceInfo curDevice in devices)
                {
                    if (curDevice.DeviceName.Equals(givenDeviceName))
                    {
                        addr = curDevice.DeviceAddress;
                    }
                }//for
                if (addr == null)
                {
                    Console.WriteLine(
                        "No Bluetooth device with the given name was found.");
                    return;
                }
                BluetoothEndPoint ep = new BluetoothEndPoint(addr,
                    InTheHand.Net.Bluetooth.BluetoothService.ObexObjectPush);
                btCli.Connect(ep);
                peerStream = btCli.GetStream();
            }
            else
            {
                try
                {
                    irdaCli = new IrDAClient("OBEX");
                }
                catch (InvalidOperationException ioex)
                {
                    System.Diagnostics.Debug.Assert(ioex.Message == "No device");
                    Console.WriteLine("No peer IrDA device found.");
                    return;
                }
                peerStream = irdaCli.GetStream();
            }
            //----------------------------------------------------------
            // OBEX Connect & Send(s)
            //----------------------------------------------------------
            try
            {
                if (curArg == args.Length)
                {
                    Console.WriteLine("No filenames given");
                    return;
                }
                ObexClientSession sess = new ObexClientSession(peerStream, UInt16.MaxValue);
                ObexHeaderCollection headers = new ObexHeaderCollection();
                //The number of files we're gonna send.
                headers.Add(ObexHeaderId.Count, args.Length);
                sess.Connect(headers);
                //----------------------------------------------------------
                // OBEX Send files
                //----------------------------------------------------------
                String curPath;
                for (int i = curArg; i < args.Length; ++i)
                {
                    // Send a file
                    curPath = args[i];
                    PutFile(sess, curPath);
                }
                //
            }
            finally
            {
                if (peerStream != null) peerStream.Close();
                if (irdaCli != null) irdaCli.Close();
                if (btCli != null) btCli.Close();
            }//try-finally
        }//fn--Main

    }//class
}
