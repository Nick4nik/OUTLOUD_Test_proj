namespace OUTLOUD_Test_proj.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]", Name = "Login Controller")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepo;

        public LoginController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("Login")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> Login([FromBody] RequestLogin request)
        {
            var token = await _userRepo.Login(request.Login, request.Password);
            if (token is null)
                return BadRequest();

            return Ok('2');
        }
    }
}