using Image_manupulation.Data.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace Image_manupulation.Data.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbcontext context;

        public FileService(IWebHostEnvironment _environment, ApplicationDbcontext applicationdbcontext)
        {
            this.environment = _environment;
            this.context= applicationdbcontext;
        }
        
        public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions)
        {
            if (imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile));
            }
     

            var wwwRootPath = environment.WebRootPath;
            var path = Path.Combine(wwwRootPath, "Uploads");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }




            var ext = Path.GetExtension(imageFile.FileName);
            if (!allowedFileExtensions.Contains(ext))
            {
                throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
            }
            //2793
        
            var fileName = $"{DateTime.Now:dd-MM-yyy_}_{Guid.NewGuid()}{ext}";
            var fileNameWithPath = Path.Combine(path, fileName);
            using var stream = new FileStream(fileNameWithPath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return fileName;
        }

       
        public void DeleteFile(string fileNameWithExtension)
        {
            if (string.IsNullOrWhiteSpace(fileNameWithExtension))
            {
                throw new ArgumentException("Filename cannot be null or empty.", nameof(fileNameWithExtension));
            }
          


            var uploadsFolderPath = Path.Combine(environment.WebRootPath, "Uploads");
            var fullPath = Path.Combine(uploadsFolderPath, fileNameWithExtension);


            // Prevent directory traversal attack
            if (!fullPath.StartsWith(uploadsFolderPath, StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("Attempted to delete a file outside the allowed directory.");
            }

            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);                  
                
            }
                else
                {
                    
                    throw new FileNotFoundException($"File not found: {fileNameWithExtension}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {fileNameWithExtension}, Exception: {ex.Message}");
                throw new IOException($"Error deleting file: {fileNameWithExtension}", ex);
            }
        }

        
    }
}
