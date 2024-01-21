# Warehouse Pseudo Amazon Management App

We run a small warehouse connected to an online store. The database helps us organize the warehouse, manage the work of our employees, and serves our store, which operates as a web application.

## Building the project

You need to fetch latest postgres image, build the WebAPI image and run the PostgreSQL alongside the WebAPI Project. To do that, go to the /src/Warehouse.Backend and paste these commands:

    docker pull postgres
    docker build -t warehouse-webapi .
    docker compose up

By default, the API listens to the :5276 port

## Swagger

You can access the swagger page by going to the

    http://localhost:5276/swagger

## Updating the database schema

In order to update the PostgreSQL database with code-first approach, run these commands

    dotnet ef migrations add MigrationName --project Warehouse.Infrastructure --startup-project Warehouse.WebAPI
    dotnet ef database update --project Warehouse.Infrastructure --startup-project Warehouse.WebAPI