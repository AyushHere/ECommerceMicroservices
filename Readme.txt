cd Services
cd UserService
dotnet run --urls=http://localhost:5001
cd..

cd Services
cd ProductService
dotnet run --urls=http://localhost:5002
cd..
cd Services
cd OrderService
dotnet run --urls=http://localhost:5003
cd..
cd Services
cd NotificationService
dotnet run --urls=http://localhost:5004
cd..
cd Services
cd APIGateway
dotnet run --urls=http://localhost:5000
cd..