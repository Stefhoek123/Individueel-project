using DAL.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamilyController : ControllerBase
    {
        // Dependency Injection of family container
        private readonly IFamilyContainer _familyContainer;
        public FamilyController(IFamilyContainer familyContainer)
        {
            _familyContainer = familyContainer;
        }

        [HttpGet(nameof(GetAllFamilies))]
        public ActionResult<IEnumerable<FamilyDto>> GetAllFamilies()
        {
            return Ok(_familyContainer.GetAllFamilies());
        }

        [HttpGet(nameof(GetFamilyById))]
        public ActionResult<FamilyDto> GetFamilyById(Guid id)
        {
            FamilyDto family = _familyContainer.GetFamilyById(id);
            return Ok(family);
        }

        [HttpGet(nameof(SearchFamilyByName))]
        public ActionResult<IEnumerable<FamilyDto>> SearchFamilyByName(string? search)
        {
            if (search != null)
            {
                return Ok(_familyContainer.SearchFamilyByName(search));
            }

            return Ok(_familyContainer.GetAllFamilies());
        }

        [HttpPost(nameof(CreateFamily))]
        public ActionResult CreateFamily(FamilyDto family)
        {
            _familyContainer.CreateFamily(family);
            return Ok();
        }

        [HttpPut(nameof(UpdateFamily))]
        public ActionResult UpdateFamily(FamilyDto family)
        {
            _familyContainer.UpdateFamily(family);
            return Ok();
        }

        [HttpDelete(nameof(DeleteFamilyById))]
        public ActionResult DeleteFamilyById(Guid id)
        {
            _familyContainer.DeleteFamilyById(id);
            return Ok();
        }
    }
}
