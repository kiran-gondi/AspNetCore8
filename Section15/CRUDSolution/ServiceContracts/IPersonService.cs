﻿using ServiceContracts.DTO;

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
    }
}
