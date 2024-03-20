namespace VideoFileRenamer;

public partial class VideoFileTemplate : ContentView
{
	public VideoFileTemplate()
	{
		InitializeComponent();
        BindingContext = ServiceHelper.GetService<MainpageViewModel>();
    }
}