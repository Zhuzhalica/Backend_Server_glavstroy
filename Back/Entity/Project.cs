namespace Entity
{
    public class Project
    {
        public string Title { get; set; }
        public string RoofType { get; set; }

        public Project(string title, string roofType)
        {
            Title = title;
            RoofType = roofType;
        }
    }
}