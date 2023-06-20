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
        public DbSet<ProductFamily> ProductFamilies { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<HeadingOne> HeadingsOne { get; set; }
        public DbSet<HeadingTwo> HeadingsTwo { get; set; }
        public DbSet<HeadingThree> HeadingsThree { get; set; }
        public DbSet<UnitMeasurement> UnitMeasurements { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<ProductProperty> ProductProperties { get; set; }
        public DbSet<PropertyValues> PropertyValues { get; set; }
        public DbSet<SaleProducts> SaleProducts { get; set; }
        public DbSet<ProjectMaterials> ProjectMaterials { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            HeadingOneCreating(modelBuilder);
            HeadingTwoCreating(modelBuilder);
            PropertiesCreating(modelBuilder);
            SalesCreating(modelBuilder);
            ProjectsCreating(modelBuilder);
            PropertyValuesCreating(modelBuilder);
            HeadingThreeCreating(modelBuilder);
            UnitMeasurementCreating(modelBuilder);
            ProductFamilyCreating(modelBuilder);
            ProductCreating(modelBuilder);
            ProductPropertiesCreating(modelBuilder);
            SaleProductsCreating(modelBuilder);
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
                entity.Property(e => e.PageLink).HasColumnName("page_link");
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
                    .HasForeignKey("heading_one_id")
                    .IsRequired();

                entity.Property(e => e.Id)
                    .HasColumnName("heading_two_id")
                    .HasDefaultValueSql("nextval('account.item_id_seq'::regclass)");

                entity.Property(e => e.Title).IsRequired().HasColumnName("title");
                entity.Property(e => e.PageLink).HasColumnName("page_link");
                entity.Property(e => e.ImageRef).HasColumnName("image_ref");
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
                entity.Property(e => e.Percent).IsRequired().HasColumnName("percent");
                entity.Property(e => e.PageLink).HasColumnName("page_link");
                entity.Property(e => e.ImageRef).HasColumnName("image_ref");
                entity.Property(e => e.Priority).HasColumnName("priority");
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
                entity.Property(e => e.PageLink).HasColumnName("page_link");
                entity.Property(e => e.ImageRef).HasColumnName("image_ref");
                entity.Property(e => e.Priority).HasColumnName("priority");
            });
        }

        private void PropertyValuesCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyValues>(e => e.ToTable("property_values"));
            modelBuilder.Entity<PropertyValues>(entity =>
            {
                entity
                    .HasOne(e => e.Property)
                    .WithMany(e => e.PropertyValues)
                    .HasForeignKey("property_id")
                    .IsRequired();

                entity.Property(e => e.Id)
                    .HasColumnName("property_values_id")
                    .HasDefaultValueSql("nextval('account.item_id_seq'::regclass)");

                entity.Property(e => e.PropertyValue).HasColumnName("property_value").IsRequired();
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
                    .HasForeignKey("heading_two_id")
                    .IsRequired();

                entity
                    .HasOne(e => e.PropertyValues)
                    .WithOne(e => e.HeadingThree)
                    .HasForeignKey(typeof(HeadingThree).ToString(), "property_values_id")
                    .IsRequired();

                entity.Property(e => e.Id)
                    .HasColumnName("heading_three_id")
                    .HasDefaultValueSql("nextval('account.item_id_seq'::regclass)");
                entity.Property(e => e.PageLink).HasColumnName("page_link");
                entity.Property(e => e.ImageRef).HasColumnName("image_ref");
            });
        }

        private void UnitMeasurementCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UnitMeasurement>(e => e.ToTable("units_measurement"));
            modelBuilder.Entity<UnitMeasurement>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("unit_measurement_id")
                    .HasDefaultValueSql("nextval('account.item_id_seq'::regclass)");

                entity.Property(e => e.Value).IsRequired().HasColumnName("unit_measurement_value");
            });
        }

        private void ProductFamilyCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductFamily>(e => e.ToTable("product_family"));

            modelBuilder.Entity<ProductFamily>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("product_family_id")
                    .HasDefaultValueSql("nextval('account.item_id_seq'::regclass)");
                
                entity.Property(e => e.Title).IsRequired().HasColumnName("title");

                entity
                    .HasOne(e => e.UnitMeasurement)
                    .WithMany(e => e.ProductFamilies)
                    .HasForeignKey("unit_measurement_id")
                    .IsRequired();

                entity
                    .HasOne(e => e.HeadingOne)
                    .WithMany(e => e.ProductFamilies)
                    .HasForeignKey("heading_one_id")
                    .IsRequired();

                entity
                    .HasOne(e => e.HeadingTwo)
                    .WithMany(e => e.ProductFamilies)
                    .HasForeignKey("heading_two_id")
                    .IsRequired();
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
                entity.Property(e => e.Popularity).IsRequired().HasColumnName("popularity");
                entity.Property(e => e.Available).IsRequired().HasColumnName("available");
                entity.Property(e => e.PageLink).HasColumnName("page_link");
                entity.Property(e => e.ImageRef).HasColumnName("image_ref");

                entity.Property(e => e.product_family_id).IsRequired().HasColumnName("product_family_id");

                entity
                    .HasOne(e => e.HeadingThree)
                    .WithMany(e => e.Products)
                    .HasForeignKey("heading_three_id");
                
                entity
                    .HasOne(e => e.ProductFamily)
                    .WithMany(e => e.Products)
                    .HasForeignKey("product_family_id");
            });
        }

        private void ProductPropertiesCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductProperty>(e => e.ToTable("product_properties"));
            modelBuilder.Entity<ProductProperty>(entity =>
            {
                entity.HasKey(x => new {PropertyValuesId = x.property_values_id, ProductId = x.product_id});

                entity
                    .HasOne(e => e.Product)
                    .WithMany(e => e.ProductProperties)
                    .HasForeignKey("product_id")
                    .IsRequired();

                entity
                    .HasOne(e => e.PropertyValues)
                    .WithMany(e => e.ProductProperties)
                    .HasForeignKey("property_values_id")
                    .IsRequired();
                entity.Property(e => e.IsPriority).IsRequired().HasColumnName("is_priority");
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
                    .HasForeignKey("sale_id")
                    .IsRequired();

                entity
                    .HasOne(e => e.Product)
                    .WithMany(e => e.SaleProducts)
                    .HasForeignKey("product_id")
                    .IsRequired();

                entity.HasKey(x => new {SaleId = x.sale_id, ProductId = x.product_id});
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
                        .HasForeignKey("product_id")
                        .IsRequired(),
                    e => e
                        .HasOne(x => x.Project)
                        .WithMany(x => x.ProjectMaterials)
                        .HasForeignKey("project_id")
                        .IsRequired(),
                    e => e.HasKey(x => new {ProjectId = x.project_id, ProductId = x.product_id}));
        }
    }
}