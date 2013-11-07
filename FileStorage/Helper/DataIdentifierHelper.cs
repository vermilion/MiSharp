using System;
using System.Linq;
using System.Text;

namespace FileStorage.Helper
{
    public static class DataIdentifierHelper
    {
        /// <summary>
        ///     Returns a byte array of a specified string
        /// </summary>
        /// <param name="text">The text to go into the byte array</param>
        /// <returns>A byte array of text</returns>
        private static byte[] GetBytes(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        public static string ToNFileStorageOrigFileName(this Guid dataIdentifier, bool suppressNonReadableCharacters, char placeholderForUnreadableChars)
        {
            string result;

            try
            {
                byte[] bytes = dataIdentifier.ToByteArray();
                var encoding = new ASCIIEncoding();
                result = encoding.GetString(bytes);
                if (suppressNonReadableCharacters)
                {
                    //
                    // only return chars 32 - 127
                    //
                    var tempResult = new StringBuilder();
                    foreach (char c in result)
                    {
                        if (c >= 32 && c <= 127)
                        {
                            tempResult.Append(c);
                        }
                        else
                        {
                            tempResult.Append(placeholderForUnreadableChars);
                        }
                    }
                    result = tempResult.ToString();
                }
            }
            catch (Exception)
            {
                result = "****************";
            }

            return result;
        }

        public static string ToNFileStorageOrigFileName(this Guid dataIdentifier)
        {
            const char placeholder = '*';
            string result = ToNFileStorageOrigFileName(dataIdentifier, true, placeholder);
            if (result.Contains(placeholder))
            {
                result = "****************";
            }
            return result;
        }

        public static Guid ToNFileStorageDataIdentifier(this string text)
        {
            return ToNFileStorageDataIdentifier(text, true, true, true, '_');
        }

        public static Guid ToNFileStorageDataIdentifier(this string text, bool autoPadLeft, bool autoTakeFirst16Chars, bool caseInsensitive, char padLeftChar)
        {
            if (autoPadLeft)
            {
                text = text.PadLeft(16, padLeftChar);
            }
            if (caseInsensitive)
            {
                text = text.ToLowerInvariant();
            }

            byte[] bytes = GetBytes(text);
            if (autoTakeFirst16Chars)
            {
                bytes = bytes.Take(16).ToArray();
            }

            int length = bytes.Length;

            if (length > 16)
            {
                throw new Exception(string.Format("The text {0} you provided cannot be used for a dataIdentifier; its too big (resulting in {1} bytes)", text, length));
            }
            if (length < 16)
            {
                throw new Exception(string.Format("The text {0} you provided cannot be used for a dataIdentifier; its too small (resulting in {1} bytes). Consider appending spaces?", text, length));
            }

            return new Guid(bytes);
        }
    }
}