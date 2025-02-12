
using System.Globalization;
using Microsoft.JSInterop;

namespace BlazorApp.Services
{
    public class LanguageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LanguageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetLanguage(string culture)
        {
            var cultureInfo = new CultureInfo(culture);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
            
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "culture", culture);
        }
    }
}
