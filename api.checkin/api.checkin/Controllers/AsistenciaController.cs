using api.checkin.data.Repositories;
using api.checkin.model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.checkin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AsistenciaController : ControllerBase
    {
        private readonly IControlAsistenciaRepository _asistenciaRepository;

        public AsistenciaController(IControlAsistenciaRepository asistenciaRepository)
        {
            _asistenciaRepository = asistenciaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsistencias()
        {
            return Ok(await _asistenciaRepository.GetAllAsistencias());
        }

        [HttpGet("Asistencia/")]
        public async Task<IActionResult> GetAsistencia(DateTime fecha, int id)
        {
            return Ok(await _asistenciaRepository.GetAsistencia(fecha, id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsistencia([FromBody] control_asistencia asistencia)
        {
            if (asistencia == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var created = await _asistenciaRepository.InsertAsistencia(asistencia);

            return Created("Created", created);

        }

        [HttpPut("Update/")]
        [HttpOptions]
        public async Task<IActionResult> UpdateAsistencia([FromBody] control_asistencia asistencia)
        {
            if (asistencia == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _asistenciaRepository.UpdateAsistencia(asistencia);

            return NoContent();
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteIngeniero(int id)
        //{
        //    await _ingenieroRepository.DeleteIngeniero(new Ingeniero { id_ingeniero = id});

        //    return NoContent();
        //}

    }
}
