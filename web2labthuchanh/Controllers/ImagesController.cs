using Microsoft.AspNetCore.Mvc;
using web2labthuchanh.Models.Domain;
using web2labthuchanh.Models.DTO;
using web2labthuchanh.Repositories;

namespace web2labthuchanh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        // =========================
        // UPLOAD
        // =========================
        [HttpPost]
        [Route("Upload")]
        public IActionResult Upload(
            [FromForm] ImageUploadRequestDTO request)
        {
            ValidateFileUpload(request);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageDomainModel = new Image
            {
                File = request.File,

                FileName = Path.GetFileNameWithoutExtension(
                    request.File!.FileName),

                FileExtension = Path.GetExtension(
                    request.File.FileName),

                FileSizeInBytes = request.File.Length,

                FileDescription = request.FileDescription
            };

            var image = _imageRepository.Upload(
                imageDomainModel);

            return Ok(image);
        }

        // =========================
        // GET ALL IMAGES
        // =========================
        [HttpGet]
        public IActionResult GetAllImages()
        {
            var allImages =
                _imageRepository.GetAllInfoImages();

            return Ok(allImages);
        }

        // =========================
        // DOWNLOAD
        // =========================
        [HttpGet]
        [Route("Download")]
        public IActionResult DownloadImage(int id)
        {
            var result =
                _imageRepository.DownloadFile(id);

            return File(
                result.Item1,
                result.Item2,
                result.Item3);
        }

        // =========================
        // VALIDATE FILE
        // =========================
        private void ValidateFileUpload(
            ImageUploadRequestDTO request)
        {
            if (request.File == null)
            {
                ModelState.AddModelError(
                    "file",
                    "Please upload a file.");

                return;
            }

            var allowExtensions =
                new string[]
                {
                    ".jpg",
                    ".jpeg",
                    ".png"
                };

            if (!allowExtensions.Contains(
                    Path.GetExtension(
                        request.File.FileName)
                    .ToLower()))
            {
                ModelState.AddModelError(
                    "file",
                    "Unsupported file extension");
            }

            if (request.File.Length > 10400000)
            {
                ModelState.AddModelError(
                    "file",
                    "File size too big, please upload file <10M");
            }
        }
    }
}