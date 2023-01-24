winget install Microsoft.DotNet.SDK.6
cd Client
npm i live-server
start powershell {live-server --open=src/index.html --cors}
cd ..
start powershell {dotnet run --project backend}
