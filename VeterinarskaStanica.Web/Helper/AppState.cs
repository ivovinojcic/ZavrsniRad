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
        private string _registeredUsername = null;
        private bool _spinner = false;

        /// <summary>
        /// Does LeftNav is Active?
        /// </summary>
        public bool LeftNav
        {
            get => _leftNav;
            set
            {
                _leftNav = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// When user make success registration
        /// Save "Username" into State, and show on login page
        /// </summary>
        public string RegisteredUsername
        {
            get => _registeredUsername;
            set
            {
                _registeredUsername = value;
                OnPropertyChanged();
            }
        }

        public bool Spinner
        {
            get => _spinner;
            set
            {
                _spinner = value;
                OnPropertyChanged();
            }
        }
    }
}