using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics.CodeAnalysis;


namespace AndyH.SWF
{
    class FormsControlWriter : TextWriter
    {
        //--------------------------------------------------------------
        Control m_control;

        //--------------------------------------------------------------
        public FormsControlWriter(Control control)
        {
            m_control = control;
        }

        //--------------------------------------------------------------
        public override void Write(char[] buffer, int index, int count)
        {
            String s = new string(buffer, index, count);
            AppendToTextBox(s);
        }

        delegate void delegateAppendToTextBox(String line);
        void AppendToTextBox(String line)
        {
            if (m_control.InvokeRequired) {
                m_control.BeginInvoke(new delegateAppendToTextBox(AppendToTextBox), line);
                //this causes big hang:- m_control.Invoke(new delegateAppendToTextBox(AppendToTextBox), line);
                return;
            }
            //
            m_control.Text += line;
            //((TextBox)m_control).ScrollToCaret();
        }

        //--------------------------------------------------------------
        public override System.Text.Encoding Encoding
        {
            get { throw new NotSupportedException("We write characters to the TextBox so no Encoding used."); }
        }

        //--------------------------------------------------------------
    }//class

}
