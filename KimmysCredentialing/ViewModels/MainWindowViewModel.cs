using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private string selectedCredentialStatusFilter = "All";

        public List<string> CredentialStatusFilters { get; } = new()
        {
            "All",
            "Active",
            "Expired",
            "Expiring Soon",
            "No Expiration Date"
        };

        [ObservableProperty]
        private string providerSearchText = string.Empty;

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

            if (value is null)
                return;

            Name = value.Name;
            Npi = value.NPI;
            Specialty = value.Specialty;
            Email = value.Email;
            Phone = value.Phone;
        }

        partial void OnSelectedCredentialChanged(Credential? value)
        {
            if (value is null)
                return;

            CredentialName = value.Name;
            CredentialIssueDate = value.IssueDate.HasValue
                ? new DateTimeOffset(value.ExpirationDate.Value)
                : null;

            CredentialNotes = value.Notes;
        }

        partial void OnSelectedCredentialStatusFilterChanged(string value)
        {
            LoadCredentials();
        }
        partial void OnProviderSearchTextChanged(string value)
        {
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
            LoadExpiringSoonCredentials();
    }
        [RelayCommand]
        private void UpdateProvider()
        {
            if (SelectedProvider is null)
                return;

            SelectedProvider.Name = Name;
            SelectedProvider.NPI = Npi;
            SelectedProvider.Specialty = Specialty;
            SelectedProvider.Email = Email;
            SelectedProvider.Phone = Phone;

            _providerService.UpdateProvider(SelectedProvider);

            LoadProviders();
            LoadExpiringSoonCredentials();
        }
        [RelayCommand]
        private void DeleteProvider()
        {
            if (SelectedProvider is null)
                return;

            _providerService.DeleteProvider(SelectedProvider.ProviderId);

            SelectedProvider = null;
            SelectedCredential = null;

            Credentials.Clear();
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
        private void UpdateCredential()
        {
            if (SelectedCredential is null)
                return;

            SelectedCredential.Name = CredentialName;
            SelectedCredential.IssueDate = CredentialIssueDate?.DateTime;
            SelectedCredential.ExpirationDate = CredentialExpirationDate?.DateTime;
            SelectedCredential.Notes = CredentialNotes;

            _credentialService.UpdateCredential(SelectedCredential);

            LoadCredentials();
            LoadExpiringSoonCredentials();
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

            if(!string.IsNullOrWhiteSpace(ProviderSearchText))
            {
                var search = providerSearchText.Trim().ToLower();

                providers = providers
                    .Where(p =>
                    (!string.IsNullOrWhiteSpace(p.Name) && p.Name.ToLower().Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(p.NPI) && p.NPI.ToLower().Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(p.Specialty) && p.Specialty.ToLower().Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(p.Email) && p.Email.ToLower().Contains(search)) ||
                    (!string.IsNullOrWhiteSpace(p.Phone) && p.Phone.ToLower().Contains(search)))
                    .ToList();
            }

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
            
            if(!string.IsNullOrWhiteSpace(SelectedCredentialStatusFilter) &&
                selectedCredentialStatusFilter != "All")
            {
                credentials = credentials
                    .Where(c => c.Status == selectedCredentialStatusFilter)
                    .ToList();
            }

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

