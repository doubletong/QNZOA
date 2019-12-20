using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIGOA.Data;
using SIGOA.Infrastructure;

namespace SIGOA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {

        // appId
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly SigbugsdbContext _context;
        public CommonController(IWebHostEnvironment hostingEnvironment, SigbugsdbContext context, IMapper iMapper)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _mapper = iMapper;

        }

        //[HttpPost("[action]")]
        //public async Task<IActionResult> Upload(IFormFile logo)
        //{
        //    //try
        //    //{
        //    var wwwroot = _hostingEnvironment.WebRootPath?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        //    //if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
        //    //{
        //    //    _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        //    //}

        //    var uploads = Path.Combine(wwwroot, "uploads");
        //    var filename = logo.FileName;
        //         var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create);
        //         await logo.CopyToAsync(fileStream);                 
                   
        //         return Ok(new { name= filename, length= logo.Length});
        //    //}
        //    //catch
        //    //{
        //    //    return BadRequest();
        //    //}
     
         
        //}

        //[HttpPost("Photos")]
        //[Consumes("application/json", "multipart/form-data")]//此处为新增
        //public async Task<IActionResult> UploadPhotosAsync([FromForm]IFormFileCollection files)
        //{
        //    var wwwroot = _hostingEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        //    long size = files.Sum(f => f.Length);
        //    var fileFolder = Path.Combine(wwwroot, "uploads");

        //    if (!Directory.Exists(fileFolder))
        //        Directory.CreateDirectory(fileFolder);

        //    foreach (var file in files)
        //    {
        //        if (file.Length > 0)
        //        {
        //            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") +
        //                           Path.GetExtension(file.FileName);
        //            var filePath = Path.Combine(fileFolder, fileName);

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await file.CopyToAsync(stream);
        //            }
        //        }
        //    }

        //    return Ok(new { count = files.Count, size });
        //}



        /// <summary>
        /// 上传图片[单图上传与Tinymce图片上传共用]
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]"), DisableRequestSizeLimit]

        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "/Uploads/Images";
                string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                string newPath = Path.Combine(webRootPath, "Images");

                if (!ImageHandler.CheckImageType(file.OpenReadStream()))
                {
                    return BadRequest("文件格式不正确");
                }

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ImageHandler.GetRandomFileName(Path.GetExtension(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"')));
                    string fullPath = Path.Combine(newPath, fileName);
                  
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {

                        await file.CopyToAsync(stream);
                    }
                    return Ok($"{folderName}/{fileName}");
                }
                return BadRequest("上传失败");
            }
            catch (Exception ex)
            {
                return BadRequest("上传失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 上传图片[Ckeditor图片上传]
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]"), DisableRequestSizeLimit]

        public async Task<IActionResult> CkEditorUploadImage()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "/Uploads/Images";
                string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                string newPath = Path.Combine(webRootPath, "Images");

                if (!ImageHandler.CheckImageType(file.OpenReadStream()))
                {
                    return BadRequest("文件格式不正确");
                }

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ImageHandler.GetRandomFileName(Path.GetExtension(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"')));
                    string fullPath = Path.Combine(newPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {

                        await file.CopyToAsync(stream);
                    }
                    return Ok(new { uploaded=true, url = $"{folderName}/{fileName}" });
                }
                return BadRequest("上传失败");
            }
            catch (Exception ex)
            {
                return BadRequest("上传失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="imageUrl">图片地址</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult RemoveImage([FromBody] string imageUrl)
        {
            try
            {

                var filePath = _hostingEnvironment.WebRootPath + imageUrl.Replace("/", "\\");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                return Ok("已删除图片");
            }
            catch (Exception ex)
            {
                return BadRequest("删除失败：" + ex.Message);
            }
        }

        //[HttpPost("UploadImages")]
        ////[Authorize]
        //public async Task<IActionResult> UploadImages([FromBody] ICollection<IFormFile> files)
        //{
        //    long size = files.Sum(f => f.Length);

        //    // full path to file in temp location
        //    var filePath = PlatformServices.Default.MapPath("/Uploads/Images");
        //    if (!Directory.Exists(filePath))
        //    {
        //        Directory.CreateDirectory(filePath);
        //    }

        //    foreach (var formFile in files)
        //    {
        //        if (formFile.Length > 0)
        //        {
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await formFile.CopyToAsync(stream);
        //            }
        //        }
        //    }

        //    // process uploaded files
        //    // Don't rely on or trust the FileName property without validation.

        //    return Ok(new { count = files.Count, size, filePath });
        //}

    }
}