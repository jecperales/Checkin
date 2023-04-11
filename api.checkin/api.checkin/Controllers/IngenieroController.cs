using api.checkin.data.Repositories;
using api.checkin.model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.checkin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class IngenieroController : ControllerBase
    {
        private readonly IIngenieroRepository _ingenieroRepository;

        public IngenieroController(IIngenieroRepository ingenieroRepository) 
        {
            _ingenieroRepository = ingenieroRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIngenieros() 
        {
            return Ok(await _ingenieroRepository.GetAllIngenieros());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIngeniero(int id) 
        {
            return Ok(await _ingenieroRepository.GetIngeniero(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateIngeniero([FromBody] Ingeniero ingeniero) 
        {
            if (ingeniero == null)
                return BadRequest();

            if(!ModelState.IsValid)
                return BadRequest();

            var created = await _ingenieroRepository.InsertIngeniero(ingeniero);

            return Created("Created", created);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateIngeniero([FromBody] Ingeniero ingeniero)
        {
            if (ingeniero == null)
                return BadRequest();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await _ingenieroRepository.UpdateIngeniero(ingeniero);

            return NoContent();
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteIngeniero(int id)
        //{
        //    await _ingenieroRepository.DeleteIngeniero(new Ingeniero { id_ingeniero = id});

        //    return NoContent();
        //}

        [HttpGet("Auth/")]
        public async Task<IActionResult> Auth(string uid, string pwd)
        {
            if (String.IsNullOrEmpty(uid) | String.IsNullOrEmpty(pwd))            
                return BadRequest("Usuario/Password no pueden ser vacios");

            return Ok(await _ingenieroRepository.Authenticate(uid, pwd));
            
        }

    }
}
