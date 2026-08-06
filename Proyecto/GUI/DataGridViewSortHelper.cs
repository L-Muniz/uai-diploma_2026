using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public static class DataGridViewSortHelper
    {
        public static SortableBindingList<T> Bind<T>(DataGridView dgv, IEnumerable<T> data)
        {
            var list = data as List<T> ?? data?.ToList() ?? new List<T>();
            var sortable = new SortableBindingList<T>(list);

            dgv.DataSource = null;
            dgv.DataSource = sortable;
            HabilitarOrdenamiento(dgv);
            UITheme.EstilizarGrid(dgv);
            return sortable;
        }

        public static void HabilitarOrdenamiento(DataGridView dgv)
        {
            if (dgv == null)
                return;

            foreach (DataGridViewColumn col in dgv.Columns)
                col.SortMode = DataGridViewColumnSortMode.Automatic;
        }
    }
}
