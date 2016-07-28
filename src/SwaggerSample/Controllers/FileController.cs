using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SwaggerSample.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        /// <summary>
        /// Action to upload file
        /// </summary>
        /// <param name="file"></param>
        [HttpPost]
        [Route("upload")]
        public void PostFile(IFormFile file)
        {
            var stream = file.OpenReadStream();
            var name = Path.GetFileName(file.FileName);

            //TODO: Save file
        }
    }
}
