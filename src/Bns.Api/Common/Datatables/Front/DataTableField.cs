using Bns.Api.Common.Datatables;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Bns.General.Domain.Common.Datatables.Front
{
    public class DataTableField(
        string name,
        string label,
        JRaw? data = null,
        JRaw? def = null,
        string? className = null,
        bool? EntityDecode = null,
        string? fieldInfo = null,
        string? id = null,
        string? labelInfo = null,
        string? message = null,
        bool? MultiEditable = null,
        bool? NullDefault = null,
        Dictionary<string, string>? options = null,
        Dictionary<string, string>? optionsPair = null,
        bool? Submit = null,
        DataTableFieldTypeEnum? type = null,
        JRaw? Compare = null,
        JRaw? getFormatter = null,
        JRaw? setFormatter = null,
        DataTableEditorFieldDateTimeOptions? datetimeOptions = null,
        DataTableEditorFieldCheckboxOptions? checkboxOptions = null,
        Dictionary<string, object>? attribute = null
        )
    {
        #region Private Fields

        private DataTableFieldTypeEnum? _dataTableColumnFieldTypeEnum = type;

        #endregion Private Fields

        #region Public Properties

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "className")]
        public string? ClassName { get; set; } = className;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "compare")]
        public JRaw? Compare { get; set; } = Compare;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "data")]
        public JRaw? Data { get; set; } = data;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "def")]
        public JRaw? Default { get; set; } = def;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "entityDecode")]
        public bool? EntityDecode { get; set; } = EntityDecode;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "fieldInfo")]
        public string? FieldInfo { get; set; } = fieldInfo;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "getFormatter")]
        public JRaw? GetFormatter { get; set; } = getFormatter;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id")]
        public string? Id { get; set; } = id;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "label")]
        public string Label { get; set; } = label;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "labelInfo")]
        public string? LabelInfo { get; set; } = labelInfo;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "message")]
        public string? Message { get; set; } = message;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "multiEditable")]
        public bool? MultiEditable { get; set; } = MultiEditable;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name")]
        public string Name { get; set; } = name;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "nullDefault")]
        public bool? NullDefault { get; set; } = NullDefault;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "options")]
        public Dictionary<string, string>? Options { get; set; } = options is not null ? options : checkboxOptions?.options;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "optionsPair")]
        public Dictionary<string, string>? OptionsPair { get; set; } = optionsPair;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "setFormatter")]
        public JRaw? SetFormatter { get; set; } = setFormatter;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "submit")]
        public bool? Submit { get; set; } = Submit;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DataTableFieldTypeEnum? Type { get; set; } = type;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "attr")]
        public Dictionary<string, object>? Attribute { get; set; } = attribute;

        #region datetime type

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "displayFormat")]
        public string? DisplayFormat { get; set; } = datetimeOptions?.DisplayFormat;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "format")]
        public string? Format { get; set; } = datetimeOptions?.Format;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "keyInput")]
        public bool? KeyInput { get; set; } = datetimeOptions?.KeyInput;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "opts")]
        public DataTableEditorFieldDateTimeOptsOptions? Opts { get; set; } = datetimeOptions?.Opts;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "wireFormat")]
        public string? WireFormat { get; set; } = datetimeOptions?.WireFormat;

        #endregion datetime type

        #region Checkbox type

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "separator")]
        public string? Separator { get; set; } = checkboxOptions?.separator;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "unselectedValue")]
        public string? UnselectedValue { get; set; } = checkboxOptions?.unselectedValue;

        #endregion Checkbox type

        #endregion Public Properties

        #region Public Methods

        public void FillEmptyPropertiesBasedOnRenderType(DataTableColumnFieldDescriptionRenderType renderColumnType)
        {
            Data ??= new($"\"{Name}\"");
            switch (renderColumnType)
            {
                case DataTableColumnFieldDescriptionRenderType.@bool:
                    SetBoolProperties();
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
                    SetStringProperties();
                    break;

                case DataTableColumnFieldDescriptionRenderType.time:
                    SetTimeProperties();
                    break;

                case DataTableColumnFieldDescriptionRenderType.xml:
                    SetXmlProperties();
                    break;
            }
        }

        public DataTableFieldTypeEnum? GetDataTableColumnField() => _dataTableColumnFieldTypeEnum;

        private void SetBoolProperties()
        {
            Type ??= DataTableFieldTypeEnum.checkbox;
            Separator ??= DataTableFieldDefaults.Bool.Options.separator;
            Options ??= DataTableFieldDefaults.Bool.Options.options;
            GetFormatter ??= DataTableFieldDefaults.Bool.GetFormatter;
            SetFormatter ??= DataTableFieldDefaults.Bool.SetFormatter;
        }

        private void SetByteProperties()
        {
            Type ??= DataTableFieldTypeEnum.text;
            this.AddDefaultAttributes(DataTableFieldDefaults.Int.DefaultAttributes);
        }

        private void SetDateProperties()
        {
            Type ??= DataTableFieldTypeEnum.datetime;
            if (Type == DataTableFieldTypeEnum.display)
            {
                SetFormatter ??= DataTableFieldDefaults.Date.SetFormatterDisplay;
                GetFormatter ??= DataTableFieldDefaults.Date.GetFormatterDisplay;
            }
            else
            {
                SetFormatter ??= DataTableFieldDefaults.Date.SetFormatter;
                GetFormatter ??= DataTableFieldDefaults.Date.GetFormatter;
            }
            Opts ??= DataTableFieldDefaults.Date.Opts;
            DisplayFormat ??= DateTimeExtensions.DateOnlyConstants.DisplayFormat;
            this.AddDefaultAttributes(DataTableFieldDefaults.Date.DefaultAttributes);
        }

        private void SetDateTimeProperties()
        {
            Type ??= DataTableFieldTypeEnum.datetime;
            if (Type == DataTableFieldTypeEnum.display)
            {
                SetFormatter ??= DataTableFieldDefaults.DateTime.SetFormatterDisplay;
                GetFormatter ??= DataTableFieldDefaults.DateTime.GetFormatterDisplay;
            }
            else
            {
                SetFormatter ??= DataTableFieldDefaults.DateTime.SetFormatter;
                GetFormatter ??= DataTableFieldDefaults.DateTime.GetFormatter;
            }
            DisplayFormat ??= DateTimeExtensions.DateTimeConstants.DisplayFormat;
            Opts ??= DataTableFieldDefaults.DateTime.Opts;
            this.AddDefaultAttributes(DataTableFieldDefaults.DateTime.DefaultAttributes);
        }

        private void SetDecimalProperties()
        {
            Type ??= DataTableFieldTypeEnum.text;
            this.AddDefaultAttributes(DataTableFieldDefaults.Decimal.DefaultAttributes);
        }

        private void SetDoubleProperties()
        {
            Type ??= DataTableFieldTypeEnum.text;
            this.AddDefaultAttributes(DataTableFieldDefaults.Decimal.DefaultAttributes);
        }

        private void SetIntProperties()
        {
            Type ??= DataTableFieldTypeEnum.text;
            this.AddDefaultAttributes(DataTableFieldDefaults.Int.DefaultAttributes);
        }

        private void SetStringProperties()
        {
            Type ??= DataTableFieldTypeEnum.text;
            this.AddDefaultAttributes(DataTableFieldDefaults.String.DefaultAttributes);
        }

        private void SetTimeProperties()
        {
            Type ??= DataTableFieldTypeEnum.datetime;

            if (Type == DataTableFieldTypeEnum.display)
            {
                SetFormatter ??= DataTableFieldDefaults.Time.SetFormatterDisplay;
                GetFormatter ??= DataTableFieldDefaults.Time.GetFormatterDisplay;
            }
            else
            {
                SetFormatter ??= DataTableFieldDefaults.Time.SetFormatter;
                GetFormatter ??= DataTableFieldDefaults.Time.GetFormatter;
            }
            DisplayFormat ??= DateTimeExtensions.TimeOnlyConstants.DisplayFormat;
            Opts ??= new(firstDay: 1);

            this.AddDefaultAttributes(DataTableFieldDefaults.Time.DefaultAttributes);
        }

        private void SetXmlProperties()
        {
            Type ??= DataTableFieldTypeEnum.textarea;
            //GetFormatter ??= (DataTableFrontFieldDefaults.Debug.GetFormatter);
            //SetFormatter ??= (DataTableFrontFieldDefaults.Debug.SetFormatter);
            //Data ??= DataTableFrontFieldDefaults.Debug.Data(Name);
        }

        #endregion Public Methods
    }

    public static class DataTableFrontEditorFieldExtensions
    {
        public static DataTableField WithClassName(this DataTableField column, string className)
        { column.ClassName = className; return column; }

        public static DataTableField WithCompare(this DataTableField column, JRaw value)
        { column.Compare = value; return column; }

        public static DataTableField WithData(this DataTableField column, string data)
        { column.Data = new(data); return column; }

        public static DataTableField WithData(this DataTableField column, JRaw data)
        { column.Data = data; return column; }

        public static DataTableField WithData(this DataTableField column, string data, bool IsName = true)
        {
            column.Data = IsName ? new($"\"{data}\"") : new(data);
            return column;
        }

        public static DataTableField SetDateTimeOptions(this DataTableField column, DataTableEditorFieldDateTimeOptions opts)
        {
            column.DisplayFormat = opts.DisplayFormat;
            column.Format = opts.Format;
            column.KeyInput = opts.KeyInput;
            column.Format = opts.Format;
            column.Opts = opts.Opts;
            column.WireFormat = opts.WireFormat;
            return column;
        }

        public static DataTableField WithDef(this DataTableField column, string def)
        { column.Default = new JRaw(def); return column; }

        public static DataTableField WithDef(this DataTableField column, JRaw def)
        { column.Default = def; return column; }

        public static DataTableField WithEntityDecode(this DataTableField column, bool entityDecode)
        { column.EntityDecode = entityDecode; return column; }

        public static DataTableField WithFieldInfo(this DataTableField column, string fieldInfo)
        { column.FieldInfo = fieldInfo; return column; }

        public static DataTableField WithGetFormatter(this DataTableField column, JRaw value)
        { column.GetFormatter = value; return column; }

        public static DataTableField WithId(this DataTableField column, string id)
        { column.Id = id; return column; }

        public static DataTableField WithLabel(this DataTableField column, string label)
        { column.Label = label; return column; }

        public static DataTableField WithLabelInfo(this DataTableField column, string labelInfo)
        { column.LabelInfo = labelInfo; return column; }

        public static DataTableField WithMessage(this DataTableField column, string message)
        { column.Message = message; return column; }

        public static DataTableField WithMultiEditable(this DataTableField column, bool multiEditable)
        { column.MultiEditable = multiEditable; return column; }

        public static DataTableField WithName(this DataTableField column, string name)
        { column.Name = name; return column; }

        public static DataTableField WithNullDefault(this DataTableField column, bool nullDefault)
        { column.NullDefault = nullDefault; return column; }

        public static DataTableField WithOptions(this DataTableField column, Dictionary<string, string> options)
        { column.Options = options; return column; }

        public static DataTableField WithOptionsPair(this DataTableField column, Dictionary<string, string> optionsPair)
        { column.OptionsPair = optionsPair; return column; }

        public static DataTableField WithSetFormatter(this DataTableField column, JRaw value)
        { column.SetFormatter = value; return column; }

        public static DataTableField WithSubmit(this DataTableField column, bool submit)
        { column.Submit = submit; return column; }

        public static DataTableField WithType(this DataTableField column, DataTableFieldTypeEnum type)
        { column.Type = type; return column; }

        public static DataTableField AddDefaultAttributes(this DataTableField column, Dictionary<string, object> DefaultAttributes)
        {
            if (column.Attribute is null)
            {
                column.Attribute = DefaultAttributes;
            }
            else
            {
                foreach (var attr in DefaultAttributes)
                {
                    column.Attribute.TryAdd(attr.Key, attr.Value);
                }
            }
            return column;
        }

        public static DataTableField SetBoolOptions(this DataTableField column, DataTableEditorFieldCheckboxOptions opts)
        {
            column.Separator = opts.separator;
            column.UnselectedValue = opts.unselectedValue;
            column.Options = opts.options;
            column.WithGetFormatter(DataTableFieldDefaults.Bool.GetFormatter);
            column.WithSetFormatter(DataTableFieldDefaults.Bool.SetFormatter);
            column.WithType(DataTableFieldTypeEnum.checkbox);
            return column;
        }
    }

    public class DataTableEditorFieldDateTimeOptions(
        string? displayFormat = null,
        string? format = null,
        bool? keyInput = null,
        DataTableEditorFieldDateTimeOptsOptions? opts = null,
        string? wireFormat = null)
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "displayFormat")]
        public string? DisplayFormat { get; private set; } = displayFormat;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "format")]
        public string? Format { get; private set; } = format;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "keyInput")]
        public bool? KeyInput { get; private set; } = keyInput;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "opts")]
        public DataTableEditorFieldDateTimeOptsOptions? Opts { get; private set; } = opts;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "wireFormat")]
        public string? WireFormat { get; private set; } = wireFormat;
    }

    public class DataTableEditorFieldDateTimeOptsOptions(
        DataTableEditorFieldDateTimeOptionsButtons? buttons = null,
        uint[]? disableDays = null,
        uint? firstDay = null,
        string? format = null,
        uint[]? hoursAvailable = null,
        DataTableEditorFieldDateTimeOptionsI18? i18 = null,
        string? locale = null,
        JRaw? maxDate = null,
        JRaw? minDate = null,
        uint[]? minutesAvailable = null,
        uint[]? secondsAvailable = null,
        bool? showWeekNumber = null,
        uint? yearRange = null)
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "buttons")]
        public DataTableEditorFieldDateTimeOptionsButtons? buttons { get; private set; } = buttons;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "disableDays")]
        public uint[]? DisableDays { get; private set; } = disableDays;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "firstDay")]
        public uint? FirstDay { get; private set; } = firstDay;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "format")]
        public string? Format { get; private set; } = format;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "hoursAvailable")]
        public uint[]? HoursAvailable { get; private set; } = hoursAvailable;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "i18")]
        public DataTableEditorFieldDateTimeOptionsI18? I18 { get; private set; } = i18;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "locale")]
        public string? Locale { get; private set; } = locale;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "maxDate")]
        public JRaw? MaxDate { get; private set; } = maxDate;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "minDate")]
        public JRaw? MinDate { get; private set; } = minDate;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "minutesAvailable")]
        public uint[]? MinutesAvailable { get; private set; } = minutesAvailable;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "secondsAvailable")]
        public uint[]? SecondsAvailable { get; private set; } = secondsAvailable;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "showWeekNumber")]
        public bool? ShowWeekNumber { get; private set; } = showWeekNumber;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "yearRange")]
        public uint? YearRange { get; private set; } = yearRange;
    }

    public class DataTableEditorFieldDateTimeOptionsButtons(
        bool? clear = null,
        bool? today = null)
    {
        public bool? clear { get; private set; } = clear;
        public bool? today { get; private set; } = today;
    }

    public class DataTableEditorFieldDateTimeOptionsI18(
                string[]? amPm = null,
                string? clear = null,
                string? hours = null,
                string? minutes = null,
                string? months = null,
                string? next = null,
                string? previous = null,
                string? seconds = null,
                string? today = null,
                string? unknown = null,
                string[]? weekdays = null)
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "amPm")]
        public string[]? AmPm { get; private set; } = amPm;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "clear")]
        public string? Clear { get; private set; } = clear;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "hours")]
        public string? Hours { get; private set; } = hours;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "minutes")]
        public string? Minutes { get; private set; } = minutes;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "months")]
        public string? Months { get; private set; } = months;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "next")]
        public string? Next { get; private set; } = next;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "previous")]
        public string? Previous { get; private set; } = previous;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "seconds")]
        public string? Seconds { get; private set; } = seconds;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "today")]
        public string? Today { get; private set; } = today;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "unknown")]
        public string? Unknown { get; private set; } = unknown;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "weekdays")]
        public string[]? Weekdays { get; private set; } = weekdays;
    }

    public class DataTableEditorFieldCheckboxOptions(Dictionary<string, string>? options = null, string? separator = null, string? unselectedValue = null)
    {
        public Dictionary<string, string>? options { get; private set; } = options;
        public string? separator { get; private set; } = separator;
        public string? unselectedValue { get; private set; } = unselectedValue;
    }
}
