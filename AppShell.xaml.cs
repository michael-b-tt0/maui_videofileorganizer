namespace VideoFileRenamer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
            BindingContext = ServiceHelper.GetService<MainpageViewModel>();
        }
    }
}
