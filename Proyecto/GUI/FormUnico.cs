using System;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public static class FormUnico
    {
        public static T Abrir<T>(Func<T> factory = null) where T : Form, new()
        {
            T existente = Application.OpenForms.OfType<T>().FirstOrDefault();
            if (existente != null)
            {
                if (existente.WindowState == FormWindowState.Minimized)
                    existente.WindowState = FormWindowState.Normal;
                existente.BringToFront();
                existente.Activate();
                return existente;
            }

            T form = factory != null ? factory() : new T();
            UITheme.Aplicar(form);
            form.Show();
            return form;
        }

        public static DialogResult AbrirDialogo<T>(Func<T> factory = null) where T : Form, new()
        {
            T existente = Application.OpenForms.OfType<T>().FirstOrDefault();
            if (existente != null)
            {
                existente.BringToFront();
                existente.Activate();
                return DialogResult.None;
            }

            using (T form = factory != null ? factory() : new T())
            {
                UITheme.Aplicar(form);
                return form.ShowDialog();
            }
        }
    }
}
