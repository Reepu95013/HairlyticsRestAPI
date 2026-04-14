using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ServiceInterfaces
{
    public interface IFileService
    {
        Task<string> SaveImage(IFormFile file, string folderName);
        string GetCategoryImage(string fileName);

        string GetImage(string fileName);
    }
}
