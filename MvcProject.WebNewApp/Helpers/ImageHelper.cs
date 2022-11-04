namespace MvcProject.WebNewApp.Helpers
{
    public static class ImageHelper
    {
        private const string Folder = "Pictures";

        public static string SavePicture(this IFormFile picture, string root)
        {
            var fileName = GenerateFileName();

            var localPath = Path.Combine(Folder, fileName);

            var path = Path.Combine(root, localPath);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                picture.CopyTo(stream);
            }

            return localPath;
        }

        private static string GenerateFileName()
        {
            return $"{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}_{Guid.NewGuid()}_artists.jpg";
        }
    }
}
