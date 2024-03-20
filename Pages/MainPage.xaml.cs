namespace VideoFileRenamer;

public partial class MainPage : ContentPage
{
    

    public MainPage()
    {
        
        BindingContext = ServiceHelper.GetService<MainpageViewModel>();

        InitializeComponent();

        
    }

    public async void Go_to_DetailPage(object sender, TappedEventArgs e)
    {
        if (sender is View tappedView && tappedView.BindingContext is VideoFile video)
        {
            // Access the 'video' object and perform your actions

            var navigationParameter = new ShellNavigationQueryParameters
    {
        { "VideoFile", video }
    };

            await Shell.Current.GoToAsync(nameof(DetailsPage), navigationParameter);
        }
    }
}
