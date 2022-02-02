using Microsoft.AspNetCore.Cors;

namespace AdvertsAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class AdvertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AdvertController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE ////////////////////////////////////////////////////////
        /// <summary>
        /// (Admin only) Create and add an advert to the database
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
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Advert>>> Post([FromBody] AdvertDTO advertModel)
        {
            // Too lazy for Automapper
            var advert = new Advert
            {
                Id = advertModel.Id,
                Name = advertModel.Name,
                Description = advertModel.Description,
                Price = advertModel.Price,
                DateAdded = advertModel.DateAdded,
            };
            
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
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<List<AdvertDTO>>> GetAll()
        {
            // Too lazy for Automapper
            var adverts = _context.Adverts
                .Select(a => new AdvertDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Price = a.Price,
                    DateAdded = a.DateAdded,
                });

            return Ok(adverts);
               
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
        [HttpGet("GetOne/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<AdvertDTO>> GetOne(int id)
        {
            // Too lazy for Automapper
            var advert = _context.Adverts
                .Where(a => a.Id == id)
                .Select(a => new AdvertDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Price = a.Price,
                    DateAdded = a.DateAdded,
                });

            if (advert == null)
            {
                return NotFound("Advert not found");
            }
            return Ok(advert);
        }


        // UPDATE ////////////////////////////////////////////////////////
        /// <summary>
        /// (Admin only) Update ALL properties of 1 advert in the database
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
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<AdvertDTO>>> Put([FromBody] AdvertDTO request)
        {
            var dbAdvert = await _context.Adverts.FindAsync(request.Id);

            if (dbAdvert == null)
            {
                return NotFound("Advert not found");
            }

            // Too lazy for Automapper
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
        /// (Admin only) Partially update a SPECIFIC advert from the database
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
        [HttpPatch("PartialUpdate/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Patch([FromBody] JsonPatchDocument request, int id)
        {
            var dbAdvert = await _context.Adverts.FindAsync(id);

            if (dbAdvert == null)
            {
                return NotFound("Advert not found");
            }

            request.ApplyTo(dbAdvert);
            await _context.SaveChangesAsync();

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
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<AdvertDTO>>> Delete(int id)
        {
            var dbAdvert = await _context.Adverts.FindAsync(id);

            if (dbAdvert == null)
            {
                return NotFound("Advert not found");
            }

            _context.Adverts.Remove(dbAdvert);
            await _context.SaveChangesAsync();

            return Ok(await _context.Adverts.ToListAsync());
        }
    }
}
