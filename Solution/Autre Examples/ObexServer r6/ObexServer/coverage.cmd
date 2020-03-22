setlocal
set NCoverExe="c:\Program Files\NCover\NCover.Console.exe"
set NunitExe="d:\Program Files\NUnit-Net-2.0 2.2.8\bin\nunit-console.exe"
set TestAssembly="ObexServer.Test\bin\Debug\ObexServer.Test.dll"
rem
%NCoverExe%  %NunitExe% %TestAssembly%
rem
