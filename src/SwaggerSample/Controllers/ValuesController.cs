using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SwaggerSample.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        /// <summary>
        /// API to get all values
        /// </summary>
        /// <remarks>
        /// This API will get all values
        /// </remarks>
        /// <returns>All values</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// API that takes Enum
        /// </summary>
        /// <param name="id"></param>
        /// <param name="subscriptionStatus"></param>
        /// <returns></returns>
        [HttpGet("{subscriptionStatus}")]
        public string Get(SubscriptionStatus subscriptionStatus)
        {
            return $"{subscriptionStatus} as enum";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public enum SubscriptionStatus
    {
        Free = 0,
        Paid = 1
    }
}
