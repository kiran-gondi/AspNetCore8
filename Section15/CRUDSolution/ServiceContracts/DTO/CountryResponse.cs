﻿using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as return type for most of CoutnriesService methods.
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

        //It compares the curent object to another object of CountryResponse type and returns true,
        //if both values are same otherwise return false.
        public override bool Equals(object? obj)
        {
            if(obj == null)
            {
                return false;
            }

            if(obj.GetType() != typeof(CountryResponse))
            {
                return false; 
            }
            CountryResponse country_to_compare = (CountryResponse)obj;
            return this.CountryID == country_to_compare.CountryID && this.CountryName == country_to_compare.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CountryExtensions 
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName
            };
        }
    }
}
