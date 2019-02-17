namespace InfrastructureLight.Wpf.Dialogs
{
    public class DialogSettings : IDialogSettings
    {
        public string Title { get; set; }
        public double DialogWidth { get; set; }
        public double DialogHeight { get; set; }
        public string Color { get; set; }
    }
}
