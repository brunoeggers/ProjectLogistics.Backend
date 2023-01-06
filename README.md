# Backend
This repository is for the backend.

For the frontend, go to <https://github.com/brunoeggers/ProjectLogistics.Frontend>

## Tech Stack
C# .Net 6 using Visual Studio 2022

SQLite

## Setup
1. Clone this repository and open ProjectLogistics.sln in Visual Studio
2. Restore Nuget packages if needed and compile the entire solution
3. Set the ProjectLogistics.API project as the main startup project
4. Start (with or without debuging)

## Database
A SQLite database will be created automatically as soon as the application is run for the first time. Some seed data will be inserted to populate the database. You can reset it at any time by deleting the logistics_db.sqlite file in ProjectLogistics.API and running the application again.

By default a warehouse will be created with an address from San Francisco, CA. You can edit this by changing the latitude and longitude coordinates in the Warehouse table inside the database.

## Remarks
1. I've created a storage solution assuming the warehouse will have aisles (rows) and a shelves. For simplicity sake, a shelve can only store one package and has only one level. In real life shelves could have multiple bays and levels.
2. I wanted to improve how DTOs and Entities are handled. I've started to add automapper to do that, but I ran out of time. This will not compromise anything in the project.
