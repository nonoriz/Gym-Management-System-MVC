using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GymManagementSystemBLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","images");

        //public AttachmentService(IWebHostEnvironment webHostEnvironment)
        //{

        //}
        private readonly string[] AllowedExtension = { ".jpg", ".jpeg", ".png" };
        private readonly long MaxFileSize = 5 * 1024 * 1024;


        public string? Upload(string FolderName, IFormFile file)
        {
            try
            {
                if (FolderName is null || file is null || file.Length == 0) return null;
                if (file.Length > MaxFileSize) return null;
                var extension = Path.GetExtension(file.FileName);
                if (!AllowedExtension.Contains(extension.ToLower())) return null;

                var folderPath = Path.Combine(rootPath, FolderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(folderPath, fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);
                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }



        }

        public bool Delete(string fileName, string FolderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(FolderName)) return false;

                var fullPath = Path.Combine(rootPath, "images",FolderName, fileName);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
