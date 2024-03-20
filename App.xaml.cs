using Microsoft.Maui;
using Microsoft.Maui.LifecycleEvents;

namespace VideoFileRenamer;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();


        MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        var folder = VideoFile.temp_photo_folder;
        if (Directory.Exists(folder))
        {
            try
            {
                // Delete all files in the folder
                string[] files = Directory.GetFiles(folder);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file deletion
                Console.WriteLine($"An error occurred while deleting files: {ex.Message}");
            }

        }


    }
}

