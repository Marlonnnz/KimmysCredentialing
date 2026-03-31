using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KimmysCredentialing.Models;
using KimmysCredentialing.Services;

namespace KimmysCredentialing.ViewModels;


    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly ProviderService _providerService;
        private readonly CredentialService _credentialService;

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

        [ObservableProperty]
        private Provider? selectedProvider;

        [ObservableProperty]
        private Credential? selectedCredential;

        [ObservableProperty]
        private string credentialName = string.Empty;

        [ObservableProperty]
        private DateTimeOffset? credentialIssueDate;

        [ObservableProperty]
        private DateTimeOffset? credentialExpirationDate;

        public ObservableCollection<Credential> ExpiringSoonCredentials { get; } = new();

        [ObservableProperty]
        private string credentialNotes = string.Empty;

        public ObservableCollection<Provider> Providers {get;} = new();
        public ObservableCollection<Credential> Credentials { get; } = new();

        public MainWindowViewModel()
        {
            _providerService = new ProviderService();
            _credentialService = new CredentialService();

            LoadProviders();
            LoadExpiringSoonCredentials();
    }

        partial void OnSelectedProviderChanged(Provider? value)
        {
            LoadCredentials();
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
            LoadExpiringSoonCredentials();
    }

        [RelayCommand]
        private void AddCredential()
    {
        if (SelectedProvider is null)
            return;

        if (string.IsNullOrWhiteSpace(CredentialName))
            return;

        var credential = new Credential
        {
            ProviderId = SelectedProvider.ProviderId,
            Name = credentialName,
            IssueDate = credentialIssueDate?.DateTime,
            ExpirationDate = credentialExpirationDate?.DateTime,
            Notes = CredentialNotes
        };

        _credentialService.AddCredential(credential);
        ClearCredentialForm();
        LoadCredentials();
    }

        [RelayCommand]    
        private void DeleteCredential()
    {
        if (selectedCredential is null)
            return;

        _credentialService.DeleteCredential(selectedCredential.CredentialId);
        selectedCredential = null;
        LoadCredentials();
        LoadExpiringSoonCredentials();
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

        private void LoadCredentials()
    {
        Credentials.Clear();

        if (SelectedProvider is null)
            return;

        var credentials = _credentialService.GetCredentialsByProviderId(SelectedProvider.ProviderId);
        
        foreach (var credential in credentials)
        {
            Credentials.Add(credential);
        }

    }

        private void ClearCredentialForm()
    {
        credentialName = string.Empty;
        credentialIssueDate = null;
        credentialExpirationDate = null;
        credentialNotes = string.Empty;
    }

    private void LoadExpiringSoonCredentials()
    {
        ExpiringSoonCredentials.Clear();

        var today = DateTime.Today;
        var soon = today.AddDays(30);

        foreach(var provider in Providers)
        {
            var credentials = _credentialService.GetCredentialsByProviderId(provider.ProviderId);

            foreach (var credential in credentials)
            {
                if (credential.ExpirationDate.HasValue &&
                    credential.ExpirationDate.Value >= today &&
                    credential.ExpirationDate.Value.Date <= soon)
                {
                    ExpiringSoonCredentials.Add(credential);
                }
            }
        }
    }
    }

