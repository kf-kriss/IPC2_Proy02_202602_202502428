using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IPC2_Proyecto2.Pages
{
    public class CategoriasModel : PageModel
    {
        private readonly Catalogo catalogo;

        public CategoriasModel(Catalogo catalogo)
        {
            this.catalogo = catalogo;
        }

        [BindProperty]
        public string NuevoNombre { get; set; }

        [BindProperty]
        public string NuevoPadre { get; set; }

        [BindProperty]
        public string CategoriaInicio { get; set; }

        public string MensajeAgregar { get; set; }
        public string Estructura { get; set; }

        public void OnGet()
        {
            Estructura = catalogo.ObtenerEstructuraCategorias();
        }

        public IActionResult OnPostAgregar()
        {
            if (string.IsNullOrWhiteSpace(NuevoNombre))
            {
                MensajeAgregar = "El nombre de la categoría no puede estar vacío.";
            }
            else
            {
                bool agregada = catalogo.AgregarCategoria(NuevoNombre.Trim(), NuevoPadre?.Trim());

                MensajeAgregar = agregada
                    ? "Categoría agregada correctamente."
                    : "No se pudo agregar: el nombre ya existe o la categoría padre no fue encontrada.";
            }

            Estructura = catalogo.ObtenerEstructuraCategorias();
            return Page();
        }

        public IActionResult OnPostVerDesde()
        {
            Estructura = catalogo.ObtenerEstructuraCategorias(CategoriaInicio);
            return Page();
        }
    }
}