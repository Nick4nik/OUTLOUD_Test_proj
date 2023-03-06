namespace Web_API_tests.Controllers.V2
{
    [ApiVersion("2.0"), ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v{version:apiVersion}/[controller]", Name = "RSS Controller")]
    public class RSSFeedController : Controller
    {
        private readonly IRssRepository _rssRepo;
        private readonly User? _user;
        private readonly XmlWriterSettings _setting;
        private const string contentType = "application/rss+xml;charset=utf-8";

        private SyndicationFeed Feed { get; set; }

        public RSSFeedController(IRssRepository rssRepo, IUserRepository userRepo)
        {
            _rssRepo = rssRepo;
            _user = LoginController.LoggedUser;
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

        [HttpGet("GetRssFeed")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> GetRssFeed()
        {
            var rssList = await _rssRepo.GetAllRss(_user!);

            Feed.Items = CreateFeedItems(rssList);

            return GetResult();
        }

        [HttpPost("AddRss")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> AddRss([FromBody] RequestAddRssV2 request)
        {
            if (!await _rssRepo.AddRss(_user!, request.Link))
                return BadRequest();

            return Ok();
        }

        [HttpGet("GetUnreadRss")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> GetUnreadRss()
        {
            var rssList = await _rssRepo.GetUnreadRss(_user!);

            Feed.Items = CreateFeedItems(rssList);

            return GetResult();
        }

        [HttpPost("GetUnreadRssWithDate")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> GetUnreadRssWithDate([FromBody] RequestUnreadRssV2 request)
        {
            var rssList = await _rssRepo.GetUnreadRssWithDate(_user!, request.Date.AddDays(request.AddDays));

            Feed.Items = CreateFeedItems(rssList);

            return GetResult();
        }

        [HttpPut("SetRssAsRead")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> SetRssAsRead([FromBody] RequestSetAsReadV2 request)
        {
            if (await _rssRepo.SetRssAsRead(_user!, request.Link))
                return BadRequest();

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
        private static List<SyndicationItem> CreateFeedItems(List<RSS> rssList)
        {
            List<SyndicationItem> result = new();

            foreach (var rss in rssList)
                result.Add(new(rss.Title, rss.Content, new Uri(rss.Link), rss.Id.ToString(), rss.Created));

            return result;
        }
    }
}
