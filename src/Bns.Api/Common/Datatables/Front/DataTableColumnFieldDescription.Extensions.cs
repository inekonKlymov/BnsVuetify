using Newtonsoft.Json.Linq;

namespace Bns.General.Domain.Common.Datatables.Front
{
    public static class DataTableColumnFieldDescriptionExtensions
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
