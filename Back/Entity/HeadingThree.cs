namespace Entity
{
    public class HeadingThree
    {
        public string Title { get; set; }
        public HeadingTwo HeadingTwo { get; set; }

        public HeadingThree(string title, HeadingTwo headingTwo)
        {
            Title = title;
            HeadingTwo = headingTwo;
        }
    }
}