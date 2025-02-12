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
#nullable restore
#line 1 "/home/runner/workspace/IncliSafeCliente/Dialogs/VehiculoDialog.razor"
using IncliSafe.Cliente.Models;

#line default
#line hidden
#nullable disable
    public partial class VehiculoDialog : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 21 "/home/runner/workspace/IncliSafeCliente/Dialogs/VehiculoDialog.razor"
       
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public Vehiculo Vehiculo { get; set; } = new();
    private Vehiculo vehiculo;

    protected override void OnInitialized()
    {
        vehiculo = new Vehiculo
        {
            Id = Vehiculo.Id,
            Placa = Vehiculo.Placa,
            Marca = Vehiculo.Marca,
            Modelo = Vehiculo.Modelo,
            Año = Vehiculo.Año,
            Estado = Vehiculo.Estado,
            Activo = Vehiculo.Activo
        };
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task Submit()
    {
        
        MudDialog.Close(DialogResult.Ok(vehiculo));
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
