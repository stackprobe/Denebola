C:\Factory\Tools\RDMD.exe /RC out

COPY ConvCsv\ConvCsv\bin\Release\ConvCsv.exe out
COPY C:\Dev\CSharp\Chocolate\Chocolate\bin\Release\Chocolate.dll out

C:\Factory\Tools\xcp.exe doc out

C:\Factory\SubTools\zip.exe /O out ConvCsv_201905

PAUSE
