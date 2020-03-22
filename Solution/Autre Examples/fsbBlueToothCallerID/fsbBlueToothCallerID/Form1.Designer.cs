namespace fsbBlueToothCallerID
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.deviceAddress = new System.Windows.Forms.TextBox();
            this.PhoneNumber = new System.Windows.Forms.TextBox();
            this.SubscriberBut = new System.Windows.Forms.Button();
            this.GetNetwork = new System.Windows.Forms.Button();
            this.Dial = new System.Windows.Forms.Button();
            this.Hangup = new System.Windows.Forms.Button();
            this.Answer = new System.Windows.Forms.Button();
            this.Label8 = new System.Windows.Forms.Label();
            this.OutgoingCall = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.ModelId = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.ManufacturerId = new System.Windows.Forms.Label();
            this.BatteryCharge = new System.Windows.Forms.Label();
            this.SignalStrength = new System.Windows.Forms.Label();
            this.Subscriber = new System.Windows.Forms.Label();
            this.NetOperator = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Ringing = new System.Windows.Forms.Label();
            this.CallerId = new System.Windows.Forms.Label();
            this.Button2 = new System.Windows.Forms.Button();
            this.Connected = new System.Windows.Forms.Label();
            this.NetAvail = new System.Windows.Forms.Label();
            this.Button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // deviceAddress
            // 
            this.deviceAddress.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceAddress.Location = new System.Drawing.Point(37, 219);
            this.deviceAddress.Name = "deviceAddress";
            this.deviceAddress.Size = new System.Drawing.Size(195, 29);
            this.deviceAddress.TabIndex = 61;
            this.deviceAddress.Text = "00:12:D2:A6:30:3E";
            // 
            // PhoneNumber
            // 
            this.PhoneNumber.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PhoneNumber.Location = new System.Drawing.Point(403, 186);
            this.PhoneNumber.Name = "PhoneNumber";
            this.PhoneNumber.Size = new System.Drawing.Size(195, 29);
            this.PhoneNumber.TabIndex = 60;
            this.PhoneNumber.Text = "08450928081";
            // 
            // SubscriberBut
            // 
            this.SubscriberBut.Location = new System.Drawing.Point(403, 130);
            this.SubscriberBut.Name = "SubscriberBut";
            this.SubscriberBut.Size = new System.Drawing.Size(80, 48);
            this.SubscriberBut.TabIndex = 59;
            this.SubscriberBut.Text = "Subscriber";
            this.SubscriberBut.UseVisualStyleBackColor = true;
            this.SubscriberBut.Click += new System.EventHandler(this.SubscriberBut_Click);
            // 
            // GetNetwork
            // 
            this.GetNetwork.Location = new System.Drawing.Point(403, 76);
            this.GetNetwork.Name = "GetNetwork";
            this.GetNetwork.Size = new System.Drawing.Size(80, 48);
            this.GetNetwork.TabIndex = 58;
            this.GetNetwork.Text = "GetNetwork";
            this.GetNetwork.UseVisualStyleBackColor = true;
            this.GetNetwork.Click += new System.EventHandler(this.GetNetwork_Click);
            // 
            // Dial
            // 
            this.Dial.Location = new System.Drawing.Point(304, 184);
            this.Dial.Name = "Dial";
            this.Dial.Size = new System.Drawing.Size(80, 48);
            this.Dial.TabIndex = 57;
            this.Dial.Text = "Dial";
            this.Dial.UseVisualStyleBackColor = true;
            this.Dial.Click += new System.EventHandler(this.Dial_Click);
            // 
            // Hangup
            // 
            this.Hangup.Location = new System.Drawing.Point(304, 130);
            this.Hangup.Name = "Hangup";
            this.Hangup.Size = new System.Drawing.Size(80, 48);
            this.Hangup.TabIndex = 56;
            this.Hangup.Text = "Hangup";
            this.Hangup.UseVisualStyleBackColor = true;
            this.Hangup.Click += new System.EventHandler(this.Hangup_Click);
            // 
            // Answer
            // 
            this.Answer.Location = new System.Drawing.Point(304, 76);
            this.Answer.Name = "Answer";
            this.Answer.Size = new System.Drawing.Size(80, 48);
            this.Answer.TabIndex = 55;
            this.Answer.Text = "Answer";
            this.Answer.UseVisualStyleBackColor = true;
            this.Answer.Click += new System.EventHandler(this.Answer_Click);
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(34, 186);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(67, 13);
            this.Label8.TabIndex = 54;
            this.Label8.Text = "OutgoingCall";
            // 
            // OutgoingCall
            // 
            this.OutgoingCall.AutoSize = true;
            this.OutgoingCall.Location = new System.Drawing.Point(127, 186);
            this.OutgoingCall.Name = "OutgoingCall";
            this.OutgoingCall.Size = new System.Drawing.Size(67, 13);
            this.OutgoingCall.TabIndex = 53;
            this.OutgoingCall.Text = "OutgoingCall";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(34, 171);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(45, 13);
            this.Label9.TabIndex = 52;
            this.Label9.Text = "ModelId";
            // 
            // ModelId
            // 
            this.ModelId.AutoSize = true;
            this.ModelId.Location = new System.Drawing.Point(127, 171);
            this.ModelId.Name = "ModelId";
            this.ModelId.Size = new System.Drawing.Size(45, 13);
            this.ModelId.TabIndex = 51;
            this.ModelId.Text = "ModelId";
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(34, 156);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(79, 13);
            this.Label11.TabIndex = 50;
            this.Label11.Text = "ManufacturerId";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(34, 140);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(74, 13);
            this.Label10.TabIndex = 49;
            this.Label10.Text = "BatteryCharge";
            // 
            // ManufacturerId
            // 
            this.ManufacturerId.AutoSize = true;
            this.ManufacturerId.Location = new System.Drawing.Point(126, 156);
            this.ManufacturerId.Name = "ManufacturerId";
            this.ManufacturerId.Size = new System.Drawing.Size(79, 13);
            this.ManufacturerId.TabIndex = 48;
            this.ManufacturerId.Text = "ManufacturerId";
            // 
            // BatteryCharge
            // 
            this.BatteryCharge.AutoSize = true;
            this.BatteryCharge.Location = new System.Drawing.Point(126, 140);
            this.BatteryCharge.Name = "BatteryCharge";
            this.BatteryCharge.Size = new System.Drawing.Size(74, 13);
            this.BatteryCharge.TabIndex = 47;
            this.BatteryCharge.Text = "BatteryCharge";
            // 
            // SignalStrength
            // 
            this.SignalStrength.AutoSize = true;
            this.SignalStrength.Location = new System.Drawing.Point(125, 124);
            this.SignalStrength.Name = "SignalStrength";
            this.SignalStrength.Size = new System.Drawing.Size(76, 13);
            this.SignalStrength.TabIndex = 46;
            this.SignalStrength.Text = "SignalStrength";
            // 
            // Subscriber
            // 
            this.Subscriber.AutoSize = true;
            this.Subscriber.Location = new System.Drawing.Point(124, 107);
            this.Subscriber.Name = "Subscriber";
            this.Subscriber.Size = new System.Drawing.Size(57, 13);
            this.Subscriber.TabIndex = 45;
            this.Subscriber.Text = "Subscriber";
            // 
            // NetOperator
            // 
            this.NetOperator.AutoSize = true;
            this.NetOperator.Location = new System.Drawing.Point(124, 91);
            this.NetOperator.Name = "NetOperator";
            this.NetOperator.Size = new System.Drawing.Size(65, 13);
            this.NetOperator.TabIndex = 44;
            this.NetOperator.Text = "NetOperator";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(34, 124);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(76, 13);
            this.Label7.TabIndex = 43;
            this.Label7.Text = "SignalStrength";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(34, 107);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(57, 13);
            this.Label6.TabIndex = 42;
            this.Label6.Text = "Subscriber";
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            this.Timer1.Interval = 10000;
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(34, 90);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(65, 13);
            this.Label5.TabIndex = 41;
            this.Label5.Text = "NetOperator";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(34, 19);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(59, 13);
            this.Label4.TabIndex = 40;
            this.Label4.Text = "Connected";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(34, 74);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(43, 13);
            this.Label3.TabIndex = 39;
            this.Label3.Text = "Ringing";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(33, 56);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(44, 13);
            this.Label2.TabIndex = 38;
            this.Label2.Text = "Caller id";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(33, 37);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(47, 13);
            this.Label1.TabIndex = 37;
            this.Label1.Text = "NetAvail";
            // 
            // Ringing
            // 
            this.Ringing.AutoSize = true;
            this.Ringing.Location = new System.Drawing.Point(123, 74);
            this.Ringing.Name = "Ringing";
            this.Ringing.Size = new System.Drawing.Size(43, 13);
            this.Ringing.TabIndex = 36;
            this.Ringing.Text = "Ringing";
            // 
            // CallerId
            // 
            this.CallerId.AutoSize = true;
            this.CallerId.Location = new System.Drawing.Point(123, 56);
            this.CallerId.Name = "CallerId";
            this.CallerId.Size = new System.Drawing.Size(42, 13);
            this.CallerId.TabIndex = 35;
            this.CallerId.Text = "CallerId";
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(403, 25);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(80, 45);
            this.Button2.TabIndex = 34;
            this.Button2.Text = "Disconnect";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Connected
            // 
            this.Connected.AutoSize = true;
            this.Connected.Location = new System.Drawing.Point(123, 19);
            this.Connected.Name = "Connected";
            this.Connected.Size = new System.Drawing.Size(32, 13);
            this.Connected.TabIndex = 33;
            this.Connected.Text = "False";
            // 
            // NetAvail
            // 
            this.NetAvail.AutoSize = true;
            this.NetAvail.Location = new System.Drawing.Point(123, 37);
            this.NetAvail.Name = "NetAvail";
            this.NetAvail.Size = new System.Drawing.Size(47, 13);
            this.NetAvail.TabIndex = 32;
            this.NetAvail.Text = "NetAvail";
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(304, 22);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(80, 48);
            this.Button1.TabIndex = 31;
            this.Button1.Text = "Connect";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 266);
            this.Controls.Add(this.deviceAddress);
            this.Controls.Add(this.PhoneNumber);
            this.Controls.Add(this.SubscriberBut);
            this.Controls.Add(this.GetNetwork);
            this.Controls.Add(this.Dial);
            this.Controls.Add(this.Hangup);
            this.Controls.Add(this.Answer);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.OutgoingCall);
            this.Controls.Add(this.Label9);
            this.Controls.Add(this.ModelId);
            this.Controls.Add(this.Label11);
            this.Controls.Add(this.Label10);
            this.Controls.Add(this.ManufacturerId);
            this.Controls.Add(this.BatteryCharge);
            this.Controls.Add(this.SignalStrength);
            this.Controls.Add(this.Subscriber);
            this.Controls.Add(this.NetOperator);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Ringing);
            this.Controls.Add(this.CallerId);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.Connected);
            this.Controls.Add(this.NetAvail);
            this.Controls.Add(this.Button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox deviceAddress;
        internal System.Windows.Forms.TextBox PhoneNumber;
        internal System.Windows.Forms.Button SubscriberBut;
        internal System.Windows.Forms.Button GetNetwork;
        internal System.Windows.Forms.Button Dial;
        internal System.Windows.Forms.Button Hangup;
        internal System.Windows.Forms.Button Answer;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label OutgoingCall;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.Label ModelId;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.Label ManufacturerId;
        internal System.Windows.Forms.Label BatteryCharge;
        internal System.Windows.Forms.Label SignalStrength;
        internal System.Windows.Forms.Label Subscriber;
        internal System.Windows.Forms.Label NetOperator;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Timer Timer1;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Ringing;
        internal System.Windows.Forms.Label CallerId;
        internal System.Windows.Forms.Button Button2;
        internal System.Windows.Forms.Label Connected;
        internal System.Windows.Forms.Label NetAvail;
        internal System.Windows.Forms.Button Button1;

    }
}

