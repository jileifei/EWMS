using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace CMS.CommonLib.Json
{
      public static class JsonUtility  
     {  
         public static string ObjectToJson(object item)  
         {  
             DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());  
             using (MemoryStream ms = new MemoryStream())  
             {  
                 serializer.WriteObject(ms, item);  
                 StringBuilder sb = new StringBuilder();  
                 sb.Append(Encoding.UTF8.GetString(ms.ToArray()));  
                 return sb.ToString();  
             }  
         }  
   
         public static T JsonToObject<T>(string jsonString)  
         {  
             DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));  
             MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));  
             T jsonObject = (T)ser.ReadObject(ms);  
             ms.Close();  
             return jsonObject;  
         }  
     }  
}
