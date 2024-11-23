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
using Windows.Devices.Bluetooth.Advertisement;

namespace VideoFileRenamer;

public class MainpageViewModel : BindableObject
{
    VideoFolder VideoFolder = new();

    public int maxItemsPerList = 5;

    public string donuts { get; set; } = "what is this";

    List<List<string>> listOfListsof_files_to_process;

    public ObservableCollection<CSV_entry> CsvCollection  {get; set;}

    

    public ObservableCollection<VideoFile> VideoCollection { get; set; }
    
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

     public List<List<string>> SplitList(List<string> originalList, int maxItemsPerList)
     {
         listOfListsof_files_to_process = new();
         for (int i = 0; i < originalList.Count; i += maxItemsPerList)
         {
             // Get a range of items from the original list
             List<string> sublist = originalList.GetRange(i, Math.Min(maxItemsPerList, originalList.Count - i));
             listOfListsof_files_to_process.Add(sublist);
         }
         return listOfListsof_files_to_process;
     }


     public MainpageViewModel()
     {

        VideoCollection = new ObservableCollection<VideoFile>();
         CsvCollection = new ObservableCollection<CSV_entry>();
     }

     public async void select_directory(object args)
     {
         List<string> output = new List<string>();
         if (listOfListsof_files_to_process != null)
         {
             listOfListsof_files_to_process.Clear();
         }
         var result = await FolderPicker.Default.PickAsync();
         if (result.IsSuccessful)
         {

             Debug.WriteLine($"selected {result.Folder.Name} path is {result.Folder.Path}");
             VideoFolder.Directory_name = result.Folder.Name;
             VideoFolder.Directory_Full_Path = result.Folder.Path;
             VideoFile.Checkscreenshotfolder();
             VideoFolder.Load_filepaths();
             foreach (var i in VideoFolder.Directory_filelist)
             {
                 Debug.WriteLine($"file is {i}");
                 if (VideoFolder.IsVideoFile(i))
                 {
                     output.Add(i);

                 }
             }
             /*else { throw new Exception($"Error: cannot load {result.Folder.Name}"); }*/
            if (output.Count > 0)
            {
                listOfListsof_files_to_process = SplitList(output, maxItemsPerList);
                List<string> first_items_to_process = listOfListsof_files_to_process[0];
                listOfListsof_files_to_process.RemoveAt(0);

                foreach (var i in first_items_to_process)
                {
                    VideoFile video1 = new VideoFile
                    {
                        VideoPath = i
                    };
                    //why are images appearing in detail but not mainpage on release version!??/
                    video1.getVideoImages();
                    VideoCollection.Add(video1);
                }
            }
        }
        
    }

    public ICommand RemainingItemsThresholdReachedCommand => new Command(AddMoreItems);

    public async void AddMoreItems(object args)
    {
        if (listOfListsof_files_to_process.Count > 0)
        {

            List<string> remainingItems = listOfListsof_files_to_process[0];
            listOfListsof_files_to_process.RemoveAt(0);
            foreach (var i in remainingItems)
            {
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
        
        


    

