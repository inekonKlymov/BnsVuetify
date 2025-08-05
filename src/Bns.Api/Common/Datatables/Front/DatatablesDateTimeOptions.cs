using Bns.Api.Common.Datatables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bns.General.Domain.Common.Datatables.Front
{
    public record DatatablesDateTimeOptions
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "buttons")]
        public DatatablesDateTimeButtonsOptions? buttons { get; init; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "classPrefix")]
        public string? classPrefix { get; init; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "disableDays")]
        public int[]? disableDays { get; init; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "firstDay")]
        public int? firstDay { get; init; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "format")]
        public string? format { get; init; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "hoursAvailable")]
        public int[]? hoursAvailable { get; init; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "locale")]
        public string? locale { get; init; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "maxDate")]
        public DateTime? maxDate { get; init; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "minDate")]
        public DateTime? minDate { get; init; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "minutesAvailable")]
        public int[]? minutesAvailable { get; init; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "strict")]
        //public bool? strict {get;init;}

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "onChange")]
        public JRaw? onChange { get; init; } //?: (value: string, date: Date, el: HTMLElement) => void {get;init;}

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "secondsAvailable")]
        public int[]? secondsAvailable { get; init; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "showWeekNumber")]
        public bool? showWeekNumber { get; init; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "yearRange")]
        public int? yearRange { get; init; }

        public static readonly DatatablesDateTimeOptions Default = new()
        {
            firstDay = 1,
            format = DateTimeExtensions.DateTimeConstants.DisplayFormat,
            buttons = new() { clear = true }
        };
    }

    public record DatatablesDateTimeButtonsOptions
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "today")]
        public bool? today;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "clear")]
        public bool? clear;
    }
}
