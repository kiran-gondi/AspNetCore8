using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface IPersonsRepository
    {
        Task<Person> AddPerson(Person person);
        Task<List<Person>> GetAllPersons();

        Task<Person?> GetPersonByPersonID(Guid personID);

        /// <summary>
        /// Returns all person objects based on the give expression
        /// </summary>
        /// <param name="perdicate">LINQ expression to check</param>
        /// <returns>All matching persons with give conditions</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> perdicate);

        Task<bool> DeletePersonByPersonID(Guid personID);
        Task<Person> UpdatePerson(Person person);
    }
}
