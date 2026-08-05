using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GUI
{
    internal static class TreeViewExpansionHelper
    {
        public static HashSet<int> CapturarExpandidos(TreeView tree, Func<object, int?> obtenerId)
        {
            var ids = new HashSet<int>();
            if (tree != null)
                Capturar(tree.Nodes, obtenerId, ids);
            return ids;
        }

        public static int? CapturarSeleccionado(TreeView tree, Func<object, int?> obtenerId)
        {
            if (tree?.SelectedNode == null)
                return null;
            return obtenerId(tree.SelectedNode.Tag);
        }

        public static void RestaurarExpandidos(TreeView tree, HashSet<int> ids, Func<object, int?> obtenerId)
        {
            if (tree == null || ids == null || ids.Count == 0)
                return;

            Restaurar(tree.Nodes, ids, obtenerId);
        }

        private static void Capturar(TreeNodeCollection nodos, Func<object, int?> obtenerId, HashSet<int> ids)
        {
            foreach (TreeNode nodo in nodos)
            {
                if (nodo.IsExpanded)
                {
                    int? id = obtenerId(nodo.Tag);
                    if (id.HasValue)
                        ids.Add(id.Value);
                }

                Capturar(nodo.Nodes, obtenerId, ids);
            }
        }

        private static void Restaurar(TreeNodeCollection nodos, HashSet<int> ids, Func<object, int?> obtenerId)
        {
            foreach (TreeNode nodo in nodos)
            {
                int? id = obtenerId(nodo.Tag);
                if (id.HasValue && ids.Contains(id.Value))
                    nodo.Expand();

                Restaurar(nodo.Nodes, ids, obtenerId);
            }
        }
    }
}
