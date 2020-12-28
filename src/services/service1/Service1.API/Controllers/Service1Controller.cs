using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Service1.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class Service1Controller : ControllerBase
    {
       

        private readonly ILogger<Service1Controller> _logger;
        private IConfiguration _config;
        public Service1Controller(ILogger<Service1Controller> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(DemoDTO1), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Get()
        {
            try
            {
                DemoDTO1 dto;

                using (var httpClient = new HttpClient())
                {
                    //StringContent content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.GetAsync(_config.GetValue<string>("Service2Url")))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        dto = JsonConvert.DeserializeObject<DemoDTO1>(apiResponse);
                    }
                }
                if(dto == null)
                {
                    return NotFound();
                }

                return Ok(dto) ;
            }
            catch (Exception)
            {
                return NotFound();
                throw;
            }
        }

        [HttpPost]
        [Route("Increment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Increment([FromBody] DemoDTO1 postData)
        {
            DemoDTO1 dd;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PutAsync(_config.GetValue<string>("Service2Url")+ "/Increment", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        dd = JsonConvert.DeserializeObject<DemoDTO1>(apiResponse);
                    }
                }
                return Ok();
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
        public async Task<ActionResult> Create([FromBody] DemoDTO1 postData)
        {
            DemoDTO1 dd;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(_config.GetValue<string>("Service2Url"), content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        dd = JsonConvert.DeserializeObject<DemoDTO1>(apiResponse);
                    }
                }
                return Ok(dd);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
           
        }
    }
}
