# API

[Back](/README.md#api)

## Available API endpoints

-   [Add OEE metric to the context](#add-oee-metric-to-the-context)
-   [Fetch stations](#fetch-stations)
-   [Update station meta data](#update-station-meta-data)
-   [Fetch and calculate OEE data set by station Id](#fetch-and-calculate-oee-data-set-by-station-id)

### Add OEE metric to the context
* **URL**

  `/api/oee/add`

* **Method:**

  `PUT`

* **URL Params**

  None

* **Data Params**

  **Required:**
  ```
  productName=[string]
  stationName=[string]
  productionBreakDuration=[string]
  productionIdealDuration=[string]
  totalProductCount=[integer]
  ```

* **Success Response:**

  * **Code:** 200 <br />
    **Content:**
    ```json
    { "hasError": false, "message": "Metric added" }
    ```

* **Error Response:**

  * **Code:** 400 Missing required body parameters <br />
    **Content:**
    ```json
    {
      "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
      "title": "One or more validation errors occurred.",
      "status": 400,
      "traceId": "00-192a737437230c4ea2e79d795c1c7006-9ad6880723217745-00",
      "errors": {
        "ProductionBreakDuration": [ "The ProductionBreakDuration field is required." ],
        "ProductionIdealDuration": [ "The ProductionIdealDuration field is required." ]
      }
    }
    ```

  OR

  * **Code:** 500 Error thrown by context broker <br />
    **Content:**
    ```json
    { "hasError": true, "message": "Internal server error" }
    ```

* **Sample Call:**

  ```
  curl -iX PUT \
    --url 'http://localhost:51803/api/oee/add' \
    -header 'Content-Type: application/json' \
    -data '
  {
    "productName": "Test Product",
    "stationName": "Test Station",
    "productionBreakDuration": "00:00:01.221",
    "productionIdealDuration": "00:00:55.123",
    "totalProductCount": 1
  }'
  ```

  OR

  ```javascript
    fetch("http://localhost:51803/api/oee/add", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        productName: "Test Product",
        stationName: "Test Station",
        productionBreakDuration: "00:00:01.221",
        productionIdealDuration: "00:00:55.123",
        totalProductCount: 1
      })
    })
      .then(response => response.json())
      .then(data => console.log(data));
  ```

### Fetch stations

* **URL**

  `/api/oee/stations`

* **Method:**

  `GET`

* **URL Params**

  None

* **Data Params**

  None

* **Success Response:**

  * **Code:** 200 <br />
    **Content:**
    ```json
    {
      "hasError": false,
      "message": "",
      "content": [
        {
          "id": "urn:ngsi-ld:Product:347a63f1-bdf8-4243-bd7b-d0bc16ae5241",
          "name": "Product Test Name",
          "stations": [
            {
              "id": "urn:ngsi-ld:Station:9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb",
              "name": "Station Test Name",
              "refProduct": "urn:ngsi-ld:Product:347a63f1-bdf8-4243-bd7b-d0bc16ae5241"
            }
          ]
        }
      ]
    }
    ```

* **Error Response:**

  * **Code:** 500 Error thrown by context broker <br />
    **Content:**
    ```json
    { "hasError": true, "message": "Internal server error" }
    ```

* **Sample Call:**

  ```
  curl -iX GET --url 'http://localhost:51803/api/oee/stations'
  ```

  OR

  ```javascript
    fetch("http://localhost:51803/api/oee/stations")
      .then(response => response.json())
      .then(data => console.log(data));
  ```

### Update station meta data
* **URL**

  `/api/oee/station/{id}`

* **Method:**

  `POST`

* **URL Params**

  **Required:**

  `id=[string]`

* **Data Params**

  ```
  productionBreakDuration=[string]
  productionIdealDuration=[string]
  totalProductCount=[integer]
  ```

* **Success Response:**

  * **Code:** 200 <br />
    **Content:**
    ```json
    { "hasError": false, "message": "Station meta updated" }
    ```

* **Error Response:**

  * **Code:** 500 Error thrown by context broker <br />
    **Content:**
    ```json
    { "hasError": true, "message": "Internal server error" }
    ```

* **Sample Call:**

  ```
  curl -iX POST \
    --url 'http://localhost:51803/api/oee/station/urn:ngsi-ld:Station:9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb' \
    -header 'Content-Type: application/json' \
    -data '
  {
    "productionBreakDuration": "00:00:01.100",
    "productionIdealDuration": "00:00:55.123",
    "totalProductCount": 1
  }'
  ```

  OR

  ```javascript
    fetch("http://localhost:51803/api/oee/station/urn:ngsi-ld:Station:9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        productionBreakDuration: "00:00:01.100",
        productionIdealDuration: "00:00:55.123",
        totalProductCount: 1
      })
    })
      .then(response => response.json())
      .then(data => console.log(data));
  ```

### Fetch and calculate OEE data set by station Id

Calculation OEE awailable in two ways: Simple and Advanced

#### Simple OEE calculation

The simplest way to calculate OEE is as the ratio of Fully Productive Time to Planned Production Time. Fully Productive Time is just another way of saying manufacturing only Good Parts as fast as possible (Ideal Cycle Time) with no Stop Time. Hence the calculation is:

```
OEE = (Good Count × Ideal Cycle Time) / Planned Production Time
```

#### Advanced OEE calculation

The advanced OEE calculation is based on the three OEE Factors: Availability, Performance, and Quality.

```
OEE = Availability × Performance × Quality
```

##### Availability
Availability takes into account all events that stop planned production long enough where it makes sense to track a reason for being down (typically several minutes). Availability is calculated as the ratio of Run Time to Planned Production Time:

```
Availability = Run Time / Planned Production Time
```

##### Performance

Performance takes into account anything that causes the manufacturing process to run at less than the maximum possible speed when it is running (including both Slow Cycles and Small Stops). Performance is the ratio of Net Run Time to Run Time. It is calculated as:

```
Performance = (Ideal Cycle Time × Total Count) / Run Time
```

##### Quality

Quality takes into account manufactured parts that do not meet quality standards, including parts that need rework. Remember, OEE Quality is similar to First Pass Yield, in that it defines Good Parts as parts that successfully pass through the manufacturing process the first time without needing any rework. Quality is calculated as:

```
Quality = Good Count / Total Count
```

* **URL**

  `/api/oee/calculate`

* **Method:**

  `GET`

* **URL Params**

  **Required:**

  ```
  id=[string]
  reportingPeriod=[integer]
  type=[integer]
  ```
  
  **Awailable values:**
    * reportingPeriod = Reporting period in hours. Available values : 1, 2, 3, 4, 5, 6, 7, 8
    * type = OEE calculation type. Available values : 0, 1

* **Data Params**

  None

* **Success Response:**

  * **Code:** 200 <br />
    **Content:**
    ```json
    {
      "hasError": true,
      "message": "",
      "content": [
        {
          "text": "12:00 29-03",
          "value": 89,
          "availability": 98,
          "performance": 76,
          "quality": 85
        }
      ]
    }
    ```

* **Error Response:**

  * **Code:** 400 Missing required body parameters <br />
    **Content:**
    ```json
    {
      "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
      "title": "One or more validation errors occurred.",
      "status": 400,
      "traceId": "00-192a737437230c4ea2e79d795c1c7006-9ad6880723217745-00",
      "errors": {
        "ProductionBreakDuration": [ "The ReportingPeriod field is required." ],
      }
    }
    ```

  OR

  * **Code:** 500 Error thrown by context broker <br />
    **Content:**
    ```json
    { "hasError": true, "message": "Internal server error" }
    ```

* **Sample Call:**

  ```
  curl -iX GET --url 'http://localhost:51803/api/oee/calculate?id=urn:ngsi-ld:Station:9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb&reportingPeriod=1&type=0'
  ```

  OR

  ```javascript
    fetch("http://localhost:51803/api/oee/calculate?id=urn:ngsi-ld:Station:9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb&reportingPeriod=1&type=0")
      .then(response => response.json())
      .then(data => console.log(data));
  ```
