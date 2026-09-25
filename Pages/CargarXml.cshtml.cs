using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IPC2_Proyecto2.Persistencia;

namespace IPC2_Proyecto2.Pages
{
    public class CargarXmlModel : PageModel
    {
        // El Catalogo llega "inyectado" automáticamente porque lo
        // registramos como servicio (AddSingleton) en Program.cs.
        private readonly Catalogo catalogo;
        private readonly IWebHostEnvironment entorno;

        public CargarXmlModel(Catalogo catalogo, IWebHostEnvironment entorno)
        {
            this.catalogo = catalogo;
            this.entorno = entorno;
        }

        // [BindProperty] hace que este campo se llene automáticamente
        // con el archivo que el usuario suba desde el formulario.
        [BindProperty]
        public IFormFile ArchivoXml { get; set; }

        // Mensaje que se muestra en pantalla después de procesar.
        public string Mensaje { get; set; }

        // Se ejecuta cuando el usuario solo abre la página (GET).
        public void OnGet()
        {
        }

        // Se ejecuta cuando el usuario envía el formulario (POST).
        public async Task OnPostAsync()
        {
            if (ArchivoXml == null || ArchivoXml.Length == 0)
            {
                Mensaje = "Debes seleccionar un archivo XML antes de continuar.";
                return;
            }

            // Guardamos el archivo subido en una carpeta temporal del servidor,
            // porque LectorXml necesita una ruta física para poder leerlo.
            string carpetaTemporal = Path.Combine(entorno.ContentRootPath, "TemporalXml");
            if (!Directory.Exists(carpetaTemporal))
            {
                Directory.CreateDirectory(carpetaTemporal);
            }

            string rutaArchivo = Path.Combine(carpetaTemporal, Path.GetFileName(ArchivoXml.FileName));

            using (FileStream flujo = new FileStream(rutaArchivo, FileMode.Create))
            {
                await ArchivoXml.CopyToAsync(flujo);
            }

            LectorXml lector = new LectorXml();
            Mensaje = lector.CargarArchivo(rutaArchivo, catalogo);
        }
    }
}
