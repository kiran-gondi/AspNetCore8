using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents DTO class that is used as return type of most methods of Persons Service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }
        public string? Tin { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj == null) return false;

            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;
            return PersonID == person.PersonID && PersonName == person.PersonName && Email == person.Email 
                && DateOfBirth == person.DateOfBirth && Gender == person.Gender && CountryID == person.CountryID 
                && Address == person.Address && ReceiveNewsLetters == person.ReceiveNewsLetters && Tin == person.Tin;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Person ID: {PersonID}, Person Name: {PersonName}, Email: {Email}, Date Of Birth: {DateOfBirth?.ToString("dd MMM yyyy")}," +
                $"Gender: {Gender}, Country ID: {CountryID}, Country: {Country}, Address: {Address}, Receive News Letters: {ReceiveNewsLetters}, " +
                $"Tin: {Tin}";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest
            {
                PersonID = PersonID, PersonName = PersonName, Email = Email, DateOfBirth = DateOfBirth, Address = Address,
                Gender= (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true), CountryID = CountryID, 
                ReceiveNewsLetters = ReceiveNewsLetters, Tin = Tin
            };
        }
    }

    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryID = person.CountryID,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.DateOfBirth != null) ?
                Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
                Tin = person.TIN
            };
        }
    }
}
