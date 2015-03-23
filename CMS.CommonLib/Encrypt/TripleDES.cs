using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace CMS.CommonLib.Encrypt
{
	/// <summary>
	/// TripleDES 的摘要说明。
	/// </summary>
	public class TDES
	{
		private static string[] Keys={"ASvfj4YszCU5Dln7Qta7a8YmjEode0tn","5I81bpFKGE1i/cgbfXaqTaur3se2tLbF","NJPeHFXnrlGdo/gbYAOSn9az0EWtAVZ2","03v8dFD4+m6YrnraZCwcXEZoNdfDysvG","vUlb1fYVjk1XjsB1yFwkssQvM3gjTkAT","LruoGB1qSNjR1gbK5QUh4KnI4VLop5cc","K/driYRw5svVpyyyDFnfUUdcym8Jjpdh","GHJEFHSuSumRi+H4qFcITL6Rcrm7hHE7","IUDKdc9jDxyBMoHMs8kNLJQv6Rv+jnyF","SqdGjfJ2eBQZbI/FVyF/+Rw0xipgyTc8"};
		private static string[] IVs={"x6ILMV6XC7c=","sU+0klWi5FE=","FM1oikdpccA=","VrYlstL6AFM=","JXi1xQUtNSg=","YkivON1sioU=","7AK/FpPYeck=","XBuI8xt7LEM=","MSj65SecUEw=","6pucdHa4eaA="};

		private TripleDES des;
		private int v;

		public int V
		{
			get
			{
				return v;
			}
		}


		public TDES()
		{
			des=TripleDES.Create();
		}

		public byte[] Key
		{
			get
			{
				return des.Key;
			}
			set
			{
				des.Key=value;
			}
		}

		public byte[] IV
		{
			get
			{
				   return des.IV;
			   }
			set
			{
				des.IV=value;
			}
		}
		public string Encrypt(byte[] data)
		{
			MemoryStream ms=new MemoryStream();
			Random ra=new Random();
			v=ra.Next(0,9);
			des.Key=Convert.FromBase64String(Keys[v]);
			des.IV=Convert.FromBase64String(IVs[v]);
			CryptoStream cs=new CryptoStream(ms,des.CreateEncryptor(),CryptoStreamMode.Write);
			cs.Write(data,0,data.Length);
			cs.Flush();
			cs.Close();
			return Convert.ToBase64String(ms.ToArray());
		}

		public string Decrypt(string data,int vi)
		{
			MemoryStream ms = new MemoryStream();
			Byte[] tb = Convert.FromBase64String(data);
			des.Key = Convert.FromBase64String(Keys[vi]);
			des.IV = Convert.FromBase64String(IVs[vi]);
			CryptoStream cs = new CryptoStream(ms,des.CreateDecryptor(),CryptoStreamMode.Write);
			cs.Write(tb,0,tb.Length);
			cs.Flush();
			cs.Close();
			return Encoding.Unicode.GetString(ms.ToArray());
		}

		public string Decrypt(string data)
		{
			MemoryStream ms = new MemoryStream();
			Byte[] tb = Convert.FromBase64String(data);
			CryptoStream cs = new CryptoStream(ms,des.CreateDecryptor(),CryptoStreamMode.Write);
			cs.Write(tb,0,tb.Length);
			cs.Flush();
			cs.Close();
			return Encoding.Unicode.GetString(ms.ToArray());
		}
	}
}
