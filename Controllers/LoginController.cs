namespace OUTLOUD_Test_proj.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]", Name = "Login Controller")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepo;

        public LoginController(IUserRepository userRepo, ApplicationContext qwe)
        {
            _userRepo = userRepo;
        }

        [HttpPost("Login")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> Login([FromBody] RequestLogin request)
        {
            var token = await _userRepo.Login(request.Login, request.Password);
            if (token is null)
                return BadRequest();

            return Ok(token);
            return Ok('1');
        }
    }
}

