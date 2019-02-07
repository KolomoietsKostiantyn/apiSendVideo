using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class VideosController : ApiController
    {
        public HttpResponseMessage Get(string filename, string ext)
        {
            var video = new VideoStream(filename, ext);

            var response = Request.CreateResponse(); // Stream outputStream, HttpContent content, TransportContext context

            Action<Stream, HttpContent, TransportContext> ww = video.WriteToStream;
            response.Content = new PushStreamContent(ww, new MediaTypeHeaderValue("video/" + ext));

            return response;
        }

    }
}
