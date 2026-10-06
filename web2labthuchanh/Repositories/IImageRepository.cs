using web2labthuchanh.Models.Domain;

namespace web2labthuchanh.Repositories
{
    public interface IImageRepository
    {
        Image Upload(Image image);

        List<Image> GetAllInfoImages();

        (byte[], string, string) DownloadFile(int Id);
    }
}