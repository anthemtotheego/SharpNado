# SharpNado

Description
============
SharpNado is a proof of concept tool written in C# that demonstrates how you could use Windows Communication Foundation (WCF) or .NET Remoting to host smarter and dynamic .NET payloads.  It contains very basic examples of how one could do this and was written with the intention to be used as a building block or something to get those creative juices flowing. You really have the option to take this as far as you want.  I have only demostrated a few simple ways this could be used and provided enough code to get you up and running. So if you are looking for a tool that does ALL the work for you, this is not that tool...just a POC. :)

For more information, please see blog post below.

Contact at:
- Twitter: @anthemtotheego

Blog:

**Before submitting issues, this tool may not always be updated actively. I encourage you to borrow, add, mod, and/or make your own.  Remember, all the awesome code out there (and there is a lot) can be taken/modified to create your own custom tools.**

![Alt text](/sharpnadoimg1.PNG?raw=true "SharpNado")
![Alt text](/sharpnadoimg2.PNG?raw=true "")

Setup - Quick and Dirty
==============================

**Note: For those of you who don't want to go through the trouble of compiling your own I uploaded an x64 and x86 binary found in the CompiledBinaries folder. I have also included stager examples that must be compiled seperately found in the Stagers folder and XML Payload template examples found in the Payloads folder.  As always, those of you who do want to compile your own... I used Windows 10, Visual Studio 2017 - mileage may vary**

1. Download and open up SharpNado.sln in Visual Studio.

2. Inside visual studio, right click References on the righthand side, choose Add Reference, then under Assemblies add a reference to      System.Runtime.Remoting and System.ServiceModel.  Side Note - System.ServiceModel reference needs added to all WCF Stagers and          System.Runtime.Remoting to each .NET Remoting stager. 

3. Compile (make sure to compile for correct architecture) - Should see drop down with Any CPU > Click on it and open                      Configuration Manager > under platform change to desired architecture and select ok.

4. (If multiple IPs on server) Configure your App.config file to make sure interface is opened on correct IP server side.

<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6.1" />
    </startup>
  <system.runtime.remoting>
    <application>
      <channels>
        <channel ref="put tcp or http here" port="put port here" bindTo="put your IP here" />
      </channels>
    </application>
  </system.runtime.remoting>
</configuration>

5. If you like, add custom methods, stagers, payloads, interfaces...take it as far as you want.

6. Run SharpNado.exe as administrator on your server to start the console application and type help for options.
  
7. Stager examples (what is executed client side) and payload template examples (what the server uses to send your payload to the          stagers) can be found in the folders Stagers and Payloads.

8. If you run into issues, there are tons of tutorials out there to help fill in the gaps when it comes to WCF or .NET Remoting that I      skipped over or you can read my associated blog post found towards the top of the page but even that won't fill in all the gaps as it    will explain more about how to use the tool itself.

Examples 
========

Note:  All commands are case insensitive

Starts SharpNado console application

```SharpNado.exe```

Displays help options

```SharpNado:> help ```

Shows service options that can be changed via the set command

```SharpNado:> show serviceOptions```

Shows payload options that can be changed via the set command

```SharpNado:> show payloadOptions```

Shows available payloads that can be set for SharpNado to use

```SharpNado:> Show payloads```

Starts listening service using options set in service options menu - When running mulitple services, run first service then change service options and run again to open another listening service - rinse and repeat

```SharpNado:> run```

Stop all listening services

```SharpNado:> stop```

sets option variables - When setting payloads just use the name and do not add .xml

```SharpNado:> set srvhost 192.168.0.10```

```SharpNado:> set port 8080```

```SharpNado:> set base64_1 MyBase64PayloadName```

```SharpNado:> set encrypt MyBase64PayloadName```

```SharpNado:> set compile_1 MySourceCompilePayloadName```
