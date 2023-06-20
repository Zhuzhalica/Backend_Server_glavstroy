using System.Collections.Generic;

namespace DbEntity
{
    public class UnitMeasurement
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public List<ProductFamily> ProductFamilies { get; set; }
    }
}