using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SeminarFramework
{
    public class CacheProvider
    {
        static byte[] additionalEntropy = { 5, 3, 7, 1, 5 };

        public void ChacheConnections(List<ConnectionString> connections)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ConnectionString>));
                MemoryStream memoryStream = new MemoryStream();
                XmlWriter xmlWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xmlSerializer.Serialize(xmlWriter, connections);
                byte[] protectData = Protect(memoryStream.ToArray());
                File.WriteAllBytes($"{AppDomain.CurrentDomain.BaseDirectory} data.protected", protectData);
            }
            catch (Exception e)
            {

                Console.WriteLine("Serialize data error");
            }

        }

        public List<ConnectionString> GetConnectionFromCache()
        {
            try
            {

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ConnectionString>));
                byte[] data = File.ReadAllBytes($"{AppDomain.CurrentDomain.BaseDirectory} data.protected");
                return (List<ConnectionString>)xmlSerializer.Deserialize(new MemoryStream(data));
            }
            catch (Exception e)
            {

                Console.WriteLine("Deserialize data error");
                return null;
            }



        }

        private byte[] Protect(byte[] data)
        {

            try
            {
                return ProtectedData.Protect(data, additionalEntropy, DataProtectionScope.LocalMachine);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Protected error.");
                return null;
            }

        }

        public byte[] Unprotect(byte[] data)
        {
            try
            {
                return ProtectedData.Unprotect(data, additionalEntropy, DataProtectionScope.LocalMachine);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Unprotect error.");
                return null;
            }

        }
    }
}
