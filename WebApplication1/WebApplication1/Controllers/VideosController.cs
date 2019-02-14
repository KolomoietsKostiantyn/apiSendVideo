using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web.Hosting;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class VideosController : ApiController
    {

        public string Get(int id)
        {
            string st1 = HostingEnvironment.MapPath("~/Videos/");
            return "value = " + id + "//" + st1;
        }

        // GET api/<controller>
        public HttpResponseMessage Get(string filename, string ext)
        {
            if (filename == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            
            string filePath = HostingEnvironment.MapPath("~/Videos/") + filename + "." + ext;
            if (Request.Headers.Range != null)
            {
                try
                {
                    
                    System.Text.Encoder stringEncoder = Encoding.UTF8.GetEncoder();
                    byte[] stringBytes = new byte[stringEncoder.GetByteCount(filePath.ToCharArray(), 0, filePath.Length, true)];
                    stringEncoder.GetBytes(filePath.ToCharArray(), 0, filePath.Length, stringBytes, 0, true);
                    MD5CryptoServiceProvider MD5Enc = new MD5CryptoServiceProvider();
                    string hash = BitConverter.ToString(MD5Enc.ComputeHash(stringBytes)).Replace("-", string.Empty);

                    HttpResponseMessage partialResponse = Request.CreateResponse(HttpStatusCode.PartialContent);
                    partialResponse.Headers.AcceptRanges.Add("bytes");
                    partialResponse.Headers.ETag = new EntityTagHeaderValue("\"" + hash + "\"");
                    var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    partialResponse.Content = new ByteRangeStreamContent(stream, Request.Headers.Range, new MediaTypeHeaderValue("video/mp4"));

                    return partialResponse;
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.RequestedRangeNotSatisfiable);
            }
        }
    }











}







//public HttpResponseMessage Get(string filename, string ext)
//{
//    var video = new VideoStream(filename, ext);

//    var response = Request.CreateResponse(); // Stream outputStream, HttpContent content, TransportContext context

//    Action<Stream, HttpContent, TransportContext> ww = video.WriteToStream;
//    response.Content = new PushStreamContent(ww, new MediaTypeHeaderValue("video/" + ext));

//    return response;
//}

//    }







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
