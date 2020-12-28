using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Service2.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class Service2Controller : ControllerBase
    {
        private readonly ILogger<Service2Controller> _logger;

        private IMemoryCache memoryCache;
        public Service2Controller(ILogger<Service2Controller> logger, IMemoryCache mcache)
        {
            _logger = logger;
            memoryCache = mcache;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DemoDTO1), (int)HttpStatusCode.OK)]
        public ActionResult Get()
        {
            try
            {
                DemoDTO1 dto;

                if (!memoryCache.TryGetValue<DemoDTO1>("data", out dto))
                {
                    dto = new DemoDTO1 { TotalSum = 1 };
                    memoryCache.Set<DemoDTO1>("data", dto, TimeSpan.FromDays(1));
                }

                if (dto == null)
                {
                    return NotFound();
                }

                return Ok(dto);
            }
            catch (Exception)
            {
                return NotFound();
                throw;
            }
        }

        [HttpPut]
        [Route("Increment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult Increment()
        {
            DemoDTO1 dd;
            try
            {
                dd = memoryCache.Get<DemoDTO1>("data");
                dd.TotalSum = dd.TotalSum + 1;
                memoryCache.Set<DemoDTO1>("data", dd, TimeSpan.FromDays(1));
                return Ok(dd);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }

        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult Create([FromBody] DemoDTO1 postData)
        {
            try
            {
                if (postData != null)
                {
                    memoryCache.Set<DemoDTO1>("data", postData, TimeSpan.FromDays(1));
                    return Ok(postData);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }

        }
    }
}
