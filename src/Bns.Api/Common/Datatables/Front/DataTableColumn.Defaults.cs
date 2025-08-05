using Bns.Api.Common.Datatables;
using Newtonsoft.Json.Linq;

namespace Bns.General.Domain.Common.Datatables.Front
{
    public static class DataTableColumnDefaults
    {
        private const string CutoffWidth = "0.6em + 3em";

        #region Public Classes

        public static class Bool
        {
            #region Public Fields

            public const string Width = "100px";

            #endregion Public Fields

            #region Public Methods

            public static JRaw Render(bool disabled = false, string Name = "", string addClass = "", string? exportData = null) => new(
                            $$"""
                function (data, type, row) {
                    return type === 'display'
                    ? `<input type="checkbox" name="{{Name}}" class="{{addClass}} form-check-input" {{(disabled ? "disabled" : "")}} ${(data == "True"|| data == true ? "checked" : "")}>`
                    {{(string.IsNullOrEmpty(exportData) ? "" : $": type === 'export' ? row.{exportData}")}}
                    : data
                }
                """);

            #endregion Public Methods
        }

        public static class Byte
        {
            #region Public Fields

            public static JRaw Render(string? exportData = null) => new($$"""
                function(data, type, row){
                    if (!data) return type === 'sort' || type === 'type' ? 0 : data;
                    var str = data.toString();
                    return type === 'display'
                        ?`<div style="min-width:calc(${str.length} * {{CutoffWidth}})">${str}</div>`
                        {{(string.IsNullOrEmpty(exportData) ? "" : $": type === 'export' ? row.{exportData}")}}
                        : data
                }
                """);

            public const string Width = "50px";

            #endregion Public Fields
        }

        public static class DateOnly
        {
            #region Public Fields

            public static JRaw Render(string? exportData = null) => new($$"""
                function(data, type, row){
                    if (!data) return type === 'sort' || type === 'type' ? 0 : "";
                    return type === 'display'
                        ?`<div style="min-width:{{Width}}">${moment(data).format('{{DateTimeExtensions.DateOnlyConstants.DisplayFormat}}')}</div>`
                        {{(string.IsNullOrEmpty(exportData) ? "" : $": type === 'export' ? row.{exportData}")}}
                        : moment(data).format(type === 'sort' || type === 'type' ? 'x' : '{{DateTimeExtensions.DateOnlyConstants.DisplayFormat}}')
                }
                """);

            public static readonly JRaw DefaultRender = new($"""$.fn.dataTable.render.datetime('{DateTimeExtensions.DateOnlyConstants.DisplayFormat}')""");

            public const string Width = "100px";

            #endregion Public Fields
        }

        public static class DateTime
        {
            #region Public Fields

            public static JRaw Render(string? exportData = null) => new($$"""
                function(data, type, row){
                    if (!data) return type === 'sort' || type === 'type' ? 0 : "";
                    return type === 'display'
                        ?`<div style="min-width:{{Width}}">${moment(data).format('{{DateTimeExtensions.DateTimeConstants.DisplayFormat}}')}</div>`
                        {{(string.IsNullOrEmpty(exportData) ? "" : $": type === 'export' ? row.{exportData}")}}
                        : moment(data).format(type === 'sort' || type === 'type' ? 'x' : '{{DateTimeExtensions.DateTimeConstants.DisplayFormat}}')
                }
                """);

            public static readonly JRaw DefaultRender = new($$"""$.fn.dataTable.render.datetime('{{DateTimeExtensions.DateTimeConstants.DisplayFormat}}')""");

            public const string Width = "180px";
            public const string SearchBuilderType = $"moment-{DateTimeExtensions.DateTimeConstants.DisplayFormat}";

            public static DataTableColumn Generate(string name, string title, string? data = null)
            {
                return new DataTableColumn(name, title, data: data, orderable: true, searchable: true, render: DefaultRender, searchBuilderType: SearchBuilderType);
            }

            #endregion Public Fields
        }

        public static class Debug
        {
            #region Public Fields

            public const string Width = "100px";

            public static JRaw Render = new($$"""
                        function (data, type, row) {
                            debugger;
                            return data
                        }
                        """);

            #endregion Public Fields
        }

        public static class Decimal
        {
            #region Public Fields

            public static JRaw Render(string? exportData = null) => new($$"""
                function(data, type, row){
                    if (!data) return type === 'sort' || type === 'type' ? 0 : data;
                    var str = data.toString();
                    return type === 'display'
                        ?`<div style="min-width:calc(${str.length} * {{CutoffWidth}})">${str}</div>`
                        {{(string.IsNullOrEmpty(exportData) ? "" : $": type === 'export' ? row.{exportData}")}}
                        : data
                }
                """);

            public const string Width = "70px";

            #endregion Public Fields
        }

        public static class Double
        {
            #region Public Fields

            public static JRaw Render(string? exportData = null) => new($$"""
                function(data, type, row){
                    if (!data) return type === 'sort' || type === 'type' ? 0 : data;
                    var str = data.toString();
                    return type === 'display'
                        ?`<div style="min-width:calc(${str.length} * {{CutoffWidth}})">${str}</div>`
                        {{(string.IsNullOrEmpty(exportData) ? "" : $": type === 'export' ? row.{exportData}")}}
                        : data
                }
                """);

            public const string Width = "70px";

            #endregion Public Fields
        }

        public static class Int
        {
            #region Public Fields

            public static JRaw Render(string? exportData = null) => new($$"""
                function(data, type, row){
                    if (!data) return type === 'sort' || type === 'type' ? 0 : data;
                    var str = data.toString();
                return type === 'display'
                        ?`<div style="min-width:calc(${str.length} * {{CutoffWidth}})">${str}</div>`
                        {{(string.IsNullOrEmpty(exportData) ? "" : $": type === 'export' ? row.{exportData}")}}
                        : data
                }
                """);

            public const string Width = "50px";

            #endregion Public Fields
        }

        public static class Predefined
        {
            #region Public Fields

            public static class Render
            {
                public static JRaw LinkByRow(string rowHref, string rowData = "", string icon = "", string tooltip = "")
                {
                    string tooltipPart = string.IsNullOrEmpty(tooltip)
                        ? string.Empty
                        : $""" data-bs-toggle="tooltip" data-bs-title="{tooltip}" """;
                    string textPart = string.IsNullOrEmpty(rowData)
                        ? string.Empty
                        : $$"""${row.{{rowData}}}""";
                    string iconPart = string.IsNullOrEmpty(icon)
                        ? string.Empty
                        : $"""i class="{icon} {(string.IsNullOrEmpty(textPart) ? string.Empty : "me-2")}"></i>""";
                    return new($$"""
                        function (data, type, row) {
                            if (!row.{{rowHref}}) return "";
                            if(row.{{rowHref}}){
                                return `<a class="cursor-pointer text-secondary-hover" {{tooltipPart}} href="${row.{{rowHref}}}">{{iconPart}}{{textPart}} </a>`
                            }
                        }
                    """);
                }

                public static JRaw LinkByData(string urlAlias, string icon = "", string tooltip = "", bool textInclude = true)
                {
                    string tooltipPart = string.IsNullOrEmpty(tooltip) ? string.Empty : $""" data-bs-toggle="tooltip" data-bs-title="{tooltip}" """;
                    string textPart = textInclude ? "${data}" : string.Empty;
                    string iconPart = string.IsNullOrEmpty(icon) ? string.Empty : $"""<i class="{icon} {(string.IsNullOrEmpty(textPart) ? string.Empty : "me-2")}"></i>""";
                    return new($$"""
                        function (data, type, row) {
                            if (!data) return "";
                            return type === 'display'
                                ?`<a class="cursor-pointer text-secondary-hover" {{tooltipPart}} href="${row.{{urlAlias}}}">{{iconPart}}{{textPart}} </a>`
                                : data
                        }
                    """);
                }
            }

            public static readonly DataTableColumn EditColumn = new(
                name: null,
                title: null,
                data: null,
                className: "row-edit dt-center",
                orderable: false
            );

            public static readonly DataTableColumn Select = new(
                name: null,
                title: null,
                data: null,
                orderable: false,
                searchable: false,
                className: "dt-body-center p-0",
                render: new JRaw("$.fn.dataTable.render.select()"),
                width: new("30")
            );

            public static readonly DataTableColumn Control = new(
                data: null,
                title: null,
                name: null,
                className: "dt-control",
                orderable: false,
                searchable: false,
                defaultContent: "",
                width: new("'2em'")
            );

            public static readonly DataTableColumn Reorder = new(
                data: null,
                title: "<i class=\"fa-solid fa-lg fa-bars\"></i>",
                name: null,
                //className: "dt-control",
                orderable: false,
                searchable: false,
                defaultContent: "",
                render: new JRaw("""
                    function () {
                        return `<i class="fa-solid fa-lg fa-bars cursor-grab"></i>`
                    }
                    """),
                width: new("30")
            );

            #endregion Public Fields
        }

        public static class String
        {
            public static JRaw Render(int? cutoff, string? exportData = null) => new($$"""
                function(data, type, row){
                    if (!data) return data;
                    debugger;
                    return type === 'display'
                        ?`<div style="min-width:calc({{(cutoff.HasValue ? cutoff.Value : CutoffMinDefault)}}*{{CutoffWidth}} ) ; max-width:calc({{(cutoff.HasValue ? cutoff.Value : CutoffDefault)}} *{{CutoffWidth}} )">${data}</div>`
                        {{(string.IsNullOrEmpty(exportData) ? "" : $": type === 'export' ? row.{exportData}")}}
                        : data
                }
                """);

            #region Public Fields

            public const string Width = "100px";
            public const int CutoffDefault = 30;
            public const int CutoffMinDefault = 10;

            #endregion Public Fields
        }

        public static class TimeOnly
        {
            #region Public Fields

            public static JRaw Render(string? exportData = null) => new($$"""
                function(data, type, row){
                    if (!data) return type === 'sort' || type === 'type' ? 0 : "";
                    return type === 'display'
                        ?`<div style="min-width:{{Width}}">${moment(data,'{{DateTimeExtensions.TimeOnlyConstants.ISO8601MomentJsFormat}}').format('{{DateTimeExtensions.TimeOnlyConstants.ISO8601MomentJsFormat}}')}</div>`
                        {{(string.IsNullOrEmpty(exportData) ? "" : $": type === 'export' ? row.{exportData}")}}
                        : moment(data,'{{DateTimeExtensions.TimeOnlyConstants.ISO8601MomentJsFormat}}').format(type === 'sort' || type === 'type' ? 'x' : '{{DateTimeExtensions.TimeOnlyConstants.ISO8601MomentJsFormat}}')
                }
                """);

            public static readonly JRaw DefaultRender = new($$"""$.fn.dataTable.render.datetime('{{DateTimeExtensions.DateTimeConstants.DisplayFormat}}')""");
            public const string Width = "100px";

            #endregion Public Fields
        }

        public static class Xml
        {
            #region Public Fields

            public static JRaw Render(string? exportData = null) => new($$"""
                        function (data, type, row) {
                            if (!data) return data;
                            return type === 'display'
                            ? `<textarea  style="min-width:{{Width}}" class="form-control" readonly>${data}</textarea>`
                                {{(string.IsNullOrEmpty(exportData) ? "" : $": type === 'export' ? row.{exportData}")}}
                            : data
                        }
                        """);

            public const string Width = "500px";

            #endregion Public Fields
        }

        #endregion Public Classes
    }
}