Log4NetWithDINinject
====================

Basic Log4Net example using Ninject for DI. 

I wanted a base starting point with DI and Log4Net installed and wired up with an IoCContainer. This is it.

There are 4 projects in all:

Core: only has ILogger in it because this interface needs to be accessable to several projects that would 
cause circular redundancy otherwise.

DIContainer: only has the IOCContainer in it.

Services: which contains 2 fake servies as examples.

LogWriter4: which is currently a console application. Next itteration and I will separate out the logging. 
Set this as the startup project and run it. 


UPDATE: 2/9/2014: 12:47 PM
Moved logging to it's own project. This is a better way to organize the code. The main program uses everything as librarys and doesn't know anything about the implementation.

Logger: project contains all logging code. It has 3 implementatioins.
  1) a Console logger that writes to the debug console only
  2) two Log4Net ecamples: the first (isn't necessary) implements Log4Net the same way the Console Logger is implemented. The second is more of a wrapper. I did the second one as an excersize and it can be deletd along with makeing changes to the setup Modules that bind Ninject dependancies if they uase this logger.
