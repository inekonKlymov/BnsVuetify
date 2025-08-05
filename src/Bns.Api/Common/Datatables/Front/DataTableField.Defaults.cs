using Bns.Api.Common.Datatables;
using Newtonsoft.Json.Linq;

namespace Bns.General.Domain.Common.Datatables.Front
{
    public static class DataTableFieldDefaults
    {
        #region Public Classes

        public static class Bool
        {
            #region Public Fields

            public static readonly Dictionary<string, object> DefaultAttributes = new()
            {
                //{ "style", $"min-width:{DataTableColumnDefaults.Bool.Width}"  },
            };

            public static readonly JRaw GetFormatter = new(
                            $$"""
                    function (value, field) {
                        return value == "1" ? true : false;
                    }
                """);

            public static readonly DataTableEditorFieldCheckboxOptions Options = new DataTableEditorFieldCheckboxOptions(options: new Dictionary<string, string>() { { string.Empty, "1" } }, separator: "|");

            public static readonly JRaw SetFormatter = new(
                $$"""
                function (val, field) {
                    return val == "True" || val == true;
                }
                """);

            #endregion Public Fields
        }

        public static class Byte
        {
            #region Public Fields

            public static readonly Dictionary<string, object> DefaultAttributes = new() { { "type", "number" },
                { "style", $"min-width:{DataTableColumnDefaults.Byte.Width}"  },
                //{ "pattern", "[0-9]" }
            };

            #endregion Public Fields
        }

        public static class Date
        {
            #region Public Fields

            public static readonly Dictionary<string, object> DefaultAttributes = new()
            {
                //{ "style", $"min-width:{DataTableColumnDefaults.DateOnly.Width}"  },
                //{ "style", "width:auto" }
            };

            public static readonly JRaw GetFormatter = new(
                            $$"""
                    function (val, field) {
                        if (val === undefined || val === null || val === '') return null;
                        var result = moment(val, '{{DateTimeExtensions.DateOnlyConstants.DisplayFormat}}');
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateTimeConstants.ISO8601MomentJsFormat}}');
                        result = moment(val)
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateTimeConstants.ISO8601MomentJsFormat}}');
                    }
                """);

            public static readonly JRaw GetFormatterDisplay = new(
                $$"""
                    function (val, field) {
                        if (val === undefined || val === null || val === '') return null;
                        var result = moment(val, '{{DateTimeExtensions.DateOnlyConstants.DisplayFormat}}');
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateTimeConstants.ISO8601MomentJsFormat}}');
                        result = moment(val)
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateTimeConstants.ISO8601MomentJsFormat}}');
                    }
                """);

            public static readonly DataTableEditorFieldDateTimeOptsOptions Opts = new(firstDay: 1, maxDate: new JRaw("new Date('2099-01-01')"));

            public static readonly JRaw SetFormatter = new(
                            $$"""
                    function (val, field) {
                        if (val === undefined || val === null || val === '') return;
                        var result = moment(val, '{{DateTimeExtensions.DateOnlyConstants.ISO8601MomentJsFormat}}');
                        if(result._isValid) return result.toDate();
                        result = moment(val)
                        if(result._isValid) return result.toDate();
                    }
                """);

            public static readonly JRaw SetFormatterDisplay = new(
                            $$"""
                    function (val, field) {
                        if (val === undefined || val === null || val === '') return;
                        var result = moment(val, '{{DateTimeExtensions.DateOnlyConstants.ISO8601MomentJsFormat}}');
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateOnlyConstants.DisplayFormat}}');
                        result = moment(val)
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateOnlyConstants.DisplayFormat}}');
                    }
                """);

            #endregion Public Fields
        }

        public static class DateTime
        {
            #region Public Fields

            public static readonly Dictionary<string, object> DefaultAttributes = new()
            {
                //{ "style", $"min-width:{DataTableColumnDefaults.DateTime.Width}"  },
            };

            public static readonly JRaw GetFormatter = new(
                $$"""
                    function (val, field) {
                        if (val === undefined || val === null || val === '') return null;
                        var result = moment(val, '{{DateTimeExtensions.DateTimeConstants.DisplayFormat}}');
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateTimeConstants.ISO8601MomentJsFormat}}');
                        result = moment(val)
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateTimeConstants.ISO8601MomentJsFormat}}');
                    }
                """);

            public static readonly JRaw GetFormatterDisplay = new(
                $$"""
                    function (val, field) {
                        if (val === undefined || val === null || val === '') return null;
                        var result = moment(val, '{{DateTimeExtensions.DateTimeConstants.DisplayFormat}}');
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateTimeConstants.ISO8601MomentJsFormat}}');
                        result = moment(val)
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateTimeConstants.ISO8601MomentJsFormat}}');
                    }
                """);

            public static readonly DataTableEditorFieldDateTimeOptsOptions Opts = new(firstDay: 1, maxDate: new JRaw("new Date('2099-01-01')"));

            public static readonly JRaw SetFormatter = new(
                            $$"""
                    function (val, field) {
                        if (val === undefined || val === null || val === '') return null;
                        var result = moment(val, '{{DateTimeExtensions.DateTimeConstants.DisplayFormat}}');
                        if(result._isValid) return result.toDate();
                        result = moment(val)
                        if(result._isValid) return result.toDate();
                    }
                """);

            public static readonly JRaw SetFormatterDisplay = new(
                            $$"""
                    function (val, field) {
                        if (val === undefined || val === null || val === '') return;
                        var result = moment(val, '{{DateTimeExtensions.DateTimeConstants.ISO8601MomentJsFormat}}');
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateTimeConstants.DisplayFormat}}');
                        result = moment(val)
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateTimeConstants.DisplayFormat}}');
                    }
                """);

            #endregion Public Fields
        }

        public static class Debug
        {
            #region Public Fields

            public static readonly Dictionary<string, object> DefaultAttributes = new()
            {
                //{ "style", "width:auto" }
            };

            public static readonly JRaw GetFormatter = new(
                            $$"""
                    function (val, field) {
                        debugger;
                        return val;
                    }
                """);

            public static readonly JRaw SetFormatter = new(
                $$"""
                    function (val, field) {
                        debugger;
                        return val;
                    }
                """);

            #endregion Public Fields

            #region Public Methods

            public static JRaw? Data(string colName) => new($$"""
                    function (data, type, set) {
                        debugger;
                        return data.{{colName}};
                    }
                """);

            #endregion Public Methods
        }

        public static class Decimal
        {
            #region Public Fields

            public static readonly Dictionary<string, object> DefaultAttributes = new() { { "type", "number" }, { "step", "0.01" } ,
                //{"style", "width:auto" },
                //{ "style", $"min-width:{DataTableColumnDefaults.Decimal.Width}"  },
                { "pattern", "[0-9-.]" } };

            #endregion Public Fields
        }

        public static class Double
        {
            #region Public Fields

            public static readonly Dictionary<string, object> DefaultAttributes = new() { { "type", "number" }, { "step", "0.01" } ,
                //{"style", "width:auto" },
                //{ "style", $"min-width:{DataTableColumnDefaults.Double.Width}"  },
                { "pattern", "[0-9-.]" }
            };

            #endregion Public Fields
        }

        public static class Int
        {
            #region Public Fields

            public static readonly Dictionary<string, object> DefaultAttributes = new() { { "type", "number" },
                ////{ "style", $"min-width:{DataTableColumnDefaults.Int.Width}"  },
                { "pattern", "[0-9]" }
            };

            #endregion Public Fields
        }

        public static class String
        {
            #region Public Fields

            public static readonly Dictionary<string, object> DefaultAttributes = new()
            {
                //{ "style", $"min-width:{DataTableColumnDefaults.String.Width}"  },
                //{ "style", "width:auto" }
            };

            #endregion Public Fields
        }

        public static class Time
        {
            #region Public Fields

            public static readonly Dictionary<string, object> DefaultAttributes = new()
            {
                //{ "style", $"min-width:{DataTableColumnDefaults.TimeOnly.Width}"  },
                //{ "style", "width:auto" }
            };

            public static readonly JRaw GetFormatter = new(
                            $$"""
                    function (val, field) {
                        if (val === undefined || val === null || val === '') return null;
                        var result = moment(val, '{{DateTimeExtensions.TimeOnlyConstants.DisplayFormat}}');
                        if(result._isValid) return result.format('{{DateTimeExtensions.TimeOnlyConstants.DisplayFormat}}');
                        result = moment(val)
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateTimeConstants.ISO8601MomentJsFormat}}');
                    }
                """);

            public static readonly JRaw GetFormatterDisplay = new(
                            $$"""
                    function (val, field) {
                        if (val === undefined || val === null || val === '') return null;
                        var result = moment(val, '{{DateTimeExtensions.TimeOnlyConstants.DisplayFormat}}');
                        if(result._isValid) return result.format('{{DateTimeExtensions.TimeOnlyConstants.DisplayFormat}}');
                        result = moment(val)
                        if(result._isValid) return result.format('{{DateTimeExtensions.DateTimeConstants.ISO8601MomentJsFormat}}');
                    }
                """);

            public static readonly JRaw SetFormatter = new(
                $$"""
                    function (val, field) {
                        if (val === undefined || val === null || val === '') return;
                        var result = moment(val, '{{DateTimeExtensions.TimeOnlyConstants.DisplayFormat}}');
                        if(result._isValid) return result.toDate();
                        result = moment(val)
                        if(result._isValid) return result.toDate();
                    }
                """);

            public static readonly JRaw SetFormatterDisplay = new(
                            $$"""
                    function (val, field) {
                        if (val === undefined || val === null || val === '') return;
                        var result = moment(val, '{{DateTimeExtensions.TimeOnlyConstants.ISO8601MomentJsFormat}}');
                        if(result._isValid) return result.format('{{DateTimeExtensions.TimeOnlyConstants.DisplayFormat}}');
                        result = moment(val)
                        if(result._isValid) return result.format('{{DateTimeExtensions.TimeOnlyConstants.DisplayFormat}}');
                    }
                """);

            #endregion Public Fields
        }

        public static class Xml
        {
            #region Public Fields

            public static readonly Dictionary<string, object> DefaultAttributes = new() { { "type", "number" }, { "step", "0.01" } ,
                //{"style", "width:auto" },
                //{ "style", $"min-width:{DataTableColumnDefaults.Xml.Width}"  },
            };

            #endregion Public Fields
        }

        #endregion Public Classes
    }
}
