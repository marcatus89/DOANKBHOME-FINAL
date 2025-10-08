dotnet run --project DoAnTotNghiep.csproj
dotnet build DoAnDemo-master.sln

dotnet ef migrations add InitialCreate
dotnet ef database update

