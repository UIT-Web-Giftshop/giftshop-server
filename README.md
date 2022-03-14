# giftshop-server

## Install and run

### Run with Visual studio
- Install Mongodb and create `giftshop-demo` database
- Add `products` collection to the database
- Install dotnet core 5.0 from https://dotnet.microsoft.com/download/dotnet-core/
- Open Visual Studio and run API project (Do not run IIS option)
- Open browser from link on CMD/console

### Run with docker
- Run `dotnet dev-certs https -eq $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p local`
- Run `dotnet dev-certs https --trust`
- At solution folder run `docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up -d` to start API for the latest build
- If no need to build again, just run `docker-compose up -d`
- run `docker-compose down` to stop all service
- Open browser `https://localhost:5001/swagger`, `http://localhost:5000/swagger` for API
- Open browser `http://localhost:3000/` for MongoDb GUI or use interact with mongo shell in docker
- More detail usage on dockerhub https://hub.docker.com/

### For use MongoClient web browser
- Connection string `mongodb://contextdb:27017`
- Select database `giftshop-demo` like picture
- ![img_1.png](img_1.png)
- Enjoy query selector and docker magic ^^