using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using System.Configuration;

#nullable disable

namespace PawnShop.DataAccess.Data
{
    public partial class PawnshopContext : DbContext
    {
        public PawnshopContext()
        {
        }

        public PawnshopContext(DbContextOptions<PawnshopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<ContractItem> ContractItems { get; set; }
        public virtual DbSet<ContractItemCategory> ContractItemCategories { get; set; }
        public virtual DbSet<ContractItemState> ContractItemStates { get; set; }
        public virtual DbSet<ContractRenew> ContractRenews { get; set; }
        public virtual DbSet<ContractState> ContractStates { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<DealDocument> DealDocuments { get; set; }
        public virtual DbSet<Gemstone> Gemstones { get; set; }
        public virtual DbSet<GoldProduct> GoldProducts { get; set; }
        public virtual DbSet<GoldProductGemstone> GoldProductGemstones { get; set; }
        public virtual DbSet<GoldProductType> GoldProductTypes { get; set; }
        public virtual DbSet<GoldTest> GoldTests { get; set; }
        public virtual DbSet<Laptop> Laptops { get; set; }
        public virtual DbSet<LendingRate> LendingRates { get; set; }
        public virtual DbSet<Link> Links { get; set; }
        public virtual DbSet<LocalSale> LocalSales { get; set; }
        public virtual DbSet<MoneyBalance> MoneyBalances { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Privilege> Privileges { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<Telephone> Telephones { get; set; }
        public virtual DbSet<UnitMeasure> UnitMeasures { get; set; }
        public virtual DbSet<WorkerBoss> WorkerBosses { get; set; }
        public virtual DbSet<WorkerBossType> WorkerBossTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["PawnShopDatabaseProduction"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address", "Worker");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.ApartmentNumber).HasMaxLength(10);

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.HouseNumber)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.PostCode)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Address_City");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Address_Country");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City", "Worker");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.City1)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("City");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("City_Country");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client", "Worker");

                entity.HasIndex(e => e.ClientId, "id_person")
                    .IsUnique();

                entity.Property(e => e.ClientId)
                    .ValueGeneratedNever()
                    .HasColumnName("ClientID");

                entity.Property(e => e.IdcardNumber)
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasColumnName("IDCardNumber");

                entity.Property(e => e.Pesel)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.ValidityDateIdcard)
                    .HasColumnType("date")
                    .HasColumnName("ValidityDateIDCard");

                entity.HasOne(d => d.ClientNavigation)
                    .WithOne(p => p.Client)
                    .HasForeignKey<Client>(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Client_Person");
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.HasKey(e => e.ContractNumberId)
                    .HasName("Contract_pk");

                entity.ToTable("Contract", "Worker");

                entity.HasIndex(e => e.CreateContractDealDocumentId, "DealDocumentUniqueContract")
                    .IsUnique();

                entity.Property(e => e.ContractNumberId)
                    .HasMaxLength(10)
                    .HasColumnName("ContractNumberID");

                entity.Property(e => e.AmountContract).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.BuyBackDealDocumentId).HasColumnName("BuyBackDealDocumentID");

                entity.Property(e => e.BuyBackId).HasColumnName("BuyBackID");

                entity.Property(e => e.ContractStateId).HasColumnName("ContractStateID");

                entity.Property(e => e.CreateContractDealDocumentId).HasColumnName("CreateContractDealDocumentID");

                entity.Property(e => e.DealMakerId).HasColumnName("DealMakerID");

                entity.Property(e => e.LendingRateId).HasColumnName("LendingRateID");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.WorkerBossId).HasColumnName("WorkerBossID");

                entity.HasOne(d => d.BuyBackDealDocument)
                    .WithMany(p => p.ContractBuyBackDealDocuments)
                    .HasForeignKey(d => d.BuyBackDealDocumentId)
                    .HasConstraintName("BuyBackContract_DealDocument");

                entity.HasOne(d => d.BuyBack)
                    .WithMany(p => p.ContractBuyBacks)
                    .HasForeignKey(d => d.BuyBackId)
                    .HasConstraintName("wykupienie");

                entity.HasOne(d => d.ContractState)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.ContractStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Contract_ContractState");

                entity.HasOne(d => d.CreateContractDealDocument)
                    .WithOne(p => p.ContractCreateContractDealDocument)
                    .HasForeignKey<Contract>(d => d.CreateContractDealDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CreateContract_DealDocument");

                entity.HasOne(d => d.DealMaker)
                    .WithMany(p => p.ContractDealMakers)
                    .HasForeignKey(d => d.DealMakerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("zalozenie");

                entity.HasOne(d => d.LendingRate)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.LendingRateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Contract_LendingRate");

                entity.HasOne(d => d.WorkerBoss)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.WorkerBossId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Contract_WorkerBoss");
            });

            modelBuilder.Entity<ContractItem>(entity =>
            {
                entity.ToTable("ContractItem", "Worker");

                entity.Property(e => e.ContractItemId).HasColumnName("ContractItemID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.ContractItemStateId).HasColumnName("ContractItemStateID");

                entity.Property(e => e.ContractNumberId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("ContractNumberID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EstimatedValue).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.TechnicalCondition)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ContractItems)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ContractItem_ContractItemCategory");

                entity.HasOne(d => d.ContractItemState)
                    .WithMany(p => p.ContractItems)
                    .HasForeignKey(d => d.ContractItemStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ContractItem_ContractItemState");

                entity.HasOne(d => d.ContractNumber)
                    .WithMany(p => p.ContractItems)
                    .HasForeignKey(d => d.ContractNumberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ContractItem_Contract");
            });

            modelBuilder.Entity<ContractItemCategory>(entity =>
            {
                entity.ToTable("ContractItemCategory", "Worker");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.MeasureId).HasColumnName("MeasureID");

                entity.HasOne(d => d.Measure)
                    .WithMany(p => p.ContractItemCategories)
                    .HasForeignKey(d => d.MeasureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ContractItemCategory_Measure");
            });

            modelBuilder.Entity<ContractItemState>(entity =>
            {
                entity.ToTable("ContractItemState", "Worker");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<ContractRenew>(entity =>
            {
                entity.HasKey(e => new { e.RenewContractId, e.DealDocumentId })
                    .HasName("ContractRenew_pk");

                entity.ToTable("ContractRenew", "Worker");

                entity.HasIndex(e => e.DealDocumentId, "DealDocumentUniqueRenew")
                    .IsUnique();

                entity.Property(e => e.RenewContractId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RenewContractID");

                entity.Property(e => e.DealDocumentId).HasColumnName("DealDocumentID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.ContractNumberId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("ContractNumberID");

                entity.Property(e => e.LendingRateId).HasColumnName("LendingRateID");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ContractRenews)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("ContractClientRenew_Client");

                entity.HasOne(d => d.ContractNumber)
                    .WithMany(p => p.ContractRenews)
                    .HasForeignKey(d => d.ContractNumberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ContractClientRenew_Contract");

                entity.HasOne(d => d.DealDocument)
                    .WithOne(p => p.ContractRenew)
                    .HasForeignKey<ContractRenew>(d => d.DealDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ContractClientRenew_DealDocument");

                entity.HasOne(d => d.LendingRate)
                    .WithMany(p => p.ContractRenews)
                    .HasForeignKey(d => d.LendingRateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ContractClientRenew_LendingRate");
            });

            modelBuilder.Entity<ContractState>(entity =>
            {
                entity.ToTable("ContractState", "Worker");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country", "Worker");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Country1)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Country");
            });

            modelBuilder.Entity<DealDocument>(entity =>
            {
                entity.ToTable("DealDocument", "Worker");

                entity.HasIndex(e => e.PaymentId, "PaymentIDUnique")
                    .IsUnique();

                entity.Property(e => e.DealDocumentId).HasColumnName("DealDocumentID");

                entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Income).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.MoneyBalanceId)
                    .HasColumnType("date")
                    .HasColumnName("MoneyBalanceID");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.Profit).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.RepaymentCapital).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.MoneyBalance)
                    .WithMany(p => p.DealDocuments)
                    .HasForeignKey(d => d.MoneyBalanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DealDocument_MoneyBalance");

                entity.HasOne(d => d.Payment)
                    .WithOne(p => p.DealDocument)
                    .HasForeignKey<DealDocument>(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DealDocument_Payment");
            });

            modelBuilder.Entity<Gemstone>(entity =>
            {
                entity.ToTable("Gemstone", "Worker");

                entity.Property(e => e.GemstoneId).HasColumnName("GemstoneID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<GoldProduct>(entity =>
            {
                entity.HasKey(e => e.ContractitemId)
                    .HasName("GoldProduct_pk");

                entity.ToTable("GoldProduct", "Worker");

                entity.Property(e => e.ContractitemId)
                    .ValueGeneratedNever()
                    .HasColumnName("ContractitemID");

                entity.Property(e => e.GoldTestId).HasColumnName("GoldTestID");

                entity.Property(e => e.Grammage).HasColumnType("decimal(10, 3)");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.HasOne(d => d.Contractitem)
                    .WithOne(p => p.GoldProduct)
                    .HasForeignKey<GoldProduct>(d => d.ContractitemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GoldProduct_ContractItem");

                entity.HasOne(d => d.GoldTest)
                    .WithMany(p => p.GoldProducts)
                    .HasForeignKey(d => d.GoldTestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GoldProduct_GoldTest");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.GoldProducts)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GoldProduct_GoldProductType");
            });

            modelBuilder.Entity<GoldProductGemstone>(entity =>
            {
                entity.HasKey(e => new { e.GoldProductId, e.GemstoneId })
                    .HasName("GoldProductGemstone_pk");

                entity.ToTable("GoldProductGemstone", "Worker");

                entity.Property(e => e.GoldProductId).HasColumnName("GoldProductID");

                entity.Property(e => e.GemstoneId).HasColumnName("GemstoneID");

                entity.HasOne(d => d.Gemstone)
                    .WithMany(p => p.GoldProductGemstones)
                    .HasForeignKey(d => d.GemstoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GoldProductGemstone_Gemstone");

                entity.HasOne(d => d.GoldProduct)
                    .WithMany(p => p.GoldProductGemstones)
                    .HasForeignKey(d => d.GoldProductId)
                    .HasConstraintName("GoldProductGemstone_GoldProduct");
            });

            modelBuilder.Entity<GoldProductType>(entity =>
            {
                entity.ToTable("GoldProductType", "Worker");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<GoldTest>(entity =>
            {
                entity.ToTable("GoldTest", "Worker");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GoldTest1)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnName("GoldTest");
            });

            modelBuilder.Entity<Laptop>(entity =>
            {
                entity.HasKey(e => e.ContractItemId)
                    .HasName("Laptop_pk");

                entity.ToTable("Laptop", "Worker");

                entity.Property(e => e.ContractItemId)
                    .ValueGeneratedNever()
                    .HasColumnName("ContractItemID");

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.DescriptionKit)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DriveType)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.MassStorage)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Procesor)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Ram)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("RAM");

                entity.HasOne(d => d.ContractItem)
                    .WithOne(p => p.Laptop)
                    .HasForeignKey<Laptop>(d => d.ContractItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Laptop_ContractItem");
            });

            modelBuilder.Entity<LendingRate>(entity =>
            {
                entity.ToTable("LendingRate", "Worker");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Link>(entity =>
            {
                entity.ToTable("Link", "Worker");

                entity.Property(e => e.LinkId).HasColumnName("LinkID");

                entity.Property(e => e.Link1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Link");

                entity.Property(e => e.SaleId).HasColumnName("SaleID");

                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.Links)
                    .HasForeignKey(d => d.SaleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Link_Sale");
            });

            modelBuilder.Entity<LocalSale>(entity =>
            {
                entity.HasKey(e => e.SaleId)
                    .HasName("LocalSale_pk");

                entity.ToTable("LocalSale", "Worker");

                entity.Property(e => e.SaleId)
                    .ValueGeneratedNever()
                    .HasColumnName("SaleID");

                entity.Property(e => e.Rack)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Sale)
                    .WithOne(p => p.LocalSale)
                    .HasForeignKey<LocalSale>(d => d.SaleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LocalInternetSale_Sale");
            });

            modelBuilder.Entity<MoneyBalance>(entity =>
            {
                entity.HasKey(e => e.TodayDate)
                    .HasName("MoneyBalance_pk");

                entity.ToTable("MoneyBalance", "Worker");

                entity.Property(e => e.TodayDate).HasColumnType("date");

                entity.Property(e => e.MoneyBalance1)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("MoneyBalance");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment", "Worker");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.PaymentTypeId).HasColumnName("PaymentTypeID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("Payment_Client");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Payment_PaymentType");
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.ToTable("PaymentType", "Worker");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person", "Worker");

                entity.HasIndex(e => e.AddressId, "Person_ak_1")
                    .IsUnique();

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Address)
                    .WithOne(p => p.Person)
                    .HasForeignKey<Person>(d => d.AddressId)
                    .HasConstraintName("Person_Address");
            });

            modelBuilder.Entity<Privilege>(entity =>
            {
                entity.ToTable("Privilege", "Boss");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("Sale", "Worker");

                entity.Property(e => e.SaleId).HasColumnName("SaleID");

                entity.Property(e => e.ContractItemId).HasColumnName("ContractItemID");

                entity.Property(e => e.PutOnSaleDate).HasColumnType("date");

                entity.Property(e => e.SaleDate).HasColumnType("date");

                entity.Property(e => e.SalePrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.SoldPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.ContractItem)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.ContractItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Sale_ContractItem");
            });

            modelBuilder.Entity<Telephone>(entity =>
            {
                entity.HasKey(e => e.ContractitemId)
                    .HasName("Telephone_pk");

                entity.ToTable("Telephone", "Worker");

                entity.Property(e => e.ContractitemId)
                    .ValueGeneratedNever()
                    .HasColumnName("ContractitemID");

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.DescriptionKit)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MassStorage)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Procesor)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Ram)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("RAM");

                entity.Property(e => e.ScreenSize).HasColumnType("decimal(4, 2)");

                entity.HasOne(d => d.Contractitem)
                    .WithOne(p => p.Telephone)
                    .HasForeignKey<Telephone>(d => d.ContractitemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Telephone_ContractItem");
            });

            modelBuilder.Entity<UnitMeasure>(entity =>
            {
                entity.ToTable("UnitMeasure", "Worker");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Measure)
                    .IsRequired()
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<WorkerBoss>(entity =>
            {
                entity.ToTable("WorkerBoss", "Boss");

                entity.Property(e => e.WorkerBossId)
                    .ValueGeneratedNever()
                    .HasColumnName("WorkerBossID");

                entity.Property(e => e.DatePhysicalCheckUp).HasColumnType("date");

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .HasMaxLength(89);

                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.Pesel)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.PrivilegeId).HasColumnName("PrivilegeID");

                entity.Property(e => e.WorkerBossTypeId).HasColumnName("WorkerBossTypeID");

                entity.HasOne(d => d.Privilege)
                    .WithMany(p => p.WorkerBosses)
                    .HasForeignKey(d => d.PrivilegeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WorkerBoss_Privilege");

                entity.HasOne(d => d.WorkerBossNavigation)
                    .WithOne(p => p.WorkerBoss)
                    .HasForeignKey<WorkerBoss>(d => d.WorkerBossId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("WorkerBoss_Person");

                entity.HasOne(d => d.WorkerBossType)
                    .WithMany(p => p.WorkerBosses)
                    .HasForeignKey(d => d.WorkerBossTypeId)
                    .HasConstraintName("WorkerBoss_WorkerBossType");
            });

            modelBuilder.Entity<WorkerBossType>(entity =>
            {
                entity.ToTable("WorkerBossType", "Boss");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
