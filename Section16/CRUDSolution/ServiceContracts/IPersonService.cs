using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity.
    /// </summary>
    public interface IPersonService
    {
        /// <summary>
        /// Adds a new person into the existing list of persons
        /// </summary>
        /// <param name="personAddRequest"></param>
        /// <returns></returns>
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);
        
        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns></returns>
        List<PersonResponse> GetAllPersons();

        /// <summary>
        /// Returns the person object based on the given person id
        /// </summary>
        /// <param name="personID">Person id to search</param>
        /// <returns>Returns matching person object</returns>
        PersonResponse? GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Returns all person objects that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Returns all matching persons based on the given search field and search string</returns>
        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);

        /// <summary>
        /// Retrusn sorted list of persons
        /// </summary>
        /// <param name="allPersons"></param>
        /// <param name="sortBy">Name of the property (key), based on which the persons should be sorted.</param>
        /// <param name="sortOrder"></param>
        /// <returns>Returns sorted persons as List<PersonResponse></returns>
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Updates the specified person details based on the give person ID
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update, incuding the person id</param>
        /// <returns>PersonResponse</returns>
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Deletes a person by using personID
        /// </summary>
        /// <param name="personID"></param>
        /// <returns>bool true or false if the person is deleted or not</returns>
        bool DeletePerson(Guid? personID);
    }
}
