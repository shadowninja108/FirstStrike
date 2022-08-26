using System.Text;

namespace FirstStrike.Sead
{
    /* This is based on Splatoon 2 3.1.0-ish's design, which does CRC32 in software. Later sead versions use ARM's crypto extensions. */
    public class HashCrc32
    {
        private const uint Polynomial = 0xEDB88320;
        private static readonly uint[] Table = new uint[0x100];

        static HashCrc32()
        {
            for (uint i = 0; i < 0x100; i++)
            {
                uint item = i;
                for (int bit = 0; bit < 8; ++bit)
                    item = ((item & 1) != 0) ? (Polynomial ^ (item >> 1)) : (item >> 1);
                Table[i] = item;
            }
        }

        public static uint CalcHash(byte[] data, uint ctx = 0xFFFFFFFF)
        {
            for (var i = 0; i < data.Length; i++)
            {
                ctx = Table[data[i] ^ ctx & 0xFF] ^ (ctx >> 8);
            }

            return ~ctx;
        }

        public static uint CalcStringHash(string data, uint ctx = 0xFFFFFFFF)
        {
            return CalcHash(Encoding.ASCII.GetBytes(data), ctx);
        }
    }
}
