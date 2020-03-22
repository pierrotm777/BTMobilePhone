using System;

namespace PhoneNumberTest
{
    public class PhoneNumberTest
    {
        static void Main()
        {
            PhoneNumber v_phone = new PhoneNumber("1-800-myphone ex 1234", true);
            Console.WriteLine(v_phone);
            Console.WriteLine("Is NANP? {0}", v_phone.IsNanpValid);
            return;
        }
    }
}