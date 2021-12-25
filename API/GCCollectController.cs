using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;
using serssly.Data;
using serssly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace serssly.API
{
    [Route("api/gccollect")]
    [AllowAnonymous]
    [ApiController]
    public class GCCollectController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public GCCollectController(ApplicationDbContext context) => this.context = context;

        public ActionResult Get(int gen = 0)
        {
            GC.Collect(gen);
            return Ok();
        }
    }
}
