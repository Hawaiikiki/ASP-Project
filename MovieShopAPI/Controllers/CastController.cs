﻿using ApplicationCore.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastController : ControllerBase
    {
        private readonly ICastService _castService;
        public CastController(ICastService castService)
        {
            _castService = castService;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> CastDetails(int id)
        {
            var cast = await _castService.GetCastDetails(id);
            if(cast!= null)
            {
                return Ok(cast);
            }
            return BadRequest(new { errorMessage = "There is no cast with given id" });
        }
    }
}
