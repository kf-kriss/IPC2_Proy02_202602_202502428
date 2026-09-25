using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IPC2_Proyecto2.Modelos;
using IPC2_Proyecto2.TDA;

namespace IPC2_Proyecto2.Pages
{
    public class LibrosModel : PageModel
    {
        private readonly Catalogo catalogo;

        public LibrosModel(Catalogo catalogo)
        {
            this.catalogo = catalogo;
        }

        [BindProperty]
        public int NuevoIsbn { get; set; }

        [BindProperty]
        public string NuevoTitulo { get; set; }

        [BindProperty]
        public string NuevoAutor { get; set; }

        [BindProperty]
        public string NuevaCategoria { get; set; }

        [BindProperty]
        public int IsbnConsulta { get; set; }

        public string MensajeRegistro { get; set; }
        public string MensajeEliminar { get; set; }
        public string MensajeBusqueda { get; set; }
        public Libro LibroEncontrado { get; set; }

        public Libro LibroMenor { get; set; }
        public Libro LibroMayor { get; set; }
        public ListaLibros TodosLosLibros { get; set; }

        public void OnGet()
        {
            CargarDatosGenerales();
        }

        // Botón "Registrar libro"
        public IActionResult OnPostRegistrar()
        {
            if (NuevoIsbn <= 0)
            {
                MensajeRegistro = "El ISBN debe ser un número mayor a cero.";
            }
            else if (string.IsNullOrWhiteSpace(NuevoTitulo))
            {
                MensajeRegistro = "El título no puede estar vacío.";
            }
            else if (string.IsNullOrWhiteSpace(NuevoAutor))
            {
                MensajeRegistro = "El autor no puede estar vacío.";
            }
            else if (string.IsNullOrWhiteSpace(NuevaCategoria))
            {
                MensajeRegistro = "Debes indicar la categoría del libro.";
            }
            else
            {
                MensajeRegistro = catalogo.RegistrarLibro(NuevoIsbn, NuevoTitulo.Trim(), NuevoAutor.Trim(), NuevaCategoria.Trim());
            }

            CargarDatosGenerales();
            return Page();
        }

        // Botón "Buscar por ISBN"
        public IActionResult OnPostBuscar()
        {
            if (IsbnConsulta <= 0)
            {
                MensajeBusqueda = "Escribe un ISBN válido para buscar.";
            }
            else
            {
                LibroEncontrado = catalogo.BuscarLibro(IsbnConsulta);

                if (LibroEncontrado == null)
                {
                    MensajeBusqueda = "No se encontró ningún libro con ese ISBN.";
                }
            }

            CargarDatosGenerales();
            return Page();
        }

        // Botón "Eliminar por ISBN"
        public IActionResult OnPostEliminar()
        {
            if (IsbnConsulta <= 0)
            {
                MensajeEliminar = "Escribe un ISBN válido para eliminar.";
            }
            else
            {
                bool eliminado = catalogo.EliminarLibro(IsbnConsulta);

                MensajeEliminar = eliminado
                    ? "Libro eliminado correctamente."
                    : "No se encontró ningún libro con ese ISBN para eliminar.";
            }

            CargarDatosGenerales();
            return Page();
        }

        private void CargarDatosGenerales()
        {
            LibroMenor = catalogo.ObtenerLibroMenorIsbn();
            LibroMayor = catalogo.ObtenerLibroMayorIsbn();
            TodosLosLibros = catalogo.ObtenerLibrosOrdenados();
        }
    }
}