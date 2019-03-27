using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ltwlf.Functions.Word
{
    [JsonObject]
    public class GetContentControlsMessage
    {
        [JsonProperty(PropertyName = "file")]
        public string WordAsBase64 { get; set; }
    }


    public static class GetContentControls
    {

        [FunctionName("GetPlaceholders")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("ProcessWordTemplate function is processing a request...");

            string wordAsBase64 = String.Empty;
            byte[] wordAsBinary = null;
            GetContentControlsMessage data = null;

            try
            {
                var json = await req.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<GetContentControlsMessage>(json);
                wordAsBinary = Convert.FromBase64String(data.WordAsBase64);
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message);
                return new ExceptionResult(ex, false);
            }
   

            using (var stream = new MemoryStream(wordAsBinary))
            {
                using (WordprocessingDocument theDoc = WordprocessingDocument.Open(stream, true))
                {
                   
                    var contentControls = theDoc.MainDocumentPart.Document.Body.Descendants<SdtElement>()
                        .Where(sdt => sdt.SdtProperties.GetFirstChild<Tag>()?.Val != null)
                        .Select(sdt => sdt.SdtProperties.GetFirstChild<Tag>().Val.Value);

                    theDoc.Close();

                    var schema = new JSchema
                    {
                        Type = JSchemaType.Object
                    };

                    contentControls.ToList().ForEach(c => {
                        schema.Properties.Add(c, new JSchema { Type = JSchemaType.String });
                    });
                    
                    return new OkObjectResult(schema.ToString());
                }
            }
        }
    }
}
