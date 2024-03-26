


using System.Diagnostics;
using System.Windows.Input;

namespace VideoFileRenamer;


[QueryProperty(nameof(VideoFile), nameof(VideoFile))]
public partial class DetailsPage : ContentPage
{
    VideoFile videoFile;
    public VideoFile VideoFile

    {
        get => videoFile;
        set
        {
            if (videoFile != value)
            {
                videoFile = value;
                OnPropertyChanged(); // Call OnPropertyChanged when the property changes
            }
        }
    }

    private string selectedVideoChoice = string.Empty;

    

    public string SelectedVideoChoice
    {

        get => selectedVideoChoice;
        set
        {
            if (selectedVideoChoice != value)
            {
                selectedVideoChoice = value;
                OnPropertyChanged(); // Call OnPropertyChanged when the property changes
            }
        }
    }

    MainpageViewModel mainpageViewModel;
    public DetailsPage()
    {

        InitializeComponent();
        BindingContext = this;
        mainpageViewModel = ServiceHelper.GetService<MainpageViewModel>();
        csv_component.BindingContext = mainpageViewModel;




    }

    public void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        SelectedVideoChoice = (e.CurrentSelection.FirstOrDefault() as CSV_entry)?.FullFilePath;

    }

    public ICommand Triggerrenaming => new Command(RenameFile);

    public async void RenameFile(object args)
    {

        bool result = await DisplayAlert("Renaming", $"Rename and move file from {videoFile.VideoPath} to {SelectedVideoChoice}?", "OK", "Cancel");

        if (result)
        {

            var progressReporter = new Progress<int>(progress =>
                {
                    if (progress == 1)
                    {
                        csv_component.IsVisible = false;
                        ActivityComponent.IsRunning = true;
                        ActivityComponent.IsVisible = true;

                        DisplayAlert("Title", "Moving started!", "OK");

                    }

                    else if (progress == 2)
                    {
                        ActivityComponent.IsRunning = false;
                        ActivityComponent.IsVisible = false;
                        csv_component.IsVisible = true;
                        DisplayAlert("Title", "Moving Finished!", "OK");
                    }
                }) ;

                // Call your function here
                var (output, hasMoved) = await FileRenameandmove.RenameAsync(videoFile.VideoPath, SelectedVideoChoice, progressReporter);
            if (hasMoved)
            {
                //remove the current video file from the collection and go back to the mainpage
                var done = mainpageViewModel.VideoCollection.Remove(VideoFile);
                if (done)
                {
                    Debug.WriteLine("video removed!");
                }

                await Shell.Current.GoToAsync("..");

            }
            
                await DisplayAlert("Output", output, "OK");
            

            
        }






    }

    public async void DeleteVideo(object sender, EventArgs args)
    {
        bool result = await DisplayAlert("Deleting", $"Really delete file {videoFile.VideoPath} ?", "OK", "Cancel");

        if (result)
        {
            var progressReporter = new Progress<int>(progress =>
            {
                if (progress == 1)
                {
                    csv_component.IsVisible = false;
                    ActivityComponent.IsRunning = true;
                    ActivityComponent.IsVisible = true;



                }

                else if (progress == 2)
                {
                    ActivityComponent.IsRunning = false;
                    ActivityComponent.IsVisible = false;
                    csv_component.IsVisible = true;
                    DisplayAlert("Title", "Delete Finished!", "OK");
                }
            });

            var (output, hasDeleted) = await FileRenameandmove.DeleteAsync(videoFile.VideoPath, progressReporter);
            if (hasDeleted)
            {
                //remove the current video file from the collection and go back to the mainpage
                var done = mainpageViewModel.VideoCollection.Remove(VideoFile);
                if (done)
                {
                    Debug.WriteLine("video removed!");
                }

                await Shell.Current.GoToAsync("..");

            }

            await DisplayAlert("Output", output, "OK");

        }

    }

    public async void Go_to_link(object sender, TappedEventArgs e)
    {
        if (sender is View tappedView && tappedView.BindingContext is CSV_entry csv)
        {
            // Access the 'video' object and perform your actions
            if (csv.scene_link_path != null)
                await Launcher.Default.OpenAsync(csv.scene_link_path);
        }
    }
    public void filter_csv_options(object sender, TextChangedEventArgs e)

    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
            csv_component.ItemsSource = mainpageViewModel.CsvCollection;
        else if (e.NewTextValue.Any(char.IsDigit))
        {
            csv_component.ItemsSource = mainpageViewModel.CsvCollection.Where(obj => obj.Date.ToString().Contains(e.NewTextValue) || obj.scene_code != null && obj.scene_code.Contains(e.NewTextValue.ToLower()));
        }
        else {

            csv_component.ItemsSource = mainpageViewModel.CsvCollection.Where(obj => obj.Title.ToLower().Contains(e.NewTextValue.ToLower()) || obj.Actors.ToLower().Contains(e.NewTextValue.ToLower()));
        }
    }

}