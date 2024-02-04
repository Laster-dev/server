dotnet publish -r win-x64 --self-contained -c Release -p:PublishAot=true -p:PublishSingleFile=false -p:EnableCompressionInSingleFile=true -p:PublishTrimmed=true
pause