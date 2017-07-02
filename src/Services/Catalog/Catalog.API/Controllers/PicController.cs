﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Catalog.API.Infrastructure;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Microsoft.eShopOnContainers.Services.Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class PicController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly CatalogContext _catalogContext;

        public PicController(IHostingEnvironment env,
            CatalogContext catalogContext)
        {
            _env = env;
            _catalogContext = catalogContext;
        }

        [HttpGet("{filename}")]
        // GET: /<controller>/
        public IActionResult GetImage(string filename)
        {
            var webRoot = _env.WebRootPath;
            var path = Path.Combine(webRoot, Path.GetFileName(filename));

            string imageFileExtension = Path.GetExtension(Path.GetFileName(filename));

            if (string.IsNullOrEmpty(imageFileExtension) && System.IO.File.Exists(path + ".png"))
            {
                path += ".png";
            }

            if ( !System.IO.File.Exists(path) )
            {
                return new NotFoundResult();
            }

            imageFileExtension = Path.GetExtension(path);

            string mimetype = GetImageMimeTypeFromImageFileExtension(imageFileExtension);

            var buffer = System.IO.File.ReadAllBytes(path);

            return new FileContentResult(buffer, mimetype)
            {
                FileDownloadName = Path.GetFileName(path)
            };
        }

        private string GetImageMimeTypeFromImageFileExtension(string extension)
        {
            string mimetype;

            switch (extension)
            {
                case ".png":
                    mimetype = "image/png";
                    break;
                case ".gif":
                    mimetype = "image/gif";
                    break;
                case ".jpg":
                case ".jpeg":
                    mimetype = "image/jpeg";
                    break;
                case ".bmp":
                    mimetype = "image/bmp";
                    break;
                case ".tiff":
                    mimetype = "image/tiff";
                    break;
                case ".wmf":
                    mimetype = "image/wmf";
                    break;
                case ".jp2":
                    mimetype = "image/jp2";
                    break;
                case ".svg":
                    mimetype = "image/svg+xml";
                    break;
                default:
                    mimetype = "application/octet-stream";
                    break;
            }

            return mimetype;
        }
    }
}
