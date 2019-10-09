using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Ltwlf.Functions.Word.Test
{
    public class ProcessTemplateTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ProcessTemplateTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async void Should_Return_OK()
        {   
            var wordAsBase64 = Convert.ToBase64String(await File.ReadAllBytesAsync("test.docx"));
            var inputJson = $@"
            {{
                ""document"":""{wordAsBase64}"",
                ""data"": {{
                    ""Company"":""Hololux"",
                    ""Street"":""Europaallee 27d"",
                    ""City"":""Saarbruecken"",
                    ""Items"":[
                        {{
                            ""Article"":""4711"",
                            ""Quantity"":""2"",
                            ""Price"":""5"",
                            ""Sum"":""10""
                        }},
                        {{
                            ""Article"":""4712"",
                            ""Quantity"":""1"",
                            ""Price"":8,
                            ""Sum"":8
                        }}
                    ]
                }}
            }}";

            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = new MemoryStream(Encoding.UTF8.GetBytes(inputJson))
            };

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = await ProcessTemplate.Run(request, logger);

            Assert.IsType<OkObjectResult>(response);

            var tempPath = Path.GetTempFileName() + ".docx";
            File.WriteAllBytes(tempPath, Convert.FromBase64String((response as OkObjectResult).Value.ToString()));
            _testOutputHelper.WriteLine("Generated Word Document: " + tempPath);
        }
    }
}
