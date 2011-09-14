@echo Visual studio can report that the build failed because of 0 errors found.
@echo The only way to recover from this is to find the errors.
@echo This program performs a command line build of the system and puts the errors into results.txt.


C:\WINDOWS\Microsoft.NET\Framework\v3.5\msbuild.exe /verbosity:diagnostic C:\EDrive\SourceVS2008\AT1.1\at1.1.sln >results.txt
pause

