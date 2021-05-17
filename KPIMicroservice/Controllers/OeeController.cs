using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KPIMicroservice.DTOs;
using KPIMicroservice.Models;
using KPIMicroservice.Models.OEE;
using KPIMicroservice.Utils;
using KPIMicroservice.Utils.Calculator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KPIMicroservice.Controllers
{
    [Route("api/oee")]
    [ApiController]
    [Produces("application/json")]
    public class OeeController : ControllerBase
    {
        private readonly IContextClient _client;

        public OeeController(IContextClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Add OEE metric to the context
        /// </summary>
        /// <param name="metric">Metric Data</param>
        /// <response code="200">Successfully added metric</response>
        /// <response code="500">Error thrown by context broker</response>
        /// <returns></returns>
        [HttpPut("add")]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromBody][Required] MetricData metric)
        {
            try
            {
                await _client.CreateEntityAsync(metric.ProductName, metric.StationName, metric.BreakDuration, metric.IdealDuration, metric.TotalProductCount);

                return StatusCode(StatusCodes.Status201Created, new ResponseMessage
                {
                    Message = "Metric added.",
                    HasError = false
                });
            }
            catch (AggregateException ae)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    Message = ae.InnerException.Message,
                    HasError = true
                });
            }
        }

        /// <summary>
        /// Fetch OEE data set by station Id
        /// </summary>
        /// <response code="200">List of the products with the stations</response>
        /// <response code="500">Error thrown by context broker</response>
        /// <returns></returns>
        [HttpGet("stations")]
        [ProducesResponseType(typeof(ResponseMessage<IEnumerable<ProductDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public IActionResult StationsList()
        {
            try
            {
                var result = _client.GetProducts();

                var products = result.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Stations = p.Stations.Select(s => new StationDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        RefProduct = s.RefProduct
                    }).ToList()
                }).ToList();

                return Ok(new ResponseMessage<IEnumerable<ProductDto>>
                {
                    Content = products,
                    HasError = false
                });
            }
            catch (AggregateException ae)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    Message = ae.InnerException.Message,
                    HasError = true
                });
            }
        }

        /// <summary>
        /// Update station meta data
        /// </summary>
        /// <param name="id">Station Id</param>
        /// <param name="data">Meta data</param>
        /// <response code="200">List of the products with the stations</response>
        /// <response code="500">Error thrown by context broker</response>
        /// <returns></returns>
        [HttpPost("station")]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStation(
            [FromQuery][Required] string id,
            [FromBody][Required] StationMeta data)
        {
            try
            {
                var meta = new StationMeta
                {
                    ProductionBreakDuration = data.ProductionBreakDuration,
                    ProductionIdealDuration = data.ProductionIdealDuration,
                    TotalProductCount = data.TotalProductCount,
                };
                
                await _client.UpdateStationMeta(id, meta);

                return Ok(new ResponseMessage
                {
                    Message = "Statin meta updated.",
                    HasError = false
                });
            } catch (AggregateException ae)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    Message = ae.InnerException.Message,
                    HasError = true
                });
            }
        }

        /// <summary>
        /// Fetch and calculate OEE data set by station Id
        /// </summary>
        /// <param name="station">Station Id (e.g. urn:ngsi-ld:Station:8b960a8e-ab44-40e6-aaed-8499cb428d18)</param>
        /// <param name="reportingPeriod">Reporting perion in hours</param>
        /// <param name="type">Calculation type (0 = Simple, 1 = Advanced). More details of [OEE](https://www.oee.com/calculating-oee.html)</param>
        /// <param name="calculator"></param>
        /// <response code="200">Data set for selected station</response>
        /// <response code="400">Request error</response>
        /// <response code="500">Error thrown by context broker</response>
        /// <returns></returns>
        [HttpGet("calculate")]
        [ProducesResponseType(typeof(ResponseMessage<IEnumerable<DataSet>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseMessage), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CalculateSimpleOEE(
            [FromQuery][Required] string station,
            [FromQuery][Required] int reportingPeriod,
            [FromQuery][Required] CalculationType type,
            [FromServices] ICalculatorContext calculator)
        {
            try
            {
                var stationEntities = await _client.GetEntitiesAsync(station);

                calculator.SetCalculator(type);
                var dataSet = calculator.ExecuteCalculation(stationEntities, reportingPeriod);

                return Ok(new ResponseMessage<IEnumerable<DataSet>>
                {
                    Content = dataSet,
                    HasError = false
                });
            }
            catch (AggregateException ae)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    Message = ae.InnerException.Message,
                    HasError = true
                });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    Message = e.Message,
                    HasError = true
                });
            }
        }
    }
}
