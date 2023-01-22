cd Client
start-job -scriptblock { http-server -p 9090 --cors }
cd ..
Start-Process http://localhost:9090/src/
dotnet run --project backend
