using System.Collections.Generic;

namespace Entity
{
    public class Sale
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Percent { get; set; }
        public string? PageLink { get; set; }
        public string? ImageRef { get; set; }

        public List<Product> Products { get; set; }
        


        public Sale(string title, string description, int percent, string? pageLink, string? imageRef)
        {
            Title = title;
            Description = description;
            Percent = percent;
            PageLink = pageLink;
            ImageRef = imageRef;
        }
    }
}