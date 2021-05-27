# OEE Microservice

![GitHub](https://img.shields.io/github/license/Exsensio-Ltd/OEEMicroservice)
![CI](https://github.com/Exsensio-Ltd/OEEMicroservice/actions/workflows/dotnet.yml/badge.svg)
[![CII Best Practices](https://bestpractices.coreinfrastructure.org/projects/4187/badge)](https://bestpractices.coreinfrastructure.org/projects/4187)

OEE (Overall Equipment Effectiveness) is the gold standard for measuring manufacturing productivity. Simply put – it identifies the percentage of manufacturing time that is truly productive. An OEE score of 100% means you are manufacturing only Good Parts, as fast as possible, with no Stop Time. In the language of OEE that means 100% Quality (only Good Parts), 100% Performance (as fast as possible), and 100% Availability (no Stop Time).

Measuring OEE is a manufacturing best practice. By measuring OEE and the underlying losses, you will gain important insights on how to systematically improve your manufacturing process. OEE is the single best metric for identifying losses, benchmarking progress, and improving the productivity of manufacturing equipment (i.e., eliminating waste).

The microservice allows you to capture and analyze OEE of Station / cell in Manufacturing line. This Microservice can be used with existing software systems along with orion context broker. The Graphana Pluggin allows you to visually the OEE Metric (Availablity, Quality and Performance indicators) by a given station and a given product.

The vision here is that no matter what the product or the station in the prodcution line, we will be able to capture the OEE and this value will help the operation mangers see the bottle necks int the production line and also usage of the equipments.

## Contents

-   [Background](#background)
-   [Install](#install)
-   [API](#api)
-   [Testing](#testing)
-   [License](#license)

## Background

OEE Microservice provides API to interact with Fiware context broker to store test metrics, station metadata, calculate OEE and build time series data for Grafana plugin.

## Install

Information about how to install the OEEMicroservice can be found at the corresponding section of the [Installation Guide](https://github.com/Exsensio-Ltd/OEEMicroservice/blob/master/docs/installationguide.md).

## API

More information about API usage of the OEE microservice can be found in [API documentation](https://github.com/Exsensio-Ltd/OEEMicroservice/blob/master/docs/api.md)

## Testing

The project contains integration and unit test implemented with xUnit and MSTest. To run tests use the following command:

```
dotnet test --no-build --verbosity quiet
```

## License

[Apache2.0](LICENSE) © Exsensio Ltd
