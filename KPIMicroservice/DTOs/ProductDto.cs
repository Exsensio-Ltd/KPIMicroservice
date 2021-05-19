using System.Collections.Generic;

namespace KPIMicroservice.DTOs
{
    public class ProductDto
    {
        /// <summary>
        /// Product entity Id
        /// </summary>
        /// <example>urn:ngsi-ld:Product:347a63f1-bdf8-4243-bd7b-d0bc16ae5241</example>
        public string Id { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        /// <example>Product Test Name</example>
        public string Name { get; set; }

        public IEnumerable<StationDto> Stations { get; set; }
    }
}
