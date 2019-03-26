using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Ltwlf.Functions.Word.Test
{
    public class GetContentControlsTest
    {
        [Fact]
        public async void Should_Return_OK_Address()
        {
            var expectedTags = new string[] { "Company", "Street", "City" };

            var wordAsBase64 = Convert.ToBase64String(await File.ReadAllBytesAsync("test.docx"));

            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = new MemoryStream(Encoding.UTF8.GetBytes($"{{\"file\":\"{wordAsBase64}\"}}"))
            };

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = await GetContentControls.Run(request, logger);

            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(expectedTags, (response as OkObjectResult).Value);

        }
    }
}
