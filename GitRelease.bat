IF NOT EXIST .\GitRelease.bat GOTO END
CALL qq
C:\Factory\SubTools\GitFactory.exe /ow . C:\huge\GitHub\Denebola
rem CALL C:\var\go.bat
:END
