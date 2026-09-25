using IPC2_Proyecto2.Modelos;
using IPC2_Proyecto2.TDA;
using IPC2_Proyecto2.Excepciones;

namespace IPC2_Proyecto2
{
    public class Catalogo
    {
        public ArbolCategorias Categorias { get; private set; }
        public ArbolLibros Libros { get; private set; }

        public Catalogo()
        {
            Categorias = new ArbolCategorias();
            Libros = new ArbolLibros();
        }

        public void Reiniciar()
        {
            Categorias = new ArbolCategorias();
            Libros = new ArbolLibros();
        }

        public string RegistrarLibro(int isbn, string titulo, string autor, string nombreCategoria)
        {
            if (isbn <= 0)
            {
                return "El ISBN debe ser un número mayor a cero.";
            }

            string tituloNormalizado = titulo?.Trim() ?? string.Empty;
            string autorNormalizado = autor?.Trim() ?? string.Empty;
            string categoriaNormalizada = nombreCategoria?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(tituloNormalizado))
            {
                return "El título no puede estar vacío.";
            }

            if (string.IsNullOrWhiteSpace(autorNormalizado))
            {
                return "El autor no puede estar vacío.";
            }

            if (string.IsNullOrWhiteSpace(categoriaNormalizada))
            {
                return "La categoría del libro no puede estar vacía.";
            }

            try
            {
                ValidarRegistro(isbn, categoriaNormalizada);

                Libro nuevoLibro = new Libro(isbn, tituloNormalizado, autorNormalizado, categoriaNormalizada);
                Libros.Insertar(nuevoLibro);
                Categorias.AgregarLibroACategoria(nuevoLibro);

                return "Libro registrado correctamente.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void ValidarRegistro(int isbn, string nombreCategoria)
        {
            if (Libros.Buscar(isbn) != null)
            {
                throw new IsbnDuplicadoException(isbn);
            }

            if (Categorias.BuscarCategoria(nombreCategoria) == null)
            {
                throw new CategoriaNoEncontradaException(nombreCategoria);
            }
        }

        public bool EliminarLibro(int isbn)
        {
            return Libros.Eliminar(isbn);
        }

        public Libro BuscarLibro(int isbn)
        {
            return Libros.Buscar(isbn);
        }

        public Libro ObtenerLibroMenorIsbn()
        {
            return Libros.ObtenerMinimo();
        }

        public Libro ObtenerLibroMayorIsbn()
        {
            return Libros.ObtenerMaximo();
        }

        public ListaLibros ObtenerLibrosOrdenados()
        {
            return Libros.RecorridoAscendente();
        }

        public bool AgregarCategoria(string nombre, string? nombrePadre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return false;
            }

            return Categorias.AgregarCategoria(nombre.Trim(), nombrePadre);
        }

        public string ObtenerEstructuraCategorias(string nombreCategoriaInicio = null)
        {
            return Categorias.MostrarEstructura(nombreCategoriaInicio);
        }

        public ListaLibros ObtenerLibrosDeCategoria(string nombreCategoria)
        {
            return Libros.ObtenerLibrosPorCategoria(nombreCategoria);
        }
    }
}