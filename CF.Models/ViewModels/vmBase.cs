using System.ComponentModel;
using System.Runtime.CompilerServices;
using CF.Models.Interfaces;


namespace CF.Models.ViewModels
{
    public abstract class vmBase : INotifyPropertyChanged, iViewmodel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public abstract void SetInitialData();
        public abstract void UpdateOriginData();

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
