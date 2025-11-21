using Microsoft.Extensions.Logging;
using JIR.MauiApp.Services;

namespace JIR.MauiApp;

public static class MauiProgram
{
	public static Microsoft.Maui.Hosting.MauiApp CreateMauiApp()
	{
		var builder = Microsoft.Maui.Hosting.MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

		// Configuration HttpClient pour l'API
		builder.Services.AddHttpClient<MagasinService>(client =>
		{
			client.BaseAddress = new Uri("http://localhost:5001/api/");
			client.Timeout = TimeSpan.FromSeconds(30);
		});

		builder.Services.AddHttpClient<ArticleMagasinService>(client =>
		{
			client.BaseAddress = new Uri("http://localhost:5001/api/");
			client.Timeout = TimeSpan.FromSeconds(30);
		});

		builder.Services.AddHttpClient<MouvementStockService>(client =>
		{
			client.BaseAddress = new Uri("http://localhost:5001/api/");
			client.Timeout = TimeSpan.FromSeconds(30);
		});

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
