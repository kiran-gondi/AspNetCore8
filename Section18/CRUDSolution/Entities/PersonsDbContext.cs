using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class PersonsDbContext : DbContext
    {
        public PersonsDbContext(DbContextOptions options) : base (options)
        {
                
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            SeedCountries(modelBuilder);
            SeedPersons(modelBuilder);

            //Fluent API
            modelBuilder.Entity<Person>().Property(temp => temp.TIN)
                .HasColumnName("TIN")
                .HasColumnType("varchar(8)")
                .HasDefaultValue("ABC12345");

            //modelBuilder.Entity<Person>().HasIndex(temp => temp.TIN).IsUnique();

            /*Obsolete
             * modelBuilder.Entity<Person>()
                .HasCheckConstraint("CHK_TIN", "len([TIN]) = 8");  */

            //Check Constraints
            modelBuilder.Entity<Person>().ToTable(x => x.HasCheckConstraint("CHK_TIN", "len([TIN]) = 8"));
        }

        private static void SeedCountries(ModelBuilder modelBuilder)
        {
            //Seed to Countries
            string countriesJson = File.ReadAllText("countries.json");
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach (var country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }
        }

        private static void SeedPersons(ModelBuilder modelBuilder)
        {
            //Seed to Countries
            string personsJson = File.ReadAllText("persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (var person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }
        }

        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@PersonID", person.PersonID),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryID", person.CountryID),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters),
                new SqlParameter("@TIN", person.TIN),
            };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson]@PersonID, @PersonName, @Email, @DateOfBirth, @Gender, " +
                "@CountryID, @Address, @ReceiveNewsLetters, @TIN", parameters);
        }
    }
}
