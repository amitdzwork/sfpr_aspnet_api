    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using FacilityApi.Data;
    using FacilityApi.Models;
    using Swashbuckle.AspNetCore.Annotations;
    using System.Net;
    using MongoDB.Driver;

    namespace FacilityApi.Controllers
    {

        [Route("api/v1/facilities")]
        [ApiController]
        public class FacilityController : ControllerBase
        {
            private readonly FacilityService _facilityService;

            public FacilityController(FacilityService facilityService)
            {
                _facilityService = facilityService;
            }

            /// <summary>
            /// Gets all the facilities listed in San Francisco
            /// </summary>
            /// <returns></returns>
            [HttpGet]
            [SwaggerResponse((int)HttpStatusCode.NoContent, "No facilities have been filed with this system")]
            public async Task<IActionResult> GetFacilities()
            {
                try
                {
                    var facilityList = await _facilityService.GetAsync();

                    if (facilityList.Count == 0)
                    {
                        return StatusCode(StatusCodes.Status204NoContent);
                    }
                    return Ok(facilityList);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }

            /// <summary>
            /// Gets all the facilities in San Francisco of the specified type
            /// </summary>
            /// <param name="facilityType"></param>
            /// <response code="204">No facilities of the specified type found</response>
            /// <returns></returns>
            [HttpGet]
            [Route("{facilitytype}")]

            public async Task<IActionResult> GetFacilitiesByType(string facilityType)
            {
                var facilityList = await _facilityService.GetFacilityByTypeAsync(facilityType);
                if (facilityList.Count == 0)
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                return Ok(facilityList);
            }

            /// <summary>
            /// Files a new facility with the system
            /// </summary>
            /// <remarks>
            /// Sample request:
            ///     POST /api/v1/facility/file
            ///     {
            ///         "facilityname": "s",
            ///         "facilitytype": "s",
            ///         "address":  "Haight Ashbury"
            ///         "state":    "CA",
            ///         "zipcode":  94510,
            ///         "latitude": 37.7602,
            ///         "longitude":    -122.5094
            ///     }
            /// </remarks>
            /// <param name="facility"></param>
            /// <response code="400">There is a problem with the facility data received by this system</response>
            /// <response code="409">The facility already exists in the system</response>
            /// <response code="500">The facility is valid but this system cannot process it</response>
            /// <returns></returns>

            [HttpPost]
            [Route("add")]
            public async Task<IActionResult> AddFacility(Facility facility)
            {
                var transactionResult = await _facilityService.AddFacitity(facility);
                switch (transactionResult)
                {
                    case TransactionResult.Success:
                        return Ok();
                    case TransactionResult.Conflict:
                        return StatusCode(StatusCodes.Status409Conflict);
                    case TransactionResult.BadRequest:
                        return StatusCode(StatusCodes.Status400BadRequest);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

        /// <summary>
        /// Updates the facility with entered information
        /// </summary>
        /// <param name="facilityName"></param>
        /// <param name="facility"></param>
        /// <response code="400">There is a problem with the facility data received by this system</response>
        /// <response code="404">The facility does not exist</response>
        /// <response code="500">The facility is valid but this system cannot process it</response>
        /// <returns></returns>
        [HttpPut]
            [Route("{facilityname}")]
            public async Task<IActionResult> UpdateFacility(string facilityName, Facility facility)
            {
                var transactionResult = await _facilityService.UpdateFacility(facilityName, facility);
                switch (transactionResult)
                {
                    case TransactionResult.Success:
                        return Ok();
                    case TransactionResult.BadRequest:
                        return StatusCode(StatusCodes.Status400BadRequest);
                case TransactionResult.NotFound:
                    return StatusCode(StatusCodes.Status404NotFound);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            /// <summary>
            /// Deletes the facility by specified Name
            /// </summary>
            /// <param name="facilityName"></param>
            /// <response code="404">The facility was not found in the system</response>
            /// <returns></returns>
            [HttpDelete]
            [Route("{facilityname}")]
            public async Task<IActionResult> DeleteFacilityByName(string facilityName)
            {
                var result = await _facilityService.DeleteFacilityByName(facilityName);

                if (result)
                {
                    return Ok();
                }
                return StatusCode(StatusCodes.Status404NotFound);
        }
        }
    }