using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GlauxSoft.Phone.NumberUtil
{
    [DebuggerDisplay("{TwoLetterISOCountryCode} / {ITUCallingCode} / {FormattedInternationalNumber} / {EnglishCountryNameISO3166}")]
    public class PhoneNumberInfo
    {
        internal PhoneNumberInfo()
        {
        }

        public string TwoLetterISOCountryCode { get; internal set; }
        public string ITUCallingCode { get; internal set; }
        public string FormattedInternationalNumber { get; internal set; }
        public string EnglishCountryNameISO3166 { get; internal set; }
    }
}
