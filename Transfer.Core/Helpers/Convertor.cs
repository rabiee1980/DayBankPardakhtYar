using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.IO;

namespace Transfer.Core.Helpers
{
    public class Convertor
    {
        public static dynamic JsonDeserialize(string value)
        {
            var json = JsonConvert.DeserializeObject<JObject>(value);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic dobj = jsonSerializer.Deserialize<dynamic>(json.ToString());
            return dobj;
        }

        public static string CombineValueWithDateTime(string value)
        {
            string result = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + value;
            return result;
        }

        public static string MiladiDateToJalaliDate(DateTime date)
        {
            PersianCalendar jc = new PersianCalendar();
            return string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));
        }

        public static string Base64ToImage(string base64String, string imagePath, string imageName)
        {
            try
            {
                //***Convert Image File to Base64 Encoded string***//

                //Read the uploaded file using BinaryReader and convert it to Byte Array.
                //BinaryReader br = new BinaryReader(FileUpload1.PostedFile.InputStream);
                //byte[] bytes = br.ReadBytes((int)FileUpload1.PostedFile.InputStream.Length);

                //Convert the Byte Array to Base64 Encoded string.
                //string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                //***Save Base64 Encoded string as Image File***//

                //Convert Base64 Encoded string to Byte Array.
                byte[] imageBytes = Convert.FromBase64String(base64String);

                //Save the Byte Array as Image File.
                string filePath = imagePath +  imageName + ".jpeg";
                File.WriteAllBytes(filePath, imageBytes);
                return imageName + ".jpeg";
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return  "default.jpeg"; 
            }
        }

        public static string Base64ToFile(string base64String, string filePath, string fileName , string fileFormat)
        {
            try
            {
                //***Convert Image File to Base64 Encoded string***//

                //Read the uploaded file using BinaryReader and convert it to Byte Array.
                //BinaryReader br = new BinaryReader(FileUpload1.PostedFile.InputStream);
                //byte[] bytes = br.ReadBytes((int)FileUpload1.PostedFile.InputStream.Length);

                //Convert the Byte Array to Base64 Encoded string.
                //string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                //***Save Base64 Encoded string as Image File***//

                //Convert Base64 Encoded string to Byte Array.

                if (fileFormat.ToLower() == ".jpeg" || fileFormat.ToLower() == ".mp4" || fileFormat.ToLower() == ".gif")
                {
                    byte[] imageBytes = Convert.FromBase64String(base64String);

                    //Save the Byte Array as Image File.
                    string Path = filePath + fileName + fileFormat;
                    File.WriteAllBytes(Path, imageBytes);
                    return fileName + fileFormat;
                }
                else
                {
                    return "InvalidFormat";
                }
                
            }
            catch
            {
                return "ConvertError";
            }
        }       
    }
}
