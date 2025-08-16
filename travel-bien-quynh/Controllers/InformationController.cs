using travel_bien_quynh.Entities;
using travel_bien_quynh.Repositories;
using travel_bien_quynh.Repositories.Interface;
using travel_bien_quynh.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace travel_bien_quynh.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class InformationController : ControllerBase
    {
        private readonly IInformationRepository _informationRepository;
        public InformationController(IInformationRepository infomaionRepository)
        {
            _informationRepository = infomaionRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetInformation()
        {
            var informationList = await _informationRepository.GetAsync();
            if (informationList == null)
            {
                return NotFound(new { msg = "No news found" });
            }

            var informationResponse = informationList.Select(information => new
            {
                information.Address,
                information.Logo,
                information.Email,
                information.Hotline,
                information.Copyright,
                information.Country,
                information.NameWebsite,
                information.Facebook,
                information.Instagram,
                information.TikTok,
                information.Youtube,
                information.Zalo,
            });

            return Ok(new { data = informationResponse });
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateInformation([FromBody] CreateInformationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { msg = "Invalid request data" });
            }

            try
            {
                var newInformation = new Information
                {
                    Address = request.Address,
                    Copyright = request.Copyright,
                    Hotline = request.Hotline,
                    Country = request.Country,
                    NameWebsite = request.NameWebsite,
                    Logo = request.Logo,
                    Email = request.Email,
                    Facebook = request.Facebook,
                    Instagram = request.Instagram,
                    Youtube = request.Youtube,
                    TikTok = request.TikTok,
                    Zalo = request.Zalo,
                };

                await _informationRepository.CreateAsync(newInformation);

                return Ok(new { msg = "okay created successfully" });
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, new { msg = "An error occurred while creating the Information", error = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInformation(string id, [FromBody] UpdateInformationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { msg = "Invalid request data" });
            }

            var existingInformation = await _informationRepository.GetAsync(id);
            if (existingInformation == null)
            {
                return NotFound(new { msg = "Information not found" });
            }

            try
            {
                existingInformation.Hotline = request.Hotline;
                existingInformation.Copyright = request.Copyright;
                existingInformation.Logo = request.Logo;
                existingInformation.Country = request.Country;
                existingInformation.NameWebsite = request.NameWebsite;
                existingInformation.Address = request.Address;
                existingInformation.Email = request.Email;
                existingInformation.Facebook = request.Facebook;
                existingInformation.TikTok = request.TikTok;
                existingInformation.Instagram = request.Instagram;
                existingInformation.Youtube = request.Youtobe;
                existingInformation.Zalo = request.Zalo;

                await _informationRepository.UpdateAsync(id, existingInformation);

                return Ok(new { msg = "News updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, new { msg = "An error occurred while updating the news", error = ex.Message });
            }
        }
    }
}
