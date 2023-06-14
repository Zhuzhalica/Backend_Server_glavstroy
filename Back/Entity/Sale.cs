namespace Entity
{
    public class Sale
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Percent { get; set; }

        public Sale(string title, string description, int percent)
        {
            Title = title;
            Description = description;
            Percent = percent;
        }
    }
}