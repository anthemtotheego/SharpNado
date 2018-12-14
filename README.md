# SharpNado

Description
============
SharpNado is a proof of concept tool written in C# that demonstrates how you could use Windows Communication Foundation (WCF) or .NET Remoting as another way to host and call your .NET payloads.  It contains very basic examples of how one could do this and was written with the intention to be used as a building block or something to get those creative juices flowing. You really have the option to take this as far as you want.  I have only demostrated a few simple ways this could be used and provided enough code to get you up and running. So if you are looking for a tool that does ALL the work for you, this is not that tool...just a POC. :)

Contact at:
- Twitter: @anthemtotheego

Blog:

A few examples taken from my blog of why this might be useful:

-Your malicious code you want to execute sits behind an interface and resides in a remote server side method that is called by your stager. In the examples I have provided, when the stager is executed on the client it makes a remote call to the exposed server interface and executes the service method(s) you want.  The service method that is called sends a string array to the client stager (encypted if we want) that either contains a base64 .Net Assembly to be executed in memory or C# source code to be built and executed in memory. Keep in mind this all could be expanded on, including languages, protocols, using redirectors, etc..  

Continuing with this, if the blue team were to find your hosting server, they can't just easily download/copy/reverse engineer your payloads and burn all your hard work because they aren't just chilling in some open web directory waiting to be downloaded by anyone with an internet connection. Secondly, if they analyze your stager payload, they will only see that it makes a remote callout, retrieves a string array containing god knows what, and executes something in memory or builds and executes something in memory.  This helps keep the stager light or small in size and keeps it clean of malicious code that could get flagged more easily. One thing to be aware of however, is you would still want to build in logic server side that doesn't just automatically send our payload but instead sends something non-malicious or nothing at all if we think we are being sandboxed and analyzed or sends our payload encrypted and only decrypts it if we believe it is safe to do so.  There are many ways one could do this and I will leave that up to you but I did leave you with a few simple examples to build upon. In the end the goal would be that by the time an analyst is looking at our stager, we have built in enough safeguards that our server service was killed long before they had a chance of potentially finding out what malicous code we were planning on running. 

-My favorite feature is it allows flexability and you to dynmaically change out your payloads quickly server side without ever having to change your stager program.  For example, you send 10 stager payloads to a large company that will retrieve and execute your malicious payload if the domain name matches X and the username is correct.  After your first stager fires it sends back to the server that it hit our target, the domain was correct and it executed our malicious C# no Powershell payload that should have spawned an Empire shell. Unfortunately no shell ever comes.  Traditionally, you would have to go back and create either new payloads to resend to 10 new people and hope for the best again or maybe create a new payload, name it with the same name and replace it in the web directory your stager is reaching out to. You also might not get any warnings that your payload had been fired at all.  Within the SharpNado console application, we can get notifications our stagers fired, retrieve notifications from our stagers, like what the correct domain name actually is, or quickly change our payload(s) to something different, like in this example, we could set the payload to our malicious C# no Powershell payload that spawns a Cobalt Strike beacon and now the next time any of the stagers that were already sent get executed, the new Cobalt Strike beacon payload will fire instead.  Or you could use a completely different .NET payload depending on the situation.  The cool part is, once you add all your payloads to the server using simple XML templates, you can easily switch between them at any time.  Making this useful for internal pen testing as well.

Continuing on with this, another example of how this could be useful is lets say in the last example we didn't get the domain correct.  Well if that is hardcoded into our 10 stager payloads, they all would be lost, 0 out of 10 would fire.  However, we could easily create a stager that once fired, sends us the username and domain information and on the server side build in logic that if the username is correct but domain name isn't, give us the option to choose to fire our payload anyways or we could have already built in server logic that allows us to quickly change the domain so that when the next 9 stagers fire, it now runs our malicious payload.  How you spin it and how far you go is really up to you.

-Lastly, .NET Remoting and WCF have been around FOREVER.  There are tons of examples out there from developers on lots of ways to use this technology legitimately and it is probably a pretty safe bet that there are still a lot of organizations using this technology in legit applications. Like you, I like finding ways one might do evil things with things people use for legit purposes.  This same concept could be used with other technologies as well like web API's etc.

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
