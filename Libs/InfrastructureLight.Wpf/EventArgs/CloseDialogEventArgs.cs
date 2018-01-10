namespace InfrastructureLight.Wpf.EventArgs
{
    public class CancelDialogEventArgs : System.EventArgs
    {
        public bool DialogResult { get; private set; }

        public CancelDialogEventArgs(bool dialogResult)
        {
            DialogResult = dialogResult;
        }
    }
}
