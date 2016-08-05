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

        /// <summary>
        /// This function will join the TIFF file with a specific compression format. Uses GDI. Saves it to a byte array
        /// </summary>
        /// <param name="ImageFilesToJoin">string array with source image files</param>
        /// <param name="CompressEncoder">compression codec enum</param>
        /// <remarks>Uses GDI --> Will grab all pages of the tiff file</remarks>
        [MethodIsNotTestable("I guess you could add an image and test it. This api isn't used much. Will just port it and not add a unit test")]
        public static byte[] JoinTiffImages(IEnumerable<string> ImageFilesToJoin, EncoderValue CompressEncoder)
        {
            //create a memory stream to save
            using (var MemoryStreamToSave = new MemoryStream())
            {
                //How many pictures do we have
                int HowManyPictures;

                //Hold the main Bitmap which we add to.
                Bitmap ImageToSave = null;

                //Base Image Memory Stream (need this because we can't dispose of this until the end of the method)
                MemoryStream BaseImage = null;

                //Holds a tally of what image we are on when we loop through
                int ImageNumber = 0;

                //Encoder Parameter Array
                var EncodedParameters = new EncoderParameters(2);

                try
                {
                    //How many pictures do we have
                    HowManyPictures = ImageFilesToJoin.Count();

                    //if we only have 1 image then return that
                    if (HowManyPictures == 1)
                    {
                        //go read the file and just return it
                        return File.ReadAllBytes(ImageFilesToJoin.ElementAt(0));
                    }

                    //use the save encoder
                    var EncoderToUse = System.Drawing.Imaging.Encoder.SaveFlag;

                    //Add the parameters to the array
                    EncodedParameters.Param[0] = new EncoderParameter(EncoderToUse, (long)EncoderValue.MultiFrame);
                    EncodedParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)CompressEncoder);

                    //Set the Encoder Info
                    var EncoderInfo = GetEncoderInfo("image/tiff");

                    //Loop through the images we want to combine
                    foreach (string strImageFile in ImageFilesToJoin)
                    {
                        //if we are on the first image
                        if (ImageNumber == 0)
                        {
                            //get the bitmap from the memory stream...don't dispose this until we are completely done!!!!
                            BaseImage = new MemoryStream(File.ReadAllBytes(strImageFile));

                            //Set the base image variable...don't dispose this until we are completely done!!!!
                            ImageToSave = (Bitmap)Image.FromStream(BaseImage);

                            //save the first image
                            ImageToSave.Save(MemoryStreamToSave, EncoderInfo, EncodedParameters);

                            //Now we need to loop through each of the frames --> starting with page 2 (1 - zero based index) because we saved the first page above
                            for (int i = 1; i < ImageToSave.GetFrameCount(FrameDimension.Page); i++)
                            {
                                //Set the parameter to be a next cont. page
                                EncodedParameters.Param[0] = new EncoderParameter(EncoderToUse, (long)EncoderValue.FrameDimensionPage);

                                //set the active frame
                                ImageToSave.SelectActiveFrame(FrameDimension.Page, i);

                                //save the page to the image
                                ImageToSave.SaveAdd(ImageToSave, EncodedParameters);
                            }

                            //Don't dispose of the base image we are using to add the rest of the tiff's onto the base image
                        }
                        else
                        {
                            //set the parameter 
                            EncodedParameters.Param[0] = new EncoderParameter(EncoderToUse, (long)EncoderValue.FrameDimensionPage);

                            //Grab it using the memory stream...we can dispose of this once we are done with this image
                            using (var MemoryStreamToUse = new MemoryStream(File.ReadAllBytes(strImageFile)))
                            {
                                //grab the bitmap image from the memory stream
                                using (var ImageToAddToTheBaseImage = (Bitmap)Image.FromStream(MemoryStreamToUse))
                                {
                                    //now we need to loop though all the pages to add each page to the tiff we are using as the base tiff
                                    for (int i = 0; i < ImageToAddToTheBaseImage.GetFrameCount(FrameDimension.Page); i++)
                                    {
                                        //set the active page
                                        ImageToAddToTheBaseImage.SelectActiveFrame(FrameDimension.Page, i);

                                        //add the page to the image
                                        ImageToSave.SaveAdd(ImageToAddToTheBaseImage, EncodedParameters);
                                    }
                                }
                            }
                        }

                        //if we are up to the last image then we need to flush it out
                        if (ImageNumber == HowManyPictures - 1)
                        {
                            //flush and close.
                            EncodedParameters.Param[0] = new EncoderParameter(EncoderToUse, (long)EncoderValue.Flush);

                            //save
                            ImageToSave.SaveAdd(EncodedParameters);
                        }

                        //increase the image number
                        ImageNumber++;
                    }

                    //close the memory stream
                    MemoryStreamToSave.Flush();

                    //close the memory stream
                    MemoryStreamToSave.Close();

                    //return the byte array now
                    return MemoryStreamToSave.ToArray();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    //Dispose of the memory stream first
                    if (BaseImage != null)
                    {
                        BaseImage.Dispose();
                    }

                    //If my image is not null then dispose of it
                    if (ImageToSave != null)
                    {
                        ImageToSave.Dispose();
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
        /// Getting the supported codec info.
        /// </summary>
        /// <param name="mimeType">description of mime type</param>
        /// <returns>image codec info</returns>
        private static ImageCodecInfo GetEncoderInfo(string MimeType)
        {
            //go grab the encoder
            return ImageCodecInfo.GetImageEncoders().First(x => string.Equals(x.MimeType, MimeType, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

    }

}
