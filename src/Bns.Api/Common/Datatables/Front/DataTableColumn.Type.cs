using System.Runtime.Serialization;

namespace Bns.General.Domain.Common.Datatables.Front
{
    public enum DataTableColumnTypeEnum
    {
        /// <summary>
        /// Date / time values. Note that DataTables' built in date parsing works to an ISO 8601 format with 3 separators (/, - and ,). Additional date format support can be added through the use of the built in datetime renderer plus one of the Moment.js or Luxon libraries.
        /// Sorting - sorted chronologically
        /// Filtering - no effect
        /// </summary>
        ///
        [EnumMember(Value = "date")]
        date,

        /// <summary>
        /// Simple number sorting
        /// Sorting - sorted numerically
        /// Filtering - no effect
        /// </summary>
        [EnumMember(Value = "num")]
        number,

        /// <summary>
        ///  Numeric sorting of formatted numbers. Numbers which are formatted with thousands separators, currency symbols or a percentage indicator will be sorted numerically automatically by DataTables.
        ///  Supported built-in currency symbols are $, £, € and ¥.
        ///  Supported built-in thousands separators are ' and ,.
        ///  Examples:
        ///  $100,000 - sorted as 100000
        ///  £10'000 - sorted as 10000
        ///  5'000 - sorted as 5000
        ///  40% - sorted as 40
        ///  Sorting - sorted numerically
        ///  Filtering - no effect
        /// </summary>
        [EnumMember(Value = "num-fmt")]
        formattedNumber,

        /// <summary>
        /// As per the num option, but with HTML tags also in the data.
        /// Sorting - sorted numerically
        /// Filtering - HTML tags removed from filtering string
        /// </summary>
        [EnumMember(Value = "html-num")]
        htmlNumber,

        /// <summary>
        /// As per the num-fmt option, but with HTML tags also in the data.
        /// Sorting - sorted numerically
        /// Filtering - HTML tags removed from filtering string
        /// </summary>
        [EnumMember(Value = "html-num-fmt")]
        htmlFormattedNumber,

        /// <summary>
        /// Detected if the string contains HTML tags and it contains non-ASCII characters
        /// Sorting - Sorts using localeCompare to ensure diacritc characters are correctly sorted
        /// Filtering - Can search either with or without diacritic characters, and will match with or without
        /// </summary>
        [EnumMember(Value = "html-utf8")]
        htmlUtf8,

        /// <summary>
        /// Basic string processing for HTML tags
        /// Sorting - sorted with HTML tags removed
        /// Filtering - HTML tags removed from filtering string
        /// </summary>
        [EnumMember(Value = "html")]
        html,

        /// <summary>
        /// String data type if the text is found to contain non-ASCII characters
        /// Sorting - Sorts using localeCompare
        /// Filtering - Can search either with or without diacritic characters, and will match with or without
        /// </summary>
        [EnumMember(Value = "string-utf8")]
        stringUtf8,

        /// <summary>
        /// Fall back type if the data in the column does not match the requirements for the other data types.
        /// Sorting - no effect
        /// Filtering - no effect
        /// </summary>
        [EnumMember(Value = "string")]
        @string,

        [EnumMember(Value = "boolean")]
        customBool,

        [EnumMember(Value = "hierarchy")]
        hierarchyView
    }
}
