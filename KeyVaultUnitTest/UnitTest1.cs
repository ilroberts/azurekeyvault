using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;

using KeyVaultTest1.Helpers;

namespace KeyVaultUnitTest
{
    [TestClass]
    public class EncryptionTest
    {
        [TestMethod]
        public void TestEncrypt()
        {
            using (var random = new RNGCryptoServiceProvider())
            {
                var key = new byte[16];
                random.GetBytes(key);

                var testString = "now is the winter of our discontent made glorious summer by this son of york";

                byte[] result = Encryption.EncryptStringToBytes_Aes(testString, key);
                string decryptedResult = Encryption.DecryptStringFromBytes_Aes(result, key);

                Assert.AreEqual(testString, decryptedResult);
            }
        }
    }
}
