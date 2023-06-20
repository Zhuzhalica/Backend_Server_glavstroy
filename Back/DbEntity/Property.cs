using System.Collections.Generic;

namespace  DbEntity
{
    public class Property
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<PropertyValues> PropertyValues { get; set; }
    }
}