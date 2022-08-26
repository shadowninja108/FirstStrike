using FirstStrike.Sead;

namespace FirstStrike
{
    public class Nisasyst
    {
        private const string KeyMaterialString = "e413645fa69cafe34a76192843e48cbd691d1f9fba87e8a23d40e02ce13b0d534d10301576f31bc70b763a60cf07149cfca50e2a6b3955b98f26ca84a5844a8aeca7318f8d7dba406af4e45c4806fa4d7b736d51cceaaf0e96f657bb3a8af9b175d51b9bddc1ed475677260f33c41ddbc1ee30b46c4df1b24a25cf7cb6019794";
        private static readonly char[] KeyMaterial = KeyMaterialString.ToCharArray();
        public static byte[] Decrypt(string path, byte[] key, Stream stream)
        {
            var hash = HashCrc32.CalcStringHash(path);
            var random = new SeadRandom(hash);

            /* Derive IV. */
            var iv = GenKey(random);

            /* Decrypt file. */
            return stream.Decrypt(key, iv, (int)(stream.Length - stream.Position));
        }

        private static byte[] GenKey(SeadRandom random)
        {
            byte[] key = new byte[0x10];

            for (int i = 0; i < key.Length; i++)
            {
                var str = "";

                str += KeyMaterial[random.GetUInt32() >> 24];
                str += KeyMaterial[random.GetUInt32() >> 24];

                key[i] = Convert.ToByte(str, 16);
            }

            return key;
        }
    }
}
