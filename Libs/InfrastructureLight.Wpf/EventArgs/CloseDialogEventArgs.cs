namespace InfrastructureLight.Wpf.EventArgs
{
    public class CloseDialogEventArgs : System.EventArgs
    {
        public bool DialogResult { get; private set; }

        public CloseDialogEventArgs(bool dialogResult)
        {
            DialogResult = dialogResult;
        }
    }
}
