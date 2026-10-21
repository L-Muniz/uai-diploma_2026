using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using BE;
using DAL;
using SERVICE;

namespace BLL
{
    // Caretaker del Memento de usuario: guarda, lista y restaura mementos.
    // Solo conoce el memento como objeto; el estado lo crea y aplica BE_USUARIO.
    public class BLL_UsuarioMemento
    {
        private static readonly XmlSerializer serializador = new XmlSerializer(typeof(BE_UsuarioMemento));

        // Captura el estado actual del usuario. Lanza excepción si no se pudo guardar,
        // para que quien la llama no modifique el usuario sin poder deshacerlo.
        public static BE_UsuarioMemento Capturar(int idUsuario, string descripcion)
        {
            MP_USUARIO mpUsuario = new MP_USUARIO();
            BE_USUARIO usuario = mpUsuario.BuscarPorId(idUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado al capturar su estado.");

            BE_UsuarioMemento memento = usuario.CrearMemento(mpUsuario.ObtenerIdRoles(idUsuario), descripcion);
            memento.Contenido = Codificar(memento);

            memento.ID = new MP_MEMENTO_USUARIO().Alta(memento);
            if (memento.ID <= 0)
                throw new Exception("No se pudo guardar el memento del usuario.");

            return memento;
        }

        public static List<BE_UsuarioMemento> ListarPorUsuario(int idUsuario)
        {
            return new MP_MEMENTO_USUARIO().ListarPorUsuario(idUsuario)
                .Select(Decodificar)
                .ToList();
        }

        public static BE_UsuarioMemento Obtener(int idMemento)
        {
            BE_UsuarioMemento persistido = new MP_MEMENTO_USUARIO().ObtenerPorId(idMemento);
            if (persistido == null)
                throw new Exception("Memento no encontrado.");

            return Decodificar(persistido);
        }

        public static void Restaurar(int idMemento)
        {
            BE_UsuarioMemento memento = Obtener(idMemento);

            MP_USUARIO mpUsuario = new MP_USUARIO();
            BE_USUARIO usuario = mpUsuario.BuscarPorId(memento.IdUsuario);
            if (usuario == null)
                throw new InvalidOperationException("El usuario ya no existe; su estado no puede restaurarse.");

            BLL_USUARIO.ValidarUsername(memento.Username);
            BE_USUARIO conMismoNombre = mpUsuario.BuscarPorUserName(memento.Username);
            if (conMismoNombre != null && conMismoNombre.ID != usuario.ID)
                throw new InvalidOperationException("El nombre de usuario del memento ya está en uso por otro usuario.");

            if (memento.Bloqueado && BLL_USUARIO.EsRolAdministrador(mpUsuario.ObtenerRolUsuario(usuario.ID)))
                throw new InvalidOperationException("No se puede bloquear a un usuario administrador.");

            // Se captura el estado actual antes de restaurar, así la restauración también es reversible.
            Capturar(usuario.ID, $"Antes de restaurar el memento #{idMemento}");

            string usernameAnterior = usuario.Username;
            bool bloqueadoAnterior = usuario.Bloqueado;
            string hashAnterior = usuario.HashedPassword;
            List<int> rolesAnteriores = mpUsuario.ObtenerIdRoles(usuario.ID);

            usuario.RestaurarDesde(memento);

            foreach (int idRol in memento.IdRoles.Except(rolesAnteriores))
                mpUsuario.AsignarRol(usuario.ID, idRol);
            foreach (int idRol in rolesAnteriores.Except(memento.IdRoles))
                mpUsuario.QuitarRol(usuario.ID, idRol);

            BLL_IntegridadDatos.RecalcularYPersistirUsuario(usuario, mpUsuario);

            var usuarioActual = SV_Singleton.GetInstance().Usuario;
            string motivo = $"Restaurado desde memento #{idMemento}";
            BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "USERNAME", usernameAnterior, usuario.Username, usuarioActual, motivo);
            BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "BLOQUEADO", bloqueadoAnterior.ToString(), usuario.Bloqueado.ToString(), usuarioActual, motivo);
            if (hashAnterior != usuario.HashedPassword)
                BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "PASS", "[hash]", "[restaurado]", usuarioActual, motivo);
            if (!rolesAnteriores.OrderBy(r => r).SequenceEqual(memento.IdRoles.OrderBy(r => r)))
                BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "ROL", string.Join(",", rolesAnteriores), string.Join(",", memento.IdRoles), usuarioActual, motivo);

            new BLL_BITACORA().IngresarRegistro(usuarioActual ?? usuario, $"Usuario {usuario.Username} restaurado desde memento #{idMemento}");
        }

        private static string Codificar(BE_UsuarioMemento memento)
        {
            using (StringWriter escritor = new StringWriter())
            {
                serializador.Serialize(escritor, memento);
                return SV_Encriptacion.Encriptar(escritor.ToString());
            }
        }

        private static BE_UsuarioMemento Decodificar(BE_UsuarioMemento persistido)
        {
            string xml = SV_Encriptacion.Desencriptar(persistido.Contenido);

            BE_UsuarioMemento memento;
            using (StringReader lector = new StringReader(xml))
            {
                memento = (BE_UsuarioMemento)serializador.Deserialize(lector);
            }

            memento.ID = persistido.ID;
            memento.Descripcion = persistido.Descripcion;
            memento.FechaCaptura = persistido.FechaCaptura;
            memento.Contenido = persistido.Contenido;
            return memento;
        }
    }
}
