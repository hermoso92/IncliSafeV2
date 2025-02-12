// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace IncliSafe.Cliente.Dialogs
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 2 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using MudBlazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using IncliSafe.Cliente;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using IncliSafe.Cliente.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using IncliSafe.Cliente.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using IncliSafe.Cliente.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "/home/runner/workspace/IncliSafeCliente/_Imports.razor"
using System.Security.Claims;

#line default
#line hidden
#nullable disable
    public partial class InspeccionDialog : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 29 "/home/runner/workspace/IncliSafeCliente/Dialogs/InspeccionDialog.razor"
       
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public Inspeccion inspeccion { get; set; } = new();
    
    private MudForm form;
    private List<Vehiculo> vehiculos = new();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            vehiculos = await Http.GetFromJsonAsync<List<Vehiculo>>("api/vehiculos") ?? new();
        }
        catch (Exception ex)
        {
            Snackbar.Add("Error al cargar vehículos: " + ex.Message, Severity.Error);
        }
    }

    private void Cancel() => MudDialog.Cancel();

    private async Task Submit()
    {
        await form.Validate();
        if (form.IsValid)
        {
            try
            {
                var response = inspeccion.Id == 0 
                    ? await Http.PostAsJsonAsync("api/inspecciones", inspeccion)
                    : await Http.PutAsJsonAsync($"api/inspecciones/{inspeccion.Id}", inspeccion);
                
                if (response.IsSuccessStatusCode)
                {
                    Snackbar.Add("Inspección guardada correctamente", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(true));
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error al guardar: {ex.Message}", Severity.Error);
            }
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ISnackbar Snackbar { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private HttpClient Http { get; set; }
    }
}
#pragma warning restore 1591
