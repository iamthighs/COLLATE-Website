namespace COLLATEFINAL.Helpers
{
    public class FileHelper
    {
        private readonly IWebHostEnvironment _env;

        public FileHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        public bool IsValidImage(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return ext == ".jpg" || ext == ".png";
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            var uploadsPath = Path.Combine(_env.WebRootPath, folder);
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var path = Path.Combine(uploadsPath, fileName);

            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }

        public void DeleteFile(string folder, string? fileName)
        {

            if (string.IsNullOrWhiteSpace(fileName))
                return;

            var path = Path.Combine(_env.WebRootPath, folder, fileName);
            Console.WriteLine($"Deleting file at path: {path}");
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            Console.WriteLine($"File deleted: {path}");

        }
    }
}
