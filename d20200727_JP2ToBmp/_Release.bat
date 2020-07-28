C:\Factory\Tools\RDMD.exe /RC out

COPY JP2ToBmp\JP2ToBmp\bin\Release\*.exe out
COPY JP2ToBmp\JP2ToBmp\bin\Release\*.dll out

C:\Factory\Tools\xcp.exe doc out

C:\Factory\SubTools\zip.exe /O out JP2ToBmp

PAUSE
