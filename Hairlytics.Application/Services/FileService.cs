using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hairlytics.Application.Services
{
    public class FileService : IFileService
    {
        private readonly string _basePath;
        private readonly IConfiguration _config;

        public FileService(IConfiguration config)
        {
            _basePath = config["FileStorage:BasePath"]
                ?? throw new ArgumentNullException("FileStorage:BasePath is missing in configuration");
            _config = config;
        }

        public bool DeleteFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path is required");
            }

            try
            {
                var fullPath = Path.Combine(_basePath, filePath.TrimStart('/'));

                if (!System.IO.File.Exists(fullPath))
                {
                    return false;
                }

                System.IO.File.Delete(fullPath);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public string GetCategoryImage(string fileName)
        {
            var baseUrl = _config["FileStorage:BasePath"];
            return $"{baseUrl}{fileName}";
        }


        public string GetImage(string fileName)
        {
            var baseUrl = _config["FileStorage:BasePath"];
            return $"{baseUrl}{fileName}";
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentNullException("Please choose a file!");

            folderName = folderName.ToLower();

            var folderPath = Path.Combine(_basePath, "documents", folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/documents/{folderName}/{fileName}";
        }

        public async Task<string> SaveImage(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                 throw new ArgumentNullException("Please choose a file!");

            folderName = folderName.ToLower();

            var folderPath = Path.Combine(_basePath, "images", folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/images/{folderName}/{fileName}";
        }
    }
}
