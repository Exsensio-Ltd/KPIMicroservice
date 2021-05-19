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
        /// <response code="400">Missing required body parameters</response>
        /// <response code="500">Error thrown by context broker</response>
        /// <returns></returns>
        [HttpPut("add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> Add([FromBody][Required] MetricData metric)
        {
            try
            {
                await _client.CreateEntityAsync(
                    metric.ProductName,
                    metric.StationName,
                    metric.ProductionBreakDuration,
                    metric.ProductionIdealDuration,
                    metric.TotalProductCount
                );

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
                    Message = ae.InnerException?.Message,
                    HasError = true
                });
            }
        }

        /// <summary>
        /// Fetch OEE data set by station Id
        /// </summary>
        /// <response code="200">List of the stations</response>
        /// <response code="500">Error thrown by context broker</response>
        /// <returns>List of stations</returns>
        [HttpGet("stations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMessage<IEnumerable<ProductDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseMessage))]
        public IActionResult StationsList()
        {
            try
            {
                var result = _client.GetProducts();

                var products = result.Select(p => new ProductDto
                {
                    Id = p.GetFullId(),
                    Name = p.Name,
                    Stations = p.Stations.Select(s => new StationDto
                    {
                        Id = s.GetFullId(),
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
                    Message = ae.InnerException?.Message,
                    HasError = true
                });
            }
        }

        /// <summary>
        /// Update station meta data
        /// </summary>
        /// <param name="id" example="urn:ngsi-ld:Station:8b960a8e-ab44-40e6-aaed-8499cb428d18">Station entity Id</param>
        /// <param name="data">Meta data</param>
        /// <response code="200">List of the products with the stations</response>
        /// <response code="500">Error thrown by context broker</response>
        /// <returns></returns>
        [HttpPost("station/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMessage))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> UpdateStation(string id, [FromBody] MetaData data)
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
                    Message = "Station meta updated.",
                    HasError = false
                });
            }
            catch (AggregateException ae)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    Message = ae.InnerException?.Message,
                    HasError = true
                });
            }
        }

        /// <summary>
        /// Fetch and calculate OEE data set by station Id
        /// </summary>
        /// <param name="calculationData">More details of [OEE](https://www.oee.com/calculating-oee.html)</param>
        /// <param name="calculator"></param>
        /// <response code="200">Data set for selected station</response>
        /// <response code="400">Request error</response>
        /// <response code="500">Error thrown by context broker</response>
        /// <returns></returns>
        [HttpGet("calculate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMessage<IEnumerable<DataSet>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseMessage))]
        public async Task<IActionResult> CalculateOee(
            [FromQuery] CalculationData calculationData,
            [FromServices] ICalculatorContext calculator)
        {
            try
            {
                var stationEntities = await _client.GetEntitiesAsync(calculationData.Id);

                calculator.SetCalculator(calculationData.Type);
                var dataSet = calculator.ExecuteCalculation(stationEntities, calculationData.ReportingPeriod);

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
                    Message = ae.InnerException?.Message,
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
