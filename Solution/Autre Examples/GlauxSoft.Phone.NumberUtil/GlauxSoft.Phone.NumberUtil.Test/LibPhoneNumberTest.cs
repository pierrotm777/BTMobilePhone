using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GlauxSoft.Phone.NumberUtil.Test
{
    [TestClass]
    public class LibPhoneNumberTest
    {


        [TestMethod]
        public void SpecialPhoneNumberFormatTest()
        {
            // to check whether the white space and bracket removing works

            PhoneNumberInfo phoneInfo = null;

            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+41 (0)31 388 10 10");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "41");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+41 31 388 10 10");

            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+41 (0)79 323 73 62");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "41");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+41 79 323 73 62");

        }


        [TestMethod]
        public void NullEmptyAndWrongInputTest()

        {
            // to check whether the white space and bracket removing works

            PhoneNumberInfo phoneInfo = null;
            
            // does not start with leading plus sign
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion("1234");
            Assert.IsTrue(phoneInfo == null);

            // null input
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(null);
            Assert.IsTrue(phoneInfo == null);

            // empty input
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion("");
            Assert.IsTrue(phoneInfo == null);

        }
        

        [TestMethod]
        public void PerformanceTest()
        {
            // main region where we use this library. So, let's add some explicit tests

            PhoneNumberInfo phoneInfo = null;

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+41800800600");
            }
            // in general, 1000 times parsing of a number should be done within 200ms... let's say in less than one second.
            // so this test is more an indicator that something might be changed than a real rule...
            Assert.IsTrue(sw.ElapsedMilliseconds < 1000);

        }

        [TestMethod]
        public void ConcurrencyTest()
        {
            // it's not an ideal test, but we simply try to run 200 thread doing the AllOverTheWorldNumbersTest and check whether there sometimes
            // multithreading issues. So far there are none.

            var tasks = new List<Task>();

            for (int i = 0; i < 200; i++)
            {
                Task task = new Task(new Action(this.AllOverTheWorldNumbersTest));
                tasks.Add(task);
                task.Start();
            }

            tasks.ForEach(task => task.Wait());

        }

        [TestMethod]
        public void SwitzerlandTest()
        {
            // main region where we use this library. So, let's add some explicit tests

            PhoneNumberInfo phoneInfo = null;

            // Switzerland
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+41800800600");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "41");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+41 800 800 600");

            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+41313881010");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "41");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+41 31 388 10 10");

            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+41793237362");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "41");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+41 79 323 73 62");


        }

        [TestMethod]
        public void AllOverTheWorldNumbersTest()
        {
            // automatically added tests. Extracted with a tool from the PhoneNumberMetaData.xml. 
            // it contains the example numbers from number description section.
            // The number format was resolved with calling the javascript function from the JS demo.

            PhoneNumberInfo phoneInfo = null;

            //AC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2476889");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "247");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+247 6889");

            //AD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+376712345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "376");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+376 712 345");

            //AD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+376312345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "376");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+376 312 345");

            //AD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37618001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "376");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+376 1800 1234");

            //AD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+376912345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "376");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+376 912 345");

            //AE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97122345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "971");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+971 2 234 5678");

            //AE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+971501234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "971");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+971 50 123 4567");

            //AE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+971800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "971");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+971 800 123456");

            //AE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+971900234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "971");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+971 900 2 34567");

            //AE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+971700012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "971");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+971 700 0 12345");

            //AF
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+93234567890");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "93");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AF");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+93 23 456 7890");

            //AF
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+93701234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "93");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AF");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+93 70 123 4567");

            //AG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12684601234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 268-460-1234");

            //AG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12684641234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 268-464-1234");

            //AG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12684061234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 268-406-1234");

            //AG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //AG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //AG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //AG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12684801234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 268-480-1234");

            //AI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12644612345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 264-461-2345");

            //AI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12642351234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 264-235-1234");

            //AI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //AI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //AI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //AL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35522345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "355");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+355 22 345 678");

            //AL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+355661234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "355");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+355 66 123 4567");

            //AL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3558001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "355");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+355 800 1234");

            //AL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+355900123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "355");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+355 900 123");

            //AL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+355808123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "355");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+355 808 123");

            //AL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35570012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "355");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+355 700 12345");

            //AM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37410123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "374");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+374 10 123456");

            //AM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37477123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "374");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+374 77 123456");

            //AM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37480012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "374");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+374 800 12 345");

            //AM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37490012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "374");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+374 900 12 345");

            //AM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37480112345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "374");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+374 801 12 345");

            //AM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37460271234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "374");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+374 60 271234");

            //AM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3748711");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "374");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+374 8711");

            //AN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5997151234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "599");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+599 715 1234");

            //AN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5993181234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "599");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+599 318 1234");

            //AN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5991011234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "599");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+599 101 1234");

            //AO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+244222123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "244");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+244 222 123 456");

            //AO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+244923123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "244");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+244 923 123 456");

            //AR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+541123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "54");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+54 11 2345-6789");

            //AR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5491123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "54");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+54 9 11 2345-6789");

            //AR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+548012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "54");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+54 801-234-5678");

            //AR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+546001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "54");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+54 600-123-4567");

            //AS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+16846221234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 684-622-1234");

            //AS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+16847331234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 684-733-1234");

            //AS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //AS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //AS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //AT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+431234567890");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "43");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+43 1 234567890");

            //AT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+43644123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "43");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+43 644 123456");

            //AT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+43800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "43");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+43 800 123456");

            //AT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+43900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "43");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+43 900 123456");

            //AT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+43810123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "43");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+43 810 123456");

            //AT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+43780123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "43");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+43 780 123456");

            //AU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+61212345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 2 1234 5678");

            //AU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+61412345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 412 345 678");

            //AU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+611800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 1800 123 456");

            //AU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+611900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 1900 123 456");

            //AU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+61500123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 500 123 456");

            //AU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+61550123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 550 123 456");

            //AW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2975212345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "297");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+297 521 2345");

            //AW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2975601234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "297");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+297 560 1234");

            //AW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2978001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "297");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+297 800 1234");

            //AW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2979001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "297");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+297 900 1234");

            //AW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2975011234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "297");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+297 501 1234");

            //AX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3581812345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "358");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AX");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+358 18 12345678");

            //AX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+358412345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "358");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+358 41 2345678");

            //AX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3588001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "358");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+358 800 1234567");

            //AX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+358600123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "358");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+358 600 123456");

            //AX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35810112345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "358");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+358 10 112345");

            //AZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+994123123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "994");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+994 12 312 34 56");

            //AZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+994401234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "994");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+994 40 123 45 67");

            //AZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+994881234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "994");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+994 88 123 45 67");

            //AZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+994900200123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "994");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+994 900 20 01 23");

            //BA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38730123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "387");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+387 30 123-456");

            //BA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38761123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "387");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+387 61 123-456");

            //BA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38780123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "387");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+387 80 123-456");

            //BA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38790123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "387");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+387 90 123-456");

            //BA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38782123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "387");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+387 82 123-456");

            //BA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38781123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "387");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+387 81 123-456");

            //BB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12462345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 246-234-5678");

            //BB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12462501234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 246-250-1234");

            //BB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //BB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //BB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //BD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+88027111234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "880");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+880 2 7111234");

            //BD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+8801812345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "880");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+880 1812 345678");

            //BD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+8808001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "880");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+880 800 1234567");

            //BE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3212345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "32");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+32 12 34 56 78");

            //BE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+32470123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "32");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+32 470 12 34 56");

            //BE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3280012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "32");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+32 800 12 345");

            //BE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3290123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "32");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+32 901 23 456");

            //BE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3287123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "32");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+32 87 12 34 56");

            //BF
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22620491234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "226");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BF");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+226 20 49 12 34");

            //BF
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22670123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "226");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BF");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+226 70 12 34 56");

            //BG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3592123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "359");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+359 2/123 456");

            //BG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35948123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "359");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+359 48 123 456");

            //BG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35980012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "359");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+359 800 12 345");

            //BG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35990123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "359");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+359 90 123 456");

            //BG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35970012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "359");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+359 700 12 345");

            //BH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97317001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "973");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+973 1700 1234");

            //BH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97336001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "973");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+973 3600 1234");

            //BH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97380123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "973");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+973 8012 3456");

            //BH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97390123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "973");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+973 9012 3456");

            //BH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97384123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "973");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+973 8412 3456");

            //BI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+25722201234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "257");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+257 22 20 12 34");

            //BI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+25779561234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "257");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+257 79 56 12 34");

            //BJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22920211234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "229");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BJ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+229 20 21 12 34");

            //BJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22990011234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "229");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BJ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+229 90 01 12 34");

            //BJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2297312");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "229");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BJ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+229 7312");

            //BJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22985751234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "229");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BJ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+229 85 75 12 34");

            //BL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+590590271234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "590");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+590 590 27-1234");

            //BL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+590690221234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "590");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+590 690 22-1234");

            //BM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+14412345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 441-234-5678");

            //BM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+14413701234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 441-370-1234");

            //BM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //BM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //BM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //BN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6732345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "673");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+673 234 5678");

            //BN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6737123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "673");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+673 712 3456");

            //BO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+59122123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "591");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+591 2 2123456");

            //BO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+59171234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "591");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+591 71234567");

            //BR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+551123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "55");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+55 11 2345-6789");

            //BR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+551161234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "55");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+55 11 6123-4567");

            //BR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+55800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "55");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+55 800 12 3456");

            //BR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+55300123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "55");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+55 300 12 3456");

            //BR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5540041234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "55");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+55 4004-1234");

            //BS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12423456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 242-345-6789");

            //BS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12423591234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 242-359-1234");

            //BS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //BS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //BS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //BT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9752345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "975");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+975 2 345 678");

            //BT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97517123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "975");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+975 17 12 34 56");

            //BW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2672401234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "267");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+267 240 1234");

            //BW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+26771123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "267");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+267 71 123 456");

            //BW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2679012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "267");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+267 90 12345");

            //BW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+26779101234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "267");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+267 79 101 234");

            //BY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+375152450911");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "375");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+375 15 245 0911");

            //BY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+375294911911");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "375");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+375 29 491 1911");

            //BY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3758011234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "375");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+375 801 123 4567");

            //BY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3759021234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "375");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+375 902 123 4567");

            //BZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5012221234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "501");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+501 222-1234");

            //BZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5016221234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "501");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+501 622-1234");

            //BZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50108001234123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "501");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+501 0-800-1234-123");

            //CA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12042345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 204-234-5678");

            //CA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12042345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 204-234-5678");

            //CA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //CA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //CA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //CC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+61891621234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 8 9162 1234");

            //CC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+61412345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 412 345 678");

            //CC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+611800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 1800 123 456");

            //CC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+611900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 1900 123 456");

            //CC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+61500123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 500 123 456");

            //CC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+61550123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 550 123 456");

            //CD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2431234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "243");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+243 12 34567");

            //CD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+243991234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "243");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+243 991 234 567");

            //CF
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23621612345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "236");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CF");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+236 21 61 23 45");

            //CF
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23670012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "236");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CF");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+236 70 01 23 45");

            //CF
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23687761234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "236");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CF");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+236 87 76 12 34");

            //CG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+242222123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "242");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+242 22 212 3456");

            //CG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+242061234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "242");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+242 06 123 4567");

            //CG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+242800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "242");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+242 8 0012 3456");

            //CH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+41212345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "41");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+41 21 234 56 78");

            //CH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+41741234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "41");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+41 74 123 45 67");

            //CH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+41800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "41");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+41 800 123 456");

            //CH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+41900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "41");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+41 900 123 456");

            //CH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+41840123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "41");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+41 840 123 456");

            //CH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+41878123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "41");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+41 878 123 456");

            //CI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22521234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "225");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+225 21 23 45 67");

            //CI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22501234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "225");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+225 01 23 45 67");

            //CK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+68221234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "682");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+682 21 234");

            //CK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+68271234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "682");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+682 71 234");

            //CL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5621234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "56");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+56 2 123 4567");

            //CL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+56961234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "56");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+56 9 6123 4567");

            //CL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+56800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "56");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+56 800 123 456");

            //CL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+566001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "56");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+56 600 123 4567");

            //CL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+56441234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "56");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+56 44 123 4567");

            //CM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23722123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "237");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+237 22 12 34 56");

            //CM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23771234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "237");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+237 71 23 45 67");

            //CM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23780012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "237");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+237 800 12 345");

            //CM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23788012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "237");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+237 88 01 23 45");

            //CN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+861012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "86");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+86 10 1234 5678");

            //CN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+8613123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "86");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+86 131 2345 6789");

            //CN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+868001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "86");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+86 800 123 4567");

            //CN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+8616812345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "86");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+86 16812345");

            //CN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+864001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "86");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+86 400 123 4567");

            //CO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5712345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "57");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+57 1 2345678");

            //CO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+573211234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "57");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+57 321 1234567");

            //CO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5718001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "57");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+57 1 800 1234567");

            //CO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5719001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "57");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+57 1 900 1234567");

            //CR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50622123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "506");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+506 2212 3456");

            //CR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50683123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "506");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+506 8312 3456");

            //CR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5068001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "506");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+506 800-123-4567");

            //CR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5069001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "506");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+506 900-123-4567");

            //CR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50640001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "506");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+506 4000 1234");

            //CR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5061022");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "506");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+506 1022");

            //CU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5371234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "53");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+53 7 1234567");

            //CU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5351234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "53");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+53 5 1234567");

            //CV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2382211234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "238");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+238 221 12 34");

            //CV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2389911234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "238");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+238 991 12 34");

            //CY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35722345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "357");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+357 22 345678");

            //CY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35796123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "357");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+357 96 123456");

            //CY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35780001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "357");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+357 80 001234");

            //CY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35790091234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "357");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+357 90 091234");

            //CY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35770012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "357");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+357 70 012345");

            //CX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+61891641234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CX");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 8 9164 1234");

            //CX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+61412345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 412 345 678");

            //CX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+611800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 1800 123 456");

            //CX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+611900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 1900 123 456");

            //CX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+61500123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 500 123 456");

            //CX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+61550123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "61");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "AU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+61 550 123 456");

            //CZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+420212345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "420");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+420 212 345 678");

            //CZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+420601123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "420");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+420 601 123 456");

            //CZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+420800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "420");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+420 800 123 456");

            //CZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+420900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "420");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+420 900 123 456");

            //CZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+420811234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "420");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+420 811 234 567");

            //CZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+420700123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "420");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "CZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+420 700 123 456");

            //DE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4930123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "49");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+49 30/123456");

            //DE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4915123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "49");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+49 151 23456789");

            //DE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4916412345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "49");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+49 16412345");

            //DE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+498001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "49");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+49 800 1234567");

            //DE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+499001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "49");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+49 900 1 234567");

            //DE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4918012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "49");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+49 180 1 2345");

            //DE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4970012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "49");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+49 700 1234 5678");

            //DJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+253251234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "253");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DJ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+253 25 12 34");

            //DJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+253601234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "253");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DJ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+253 60 12 34");

            //DK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4532123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "45");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+45 32 12 34 56");

            //DK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4520123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "45");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+45 20 12 34 56");

            //DK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4580123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "45");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+45 80 12 34 56");

            //DK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4590123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "45");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+45 90 12 34 56");

            //DM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+17674201234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 767-420-1234");

            //DM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+17672251234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 767-225-1234");

            //DM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //DM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //DM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //DO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18092345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 809-234-5678");

            //DO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18092345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 809-234-5678");

            //DO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //DO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //DO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //DZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+21312345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "213");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+213 12 34 56 78");

            //DZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+213551234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "213");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+213 551 23 45 67");

            //DZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+213800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "213");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+213 800 12 34 56");

            //DZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+213808123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "213");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+213 808 12 34 56");

            //DZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+213801123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "213");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+213 801 12 34 56");

            //DZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+213983123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "213");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "DZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+213 98 312 34 56");

            //EC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+59322123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "593");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+593 2-212-3456");

            //EC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+59399123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "593");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+593 99 123 456");

            //EC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+59318001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "593");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+593 1800 123 4567");

            //EE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3728002123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "372");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+372 8002 123");

            //EE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3723212345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "372");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+372 321 2345");

            //EE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37251234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "372");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+372 5123 4567");

            //EE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37280012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "372");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+372 8001 2345");

            //EE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3729001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "372");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+372 900 1234");

            //EE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37270012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "372");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+372 70 01 2345");

            //EE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+372112");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "372");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+372 112");

            //EG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+20234567890");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "20");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+20 2 34567890");

            //EG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+20101234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "20");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+20 10 1234567");

            //EG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+208001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "20");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+20 800 123 4567");

            //EG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+209001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "20");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "EG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+20 900 123 4567");

            //ER
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2918370362");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "291");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ER");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+291 8 370 362");

            //ER
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2917123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "291");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ER");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+291 7 123 456");

            //ES
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+34812345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "34");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ES");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+34 812 34 56 78");

            //ES
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+34612345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "34");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ES");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+34 612 34 56 78");

            //ES
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+34800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "34");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ES");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+34 800 12 34 56");

            //ES
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+34803123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "34");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ES");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+34 803 12 34 56");

            //ES
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+34901123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "34");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ES");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+34 901 12 34 56");

            //ES
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+34701234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "34");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ES");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+34 701 23 45 67");

            //ES
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+34511234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "34");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ES");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+34 511 23 45 67");

            //ET
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+251111112345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "251");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ET");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+251 11 111 2345");

            //ET
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+251911234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "251");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ET");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+251 91 123 4567");

            //FI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3581312345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "358");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+358 13 12345678");

            //FI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+358412345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "358");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+358 41 2345678");

            //FI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3588001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "358");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+358 800 1234567");

            //FI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+358600123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "358");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+358 600 123456");

            //FI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35810112345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "358");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+358 10 112345");

            //FJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6793212345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "679");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FJ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+679 321 2345");

            //FJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6797012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "679");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FJ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+679 701 2345");

            //FJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+67908001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "679");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FJ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+679 0800 123 4567");

            //FJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+67922");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "679");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FJ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+679 22");

            //FK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50031234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "500");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+500 31234");

            //FK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50051234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "500");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+500 51234");

            //FK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+500123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "500");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+500 123");

            //FM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6913201234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "691");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+691 320 1234");

            //FM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6913501234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "691");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+691 350 1234");

            //FO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+298201234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "298");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+298 201234");

            //FO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+298211234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "298");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+298 211234");

            //FO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+298802123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "298");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+298 802123");

            //FO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+298901123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "298");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+298 901123");

            //FO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+298601234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "298");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+298 601234");

            //FO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+298211234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "298");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+298 211234");

            //FR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+33123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "33");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+33 1 23 45 67 89");

            //FR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+33612345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "33");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+33 6 12 34 56 78");

            //FR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+33801234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "33");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+33 801 23 45 67");

            //FR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+33891123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "33");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+33 891 12 34 56");

            //FR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+33810123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "33");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+33 810 12 34 56");

            //FR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+33912345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "33");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "FR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+33 9 12 34 56 78");

            //GA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+241441234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "241");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+241 44 12 34");

            //GA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+24106031234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "241");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+241 06 03 12 34");

            //GB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+441332456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 1332 456789");

            //GB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+441212345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 121 234 5678");

            //GB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+447400123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 7400 123456");

            //GB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+447640123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 76 4012 3456");

            //GB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+448001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 800 123 4567");

            //GB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+449012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 901 234 5678");

            //GB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+448431234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 843 123 4567");

            //GB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+447012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 70 1234 5678");

            //GB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+445612345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 56 1234 5678");

            //GB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+445512345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 55 1234 5678");

            //GD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+14732691234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 473-269-1234");

            //GD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+14734031234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 473-403-1234");

            //GD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //GD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //GD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //GE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+99532123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "995");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+995 32 12 34 56");

            //GE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+99555123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "995");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+995 55 12 34 56");

            //GE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+995800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "995");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+995 800 12 34 56");

            //GG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+441481250123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 1481 250123");

            //GG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+441481456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 1481 456789");

            //GG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+447781123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 7781 123456");

            //GG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+447640123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 76 4012 3456");

            //GG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+448001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 800 123 4567");

            //GG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+449012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 901 234 5678");

            //GG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+448431234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 843 123 4567");

            //GG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+447012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 70 1234 5678");

            //GG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+445612345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 56 1234 5678");

            //GG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+445512345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 55 1234 5678");

            //GG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+44150");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 150");

            //GH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+233302345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "233");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+233 30 234 5678");

            //GH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+233231234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "233");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+233 23 123 4567");

            //GI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35020012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "350");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+350 20012345");

            //GI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35057123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "350");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+350 57123456");

            //GI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35080123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "350");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+350 80123456");

            //GI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35088123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "350");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+350 88123456");

            //GI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35087123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "350");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+350 87123456");

            //GI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+350116123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "350");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+350 116123");

            //GL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+299321000");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "299");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+299 32 10 00");

            //GL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+299221234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "299");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+299 22 12 34");

            //GL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+299801234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "299");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+299 80 12 34");

            //GL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+299381234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "299");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+299 38 12 34");

            //GM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2205661234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "220");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+220 566 1234");

            //GM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2203012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "220");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+220 301 2345");

            //GN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22430241234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "224");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+224 30 24 12 34");

            //GN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22460201234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "224");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+224 60 20 12 34");

            //GP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+590590201234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "590");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+590 590 20-1234");

            //GP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+590690301234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "590");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+590 690 30-1234");

            //GQ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+240333091234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "240");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GQ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+240 333 091 234");

            //GQ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+240222123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "240");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GQ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+240 222 123 456");

            //GQ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+240800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "240");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GQ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+240 800 123456");

            //GQ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+240900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "240");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GQ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+240 900 123456");

            //GR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+302123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "30");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+30 21 2345 6789");

            //GR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+306912345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "30");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+30 691 234 5678");

            //GR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+308001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "30");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+30 800 123 4567");

            //GR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+309091234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "30");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+30 909 123 4567");

            //GR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+308011234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "30");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+30 801 123 4567");

            //GR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+307012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "30");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+30 70 1234 5678");

            //GT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50222456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "502");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+502 2245 6789");

            //GT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50251234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "502");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+502 5123 4567");

            //GT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50218001112222");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "502");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+502 1800 111 2222");

            //GT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50219001112222");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "502");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+502 1900 111 2222");

            //GT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+502123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "502");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+502 123");

            //GU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+16713001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 671-300-1234");

            //GU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+16713001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 671-300-1234");

            //GU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //GU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //GU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //GW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2453201234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "245");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+245 320 1234");

            //GW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2455012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "245");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+245 501 2345");

            //GY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5922201234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "592");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+592 220 1234");

            //GY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5926091234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "592");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+592 609 1234");

            //GY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5922891234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "592");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+592 289 1234");

            //GY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5929008123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "592");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+592 900 8123");

            //GY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5920801");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "592");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+592 0801");

            //HK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+85221234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "852");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+852 2123 4567");

            //HK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+85251234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "852");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+852 5123 4567");

            //HK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+852800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "852");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+852 800 123 456");

            //HK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+85290012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "852");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+852 900 12 345 678");

            //HK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+85281123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "852");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+852 8112 3456");

            //HN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50422123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "504");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+504 2212-3456");

            //HN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50491234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "504");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+504 9123-4567");

            //HR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38512345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "385");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+385 1 2345 678");

            //HR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+385912345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "385");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+385 91 2345 678");

            //HR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3858001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "385");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+385 800 1234 567");

            //HR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+385611234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "385");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+385 61 12 34");

            //HR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+385741234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "385");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+385 74 1234 567");

            //HT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50922453300");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "509");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+509 22 45 3300");

            //HT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50934101234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "509");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+509 34 10 1234");

            //HT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50980012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "509");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+509 80 01 2345");

            //HT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50998901234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "509");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+509 98 90 1234");

            //HT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+509114");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "509");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+509 114");

            //HU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3612345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "36");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+36 1 234 5678");

            //HU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+36201234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "36");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+36 20 123 4567");

            //HU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3680123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "36");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+36 80 123 456");

            //HU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3690123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "36");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+36 90 123 456");

            //HU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3640123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "36");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "HU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+36 40 123 456");

            //ID
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+62612345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "62");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ID");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+62 61 2345678");

            //ID
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+62812345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "62");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ID");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+62 812-345-678");

            //ID
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+628001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "62");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ID");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+62 800 1234567");

            //ID
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+628091234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "62");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ID");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+62 809 1 234 567");

            //IE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3532212345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "353");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+353 22 12345");

            //IE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+353850123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "353");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+353 85 012 3456");

            //IE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3531800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "353");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+353 1800 123 456");

            //IE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3531520123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "353");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+353 1520 123 456");

            //IE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3531850123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "353");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+353 1850 123 456");

            //IE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+353700123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "353");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+353 700 123 456");

            //IE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+353761234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "353");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+353 76 123 4567");

            //IE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+353818123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "353");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+353 818 123 456");

            //IL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9721700123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "972");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+972 1-700-123-456");

            //IL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97221234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "972");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+972 2-123-4567");

            //IL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+972501234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "972");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+972 50-123-4567");

            //IL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9721800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "972");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+972 1-800-123-456");

            //IL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9721919123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "972");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+972 1-919-123-456");

            //IL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9721700123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "972");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+972 1-700-123-456");

            //IL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+972771234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "972");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+972 77-123-4567");

            //IL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9722250");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "972");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+972 *2250");

            //IL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9721455");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "972");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+972 1455");

            //IM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+441624250123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 1624 250123");

            //IM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+441624456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 1624 456789");

            //IM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+447924123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 7924 123456");

            //IM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+448081624567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 808 162 4567");

            //IM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+449016247890");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 901 624 7890");

            //IM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+448456247890");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 845 624 7890");

            //IM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+447012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 70 1234 5678");

            //IM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+445612345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 56 1234 5678");

            //IM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+445512345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 55 1234 5678");

            //IM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+44150");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 150");

            //IN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+911123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "91");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+91 11 2345 6789");

            //IN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+919123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "91");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+91 91 23 456789");

            //IN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+911800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "91");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+91 1800 12 3456");

            //IN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9118603451234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "91");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+91 1860 345 1234");

            //IO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2463709100");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "246");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+246 370 9100");

            //IO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2463801234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "246");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+246 380 1234");

            //IQ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96412345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "964");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IQ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+964 1 234 5678");

            //IQ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9647912345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "964");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IQ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+964 791 234 5678");

            //IR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+982123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "98");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+98 21 2345 6789");

            //IR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+989123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "98");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+98 912 345 6789");

            //IR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+989432123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "98");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+98 943 212 3456");

            //IR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+989932123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "98");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+98 993 212 3456");

            //IR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+989990123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "98");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+98 999 012 3456");

            //IS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3544101234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "354");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+354 410 1234");

            //IS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3546101234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "354");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+354 610 1234");

            //IS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3548001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "354");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+354 800 1234");

            //IS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3549011234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "354");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+354 901 1234");

            //IS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3544931234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "354");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+354 493 1234");

            //IT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+390212345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "39");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+39 02 1234 5678");

            //IT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+39312345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "39");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+39 312 345 678");

            //IT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+39800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "39");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+39 800 123 456");

            //IT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+39899123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "39");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+39 899 123456");

            //IT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+398481234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "39");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+39 848 123 4567");

            //IT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+391781234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "39");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "IT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+39 178 123 4567");

            //JE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+441534250123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 1534 250123");

            //JE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+441534456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 1534 456789");

            //JE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+447797123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 7797 123456");

            //JE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+447640123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 76 4012 3456");

            //JE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+448007354567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 800 735 4567");

            //JE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+449018105678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 901 810 5678");

            //JE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+448447034567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 844 703 4567");

            //JE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+447015115678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 70 1511 5678");

            //JE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+445612345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 56 1234 5678");

            //JE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+445512345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 55 1234 5678");

            //JE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+44150");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "44");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "GB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+44 150");

            //JM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18765123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 876-512-3456");

            //JM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18762101234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 876-210-1234");

            //JM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //JM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //JM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //JO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96262001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "962");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+962 6 200 1234");

            //JO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+962790123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "962");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+962 7 90 12 34 56");

            //JO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96280012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "962");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+962 800 12345");

            //JO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96290012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "962");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+962 900 12345");

            //JO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96285012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "962");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+962 850 12345");

            //JO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+962700123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "962");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+962 700 123456");

            //JO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96287101234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "962");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+962 871 01234");

            //JP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+81312345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "81");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+81 3-1234-5678");

            //JP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+817012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "81");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+81 70-1234-5678");

            //JP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+812012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "81");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+81 20-1234-5678");

            //JP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+81120123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "81");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+81 120-123-456");

            //JP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+81990123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "81");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+81 990-123-456");

            //JP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+81601234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "81");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+81 60-123-4567");

            //JP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+815012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "81");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+81 50-1234-5678");

            //JP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+81570123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "81");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "JP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+81 570-123-456");

            //KE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+254202012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "254");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+254 20 2012345");

            //KE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+254712123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "254");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+254 712 123456");

            //KE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+254800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "254");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+254 800 123456");

            //KE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+254900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "254");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+254 900 123456");

            //KG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+996312123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "996");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+996 312 123 456");

            //KG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+996700123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "996");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+996 700 123 456");

            //KG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+996800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "996");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+996 800 123 456");

            //KH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+85523456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "855");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+855 23 456 789");

            //KH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+85591234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "855");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+855 91 234 567");

            //KH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+8551800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "855");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+855 1800 123 456");

            //KH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+8551900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "855");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+855 1900 123 456");

            //KI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+68631234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "686");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+686 31234");

            //KI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+68661234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "686");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+686 61234");

            //KI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+686992");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "686");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+686 992");

            //KM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2697712345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "269");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+269 7 712 345");

            //KM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2693212345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "269");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+269 3 212 345");

            //KM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2699001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "269");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+269 9 001 234");

            //KN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18692361234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 869-236-1234");

            //KN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18695561234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 869-556-1234");

            //KN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //KN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //KN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //KR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+8222123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "82");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+82 22123456");

            //KR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+821023456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "82");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+82 10-2345-6789");

            //KR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+82801234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "82");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+82 80-123-4567");

            //KR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+82602345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "82");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+82 60-234-5678");

            //KR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+825012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "82");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+82 50-1234-5678");

            //KR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+827012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "82");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+82 70-1234-5678");

            //KR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+8215441234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "82");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+82 1544-1234");

            //KW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96522345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "965");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+965 2234 5678");

            //KW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96550012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "965");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+965 500 12345");

            //KW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+965177");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "965");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+965 177");

            //KY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+13452221234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 345-222-1234");

            //KY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+13453231234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 345-323-1234");

            //KY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-234-5678");

            //KY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-234-5678");

            //KY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //KZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+77511234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "7");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+7 751 123 4567");

            //KZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+77123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "7");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+7 712 345 6789");

            //KZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+77710009998");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "7");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+7 771 000 9998");

            //KZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+78001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "7");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+7 800 123-45-67");

            //KZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+78091234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "7");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+7 809 123-45-67");

            //KZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+77511234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "7");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "KZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+7 751 123 4567");

            //LA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+85621212862");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "856");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+856 21 212 862");

            //LA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+8562023123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "856");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+856 20 23 123 456");

            //LB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9611123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "961");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+961 1 123 456");

            //LB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96171123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "961");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+961 71 123 456");

            //LB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96190123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "961");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+961 90 123 456");

            //LB
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96180123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "961");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LB");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+961 80 123 456");

            //LC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+17582345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 758-234-5678");

            //LC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+17582845678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 758-284-5678");

            //LC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //LC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //LC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //LI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4232345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "423");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+423 234 56 78");

            //LI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+423661234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "423");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+423 661234567");

            //LI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4238002222");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "423");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+423 800 22 22");

            //LI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4239002222");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "423");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+423 900 22 22");

            //LI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4237011234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "423");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+423 701 12 34");

            //LK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+94112345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "94");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+94 11 2 345678");

            //LK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+94712345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "94");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+94 71 234 5678");

            //LR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23121234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "231");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+231 21 234 567");

            //LR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2314612345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "231");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+231 4 612 345");

            //LR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23190123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "231");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+231 90 123 456");

            //LR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+231332001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "231");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+231 33 200 1234");

            //LS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+26622123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "266");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+266 2212 3456");

            //LS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+26650123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "266");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+266 5012 3456");

            //LS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+26680021234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "266");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+266 8002 1234");

            //LT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37031234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "370");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+370 312 34 567");

            //LT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37061234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "370");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+370 612 34 567");

            //LT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37080012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "370");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+370 800 12 345");

            //LT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37090012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "370");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+370 900 12 345");

            //LU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35227123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "352");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+352 27 12 34 56");

            //LU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+352628123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "352");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+352 628 123 456");

            //LU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35280012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "352");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+352 800 12 345");

            //LU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35290012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "352");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+352 900 12 345");

            //LU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35280112345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "352");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+352 801 12 345");

            //LU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35270123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "352");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+352 701 23 456");

            //LU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3522012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "352");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+352 20 12 345");

            //LU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35212123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "352");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+352 12123");

            //LV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37161234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "371");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+371 61 234 567");

            //LV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37121234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "371");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+371 21 234 567");

            //LV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37180123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "371");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+371 80 123 456");

            //LV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37190123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "371");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+371 90 123 456");

            //LY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+218212345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "218");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+218 21-2345678");

            //LY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+218912345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "218");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "LY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+218 91-2345678");

            //MA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+212520123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "212");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+212 520-123456");

            //MA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+212650123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "212");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+212 650-123456");

            //MA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+212801234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "212");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+212 80-1234567");

            //MA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+212891234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "212");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+212 89-1234567");

            //MC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37799123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "377");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+377 99 12 34 56");

            //MC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+377612345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "377");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+377 6 12 34 56 78");

            //MC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37790123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "377");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+377 90 12 34 56");

            //MD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37322212345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "373");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+373 22 212 345");

            //MD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37365012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "373");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+373 650 12 345");

            //MD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37380012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "373");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+373 800 12345");

            //MD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37390012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "373");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+373 900 12345");

            //MD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37380812345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "373");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+373 808 12345");

            //MD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37380312345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "373");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+373 803 12345");

            //MD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+373116000");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "373");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+373 116000");

            //ME
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38230234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "382");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ME");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+382 30 234 567");

            //ME
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38267622901");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "382");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ME");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+382 67 622 901");

            //ME
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38280080002");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "382");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ME");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+382 80 080 002");

            //ME
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38294515151");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "382");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ME");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+382 94 515 151");

            //ME
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38278108780");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "382");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ME");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+382 78 108 780");

            //ME
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38277273012");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "382");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ME");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+382 77 273 012");

            //ME
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+382123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "382");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ME");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+382 123");

            //MG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+261202123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "261");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+261 20 21 234 56");

            //MG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+261301234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "261");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+261 30 12 345 67");

            //MF
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+590590271234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "590");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+590 590 27-1234");

            //MF
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+590690221234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "590");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "BL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+590 690 22-1234");

            //MK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38922212345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "389");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+389 2 221 2345");

            //MK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38972345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "389");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+389 72 345 678");

            //MK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38980012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "389");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+389 800 1 23 45");

            //MK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38950012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "389");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+389 500 1 23 45");

            //MK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38980123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "389");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+389 801 2 34 56");

            //ML
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22320212345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "223");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ML");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+223 20 21 23 45");

            //ML
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22365012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "223");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ML");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+223 65 01 23 45");

            //ML
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22380012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "223");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ML");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+223 80 01 23 45");

            //MM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+951234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "95");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+95 1 234 567");

            //MM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9592123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "95");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+95 9 212 3456");

            //MN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97670123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "976");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+976 7012 3456");

            //MN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97688123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "976");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+976 8812 3456");

            //MN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97675123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "976");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+976 7512 3456");

            //MO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+85328212345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "853");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+853 2821 2345");

            //MO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+85366123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "853");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+853 6612 3456");

            //MP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+16702345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 670-234-5678");

            //MP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+16702345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 670-234-5678");

            //MP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //MP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //MP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //MQ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+596596301234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "596");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MQ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+596 596 30 12 34");

            //MQ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+596696201234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "596");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MQ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+596 696 20 12 34");

            //MR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22235123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "222");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+222 35 12 34 56");

            //MR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22222123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "222");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+222 22 12 34 56");

            //MR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22280012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "222");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+222 80 01 23 45");

            //MS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+16644912345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 664-491-2345");

            //MS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+16644923456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 664-492-3456");

            //MS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-212-3456");

            //MS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-212-3456");

            //MS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //MT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35621001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "356");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+356 2100 1234");

            //MT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35696961234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "356");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+356 9696 1234");

            //MT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35671171234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "356");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+356 7117 1234");

            //MT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+35650031234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "356");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+356 5003 1234");

            //MU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2302012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "230");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+230 201 2345");

            //MU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2302512345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "230");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+230 251 2345");

            //MU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2308001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "230");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+230 800 1234");

            //MU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2303012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "230");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+230 301 2345");

            //MV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9606701234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "960");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+960 670-1234");

            //MV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9607712345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "960");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+960 771-2345");

            //MV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9607812345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "960");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+960 781-2345");

            //MV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9609001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "960");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+960 900 123 4567");

            //MV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+960123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "960");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+960 123");

            //MW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2651234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "265");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+265 1 234 567");

            //MW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+265991234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "265");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+265 9 9123 4567");

            //MX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+522221234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "52");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MX");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+52 222 123 4567");

            //MX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5212221234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "52");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MX");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+52 1 222 123 4567");

            //MX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+528001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "52");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MX");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+52 800 123 4567");

            //MX
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+529001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "52");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MX");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+52 900 123 4567");

            //MY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+60312345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "60");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+60 3-1234 5678");

            //MY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+60123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "60");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+60 12-345 6789");

            //MY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+601300123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "60");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+60 1-300-12-3456");

            //MY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+601600123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "60");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+60 1-600-12-3456");

            //MY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+601700123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "60");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+60 1-700-12-3456");

            //MY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+601541234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "60");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+60 154-123 4567");

            //MZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+25821123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "258");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+258 21 123 456");

            //MZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+258821234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "258");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+258 82 123 4567");

            //MZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+258800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "258");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "MZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+258 800 123 456");

            //NA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+264612012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "264");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+264 61 201 2345");

            //NA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+264811234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "264");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+264 81 123 4567");

            //NA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+264870123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "264");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+264 870 123 456");

            //NA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+26488612345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "264");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+264 88 612 345");

            //NA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+26493111");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "264");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+264 93111");

            //NE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22720201234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "227");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+227 20 20 12 34");

            //NE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22793123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "227");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+227 93 12 34 56");

            //NE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22708123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "227");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+227 08 123 456");

            //NE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+22709123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "227");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+227 09 12 34 56");

            //NF
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+672106609");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "672");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NF");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+672 10 6609");

            //NF
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+672381234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "672");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NF");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+672 3 81234");

            //NG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23412345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "234");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+234 1 234 5678");

            //NG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2348021234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "234");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+234 802 123 4567");

            //NG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23480017591759");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "234");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+234 800 1759 1759");

            //NG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2347001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "234");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+234 700 123 4567");

            //NI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50521234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "505");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+505 2123 4567");

            //NI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50581234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "505");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+505 8123 4567");

            //NI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50518001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "505");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+505 1800 1234");

            //NL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+31101234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "31");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+31 10 123 4567");

            //NL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+31612345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "31");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+31 6 12345678");

            //NL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+318001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "31");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+31 800 1234");

            //NL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+319001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "31");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+31 900 1234");

            //NO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4721234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 21 23 45 67");

            //NO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4741234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 412 34 567");

            //NO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4780012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 800 12 345");

            //NO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4782012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 820 12 345");

            //NO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4781021234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 810 21 234");

            //NO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4788012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 880 12 345");

            //NO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4701234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 01234");

            //NP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97714567890");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "977");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+977 1-4567890");

            //NP
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9779841234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "977");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NP");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+977 984-1234567");

            //NR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6744441234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "674");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+674 444 1234");

            //NR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6745551234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "674");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+674 555 1234");

            //NR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+674110");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "674");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+674 110");

            //NU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6834002");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "683");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+683 4002");

            //NU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6831234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "683");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+683 1234");

            //NZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6432345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "64");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+64 3-234 5678");

            //NZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+64211234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "64");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+64 21 1234 567");

            //NZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6426123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "64");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+64 26 123 456");

            //NZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+64800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "64");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+64 800 123 456");

            //NZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+64900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "64");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+64 900 123 456");

            //OM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96823123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "968");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "OM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+968 23 123456");

            //OM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96892123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "968");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "OM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+968 9212 3456");

            //OM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96880071234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "968");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "OM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+968 800 71234");

            //PE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5111234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "51");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+51 1 1234567");

            //PE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+51912345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "51");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+51 912 345 678");

            //PG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6753123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "675");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+675 312 3456");

            //PG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6756812345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "675");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+675 681 2345");

            //PG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6751801234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "675");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+675 180 1234");

            //PG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6752751234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "675");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+675 275 1234");

            //PH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6321234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "63");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+63 2 123 4567");

            //PH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+639051234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "63");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+63 905 123 4567");

            //PH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+63180012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "63");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+63 1800 1 234 5678");

            //PK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+922123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "92");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+92 21 23456789");

            //PK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+923012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "92");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+92 301 2345678");

            //PK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9280012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "92");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+92 800 123 45");

            //PK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9290012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "92");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+92 900 123 45");

            //PK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+92122044444");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "92");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+92 1220 44444");

            //PK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9221111825888");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "92");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+92 21 111 825 888");

            //PL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+48123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "48");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+48 12 345 67 89");

            //PL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+48512345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "48");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+48 512 345 678");

            //PL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+48800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "48");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+48 800 123 456");

            //PL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+48701234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "48");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+48 701 234 567");

            //PL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+48801234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "48");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+48 801 234 567");

            //PL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+48391234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "48");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+48 391 234 567");

            //PR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+17872345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 787-234-5678");

            //PR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+17872345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 787-234-5678");

            //PR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-234-5678");

            //PR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-234-5678");

            //PR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //PS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97022234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "970");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+970 2 223 4567");

            //PS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+970599123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "970");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+970 599 123 456");

            //PS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9701800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "970");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+970 1800 123 456");

            //PS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97019123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "970");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+970 19123");

            //PS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9701700123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "970");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+970 1700 123 456");

            //PT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+351212345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "351");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+351 212 345 678");

            //PT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+351912345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "351");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+351 912 345 678");

            //PT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+351800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "351");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+351 800 123 456");

            //PT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+351712345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "351");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+351 712 345 678");

            //PT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+351808123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "351");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+351 808 123 456");

            //PT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+351301234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "351");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "PT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+351 301 234 567");

            //QA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97444123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "974");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "QA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+974 4412 3456");

            //QA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+97433123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "974");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "QA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+974 3312 3456");

            //QA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9748001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "974");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "QA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+974 800 1234");

            //RE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+262262161234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "262");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+262 262 16 12 34");

            //RE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+262692123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "262");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+262 692 12 34 56");

            //RE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+262801234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "262");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+262 801 23 45 67");

            //RE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+262891123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "262");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+262 891 12 34 56");

            //RE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+262810123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "262");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+262 810 12 34 56");

            //RO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+40211234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "40");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+40 21 123 4567");

            //RO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+40712345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "40");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+40 71 234 5678");

            //RO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+40800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "40");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+40 800 123 456");

            //RO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+40900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "40");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+40 900 123 456");

            //RO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+40801123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "40");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+40 801 123 456");

            //RO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+40802123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "40");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+40 802 123 456");

            //RS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3811012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "381");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+381 10 12345");

            //RS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3816012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "381");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+381 60 12345");

            //RS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38180012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "381");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+381 800 12345");

            //RS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38190012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "381");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+381 900 12345");

            //RU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+73011234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "7");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+7 301 123-45-67");

            //RU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+79123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "7");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+7 912 345-67-89");

            //RU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+78001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "7");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+7 800 123-45-67");

            //RU
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+78091234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "7");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RU");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+7 809 123-45-67");

            //RW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+250250123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "250");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+250 250 123 456");

            //RW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+250720123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "250");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+250 720 123 456");

            //RW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+250800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "250");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+250 800 123 456");

            //RW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+250900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "250");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+250 900 123 456");

            //SA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96612345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "966");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+966 1 234 5678");

            //SA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+966512345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "966");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+966 51 234 5678");

            //SA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9668001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "966");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+966 800 123 4567");

            //SA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+96692001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "966");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+966 9200 123 4567");

            //SC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2484217123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "248");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+248 4 217 123");

            //SC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2482510123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "248");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+248 2 510 123");

            //SC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+248800000");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "248");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+248 800 000");

            //SC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2484410123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "248");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+248 4 410 123");

            //SD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+249121231234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "249");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+249 12 123 1234");

            //SD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+249911231234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "249");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+249 91 123 1234");

            //SE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+468123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "46");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+46 8 12 34 56");

            //SE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+46701234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "46");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+46 70 123 45 67");

            //SE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+46201234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "46");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+46 20 123 45 67");

            //SE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+469001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "46");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+46 900 12 34 567");

            //SE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+46771234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "46");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+46 77 123 45 67");

            //SG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6561234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "65");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+65 6123 4567");

            //SG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6581234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "65");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+65 8123 4567");

            //SG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6518001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "65");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+65 1800 123 4567");

            //SG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6519001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "65");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+65 1900 123 4567");

            //SG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6531234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "65");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+65 3123 4567");

            //SG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6570001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "65");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+65 7000 1234 567");

            //SG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+651312");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "65");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+65 1312");

            //SH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2902158");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "290");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+290 2158");

            //SH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2905012");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "290");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+290 5012");

            //SI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38611234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "386");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+386 1 123 45 67");

            //SI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38631234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "386");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+386 31 234 567");

            //SI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38680123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "386");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+386 80 123456");

            //SI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38690123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "386");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+386 90 123456");

            //SI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+38659012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "386");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+386 590 12345");

            //SJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4779123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SJ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 79 12 34 56");

            //SJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4741234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 412 34 567");

            //SJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4780012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 800 12 345");

            //SJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4782012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 820 12 345");

            //SJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4781021234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 810 21 234");

            //SJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4788012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 880 12 345");

            //SJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+4701234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "47");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "NO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+47 01234");

            //SK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+421212345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "421");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+421 2/123 456 78");

            //SK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+421912123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "421");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+421 912 123 456");

            //SK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+421800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "421");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+421 800 123 456");

            //SK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+421900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "421");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+421 900 123 456");

            //SK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+421850123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "421");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+421 850 123 456");

            //SK
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+421690123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "421");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SK");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+421 690 123 456");

            //SL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23222221234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "232");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+232 22 221234");

            //SL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23225123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "232");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+232 25 123456");

            //SM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3780549886377");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "378");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+378 (0549) 886377");

            //SM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37866661212");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "378");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+378 66 66 12 12");

            //SM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37871123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "378");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+378 71 12 34 56");

            //SM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+37858001110");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "378");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+378 58 00 11 10");

            //SN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+221301012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "221");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+221 30 101 23 45");

            //SN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+221701012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "221");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+221 70 101 23 45");

            //SN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+221333011234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "221");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+221 33 301 12 34");

            //SO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2525522010");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "252");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+252 5 522010");

            //SO
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+25290792024");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "252");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SO");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+252 90 792024");

            //ST
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2392221234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "239");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ST");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+239 222 1234");

            //ST
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2399812345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "239");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ST");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+239 981 2345");

            //SV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50321234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "503");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+503 2123 4567");

            //SV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+50370123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "503");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+503 7012 3456");

            //SV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5038001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "503");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+503 800 1234");

            //SV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+5039001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "503");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+503 900 1234");

            //SY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+963112345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "963");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+963 11 234 5678");

            //SY
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+963944567890");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "963");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SY");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+963 94 4567 890");

            //SZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+26808001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "268");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+268 0800 1234");

            //SZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+26822171234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "268");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+268 2217 1234");

            //SZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+26876123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "268");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+268 7612 3456");

            //SZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+26808001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "268");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "SZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+268 0800 1234");

            //TC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+16497121234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 649-712-1234");

            //TC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+16492311234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 649-231-1234");

            //TC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-234-5678");

            //TC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-234-5678");

            //TC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //TC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+16497101234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 649-710-1234");

            //TD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23522501234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "235");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+235 22 50 12 34");

            //TD
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+23563012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "235");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TD");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+235 63 01 23 45");

            //TG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2282212345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "228");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+228 221 23 45");

            //TG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2280112345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "228");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+228 011 23 45");

            //TH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6621234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "66");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+66 2 123 4567");

            //TH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+66812345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "66");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+66 8 1234 5678");

            //TH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+661800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "66");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+66 1800 123 456");

            //TH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+661900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "66");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+66 1900 123 456");

            //TH
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+66601234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "66");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TH");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+66 60 123 4567");

            //TJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+992372123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "992");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TJ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+992 372 12 3456");

            //TJ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+992917123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "992");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TJ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+992 917 12 3456");

            //TL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6702112345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "670");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+670 211 2345");

            //TL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6707212345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "670");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+670 721 2345");

            //TL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6708012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "670");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+670 801 2345");

            //TL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6709012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "670");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+670 901 2345");

            //TL
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+6707012345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "670");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TL");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+670 701 2345");

            //TM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+99312345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "993");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+993 12 34 56 78");

            //TM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+99366123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "993");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+993 66 12 34 56");

            //TN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+21671234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "216");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+216 71 234 567");

            //TN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+21620123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "216");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+216 20 123 456");

            //TN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+21680123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "216");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+216 80 123 456");

            //TR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+902123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "90");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+90 212 345 6789");

            //TR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+905012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "90");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+90 501 234 5678");

            //TR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+905123456789");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "90");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+90 512 345 6789");

            //TR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+908001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "90");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+90 800 123 4567");

            //TR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+909001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "90");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+90 900 123 4567");

            //TR
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+904441444");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "90");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TR");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+90 444 1 444");

            //TT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18682211234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 868-221-1234");

            //TT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18682911234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 868-291-1234");

            //TT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-234-5678");

            //TT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-234-5678");

            //TT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //TV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+68820123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "688");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+688 20123");

            //TV
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+688901234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "688");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TV");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+688 901234");

            //TW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+88621234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "886");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+886 2 123 4567");

            //TW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+886912345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "886");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+886 912 345 678");

            //TW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+886800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "886");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+886 800 123 456");

            //TW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+886900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "886");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+886 900 123 456");

            //TZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+255222345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "255");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+255 22 234 5678");

            //TZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+255612345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "255");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+255 612 345 678");

            //TZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+255800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "255");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+255 800 12 3456");

            //TZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+255900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "255");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+255 900 12 3456");

            //TZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+255840123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "255");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+255 840 12 3456");

            //TZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+255412345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "255");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "TZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+255 41 234 5678");

            //UA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+380311234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "380");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "UA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+380 3112 34567");

            //UA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+380391234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "380");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "UA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+380 39 123 4567");

            //UA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+380800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "380");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "UA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+380 800 123 456");

            //UA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+380900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "380");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "UA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+380 900 123 456");

            //UG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+256312345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "256");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "UG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+256 31 2345678");

            //UG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+256712345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "256");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "UG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+256 712 345678");

            //UG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+256800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "256");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "UG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+256 800 123456");

            //UG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+256901123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "256");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "UG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+256 901 123456");

            //US
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 201-234-5678");

            //US
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12012345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 201-234-5678");

            //US
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-234-5678");

            //US
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-234-5678");

            //US
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //UZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+998612345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "998");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "UZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+998 61 234 56 78");

            //UZ
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+998912345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "998");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "UZ");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+998 91 234 56 78");

            //VA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+3790669812345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "379");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+379 06 6981 2345");

            //VC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+17842661234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 784-266-1234");

            //VC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+17844301234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VC");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 784-430-1234");

            //VC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-234-5678");

            //VC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-234-5678");

            //VC
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //VE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+582121234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "58");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+58 212-1234567");

            //VE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+584121234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "58");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+58 412-1234567");

            //VE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+588001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "58");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+58 800-1234567");

            //VE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+589001234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "58");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+58 900-1234567");

            //VG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12842291234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 284-229-1234");

            //VG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+12843001234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VG");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 284-300-1234");

            //VG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-234-5678");

            //VG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-234-5678");

            //VG
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //VI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+13406421234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 340-642-1234");

            //VI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+13406421234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VI");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 340-642-1234");

            //VI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+18002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 800-234-5678");

            //VI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+19002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 900-234-5678");

            //VI
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+15002345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "1");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "US");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+1 500-234-5678");

            //VN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+842101234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "84");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+84 210 1234 567");

            //VN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+84912345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "84");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+84 91 234 56 78");

            //VN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+841800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "84");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+84 1800 123456");

            //VN
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+841900123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "84");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "VN");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+84 1900 123456");

            //WF
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+681501234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "681");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "WF");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+681 50 12 34");

            //WS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+68522123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "685");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "WS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+685 22123");

            //WS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+685601234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "685");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "WS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+685 601234");

            //WS
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+685800123");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "685");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "WS");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+685 800 123");

            //YE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+9671234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "967");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "YE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+967 1 234 567");

            //YE
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+967712345678");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "967");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "YE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+967 712 345 678");

            //YT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+262269601234");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "262");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "YT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+262 269 60 12 34");

            //YT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+262639123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "262");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "YT");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+262 639 12 34 56");

            //YT
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+262801234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "262");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "RE");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+262 801 23 45 67");

            //ZA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+27101234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "27");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ZA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+27 10 123 4567");

            //ZA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+27711234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "27");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ZA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+27 71 123 4567");

            //ZA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+27801234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "27");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ZA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+27 80 123 4567");

            //ZA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+27861234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "27");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ZA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+27 86 123 4567");

            //ZA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+27860123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "27");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ZA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+27 860 123 456");

            //ZA
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+27871234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "27");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ZA");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+27 87 123 4567");

            //ZM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+260211234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "260");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ZM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+260 21 1234567");

            //ZM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+260955123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "260");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ZM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+260 95 5123456");

            //ZM
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+260800123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "260");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ZM");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+260 800 123 456");

            //ZW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2631312345");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "263");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ZW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+263 13 12345");

            //ZW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+263711234567");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "263");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ZW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+263 71 123 4567");

            //ZW
            phoneInfo = LibPhoneNumber.DoFormatAndFindRegion(@"+2638686123456");
            Assert.IsTrue(phoneInfo.ITUCallingCode == "263");
            Assert.IsTrue(phoneInfo.TwoLetterISOCountryCode == "ZW");
            Assert.IsTrue(phoneInfo.FormattedInternationalNumber == "+263 8686 123456");


        }
    }
}
