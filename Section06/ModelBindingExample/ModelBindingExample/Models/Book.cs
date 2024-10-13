﻿using Microsoft.AspNetCore.Mvc;

namespace ModelBindingExample.Models
{
    public class Book
    {
        //[FromQuery]
        public int? BookId { get; set; }
        public string? Author { get; set; }

        public override string ToString()
        {
            return $"Book object - id {BookId}, author {Author}"; 
        }
    }
}