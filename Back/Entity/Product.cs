using System.Collections.Generic;

namespace Entity
{
    public class Product
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public HeadingOne HeadingOne { get; set; }
        public HeadingTwo HeadingTwo { get; set; }
        public HeadingThree HeadingThree { get; set; }
        public int Price { get; set; }
        public int SalePrice { get; set; }
        public int Quantity { get; set; }

        public Product(string title, string description, int price, int quantity, HeadingOne headingOne, HeadingTwo headingTwo, HeadingThree headingThree)
        {
            Title = title;
            Description = description;
            Price = price;
            Quantity = quantity;
            HeadingOne = headingOne;
            HeadingTwo = headingTwo;
            HeadingThree = headingThree;
        }
    }
}