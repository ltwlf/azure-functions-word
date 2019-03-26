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
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Ltwlf.Functions.Word.Models;
using Microsoft.Extensions.Logging;

namespace Ltwlf.Functions.Word
{
    public static class ProcessWordTemplate
    {

        [FunctionName("ProcessWordTemplate")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("ProcessWordTemplate function is processing a request...");

            string wordAsBase64 = String.Empty;
            byte[] wordAsBinary = null;
            ProcessWordTemplateJson data = null;

            try
            {
                data = await req.Content.ReadAsAsync<ProcessWordTemplateJson>();
                wordAsBinary = Convert.FromBase64String(data.WordDocAsBase64);
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message);
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data was send");
            }

            IList<PropertyInfo> props = new List<PropertyInfo>(typeof(Placeholders).GetProperties());

            using (var stream = new MemoryStream(wordAsBinary))
            {
                using (WordprocessingDocument theDoc = WordprocessingDocument.Open(stream, true))
                {
                    foreach (var prop in props.Where(p => p.Name.EndsWith("Tag")))
                    {
                        var tag = prop.GetValue(data.Placeholders) as string;
                        if (tag == null) continue;

                        var value = typeof(Placeholders).InvokeMember(prop.Name.Replace("Tag", "Value"),
                            BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public, null, data.Placeholders, null) as String;
                        value = value ?? String.Empty;

                        var elements = theDoc.MainDocumentPart.Document.Body.Descendants<SdtElement>()
                           .Where(sdt => sdt.SdtProperties.GetFirstChild<Tag>()?.Val == tag);

                        foreach (var element in elements)
                        {
                            if (element != null)
                            {
                                log.LogInformation(String.Format("Placeholder '{0}' was found and will be replaced.", value));

                                element.Descendants<Run>().Skip(1).ToList().ForEach(r => r.Remove());

                                var run = element.Descendants<Run>().FirstOrDefault();
                                run.Descendants().ToList().ForEach(e => { if (e is Text) e.Remove(); });

                                var lines = value.Split('\n');

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
                                log.LogInformation(String.Format("Placeholder '{0}' was not found.", tag));
                            }
                        }
                    }

                    theDoc.MainDocumentPart.Document.Save();
                    theDoc.Close();

                    stream.Flush();
                    stream.Position = 0;
                    wordAsBase64 = Convert.ToBase64String(stream.ToArray());
                }

                return req.CreateResponse(HttpStatusCode.OK, wordAsBase64);
            }
        }
    }
}
