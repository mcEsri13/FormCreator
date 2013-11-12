using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormGeneratorAdmin
{
    public class Crypto
    {
        public static string Encrypt(string value)
        {
            try
            {
                byte[] encData_byte = new byte[value.Length];
                encData_byte = Encoding.UTF8.GetBytes(value);
                string encodedData = Convert.ToBase64String(encData_byte);

                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Encryption Error." + ex.Message);
            }
        }

        public static string Decrypt(string value)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            Decoder utf8Decode = encoder.GetDecoder();

            byte[] todecode_byte = Convert.FromBase64String(value);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];

            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
    }
}