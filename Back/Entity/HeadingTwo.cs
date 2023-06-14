namespace Entity
{
    public class HeadingTwo
    {
        public string Title { get; set; }
        public HeadingOne HeadingOne { get; set; }

        public HeadingTwo(string title, HeadingOne headingOne)
        {
            Title = title;
            HeadingOne = headingOne;
        }
    }
}