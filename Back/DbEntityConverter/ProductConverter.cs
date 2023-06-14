using System.Threading.Tasks;
using Entity;

namespace DbEntityConverter
{
    public class ProductConverter
    {
        public static Entity.Product ToEntity(DbEntity.Product dbProduct)
        {
            var headingOne = new HeadingOne(dbProduct.HeadingOne.Title);
            var headingTwo = new HeadingTwo(dbProduct.HeadingTwo.Title, headingOne);
            var headingThree = new HeadingThree(dbProduct.HeadingThree.Property.Title, headingTwo);
            return new Product(dbProduct.Title, dbProduct.Description, dbProduct.Price, dbProduct.Quantity, headingOne,
                headingTwo, headingThree);
        }

        public static DbEntity.Product ToDbEntity(Product product, DbEntity.HeadingOne headingOne, DbEntity.HeadingTwo headingTwo, DbEntity.HeadingThree headingThree)
        {
            return new DbEntity.Product()
            {
                Title = product.Title, Description = product.Description, Price = product.Price,
                Quantity = product.Quantity, HeadingOne = headingOne, HeadingTwo = headingTwo, HeadingThree = headingThree
            };
        }
    }
}