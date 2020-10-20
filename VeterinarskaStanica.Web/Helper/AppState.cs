using VeterinarskaStanica.Model.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VeterinarskaStanica.Web.Helper
{
    public class AppState : INotifyPropertyChanged
    {
        /// <summary>
        /// Event when "Global Var" have been changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _leftNav = true;

        public bool LeftNav
        {
            get => _leftNav;
            set
            {
                _leftNav = value;
                OnPropertyChanged();
            }
        }
    }
}