using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using OpenNETCF;
using OpenNETCF.Net;
using OpenNETCF.Net.Bluetooth;
using OpenNETCF.Net.Sockets;

namespace WindowsApplication3
{
    public partial class Form1 : Form
    {
        BluetoothDeviceInfo[] bdi;
        string BTMAC;
        string BTName;
        OpenNETCF.Net.BluetoothAddress btaddress;
        OpenNETCF.Net.Sockets.BluetoothClient client;
        OpenNETCF.Net.BluetoothEndPoint endpoint;
        NetworkStream stream;

        public Form1()
        {
            InitializeComponent();
        }


        //Looking for devices in the area
        private void discover()
        {
            //discover devices
            addtolog("Discovering Devices...");

            BluetoothClient bc = new BluetoothClient();
            bdi = bc.DiscoverDevices(5);

            comboBox1.DataSource = bdi;
            comboBox1.DisplayMember = "DeviceName";

            addtolog("Done, " + bdi.Length + " Device(s) found");
        }

        private void addtolog(string msg)
        {
            textBox2.Text = textBox2.Text + msg + "\r\n";
            Application.DoEvents(); // update display
        }

        //DISCOVER
        private void button1_Click(object sender, EventArgs e)
        {
            discover();
        }

        //TEXT BOX
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        //COMBO BOX
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BTMAC = bdi[comboBox1.SelectedIndex].DeviceID.ToString();
            BTName = bdi[comboBox1.SelectedIndex].DeviceName.ToString();

            Application.DoEvents(); // update display
        }

        //CONNECT BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            BTMAC = bdi[comboBox1.SelectedIndex].DeviceID.ToString();
            BTName = bdi[comboBox1.SelectedIndex].DeviceName.ToString();

            if (OBEXOpenStream(BTMAC))
            {

                if (OBEXConnect())
                {
                    addtolog("Connected");
                }
                else
                {
                    textBox2.Text = textBox2.Text + "Unable to connect\r\n";
                }

                OBEXCloseStream();
            }
            else
            {
                addtolog("Failed to connect to OBEX Server");
            }
        }


        //SEND BUTTON
        private void button3_Click(object sender, EventArgs e)
        {
            //Send a file
            addtolog("Sending File...");

            if (OBEXOpenStream(BTMAC))
            {
                if (OBEXConnect())
                {
                    //send client request, start put
                    string tName = "HelloWorld.txt";
                    string tType = "";
                    // string tFileContent = "Hi this is a test!!";
                    string tFileContent = OpenFile();

                    int result = OBEXRequest("PUT", tName, tType, tFileContent);

                    switch (result)
                    {
                        case 160: // 0xa0
                            addtolog("OK");
                            break;

                        case 197: // 0xc5
                            addtolog("Method not allowed");
                            break;

                        case 192: // 0xc0
                            addtolog("Bad Request");
                            break;

                        default:
                            addtolog("Other Error");
                            break;
                    }
                }

                OBEXCloseStream();
            }
            else
            {
                addtolog("Failed to connect to OBEX Server");
            }
        }

        /*************************************************************************************/
        private bool OBEXOpenStream(string BTMAC)
        {
            // serial port UUID
            Guid spguid = OpenNETCF.Net.Bluetooth.BluetoothService.ObexObjectPush;
            btaddress = OpenNETCF.Net.BluetoothAddress.Parse(BTMAC);
            client = new OpenNETCF.Net.Sockets.BluetoothClient();
            // define endpoint
            endpoint = new OpenNETCF.Net.BluetoothEndPoint(btaddress, spguid);

            try
            {
                //open socket
                client.Connect(endpoint);
            }
            catch (System.Exception e)
            {
                //unable to connect (server not listening on spguid)
                return false;
            }

            //connect socket
            stream = client.GetStream();
            return true;
        }

        private void OBEXCloseStream()
        {
            stream.Close();
            client.Close();
        }

        private bool OBEXConnect()
        {
            //send client request
            byte[] ConnectPacket = new byte[7];

            ConnectPacket[0] = 0x80;			// Connect
            ConnectPacket[1] = 0x00;			// Packetlength Hi Byte
            ConnectPacket[2] = 0x07;			// Packetlength Lo Byte
            ConnectPacket[3] = 0x10;			// Obex v1
            ConnectPacket[4] = 0x00;			// no flags
            ConnectPacket[5] = 0x20;			// 8k max packet size Hi Byte
            ConnectPacket[6] = 0x00;			// 8k max packet size Lo Byte

            stream.Write(ConnectPacket, 0, ConnectPacket.Length);

            //listen for server response
            byte[] ReceiveBufferA = new byte[3];
            stream.Read(ReceiveBufferA, 0, 3);

            if (ReceiveBufferA[0] == 160) // 0xa0
            {

                //success, decode rest of packet
                int plength = (0xff * ReceiveBufferA[1]) + ReceiveBufferA[2]; //length of packet is...

                //listen for rest of packet
                byte[] ReceiveBufferB = new byte[plength - 3];
                stream.Read(ReceiveBufferB, 0, plength - 3);

                int obver = ReceiveBufferB[0]; //server obex version (16 = v1.0)
                int cflags = ReceiveBufferB[1]; //connect flags
                int maxpack = (0xff * ReceiveBufferB[2]) + ReceiveBufferB[3]; //max packet size

                return true;
            }
            else
            {
                return false;
            }
        }

        private int OBEXRequest(string tReqType, string tName, string tType, string tFileContent)
        {
            //send client request

            int i;
            int offset;
            int packetsize;
            byte reqtype = 0x82;

            int tTypeLen = 0x03;
            int typeheadsize;
            int typesizeHi = 0x00;
            int typesizeLo = 0x03;


            if (tReqType == "GET")
            {
                reqtype = 0x83;			// 131 GET-Final
            }

            if (tReqType == "PUT")
            {
                reqtype = 0x82;			// 130 PUT-Final
            }

            packetsize = 3;

            //Name Header
            int tNameLength = tName.Length;
            int nameheadsize = (3 + (tNameLength * 2) + 2);
            int namesizeHi = (nameheadsize & 0xff00) / 0xff;
            int namesizeLo = nameheadsize & 0x00ff;
            packetsize = packetsize + nameheadsize;

            if (tType != "")
            {
                //Type Header
                tTypeLen = tType.Length;
                typeheadsize = 3 + tTypeLen + 1;
                typesizeHi = (typeheadsize & 0xff00) / 0xff;
                typesizeLo = typeheadsize & 0x00ff;
                packetsize = packetsize + typeheadsize;
            }

            //Body
            int fileLen = tFileContent.Length;
            int fileheadsize = 3 + fileLen;
            int filesizeHi = (fileheadsize & 0xff00) / 0xff; ;
            int filesizeLo = fileheadsize & 0x00ff; ;

            packetsize = packetsize + fileheadsize;

            int packetsizeHi = (packetsize & 0xff00) / 0xff;
            int packetsizeLo = packetsize & 0x00ff;

            byte[] tSendByte = new byte[packetsize];

            //PUT-final Header
            tSendByte[0] = reqtype;										// Request type e.g. PUT-final 130
            tSendByte[1] = Convert.ToByte(packetsizeHi);				// Packetlength Hi
            tSendByte[2] = Convert.ToByte(packetsizeLo);				// Packetlength Lo

            offset = 2;

            //Name Header
            tSendByte[offset + 1] = 0x01;									// HI for Name header		
            tSendByte[offset + 2] = Convert.ToByte(namesizeHi);			// Length of Name header (2 bytes per char)
            tSendByte[offset + 3] = Convert.ToByte(namesizeLo);			// Length of Name header (2 bytes per char)

            // Name+\n\n in unicode
            byte[] tNameU = System.Text.Encoding.BigEndianUnicode.GetBytes(tName);
            tNameU.CopyTo(tSendByte, offset + 4);

            offset = offset + 3 + (tNameLength * 2);
            tSendByte[offset + 1] = 0x00;									// null term
            tSendByte[offset + 2] = 0x00;									// null term

            offset = offset + 2;

            if (tType != "")
            {
                //Type Header
                tSendByte[offset + 1] = 0x42;									// HI for Type Header 66
                tSendByte[offset + 2] = Convert.ToByte(typesizeHi);			// Length of Type Header
                tSendByte[offset + 3] = Convert.ToByte(typesizeLo);			// Length of Type Header

                for (i = 0; i <= (tTypeLen - 1); i++)
                {
                    tSendByte[offset + 4 + i] = Convert.ToByte(Convert.ToChar(tType.Substring(i, 1)));
                }
                tSendByte[offset + 3 + tTypeLen + 1] = 0x00;						// null terminator

                offset = offset + 3 + tTypeLen + 1;
            }

            //Body
            tSendByte[offset + 1] = 0x49;									//HI End of Body 73
            tSendByte[offset + 2] = Convert.ToByte(filesizeHi);			//
            tSendByte[offset + 3] = Convert.ToByte(filesizeLo);			//1k payload + 3 for HI header

            for (i = 0; i <= (fileLen - 1); i++)
            {
                tSendByte[offset + 4 + i] = Convert.ToByte(Convert.ToChar(tFileContent.Substring(i, 1)));
            }
            //tSendByte[offset+4+fileLen] = 0x00;							// null terminator

            offset = offset + 3 + fileLen;

            stream.Write(tSendByte, 0, tSendByte.Length);

            //listen for server response

            //TODO: can hang here forever waiting response...

            bool x = stream.DataAvailable; // changed bluetoothclient - public NetworkStream GetStream()

            byte[] tArray4 = new byte[3];
            stream.Read(tArray4, 0, 3);

            x = stream.DataAvailable;

            if (tArray4[0] == 160) // 0xa0
            {
                int plength = (tArray4[1] * 256) + tArray4[2] - 3;
                byte[] tArray5 = new byte[plength];
                if (plength > 0)
                {
                    stream.Read(tArray5, 0, plength);
                    //TODO: data in returned packet to deal with
                }
                return 160;
            }

            if (tArray4[0] == 197) // 0xc5 Method not allowed
            {
                return 197;
            }

            if (tArray4[0] == 192) // 0xc0 Bad Request
            {
                return 192;
            }

            return 0;
        }
    
        /************************************************************************************/


        public String OpenFile()
        {
            StreamReader sr = new StreamReader("c:\\Tester.txt");

            String line = sr.ReadLine();
            return line;
        }

        
           
        
    }
}
