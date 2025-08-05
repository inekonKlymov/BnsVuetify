using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Bns.General.Domain.Common.Datatables.Front
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DataTableColumn(
        string? name,
        string? title,
        string? cellType = null,
        string? editField = null,
        string? className = null,
        string? data = null,
        string? defaultContent = null,
        string? footer = null,
        bool? orderable = null,
        uint[]? orderData = null,
        DataTableColumnOrderDataTypesEnum? orderDataType = null,
        OrderType[]? orderSequence = null,
        bool? searchable = null,
        DataTableColumnTypeEnum? typeProcessing = null,
        bool? visible = null,
        JRaw? width = null,
        JRaw? render = null,
        string? searchBuilderType = null,
        string? exportData = null)
    {
        public const string BoolActiveClass = "editor-bool-active";
        public const string EditableClass = "dtUserEditable";

        #region Private Fields

        private readonly DataTableColumnTypeEnum? _dataTableColumnTypeEnum = typeProcessing;
        private string _exportData = exportData;

        #endregion Private Fields

        #region Public Properties

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "searchBuilderType")]
        public string? SearchBuilderType { get; set; } = searchBuilderType;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "editField")]
        public string? EditField { get; set; } = editField;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cellType")]
        public string? CellType { get; set; } = cellType;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "className")]
        public string? ClassName { get; set; } = className;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "data")]
        public string? Data { get; set; } = data ?? name;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "defaultContent")]
        public string? DefaultContent { get; set; } = defaultContent;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "footer")]
        public string? Footer { get; set; } = footer;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name")]
        public string Name { get; set; } = name;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "orderable")]
        public bool? Orderable { get; set; } = orderable;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "orderData")]
        public uint[]? OrderData { get; set; } = orderData;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "orderDataType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DataTableColumnOrderDataTypesEnum? OrderDataType { get; set; } = orderDataType;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "orderSequence")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType[]? OrderSequence { get; set; } = orderSequence;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "searchable")]
        public bool? Searchable { get; set; } = searchable;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title")]
        public string Title { get; set; } = title;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DataTableColumnTypeEnum? Type { get; set; } = typeProcessing;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "visible")]
        public bool? Visible { get; set; } = visible;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "width")]
        public JRaw? Width { get; set; } = width;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "render")]
        public JRaw? Render { get; set; } = render;

        #endregion Public Properties

        #region Public Methods

        //public DataTableColumnTypeEnum? GetColumnType() => _dataTableColumnTypeEnum;

        public void FillEmptyPropertiesBasedOnRenderType(DataTableColumnFieldDescriptionRenderType renderColumnType, bool IsInputActive, int? experctedLength)
        {
            Data ??= Name;
            switch (renderColumnType)
            {
                case DataTableColumnFieldDescriptionRenderType.@bool:
                    SetBoolProperties(IsInputActive);
                    break;

                case DataTableColumnFieldDescriptionRenderType.@byte:
                    SetByteProperties();
                    break;

                case DataTableColumnFieldDescriptionRenderType.date:
                    SetDateProperties();
                    break;

                case DataTableColumnFieldDescriptionRenderType.dateTime:
                    SetDateTimeProperties();
                    break;

                case DataTableColumnFieldDescriptionRenderType.@decimal:
                    SetDecimalProperties();
                    break;

                case DataTableColumnFieldDescriptionRenderType.@double:
                    SetDoubleProperties();
                    break;

                case DataTableColumnFieldDescriptionRenderType.@int:
                    SetIntProperties();
                    break;

                case DataTableColumnFieldDescriptionRenderType.@string:
                    SetStringProperties(experctedLength);
                    break;

                case DataTableColumnFieldDescriptionRenderType.time:
                    SetTimeProperties();
                    break;

                case DataTableColumnFieldDescriptionRenderType.xml:
                    SetXmlProperties();
                    break;
            }
        }

        #endregion Public Methods

        private void SetXmlProperties()
        {
            Type ??= DataTableColumnTypeEnum.htmlUtf8;
            Render ??= DataTableColumnDefaults.Xml.Render();
            //Width ??= DataTableColumnDefaults.Xml.Width;
        }

        private void SetDoubleProperties()
        {
            Render ??= DataTableColumnDefaults.Decimal.Render();
            //Width ??= DataTableColumnDefaults.Double.Width;
        }

        private void SetDateTimeProperties()
        {
            Type ??= DataTableColumnTypeEnum.date;
            Render ??= DataTableColumnDefaults.DateTime.DefaultRender;
            SearchBuilderType ??= DataTableColumnDefaults.DateTime.SearchBuilderType;
            //Width ??= DataTableColumnDefaults.DateTime.Width;
        }

        private void SetDateProperties()
        {
            Type ??= DataTableColumnTypeEnum.date;
            Render ??= DataTableColumnDefaults.DateOnly.Render();
            //Width ??= DataTableColumnDefaults.DateOnly.Width;
        }

        private void SetTimeProperties()
        {
            Type ??= DataTableColumnTypeEnum.date;
            Render ??= DataTableColumnDefaults.TimeOnly.Render();
            //Width ??= DataTableColumnDefaults.TimeOnly.Width;
        }

        private void SetBoolProperties(bool IsInputActive)
        {
            //if (type is null) this.SetType(DataTableColumnTypeEnum.htmlNumber);
            Render ??= DataTableColumnDefaults.Bool.Render(!IsInputActive, Name, BoolActiveClass);
            Type ??= DataTableColumnTypeEnum.customBool;

            //Width ??= DataTableColumnDefaults.Bool.Width;
            //if (ClassName is null) SetClassName("dt-body-center");
        }

        private void SetDecimalProperties()
        {
            Render ??= DataTableColumnDefaults.Decimal.Render();
            //Width ??= DataTableColumnDefaults.Decimal.Width;
            //if (Width is null) SetWidth("300px");
            //if (Width is null) SetWidth("300em");
        }

        private void SetByteProperties()
        {
            //Width ??= DataTableColumnDefaults.Byte.Width;
            Render ??= DataTableColumnDefaults.Byte.Render();
        }

        private void SetIntProperties()
        {
            //Width ??= DataTableColumnDefaults.Int.Width;
            Render ??= DataTableColumnDefaults.Int.Render(_exportData);
        }

        private void SetStringProperties(int? experctedLength)
        {
            Type ??= DataTableColumnTypeEnum.htmlUtf8;
            int? cutoff = experctedLength.HasValue && experctedLength.Value > -1 && experctedLength.Value < DataTableColumnDefaults.String.CutoffDefault
                ? experctedLength.Value
                : null;
            Render ??= DataTableColumnDefaults.String.Render(cutoff, _exportData);
        }
    }

    public static class DataTableColumnExtensions
    {
        public static DataTableColumn WithEditField(this DataTableColumn column, string editField)
        { column.EditField = editField; return column; }

        public static DataTableColumn WithCellType(this DataTableColumn column, string cellType)
        { column.CellType = cellType; return column; }

        public static DataTableColumn WithClassName(this DataTableColumn column, string className)
        { column.ClassName = className; return column; }

        public static DataTableColumn AddClassName(this DataTableColumn column, string className)
        {
            if (string.IsNullOrEmpty(column.ClassName))
            {
                column.ClassName = className;
            }
            else
            {
                column.ClassName += " " + className;
            }
            return column;
        }

        public static DataTableColumn WithData(this DataTableColumn column, string data)
        { column.Data = data; return column; }

        public static DataTableColumn WithDefaultContent(this DataTableColumn column, string defaultContent)
        { column.DefaultContent = defaultContent; return column; }

        public static DataTableColumn WithFooter(this DataTableColumn column, string footer)
        { column.Footer = footer; return column; }

        public static DataTableColumn WithName(this DataTableColumn column, string name)
        { column.Name = name; return column; }

        public static DataTableColumn WithOrderable(this DataTableColumn column, bool orderable)
        { column.Orderable = orderable; return column; }

        public static DataTableColumn WithOrderData(this DataTableColumn column, uint[] orderData)
        { column.OrderData = orderData; return column; }

        public static DataTableColumn WithOrderDataType(this DataTableColumn column, DataTableColumnOrderDataTypesEnum orderDataType)
        { column.OrderDataType = orderDataType; return column; }

        public static DataTableColumn WithOrderSequence(this DataTableColumn column, OrderType[] orderSequence)
        { column.OrderSequence = orderSequence; return column; }

        public static DataTableColumn WithSearchable(this DataTableColumn column, bool searchable)
        { column.Searchable = searchable; return column; }

        public static DataTableColumn WithTitle(this DataTableColumn column, string title)
        { column.Title = title; return column; }

        public static DataTableColumn WithType(this DataTableColumn column, DataTableColumnTypeEnum type)
        { column.Type = type; return column; }

        public static DataTableColumn WithRender(this DataTableColumn column, JRaw render)
        { column.Render = render; return column; }

        public static DataTableColumn WithVisible(this DataTableColumn column, bool visible)
        { column.Visible = visible; return column; }

        public static DataTableColumn WithWidth(this DataTableColumn column, JRaw width)
        { column.Width = width; return column; }

        public static DataTableColumn FillDefaultRenderType(this DataTableColumn column, DataTableColumnFieldDescriptionRenderType type,
            bool IsInputActive = false, int? experctedLength = null)
        {
            column.FillEmptyPropertiesBasedOnRenderType(type, IsInputActive, experctedLength);
            return column;
        }
    }
}