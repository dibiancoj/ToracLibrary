using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.DiskIO.Zip
{

    //*** currently I don't have any unit test for this. I could zip up 2 text files, then unzip them, and make sure i can read them. Maybe do it later

    /// <summary>
    /// Zips And UnZips Files And Directorys
    /// </summary>
    public static class FileZipper
    {

        #region Zip Up

        /// <summary>
        /// Zip's up a list of files where you only have the byte array. So everything is in memory, and you want to zip it up and send it for download or do something else with a byte array
        /// </summary>
        /// <param name="FilesToZip">Files To Zip Up. Key is the file name and the value is the byte array which contains the file</param>
        /// <returns>The Zipped up files in a byte array</returns>
        public static byte[] ZipByteArray(IDictionary<string, byte[]> FilesToZip)
        {
            //create the memory stream which the zip will be created with
            using (var MemoryStreamToCreateZipWith = new MemoryStream())
            {
                //declare the working zip archive
                using (var WorkingZipArchive = new ZipArchive(MemoryStreamToCreateZipWith, ZipArchiveMode.Update, false))
                {
                    //loop through each of the files and add it to the working zip
                    foreach (var FileToZipUp in FilesToZip)
                    {
                        //Create a zip entry for each attachment
                        var ZipEntry = WorkingZipArchive.CreateEntry(FileToZipUp.Key);

                        //Get the stream of the attachment
                        using (var FileToZipUpInMemoryStream = new MemoryStream(FileToZipUp.Value))
                        {
                            //grab the memory stream from the zip entry
                            using (var ZipEntryStream = ZipEntry.Open())
                            {
                                //Copy the attachment stream to the zip entry stream
                                FileToZipUpInMemoryStream.CopyTo(ZipEntryStream);
                            }
                        }
                    }
                }

                //all done, so go return the byte array which contains the zipped up file bytes
                return MemoryStreamToCreateZipWith.ToArray();
            }
        }

        /// <summary>
        /// Zip up a directory
        /// </summary>
        /// <param name="SourceDirectoryName">Directory Source Name</param>
        /// <param name="DestinationFileName">Destination File Name</param>
        /// <param name="WhichCompressionLevel">Compression Level To Use</param>
        /// <param name="IncludeSourceDirectory">True to include the directory name from sourceDirectoryName at the root of the archive; false to include only the contents of the directory.)</param>
        /// <returns>Awaitable Task</returns>
        public static Task ZipDirectoryAsync(string SourceDirectoryName,
                                            string DestinationFileName,
                                            CompressionLevel WhichCompressionLevel,
                                            bool IncludeSourceDirectory)
        {
            //example on how to call this using await
            //string sourceDirectory = @"C:\Users\Jason\Documents\Developement";
            //string destinationDirectory = @"E:\Jason Manual Backup\jason.zip";

            //await FileZipper.ZipDirectory(sourceDirectory, destinationDirectory, System.IO.Compression.CompressionLevel.Optimal, false);

            //MessageBox.Show("File Zipped Up");

            //first validate the folder is there
            if (!Directory.Exists(SourceDirectoryName))
            {
                throw new DirectoryNotFoundException("Directory Not Found At: " + SourceDirectoryName);
            }

            //go create the task and return it so the end developer can await it
            return Task.Factory.StartNew(() => ZipFile.CreateFromDirectory(SourceDirectoryName, DestinationFileName, WhichCompressionLevel, IncludeSourceDirectory));
        }

        #endregion

        #region Un-Zip

        /// <summary>
        /// Unzip a zipped file to a directory
        /// </summary>
        /// <param name="ArchivePath">Where the archive file is</param>
        /// <param name="DestinationPath">Where To UnZip The File Contents Too</param>
        /// <param name="CreateDirectoryIfNotFound">Create The Folder If Destination Path Is Not Found. Must Have Permissions To Create Folder.</param>
        /// <returns>Awaitable Task</returns>
        public static Task UnZipFileToDirectoryAsync(string ArchivePath, string DestinationPath, bool CreateDirectoryIfNotFound)
        {
            //go run the method to unzip it
            return Task.Factory.StartNew(() =>
            {
                //let's see if we can find the archive file
                if (!File.Exists(ArchivePath))
                {
                    throw new FileNotFoundException("Archive File Not Found At: " + ArchivePath);
                }

                //if we can't find the directory and they don't want to create it then throw an error
                if (!Directory.Exists(DestinationPath))
                {
                    //if they don't want to create the directory then throw an error
                    if (!CreateDirectoryIfNotFound)
                    {
                        //they don't want to create the directory and we couldn't find it so throw an exception
                        throw new DirectoryNotFoundException($"Directory Not Found At: {DestinationPath}. If You Would Like To Create The Directory Pass In True To Parameter CreateDirectoryIfNotFound");
                    }
                    else
                    {
                        //create the directory
                        Directory.CreateDirectory(DestinationPath);
                    }
                }

                //now go unzip the directory
                ZipFile.ExtractToDirectory(ArchivePath, DestinationPath);
            });
        }

        #endregion

    }
}
