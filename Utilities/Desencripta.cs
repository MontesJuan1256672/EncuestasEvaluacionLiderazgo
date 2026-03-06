using System.Security.Cryptography;
using System.Text;

namespace EncuestasEvaluacionLiderazgo.Utilities
{
    public class Desencripta
    {
        // <summary>
        /// M  MEtodo Descencripta QueryString de CENTREX
        /// </summary>
        /// <param name="NombrePam"> a</param>
        /// <param name="valParam"></param>
        /// <returns></returns>
        public static string DecryptWitValueCENTREX(string NombreParam, string valParam)
        {
            //string sValue = "";
            string parametro = valParam;//Request.QueryString[Param].ToString();

            if (parametro.Length < 7)
                return parametro;

            CentrexEncrypt.CentrexEncrypt decrypter = new CentrexEncrypt.CentrexEncrypt();
            //string parametroDesencriptado = decrypter.Decrypt(parametro, "$C3NtR3x#2016" + DateTime.Today.ToString("dd/MM/yyyy"));
            //return parametroDesencriptado;

            string stringToDecrypt = parametro;
            string sEncryptionKey = "$C3NtR3x#2016" + DateTime.Today.ToString("dd/MM/yyyy");
            byte[] key = { };
            byte[] IV = { 0xa5, 0x53, 0xc7, 0xf1, 0x18, 0x52, 0x7b, 0xcc };
            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];

            try
            {
                stringToDecrypt = stringToDecrypt.Replace(" ", "+");
                int mod4 = stringToDecrypt.Length % 4;
                if (mod4 > 0)
                {
                    stringToDecrypt += new string('=', 4 - mod4);
                }

                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                //inputByteArray = Convert.FromBase64String(stringToDecrypt);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Encripta un valor con el mismo algoritmo DES de CENTREX (proceso inverso de DecryptWitValueCENTREX)
        /// </summary>
        /// <param name="plainText">Texto plano a encriptar</param>
        /// <returns>Texto encriptado en Base64</returns>
        public static string EncryptWitValueCENTREX(string plainText)
        {
            byte[] key = { };
            byte[] IV = { 0xa5, 0x53, 0xc7, 0xf1, 0x18, 0x52, 0x7b, 0xcc };

            try
            {
                string sEncryptionKey = "$C3NtR3x#2016" + DateTime.Today.ToString("dd/MM/yyyy");
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = System.Text.Encoding.UTF8.GetBytes(plainText);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  DesEncripta QueryString de TELVISTA SERVICES
        /// </summary>
        /// <param name="cryptTxt"> Envia el IDpersonal Encriptado</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string idPersonalDecrypt(string cryptTxt, string key)
        {
            //  EncryptionKey = "T31v15t4L1f32020Informatica";
            key = "T31v15t4L1f32020Informatica";
            cryptTxt = cryptTxt.Replace(" ", "+");
            byte[] bytesBuff = Convert.FromBase64String(cryptTxt);

            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                aes.Key = crypto.GetBytes(32);
                aes.IV = crypto.GetBytes(16);

                using (System.IO.MemoryStream mStream = new System.IO.MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cStream.Write(bytesBuff, 0, bytesBuff.Length);
                        cStream.Close();
                    }
                    cryptTxt = System.Text.Encoding.Unicode.GetString(mStream.ToArray());
                }
            }

            return cryptTxt;

        }



        ///ENCRIPTA IDPERSONAL TElVISTASERVICES

        ////IdPersonalEncrypt(Settings.IdPersonal, EncryptionKey);
        public static string IdPersonalEncrypt(string inText, string key)
        {
            //string EncryptionKey = "T31v15t4L1f32020Informatica";
            key = "T31v15t4L1f32020Informatica";
            byte[] bytesBuff = Encoding.Unicode.GetBytes(inText);

            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                aes.Key = crypto.GetBytes(32);
                aes.IV = crypto.GetBytes(16);

                using (System.IO.MemoryStream mStream = new System.IO.MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cStream.Write(bytesBuff, 0, bytesBuff.Length);
                        cStream.Close();
                    }

                    inText = Convert.ToBase64String(mStream.ToArray());
                }
            }
            return inText;
        }
    }

}
