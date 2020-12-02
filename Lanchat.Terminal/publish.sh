dotnet publish -c Release -p:PublishSingleFile=true --self-contained false -r win-x86
dotnet publish -c Release -p:PublishSingleFile=true --self-contained false -r win-x64
dotnet publish -c Release -p:PublishSingleFile=true --self-contained false -r linux-x64
dotnet publish -c Release -p:PublishSingleFile=true --self-contained false -r linux-arm
dotnet publish -c Release -p:PublishSingleFile=true --self-contained false -r linux-arm64
dotnet publish -c Release -p:PublishSingleFile=true --self-contained false -r osx-x64

mv bin/Release/net5.0/win-x86/publish/Lanchat.exe bin/Release/net5.0/win-x86/publish/Lanchat_win-x86.exe
mv bin/Release/net5.0/win-x64/publish/Lanchat.exe bin/Release/net5.0/win-x64/publish/Lanchat_win-x64.exe
mv bin/Release/net5.0/linux-x64/publish/Lanchat bin/Release/net5.0/linux-x64/publish/Lanchat_linux-x64
mv bin/Release/net5.0/linux-arm/publish/Lanchat bin/Release/net5.0/linux-arm/publish/Lanchat_linux-arm
mv bin/Release/net5.0/linux-arm64/publish/Lanchat bin/Release/net5.0/linux-arm64/publish/Lanchat_linux-arm64
mv bin/Release/net5.0/osx-x64/publish/Lanchat bin/Release/net5.0/osx-x64/publish/Lanchat_osx-x64