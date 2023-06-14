using System;
using System.Collections.Generic;

namespace DbEntity
{
    public class HeadingOne
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<HeadingTwo> HeadingsTwo { get; set; }
        public List<Product> Products { get; set; }
    }
}