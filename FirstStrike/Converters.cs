using System.Globalization;

namespace FirstStrike
{
    public class Converters
    {
        public static byte[]? StringToBytes(string input)
        {
            /* Ensure there's an even amount of characters. */
            if (input.Length % 2 != 0)
                return null;

            var byteCount = input.Length / 2;
            var bytes = new byte[byteCount];
            for (var i = 0; i < byteCount; i++)
            {
                if (!byte.TryParse(input.AsSpan(i * 2, 2), NumberStyles.HexNumber, null, out var b))
                {
                    return null;
                }

                bytes[i] = b;
            }

            return bytes;
        }

        public static byte[]? StringToKey(string input)
        {
            var bytes = StringToBytes(input);

            if (bytes == null)
                return null;

            if (bytes.Length != 16)
                return null;

            return bytes;
        }

        public static FileInfo? StringToFileInfo(string input)
        {
            var fi = new FileInfo(input);

            if (!fi.Exists)
                return null;

            return fi;
        }
    }
}
