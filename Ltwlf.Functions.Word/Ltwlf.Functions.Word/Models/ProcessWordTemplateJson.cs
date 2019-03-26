using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ltwlf.Functions.Word.Models
{
    [JsonObject]
    public class ProcessWordTemplateJson
    {
        [JsonProperty(PropertyName = "file")]
        public string WordDocAsBase64 { get; set; }

        [JsonProperty(PropertyName = "placeholders")]
        public Placeholders Placeholders { get; set; }
    }

    [JsonObject]
    public class Placeholders
    {
        [JsonProperty(PropertyName = "placeholder00Tag")]
        public string Placeholder00Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder00Value")]
        public string Placeholder00Value { get; set; }

        [JsonProperty(PropertyName = "placeholder01Tag")]
        public string Placeholder01Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder01Value")]
        public string Placeholder01Value { get; set; }

        [JsonProperty(PropertyName = "placeholder02Tag")]
        public string Placeholder02Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder02Value")]
        public string Placeholder02Value { get; set; }

        [JsonProperty(PropertyName = "placeholder03Tag")]
        public string Placeholder03Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder03Value")]
        public string Placeholder03Value { get; set; }

        [JsonProperty(PropertyName = "placeholder04Tag")]
        public string Placeholder04Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder04Value")]
        public string Placeholder04Value { get; set; }

        [JsonProperty(PropertyName = "placeholder05Tag")]
        public string Placeholder05Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder05Value")]
        public string Placeholder05Value { get; set; }

        [JsonProperty(PropertyName = "placeholder06Tag")]
        public string Placeholder06Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder06Value")]
        public string Placeholder06Value { get; set; }

        [JsonProperty(PropertyName = "placeholder07Tag")]
        public string Placeholder07Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder07Value")]
        public string Placeholder07Value { get; set; }

        [JsonProperty(PropertyName = "placeholder08Tag")]
        public string Placeholder08Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder08Value")]
        public string Placeholder08Value { get; set; }

        [JsonProperty(PropertyName = "placeholder09Tag")]
        public string Placeholder09Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder09Value")]
        public string Placeholder09Value { get; set; }

        [JsonProperty(PropertyName = "placeholder10Tag")]
        public string Placeholder10Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder10Value")]
        public string Placeholder10Value { get; set; }

        [JsonProperty(PropertyName = "placeholder11Tag")]
        public string Placeholder11Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder11Value")]
        public string Placeholder11Value { get; set; }

        [JsonProperty(PropertyName = "placeholder12Tag")]
        public string Placeholder12Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder12Value")]
        public string Placeholder12Value { get; set; }

        [JsonProperty(PropertyName = "placeholder13Tag")]
        public string Placeholder13Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder13Value")]
        public string Placeholder13Value { get; set; }

        [JsonProperty(PropertyName = "placeholder14Tag")]
        public string Placeholder14Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder14Value")]
        public string Placeholder14Value { get; set; }

        [JsonProperty(PropertyName = "placeholder15Tag")]
        public string Placeholder15Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder15Value")]
        public string Placeholder15Value { get; set; }

        [JsonProperty(PropertyName = "placeholder16Tag")]
        public string Placeholder16Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder16Value")]
        public string Placeholder16Value { get; set; }

        [JsonProperty(PropertyName = "placeholder17Tag")]
        public string Placeholder17Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder17Value")]
        public string Placeholder17Value { get; set; }

        [JsonProperty(PropertyName = "placeholder18Tag")]
        public string Placeholder18Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder18Value")]
        public string Placeholder18Value { get; set; }

        [JsonProperty(PropertyName = "placeholder19Tag")]
        public string Placeholder19Tag { get; set; }

        [JsonProperty(PropertyName = "placeholder19Value")]
        public string Placeholder19Value { get; set; }
    }
}
