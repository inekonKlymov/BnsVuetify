using Newtonsoft.Json;

namespace Bns.General.Domain.Common.Datatables.Front
{
    public record DataTableFrontFieldOption(
        [property: JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "value")] string Value,
        [property: JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "label")] string Label
        )
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "add")]
        public Dictionary<string, object>? addInfo = null;
    };
}
