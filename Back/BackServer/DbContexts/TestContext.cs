using Microsoft.EntityFrameworkCore;
using DbEntity;


namespace BackServer.Contexts
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=test;User Id=postgres;Password=postgres");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<HeadingOne> HeadingsOne { get; set; }
        public DbSet<HeadingTwo> HeadingsTwo { get; set; }
        public DbSet<HeadingThree> HeadingsThree { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<ProductProperty> ProductProperties { get; set; }
        public DbSet<SaleProducts> SaleProducts { get; set; }
        public DbSet<ProjectMaterials> ProjectMaterials { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            HeadingOneCreating(modelBuilder);
            HeadingTwoCreating(modelBuilder);
            PropertiesCreating(modelBuilder);
            HeadingThreeCreating(modelBuilder);
            ProductCreating(modelBuilder);
            ProductPropertiesCreating(modelBuilder);
            SalesCreating(modelBuilder);
            SaleProductsCreating(modelBuilder);
            ProjectsCreating(modelBuilder);
            ProjectsMaterialsCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void HeadingOneCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HeadingOne>(e => e.ToTable("heading_one"));
            modelBuilder.Entity<HeadingOne>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("heading_one_id")
                    .HasDefaultValueSql("nextval('account.item_id_seq'::regclass)");
                entity.Property(e => e.Title).IsRequired().HasColumnName("title");
            });
        }

        private void HeadingTwoCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HeadingTwo>(e => e.ToTable("heading_two"));
            modelBuilder.Entity<HeadingTwo>(entity =>
            {
                entity
                    .HasOne(e => e.HeadingOne)
                    .WithMany(e => e.HeadingsTwo)
                    .HasForeignKey("heading_one_id");

                entity.Property(e => e.Id)
                    .HasColumnName("heading_two_id")
                    .HasDefaultValueSql("nextval('account.item_id_seq'::regclass)");

                entity.Property(e => e.Title).IsRequired().HasColumnName("title");
            });
        }

        private void HeadingThreeCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HeadingThree>(e => e.ToTable("heading_three"));
            modelBuilder.Entity<HeadingThree>(entity =>
            {
                entity
                    .HasOne(e => e.HeadingTwo)
                    .WithMany(e => e.HeadingsThree)
                    .HasForeignKey("heading_two_id");

                entity
                    .HasOne(e => e.Property)
                    .WithMany(e => e.HeadingsThree)
                    .HasForeignKey("property_id");

                entity.Property(e => e.Id)
                    .HasColumnName("heading_three_id")
                    .HasDefaultValueSql("nextval('account.item_id_seq'::regclass)");
            });
        }

        private void PropertiesCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>(e => e.ToTable("properties"));
            modelBuilder.Entity<Property>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("property_id")
                    .HasDefaultValueSql("nextval('account.item_id_seq'::regclass)");

                entity.Property(e => e.Title).IsRequired().HasColumnName("title");
            });
        }

        private void ProductCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(e => e.ToTable("products"));
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("product_id")
                    .HasDefaultValueSql("nextval('account.item_id_seq'::regclass)");

                entity.Property(e => e.Title).IsRequired().HasColumnName("title").HasMaxLength(255);
                entity.Property(e => e.Description).IsRequired().HasColumnName("description");
                entity.Property(e => e.Price).IsRequired().HasColumnName("price");
                entity.Property(e => e.Quantity).IsRequired().HasColumnName("quantity");

                entity
                    .HasOne(e => e.HeadingOne)
                    .WithMany(e => e.Products)
                    .HasForeignKey("heading_one_id");

                entity
                    .HasOne(e => e.HeadingTwo)
                    .WithMany(e => e.Products)
                    .HasForeignKey("heading_two_id");

                entity
                    .HasOne(e => e.HeadingThree)
                    .WithMany(e => e.Products)
                    .HasForeignKey("heading_three_id");
            });
        }


        private void ProductPropertiesCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductProperty>(e => e.ToTable("product_properties"));
            modelBuilder.Entity<ProductProperty>(entity =>
            {
                entity.HasKey(x => new {PropertyId = x.property_id, ProductId = x.product_id});

                entity
                    .HasOne(e => e.Product)
                    .WithMany(e => e.ProductProperties)
                    .HasForeignKey("product_id");

                entity
                    .HasOne(e => e.Property)
                    .WithMany(e => e.ProductProperties)
                    .HasForeignKey("property_id");

                entity.Property(e => e.PropertyValue).HasColumnName("property_values");
            });
        }

        private void SalesCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>(e => e.ToTable("sales"));
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("sale_id")
                    .HasDefaultValueSql("nextval('account.item_id_seq'::regclass)");
                entity.Property(e => e.Title).IsRequired().HasColumnName("title");
                entity.Property(e => e.Description).IsRequired().HasColumnName("description");
                entity.Property(e => e.Percent).HasColumnName("percent");
            });
        }

        private void SaleProductsCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SaleProducts>(e => e.ToTable("sale_products"));

            modelBuilder.Entity<SaleProducts>(entity =>
            {
                entity
                    .HasOne(e => e.Sale)
                    .WithMany(e => e.ProductsSales)
                    .HasForeignKey("sale_id");

                entity
                    .HasOne(e => e.Product)
                    .WithMany(e => e.SaleProducts)
                    .HasForeignKey("product_id");
                
                entity.HasKey(x => new {SaleId = x.sale_id, ProductId = x.product_id});
            });
        }

        private void ProjectsCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(e => e.ToTable("projects"));
            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("project_id")
                    .HasDefaultValueSql("nextval('account.item_id_seq'::regclass)");
                entity.Property(e => e.Title).IsRequired().HasColumnName("title");
                entity.Property(e => e.RoofType).HasColumnName("roof_type");
            });
        }

        private void ProjectsMaterialsCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectMaterials>(e => e.ToTable("project_materials"));

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Projects)
                .UsingEntity<ProjectMaterials>(
                    e => e
                        .HasOne(x => x.Product)
                        .WithMany(x => x.ProjectMaterials)
                        .HasForeignKey("product_id"),
                    e => e
                        .HasOne(x => x.Project)
                        .WithMany(x => x.ProjectMaterials)
                        .HasForeignKey("project_id"),
                    e => e.HasKey(x => new {ProjectId = x.project_id, ProductId = x.product_id}));
            // modelBuilder.Entity<ProjectMaterials>(entity =>
            // {
            //     entity.HasKey(x => new {x.ProjectId, x.ProductId});
            //
            //     entity
            //         .HasOne(e => e.Project)
            //         .WithMany(e => e.ProjectMaterials)
            //         .HasForeignKey(x => x.ProjectId);
            //
            //     entity
            //         .HasOne(e => e.Product)
            //         .WithMany(e => e.ProjectMaterials)
            //         .HasForeignKey(x => x.ProductId);
            // });
        }
    }
}