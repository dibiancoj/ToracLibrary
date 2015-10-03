using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ToracAttributes;

namespace ToracLibrary.Core.Graphics
{

    /// <summary>
    /// Common graphics utilities. This is different from the ToracLibrary.Graphics.dll which brings in others dlls. This should be lightweight with no outside references
    /// </summary>
    public static class GraphicsUtilities
    {

        /// <summary>
        /// When using the file file reader in asp.net. You can pass an image using a string. So when we get it in the controller, we want to conver it back
        /// </summary>
        /// <param name="FileToConvert">File To Convert</param>
        /// <returns>Class with the broken out file information and file bytes</returns>
        [MethodIsNotTestable("Tough to test images. Don't feel like writing 1,000 bytes to test it")]
        public static ImageFromJsonResult ImageFromJsonBase64String(string FileToConvert)
        {
            //the format of the string should be something like this:
            //data: image / jpeg; base64, "then all the file bytes"

            //html
            //< input id = "CoursePicture" type = "file" name = "CoursePicture" />

            //javascript
            //  var file = document.getElementById('CoursePicture').files[0]; //Files[0] = 1st file

            //var reader = new FileReader();

            //create the event so when it's done we can go save the record (file reader is async)
            //reader.onload = function(e) {
            //      SaveACourseToServer(e.target.result);

            //just pass in the file using { CourseImage: Result of e.target.result }
            //}

            //go read the file now
            //reader.readAsDataURL(file);


            //declare the delimiters we will split by
            var Delimiters = new[] { ':', ';', ',' };

            //let's go split the passed in file to convert
            var FileParts = FileToConvert.Split(Delimiters, StringSplitOptions.RemoveEmptyEntries);

            //to save this, you can use this
            // System.IO.File.WriteAllBytes();

            //let's go return everything (the reason we have fileparts.length - 1) is we've seen sometimes there could be another field. Frankie saw this. I will leave it in for now
            return new ImageFromJsonResult(FileParts[1], FileParts[2], Convert.FromBase64String(FileParts[FileParts.Length - 1]));
        }

    }

}
