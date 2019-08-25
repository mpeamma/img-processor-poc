using System;
using System.IO;
using ImageMagick;

namespace image_processor_POC
{
    public static class Service 
    {
        public static void Process(string path)
        {
            var filename = Path.GetFileNameWithoutExtension(path);
            if (path.Contains(".gif")) 
            {
                using (MagickImage image = new MagickImage(path))
                {
                    // Save frame as jpg
                    image.Write("/Images/results/" + filename + ".jpg");
                }
            }
            else if (path.Contains(".pdf"))
            {
                MagickReadSettings settings = new MagickReadSettings();
                // Settings the density to 150 dpi will create an image with a better quality
                settings.Density = new Density(150, 150);

                using (MagickImageCollection images = new MagickImageCollection())
                {
                    // Add all the pages of the pdf file to the collection
                    images.Read(path, settings);

                    int page = 1;
                    foreach (MagickImage image in images)
                    {
                        // Write page to file that contains the page number
                        image.Write("/Images/results/" + filename + ".Page" + page + ".png");
                        // Writing to a specific format works the same as for a single image
                        image.Format = MagickFormat.Tif;
                        image.Write("/Images/results/" + filename + ".Page" + page + ".tif");    
                        page++;
                    }
                }
            }
            else
            {
                throw new NotImplementedException("File type not supported");
            }
        }
    }
}