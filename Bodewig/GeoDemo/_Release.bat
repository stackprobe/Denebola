C:\Factory\Tools\RDMD.exe /RM out

MD out\Client
MD out\Server

COPY /B Client\Client\bin\Release\Client.exe out\Client
COPY /B Client\Client\bin\Release\Chocolate.dll out\Client
COPY /B Server\Server\bin\Release\Server.exe out\Server
COPY /B Server\Server\bin\Release\Chocolate.dll out\Server

rem C:\Dev\Violet\Tools\CallConfuserCLI.exe Client\Client\bin\Release\Client.exe out\Client\Client.exe
rem C:\Dev\Violet\Tools\CallConfuserCLI.exe Server\Server\bin\Release\Server.exe out\Server\Server.exe

C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�S���Z���ܓx�o�xT\35138.conved.txt out\Server\�S���Z���ܓx�o�xT
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�S���Z���ܓx�o�xT\35139.conved.txt out\Server\�S���Z���ܓx�o�xT
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�S���Z���ܓx�o�xT\35140.conved.txt out\Server\�S���Z���ܓx�o�xT
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�S���Z���ܓx�o�xT\36138.conved.txt out\Server\�S���Z���ܓx�o�xT
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�S���Z���ܓx�o�xT\36139.conved.txt out\Server\�S���Z���ܓx�o�xT
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�S���Z���ܓx�o�xT\36140.conved.txt out\Server\�S���Z���ܓx�o�xT
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�����s�n�}T\FG-GML-533924-ALL-20180701 out\Server\�����s�n�}T
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�����s�n�}T\FG-GML-533925-ALL-20180401 out\Server\�����s�n�}T
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�����s�n�}T\FG-GML-533934-ALL-20180701 out\Server\�����s�n�}T
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�����s�n�}T\FG-GML-533935-ALL-20180701 out\Server\�����s�n�}T
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�����s�n�}T\FG-GML-533944-ALL-20180701 out\Server\�����s�n�}T
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�����s�n�}T\FG-GML-533945-ALL-20180701 out\Server\�����s�n�}T
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�����s�n�}T\FG-GML-533954-ALL-20180701 out\Server\�����s�n�}T
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�����s�n�}T\FG-GML-533955-ALL-20180701 out\Server\�����s�n�}T
C:\Factory\Tools\zcp.exe /B C:\Erika\���y���l���\�������H��T out\Server

C:\Factory\Tools\xcp.exe res out

C:\Factory\SubTools\zip.exe /O out GeoDemo

PAUSE
