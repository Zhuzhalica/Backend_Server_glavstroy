using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BackServer.Contexts;
using BackServer.RepositoryChangers.Implementations;
using BackServer.Services;
using BackServer.Services.Interfaces;
using DbEntity;
using NpgsqlDbExtensions;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Npgsql;
using HeadingThree = Entity.HeadingThree;
using Product = Entity.Product;
using Property = Entity.Property;

namespace BackServer.Repositories
{
    public class ProductsVisitorDb : IProductVisitor
    {
        private readonly TestContext _context;
        private readonly IPropertyService _propertyService;

        public ProductsVisitorDb(TestContext context, IPropertyService propertyService)
        {
            _context = context;
            _propertyService = propertyService;
        }

        public async Task<IEnumerable<Entity.Product>> GetAll()
        {
            var products = new List<Product>();
            await using var con = (NpgsqlConnection?) _context.Database.GetDbConnection();
            if (con.State != ConnectionState.Open)
                await con.OpenAsync();

            var sql = @$"
                SELECT p.title, p.description, p.price, p.quantity, p.popularity, p.available, p.image_ref, p.page_link, um.unit_measurement_value,
                    hone.title, htwo.title, pv.property_value 
                FROM products as p
                         JOIN product_family pf on pf.product_family_id = p.product_family_id
                         JOIN units_measurement um on um.unit_measurement_id = pf.unit_measurement_id
                         JOIN heading_one hone on hone.heading_one_id = pf.heading_one_id
                         JOIN heading_two htwo on pf.heading_two_id=htwo.heading_two_id
                         LEFT JOIN heading_three hthree on p.heading_three_id=hthree.heading_three_id
                         LEFT JOIN property_values pv on hthree.property_values_id = pv.property_values_id";
            await using var cmd = new NpgsqlCommand(sql, con);
            await using NpgsqlDataReader rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                products.Add(await ConvertProduct(rdr));
            }

            return products;
        }

        public async Task<IEnumerable<Entity.Product>> GetAvailable()
        {
            var products = new List<Product>();
            await using var con = (NpgsqlConnection?) _context.Database.GetDbConnection();
            if (con.State != ConnectionState.Open)
                await con.OpenAsync();

            var sql = @$"
                SELECT p.title, p.description, p.price, p.quantity, p.popularity, p.available, p.image_ref, p.page_link, um.unit_measurement_value,
                    hone.title, htwo.title, pv.property_value 
                FROM products as p
                         JOIN product_family pf on pf.product_family_id = p.product_family_id
                         JOIN units_measurement um on um.unit_measurement_id = pf.unit_measurement_id
                         JOIN heading_one hone on hone.heading_one_id = pf.heading_one_id
                         JOIN heading_two htwo on pf.heading_two_id=htwo.heading_two_id
                         LEFT JOIN heading_three hthree on p.heading_three_id=hthree.heading_three_id
                         LEFT JOIN property_values pv on hthree.property_values_id = pv.property_values_id
                WHERE p.available=true;";
            await using var cmd = new NpgsqlCommand(sql, con);
            await using NpgsqlDataReader rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                products.Add(await ConvertProduct(rdr));
            }

            return products;
        }

        public async Task<Product> GetByTitle(string title)
        {
            Product product = default!;
            await using var con = (NpgsqlConnection?) _context.Database.GetDbConnection();
            if (con.State != ConnectionState.Open)
                await con.OpenAsync();

            var sql = @$"
                SELECT p.title, p.description, p.price, p.quantity, p.popularity, p.available, p.image_ref, p.page_link, um.unit_measurement_value,
                    hone.title, htwo.title, pv.property_value 
                FROM products as p
                         JOIN product_family pf on pf.product_family_id = p.product_family_id
                         JOIN units_measurement um on um.unit_measurement_id = pf.unit_measurement_id
                         JOIN heading_one hone on hone.heading_one_id = pf.heading_one_id
                         JOIN heading_two htwo on pf.heading_two_id=htwo.heading_two_id
                         LEFT JOIN heading_three hthree on p.heading_three_id=hthree.heading_three_id
                         LEFT JOIN property_values pv on hthree.property_values_id = pv.property_values_id;
                WHERE p.title='{title}';";
            await using var cmd = new NpgsqlCommand(sql, con);
            await using NpgsqlDataReader rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                product = await ConvertProduct(rdr);
            }

            return product;
        }

        public async Task<IEnumerable<Entity.Product>> GetByHeadingOne(string headingOneTitle,
            HashSet<Property> reqProperties, int pageNumber, int countElements)
        {
            var products = new List<Product>();
            await using var con = (NpgsqlConnection?) _context.Database.GetDbConnection();
            if (con.State != ConnectionState.Open)
                await con.OpenAsync();

            var sql = @$"
                SELECT p.title, p.description, p.price, p.quantity, p.popularity, p.available, p.image_ref, p.page_link,
                       um.unit_measurement_value,
                       CASE WHEN s.percent ISNULL THEN p.price ELSE p.price * (100-s.percent)/100 END
                FROM products as p
                         JOIN product_family pf on pf.product_family_id = p.product_family_id
                         JOIN heading_one ho on ho.heading_one_id = pf.heading_one_id
                         JOIN units_measurement um on um.unit_measurement_id = pf.unit_measurement_id
                         LEFT JOIN sale_products sp on p.product_id = sp.product_id
                         LEFT JOIN sales s on sp.sale_id = s.sale_id
                WHERE p.available AND ho.title = '{headingOneTitle}'
                ORDER BY p.popularity DESC
                OFFSET {(pageNumber - 1) * countElements}
                LIMIT {countElements};";

            await using var cmd = new NpgsqlCommand(sql, con);
            {
                await using NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var product = await ConvertProductWithSale(rdr);
                    products.Add(product);
                }
            }
            
            // await con.CloseAsync();
            //
            // if (con.State != ConnectionState.Open)
            //     await con.OpenAsync();
            


            foreach (var product in products)
            {
                var properties = new List<Entity.Property>();
                sql = @$"
                SELECT p2.title, pv.property_value
                FROM products as p
                         LEFT JOIN (SELECT * FROM product_properties WHERE is_priority) as pp on p.product_id = pp.product_id
                         LEFT JOIN property_values pv on pp.property_values_id = pv.property_values_id
                         LEFt JOIN properties p2 on pv.property_id = p2.property_id
                WHERE p.title= '{product.Title}';";

                await using var cmd2 = new NpgsqlCommand(sql, con);
                await using NpgsqlDataReader rdr2 = await cmd2.ExecuteReaderAsync();
                while (await rdr2.ReadAsync())
                {
                    if (!rdr2.IsDBNull(0))
                        properties.Add(new Entity.Property(rdr2.GetString(0), new[] {rdr2.GetString(1)}));
                }

                product.PriorityProperties = properties;
            }


            // foreach (var product in products)
            // {
            //     var props = await _propertyService.GetPriorityByProduct(product.Title);
            //     product.PriorityProperties = props;
            // }

            return products;
        }

        public async Task<IEnumerable<Entity.Product>> GetByHeadingTwo(string headingTwoTitle, int pageNumber,
            int countElements)
        {
            var products = new List<Product>();
            await using var con = (NpgsqlConnection?) _context.Database.GetDbConnection();
            if (con.State != ConnectionState.Open)
                await con.OpenAsync();

            var sql = @$"
                SELECT p.title, p.description, p.price, p.quantity, p.popularity, p.available, p.image_ref, p.page_link,
                       um.unit_measurement_value,
                       CASE WHEN s.percent ISNULL THEN p.price ELSE p.price * (100-s.percent)/100 END
                FROM products as p
                         JOIN product_family pf on pf.product_family_id = p.product_family_id
                         JOIN heading_two ht on ht.heading_two_id = pf.heading_two_id
                         JOIN units_measurement um on um.unit_measurement_id = pf.unit_measurement_id
                         LEFT JOIN sale_products sp on p.product_id = sp.product_id
                         LEFT JOIN sales s on sp.sale_id = s.sale_id
                WHERE p.available AND ht.title = '{headingTwoTitle}'
                ORDER BY p.popularity DESC
                OFFSET {(pageNumber - 1) * countElements}
                LIMIT {countElements};";

            await using var cmd = new NpgsqlCommand(sql, con);
            {
                await using NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var product = await ConvertProductWithSale(rdr);
                    products.Add(product);
                }
            }

            return products;
        }

        public async Task<IEnumerable<Product>> GetByHeadingThree(string headingThreeTitle, int pageNumber,
            int countElements)
        {
            var products = new List<Product>();
            await using var con = (NpgsqlConnection?) _context.Database.GetDbConnection();
            if (con.State != ConnectionState.Open)
                await con.OpenAsync();

            var sql = @$"
                SELECT p.title, p.description, p.price, p.quantity, p.popularity, p.available, p.image_ref, p.page_link,
                       um.unit_measurement_value,
                       CASE WHEN s.percent ISNULL THEN p.price ELSE p.price * (100 - s.percent) / 100 END
                FROM products as p
                         JOIN product_family pf on pf.product_family_id = p.product_family_id
                             JOIN units_measurement um on um.unit_measurement_id = pf.unit_measurement_id
                         JOIN heading_three ht on p.heading_three_id = ht.heading_three_id
                 JOIN property_values pv on ht.property_values_id = pv.property_values_id
                         LEFT JOIN sale_products sp on p.product_id = sp.product_id
                         LEFT JOIN sales s on sp.sale_id = s.sale_id
                WHERE p.available AND pv.property_value={headingThreeTitle}
                ORDER BY p.popularity DESC
                OFFSET {(pageNumber - 1) * countElements}
                LIMIT {countElements};";

            await using var cmd = new NpgsqlCommand(sql, con);
            {
                await using NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var product = await ConvertProductWithSale(rdr);
                    products.Add(product);
                }
            }

            return products;
        }

        private async Task<Product> ConvertProduct(NpgsqlDataReader rdr)
        {
            return new Product(rdr.GetString(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3),
                rdr.GetInt32(4), rdr.GetBoolean(5), await rdr.ReadNullOrStringAsync(6),
                await rdr.ReadNullOrStringAsync(7), rdr.GetString(8))
            {
                HeadingOne = rdr.GetString(9),
                HeadingTwo = rdr.GetString(10),
                HeadingThree = await rdr.ReadNullOrStringAsync(11)
            };
        }

        private async Task<Product> ConvertProductWithSale(NpgsqlDataReader rdr)
        {
            return new Product(rdr.GetString(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3),
                rdr.GetInt32(4), rdr.GetBoolean(5), await rdr.ReadNullOrStringAsync(6),
                await rdr.ReadNullOrStringAsync(7), rdr.GetString(8))
            {
                SalePrice = rdr.GetInt32(9)
            };
        }
    }
}