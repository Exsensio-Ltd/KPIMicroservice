using System.Collections.Generic;

namespace KPIMicroservice.DTOs
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<StationDto> Stations { get; set; }
    }
}
