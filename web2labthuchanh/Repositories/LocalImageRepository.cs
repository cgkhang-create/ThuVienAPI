using Microsoft.AspNetCore.Http;
using web2labthuchanh.Data;
using web2labthuchanh.Models.Domain;

namespace web2labthuchanh.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;

        public LocalImageRepository(
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        // =========================
        // UPLOAD IMAGE
        // =========================
        public Image Upload(Image image)
        {
            var imagesFolder = Path.Combine(
                _webHostEnvironment.ContentRootPath,
                "Images");

            // Nếu thư mục chưa tồn tại thì tạo
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            var fileName = image.FileName + image.FileExtension;

            var localFilePath = Path.Combine(
                imagesFolder,
                fileName);

            // Lưu file vào thư mục Images
            using var stream = new FileStream(
                localFilePath,
                FileMode.Create);

            image.File?.CopyTo(stream);

            // Tạo URL
            var httpRequest = _httpContextAccessor.HttpContext!.Request;

            var urlFilePath =
                $"{httpRequest.Scheme}://{httpRequest.Host}" +
                $"{httpRequest.PathBase}/Images/{fileName}";

            image.FilePath = urlFilePath;

            // Lưu thông tin vào database
            _dbContext.Images.Add(image);
            _dbContext.SaveChanges();

            return image;
        }

        // =========================
        // GET ALL IMAGES
        // =========================
        public List<Image> GetAllInfoImages()
        {
            var allImages = _dbContext.Images.ToList();

            return allImages;
        }

        // =========================
        // DOWNLOAD IMAGE
        // =========================
        public (byte[], string, string) DownloadFile(int Id)
        {
            try
            {
                var fileById = _dbContext.Images
                    .Where(x => x.Id == Id)
                    .FirstOrDefault();

                if (fileById == null)
                {
                    throw new Exception("Image not found");
                }

                var path = Path.Combine(
                    _webHostEnvironment.ContentRootPath,
                    "Images",
                    fileById.FileName + fileById.FileExtension);

                var bytes = File.ReadAllBytes(path);

                var fileName =
                    fileById.FileName +
                    fileById.FileExtension;

                return (
                    bytes,
                    "application/octet-stream",
                    fileName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}