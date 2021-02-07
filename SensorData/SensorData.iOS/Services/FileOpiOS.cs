using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using SensorData.iOS.Services;
using SensorData.Services;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(FileOpiOS))]
namespace SensorData.iOS.Services
{
    public class FileOpiOS : IPlatformFileOp
    {
        private string _rootDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SensorDataset");

        /// <summary>
        /// Checks and creates the directory if not created
        /// </summary>
        public FileOpiOS()
        {
            if (!Directory.Exists(_rootDir))
                Directory.CreateDirectory(_rootDir);
        }

        /// <summary>
        /// Checks if the file exists
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public bool CheckFileExistence(string filepath)
        {
            return File.Exists(Path.Combine(_rootDir, filepath));
        }

        /// <summary>
        /// Delete the File.
        /// </summary>
        public void DeleteFile(string fileName)
        {
            if (CheckFileExistence(fileName))
            {
                File.Delete(Path.Combine(_rootDir, fileName));
            }
        }

        /// <summary>
        /// Gets the file data.
        /// </summary>
        /// <returns>The Credential data.</returns>
        /// <param name="filepath">File name.</param>
        public string GetData(string filepath)
        {
            try
            {
                StreamReader reader = new StreamReader(Path.Combine(_rootDir, filepath));
                string res = reader.ReadLine();
                reader.Close();
                return res;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Shows the file from local.
        /// </summary>
        /// <param name="filepath">Filepath.</param>
        public void ShowFileFromLocal(string filepath)
        {
            NSUrl url = NSUrl.FromFilename(filepath);

            var topController = GetVisibleViewController();
            while (topController.PresentedViewController != null)
            {
                topController = topController.PresentedViewController;
            }

            var viewer = UIDocumentInteractionController.FromUrl(url);
            viewer.PresentOpenInMenu(new RectangleF(0, -260, 320, 320), topController.View, true);
        }

        /// <summary>
        /// Saves the file in device
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> Save(MemoryStream fileStream, string fileName)
        {
            if (!Directory.Exists(_rootDir))
                Directory.CreateDirectory(_rootDir);

            var filePath = Path.Combine(_rootDir, fileName);

            using (var memoryStream = new MemoryStream())
            {
                await fileStream.CopyToAsync(memoryStream);
                File.WriteAllBytes(filePath, memoryStream.ToArray());
            }
            return filePath;
        }

        /// <summary>
        /// Saves the file in device
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string Save(string[] fileStream, string fileName)
        {
            if (!Directory.Exists(_rootDir))
                Directory.CreateDirectory(_rootDir);
            var filePath = Path.Combine(_rootDir, fileName);
            {
                File.WriteAllLines(filePath, fileStream);
            }
            return filePath;
        }

        //public UIKit.UIImage ImageFromByteArray(byte[] data)
        //{
        //    if (data == null)
        //        return null;

        //    return new UIKit.UIImage(Foundation.NSData.FromArray(data));
        //}

        //public byte[] ResizeImage(byte[] imageData, float width, float height)
        //{

        //    UIImage originalImage = ImageFromByteArray(imageData);

        //    var originalHeight = originalImage.Size.Height;
        //    var originalWidth = originalImage.Size.Width;


        //    nfloat newHeight = 0;
        //    nfloat newWidth = 0;

        //    UIGraphics.BeginImageContext(new SizeF(width, height));
        //    originalImage.Draw(new RectangleF(0, 0, width, height));
        //    var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
        //    UIGraphics.EndImageContext();

        //    var bytesImagen = resizedImage.AsJPEG().ToArray();
        //    resizedImage.Dispose();
        //    return bytesImagen;
        //}

        UIViewController GetVisibleViewController()
        {
            var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (rootController.PresentedViewController == null)
                return rootController;

            if (rootController.PresentedViewController is UINavigationController)
            {
                return ((UINavigationController)rootController.PresentedViewController).TopViewController;
            }

            if (rootController.PresentedViewController is UITabBarController)
            {
                return ((UITabBarController)rootController.PresentedViewController).SelectedViewController;
            }

            return rootController.PresentedViewController;
        }
    }
}
