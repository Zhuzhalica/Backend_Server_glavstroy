using System.Collections.Generic;

namespace Entity
{
    public class HeadingTwo
    {
        public string Title { get; set; }
        public string? PageLink { get; set; }
        public string? ImageRef { get; set; }
        public HeadingOne HeadingOne { get; set; }

        public HeadingTwo(string title, string? imageRef, string? pageLink)
        {
            Title = title;
            ImageRef = imageRef;
            PageLink = pageLink;
        }
    }
}