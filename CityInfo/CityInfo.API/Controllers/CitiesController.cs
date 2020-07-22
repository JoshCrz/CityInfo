﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);            
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            //var city = CitiesDataStore.Current.Cities.Where(x => x.Id == id);

            var cityToReturn = CitiesDataStore.Current.Cities
                    .FirstOrDefault(c => c.Id == id);

            if(cityToReturn == null)
            {
                return NotFound();
            }

            return Ok(cityToReturn);
                
            /*return new JsonResult(
                CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id));
            */
        }


    }
}
