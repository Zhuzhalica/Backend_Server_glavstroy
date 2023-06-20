using System;
using System.Collections.Generic;

namespace DbEntity
{
    public class HeadingOne
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? PageLink { get; set; }
        public List<HeadingTwo> HeadingsTwo { get; set; }
        public List<ProductFamily> ProductFamilies { get; set; }
    }
}