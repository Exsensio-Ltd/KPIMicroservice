namespace KPIMicroservice.DTOs
{
    public class StationDto
    {
        /// <summary>
        /// Station entity Id
        /// </summary>
        /// <example>urn:ngsi-ld:Station:9bc58c8b-bcd7-41cc-b2ce-4b2e59266dfb</example>
        public string Id { get; set; }

        /// <summary>
        /// Station name
        /// </summary>
        /// <example>Station Test Name</example>
        public string Name { get; set; }

        /// <summary>
        /// Product entity Id
        /// </summary>
        /// <example>urn:ngsi-ld:Product:347a63f1-bdf8-4243-bd7b-d0bc16ae5241</example>
        public string RefProduct { get; set; }
    }
}
