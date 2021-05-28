# Installation Guide

[Back](/README.md#install)

-   [Installation](#installation)
    -   [.NET 5](#dotnet)
    -   [Orion Context Broker](#orion-context-broker)
-   [Usage](#usage)


## Installation

#### .NET 5

This is the main tool to develop and run the microservice.
Download [here](https://dotnet.microsoft.com/download/dotnet/5.0).

#### Orion Context Broker

It is encouraged to use the official [Orion docker container at dockerhub](https://hub.docker.com/r/fiware/orion/). Other alternatives can be found [here](https://fiware-orion.readthedocs.io/en/master/admin/install/index.html#installing-orion).

## Usage

#### CLI

Restore dependencies
```
dotnet restore
```

Build project
```
dotnet build --no-restore
```

Run the project
```
dotnet run
```

### Docker

A Dockerfile is also available for your use. Prior to running the component, you must have Docker installed on your host. It is recommended to download the latest versions.

*Note:* the component was tested using the following versions:
* Docker version 20.10.6, build 370c289

Build image
```
docker build -t oeemicroservice .
```

Run container
```
docker run -it --rm -p 51803:80 --env CONTEXT_URL=http://172.17.0.1:1026 --name oeemicroservice_sample oeemicroservice
```
*Note:* Where the **CONTEXT_URL** is the URL of the Fiware context broker.
