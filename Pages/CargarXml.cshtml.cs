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
        private readonly Catalogo catalogo;
        private readonly IWebHostEnvironment entorno;

        public CargarXmlModel(Catalogo catalogo, IWebHostEnvironment entorno)
        {
            this.catalogo = catalogo;
            this.entorno = entorno;
        }

        [BindProperty]
        public IFormFile ArchivoXml { get; set; }

        public string Mensaje { get; set; }

        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            if (ArchivoXml == null || ArchivoXml.Length == 0)
            {
                Mensaje = "Debes seleccionar un archivo XML antes de continuar.";
                return;
            }

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
