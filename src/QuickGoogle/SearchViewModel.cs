using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuickGoogle
{
    class SearchViewModel : INotifyPropertyChanged
    {
        private readonly SearchModel searchModel;

        public string Search
        {
            get => searchModel.Search;
            set
            {
                if (searchModel.Search != value)
                {
                    searchModel.Search = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Search"));
                }
            }
        }

        public string SearchUri
        {
            get => $@"https://www.google.com/search?q={searchModel.Search}";
        }

        public SearchViewModel()
        {
            searchModel = new SearchModel();
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RoutedCommand();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
