using Ghotok.Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace GhotokApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppUserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
