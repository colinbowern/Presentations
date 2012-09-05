TodoMVC Development Notes
=========================
To develop on this project you will need the following items:
* Visual Studio 2012 Premium or later
  - Set the AssemblyReferenceResolveMode property in C:\Program Files (x86)\Microsoft Visual Studio 11.0\Team Tools\Static Analysis Tools\FxCop\FxCopCmd.exe.config to StrongNameIgnoringVersion to fix an assembly resolution issue with Ninject (http://davesbox.com/archive/2008/06/14/reference-resolutions-changes-in-code-analysis-and-fxcop-part-2.aspx)
* IIS Express
* SQL Server Developer Edition
  - Reporting Services installed to /ReportManager and /ReportServer endpoints
  - Your user account has administrative access to the server
* SQL Server LocalDB
* ReSharper with XUnit Test Runner (see XUnitContrib project)

Creating a Development Server Certificate
=========================================
With WinHttpCertCfg downloaded open an elevated Visual Studio Command Prompt and change to a temporary folder.  Run the following commands to create a trusted root certificate authority and server certificate:

1. Create a CA Certificate:
    MakeCert.exe -r -pe -n "CN=%ComputerName% Test Certificate Authority" -a sha1 -sky signature -cy authority -sv CA.pvk CA.cer

2. Add the CA Certificate to the computer's Trusted Root Certificate Authorities store:
    CertUtil.exe -AddStore -Enterprise Root CA.cer

3. Create a Server Certificate signed by the CA Certificate:
    MakeCert.exe -pe -n "CN=%ComputerName%.%USERDNSDOMAIN%" -a sha1 -sky exchange -eku 1.3.6.1.5.5.7.3.1 -ic CA.cer -iv CA.pvk -sp "Microsoft RSA SChannel Cryptographic Provider" -sy 12 -sv Computer.pvk Computer.cer

4. Combine the public and private key of the Server Certificate into a PFX package:
    Pvk2Pfx.exe -pvk Computer.pvk -spc Computer.cer -pfx Computer.pfx

5. Add the Server Certificate to the computer's Personal store:
    CertUtil.exe -f -p "" -ImportPFX Computer.pfx

6. Grant access to the private key for the Network Service account:
    WinHttpCertCfg.exe -g -a "NETWORK SERVICE" -c LOCAL_MACHINE\My -s "%ComputerName%.%USERDNSDOMAIN%"