using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace WebApplication1.Models
{
    public class VideoStream
    {
        private readonly string _filename;

        public VideoStream(string filename, string ext)
        {
            _filename = @"C:UsersFilipDownloads" + filename + "." + ext;
        }

        public async void WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            try
            {
                byte[] buffer = new byte[65536];

                using (var video = File.Open(_filename, FileMode.Open, FileAccess.Read))
                {

                    var length = (int)video.Length;
                    var bytesRead = 1;

                    while (length > 0 && bytesRead > 0)
                    {
                        bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));

                        await outputStream.WriteAsync(buffer, 0, bytesRead);
                        length -= bytesRead;
                    }
                }
            }
            catch (HttpException ex)
            {
                return;
            }
            finally
            {
                outputStream.Close();
            }
        }
    }
}








//public class VideoController : ApiController
//{

//    // GET api/<controller>
//    public HttpResponseMessage Get(string filename)
//    {
//        if (filename == null)
//            return new HttpResponseMessage(HttpStatusCode.BadRequest);

//        string filePath = HostingEnvironment.MapPath("~/Videos/") + filename;

//        if (Request.Headers.Range != null)
//        {
//            //Range Specifc request: Stream video on wanted range.
//            try
//            {
//                //NOTE: ETag calculation only with file name is one approach (Not the best one though - GUIDs or DateTime is may required in live applications.).
//                Encoder stringEncoder = Encoding.UTF8.GetEncoder();
//                byte[] stringBytes = new byte[stringEncoder.GetByteCount(filePath.ToCharArray(), 0, filePath.Length, true)];
//                stringEncoder.GetBytes(filePath.ToCharArray(), 0, filePath.Length, stringBytes, 0, true);
//                MD5CryptoServiceProvider MD5Enc = new MD5CryptoServiceProvider();
//                string hash = BitConverter.ToString(MD5Enc.ComputeHash(stringBytes)).Replace("-", string.Empty);

//                HttpResponseMessage partialResponse = Request.CreateResponse(HttpStatusCode.PartialContent);
//                partialResponse.Headers.AcceptRanges.Add("bytes");
//                partialResponse.Headers.ETag = new EntityTagHeaderValue("\"" + hash + "\"");

//                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
//                partialResponse.Content = new ByteRangeStreamContent(stream, Request.Headers.Range, new MediaTypeHeaderValue("video/mp4"));
//                return partialResponse;
//            }
//            catch (Exception ex)
//            {
//                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
//            }
//        }
//        else
//        {
//            return new HttpResponseMessage(HttpStatusCode.RequestedRangeNotSatisfiable);
//        }
//    }
//}