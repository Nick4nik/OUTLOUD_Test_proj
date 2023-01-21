namespace OUTLOUD_Test_proj.Controllers.V2
{
    [ApiVersion("2.0"), ApiController]
    [Route("api/v{version:apiVersion}/[controller]", Name = "Login Controller")]
    public class LoginController : Controller
    {
        public static User? LoggedUser { get; set; }
        private readonly IUserRepository _userRepo;

        public LoginController(IUserRepository userRepo, ApplicationContext qwe)
        {
            _userRepo = userRepo;
        }

        [HttpPost("Login")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> Login([FromBody] RequestLogin request)
        {
            LoggedUser = await _userRepo.Login(request.Login, request.Password);
            if (LoggedUser is null)
                return Unauthorized();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("anySecretKey123qweasdzxc"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "Rusyn Mykyta",
                audience: "https://localhost:44354",
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var token = string.Concat("Bearer ", tokenString);
            return Ok(new { Token = token });
        }
    }
}
