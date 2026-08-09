using HelaTico.Application.DTOs;
using System.Text.Json;

namespace HelaTico.Web.Helpers
{
    public static class CarritoSessionHelper
    {
        private const string SESSION_KEY = "Carrito";

        public static List<CarritoItemDTO> Obtener(ISession session)
        {
            var json = session.GetString(SESSION_KEY);
            if (string.IsNullOrEmpty(json))
                return new List<CarritoItemDTO>();

            return JsonSerializer.Deserialize<List<CarritoItemDTO>>(json) ?? new List<CarritoItemDTO>();
        }

        private static void Guardar(ISession session, List<CarritoItemDTO> items)
        {
            session.SetString(SESSION_KEY, JsonSerializer.Serialize(items));
        }

        public static void AgregarItem(ISession session, string tipo, int id, string nombre, decimal precio, int cantidad, string imagenUrl)
        {
            var items = Obtener(session);
            var existente = items.FirstOrDefault(i => i.Tipo == tipo && i.Id == id);

            if (existente != null)
                existente.Cantidad += cantidad;
            else
                items.Add(new CarritoItemDTO { Tipo = tipo, Id = id, Nombre = nombre, Precio = precio, Cantidad = cantidad, ImagenUrl = imagenUrl });

            Guardar(session, items);
        }

        public static void ActualizarCantidad(ISession session, string tipo, int id, int nuevaCantidad)
        {
            var items = Obtener(session);
            var item = items.FirstOrDefault(i => i.Tipo == tipo && i.Id == id);

            if (item != null)
            {
                if (nuevaCantidad < 1) nuevaCantidad = 1;
                item.Cantidad = nuevaCantidad;
                Guardar(session, items);
            }
        }

        public static void Eliminar(ISession session, string tipo, int id)
        {
            var items = Obtener(session);
            items.RemoveAll(i => i.Tipo == tipo && i.Id == id);
            Guardar(session, items);
        }

        public static int ObtenerCantidadTotal(ISession session)
        {
            return Obtener(session).Sum(i => i.Cantidad);
        }
    }
}