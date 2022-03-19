# giftshop-server

## Install and run

### Run with Visual studio
- Install Mongodb. Connect and create new `giftshop-demo` database & `products` collection
- Install dotnet core 5.0 from https://dotnet.microsoft.com/download/dotnet-core/
- Open Visual Studio and run API project (Do not run IIS option)
- Open browser from link on CMD/console

### Run with docker
- Run `dotnet dev-certs https -eq $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p local`
- Run `dotnet dev-certs https --trust`
- At solution folder run `docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up -d` to start API for the latest build
- If no need to build again, just run `docker-compose up -d`
- Run `docker-compose down` to stop all and remove all container
- Open browser `https://localhost:5001/swagger`, `http://localhost:5000/swagger` for API
- Interact with mongo shell `docker exec -it ${Mongo containerId} mongo`
- More detail usage on dockerhub https://hub.docker.com/.
- Mongoshell docs https://docs.mongodb.com/manual/reference/mongo-shell/

- Enjoy docker magic with fun ^^
- *Use SeedData endpoint for seeding some sample data*
