using System.Runtime.Serialization;

namespace Bns.General.Domain.Common.Datatables.Front
{
    public enum DataTableColumnOrderDataTypesEnum
    {
        [EnumMember(Value = "dom-text")]
        text,

        [EnumMember(Value = "dom-select")]
        select,

        [EnumMember(Value = "dom-checkbox")]
        checkbox
    }
}
