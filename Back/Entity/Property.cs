namespace Entity
{
    public class Property
    {
        public string Title { get; set; }
        public string[] Value { get; set; }
        //public bool IsPriority { get; set; }

        public Property(string title, string[] value)
        {
            Title = title;
            Value = value;
        }
    }
}