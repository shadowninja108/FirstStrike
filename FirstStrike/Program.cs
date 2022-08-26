using System.Runtime.CompilerServices;
using FirstStrike.Sead;
using YamlDotNet.Serialization;

namespace FirstStrike
{
    internal class Program
    {
        private const string SummaryYaml = "summary.yml";

        public class Summary
        {
            public string FestID { get; set; }
            public string FestResourceID { get; set; }
            public string FestResourceRevision { get; set; }


            public static Summary Parse(FileInfo fi)
            {
                var deserializer = new DeserializerBuilder().Build();
                return deserializer.Deserialize<Summary>(File.ReadAllText(fi.FullName));
            }
        }


        static void Main(string[] args)
        {
            var ifi = Util.GetInputFromUser("Input file path:",     Converters.StringToFileInfo);

            var summaryfi = ifi.Directory.GetFile(SummaryYaml);
            if (!summaryfi.Exists)
            {
                Console.WriteLine($"{SummaryYaml} must be adjacent to the input file.");
                return;
            }

            var key = Util.GetInputFromUser("Input file key:",      Converters.StringToKey);

            var summary = Summary.Parse(summaryfi);

            var ivSource = $"{summary.FestResourceID}.pack.zs";

            var ofi = new FileInfo(ifi.FullName.Replace(".zs.enc", ""));

            using var iss = ifi.OpenRead();
            var reader = new BinaryReader(iss);

            ulong someSize = reader.ReadUInt32();

            var decrypted = Nisasyst.Decrypt(ivSource, key, iss);

            Span<byte> decompressed;
            using (var decompressStream = new ZstdNet.DecompressionStream(new MemoryStream(decrypted)))
            {
                /* Read just the SARC header. */
                Sarc.SarcHeader header = new();
                decompressStream.Read(Util.AsSpan(ref header));

                /* Infer the rest of the size from the SARC header. */
                decompressed = new byte[header.FileSize];

                /* Copy in the header. */
                Util.AsSpan(ref header).CopyTo(decompressed);
                /* Read the rest of the data. */
                decompressStream.Read(decompressed[Unsafe.SizeOf<Sarc.SarcHeader>()..]);
            }

            using (var s = ofi.Create())
                s.Write(decompressed);

        }
    }
}