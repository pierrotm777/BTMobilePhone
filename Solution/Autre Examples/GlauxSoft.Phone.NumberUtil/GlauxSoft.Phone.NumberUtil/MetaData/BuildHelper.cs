using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace GlauxSoft.Phone.NumberUtil
{
#if DEBUG
    public static class BuildHelper
    {

        public static void ZipIt()
        {
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\GlauxSoft.Phone.NumberUtil\MetaData");
            string xml = File.ReadAllText(Path.Combine(dir, "PhoneNumberMetaData.xml"));
            FileStream fs = new FileStream(Path.Combine(dir, "PhoneNumberMetaData.compressed"), FileMode.Create);
            GZipStream zipStream = new GZipStream(fs, CompressionMode.Compress);
            StreamWriter strWriter = new StreamWriter(zipStream);
            strWriter.Write(xml);
            strWriter.Close();
        }
    }
#endif
}
