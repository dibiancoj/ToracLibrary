using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ToracLibrary.Core.ToracAttributes;

//import PresentationCore & WindowsBase assembly which contains the System.WIndows.Media.Imaging API

namespace ToracLibrary.Graphics
{

    /// <summary>
    /// Combine Multiple Tiff's Into 1 Tiff File
    /// </summary>
    public static class TiffCombiner
    {

        #region WPF Method

        /// <summary>
        /// Combine Multiple Tiff's Into One File
        /// </summary>
        /// <param name="DestinationPath">Destination Path To Save The 1 File To Output</param>
        /// <param name="FilePathToMerge">The Files To Merge</param>
        /// <param name="DestinationPathMode">Do you want to append the current file, overwrite it, etc.</param>
        /// <remarks>Uses WPF - Needs References To Windows Base and Presentation Core (WPF) - Will only work with 1 page tiffs (will grab only the first page of each tiff)</remarks>
        [MethodIsNotTestable("I guess you could add an image and test it. This api isn't used much. Will just port it and not add a unit test")]
        public static void CombineTiffs(string DestinationPath, IEnumerable<string> FilePathToMerge, FileMode DestinationPathMode)
        {
            //create the file stream to write to the file
            using (FileStream FileStreamToUse = new FileStream(DestinationPath, DestinationPathMode))
            {
                //create the tiff encoder
                var Encoder = new TiffBitmapEncoder();

                //set the compression
                Encoder.Compression = TiffCompressOption.Ccitt4;

                //loop through each of the files to combine
                foreach (string FileNameToMerge in FilePathToMerge)
                {
                    try
                    {
                        //declare the bitmap image
                        var ImageToMerge = new BitmapImage();

                        //start the process
                        ImageToMerge.BeginInit();

                        //do everything on load
                        ImageToMerge.CacheOption = BitmapCacheOption.OnLoad;

                        //set the file source
                        ImageToMerge.UriSource = new Uri(FileNameToMerge);

                        //we are done and ready to convert
                        ImageToMerge.EndInit();

                        //convert it now
                        FormatConvertedBitmap ConvertedBitMap = new FormatConvertedBitmap(ImageToMerge,
                                                                                          PixelFormats.BlackWhite,
                                                                                          BitmapPalettes.BlackAndWhite,
                                                                                          1.0);

                        //create the frames
                        Encoder.Frames.Add(BitmapFrame.Create(ConvertedBitMap));
                    }

                    catch (NotSupportedException)
                    {
                        //ignore these exception? (means bad image file...or is not a tiff file)
                        throw;
                    }
                }

                //save the file
                Encoder.Save(FileStreamToUse);
            }
        }

        #endregion

        #region GDI+ Method

        #region Public Methods

        /// <summary>
        /// This function will join the TIFF file with a specific compression format. Uses GDI. Saves it to a byte array. Please note that tiffs file size are pretty large. So saving like a 50 kb file will end up saving as like 300 kb for a tiff. Keep this mind.
        /// </summary>
        /// <param name="ImageFilesToJoin">All the files to join together where the byte array is each file</param>
        /// <param name="CompressEncoder">compression codec enum. EncoderValue.CompressionLZW is probably the best one to use for full color.</param>
        /// <remarks>Uses GDI --> Will grab all pages of the tiff file</remarks>
        [MethodIsNotTestable("I guess you could add an image and test it. This api isn't used much. Will just port it and not add a unit test")]
        public static byte[] JoinTiffImages(IEnumerable<byte[]> ImageFilesToJoin, EncoderValue CompressEncoder)
        {
            //create a memory stream to save
            using (var MemoryStreamToSave = new MemoryStream())
            {
                //if we only have 1 image then return that
                if (ImageFilesToJoin.Count() == 1)
                {
                    //go read the file and just return it
                    return ImageFilesToJoin.First();
                }

                //Encoder Parameter Array
                var EncodedParameters = new EncoderParameters(2);

                //use the save encoder
                var EncoderToUse = System.Drawing.Imaging.Encoder.SaveFlag;

                //Add the parameters to the array
                EncodedParameters.Param[0] = new EncoderParameter(EncoderToUse, (long)EncoderValue.MultiFrame);
                EncodedParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)CompressEncoder);

                //Set the Encoder Info
                var EncoderInfo = GetEncoderInfo("image/tiff");

                //go grab the first image and load it.
                using (var BaseImage = new MemoryStream(ImageFilesToJoin.ElementAt(0)))
                {
                    //create the image we will keep adding too as we add the rest of the images
                    using (var ImageToSave = (Bitmap)Image.FromStream(BaseImage))
                    {
                        //go save the first page of the image
                        ImageToSave.Save(MemoryStreamToSave, EncoderInfo, EncodedParameters);

                        //cache the frame count
                        int FirstPageFrameCount = GetImageFrameCount(ImageToSave);

                        //Set the parameter to be a next cont. page
                        EncodedParameters.Param[0] = new EncoderParameter(EncoderToUse, (long)EncoderValue.FrameDimensionPage);

                        //loop through the frames for this image
                        for (int i = 1; i < FirstPageFrameCount; i++)
                        {
                            //use the helper to add this specific frame to the image.
                            AddFrameToImage(ImageToSave, ImageToSave, i, EncodedParameters);
                        }

                        //now let's take care of the rest of the images now that we have the "base" image setup (skipping the first image because we took care of that already)
                        foreach (var ImageToAdd in ImageFilesToJoin.Skip(1))
                        {
                            //Grab it using the memory stream...we can dispose of this once we are done with this image
                            using (var MemoryStreamToUse = new MemoryStream(ImageToAdd))
                            {
                                //grab the bitmap image from the memory stream
                                using (var ImageToAddToTheBaseImage = (Bitmap)Image.FromStream(MemoryStreamToUse))
                                {
                                    //cache the frame count
                                    int FrameCount = GetImageFrameCount(ImageToAddToTheBaseImage);

                                    //now we need to loop though all the pages to add each page to the tiff we are using as the base tiff
                                    for (int i = 0; i < FrameCount; i++)
                                    {
                                        //use the helper to add this specific frame to the image.
                                        AddFrameToImage(ImageToSave, ImageToAddToTheBaseImage, i, EncodedParameters);
                                    }
                                }
                            }
                        }

                        //now that we are all done...we need to flush the image
                        EncodedParameters.Param[0] = new EncoderParameter(EncoderToUse, (long)EncoderValue.Flush);

                        //save the flush command
                        ImageToSave.SaveAdd(EncodedParameters);

                        //close the memory stream
                        MemoryStreamToSave.Flush();

                        //close the memory stream
                        MemoryStreamToSave.Close();

                        //return the byte array now
                        return MemoryStreamToSave.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// This function will join the TIFF file with a specific compression format. Uses GDI. Saves it to a byte array. Please note that tiffs file size are pretty large. So saving like a 50 kb file will end up saving as like 300 kb for a tiff. Keep this mind.
        /// </summary>
        /// <param name="ImageFilesToJoin">string array with source image files</param>
        /// <param name="CompressEncoder">compression codec enum. EncoderValue.CompressionLZW is probably the best one to use for full color.</param>
        /// <remarks>Uses GDI --> Will grab all pages of the tiff file</remarks>
        [MethodIsNotTestable("I guess you could add an image and test it. This api isn't used much. Will just port it and not add a unit test")]
        public static byte[] JoinTiffImages(IEnumerable<string> ImageFilesToJoin, EncoderValue CompressEncoder)
        {
            //holds each of the file data in a byte array
            var FileBytes = new List<byte[]>();

            //we want to pass in each file and it's byte array so we will load each file and pass it into the overload
            foreach (var FileToLoad in ImageFilesToJoin)
            {
                //add the file to load
                FileBytes.Add(File.ReadAllBytes(FileToLoad));
            }

            //now go use the overload
            return JoinTiffImages(FileBytes, CompressEncoder);
        }

        private static Stream PathToStream(string path)
        {
            var dest = new MemoryStream();

            using (Stream source = File.OpenRead(path))
            {
                byte[] buffer = new byte[2048];
                int bytesRead;
                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    dest.Write(buffer, 0, bytesRead);
                }
            }

            return dest;
        }

        public static byte[] JasonTest(IEnumerable<string> ImageFilesToJoin, EncoderValue CompressEncoder)
        {
            //create a memory stream to save
            using (var MemoryStreamToSave = new MemoryStream())
            {
                //if we only have 1 image then return that
                if (ImageFilesToJoin.Count() == 1)
                {
                    //go read the file and just return it
                    return File.ReadAllBytes(ImageFilesToJoin.First());
                }

                //Encoder Parameter Array
                var EncodedParameters = new EncoderParameters(2);

                //use the save encoder
                var EncoderToUse = System.Drawing.Imaging.Encoder.SaveFlag;

                //Add the parameters to the array
                EncodedParameters.Param[0] = new EncoderParameter(EncoderToUse, (long)EncoderValue.MultiFrame);
                EncodedParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)CompressEncoder);

                //Set the Encoder Info
                var EncoderInfo = GetEncoderInfo("image/tiff");

                //go grab the first image and load it.
                using (var BaseImage = PathToStream(ImageFilesToJoin.ElementAt(0)))
                {
                    //create the image we will keep adding too as we add the rest of the images
                    using (var ImageToSave = (Bitmap)Image.FromStream(BaseImage))
                    {
                        //go save the first page of the image
                        ImageToSave.Save(MemoryStreamToSave, EncoderInfo, EncodedParameters);

                        //cache the frame count
                        int FirstPageFrameCount = GetImageFrameCount(ImageToSave);

                        //Set the parameter to be a next cont. page
                        EncodedParameters.Param[0] = new EncoderParameter(EncoderToUse, (long)EncoderValue.FrameDimensionPage);

                        //loop through the frames for this image
                        for (int i = 1; i < FirstPageFrameCount; i++)
                        {
                            //use the helper to add this specific frame to the image.
                            AddFrameToImage(ImageToSave, ImageToSave, i, EncodedParameters);
                        }

                        //now let's take care of the rest of the images now that we have the "base" image setup (skipping the first image because we took care of that already)
                        foreach (var ImageToAdd in ImageFilesToJoin.Skip(1))
                        {
                            //Grab it using the memory stream...we can dispose of this once we are done with this image
                            using (var MemoryStreamToUse = PathToStream(ImageToAdd))
                            {
                                //grab the bitmap image from the memory stream
                                using (var ImageToAddToTheBaseImage = (Bitmap)Image.FromStream(MemoryStreamToUse))
                                {
                                    //cache the frame count
                                    int FrameCount = GetImageFrameCount(ImageToAddToTheBaseImage);

                                    //now we need to loop though all the pages to add each page to the tiff we are using as the base tiff
                                    for (int i = 0; i < FrameCount; i++)
                                    {
                                        //use the helper to add this specific frame to the image.
                                        AddFrameToImage(ImageToSave, ImageToAddToTheBaseImage, i, EncodedParameters);
                                    }
                                }
                            }
                        }

                        //now that we are all done...we need to flush the image
                        EncodedParameters.Param[0] = new EncoderParameter(EncoderToUse, (long)EncoderValue.Flush);

                        //save the flush command
                        ImageToSave.SaveAdd(EncodedParameters);

                        //close the memory stream
                        MemoryStreamToSave.Flush();

                        //close the memory stream
                        MemoryStreamToSave.Close();

                        //return the byte array now
                        return MemoryStreamToSave.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// This function will join the TIFF file with a specific compression format. Uses GDI
        /// </summary>
        /// <param name="ImageFilesToJoin">string array with source image files</param>
        /// <param name="DestinationPath">target TIFF file to be produced</param>
        /// <param name="CompressEncoder">compression codec enum</param>
        /// <remarks>Uses GDI --> Will grab all pages of the tiff file</remarks>
        [MethodIsNotTestable("I guess you could add an image and test it. This api isn't used much. Will just port it and not add a unit test")]
        public static void JoinTiffImages(string DestinationPath, IEnumerable<string> ImageFilesToJoin, EncoderValue CompressEncoder)
        {
            //use the overload and write all bytes
            File.WriteAllBytes(DestinationPath, JoinTiffImages(ImageFilesToJoin, CompressEncoder));
        }

        /// <summary>
        /// This function will join the TIFF file with a specific compression format. Uses GDI
        /// </summary>
        /// <param name="ImageFilesToJoin">Images to join where the byte array is each file in bytes</param>
        /// <param name="DestinationPath">target TIFF file to be produced</param>
        /// <param name="CompressEncoder">compression codec enum</param>
        /// <remarks>Uses GDI --> Will grab all pages of the tiff file</remarks>
        [MethodIsNotTestable("I guess you could add an image and test it. This api isn't used much. Will just port it and not add a unit test")]
        public static void JoinTiffImages(string DestinationPath, IEnumerable<byte[]> ImageFilesToJoin, EncoderValue CompressEncoder)
        {
            //use the overload and write all bytes
            File.WriteAllBytes(DestinationPath, JoinTiffImages(ImageFilesToJoin, CompressEncoder));
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Get the frame count for an image
        /// </summary>
        /// <param name="ImageToRetrieveFrameCountFrom">image to get the frame count for</param>
        /// <returns>Total number of frames</returns>
        [MethodIsNotTestable("Haven't build unit tests for graphics yet")]
        private static int GetImageFrameCount(Bitmap ImageToRetrieveFrameCountFrom)
        {
            return ImageToRetrieveFrameCountFrom.GetFrameCount(FrameDimension.Page);
        }

        /// <summary>
        /// Add a frame to the existing image
        /// </summary>
        /// <param name="ImageToSaveInto">Image to save into</param>
        /// <param name="ImageToAdd">Image where the frame exists that we want to add</param>
        /// <param name="FrameId">Frame id you want to add</param>
        /// <param name="EncodeParameters">Encode parameters to use to save the image</param>
        [MethodIsNotTestable("Haven't build unit tests for graphics yet")]
        private static void AddFrameToImage(Bitmap ImageToSaveInto, Bitmap ImageToAdd, int FrameId, EncoderParameters EncodeParameters)
        {
            //set the active page
            ImageToAdd.SelectActiveFrame(FrameDimension.Page, FrameId);

            //add the page to the image
            ImageToSaveInto.SaveAdd(ImageToAdd, EncodeParameters);
        }

        /// <summary>
        /// Getting the supported codec info.
        /// </summary>
        /// <param name="mimeType">description of mime type</param>
        /// <returns>image codec info</returns>
        [MethodIsNotTestable("Haven't build unit tests for graphics yet")]
        private static ImageCodecInfo GetEncoderInfo(string MimeType)
        {
            //go grab the encoder
            return ImageCodecInfo.GetImageEncoders().First(x => string.Equals(x.MimeType, MimeType, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #endregion

    }

}
