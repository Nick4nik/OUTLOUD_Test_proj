namespace OUTLOUD_Test_proj.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]", Name = "RSS Controller")]
    public class RSSFeedController : Controller
    {
        private readonly IRssRepository _rssRepo;
        private readonly IUserRepository _userRepo;
        private readonly XmlWriterSettings _setting;
        private const string contentType = "application/rss+xml;charset=utf-8";

        private SyndicationFeed Feed { get; set; }

        public RSSFeedController(IRssRepository rssRepo, IUserRepository userRepo)
        {
            _rssRepo = rssRepo;
            _userRepo = userRepo;
            _setting = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = true,
                Indent = true
            };
            Feed = new SyndicationFeed("Title", "Description", new Uri("https://google.com"), "RSSUrl", DateTime.Now)
            {
                Copyright = new TextSyndicationContent($"{DateTime.Now.Year} Mykyta Rusyn")
            };
        }

        [HttpPost("GetRssFeed")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> GetRssFeed([FromBody] BaseRequest request)
        {
            var user = await CheckUser(request);
            if (user is null)
                return Unauthorized();

            //Feed.Items = await _rssRepo.GetAllRss(user);
            return GetResult();
        }

        [HttpPost("AddRss")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> AddRss([FromBody] RequestAddRss request)
        {
            var user = await CheckUser(request);
            if (user is null)
                return Unauthorized();

            if (await _rssRepo.AddRss(user, request.Link))
                return BadRequest();

            return Ok();
        }

        [HttpPost("GetUnreadRss")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> GetUnreadRss([FromBody] RequestUnreadRss request)
        {
            var user = await CheckUser(request);
            if (user is null)
                return Unauthorized();

            //Feed.Items = await _rssRepo.GetUnreadRss(user);
            return GetResult();
        }

        [HttpPut("SetRssAsRead")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> SetRssAsRead([FromBody] BaseRequest request)
        {
            var user = await CheckUser(request);
            if (user is null)
                return Unauthorized();

            await _rssRepo.SetRssAsRead(user);
            return Ok();
        }

        [NonAction]
        private ActionResult GetResult()
        {
            using var stream = new MemoryStream();
            using var xmlWriter = XmlWriter.Create(stream, _setting);
            var rssFormator = new Rss20FeedFormatter(Feed, false);
            rssFormator.WriteTo(xmlWriter);
            xmlWriter.Flush();
            return File(stream.ToArray(), contentType);
        }

        [NonAction]
        private async Task<User?> CheckUser(BaseRequest request)
        {
            return await _userRepo.IsValidAuth(request.Token);
        }
    }
}
