using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Web;
using MvcProject.WebApp;
using Microsoft.AspNetCore.Http;

namespace MvcProject.WebApp.Helpers
{

    public enum StorageLocation
    {
        Local,
        LocalNotTracked
    }

    public class PictureBuilder
    {
        static Dictionary<StorageLocation, string> Locations;

        int quality = 70;
        ISupportedImageFormat format = new JpegFormat();
        Size size = new Size(150, 150);

        string folder = Locations[StorageLocation.Local];

        private PictureBuilder()
        {
        }

        static PictureBuilder()
        {
            Locations = new Dictionary<StorageLocation, string>();
            Locations.Add(StorageLocation.Local, ConfigurationManager.AppSettings["LocalPictureStorage"].ToString());
            Locations.Add(StorageLocation.LocalNotTracked, ConfigurationManager.AppSettings["LocalPictureStorageNotTracked"].ToString());
        }

        public static PictureBuilder Default => new PictureBuilder();

        public PictureBuilder Quality(int percentage)
        {
            this.quality = percentage;
            return this;
        }

        public PictureBuilder Resize(Size size)
        {
            this.size = size;
            return this;
        }

        public PictureBuilder Format(ISupportedImageFormat format)
        {
            this.format = format;
            return this;
        }

        public PictureBuilder Storage(StorageLocation location)
        {
            folder = Locations[location];
            return this;
        }

        public string Save(IFormFile upload)
        {
            string fileName = GenerateFileName();
            string filePath = GetLocalPath(fileName);


            byte[] buffer = new byte[upload.Length];

            upload.OpenReadStream().Read(buffer, 0, buffer.Length);

            using (var inStream = new MemoryStream(buffer))
            {            
                using (var imageFactory = new ImageFactory())
                {
                    imageFactory.Load(inStream)
                                        .Resize(size)
                                        .Format(format)
                                        .Quality(quality)
                                        .Save(filePath);
                }
            }
            return GetLocalPath(fileName);
        }

        public string GetLocalPath(string fileName)
        {
            return folder + fileName;
        }

        private static string GenerateFileName()
        {
            return $"{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}_{Guid.NewGuid()}_artists.jpg";
        }
    }
}