using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;



namespace Image_manupulation.Data.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment environment;
        public FileService(IWebHostEnvironment _environment)
        {
            this.environment = _environment;
        }


        public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions)
        {
            if (imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile));
            }


            //root file ma jai ne uplodes folder gote
            var wwwRootPath = environment.WebRootPath;
            var path = Path.Combine(wwwRootPath, "Uploads");
            // path = "c://projects/ImageManipulation.Ap/uploads" ,not exactly, but something like that


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Check the allowed extenstions
            var ext = Path.GetExtension(imageFile.FileName);
            if (!allowedFileExtensions.Contains(ext))
            {
                throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
            }

            // generate a unique filename
            var fileName = $"{DateTime.Now:dd-MM-yyy_HH-mm}_{Guid.NewGuid()}{ext}";
            var fileNameWithPath = Path.Combine(path, fileName);
            using var stream = new FileStream(fileNameWithPath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return fileName;
        }


        public void DeleteFile(string fileNameWithExtension)
        {
            if (string.IsNullOrEmpty(fileNameWithExtension))
            {
                throw new ArgumentNullException(nameof(fileNameWithExtension));
            }
            var contentPath = environment.ContentRootPath;
            var path = Path.Combine(contentPath, $"Uploads", fileNameWithExtension);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Invalid file path");
            }
            File.Delete(path);
        }
    }
}
