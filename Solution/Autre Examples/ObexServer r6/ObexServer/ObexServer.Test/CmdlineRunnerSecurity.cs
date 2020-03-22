using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace ObexServer.Tests
{
    public static class CmdlineRunnerSecurity_Values
    {
        public const String NameA = @"aaaa.txt";
        public const String NameASlash = @"hacker/aaaa.txt";
        public const String NameABackslash = @"hacker\aaaa.txt";

        public const String Folder1NoSlash = @"E:\tmp";
        public const String Folder1WithSlash = @"E:\tmp\";
        public const String Folder1NameA = @"E:\tmp\aaaa.txt";
    }


    [TestFixture]
    public class CmdlineRunnerSecurity
    {
        private String CreateIt(String folder, String name)
        {
            IObexServer svr = null;
            String path = CmdlineRunner.NetworkServer.CreateFilePathSafely(svr, new System.IO.DirectoryInfo(folder), name);
            return path;
        }

        private void CreateItGives(String expected, String folder, String name)
        {
            String path = CreateIt(folder, name);
            Assert.AreEqual(expected, path);
        }

        [Test]
        public void SimpleNameFolderNoSlash()
        {
            CreateItGives(CmdlineRunnerSecurity_Values.Folder1NameA, 
                CmdlineRunnerSecurity_Values.Folder1NoSlash , CmdlineRunnerSecurity_Values.NameA);
        }
        [Test]
        public void SimpleNameFolderSlash()
        {
            CreateItGives(CmdlineRunnerSecurity_Values.Folder1NameA, 
                CmdlineRunnerSecurity_Values.Folder1WithSlash, CmdlineRunnerSecurity_Values.NameA);
        }

        [Test]
        public void SlashNameFolderNoSlash()
        {
            CreateItGives(CmdlineRunnerSecurity_Values.Folder1NameA,
                CmdlineRunnerSecurity_Values.Folder1NoSlash, CmdlineRunnerSecurity_Values.NameASlash);
        }
        [Test]
        public void BackslashNameFolderNoSlash()
        {
            CreateItGives(CmdlineRunnerSecurity_Values.Folder1NameA, 
                CmdlineRunnerSecurity_Values.Folder1NoSlash, CmdlineRunnerSecurity_Values.NameABackslash);
        }


    //    [Test]
    //    public void aaaa()
    //    {
    //        bool valid = CmdlineRunner.PutToDiskServer.FileIsInDirectoryOrChild(@"D:\aaaa.txt", @"D:\");
    //        Assert.IsTrue(valid);
    //    }

    //    [Test]
    //    public void aaaa2()
    //    {
    //        bool valid = CmdlineRunner.PutToDiskServer.FileIsInDirectoryOrChild(@"D:\aaaa.txt", @"D:");
    //        Assert.IsTrue(valid);
    //    }

    //    [Test]
    //    public void bbbb()
    //    {
    //        bool valid = CmdlineRunner.PutToDiskServer.FileIsInDirectoryOrChild(@"D:\tmp\aaaa.txt", @"D:\tmp");
    //        Assert.IsTrue(valid);
    //    }

    //    [Test]
    //    public void bbbb2()
    //    {
    //        bool valid = CmdlineRunner.PutToDiskServer.FileIsInDirectoryOrChild(@"D:\tmp\aaaa.txt", @"D:\tmp\");
    //        Assert.IsTrue(valid);
    //    }

    //    [Test]
    //    public void cccc()
    //    {
    //        bool valid = CmdlineRunner.PutToDiskServer.FileIsInDirectoryOrChild(@"D:\tmp2\aaaa.txt", @"D:\tmp");
    //        Assert.IsFalse(valid);
    //    }

    //    [Test]
    //    public void cccc2()
    //    {
    //        bool valid = CmdlineRunner.PutToDiskServer.FileIsInDirectoryOrChild(@"D:\tmp2\aaaa.txt", @"D:\tmp\");
    //        Assert.IsFalse(valid);
    //    }

    }//class
}
