﻿using Microsoft.AspNetCore.Mvc;
using ModelValidationsExample.CustomModelBinders;
using ModelValidationsExample.Models;

namespace ModelValidationsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("register")]
        //public IActionResult Index([Bind(nameof(Person.PersonName), nameof(Person.Email), 
        //    nameof(Person.Password), nameof(Person.ConfirmPassword))] Person person)
        //public IActionResult Index([FromBody][ModelBinder(BinderType = typeof(PersonModelBinder))]Person person)
        public IActionResult Index(Person person, [FromHeader(Name ="User-Agent")]string UserAgent)
        {
            if (!ModelState.IsValid) {
                string errors = string.Join("\n", 
                    ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));

                //foreach (var item in ModelState.Values) {
                //    foreach(var error in item.Errors)
                //    {
                //        errorsList.Add(error.ErrorMessage);
                //    }
                //}

                return BadRequest(errors);
            }

            return Content($"{person}, {UserAgent}");
        }
    }
}
