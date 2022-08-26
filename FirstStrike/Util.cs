using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace FirstStrike
{
    public static class Util
    {

        public static Span<byte> AsSpan<T>(ref T val) where T : unmanaged
        {
            Span<T> valSpan = MemoryMarshal.CreateSpan(ref val, 1);
            return MemoryMarshal.Cast<T, byte>(valSpan);
        }

        public static byte[] Decrypt(this Stream stream, byte[] key, byte[] iv, int length)
        {
            using var aes = Aes.Create();
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
            aes.Key = key;
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var cs = new CryptoStream(stream, decryptor, CryptoStreamMode.Read);

            byte[] decrypted = new byte[length];
            cs.Read(decrypted);

            return decrypted;
        }

        public static DirectoryInfo GetDirectory(this DirectoryInfo dir, string sub)
        {
            return new DirectoryInfo(Path.Combine(dir.FullName, sub));
        }

        public static FileInfo GetFile(this DirectoryInfo dir, string name)
        {
            return new FileInfo(Path.Combine(dir.FullName, name));
        }

        public static T GetInputFromUser<T>(string message, Func<string, T?> converter)
        {
            T? input = default;

            while (input == null)
            {
                Console.WriteLine(message);
                input = converter(Console.ReadLine().Trim());

                if (input == null)
                {
                    Console.WriteLine("Invalid input, try again.");
                }
            }

            return input;
        }
    }
}
