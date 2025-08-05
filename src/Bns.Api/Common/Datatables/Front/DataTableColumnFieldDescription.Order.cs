namespace Bns.General.Domain.Common.Datatables.Front
{
    public class DataTableColumnFieldDescriptionOrder(int index = -1, OrderType direction = OrderType.Asc)
    {
        public int Index { get; init; } = index;

        public string Direction { get; init; } = Enum.GetName(direction)?.ToLower();

        public enum ColumnOrder
        {
            asc, desc
        }
    }
}
