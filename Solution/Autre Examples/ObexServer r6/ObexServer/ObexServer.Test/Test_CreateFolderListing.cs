using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace ObexServer.Tests
{

    [TestFixture]
    public class Test_CreateFolderListing
    {

        private static void DoTest(String expected, bool hasParent, DirectoryInfo[] dirs, FileInfo[] files)
        {
            Stream result = ObexGetServer.CreateFolderListing(hasParent, dirs, files);
            StreamReader rdr = new StreamReader(result);
            String resultString = rdr.ReadToEnd();
            Assert.AreEqual(expected, resultString);
        }

        //--------

        // The ObexGetServer.CreateFolderListing method takes arrays of FileInfo and 
        // DirectoryInfo so its hard to test them.  Both of those classes as sealed 
        // so it's not possible t make fake ones simply.  Perhaps can use some 
        // Mock tool.

        [Test]
        public void Empty()
        {
            DoTest(TestValues_FolderListing.ListingEmpty, false, null, null);
        }

        [Test]
        public void EmptyZeroLengths()
        {
            DirectoryInfo[] dirs = new DirectoryInfo[0];
            FileInfo[] files = new FileInfo[0];
            DoTest(TestValues_FolderListing.ListingEmpty, false, dirs, files);
        }

        [Test]
        public void EmptyHasParent()
        {
            DoTest(TestValues_FolderListing.ListingEmptyHasParent, true, null, null);
        }

        [Test]
        public void Aaaa()
        {
            FileInfo fiA = MockFileSystemInfo.CreateFileInfo("file.txt", 1234, 
                new DateTime(2007, 06, 17, 12, 30, 05),
                new DateTime(2007, 06, 17, 12, 30, 35),
                new DateTime(2007, 06, 17, 12, 30, 55));
            DirectoryInfo diB = MockFileSystemInfo.CreatedDirectoryInfo("folder1",
                new DateTime(2006, 12, 1, 2, 10, 05),
                new DateTime(2006, 12, 1, 3, 20, 35),
                new DateTime(2006, 12, 1, 4, 30, 55));
            DirectoryInfo diC = MockFileSystemInfo.CreatedDirectoryInfo("folder1utc",
                new DateTime(2006, 12, 1, 2, 10, 05, DateTimeKind.Utc),
                new DateTime(2006, 12, 1, 3, 20, 35, DateTimeKind.Utc),
                new DateTime(2006, 12, 1, 4, 30, 55, DateTimeKind.Utc));
            DoTest(TestValues_FolderListing.ListingAaaa, false,
                new DirectoryInfo[] { diB /*, diC*/ }, new FileInfo[] { fiA });
        }

        [Test]
        public void BbbbMinimumMaximum()
        {
            FileInfo fiA = MockFileSystemInfo.CreateFileInfo("file.txt", 1234,
                DateTime.MinValue, new DateTime(2007, 06, 17, 12, 30, 35), DateTime.MaxValue);
            DoTest(TestValues_FolderListing.ListingBbbbMinMax, false, null, new FileInfo[] { fiA });
        }

        //[Test]
        public void DiskParentObjRegex()
        {
            //            DoTest("", true, null, null);
            string expected = TestValues_FolderListing.ListingDiskParentRegex;
            bool hasParent = false;
            //
            Stream result = ObexGetServer.CreateFolderListing(hasParent, @"..\..\obj");
            StreamReader rdr = new StreamReader(result);
            String resultString = rdr.ReadToEnd();
            Console.Error.WriteLine(resultString);
            //
            //for (int i = 0; i < Math.Min(expected.Length, resultString.Length); ++i) {
            //    string expectedTrunc = expected.Substring(0, i);
            //    string resultStringTrunc = resultString.Substring(0, i);
            //    System.Text.RegularExpressions.Match m
            //        = System.Text.RegularExpressions.Regex.Match(resultStringTrunc, expectedTrunc);
            //    Assert.IsTrue(m.Success, "cur trunc " + i);
            //}
            //
            System.Text.RegularExpressions.Match m
                = System.Text.RegularExpressions.Regex.Match(resultString, expected);
            Assert.IsTrue(m.Success, "all regex");
        }

    }//class


    static class TestValues_FolderListing
    {
        private const String NewLine = "\r\n";

        public const String ListingEmpty
            = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + NewLine
            + "<!DOCTYPE folder-listing SYSTEM \"obex-folder-listing.dtd\">" + NewLine
            + "<folder-listing version=\"1.0\" />";
        public const String ListingEmptyHasParent
            = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + NewLine
            + "<!DOCTYPE folder-listing SYSTEM \"obex-folder-listing.dtd\">" + NewLine
            + "<folder-listing version=\"1.0\">" + NewLine
            + "  <parent-folder />" + NewLine
            + "</folder-listing>";
        public const String ListingAaaa
            = @"<?xml version=""1.0"" encoding=""utf-8""?>" + NewLine
            + @"<!DOCTYPE folder-listing SYSTEM ""obex-folder-listing.dtd"">" + NewLine
            + @"<folder-listing version=""1.0"">" + NewLine
            + @"  <folder name=""folder1"" created=""20061201T021005"" modified=""20061201T032035"" accessed=""20061201T043055"" />" + NewLine
            //+ @"  <folder name=""folder1utc"" created=""20061201T021005"" modified=""20061201T032035"" accessed=""20061201T043055"" />" + NewLine
            + @"  <file name=""file.txt"" size=""1234"" created=""20070617T123005"" modified=""20070617T123035"" accessed=""20070617T123055"" />" + NewLine
            + @"</folder-listing>";
        public const String ListingBbbbMinMax
            = @"<?xml version=""1.0"" encoding=""utf-8""?>" + NewLine
            + @"<!DOCTYPE folder-listing SYSTEM ""obex-folder-listing.dtd"">" + NewLine
            + @"<folder-listing version=""1.0"">" + NewLine
            + @"  <file name=""file.txt"" size=""1234"" created=""00010101T000000"" modified=""20070617T123035"" accessed=""99991231T235959"" />" + NewLine
            + @"</folder-listing>";
        public const String ListingDiskParent
            = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + NewLine
            + "<!DOCTYPE folder-listing SYSTEM \"obex-folder-listing.dtd\">" + NewLine
            + "<folder-listing version=\"1.0\">" + NewLine
            + "  <folder name=\"Debug\" created=\"20071216T203814\" modified=\"20080616T115411\" accessed=\"20080616T115411\" />" + NewLine
            + "  <folder name=\"Release\" created=\"20071217T[0-9]{6}\" modified=\"20080616T084748\" accessed=\"20080616T115342\" />" + NewLine
            + "  <file name=\"ObexServer.Test.csproj.FileList.txt\" size=\"1144\" created=\"20071216T203815\" modified=\"20071217T[0-9]{6}\" accessed=\"20080616T115342\" />" + NewLine
            + "  <file name=\"ObexServer.Test.csproj.FileListAbsolute.txt\" size=\"4977\" created=\"20071220T093331\" modified=\"20080616T115412\" accessed=\"20080616T115412\" />" + NewLine
            + "</folder-listing>";
        public const String ListingDiskParentRegex
            = "<\\?xml version=\"1.0\" encoding=\"utf-8\"\\?>" + NewLine
            + "<!DOCTYPE folder-listing SYSTEM \"obex-folder-listing.dtd\">" + NewLine
            + "<folder-listing version=\"1.0\">" + NewLine
            + "  <folder name=\"Debug\" created=\"[0-9]{8}T[0-9]{6}\" modified=\"[0-9]{8}T[0-9]{6}\" accessed=\"[0-9]{8}T[0-9]{6}\" />" + NewLine
            + "  <folder name=\"Release\" created=\"[0-9]{8}T[0-9]{6}\" modified=\"[0-9]{8}T[0-9]{6}\" accessed=\"[0-9]{8}T[0-9]{6}\" />" + NewLine
            + "  <file name=\"ObexServer.Test.csproj.FileList.txt\" size=\"[0-9]+\" created=\"[0-9]{8}T[0-9]{6}\" modified=\"[0-9]{8}T[0-9]{6}\" accessed=\"[0-9]{8}T[0-9]{6}\" />" + NewLine
            + "  <file name=\"ObexServer.Test.csproj.FileListAbsolute.txt\" size=\"[0-9]+\" created=\"[0-9]{8}T[0-9]{6}\" modified=\"[0-9]{8}T[0-9]{6}\" accessed=\"[0-9]{8}T[0-9]{6}\" />" + NewLine
            + "</folder-listing>";

    }//class

}
