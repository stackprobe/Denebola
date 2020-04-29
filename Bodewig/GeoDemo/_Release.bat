C:\Factory\Tools\RDMD.exe /RM out

MD out\Client
MD out\Server

COPY /B Client\Client\bin\Release\Client.exe out\Client
COPY /B Client\Client\bin\Release\Chocolate.dll out\Client
COPY /B Server\Server\bin\Release\Server.exe out\Server
COPY /B Server\Server\bin\Release\Chocolate.dll out\Server

rem C:\Dev\Violet\Tools\CallConfuserCLI.exe Client\Client\bin\Release\Client.exe out\Client\Client.exe
rem C:\Dev\Violet\Tools\CallConfuserCLI.exe Server\Server\bin\Release\Server.exe out\Server\Server.exe

C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\全国住所緯度経度T\35138.conved.txt out\Server\全国住所緯度経度T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\全国住所緯度経度T\35139.conved.txt out\Server\全国住所緯度経度T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\全国住所緯度経度T\35140.conved.txt out\Server\全国住所緯度経度T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\全国住所緯度経度T\36138.conved.txt out\Server\全国住所緯度経度T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\全国住所緯度経度T\36139.conved.txt out\Server\全国住所緯度経度T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\全国住所緯度経度T\36140.conved.txt out\Server\全国住所緯度経度T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\東京都地図T\FG-GML-533924-ALL-20180701 out\Server\東京都地図T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\東京都地図T\FG-GML-533925-ALL-20180401 out\Server\東京都地図T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\東京都地図T\FG-GML-533934-ALL-20180701 out\Server\東京都地図T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\東京都地図T\FG-GML-533935-ALL-20180701 out\Server\東京都地図T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\東京都地図T\FG-GML-533944-ALL-20180701 out\Server\東京都地図T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\東京都地図T\FG-GML-533945-ALL-20180701 out\Server\東京都地図T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\東京都地図T\FG-GML-533954-ALL-20180701 out\Server\東京都地図T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\東京都地図T\FG-GML-533955-ALL-20180701 out\Server\東京都地図T
C:\Factory\Tools\zcp.exe /B C:\Erika\国土数値情報\東京道路網T out\Server

C:\Factory\Tools\xcp.exe res out

C:\Factory\SubTools\zip.exe /O out GeoDemo

PAUSE
