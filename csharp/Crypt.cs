using System;

public  class Crypt
{
		public void Action()
		{
				//'Shared 256 bit Key and IV here
				string sKy = "lkirwf897+22#bbtrm8814z5qq=498j5"; //'32 chr shared ascii string (32 * 8 = 256 bit)
				string sIV = "741952hheeyy66#cs!9hjv887mxx7@8y";  //'32 chr shared ascii string (32 * 8 = 256 bit)

				string sTextVal = "暗号化するテキスト!!!";
				string eText;
				string dText;

				eText = EncryptRJ256(sKy, sIV, sTextVal);
				dText = DecryptRJ256(sKy, sIV, eText);

				string msg = "";
				msg += "元のテキスト=" + sTextVal;
				msg += "\n";
				msg += "暗号化=" + eText;
				msg += "\n";
				msg += "復号化=" + dText;
				msg += "\n";

				//Debug.Log(msg);
		}

		//暗号化
		public static string EncryptRJ256(string prm_key, string prm_iv, string prm_text_to_encrypt)
		{
				string sToEncrypt = prm_text_to_encrypt;

				RijndaelManaged myRijndael = new RijndaelManaged();
				myRijndael.Padding = PaddingMode.Zeros;
				myRijndael.Mode = CipherMode.CBC;
				myRijndael.KeySize = 256;
				myRijndael.BlockSize = 256;

				byte[] encrypted;
				byte[] toEncrypt;

				byte[] key = new byte[0];
				byte[] IV = new byte[0];

				key = System.Text.Encoding.UTF8.GetBytes(prm_key);
				IV = System.Text.Encoding.UTF8.GetBytes(prm_iv);

				ICryptoTransform encryptor = myRijndael.CreateEncryptor(key, IV);

				MemoryStream msEncrypt = new MemoryStream();
				CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

				toEncrypt = System.Text.Encoding.UTF8.GetBytes(sToEncrypt);

				csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
				csEncrypt.FlushFinalBlock();
				encrypted = msEncrypt.ToArray();

				return (Convert.ToBase64String(encrypted));
		}



		//復号化
		public static string DecryptRJ256(string prm_key, string prm_iv, string prm_text_to_decrypt)
		{
				string sEncryptedString = prm_text_to_decrypt;
				RijndaelManaged myRijndael = new RijndaelManaged();

				myRijndael.Padding = PaddingMode.Zeros;
				myRijndael.Mode = CipherMode.CBC;
				myRijndael.KeySize = 256;
				myRijndael.BlockSize = 256;

				byte[] key = new byte[0];
				byte[] IV = new byte[0];

				key = System.Text.Encoding.UTF8.GetBytes(prm_key);
				IV = System.Text.Encoding.UTF8.GetBytes(prm_iv);

				ICryptoTransform decryptor = myRijndael.CreateDecryptor(key, IV);

				byte[] sEncrypted = Convert.FromBase64String(sEncryptedString);
				byte[] fromEncrypt = new byte[sEncrypted.Length];

				MemoryStream msDecrypt = new MemoryStream(sEncrypted);
				CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

				csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

				return (System.Text.Encoding.UTF8.GetString(fromEncrypt));

				//
				//string text = "";
				//using (var msDecrypt = new MemoryStream(sEncrypted))
				//{
				//		using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				//		{
				//				using (var srDecrypt = new StreamReader(csDecrypt))
				//				{
				//						text = srDecrypt.ReadToEnd();
				//				}
				//		}
				//}
				//return text;
		}
}
