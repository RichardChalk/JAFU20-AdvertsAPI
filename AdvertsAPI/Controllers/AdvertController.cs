using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AdvertsAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AdvertController(ApplicationDbContext context)
        {
            _context = context;
        }


        // CREATE ////////////////////////////////////////////////////////
        /// <summary>
        /// Create and add an advert to the database
        /// </summary>
        /// <param 
        /// name="advert">Below is the format needed for the request body
        /// </param>
        /// <returns>
        /// A full list of ALL adverts
        /// </returns>
        /// <remarks>
        /// Example end point: POST /api/Advert
        /// </remarks>
        /// <response code="201">
        /// Your advert was created successfully
        /// </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]

        public async Task<ActionResult<List<Advert>>> Post([FromBody] Advert advert)
        {
            _context.Adverts.Add(advert);
            await _context.SaveChangesAsync();
            return Ok(await _context.Adverts.ToListAsync());
        }


        // READ ALL ///////////////////////////////////////////////////////
        /// <summary>
        /// Retrieve ALL adverts from the database
        /// </summary>
        /// <returns>
        /// A full list of ALL adverts
        /// </returns>
        /// <remarks>
        /// Example end point: GET /api/Advert
        /// </remarks>
        /// <response code="200">
        /// Successfully returned a full list of ALL adverts
        /// </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<Advert>>> GetAll()
        {
            return Ok(await _context.Adverts.ToListAsync());
        }


        // READ ONE ///////////////////////////////////////////////////////
        /// <summary>
        /// Retrieve a SPECIFIC advert from the database
        /// </summary>
        /// <param name="id">
        /// Id of specific advert
        /// </param>
        /// <returns>
        /// The chosen advert (by Id)
        /// </returns>
        /// <remarks>
        /// Example end point: GET /api/Advert/1
        /// </remarks>
        /// <response code="200">
        /// Successfully returned the chosen advert (by Id)
        /// </response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Advert>> GetOne(int id)
        {
            var advert = await _context.Adverts.FindAsync(id);

            if (advert == null)
            {
                return BadRequest("Advert not found");
            }
            return Ok(advert);
        }


        // UPDATE ////////////////////////////////////////////////////////
        /// <summary>
        /// Update ALL properties of 1 advert in the database
        /// </summary>
        /// <param name="request">
        /// An instance of the Advert class
        /// </param>
        /// <returns>
        /// A full list of ALL adverts
        /// </returns>
        /// <remarks>
        /// Example end point: PUT /api/Advert
        /// </remarks>
        /// <response code="200">
        /// Your advert was updated successfully
        /// </response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<Advert>>> Put([FromBody] Advert request)
        {
            var dbAdvert = await _context.Adverts.FindAsync(request.Id);

            if (dbAdvert == null)
            {
                return BadRequest("Advert not found");
            }

            // Automapper possible here
            dbAdvert.Name = request.Name;
            dbAdvert.Description = request.Description;
            dbAdvert.Price = request.Price;
            dbAdvert.DateAdded = request.DateAdded;

            await _context.SaveChangesAsync();
            return Ok(await _context.Adverts.ToListAsync());
        }


        // UPDATE (PATCH) ////////////////////////////////////////////////
        // Nuget
        // Microsoft.AspNetCore.JsonPatch
        // Microsoft.AspNetCore.MVC.NewtonsoftJson

        /// <summary>
        /// Partially update a SPECIFIC advert from the database
        /// </summary>
        /// <param name="id">
        /// Id of specific advert
        /// </param>
        /// <param name="request">
        /// An instance of the Advert class
        /// </param>
        /// <returns>
        /// The chosen partially updated advert (by Id)
        /// </returns>
        /// <remarks>
        /// Example end point: PATCH /api/Advert/1
        /// </remarks>
        /// <response code="200">
        /// Advert was partially updated successfully (by Id)
        /// </response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Patch2([FromBody] JsonPatchDocument request, int id)
        {
            var dbAdvert = await _context.Adverts.FindAsync(id);

            if (dbAdvert == null)
            {
                return BadRequest("Advert not found");
            }

            request.ApplyTo(dbAdvert);
            return Ok(dbAdvert);
        }


        // DELETE ////////////////////////////////////////////////////////
        /// <summary>
        /// Retrieve a SPECIFIC advert from the database for deletion
        /// </summary>
        /// <param name="id">
        /// Id of specific advert
        /// </param>
        /// <returns>
        /// The entire list of ALL adverts
        /// </returns>
        /// <remarks>
        /// Example end point: DELETE /api/Advert/1
        /// </remarks>
        /// <response code="200">
        /// Successfully deleted the chosen advert (by Id)
        /// </response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<Advert>>> Delete(int id)
        {
            var dbAdvert = await _context.Adverts.FindAsync(id);

            if (dbAdvert == null)
            {
                return BadRequest("Advert not found");
            }

            _context.Adverts.Remove(dbAdvert);
            await _context.SaveChangesAsync();

            return Ok(await _context.Adverts.ToListAsync());
        }
    }
}
