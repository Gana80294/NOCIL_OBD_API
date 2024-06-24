using Microsoft.EntityFrameworkCore;
using NOCIL_VP.Domain.Core.Entities.Approval;
using NOCIL_VP.Domain.Core.Entities.Auth;
using NOCIL_VP.Domain.Core.Entities.Logs;
using NOCIL_VP.Domain.Core.Entities.Mappings;
using NOCIL_VP.Domain.Core.Entities.Master;
using NOCIL_VP.Domain.Core.Entities.Registration;
using NOCIL_VP.Domain.Core.Entities.Registration.Attachments;
using NOCIL_VP.Domain.Core.Entities.Registration.CommonData;
using NOCIL_VP.Domain.Core.Entities.Registration.Domestic;
using NOCIL_VP.Domain.Core.Entities.Registration.Evaluation;
using NOCIL_VP.Domain.Core.Entities.Registration.Transport;

namespace NOCIL_VP.Domain.Core.Entities
{
    public class VpContext : DbContext
    {
        public VpContext(DbContextOptions options) : base(options)
        {
            Database.SetCommandTimeout(Int32.MaxValue);
        }

        // Authentication and MFA
        public DbSet<OtpTransaction> OtpTransactions { get; set; }
        public DbSet<ForgotPasswordOtpTransaction> ForgotPasswordOtpTransactions { get; set; }

        // User related tables
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }


        // Registration form tables
        public DbSet<Form> Forms { get; set; }
        public DbSet<VendorPersonalData> Vendor_Personal_Data { get; set; }
        public DbSet<VendorOrganizationProfile> Vendor_Organization_Profile { get; set; }
        public DbSet<TransportVendorPersonalData> Transport_Vendor_Personal_Data { get; set; }
        public DbSet<TankerDetail> Tanker_Details { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<VendorBranch> VendorBranches { get; set; }
        public DbSet<ProprietorOrPartner> Proprietor_or_Partners { get; set; }
        public DbSet<AnnualTurnOver> Annual_TurnOver { get; set; }
        public DbSet<TechnicalProfile> Technical_Profile { get; set; }
        public DbSet<Subsideries> Subsideries { get; set; }
        public DbSet<MajorCustomer> MajorCustomers { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Bank_Detail> Bank_Details { get; set; }
        public DbSet<CommercialProfile> Commercial_Profile { get; set; }
        public DbSet<NocilRelatedEmployee> NocilRelatedEmployees { get; set; }
        public DbSet<VendorGrade> VendorGrades { get; set; }

        public DbSet<AdditionalFields> AdditionalFields { get; set; }


        // Master tables
        public DbSet<CompanyCode> Company_Codes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<FormStatus> Form_Statuses { get; set; }
        public DbSet<PurchaseOrganization> Purchase_Organizations { get; set; }
        public DbSet<TankerType> Type_of_Tankers { get; set; }
        public DbSet<VendorType> Type_of_Vendors { get; set; }
        public DbSet<AddressType> Type_of_Addresses { get; set; }
        public DbSet<ContactType> Type_of_Contacts { get; set; }
        public DbSet<CompanyStatus> Company_Statuses { get; set; }
        public DbSet<OrganizationType> Organization_Types { get; set; }

        public DbSet<Title> Titles { get; set; }
        public DbSet<ReconciliationAccount> ReconciliationAccounts { get; set; }
        public DbSet<TaxBase> TaxBases { get; set; }
        public DbSet<Industry> Industry { get; set; }
        public DbSet<Incoterms> Incoterms { get; set; }
        public DbSet<SchemaGroup> SchemaGroups { get; set; }
        public DbSet<GSTVenClass> GSTVenClass { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Region> Region { get; set; }





        // Mapping Tables
        public DbSet<User_Role_Mapping> User_Role_Mappings { get; set; }

        // Approval tables
        public DbSet<WorkFlow> WorkFlows { get; set; }
        public DbSet<Tasks> Tasks { get; set; }

        // Transaction histories
        public DbSet<TransactionHistory> TransactionHistories { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User_Role_Mapping>().HasKey(t => new { t.Employee_Id, t.Role_Id });
            modelBuilder.Entity<User>(u =>
            {
                u.HasIndex(e => e.Employee_Id).IsUnique();
            });

            modelBuilder.Entity<Tasks>()
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(t => t.Owner_Id)
                .HasPrincipalKey(u => u.Employee_Id);

            modelBuilder.Entity<ForgotPasswordOtpTransaction>()
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(f => f.Employee_Id)
                .HasPrincipalKey(u => u.Employee_Id);

            modelBuilder.Entity<Country>(c =>
            {
                c.HasIndex(c => c.Code).IsUnique();
            });

            modelBuilder.Entity<Region>().HasOne(c => c.Country).WithMany().HasForeignKey(r => r.Country_Code).HasPrincipalKey(c => c.Code);
            modelBuilder.Entity<Address>().HasOne(c => c.Country).WithMany().HasForeignKey(r => r.Country_Code).HasPrincipalKey(c => c.Code);


            // Query Filters
            modelBuilder.Entity<AddressType>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<VendorType>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<TankerType>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<Role>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<PurchaseOrganization>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<OrganizationType>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<FormStatus>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<Department>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<ContactType>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<CompanyStatus>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<CompanyCode>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<User>().HasQueryFilter(x => x.Is_Active);

            modelBuilder.Entity<Title>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<ReconciliationAccount>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<TaxBase>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<Industry>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<Incoterms>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<SchemaGroup>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<GSTVenClass>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<Country>().HasQueryFilter(x => !x.Is_Deleted);
            modelBuilder.Entity<Region>().HasQueryFilter(x => !x.Is_Deleted);

        }

    }
}
