
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFMpegCore;


namespace VideoFileRenamer;

public class VideoFolder
{
    public string Directory_name { get; set; } = string.Empty;

    public string Directory_Full_Path { get; set; } = string.Empty;

    public List<string> Directory_filelist { get; set; } = new();

    public void Load_filepaths()
    {
        Directory_filelist.Clear();
        string[] filesInDirectory = Directory.GetFiles(Directory_Full_Path);
        Directory_filelist.AddRange(filesInDirectory);
        
    }

    public static HashSet<string> videoExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".avi", ".mp4", ".mkv", ".mov", ".wmv", 
    };

    public static bool IsVideoFile(string filePath)
    {
        string extension = Path.GetExtension(filePath);

        return videoExtensions.Contains(extension);
    }

}

public class VideoFile : INotifyPropertyChanged
{
    public string VideoName { get; set; } = string.Empty;

    private string videoPath = string.Empty;

    Random random = new Random();

    public VideoFile()
    {

        paths_to_screenshots = new ObservableCollection<string>();
    }

    private ObservableCollection<string> paths_to_screenshots;

    public ObservableCollection<string> Paths_to_screenshots
    {
        get { return paths_to_screenshots; }
        set
        {
            paths_to_screenshots = value;
            OnPropertyChanged(nameof(Paths_to_screenshots));
        }
    }

    public static readonly string localAppDataFolder = FileSystem.AppDataDirectory;

    public static readonly string temp_photo_folder = Path.Combine(localAppDataFolder, "TempImages");

    public event PropertyChangedEventHandler? PropertyChanged;


    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private IMediaAnalysis _mediaAnalysis;

    public IMediaAnalysis mediaAnalysis
    {
        get { return _mediaAnalysis; }
        set
        {
            if (_mediaAnalysis != value)
            {
                _mediaAnalysis = value;
                OnPropertyChanged(nameof(mediaAnalysis));
            }
        }
    }

    

    public string VideoPath
    {
        get => videoPath;
        set
        {
            // Set the full path
            videoPath = value;

            // Extract and set the file name
            VideoName = Path.GetFileName(value);
        }
    }


   /* public async void getmediainfo()
    {
        var mediaInfo = await FFProbe.AnalyseAsync(videoPath);
        if (mediaInfo != null)
        {
            mediaAnalysis = mediaInfo;
            Debug.WriteLine($"video is length {mediaAnalysis.Duration}");
        }

    }*/

    public async void getVideoImages()

    {
        var mediaInfo = await FFProbe.AnalyseAsync(videoPath);
        if (mediaInfo != null)
        {
            mediaAnalysis = mediaInfo;


            for (int i = 0; i < 4; i++)
            {
                double randomOffset = random.NextDouble() * mediaAnalysis.Duration.TotalSeconds;
                TimeSpan snapshotTime = TimeSpan.FromSeconds(randomOffset);
                string shortUuid = Guid.NewGuid().ToString("N").Substring(0, 8);
                var tempImagePath = Path.Combine(temp_photo_folder, $"{shortUuid}.PNG");

                var success = await FFMpeg.SnapshotAsync(videoPath, tempImagePath, new System.Drawing.Size(854, 480), snapshotTime);
                if (!success)
                {
                    throw new Exception($"Error taking screenshot {i + 1}!");

                }
                else
                    paths_to_screenshots.Add(tempImagePath);
                
                Debug.WriteLine($"added {tempImagePath} screenshot to {this.VideoName}");
            }

        }
    }
}


