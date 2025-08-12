using Microsoft.AspNetCore.Mvc;

namespace ContactService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
    }
}
