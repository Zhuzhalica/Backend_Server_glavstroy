namespace Entity
{
    public class HeadingThree
    {
        public string Title { get; set; }
        public string? PageLink { get; set; }
        public string? ImageRef { get; set; }
        public HeadingTwo HeadingTwo { get; set; }

        public HeadingThree(string title, string? imageRef, string? pageLink)
        {
            Title = title;
            ImageRef = imageRef;
            PageLink = pageLink;
        }
    }
}