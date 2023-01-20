start-job -scriptblock { dotnet run --project backend }
cd Client
start-job -scriptblock { http-server -p 9090 -a localhost --cors }
Start-Process http://localhost:9090/src/
Wait-Job -Any
