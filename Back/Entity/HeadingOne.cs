using System.Collections.Generic;

namespace Entity
{
    public class HeadingOne
    {
        public string Title { get; set; }
        public string? PageLink { get; set; }

        public HeadingOne(string title, string? pageLink)
        {
            Title = title;
            PageLink = pageLink;
        }
    }
}