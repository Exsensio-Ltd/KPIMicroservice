# OEE Microservice

![GitHub](https://img.shields.io/github/license/Exsensio-Ltd/OEEMicroservice)
![CI](https://github.com/Exsensio-Ltd/OEEMicroservice/actions/workflows/dotnet.yml/badge.svg)
[![CII Best Practices](https://bestpractices.coreinfrastructure.org/projects/4187/badge)](https://bestpractices.coreinfrastructure.org/projects/4187)

This project is part of [DIH^2](http://www.dih-squared.eu/).

## Contents

-   [Background](#background)
-   [Install](#install)
-   [Usage](#usage)
-   [API](#api)
-   [Testing](#testing)
-   [License](#license)

## Background

OEE Microservice provides API to interact with Fiware context broker to store test metrics, station metadata, calculate OEE and build time series data for Grafana plugin.

## Install

Information about how to install the OEEMicroservice can be found at the corresponding section of the [Installation Guide](https://github.com/Exsensio-Ltd/OEEMicroservice/blob/master/docs/installationguide.md).

## Usage

Information about how to use the the connectivity kit can be found in the [User & Programmers Manual](https://github.com/Exsensio-Ltd/OEEMicroservice/blob/master/docs/usermanual.md).


## API

More information about API usage of the OEE microservice can be found in [API documentation](https://github.com/Exsensio-Ltd/OEEMicroservice/blob/master/docs/api.md)

## Testing

The project contains integration and unit test implemented with xUnit and MSTest. To run tests use the following command:

```
dotnet test --no-build --verbosity quiet
```

## License

[Apache2.0](LICENSE) © Exsensio Ltd
