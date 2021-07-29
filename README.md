Copyright (c) 2021, Matthew Carr. All rights reserved.

### Overview
This repository contains the code for the backend of my wedding website.
This README is mainly a note to myself.
It is an ASP.NET app that runs on .NET 5 and uses a SQLite database for persistence.

### How to Deploy
0) Spin up a Linux server and setup SSH access to it. Suppose you login
   to the server with `ssh user@host`. The account you use to login must
   have sudo privileges.
1) Prepare the server by running `./prepare-server user@host`. This will
   create the user `weddingapp` with the home directory `/var/lib/weddingapp`.
   This will be the directory where the app will be deployed and which will
   hold its SQLite database.
2) Run `./deploy user@host` to build and deploy the app to the server. This
   will run `dotnet publish ...` and copy the output directory to
   `/var/lib/weddingapp/WeddingApp` on the server.
3) If the above succeeded then the app will now be running on the server
   and listening on `localhost:5050`.
4) Configure a web server as a reverse proxy and point it at `localhost:5050`
   to complete the setup.
5) When it comes time to export data from the app, login to the server over
   SSH and navigate to `/var/lib/weddingapp/WeddingApp`. The you can run
   the CLI tool with `./WeddingApp.Cli [command]`. If you run the `WeddingApp.Cli`
   binary with no arguments, then a list of commands will be printed.
