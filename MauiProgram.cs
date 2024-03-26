
using CommunityToolkit.Maui;
using FFMpegCore;

using Microsoft.Extensions.Logging;
using UraniumUI;

namespace VideoFileRenamer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MainpageViewModel>();
            var app = builder.Build();
            /*GlobalFFOptions.Configure(new FFOptions { BinaryFolder = @"C:\Users\mbman\AppData\Local\Microsoft\WinGet\Packages\Gyan.FFmpeg_Microsoft.Winget.Source_8wekyb3d8bbwe\ffmpeg - 6.1.1 - full_build\bin",  TemporaryFilesFolder = @"C:\\Users\\mbman\\temp" });*/

            app.Services.GetService<MainpageViewModel>();
            return app;
        }
    }
}
