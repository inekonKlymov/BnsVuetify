using System.Globalization;

namespace Bns.Api.Common.Datatables
{
    public static class DateTimeExtensions
    {
        #region Public Fields

        public static class DateTimeConstants
        {
            public const string ISO8601SystemFormat = @"yyyy-MM-ddTHH:mm:ss";
            public const string ISO8601MomentJsFormat = @"yyyy-MM-DDTHH:mm:ss";
            public const string DisplayFormat = "DD.MM.YYYY HH:mm:ss";
            public const string DisplayBackendFormat = "dd.MM.yyyy HH:mm:ss";
        }

        public static class DateOnlyConstants
        {
            public const string ISO8601SystemFormat = "yyyy-MM-dd";
            public const string ISO8601MomentJsFormat = "yyyy-MM-DD";
            public const string DisplayFormat = "DD.MM.YYYY";
            public const string DisplayBackendFormat = "dd.MM.yyyy";
        }

        public static class TimeOnlyConstants
        {
            public const string ISO8601SystemFormat = "HH:mm:ss";
            public const string ISO8601MomentJsFormat = "HH:mm:ss";
            public const string DisplayFormat = "HH:mm:ss";
            public const string DisplayBackendFormat = "HH:mm:ss";
        }

        //public const string ISO8601DateOnlySystemFormat = "yyyy-MM-dd";
        //public const string ISO8601DateTimeSystemFormat = @"yyyy-MM-ddTHH:mm:ss";
        //public const string ISO8601TimeOnlySystemFormat = "HH:mm:ss";

        //public const string ISO8601DateOnlyMomentJsFormat = "yyyy-MM-DD";
        //public const string ISO8601DateTimeMomentJsFormat = @"yyyy-MM-DDTHH:mm:ss";
        //public const string ISO8601TimeOnlyMomentJsFormat = "HH:mm:ss";

        //public const string DisplayDateOnlyFormat = "DD.MM.YYYY";
        //public const string DisplayDateTimeFormat = "DD.MM.YYYY HH:mm:ss";
        //public const string DisplayTimeOnlyFormat = "HH:mm:ss";
        //public const string DisplayDateOnlyBackendFormat = "dd.MM.yyyy";
        //public const string DisplayDateTimeBackendFormat = "dd.MM.yyyy HH:mm:ss";
        //public const string DisplayTimeOnlyBackendFormat = "HH:mm:ss";

        #endregion Public Fields

        #region Public Methods

        //public const string ISO8601DateTimeFormat = @"yyyy-MM-ddTHH:mm:ss.fffffffzzz";
        //public const string ISO8601TimeOnlyFormat = "HH:mm:ss.fffffffzzz";

        public static string ToISO8601(this DateTime dateTime) => dateTime.ToString(DateTimeConstants.ISO8601SystemFormat, CultureInfo.InvariantCulture);

        public static string ToISO8601(this DateOnly dateTime) => dateTime.ToString(DateOnlyConstants.ISO8601SystemFormat, CultureInfo.InvariantCulture);

        public static string ToISO8601(this TimeOnly dateTime) => dateTime.ToString(TimeOnlyConstants.ISO8601SystemFormat, CultureInfo.InvariantCulture);

        public static string ToISO8601(this TimeSpan dateTime) => dateTime.ToString(@"hh\:mm\:ss");

        #endregion Public Methods
    }
}