using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Net.Sockets;
using OpenNETCF;
using OpenNETCF.Net;
using OpenNETCF.Net.Bluetooth;
using OpenNETCF.Net.Sockets;

namespace obextest
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		//Dims
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button2;
		OpenNETCF.Net.BluetoothAddress btaddress;
		OpenNETCF.Net.Sockets.BluetoothClient client;
		OpenNETCF.Net.BluetoothEndPoint endpoint;
		//Stream stream;
		NetworkStream stream;
		string BTMAC;
		string BTName;

		BluetoothDeviceInfo[] bdi; //temp

		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;

		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button button5;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.button5 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(88, 40);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(96, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "send file";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(8, 120);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(312, 336);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(88, 80);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(96, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "get default vcard";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(8, 80);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(72, 24);
			this.button3.TabIndex = 3;
			this.button3.Text = "Send vCard";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(8, 40);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(72, 23);
			this.button4.TabIndex = 4;
			this.button4.Text = "connect";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.Location = new System.Drawing.Point(8, 8);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(168, 21);
			this.comboBox1.TabIndex = 5;
			this.comboBox1.Text = "comboBox1";
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(184, 8);
			this.button5.Name = "button5";
			this.button5.TabIndex = 6;
			this.button5.Text = "Discover";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(328, 470);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{

			discover();

			comboBox1.DataSource = bdi;
			comboBox1.DisplayMember = "DeviceName";
		
			// phone bt address
			
			BTMAC = bdi[comboBox1.SelectedIndex].DeviceID.ToString();
			BTName = bdi[comboBox1.SelectedIndex].DeviceName.ToString();

			//BTMAC = "00:09:2D:12:25:D2";

			textBox1.Text = textBox1.Text + "Connecting to " + BTName + " \r\n";

			Application.DoEvents(); // update display
			
			if (OBEXOpenStream(BTMAC))
			{
			
				if (OBEXConnect()) 
				{
					addtolog("Connected to OBEX Server");
				} 
				else 
				{
					textBox1.Text = textBox1.Text + "Unable to connect\r\n";
				}

				OBEXCloseStream();
			} 
			else 
			{ 
				addtolog("Failed to connect to OBEX Server");
			}

		}

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
			catch(System.Exception e)
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
				
			stream.Write(ConnectPacket,0,ConnectPacket.Length);

			//listen for server response
			byte[] ReceiveBufferA = new byte[3];
			stream.Read(ReceiveBufferA,0,3);

			if (ReceiveBufferA[0] == 160) // 0xa0
			{ 

				//success, decode rest of packet
				int plength = (0xff * ReceiveBufferA[1]) + ReceiveBufferA[2]; //length of packet is...

				//listen for rest of packet
				byte[] ReceiveBufferB = new byte[plength-3];
				stream.Read(ReceiveBufferB,0,plength-3);

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

			//tName = "contact.vcf";
			//tType = "text/x-vCard";
			//tFileContent = "BEGIN:VCARD\r\nVERSION:2.1\r\nN:;aardvark\r\nFN:aardvark\r\nEND:VCARD\r\n";
			
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
			int nameheadsize = (3 + (tNameLength*2) + 2); 
			int namesizeHi = (nameheadsize & 0xff00)/0xff;
			int namesizeLo = nameheadsize & 0x00ff;
			packetsize = packetsize + nameheadsize;
			
			if (tType != "") 
			{
				//Type Header
				tTypeLen = tType.Length;
				typeheadsize = 3 + tTypeLen+1;
				typesizeHi = (typeheadsize & 0xff00)/0xff;
				typesizeLo = typeheadsize & 0x00ff;
				packetsize = packetsize + typeheadsize;
			}
				
			//Body
			int fileLen = tFileContent.Length;
			int fileheadsize = 3 + fileLen ;
			int filesizeHi = (fileheadsize & 0xff00)/0xff;;
			int filesizeLo = fileheadsize & 0x00ff;;

			packetsize = packetsize + fileheadsize;

			int packetsizeHi = (packetsize & 0xff00)/0xff;
			int packetsizeLo = packetsize & 0x00ff;

			byte[] tSendByte = new byte[packetsize];

			//PUT-final Header
			tSendByte[0] = reqtype;										// Request type e.g. PUT-final 130
			tSendByte[1] = Convert.ToByte(packetsizeHi);				// Packetlength Hi
			tSendByte[2] = Convert.ToByte(packetsizeLo);				// Packetlength Lo

			offset = 2;

			//Name Header
			tSendByte[offset+1] = 0x01;									// HI for Name header		
			tSendByte[offset+2] = Convert.ToByte(namesizeHi);			// Length of Name header (2 bytes per char)
			tSendByte[offset+3] = Convert.ToByte(namesizeLo);			// Length of Name header (2 bytes per char)

			// Name+\n\n in unicode
			byte[] tNameU = System.Text.Encoding.BigEndianUnicode.GetBytes(tName);
			tNameU.CopyTo(tSendByte,offset+4);

			offset = offset + 3 + (tNameLength*2);
			tSendByte[offset+1] = 0x00;									// null term
			tSendByte[offset+2] = 0x00;									// null term

			offset = offset + 2;

			if (tType != "") 
			{
				//Type Header
				tSendByte[offset+1] = 0x42;									// HI for Type Header 66
				tSendByte[offset+2] = Convert.ToByte(typesizeHi);			// Length of Type Header
				tSendByte[offset+3] = Convert.ToByte(typesizeLo);			// Length of Type Header

				for (i=0;i<=(tTypeLen-1);i++) 
				{
					tSendByte[offset+4+i] = Convert.ToByte(Convert.ToChar(tType.Substring(i,1)));
				}
				tSendByte[offset+3+tTypeLen+1] = 0x00;						// null terminator

				offset = offset+3+tTypeLen+1;
			}

			//Body
			tSendByte[offset+1] = 0x49;									//HI End of Body 73
			tSendByte[offset+2] = Convert.ToByte(filesizeHi);			//
			tSendByte[offset+3] = Convert.ToByte(filesizeLo);			//1k payload + 3 for HI header

			for (i=0;i<=(fileLen-1);i++) 
			{
				tSendByte[offset+4+i] = Convert.ToByte(Convert.ToChar(tFileContent.Substring(i,1)));
			}
			//tSendByte[offset+4+fileLen] = 0x00;							// null terminator

			offset = offset+3+fileLen;

			stream.Write(tSendByte,0,tSendByte.Length );
					
			//listen for server response

			//TODO: can hang here forever waiting response...
			
			bool x = stream.DataAvailable; // changed bluetoothclient - public NetworkStream GetStream()

			byte[] tArray4 = new byte[3];
			stream.Read(tArray4,0,3);

			x = stream.DataAvailable;

			if (tArray4[0] == 160) // 0xa0
			{
				int plength = (tArray4[1] * 256) + tArray4[2] -3;
				byte[] tArray5 = new byte[plength];
				if (plength >0) 
				{
					stream.Read(tArray5,0,plength);
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

		private void button1_Click(object sender, System.EventArgs e)
		{

			//Send a file
			addtolog("Sending File...");

			if(OBEXOpenStream(BTMAC))
			{

				if (OBEXConnect())
				{
					//send client request, start put
					string tName = "HelloWorld.txt";
					string tType = "";
					string tFileContent = "Hi this is a test!!";

					int result = OBEXRequest("PUT",tName,tType,tFileContent);
				
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


		private void button2_Click(object sender, System.EventArgs e)
		{
			/*
			 * Get Default vCard
			 * 
			 * Connect
			 * GET-final
			 * type header text/x-vCard
			 * Get Response
			*/
			addtolog("Get Default vCard...");

			if(OBEXOpenStream(BTMAC)){

			if (this.OBEXConnect())
			{
				//send client request, start put
				string tName = "";
				string tType = "text/x-vCard";
				string tFileContent = "";

				int result = OBEXRequest("GET",tName,tType,tFileContent);
				
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

				OBEXCloseStream();} 
			else
			{ 
				addtolog("Failed to connect to OBEX Server");
			}
		}



		private void button3_Click(object sender, System.EventArgs e)
		{
		
			// Send Contact as vcf file

			addtolog("Sending Contact...");

			if(OBEXOpenStream(BTMAC))
			{
				if (OBEXConnect())
				{
					string tName = "contact.vcf";
					//string tType = "text/x-vCard";
					string tType = "";
					string tFileContent = "BEGIN:VCARD\r\nVERSION:2.1\r\nN:;aardvark\r\nFN:aardvark\r\nEND:VCARD\r\n";
					
					int result = OBEXRequest("PUT",tName,tType,tFileContent);
					
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
							addtolog("other error");
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

		

		private void button4_Click(object sender, System.EventArgs e)
		{
			//Test connect
			addtolog("Connecting...");

			if(OBEXOpenStream(BTMAC))
			{
			
				if (OBEXConnect()) 
				{
					addtolog("Connected");
				} 
				else 
				{
					addtolog("Unable to connect");
				}

				OBEXCloseStream();
			} 
			else 
			{
				addtolog("Failed to connect to OBEX Server");
			}
		}

		private void addtolog(string msg)
		{
			textBox1.Text = textBox1.Text + msg + "\r\n";
			Application.DoEvents(); // update display
		}

		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BTMAC = bdi[comboBox1.SelectedIndex].DeviceID.ToString();
			BTName = bdi[comboBox1.SelectedIndex].DeviceName.ToString();

			//BTMAC = "00:09:2D:12:25:D2";

			textBox1.Text = textBox1.Text + "Connecting to " + BTName + " \r\n";

			Application.DoEvents(); // update display
			
			if (OBEXOpenStream(BTMAC))
			{
			
				if (OBEXConnect()) 
				{
					addtolog("Connected");
				} 
				else 
				{
					textBox1.Text = textBox1.Text + "Unable to connect\r\n";
				}
				
				OBEXCloseStream();
			} 
			else 
			{
			addtolog("Failed to connect to OBEX Server");
			}
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			discover();

			comboBox1.DataSource = bdi;
			comboBox1.DisplayMember = "DeviceName";
		}

		private void discover()
		{
			//discover devices
			addtolog("Discovering Devices...");
			BluetoothClient bc = new BluetoothClient(); 
			bdi = bc.DiscoverDevices(20);
			addtolog("Done, " + bdi.Length + " Devices found");
		}



	}
}
