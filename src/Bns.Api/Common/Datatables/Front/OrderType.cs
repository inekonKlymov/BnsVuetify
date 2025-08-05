using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bns.General.Domain.Common.Datatables.Front
{
    public enum OrderType
    {
        //[Display(ResourceType = typeof(FormStrings), Name = nameof(FormStrings.Ascending))]
        [EnumMember(Value = "asc")]
        Asc,

        //[Display(ResourceType = typeof(FormStrings), Name = nameof(FormStrings.Descending))]
        [EnumMember(Value = "desc")]
        Desc
    }
}