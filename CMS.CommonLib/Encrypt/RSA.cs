using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Xml;

namespace CMS.CommonLib.Encrypt
{
	/// <summary>
	/// RSA 的摘要说明。
	/// </summary>
	/// 
	public class RSA
	{
		
		public enum KeyFileProperty
		{
			PrivateKey,
			PublicKey,
			BadFormat,
		}


		private RSACryptoServiceProvider mProvider;
		private bool isPrivateKey;

		public KeyFileProperty CheckKeyXml(string KeyXml)
		{
			XmlDocument keyDoc = new XmlDocument();
			
			try
			{
				keyDoc.LoadXml(KeyXml);
				XmlElement root = (XmlElement) keyDoc.SelectSingleNode("RSAKeyValue");
				XmlElement chknode = (XmlElement)root.SelectSingleNode("DQ");
				
                if(chknode == null)
				{
					return KeyFileProperty.PublicKey;
				}
				else
				{
					return KeyFileProperty.PrivateKey;
				}
			}
			catch
			{
				return KeyFileProperty.BadFormat;
			}
			
	
		}


		public RSA()
		{
			mProvider = new RSACryptoServiceProvider();
			this.isPrivateKey=true;
		}


		public RSA(string KeyXML)
		{
			switch(CheckKeyXml(KeyXML))
			{
				case KeyFileProperty.PrivateKey:
					this.isPrivateKey=true;
					break;

				case KeyFileProperty.PublicKey:
					this.isPrivateKey=false;
					break;

				case KeyFileProperty.BadFormat:
					throw new Exception("key file failed!");
			}

			mProvider = new RSACryptoServiceProvider();
			mProvider.FromXmlString(KeyXML);
			


		}


		public string ExportKeys(bool includePrivateKey)
		{		
			if(includePrivateKey)
			{
				if(this.isPrivateKey)
				{
					return this.mProvider.ToXmlString(includePrivateKey);
				}
				else
				{
					return "";
				}
			}
			else
			{
				return this.mProvider.ToXmlString(includePrivateKey);
			}

		}


		public byte[] Encrypt(byte[]data)
		{
			return this.mProvider.Encrypt(data,false);
		}


		public byte[] Decrypt(byte[]data)
		{
			return this.mProvider.Decrypt(data,false);
		}

	}
}
