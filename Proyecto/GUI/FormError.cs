using System;
using System.Windows.Forms;

namespace GUI
{
    public static class ManejoErrores
    {
        public static void MostrarError(Exception ex)
        {
            MessageBox.Show($"{ex.GetType().Name}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            System.Diagnostics.Debug.WriteLine($"[ERROR] {ex.Message}\n{ex.StackTrace}");
        }
    }
}
