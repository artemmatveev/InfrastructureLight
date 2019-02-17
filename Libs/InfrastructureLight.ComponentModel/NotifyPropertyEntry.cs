using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InfrastructureLight.ComponentModel
{
    public class NotifyPropertyEntity : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChangedEvent([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
