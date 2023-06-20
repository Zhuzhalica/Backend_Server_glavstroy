using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using BackServer.Contexts;
using BackServer.Services.Interfaces;
using DbEntity;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Npgsql;

namespace BackServer.Repositories
{
    public class PropertiesVisitorDb : IPropertyVisitor
    {
        private readonly TestContext _context;

        public PropertiesVisitorDb(TestContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetAllTitles()
        {
            return await _context.Properties.Select(x => x.Title).ToArrayAsync();
        }

        public async Task<IEnumerable<Entity.Property>> GetAllByProduct(string productTitle)
        {
            var res = new List<Entity.Property>();
            var con = (NpgsqlConnection?) _context.Database.GetDbConnection();
            if (con.State != ConnectionState.Open)
                await con.OpenAsync();

            var sql = @$"
            SELECT p1.property_value, prop.title
            FROM products AS p
                     JOIN product_properties pp on p.product_id = pp.product_id
                     JOIN property_values AS p1 ON pp.property_values_id = p1.property_values_id
                     JOIN properties as prop ON p1.property_id = prop.property_id
            WHERE p.title = '{productTitle}';";
            await using var cmd = new NpgsqlCommand(sql, con);
            await using NpgsqlDataReader rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                res.Add(new Entity.Property(rdr.GetString(0), new[] {rdr.GetString(1)}));
            }

            return res;
        }

        public async Task<IEnumerable<Entity.Property>> GetPriorityByProduct(string productTitle)
        {
            var properties = new List<Entity.Property>();
            await using var con = (NpgsqlConnection?) _context.Database.GetDbConnection();
            if (con.State != ConnectionState.Open)
                await con.OpenAsync();

            var sql = @$"
                SELECT p2.title, pv.property_value
                FROM products as p
                         LEFT JOIN (SELECT * FROM product_properties WHERE is_priority) as pp on p.product_id = pp.product_id
                         LEFT JOIN property_values pv on pp.property_values_id = pv.property_values_id
                         LEFt JOIN properties p2 on pv.property_id = p2.property_id
                WHERE p.title= '{productTitle}';";

            await using var cmd = new NpgsqlCommand(sql, con);
            await using NpgsqlDataReader rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                if (!rdr.IsDBNull(0))
                    properties.Add(new Entity.Property(rdr.GetString(0), new[] {rdr.GetString(1)}));
            }

            return properties;
        }


        public async Task<IEnumerable<Entity.Property>> GetByHeadingOne(string headingOneTitle)
        {
            var res = new List<Entity.Property>();
            await using var con = (NpgsqlConnection?) _context.Database.GetDbConnection();
            if (con.State != ConnectionState.Open)
                await con.OpenAsync();

            var sql = @$"
                SELECT max(CASE WHEN s.percent ISNULL THEN p.price ELSE p.price * (100 - s.percent) / 100 END),
                       min(CASE WHEN s.percent ISNULL THEN p.price ELSE p.price * (100 - s.percent) / 100 END)
                FROM products as p
                         JOIN product_family pf on pf.product_family_id = p.product_family_id
                         JOIN heading_one ho on ho.heading_one_id = pf.heading_one_id
                         LEFT JOIN sale_products sp on p.product_id = sp.product_id
                         LEFT JOIN sales s on sp.sale_id = s.sale_id
                WHERE ho.title = '{headingOneTitle}'
                GROUP BY pf.heading_one_id;";
            await using var cmd = new NpgsqlCommand(sql, con);
            await using NpgsqlDataReader rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                res.Add(new Entity.Property("Максимальная цена", new[] {rdr.GetInt32(0).ToString()}));
                res.Add(new Entity.Property("Минимальная цена", new[] {rdr.GetInt32(1).ToString()}));
            }
            
            await con.CloseAsync();
            
            if (con.State != ConnectionState.Open)
                await con.OpenAsync();

            sql = @$"
                    SELECT p2.title, string_agg(pv.property_value, ' & ')
                    FROM products as p
                             JOIN product_family pf on pf.product_family_id = p.product_family_id
                             JOIN heading_one ho on ho.heading_one_id = pf.heading_one_id
                             JOIN product_properties pp on p.product_id = pp.product_id
                             JOIN property_values pv on pp.property_values_id = pv.property_values_id
                             JOIN properties p2 on pv.property_id = p2.property_id
                    WHERE ho.title = '{headingOneTitle}'
                    GROUP BY p2.title;";
            await using var cmd2 = new NpgsqlCommand(sql, con);
            await using NpgsqlDataReader rdr2 = await cmd2.ExecuteReaderAsync();
            while (await rdr2.ReadAsync())
            {
                res.Add(new Entity.Property(rdr2.GetString(0), rdr2.GetString(1).Split(" & ")));
            }

            return res;
        }

        public async Task<IEnumerable<Entity.Property>> GetByHeadingTwo(string headingTwoTitle)
        {
            var res = new List<Entity.Property>();
            await using var con = (NpgsqlConnection?) _context.Database.GetDbConnection();
            if (con.State != ConnectionState.Open)
                await con.OpenAsync();


            var sql = @$"
                SELECT max(CASE WHEN s.percent ISNULL THEN p.price ELSE p.price * (100 - s.percent) / 100 END),
                       min(CASE WHEN s.percent ISNULL THEN p.price ELSE p.price * (100 - s.percent) / 100 END)
                FROM products as p
                         JOIN product_family pf on pf.product_family_id = p.product_family_id
                         JOIN heading_two ht on ht.heading_two_id = pf.heading_two_id
                         LEFT JOIN sale_products sp on p.product_id = sp.product_id
                         LEFT JOIN sales s on sp.sale_id = s.sale_id
                WHERE ht.title = '{headingTwoTitle}'
                GROUP BY pf.heading_two_id;";
            await using var cmd = new NpgsqlCommand(sql, con);
            await using NpgsqlDataReader rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                res.Add(new Entity.Property("Максимальная цена", new[] {rdr.GetInt32(0).ToString()}));
                res.Add(new Entity.Property("Минимальная цена", new[] {rdr.GetInt32(1).ToString()}));
            }

            await con.CloseAsync();

            if (con.State != ConnectionState.Open)
                await con.OpenAsync();

            sql = @$"
                    SELECT p2.title, string_agg(pv.property_value, ' & ')
                    FROM products as p
                             JOIN product_family pf on pf.product_family_id = p.product_family_id
                             JOIN heading_two ht on ht.heading_two_id = pf.heading_two_id
                             JOIN product_properties pp on p.product_id = pp.product_id
                             JOIN property_values pv on pp.property_values_id = pv.property_values_id
                             JOIN properties p2 on pv.property_id = p2.property_id
                    WHERE ht.title = '{headingTwoTitle}'
                    GROUP BY p2.title;";
            await using var cmd2 = new NpgsqlCommand(sql, con);
            await using NpgsqlDataReader rdr2 = await cmd2.ExecuteReaderAsync();
            while (await rdr2.ReadAsync())
            {
                res.Add(new Entity.Property(rdr2.GetString(0), rdr2.GetString(1).Split(" & ")));
            }

            return res;
        }

        public async Task<IEnumerable<Entity.Property>> GetByHeadingThree(string headingThreeTitle)
        {
            var res = new List<Entity.Property>();
            var con = (NpgsqlConnection?) _context.Database.GetDbConnection();
            if (con.State != ConnectionState.Open)
                await con.OpenAsync();

            var sql = @$"
                    SELECT max(CASE WHEN s.percent ISNULL THEN p.price ELSE p.price * (100 - s.percent) / 100 END),
                           min(CASE WHEN s.percent ISNULL THEN p.price ELSE p.price * (100 - s.percent) / 100 END)
                    FROM products as p
                             JOIN heading_three ht on p.heading_three_id = ht.heading_three_id
                             JOIN property_values pv on ht.property_values_id = pv.property_values_id
                             LEFT JOIN sale_products sp on p.product_id = sp.product_id
                             LEFT JOIN sales s on sp.sale_id = s.sale_id
                    WHERE pv.property_value='{headingThreeTitle}'
                    GROUP BY p.heading_three_id;";
            await using var cmd = new NpgsqlCommand(sql, con);
            await using NpgsqlDataReader rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                res.Add(new Entity.Property("Максимальная цена", new[] {rdr.GetInt32(0).ToString()}));
                res.Add(new Entity.Property("Минимальная цена", new[] {rdr.GetInt32(1).ToString()}));
            }

            await con.CloseAsync();

            if (con.State != ConnectionState.Open)
                await con.OpenAsync();
            
            
            sql = @$"
                    SELECT p2.title, string_agg(pv.property_value, ' & ')
                    FROM products as p
                             JOIN product_family pf on pf.product_family_id = p.product_family_id
                             JOIN heading_two ht on ht.heading_two_id = pf.heading_two_id
                             JOIN product_properties pp on p.product_id = pp.product_id
                             JOIN property_values pv on pp.property_values_id = pv.property_values_id
                             JOIN properties p2 on pv.property_id = p2.property_id
                    WHERE ht.title = (SELECT ht.title
                                        FROM heading_three
                                                 JOIN heading_two ht on ht.heading_two_id = heading_three.heading_two_id
                                                 JOIN property_values pv on pv.property_values_id = heading_three.property_values_id
                                        WHERE pv.property_value = '{headingThreeTitle}';)
                    GROUP BY p2.title;";
            await using var cmd2 = new NpgsqlCommand(sql, con);
            await using NpgsqlDataReader rdr2 = await cmd2.ExecuteReaderAsync();
            while (await rdr2.ReadAsync())
            {
                res.Add(new Entity.Property(rdr2.GetString(0), rdr2.GetString(1).Split(" & ")));
            }

            return res;
        }
    }
}