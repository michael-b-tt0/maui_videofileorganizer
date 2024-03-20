using CommunityToolkit.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VideoFileRenamer;

public class MainpageViewModel : BindableObject
{
    VideoFolder VideoFolder = new();

    public string donuts { get; set; } = "what is this";

    public ObservableCollection<CSV_entry> CsvCollection  {get; set;}

    private ObservableCollection<VideoFile> videoCollection;

    public ObservableCollection<VideoFile> VideoCollection
    {
        get { return videoCollection; }
        set
        {
            videoCollection = value;
           /* OnPropertyChanged();*/
        }
    }
    public ICommand Handle_directory_selected_Command => new Command(select_directory);

    public ICommand Handle_csv_selected_Command => new Command(select_csv);

    public async void select_csv(object args)
    {
        var CsvFileType = new FilePickerFileType(
                        new Dictionary<DevicePlatform, IEnumerable<string>>
                        {

                    { DevicePlatform.Android, new[] { "text/csv" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { ".csv"} }, // file extension

                        });
        var result = await FilePicker.PickAsync(new PickOptions { PickerTitle = "pick a csv to load data", FileTypes = CsvFileType });
        if (result == null)
        {
            return;
        }
        else
        {
            Debug.WriteLine("it worked!");
            var file = result.FullPath;
            Load_data(file);
        }


    }

    public void Load_data(string csvpath)
    {
        var csvData = CsvLoader.load_csv(csvpath);
        foreach (CSV_entry entry in csvData)
        {
            CsvCollection.Add(entry);

        }
    }




    public MainpageViewModel()
    {

        videoCollection = new ObservableCollection<VideoFile>();
        CsvCollection = new ObservableCollection<CSV_entry>();
    }

    public async void select_directory(object args)
    {

        var result = await FolderPicker.Default.PickAsync();
        if (result.IsSuccessful)
        {

            Debug.WriteLine($"selected {result.Folder.Name} path is {result.Folder.Path}");
            VideoFolder.Directory_name = result.Folder.Name;
            VideoFolder.Directory_Full_Path = result.Folder.Path;
            VideoFolder.Load_filepaths();
            foreach (var i in VideoFolder.Directory_filelist)
            {
                Debug.WriteLine($"file is {i}");
                if (VideoFolder.IsVideoFile(i))
                {
                    //check if file is video here
                    VideoFile video1 = new VideoFile
                    {

                        VideoPath = i
                    };

                    video1.getVideoImages();
                    VideoCollection.Add(video1);
                }
            }
        }


    }
}
        
        


    

