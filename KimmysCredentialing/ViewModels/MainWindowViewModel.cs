using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KimmysCredentialing.Models;
using KimmysCredentialing.Services;

namespace KimmysCredentialing.ViewModels;


    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly ProviderService _providerService;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string npi = string.Empty;

        [ObservableProperty]
        private string specialty = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string phone = string.Empty;

        public ObservableCollection<Provider> Providers {get;} = new();

        public MainWindowViewModel()
        {
            _providerService = new ProviderService();
            LoadProviders();
        }

        [RelayCommand]
        private void AddProvider()
        {
            if(string.IsNullOrWhiteSpace(Name))
                return;

            var provider = new Provider
            {
                Name = Name,
                NPI = Npi,
                Specialty = Specialty,
                Email = Email,
                Phone = Phone
            };

            _providerService.AddProvider(provider);

            ClearForm();
            LoadProviders();
        }

        private void LoadProviders()
        {
            Providers.Clear();

            var providers = _providerService.GetAllProviders();

            foreach (var provider in providers)
            {
                Providers.Add(provider);
            }
        }

        private void ClearForm()
        {
            Name = string.Empty;
            Npi = string.Empty;
            Specialty = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
        }
    }

