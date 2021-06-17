# OEE Microservice

![GitHub](https://img.shields.io/github/license/Exsensio-Ltd/OEEMicroservice)
![CI](https://github.com/Exsensio-Ltd/OEEMicroservice/actions/workflows/dotnet.yml/badge.svg)
[![CII Best Practices](https://bestpractices.coreinfrastructure.org/projects/4904/badge)](https://bestpractices.coreinfrastructure.org/projects/4904)

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

OEE Microservice provides API to interact with Fiware context broker to store test metrics, station metadata, [calculate OEE](https://www.oee.com/calculating-oee.html) and build time series data for Grafana plugin. Calculation OEE awailable in two ways: Simple and Advanced.

### Simple OEE calculation

The simplest way to calculate OEE is as the ratio of Fully Productive Time to Planned Production Time. Fully Productive Time is just another way of saying manufacturing only Good Parts as fast as possible (Ideal Cycle Time) with no Stop Time. Hence the calculation is:

```
OEE = (Good Count × Ideal Cycle Time) / Planned Production Time
```

### Advanced OEE calculation

The advanced OEE calculation is based on the three OEE Factors: Availability, Performance, and Quality.

```
OEE = Availability × Performance × Quality
```

#### Availability

Availability takes into account all events that stop planned production long enough where it makes sense to track a reason for being down (typically several minutes).
Availability is calculated as the ratio of Run Time to Planned Production Time:
```
Availability = Run Time / Planned Production Time
```

#### Performance

Performance takes into account anything that causes the manufacturing process to run at less than the maximum possible speed when it is running (including both Slow Cycles and Small Stops).

Performance is the ratio of Net Run Time to Run Time. It is calculated as:

```
Performance = (Ideal Cycle Time × Total Count) / Run Time
```

#### Quality

Quality takes into account manufactured parts that do not meet quality standards, including parts that need rework. Remember, OEE Quality is similar to First Pass Yield, in that it defines Good Parts as parts that successfully pass through the manufacturing process the first time without needing any rework.

Quality is calculated as:

```
Quality = Good Count / Total Count
```

## Install

Information about how to install the OEEMicroservice can be found at the corresponding section of the [Installation Guide](/docs/installationguide.md).

## API

More information about API usage of the OEE microservice can be found in [API documentation](/docs/api.md)

### Postman <img src="https://www.postman.com/favicon-32x32.png" align="left"  height="30" width="30">

The tutorials which use HTTP requests supply a collection for use with the Postman utility. Postman is a testing
framework for REST APIs. The tool can be downloaded from [www.postman.com](https://www.postman.com/). All the OEEMicroservice
Postman collections can downloaded directly from the
[Postman API network](https://documenter.getpostman.com/view/16273471/TzeWJ91y)

## Testing

The project contains integration and unit test implemented with xUnit and MSTest. To run tests use the following command:

`dotnet test --no-build --verbosity quiet`

## License

[Apache2.0](LICENSE) © Exsensio Ltd
