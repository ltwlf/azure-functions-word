using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ltwlf.Functions.Word
{

    [JsonObject]
    public class ProcessTemplateMessage
    {
        [JsonProperty(PropertyName = "file")]
        public string WordAsBase64 { get; set; }

        [JsonProperty(PropertyName = "map")]
        public Dictionary<string, object> Map { get; set; }
    }


    public static class ProcessWordTemplate
    {

        [FunctionName("ProcessTemplate")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("ProcessWordTemplate function is processing a request...");

            string wordAsBase64 = String.Empty;
            byte[] wordAsBinary = null;
            ProcessTemplateMessage data = null;

            try
            {
                var json = await req.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<ProcessTemplateMessage>(json);
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
                    foreach (var kv in data.Map)
                    {
                           
                        var elements = theDoc.MainDocumentPart.Document.Body.Descendants<SdtElement>()
                           .Where(sdt => sdt.SdtProperties.GetFirstChild<Tag>()?.Val == kv.Key);

                        foreach (var element in elements)
                        {
                            if (element != null)
                            {
                                log.LogInformation(String.Format("Placeholder '{0}' was found and will be replaced.", kv.Value));

                                element.Descendants<Run>().Skip(1).ToList().ForEach(r => r.Remove());

                                var run = element.Descendants<Run>().FirstOrDefault();
                                run.Descendants().ToList().ForEach(e => { if (e is Text) e.Remove(); });

                                var lines = kv.Value.ToString().Split('\n');

                                for (int i = 0; i < lines.Length; i++)
                                {
                                    run.Append(new Text(lines[i]));
                                    if (i < lines.Length - 1)
                                    {
                                        run.Append(new Break());
                                    }
                                }
                            }
                            else
                            {
                                log.LogInformation(String.Format("Placeholder '{0}' was not found.", kv.Key));
                            }
                        }
                    }

                    theDoc.MainDocumentPart.Document.Save();
                    theDoc.Close();

                    stream.Flush();
                    stream.Position = 0;
                    wordAsBase64 = Convert.ToBase64String(stream.ToArray());
                }

                return new OkObjectResult(wordAsBase64);
            }
        }
    }
}
