using System.Drawing;
using System.IO;

namespace MiSharp.Core
{
    public class ImageHelper
    {
        public static byte[] CreateThumbnail(byte[] passedImage, int largestSide)
        {
            byte[] returnedThumbnail;

            using (MemoryStream startMemoryStream = new MemoryStream(),
                                newMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                startMemoryStream.Write(passedImage, 0, passedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                var startBitmap = new Bitmap(startMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double hwRatio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = largestSide;
                    hwRatio = (double)((double)largestSide / (double)startBitmap.Height);
                    newWidth = (int)(hwRatio * (double)startBitmap.Width);
                }
                else
                {
                    newWidth = largestSide;
                    hwRatio = (double)((double)largestSide / (double)startBitmap.Width);
                    newHeight = (int)(hwRatio * (double)startBitmap.Height);
                }

                // create a new Bitmap with dimensions for the thumbnail.  
                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                Bitmap newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(newMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                returnedThumbnail = newMemoryStream.ToArray();
            }

            // return the resized image as a string of bytes.  
            return returnedThumbnail;
        }

        // Resize a Bitmap  
        private static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            var resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        } 
    }
}