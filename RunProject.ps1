cd Client
start-job -scriptblock { http-server -p 9090 --cors }
cd ..
Start-Process http://127.0.0.1:8080/Client/src/
dotnet run --project backend
