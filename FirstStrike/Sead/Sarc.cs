using System.Runtime.InteropServices;

namespace FirstStrike.Sead
{
    public class Sarc
    {
        [StructLayout(LayoutKind.Sequential, Size = 0x14)]
        public struct SarcHeader
        {
            public uint Magic;
            public ushort HeaderSize;
            public ushort Bom;
            public uint FileSize;
            public uint DataStart;
        }
    }
}
