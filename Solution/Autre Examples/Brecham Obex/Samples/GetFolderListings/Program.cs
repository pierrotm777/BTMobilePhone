using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

#if NUNIT
using NUnit.Framework;
#endif
using Brecham.TestsInfrastructure;

using Brecham.Obex;
using System.Net.Sockets;
using Brecham.Obex.Objects;   //e.g. NetworkStream

using Brecham.Obex.Net; //e.g. MenuKeyboardInput

namespace GetFolderListings
{
    class Program
    {
        static void Main(string[] args)
        {
#if NUNIT
            //new GetFolderListings.Test_TeeStream().overBigBuffer();
#endif // NUNIT

#if DEBUG
            if (args.Length > 0 && "-sockInfo".Equals(args[0], StringComparison.OrdinalIgnoreCase)) {
                //try {
                SocketInformation sockInfo = new SocketInformation();
                String input = Console.ReadLine();
                byte[] array = Convert.FromBase64String(input);
                sockInfo.ProtocolInformation = array;
                input = Console.ReadLine();
                SocketInformationOptions opts = (SocketInformationOptions)Enum.Parse(typeof(SocketInformationOptions), input);
                sockInfo.Options = opts;
                //
                using (Socket sock = new Socket(sockInfo)) {
                    NetworkStream strm = new NetworkStream(sock, true);
                    using (ObexClientSession sess = new ObexClientSession(strm, 4096)) {
                        // Do it!
                        cmdReader(sess);
                    }
                }
                //} catch (Exception ex) {
                //    Console.Error.WriteLine(ex);
                //    Console.ReadLine();
                //}
                return;
            }
#endif

            //------------------------------
            bool rethrowConnectExceptions = args.Length > 0 && "-rethrow".Equals(args[0]);
            using (ConsoleMenuObexSessionConnection conn = new ConsoleMenuObexSessionConnection()) {
                try {
                    if (!conn.Connect()) {
                        Console.WriteLine("Connection cancelled.");
                        return;
                    }
                } catch (System.IO.EndOfStreamException eofEx) {
                    // Tried to read a menu option from the console and it was closed.
                    System.Diagnostics.Debug.Assert(eofEx.Message == MenuKeyboardInput.ExMsgReaderClosed);
                    if (rethrowConnectExceptions) { throw; }
                    return;
                } catch (SocketException sex) {
                    Console.WriteLine("Connect failed with {0} \"{1}\"", sex.SocketErrorCode, sex.Message);
                    if (sex.SocketErrorCode == SocketError.HostNotFound) {
                        Console.WriteLine("Address given was: {0}", conn.RequestedRemoteAddress);
                    }
                    if (rethrowConnectExceptions) { throw; }
                    return;
                } catch (ObexResponseException obexRspEx) {
                    if (obexRspEx.ResponseCode == Brecham.Obex.Pdus.ObexResponseCode.PeerUnsupportedService) {
                        Console.WriteLine("The OBEX Server does not support the requested Target service/application.");
                    } else {
                        Console.WriteLine("The OBEX Server rejected our connection: " + obexRspEx.ResponseCode);
                        if (obexRspEx.Description != null) {
                            Console.WriteLine("    Reason: " + obexRspEx.Description);
                        }
                    }
                    if (rethrowConnectExceptions) { throw; }
                    return;
                }
                // Must only get here if the connect was successful!
                // Each catch above should exit.
                System.Diagnostics.Debug.Assert(conn.Connected);

                //
                DisplayConnectionInfo(conn.ObexClientSession);

                // Do it!
                cmdReader(conn.ObexClientSession);
                //
            }
        }//fn--Main

        
        static void cmdUsage()
        {
            Console.WriteLine("Commands:\n"
                + "Q    Quit.\n"
                + "\n"
                + "D    Dir the folder Listing.\n"
                + "L    Get the folder Listing.\n"
                + "\n"
                + "R    Reset to default folder.\n"
                + "U    Up one folder level.\n"
                + "C <name>   Change to folder with the given name.\n"
                + "\n"
                + "A    Get anonymous--no Name or Type.\n"
                + "T <type>   Get by Type alone.\n"
                + "N <name>   Get by Name.\n"
                + "B <name> <type>   Get by both Name and Type.\n"
                + "\n"
                + "P <filename>   Put (upload) the given file.\n"
                + "\n"
                + "e.g.\n"
                + "      cmd> c fooFolder\n"
                + "      cmd> d\n"
                + "      Downloading the folder listing.\n"
                + "      ... ...\n"
                + "      cmd> q\n"
                + "      Quitting.\n"
                );
        }//fn--cmdUsage


        static void cmdReader(ObexClientSession session)
        {
            FolderListingMachinery machine = new FolderListingMachinery(session);
            Dictionary<Type, bool> expectedExceptions = null;

            Console.WriteLine();
            cmdUsage();

            bool quit = false;
            while (!quit)
            {
                Console.Write("\ncmd>");
                String cmdLine;
                try {
                    cmdLine = MenuKeyboardInput.ReadLine(ReadLineInputIsEmpty.DoNotAllow);
                } catch (System.IO.EndOfStreamException) {
                    break;
                }
                Char cmdChar;
                // A command: either a letter on its own, or a letter followed by a space.
                if (cmdLine.Length == 1 || Char.IsWhiteSpace(cmdLine[1]))
                {
                    cmdChar = MenuKeyboardInput.FirstCharAsUppercase(cmdLine);
                }
                else
                {
                    cmdChar = '\x0';
                }
                String cmdTail = cmdLine.Substring(1).Trim();
                try
                {
                    if ("hidden".Equals(cmdLine))
                    {
                        cmdUsage();
                        //cmdUsageAdvanced();
                    }
                    //----
                    else if (cmdChar.Equals('Q'))    //Quit
                    {
                        Console.WriteLine("Quitting.");
                        quit = true;
                    }
                    //----
                    else if (cmdChar.Equals('D'))   //Dir the folder listing.
                    {
                        Console.WriteLine("Downloading the folder listing.");
                        machine.DirFolderListing();
                    } else if (cmdChar.Equals('L'))   //Get the folder listing.
                    {
                        Console.WriteLine("Downloading the folder listing.");
                        machine.GetFolderListing();
                    }
                        //----
                    else if (cmdChar.Equals('R'))   //Reset to default folder.
                    {
                        Console.WriteLine("Folder resetting to default.");
                        machine.FolderReset();
                    }
                    else if (cmdChar.Equals('U'))   //Change up one folder level.
                    {
                        Console.WriteLine("Folder change up one level.");
                        machine.FolderUp();
                    }
                    else if (cmdChar.Equals('C'))   //Change folder
                    {
                        if (!CheckHasArgument(cmdTail, "folder name", Console.Out)) { continue; }
                        Console.WriteLine("Folder change to {0}.", cmdTail);
                        machine.FolderChange(cmdTail);
                    }
                    //----
                    else if (cmdChar.Equals('A'))   //Get with no Name or Type.
                    {
                        Console.WriteLine("Get anonymous.");
                        using (Stream dest = machine.CreateFileStream("anonymous")) {
                            machine.ObexSession.GetTo(dest, null, null);
                        }
                    }
                    else if (cmdChar.Equals('T'))   //Get by Type alone.
                    {
                        if (!CheckHasArgument(cmdTail, "type", Console.Out)) { continue; }
                        Console.WriteLine("Get by TYPE={0}.", cmdTail);
                        using (Stream dest = machine.CreateFileStream("byType")) {
                            machine.ObexSession.GetTo(dest, null, cmdTail);
                        }
                    }
                    else if (cmdChar.Equals('N'))   //Get by Name.
                    {
                        if (!CheckHasArgument(cmdTail, "name", Console.Out)) { continue; }
                        Console.WriteLine("Get by NAME={0}.", cmdTail);
                        using (Stream dest = machine.CreateFileStream(
                                                        Path.GetFileName(cmdTail))) {
                            machine.ObexSession.GetTo(dest, cmdTail, null);
                        }
                    }
                    else if (cmdChar.Equals('B'))   //Get by both Name and Type.
                    {
                        int pos = cmdTail.LastIndexOf(' ');
                        if (pos == -1) {
                            Console.Out.WriteLine("No Name and Type argument given on command line.");
                            continue;
                        }
                        String name = cmdTail.Substring(0, pos);
                        String type = cmdTail.Substring(pos + 1);
                        // Because cmdTail is Trim'ed we can't get _nothing_ after the space.
                        System.Diagnostics.Debug.Assert(!(type == null || type.Length == 0));
                        Console.WriteLine("Get by NAME={0}, TYPE={1}.", name, type);
                        using (Stream dest = machine.CreateFileStream(name + "_andType")) {
                            machine.ObexSession.GetTo(dest, name, type);
                        }
                    }
                    //----
                    else if (cmdChar.Equals('P'))   //Put (by Name).
                    {
                        String pathName = cmdTail;
                        if (!CheckHasArgument(cmdTail, "filename", Console.Out)) { continue; }
                        String name = Path.GetFileName(pathName);
                        Console.WriteLine("Put, with NAME={0}.", name);
                        using (Stream dest = File.OpenRead(pathName)) {
                            machine.ObexSession.PutFrom(dest, name, null);
                        }
                    } else if (cmdChar.Equals('I')) { //Info
                        DisplayConnectionInfo(session);
                    } else {
                        Console.WriteLine("Unrecognized command.");
                        cmdUsage();
                    }
                }//try
                catch (Exception ex)
                {
                    // Catch all 'expected' exceptions and only let the others 
                    // propogate.  Also change the assumed value of Y/N depending
                    // on the exception.  These are the expected ones:
                    //  _type_                      _assume_
                    //  SocketException             Quit
                    //  IOException                 Quit -- Also occurs from FileStreams??
                    //  ObexResponseException       Continue
                    //  ProtocolViolationException  Quit
                    if (expectedExceptions == null) {
                        expectedExceptions = new Dictionary<Type, bool>();
                        expectedExceptions.Add(typeof(SocketException), false);
                        expectedExceptions.Add(typeof(IOException), false);
                        expectedExceptions.Add(typeof(ObexResponseException), true);
                        expectedExceptions.Add(typeof(System.Net.ProtocolViolationException), false);
                    }
                             

                    Type exType = ex.GetType();
                    bool assumeContinue = false;
                    Console.WriteLine("\n" + exType + ": " + ex.Message);
                    if (expectedExceptions.ContainsKey(exType)) {
                        assumeContinue = expectedExceptions[exType];
                        Console.Write("\nContinue after that exception");
                        if (!MenuKeyboardInput.GetCmdLineIsYesBlankIs(assumeContinue)) {
                            return;
                        }
                    } else {
                        Console.Write("\nContinue after that exception");
                        if (!MenuKeyboardInput.GetCmdLineIsYesBlankIs(assumeContinue)) {
                            throw;
                        }
                    }
                }//catch
                //
            }//while
        }//fn--cmdReader

        private static void DisplayConnectionInfo(ObexClientSession sess)
        {
            Console.WriteLine("Local MRU: {0}, MTU: {1}, Peer MRU: {2}",
                sess.PduFactory.LocalMru, sess.PduFactory.Mtu, sess.PduFactory.PeerMru);
        }

        private static bool CheckHasArgument(string cmdTail, string paramName, TextWriter output)
        {
            if (cmdTail == null || cmdTail.Length == 0) {
                output.WriteLine("No {0} argument given on command line", paramName);
                return false;
            }
            return true;
        }//fn

    }//class


    internal class FolderListingMachinery
    {
        ObexClientSession m_obexSession;

        // The name of folder last changed to.
        // Except, if the last change was upward then its value is null.
        String m_lastFolderName = RootName;

        // The we assign to the default/base folder.
        public const String RootName = "Root";

        //-------------------

        internal FolderListingMachinery(ObexClientSession session)
        {
            m_obexSession=session;
        }
        //internal FolderListingMachinery(Stream peerStream)
        //{
        //    m_obexSession = new ObexClientSession(peerStream, UInt16.MaxValue);
        //    m_obexSession.Connect(ObexConstant.Target.FolderBrowsing);
        //}

        //internal FolderListingMachinery(Stream peerStream, byte[] target)
        //{
        //    m_obexSession = new ObexClientSession(peerStream, UInt16.MaxValue);
        //    if (target != null)
        //    {
        //       m_obexSession.Connect(target);
        //    }
        //    else
        //    {
        //        m_obexSession.Connect();
        //    }
        //}//fn

        //-------------------
        public ObexClientSession ObexSession { get { return m_obexSession; } }

        //-------------------

        public void FolderUp()
        {
            m_lastFolderName = null;
            m_obexSession.SetPathUp();
        }

        public void FolderReset()
        {
            m_lastFolderName = RootName;
            m_obexSession.SetPathReset();
        }

        public void FolderChange(String name)
        {
            m_lastFolderName = name;
            m_obexSession.SetPath(BackupFirst.DoNot, name, IfFolderDoesNotExist.Fail);
        }

        public void FolderChangeAndMake(String name)
        {
            m_lastFolderName = name;
            m_obexSession.SetPath(BackupFirst.DoNot, name, IfFolderDoesNotExist.Create);
        }

        public void GetFolderListing()
        {
            /*
             * Spec section 8.1.1.1, item 1:
             * "To retrieve the current folder: Send a GET Request with an empty 
             * Name header and a Type header that specifies the folder object type."
             */
            Stream dest;
            if (m_lastFolderName == null)
            {
                Console.WriteLine("Just did an Up folder change, "
                    + "so don't know the current folder name, "
                    + "so not writing the folder-listing operation to disk...");
                dest = new TeeToConsoleStream(Stream.Null);
            }
            else
            {
                dest = CreateFileStream();
            }//else
            using(dest) {
                m_obexSession.GetTo(dest, null, ObexConstant.Type.FolderListing);
            } 
        }//fn


        DateTime adjustTimeForZoneDstEtc(DateTime dt)
        {
            return dt.AddHours(1);
        }

        //                       "19/07/2006  11:39            19,968 Backup of ToDo.wbk"
        //                       "29/07/2006  12:01    <DIR>          .."
        const String dirFormat = "{0,10:d}  {1,5:t}    {2,5} {3,8:n0} {4}";
        //                             "             305 File(s)     17,565,733 bytes";
        const String tailFilesFormat = "{0,16} File(s) {1,15:n0} bytes";
        //                            "              49 Dir(s)   2,236,100,608 bytes free"
        const String tailDirsFormat = "{0,16} Dir(s) {1,15:n0} bytes free";
        //
        public void DirFolderListing()
        {
            using (ObexGetStream strm = m_obexSession.Get(null, ObexConstant.Type.FolderListing)) {
                ObexFolderListingParser flp = new ObexFolderListingParser(strm);
                Brecham.Obex.Objects.ObexFolderListingItem item;
                //
                int countDirs = 0;
                int countFiles = 0;
                long totalSizeFiles = 0;
                //
                while ((item = flp.GetNextItem()) != null) {
                    if (item.GetType() == typeof(ObexParentFolderItem)) {
                        DateTime fakeDateTime = new DateTime();
                        // Include a completely fake "." directory when we see a parent-folder item.
                        Console.WriteLine(dirFormat, fakeDateTime, fakeDateTime, "<DIR>", null, ".");
                        countDirs++;
                        // The parent-folder item, sadly it doesn't have a 'modified' attribute.
                        Console.WriteLine(dirFormat, fakeDateTime, fakeDateTime, "<DIR>", null, "..");
                        countDirs++;
                    } else if (item.GetType() == typeof(ObexFileItem)) {
                        ObexFileOrFolderItem fofItem = (ObexFileItem)item;
                        DateTime dt = adjustTimeForZoneDstEtc(fofItem.Modified);
                        String size = null;
                        if (fofItem.HasSize) { size = fofItem.Size.ToString(); }
                        Console.WriteLine(dirFormat, dt, dt,
                            null, size, fofItem.Name);
                        countFiles++;
                        totalSizeFiles += fofItem.Size;
                    } else if (item.GetType() == typeof(ObexFolderItem)) {
                        ObexFileOrFolderItem fofItem = (ObexFolderItem)item;
                        DateTime dt = adjustTimeForZoneDstEtc(fofItem.Modified);
                        String size = null;
                        if (fofItem.HasSize) { size = fofItem.Size.ToString(); }
                        Console.WriteLine(dirFormat, dt, dt,
                            "<DIR>", size, fofItem.Name);
                        countDirs++;
                    }
                }//while
                Console.WriteLine(tailFilesFormat, countFiles, totalSizeFiles);
                Console.WriteLine(tailDirsFormat, countDirs, -1);
            }//using
        }//fn

        //-------------------
        IMockFileStreamFactory m_fakeFileStreamFactory;
        public IMockFileStreamFactory FakeFileStreamFactory
        {
            get { return (IMockFileStreamFactory)m_fakeFileStreamFactory; }
            set { m_fakeFileStreamFactory = value; }
        }



        public Stream CreateFileStream()
        {
            String path;
            path = Path.ChangeExtension(m_lastFolderName, ".xml");
            return CreateFileStream(path);
        }

        public Stream CreateFileStream(String filename)
        {
            filename = ReplaceInvalidFileNameCharsWith(filename, "-");

            if (m_fakeFileStreamFactory == null)
            {
                FileStream fs = new FileStream(filename, FileMode.Create);
                return new TeeToConsoleStream(fs);
            }
            else
            {
                return ((IMockFileStreamFactory)m_fakeFileStreamFactory)
                             .CreateFileStream(filename);
            }
        }

        public static String ReplaceInvalidFileNameCharsWith(String filename, String replacement)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            if (filename.IndexOfAny(invalidChars) != -1) {
                String[] splits = filename.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries);
                filename = String.Join(replacement, splits);
            }
            return filename;
        }
        //public Stream CreateFileStream(Stream child)
        //{
        //    if (child == null) throw new ArgumentNullException("child");
        //    //
        //    return new TeeToConsoleStream(child);
        //}
    }//class

#if NUNIT
    [TestFixture]
    public class Test_FolderListingMachinery_ReplaceInvalidFileNameChars
    {
        private String DoTestAssert(String expected, String value)
        {
            String result = RunTest(value);
            Assert.AreEqual(value, result);
            return result;
        }

        private String RunTest(String value)
        {
            String result = FolderListingMachinery.ReplaceInvalidFileNameCharsWith(
                                                value, "-");
            return result;
        }

        //---------------------

        [Test]
        public void FileNameInvalidCharsNone()
        {
            String value = "andy";
            String result = DoTestAssert(value, value);
            // Not a new instance neither!
            Assert.AreSame(value, result);
        }

        [Test]
        public void FileNameInvalidChars1()
        {
            Assert.AreEqual("text-plain", RunTest("text/plain"));
        }

        [Test]
        public void FileNameInvalidChars2()
        {
            Assert.AreEqual("text-plain", RunTest(@"text\plain"));
        }

    }//class
#endif // NUNIT

    //--------------------------------------------------------------------------

    internal class TeeToConsoleStream : TeeToWriterStream
    {
        internal TeeToConsoleStream(Stream destination)
            : base(destination, Console.Out)
        { }//ctor
    }//class

    internal class TeeToWriterStream : Stream
    {
        Stream m_childStream;
        TextWriter m_teeWriter;
        Decoder m_decoder;

        // Tee-ing to console is disabled, probably because previous write's
        // Decoder.Convert failed, likely because we're displaying a binary file.
        bool m_disabled;

        //--------------------------------------------------------------
        public TeeToWriterStream(Stream destination, TextWriter writer)
        {
            m_childStream = destination;
            m_teeWriter = writer;
        }//.ctor

        //--------------------------------------------------------------
        public bool IsDisabled { get { return m_disabled; } }
        
        
        //--------------------------------------------------------------
#if ! FX1_1
        protected override void Dispose(bool disposing)
        {
#else
        public void Dispose()
        {
            bool disposing = true;
#endif
            try
            {
                if (!disposing || (this.m_childStream == null))
                {
                    // If finalizing, or we've already run...
                    return;
                }
                //else
                try
                {
                    this.Flush();
                }
                finally
                {
                    this.m_childStream.Close();
                }
            }
            finally
            {
                this.m_childStream = null;
#if ! FX1_1
                base.Dispose(disposing);
#endif
            }
        }//fn



        public override bool CanRead { get { return false; } }

        public override bool CanSeek { get { return false; } }

        public override bool CanWrite
        {
            get
            {
                if (this.m_childStream != null)
                {
                    return this.m_childStream.CanWrite;
                }
                return false;
            }
        }//prop

        public override void Flush()
        {
            if (this.m_childStream == null)
            {
                throwIsDisposed();
            }

            // Note: A Decoder surely can only produce a character when all its 
            // bytes have arrived.  Flushing it just resets it — telling it 
            // that any subsequent bytes are of a new sequence.

            m_childStream.Flush();
        }//fn

        private void throwIsDisposed()
        {
            const String ExMsg_IsClosed = "ExMsg_IsClosed";
            throw new ObjectDisposedException(ExMsg_IsClosed);
        }

        public override long Length
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override long Position
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if ((buffer.Length - offset) < count)
            {
                throw new ArgumentException("Argument_InvalidOffLen");
            }
            //
            if (this.m_childStream == null)
            {
                throwIsDisposed();
            }
            
            // Write to console
            if (!m_disabled) {
                WriteToWriter(buffer, offset, count);
            }else{
                m_teeWriter.Write(".");
            }
            // Now tee to child
            m_childStream.Write(buffer, offset, count);
        }//fn


        bool IsPrefix(byte[] expected, byte[] buffer, int offset, int count)
        {
            if (offset + count > buffer.Length)
                throw new ArgumentOutOfRangeException();
            if (count < expected.Length)
                throw new ArgumentOutOfRangeException();
            for (int i = 0; i < expected.Length; ++i)
            {
                if (expected[i] != buffer[offset + i])
                    return false;
            }
            return true;
        }//

        public void FindEncoding(byte[] buffer, int offset, int count)
        {
            if (m_decoder == null)
            {
                const int NumBytesToDoDetection=3;
                if (count < NumBytesToDoDetection)
                    throw new ArgumentOutOfRangeException("count",
                        "Need the first Write to be at least " 
                        + NumBytesToDoDetection 
                        + " bytes to allow encoding detection.");
                Encoding encg;
                if (buffer[0] == 0
                    || IsPrefix(Encoding.BigEndianUnicode.GetPreamble(), buffer, offset, count))
                {
                    encg = Encoding.BigEndianUnicode;
                }//if
                else if (buffer[1] == 0
                        || IsPrefix(Encoding.Unicode.GetPreamble(), buffer, offset, count))
                {
                    encg = Encoding.Unicode;
                }//else
                else
                {
                    encg = Encoding.UTF8;
                }//else
                //
                m_decoder = encg.GetDecoder();
#if ! FX1_1
                m_decoder.Reset();
#endif
            }//if
        }

        public char[] Convert(byte[] buffer, int offset, int count,
            out int countOfChars)
        {
            FindEncoding(buffer, offset, count);

            // This is a bit hacky, we aren't clever about the sizing of the buffer
            // but just instead use a buffer bigger that the ... ...
            int numChars = m_decoder.GetChars(buffer, offset, count, m_chars, 0);
            countOfChars = numChars;
            return m_chars; 
        }

        char[] m_chars = new char[UInt16.MaxValue];

        public void WriteToWriter(byte[] buffer, int offset, int count)
        {
            //FindEncoding(buffer, offset, count);
            int cch;
            try {
                // TODO Solve the buffer too small problem, by using Decoder.Convert
                // which can be called repeatedly, working each time on a suitably
                // sized input chunk and thus not overflow as we need to do the conversion 
                // in a oner.
                // So replace the call here to this.Convert with a call to FindEncoding
                // above and a while loop here calling Decoder.Convert and m_teeWriter.Write
                // in series repeatedly until all the buffer's been written.
                char[] chars = Convert(buffer, offset, count, out cch);
                m_teeWriter.Write(chars, 0, cch);
            } catch(Exception ex) {
                m_disabled = true;
                m_teeWriter.WriteLine("Error while displaying, disabling displaying. ["
                    + ex.GetType() + " " + ex.Message + "]");
            } 
#if FX1_1
            // In FX2 non-Exception-derived exceptions are wrapped in a 
            // System.Runtime.CompilerServices.RuntimeWrappedException.
            // But they aren't in FX1_1, so handle them here.
            catch {
                m_disabled = true;
                Console.WriteLine("Error while displaying, disabling displaying.");
            }
#endif
        }//fn

    }//class

#if NUNIT
    [TestFixture]
    public class Test_TeeStream
    {
        public static void Assert_AreEqual_CharBuffer(char[] expected, char[] result, int filledLength)
        {
            char[] resultComplete;
            // If the result array is only partially filled resize it.
            if (result.Length != filledLength)
            {
                resultComplete = new char[filledLength];
                Array.Copy(result, resultComplete, filledLength);
            }
            else
            {
                resultComplete = result;
            }
            Assert.AreEqual(expected, resultComplete);
        }//fn

        //------------------------------------------------
        void doTest(char[] expected, byte[] input)
        {
            using (TeeToConsoleStream strm = new TeeToConsoleStream(Stream.Null)) {
                doTest(strm, expected, input);
            }
        }
        void doTest(TeeToConsoleStream strm, char[] expected, byte[] input)
        {
            int cch;
            char[] chars = strm.Convert(input, 0, input.Length, out cch);
            Assert.AreEqual(expected.Length, cch);
            Assert_AreEqual_CharBuffer(expected, chars, cch);
        }

        //------------------------------------------------
        [Test]
        public void initUtf8Noprefix()
        {
            byte[] input = { 0x61, 0x62, 0x63, 0x64 };
            char[] expected = new char[] { 'a', 'b', 'c', 'd' };
            doTest(expected,input);
        }

        [Test]
        public void initUtf8Withprefix()
        {
            byte[] input = { 0xEF, 0xBB, 0xBF, 
                0x61, 0x62, 0x63, 0x64 };
            char[] expected = new char[] { '\uFEFF', 'a', 'b', 'c', 'd' };
            doTest(expected, input);
        }

        [Test]
        public void threeUtf8()
        {
            TeeToConsoleStream strm = new TeeToConsoleStream(Stream.Null);
            byte[] input = { 0x61, 0x62, 0x63, 0x64 };
            char[] expected = new char[] { 'a', 'b', 'c', 'd' };
            doTest(strm, expected, input);
            byte[] input1 = { 0x7A, 0xC3,0xB6, 0x30, 0x44 };
            char[] expected1 = new char[] { 'z', '\u00F6', '0', 'D' };
            doTest(strm, expected1, input1);
            byte[] input2 = { 0x20, 0x62, 0x63, 0x64 };
            char[] expected2 = new char[] { ' ', 'b', 'c', 'd' };
            doTest(strm, expected2, input2);
        }

        [Test]
        public void threeUtf8Split()
        {
            TeeToConsoleStream strm = new TeeToConsoleStream(Stream.Null);
            byte[] input = { 0x61, 0x62, 0x63, 0x64 };
            char[] expected = new char[] { 'a', 'b', 'c', 'd' };
            doTest(strm, expected, input);
            byte[] input1 = { 0x7A, 0xC3,};
            char[] expected1 = new char[] { 'z', };
            doTest(strm, expected1, input1);
            byte[] input2 = { 0xB6, 0x30, 0x44 };
            char[] expected2 = new char[] { '\u00F6', '0', 'D' };
            doTest(strm, expected2, input2);
        }

        [Test]
        public void initBigeNoprefix()
        {
            byte[] input = { 0,0x61, 0,0x62, 0,0x63, 0,0x64, 0x20,0x30 };
            char[] expected = new char[] { 'a', 'b', 'c', 'd', '\u2030' };
            doTest(expected, input);
        }

        [Test]
        public void initBigeWithprefix()
        {
            byte[] input = { 0xFE, 0xFF, 
                0,0x61, 0,0x62, 0,0x63, 0,0x64, 0x20,0x30 };
            char[] expected = new char[] { '\uFEFF', 'a', 'b', 'c', 'd', '\u2030' };
            doTest(expected, input);
        }
        [Test]
        public void initLitleNoprefix()
        {
            byte[] input = { 0x61,0, 0x62,0, 0x63,0, 0x64,0, 0x30,0x20};
            char[] expected = new char[] { 'a', 'b', 'c', 'd', '\u2030' };
            doTest(expected, input);
        }

        [Test]
        public void initLitleWithprefix()
        {
            byte[] input = { 0xFF, 0xFE, 
                0x61,0, 0x62,0, 0x63,0, 0x64,0, 0x30,0x20, };
            char[] expected = new char[] { '\uFEFF', 'a', 'b', 'c', 'd', '\u2030' };
            doTest(expected, input);
        }

        [Test]
        public void litleWithprefixSplit()
        {
            byte[] input = { 0xFF, 0xFE, 
                0x61,0, 0x62,0, 0x63,};
            byte[] input1 = { 0, 0x64,0, 0x30,0x20, };
            char[] expected = new char[] { '\uFEFF', 'a', 'b', };
            char[] expected1 =             { 'c', 'd', '\u2030' };
            TeeToConsoleStream strm = new TeeToConsoleStream(Stream.Null);
            doTest(strm, expected, input);
            doTest(strm, expected1, input1);
        }

        [Test]
        public void bigeWithprefixSplit()
        {
            byte[] input = { 0xFE,0xFF,
                0,0x61, 0,0x62,0,};
            byte[] input1 = { 0x63, 0,0x64, 0x20,0x30 };
            char[] expected = new char[] { '\uFEFF', 'a', 'b', };
            char[] expected1 =             { 'c', 'd', '\u2030' };
            TeeToConsoleStream strm = new TeeToConsoleStream(Stream.Null);
            doTest(strm, expected, input);
            doTest(strm, expected1, input1);
        }


        [Test]
        public void litleWithSuppPairWithprefixSplit()
        {
            // Also splits the codepoint U+10000, which is the surrogate pair,
            // U+D800, U+DC00, over two byte arrays.
            byte[] input = { 0xFF, 0xFE, 0x61, 0, 0x62, 0, 0x63, };
            byte[] input1 = { 0, 0x64, 0, 0x30, 0x20, 
                /*supp. pair starts */0x00, 0xD8, 0x00 };
            byte[] input2 = { 0xDC };
            char[] expected = new char[] { '\uFEFF', 'a', 'b', };
            char[] expected1 = { 'c', 'd', '\u2030' };
            char[] expected2 = "\U00010000".ToCharArray();
            TeeToConsoleStream strm = new TeeToConsoleStream(Stream.Null);
            doTest(strm, expected, input);
            doTest(strm, expected1, input1);
            doTest(strm, expected2, input2);
        }

        //--------------------------------------------------------------
        [Test]
        public void smallBuffer()
        {
            byte[] big = { (byte)'a', (byte)'a', (byte)'a', (byte)'a' };
            TeeToWriterStream strm = new TeeToWriterStream(Stream.Null, TextWriter.Null);
            strm.Write(big, 0, big.Length);
            Assert.IsFalse(strm.IsDisabled);
        }

        [Test]
        public void overBigBuffer()
        {
            //throw new RankException();
            String bigStr = new String('a', UInt16.MaxValue + 5);
            byte[] big = Encoding.ASCII.GetBytes(bigStr);
            TeeToWriterStream strm = new TeeToWriterStream(Stream.Null, TextWriter.Null);
            strm.Write(big, 0, big.Length);
            Assert.IsTrue(strm.IsDisabled);
        }

    }//class


    //--------------------------------------------------------------------------

    ////[TestFixture]
    //public class Test_Ops
    //{

    //    [Test]
    //    /*public*/ void testAFew() 
    //    {
    //        MemoryStream bodyStream = new MemoryStream();
    //        MemoryStream rcvStrm = new MemoryStream(64);
    //        //
    //        const int LengthGet1 = 10;
    //        const int LengthGet2 = 9;
    //        const int LengthGet3 = 11;
    //        byte[] response ={ 
    //            //--auto Connect--
    //            0xA0,0,26, /**/0x10,0,0xFF,0xFF,
    //                // Who
    //                    0x4A,0,19, 0xF9, 0xEC, 0x7B, 0xC4, 0x95, 0x3C, 0x11, 
    //                    0xd2, 0x98, 0x4E, 0x52, 0x54, 0x00, 0xDC, 0x9E, 0x09,
    //            //---- ----
    //            /*continue*/0x90,0,16,0x48,0,13,1,2,3,4,5,6,7,8,9,10,
    //            /*ok*/0xA0, 0, 6, 0x49,0,3,
    //            //--SetPath--
    //            0xA0,0,3,
    //            /*continue*/0x90,0,15,0x48,0,12,101,2,3,4,5,6,7,8,9,
    //            /*ok*/0xA0, 0, 6, 0x49,0,3,
    //            //--SetPath--
    //            0xA0,0,3,
    //            //--SetPath--
    //            0xA0,0,3,
    //            /*continue*/0x90,0,17,0x48,0,14,201,2,3,4,5,6,7,8,9,10,99,
    //            /*ok*/0xA0, 0, 6, 0x49,0,3,
    //        };
    //        MemoryStream rspStrm = new MemoryStream(response, false);
    //        PairOfStream strm = new PairOfStream(rspStrm, rcvStrm);
    //        //
    //        IMockFileStreamFactory fileStreamFactory = new MyMockFileStreamFactory();
    //        //
    //        FolderListingMachinery machine = new FolderListingMachinery(strm);
    //        machine.FakeFileStreamFactory = fileStreamFactory;
    //        String f1 = "f1";
    //        String f2 = "f2";
    //        //
    //        machine.GetFolderListing();
    //        Assert.AreEqual(new String[] { FolderListingMachinery.RootName },
    //            fileStreamFactory.GetGivenPaths());
    //        Assert.AreEqual(LengthGet1, 
    //            ((MockFileStream)fileStreamFactory.CurrentStream).WrittenLength);
    //        machine.FolderChange(f1);
    //        machine.GetFolderListing();
    //        Assert.AreEqual(LengthGet2,
    //            ((MockFileStream)fileStreamFactory.CurrentStream).WrittenLength);
    //        machine.FolderUp();
    //        machine.FolderChange(f2);
    //        machine.GetFolderListing();
    //        Assert.AreEqual(LengthGet3,
    //            ((MockFileStream)fileStreamFactory.CurrentStream).WrittenLength);
    //        Assert.AreEqual(new String[] { FolderListingMachinery.RootName, f1, f2 },
    //            fileStreamFactory.GetGivenPaths());
    //    }

    //    [Test]
    //    //[ExpectedException]
    //    /*public*/ void testGetAfterUp()
    //    {
    //        MemoryStream bodyStream = new MemoryStream();
    //        MemoryStream rcvStrm = new MemoryStream(64);
    //        //
    //        byte[] response ={ 
    //            //--auto Connect--
    //            0xA0,0,26, /**/0x10,0,0xFF,0xFF,
    //                // Who
    //                    0x4A,0,19, 0xF9, 0xEC, 0x7B, 0xC4, 0x95, 0x3C, 0x11, 
    //                    0xd2, 0x98, 0x4E, 0x52, 0x54, 0x00, 0xDC, 0x9E, 0x09,
    //            //--SetPath--
    //            0xA0,0,3,
    //            //--SetPath--
    //            0xA0,0,3,
    //            /*continue*/0x90,0,16,0x48,0,13,201,2,3,4,5,6,7,8,9,10,
    //            /*ok*/0xA0, 0, 6, 0x49,0,3,
    //        };
    //        MemoryStream rspStrm = new MemoryStream(response, false);
    //        PairOfStream strm = new PairOfStream(rspStrm, rcvStrm);
    //        //
    //        IMockFileStreamFactory fileStreamFactory = new MyMockFileStreamFactory();
    //        //
    //        FolderListingMachinery machine = new FolderListingMachinery(strm);
    //        machine.FakeFileStreamFactory = fileStreamFactory;
    //        String f1 = "f1";
    //        //
    //        machine.FolderChange(f1);
    //        machine.FolderUp();
    //        machine.GetFolderListing();
    //    }

    //    [Test]
    //    /*public*/ void testTwoDeep() 
    //    {
    //        MemoryStream bodyStream = new MemoryStream();
    //        MemoryStream rcvStrm = new MemoryStream(64);
    //        //
    //        byte[] response ={ 
    //            //--auto Connect--
    //            0xA0,0,26, /**/0x10,0,0xFF,0xFF,
    //                // Who
    //                    0x4A,0,19, 0xF9, 0xEC, 0x7B, 0xC4, 0x95, 0x3C, 0x11, 
    //                    0xd2, 0x98, 0x4E, 0x52, 0x54, 0x00, 0xDC, 0x9E, 0x09,
    //            //--SetPath--
    //            0xA0,0,3,
    //            //--SetPath--
    //            0xA0,0,3,
    //            /*continue*/0x90,0,16,0x48,0,13,201,2,3,4,5,6,7,8,9,10,
    //            /*ok*/0xA0, 0, 6, 0x49,0,3,
    //        };
    //        MemoryStream rspStrm = new MemoryStream(response, false);
    //        PairOfStream strm = new PairOfStream(rspStrm, rcvStrm);
    //        //
    //        IMockFileStreamFactory fileStreamFactory = new MyMockFileStreamFactory();
    //        //
    //        FolderListingMachinery machine = new FolderListingMachinery(strm);
    //        machine.FakeFileStreamFactory = fileStreamFactory;
    //        String f1 = "f1";
    //        String f1b = "f1b";
    //        //
    //        machine.FolderChange(f1); 
    //        machine.FolderChange(f1b);
    //        machine.GetFolderListing();
    //        Assert.AreEqual(new String[] { f1b },
    //            fileStreamFactory.GetGivenPaths());
    //    }

    //}//class
#endif

    //--------------------------------------------------------------------------

    class MyMockFileStreamFactory : IMockFileStreamFactory
    {
        public MyMockFileStreamFactory()
        {
            //m_paths = new List/*<string>*/();
            m_paths = new System.Collections.ArrayList/*<string>*/();
        }
        public Stream CreateFileStream(String path)
        {
            Stream stream = new MockFileStream(path);
            m_lastCreatedStream = stream;
            AppendPath(path);
            return stream;
        }

        public Stream CurrentStream { get { return m_lastCreatedStream;} }

        public void AppendPath(String path)
        {
            m_paths.Add(path);
        }

        public string[] GetGivenPaths()
        {
            String[] xxx = (String[])m_paths.ToArray();
            return xxx;
        }

        System.Collections.ArrayList/*<String>*/ m_paths;
        Stream m_lastCreatedStream;
    }

}//namespace
