using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using IPC2_Proyecto2.Graphviz;

namespace IPC2_Proyecto2.Pages
{
    public class VerCategoriaGraficaModel : PageModel
    {
        private readonly Catalogo catalogo;
        private readonly IWebHostEnvironment entorno;
        private readonly GeneradorGraphviz generador;

        public VerCategoriaGraficaModel(Catalogo catalogo, IWebHostEnvironment entorno)
        {
            this.catalogo = catalogo;
            this.entorno = entorno;
            this.generador = new GeneradorGraphviz();
        }

        [BindProperty]
        public string NombreCategoria { get; set; }

        [BindProperty]
        public string TipoGrafico { get; set; } = "libros";

        public string RutaImagen { get; set; }
        public string MensajeError { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPostGenerar()
        {
            string carpetaSalida = Path.Combine(entorno.WebRootPath, "imagenes");

            string nombreArchivo;

            if (TipoGrafico == "estructura")
            {
                nombreArchivo = generador.GenerarImagenEstructura(catalogo, NombreCategoria, carpetaSalida, out string error);
                MensajeError = error;
            }
            else
            {
                nombreArchivo = generador.GenerarImagenLibrosCategoria(catalogo, NombreCategoria, carpetaSalida, out string error);
                MensajeError = error;
            }

            if (nombreArchivo != null)
            {
                RutaImagen = "/imagenes/" + nombreArchivo;
                MensajeError = null;
            }

            return Page();
        }
    }
}