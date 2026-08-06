using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SERVICE
{
    
    public static class SV_Encriptacion
    {
        private const string Prefijo = "AES:";
        private static readonly byte[] Salt = Encoding.UTF8.GetBytes("CampoDiplomaSalt16");

        private static string ObtenerClave()
        {
            string key = ConfigurationManager.AppSettings["EncryptionKey"];
            if (string.IsNullOrWhiteSpace(key))
                key = "CampoDiploma2026-ClaveAES!";
            return key;
        }

        private static byte[] DerivarClave(string password)
        {
            using (var derive = new Rfc2898DeriveBytes(password, Salt, 10000, HashAlgorithmName.SHA256))
            {
                return derive.GetBytes(32);
            }
        }

        public static string Encriptar(string textoPlano)
        {
            if (string.IsNullOrEmpty(textoPlano))
                return textoPlano;

            if (EstaEncriptado(textoPlano))
                return textoPlano;

            byte[] plainBytes = Encoding.UTF8.GetBytes(textoPlano);
            byte[] key = DerivarClave(ObtenerClave());

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plainBytes, 0, plainBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return Prefijo + Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Desencriptar(string textoEncriptado)
        {
            if (string.IsNullOrEmpty(textoEncriptado))
                return textoEncriptado;

            if (!EstaEncriptado(textoEncriptado))
                return textoEncriptado;

            string payload = textoEncriptado.Substring(Prefijo.Length);
            byte[] fullCipher = Convert.FromBase64String(payload);
            byte[] key = DerivarClave(ObtenerClave());

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] cipher = new byte[fullCipher.Length - iv.Length];
                Array.Copy(fullCipher, 0, iv, 0, iv.Length);
                Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream(cipher))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static bool EstaEncriptado(string texto)
        {
            return !string.IsNullOrEmpty(texto) && texto.StartsWith(Prefijo, StringComparison.Ordinal);
        }

        public static void EncriptarArchivo(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
                return;

            string contenido = File.ReadAllText(rutaArchivo, Encoding.UTF8);
            if (EstaEncriptado(contenido))
                return;

            File.WriteAllText(rutaArchivo, Encriptar(contenido), Encoding.UTF8);
        }

        public static string LeerArchivoDesencriptado(string rutaArchivo)
        {
            string contenido = File.ReadAllText(rutaArchivo, Encoding.UTF8);
            return Desencriptar(contenido);
        }
    }
}
