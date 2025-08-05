using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bns.General.Domain.Common.Datatables.Front
{
    public class DataTablesFrontOptions
    {
        public class Layout
        {
            public class Pdf
            {
                public const string Text = "PDF";
            }

            public class Csv
            {
                public const string Text = "Csv";
            }
        }
    }

    public class DataTableColumnFieldDescription
    {
        #region Private Fields

        public DataTableColumnFieldDescriptionRenderType RenderColumnType { get; private set; }

        #endregion Private Fields

        #region Public Properties

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataTableField? Field { get; private set; } = null;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataTableColumn Column { get; private set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataTableColumnFieldDescriptionOrder Order { get; private set; }

        public int ColumnIndex { get; private set; } = 0;
        public bool IsPrimaryColumn { get; private set; } = false;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DataTableColumnFieldDescriptionSettings ExtensionsSettings { get; private set; }

        #endregion Public Properties

        #region Private Constructors

        private DataTableColumnFieldDescription(DataTableColumn column, DataTableField? field, DataTableColumnFieldDescriptionRenderType renderColumnType, DataTableColumnFieldDescriptionOrder? order, int ColumnIndex, bool isPrimaryColumn, DataTableColumnFieldDescriptionSettings ExtensionsSettings)
        {
            Column = column;
            if (field is not null) Field = field;
            //FilterType = filterType;
            RenderColumnType = renderColumnType;
            Order = order is null ? new() : order;
            this.ColumnIndex = ColumnIndex;
            IsPrimaryColumn = isPrimaryColumn;
            this.ExtensionsSettings = ExtensionsSettings;
        }

        #endregion Private Constructors

        #region Public Methods

        public void SetField(DataTableField field) => Field = field;

        public static DataTableColumnFieldDescription Create(
            DataTableColumn column,
            DataTableField? field = null,
            DataTableColumnFieldDescriptionRenderType renderColumnType = DataTableColumnFieldDescriptionRenderType.@string,
            DataTableColumnFieldDescriptionOrder? order = null,
            int ColumnIndex = 0,
            bool IsPrimaryColumn = false,
            bool IsInputActive = false,
            DataTableColumnFieldDescriptionSettings ExtensionsSettings = null,
            int? experctedLength = null)
        {
            if(column is not null)
            {
                column.FillEmptyPropertiesBasedOnRenderType(renderColumnType, IsInputActive, experctedLength);
                if (IsPrimaryColumn)
                {
                    //column.WithCellType("th");
                    column.AddClassName("fw-bold");
                    //column.AddClassName("fs-6");
                }
            }
            if (field is not null)
            {
                field.FillEmptyPropertiesBasedOnRenderType(renderColumnType);
                if (field.Type != DataTableFieldTypeEnum.hidden && field.Type != DataTableFieldTypeEnum.@readonly)
                {
                    column.AddClassName(DataTableColumn.EditableClass);
                }
            }
            ExtensionsSettings ??= new();
            return new(column, field, renderColumnType, order, ColumnIndex, IsPrimaryColumn, ExtensionsSettings);
        }

        public static DataTableColumnFieldDescription Create(
            string name,
            string title,
            string? cellType = null,
            string? className = null,
            string? data = null,
            string? defaultContent = null,
            string? footer = null,
            bool? orderable = null,
            uint[]? orderData = null,
            DataTableColumnOrderDataTypesEnum? orderDataType = null,
            OrderType[]? orderSequence = null,
            bool? searchable = null,
            DataTableColumnTypeEnum? type = null,
            bool? visible = null,
            JRaw? width = null,
            DataTableColumnFieldDescriptionRenderType renderColumnType = DataTableColumnFieldDescriptionRenderType.@string, DataTableColumnFieldDescriptionOrder? Order = null, int ColumnIndex = 0, bool IsPrimaryColumn = false, DataTableColumnFieldDescriptionSettings ExtensionsSettings = null)
        {
            DataTableColumn options = new(
                name: name,
                title: title,
                cellType: cellType,
                className: className,
                data: data is null ? name : data,
                defaultContent: defaultContent,
                footer: footer,
                orderable: orderable,
                orderData: orderData,
                orderDataType: orderDataType,
                orderSequence: orderSequence,
                searchable: searchable,
                typeProcessing: type,
                visible: visible,
                width: width);
            ExtensionsSettings ??= new();
            return new DataTableColumnFieldDescription(options, field: null, renderColumnType, Order, ColumnIndex, IsPrimaryColumn, ExtensionsSettings);
        }

        #endregion Public Methods
    }

    public record DataTablesColumnExtensionsSettings(bool searchBuilder = false);

    public static class DataTablesColumnExtensions
    {
        public static DataTableColumnFieldDescription WithEditorField(this DataTableColumnFieldDescription column, DataTableField field)
        {
            column.SetField(field);
            return column;
        }

        public static DataTableColumnFieldDescription WithEditorField(
            this DataTableColumnFieldDescription column,
            string name,
            string label,
            JRaw? data = null,
            JRaw? def = null,
            string? className = null,
            bool? entityDecode = null,
            string? fieldInfo = null,
            string? id = null,
            string? labelInfo = null,
            string? message = null,
            bool? multiEditable = null,
            bool? nullDefault = null,
            Dictionary<string, string>? options = null,
            Dictionary<string, string>? optionsPair = null,
            bool? submit = null,
            DataTableFieldTypeEnum? type = null
        )
        {
            column.SetField(new(
                name: name,
                label: label,
                data: data,
                def: def,
                className: className,
                EntityDecode: entityDecode,
                fieldInfo: fieldInfo,
                id: id,
                labelInfo: labelInfo,
                message: message,
                MultiEditable: multiEditable,
                NullDefault: nullDefault,
                options: options,
                optionsPair: optionsPair,
                Submit: submit,
                type: type
                ));
            return column;
        }
    }
}
