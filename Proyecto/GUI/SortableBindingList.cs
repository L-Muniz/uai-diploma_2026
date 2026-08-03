using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GUI
{
    /// <summary>
    /// BindingList con soporte de ordenamiento para DataGridView (clic en encabezados).
    /// </summary>
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool isSorted;
        private ListSortDirection sortDirection = ListSortDirection.Ascending;
        private PropertyDescriptor sortProperty;

        public SortableBindingList()
        {
        }

        public SortableBindingList(IList<T> list) : base(list)
        {
        }

        protected override bool SupportsSortingCore => true;
        protected override bool IsSortedCore => isSorted;
        protected override PropertyDescriptor SortPropertyCore => sortProperty;
        protected override ListSortDirection SortDirectionCore => sortDirection;

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            var items = (List<T>)Items;
            items.Sort((a, b) =>
            {
                object valueA = prop.GetValue(a);
                object valueB = prop.GetValue(b);

                int result;
                if (valueA == null && valueB == null) result = 0;
                else if (valueA == null) result = -1;
                else if (valueB == null) result = 1;
                else if (valueA is IComparable comparable)
                    result = comparable.CompareTo(valueB);
                else
                    result = string.Compare(valueA.ToString(), valueB.ToString(), StringComparison.CurrentCultureIgnoreCase);

                return direction == ListSortDirection.Ascending ? result : -result;
            });

            sortProperty = prop;
            sortDirection = direction;
            isSorted = true;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            isSorted = false;
            sortProperty = null;
        }
    }
}
