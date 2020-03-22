using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace Speechpad
{
    public partial class Main : Form
    {

        #region Local Members

        private SpeechSynthesizer synthesizer = null;
        private string selectedVoice = string.Empty;
        private SpeechRecognitionEngine recognizer = null;
        private DictationGrammar dictationGrammar = null;

        #endregion

        public Main()
        {
            InitializeComponent();
            synthesizer = new SpeechSynthesizer();
            LoadSelectVoiceMenu();
            recognizer = new SpeechRecognitionEngine();
            InitializeSpeechRecognitionEngine();
            dictationGrammar = new DictationGrammar();
        }

        #region Speech Recognition

        private void InitializeSpeechRecognitionEngine()
        {
            recognizer.SetInputToDefaultAudioDevice();
            Grammar customGrammar = CreateCustomGrammar();
            recognizer.UnloadAllGrammars();
            recognizer.LoadGrammar(customGrammar);
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            recognizer.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(recognizer_SpeechHypothesized);
        }

        private Grammar CreateCustomGrammar()
        {
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(new Choices("cut", "copy", "paste", "delete"));
            return new Grammar(grammarBuilder);
        }

        private void TurnSpeechRecognitionOn()
        {
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            this.speechRecognitionMenuItem.Checked = true;
        }

        private void TurnSpeechRecognitionOff()
        {
            if (recognizer != null)
            {
                recognizer.RecognizeAsyncStop();
                this.speechRecognitionMenuItem.Checked = false;
                TurnDictationOff();
            }
        }

        private void recognizer_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            GuessText(e.Result.Text);
        }

        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string text = e.Result.Text;
            SpeechToAction(text);
        }

        private void SpeechToAction(string text)
        {
            TextDocument document = ActiveMdiChild as TextDocument;
            if (document != null)
            {
                DetermineText(text);

                switch (text)
                {
                    case "cut":
                        document.Cut();
                        break;
                    case "copy":
                        document.Copy();
                        break;
                    case "paste":
                        document.Paste();
                        break;
                    case "delete":
                        document.Delete();
                        break;
                    default:
                        document.InsertText(text);
                        break;
                }
            }
        }

        private void DetermineText(string text)
        {
            this.toolStripStatusLabel1.Text = text;
            this.toolStripStatusLabel1.ForeColor = Color.SteelBlue;
        }

        private void GuessText(string guess)
        {
            toolStripStatusLabel1.Text = guess;
            this.toolStripStatusLabel1.ForeColor = Color.DarkSalmon;
        }

        #endregion

        #region Dictation

        private void TurnDictationOn()
        {
            if (!speechRecognitionMenuItem.Checked)
            {
                TurnSpeechRecognitionOn();
            }
            recognizer.LoadGrammar(dictationGrammar);
            takeDictationMenuItem.Checked = true;
        }

        private void TurnDictationOff()
        {
            if (dictationGrammar != null)
            {
                recognizer.UnloadGrammar(dictationGrammar);
            }
            takeDictationMenuItem.Checked = false;
        }

        #endregion

        #region Select Voice

        private void LoadSelectVoiceMenu()
        {
            foreach (InstalledVoice voice in synthesizer.GetInstalledVoices())
            {
                MenuItem voiceMenuItem = new MenuItem(voice.VoiceInfo.Name);
                voiceMenuItem.RadioCheck = true;
                voiceMenuItem.Click += new EventHandler(voiceMenuItem_Click);
                this.selectVoiceMenuItem.MenuItems.Add(voiceMenuItem);
            }
            if (this.selectVoiceMenuItem.MenuItems.Count > 0)
            {
                this.selectVoiceMenuItem.MenuItems[0].Checked = true;
                selectedVoice = selectVoiceMenuItem.MenuItems[0].Text;
            }
        }

        private void voiceMenuItem_Click(object sender, EventArgs e)
        {
            SelectVoice(sender);
        }

        private void SelectVoice(object sender)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                //toggle checked value
                mi.Checked = !mi.Checked;

                if (mi.Checked)
                {
                    //set selectedVoice variable
                    selectedVoice = mi.Text;
                    //clear all other checked items
                    foreach (MenuItem voiceMi in this.selectVoiceMenuItem.MenuItems)
                    {
                        if (!voiceMi.Equals(mi))
                        {
                            voiceMi.Checked = false;
                        }
                    }
                }
                else
                {
                    //if deselecting, make first value checked, 
                    //so there is always a default value
                    this.selectVoiceMenuItem.MenuItems[0].Checked = true;
                }
            }
        }

        #endregion

        #region Speech Synthesizer Commands

        private void ReadSelectedText()
        {
            TextDocument doc = ActiveMdiChild as TextDocument;
            if (doc != null)
            {
                RichTextBox textBox = doc.richTextBox1;
                if (textBox != null)
                {
                    string speakText = textBox.SelectedText;
                    ReadAloud(speakText);
                }
            }
        }

        private void ReadDocument()
        {
            TextDocument doc = ActiveMdiChild as TextDocument;
            if (doc != null)
            {
                RichTextBox textBox = doc.richTextBox1;
                if (textBox != null)
                {
                    string speakText = textBox.Text;
                    ReadAloud(speakText);
                }
            }
        }

        private void ReadAloud(string speakText)
        {
            try
            {
                SetVoice();
                recognizer.RecognizeAsyncCancel();
                synthesizer.Speak(speakText);
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SetVoice()
        {
            try
            {
                synthesizer.SelectVoice(selectedVoice);
            }
            catch (Exception)
            {
                MessageBox.Show("The synthetic voice \"" + selectedVoice + "\" is not available.  The default voice will be used instead.");
            }
        }

        #endregion

        #region Main File Operations

        private void NewMDIChild()
        {
            NewMDIChild("Untitled");
        }

        private void NewMDIChild(string filename)
        {
            TextDocument newMDIChild = new TextDocument();
            newMDIChild.MdiParent = this;
            newMDIChild.Text = filename;
            newMDIChild.WindowState = FormWindowState.Maximized;
            newMDIChild.Show();
        }

        private void OpenFile()
        {
            try
            {
                openFileDialog1.FileName = "";
                DialogResult dr = openFileDialog1.ShowDialog();
                if (dr == DialogResult.Cancel)
                {
                    return;
                }
                string fileName = openFileDialog1.FileName;
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string text = sr.ReadToEnd();
                    NewMDIChild(fileName, text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NewMDIChild(string filename, string text)
        {
            NewMDIChild(filename);
            LoadTextToActiveDocument(text);
        }

        private void LoadTextToActiveDocument(string text)
        {
            TextDocument doc = (TextDocument)ActiveMdiChild;
            doc.richTextBox1.Text = text;
        }

        private void Exit()
        {
            Dispose();
        }

        #endregion

        #region MDI Layout

        private void cascadeMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void tileVerticalMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void tileHorizontalMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        #endregion

        #region Main Menu Handlers

        private void newMenuItem_Click(object sender, EventArgs e)
        {
            NewMDIChild();
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void readSelectedTextMenuItem_Click(object sender, EventArgs e)
        {
            ReadSelectedText();
        }

        private void readDocumentMenuItem_Click(object sender, EventArgs e)
        {
            ReadDocument();
        }

        private void speechRecognitionMenuItem_Click(object sender, EventArgs e)
        {
            if (this.speechRecognitionMenuItem.Checked)
            {
                TurnSpeechRecognitionOff();
            }
            else
            {
                TurnSpeechRecognitionOn();
            }
        }

        private void takeDictationMenuItem_Click(object sender, EventArgs e)
        {
            if (this.takeDictationMenuItem.Checked)
            {
                TurnDictationOff();
            }
            else
            {
                TurnDictationOn();
            }
        }

        #endregion

    }
}