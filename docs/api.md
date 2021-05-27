# API

[Back](https://github.com/Exsensio-Ltd/OEEMicroservice/blob/master/README.md#api)

## Available API endpoints

-   [Add OEE metric to the context](#add-oee-metric-to-the-context)
-   [Fetch OEE data set by station Id](#fetch-oee-data-set-by-station-id)
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
    { "hasError": false, "message": "Metric added." }
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
    --url 'https://localhost:5004/api/oee/add' \
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
    fetch("https://localhost:5004/api/oee/add", {
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

### Fetch OEE data set by station Id

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
  curl -iX GET --url 'https://localhost:5004/api/oee/stations'
  ```

  OR

  ```javascript
    fetch("https://localhost:5004/api/oee/stations")
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
    { "hasError": false, "message": "Station meta updated." }
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
    --url 'https://localhost:5004/api/oee/station/urn:ngsi-ld:Station:9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb' \
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
    fetch("https://localhost:5004/api/oee/station/urn:ngsi-ld:Station:9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb", {
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
  curl -iX GET --url 'https://localhost:5004/api/oee/calculate?id=urn:ngsi-ld:Station:9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb&reportingPeriod=1&type=0'
  ```

  OR

  ```javascript
    fetch("https://localhost:5004/api/oee/calculate?id=urn:ngsi-ld:Station:9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb&reportingPeriod=1&type=0")
      .then(response => response.json())
      .then(data => console.log(data));
  ```
