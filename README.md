# Folder Scanner

## Description
Write a simple application that detects changes in a local folder. During the first run the app will analyze the content of the given folder. During each additional run the app will return changes detected since last run:

* List of new files and subfolders
* List of modified files (change of file content)
* List of deleted files and subfolders

For each file maintain the version. At the beginning all files will have version equal to 1. With each  detected change, the version is increased by 1.

Assume that the size of files in given folder will be less than 50MB and that number of files in each folder will be 100 at most.

App will be executed from the UI by clicking on a button (doesn't detect changes automatically).

Do not use databases.

## How to run

You can either run backend and frontend separately or launch the whole app via docker compose.

### Backend

Requirements: .Net 7

    cd Be\FolderScanner
    dotnet run

### Frontend

Requirements: Node.js 16

    cd Fe
    pnpm i
    pnpm dev

### Docker compose

    docker-compose up

Then just open `http://localhost:5055/` in your browser.

Folder `DockerCompose` located in the root of this repo is mounted to the `DockerCompose` folder in the backend container. Therefore all files you place and modify in this folder will be scanned by the application.
