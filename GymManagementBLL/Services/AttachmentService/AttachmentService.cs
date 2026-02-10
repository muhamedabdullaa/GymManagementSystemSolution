using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.AttachmentService
{
	public class AttachmentService : IAttachmentService
	{
		public AttachmentService(IWebHostEnvironment webHost)
		{
			this.webHost = webHost;
		}
		private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
		private readonly long maxFileSize = 5 * 1024 * 1024;
		private readonly IWebHostEnvironment webHost;

		public string? Upload(string folderName, IFormFile file)
		{
			try
			{
				if (folderName is null || file is null || file.Length == 0) return null;
				if (file.Length > maxFileSize) return null;
				var extension = Path.GetExtension(file.FileName).ToLower();
				if (!AllowedExtensions.Contains(extension)) return null;
				var folderPath = Path.Combine(webHost.WebRootPath, "images", folderName);
				if (!Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}
				var fileName = Guid.NewGuid().ToString() + extension;
				var filePath = Path.Combine(folderPath, fileName);
				using var filestream = new FileStream(filePath, FileMode.Create);
				file.CopyTo(filestream);
				return fileName;

			}
			catch (Exception ex)
			{

				Console.WriteLine($"Failed To Upload File To Folder {folderName} : {ex}");
				return null;
			}
		
		}
		public bool Delete(string fileName, string folderName)
		{
			try
			{
				if(String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(folderName))
					return false;
				var fullPath = Path.Combine(webHost.WebRootPath, "images", folderName, fileName);
				if (File.Exists(fullPath))
				{
					File.Delete(fullPath);
					return true;
						
				}
				return false;

			}
			catch (Exception ex)
			{

				Console.WriteLine($"Failed To Delete File With Name {fileName} : {ex}");
				return false;
			}
		}
	}
}
