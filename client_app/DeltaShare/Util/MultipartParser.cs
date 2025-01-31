using System.Net;
using MimeKit;

namespace DeltaShare.Util
{
    public static class MultipartParser
    {
        public static void SendResponse(HttpListenerContext ctx, string responseString)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            HttpListenerResponse response = ctx.Response;
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        public static async Task<Dictionary<string, MimePart>> Parse(HttpListenerContext ctx, string requestPath)
        {
            // check request type and path
            HttpListenerRequest request = ctx.Request;
            ContentType contentType;
            if (request.HttpMethod != "POST" ||
                request.ContentType == null ||
                request.Url?.AbsolutePath != requestPath)
                throw new ArgumentException("Invalid request");
            ContentType.TryParse(request.ContentType, out contentType);
            if (!contentType.MimeType.Contains("multipart/form-data", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Invalid content type");

            // parse multipart form data
            ParserOptions parserOptions = new();
            Multipart multipart = (Multipart)await MimeEntity.LoadAsync(parserOptions, contentType, request.InputStream);

            // extract form parts
            Dictionary<string, MimePart> formParts = new();
            foreach (MimeEntity part in multipart)
            {
                if (part.ContentDisposition == null) continue;
                string fieldName = part.ContentDisposition.Parameters["name"];
                formParts[fieldName] = (MimePart)part;
            }

            return formParts;
        }

    }
}
