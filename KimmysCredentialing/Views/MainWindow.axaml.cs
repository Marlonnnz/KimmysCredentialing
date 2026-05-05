using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using KimmysCredentialing.ViewModels;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using KimmysCredentialing.Models;
using KimmysCredentialing.Services;
using System.Linq.Expressions;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace KimmysCredentialing.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public async void BrowseDocument_Click(object? sender, RoutedEventArgs e)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select Credential Document",
                AllowMultiple = false
            });

            var file = files.FirstOrDefault();

            if (file is null)
                return;

            if (DataContext is MainWindowViewModel vm)
            {
                vm.CredentialFilePath = file.Path.LocalPath;
            }
        }

        private void OpenDocument_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is not MainWindowViewModel vm)
                return;

            if (string.IsNullOrWhiteSpace(vm.CredentialFilePath))
                return;

            if (!File.Exists(vm.CredentialFilePath))
                return;

            var startInfo = new ProcessStartInfo
            {
                FileName = vm.CredentialFilePath,
                UseShellExecute = true
            };

            Process.Start(startInfo);
        }

        private async void DeleteProvider_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is not MainWindowViewModel vm)
                return;

            if (vm.SelectedProvider is null)
            {
                vm.StatusMessage = "Select a provider to delete.";
                vm.IsError = true;
                return;
            }

            var dialog = new ConfirmationWindow(
                $"Are you sure you want to delete provider '{vm.SelectedProvider.Name}'?");

            await dialog.ShowDialog(this);

            if (dialog.Confirmed)
            {
                vm.DeleteProviderCommand.Execute(null);
            }
        }

        private async void DeleteCredential_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is not MainWindowViewModel vm)
                return;

            if (vm.SelectedCredential is null)
            {
                vm.StatusMessage = "Select a credential to delete.";
                vm.IsError = true;
                return;
            }

            var dialog = new ConfirmationWindow(
                $"Are you sure you want to delete credential '{vm.SelectedCredential.Name}'?"
            );

            await dialog.ShowDialog(this);

            if (dialog.Confirmed)
            {
                vm.DeleteCredentialCommand.Execute(null);
            }
        }

        private async void ExportBackup_Click(object? sender, RoutedEventArgs e)
        {
            var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Export Backup",
                SuggestedFileName = "kimmys-credentialing-backup.json",
                DefaultExtension = "json"
            });

            if (file is null)
                return;

            try
            {
                var providerService = new ProviderService();
                var providers = providerService.GetAllProvidersWithCredentials();

                var backup = new AppBackup
                {
                    Providers = providers
                };

                var json = JsonSerializer.Serialize(backup, new JsonSerializerOptions
                {
                    WriteIndented = true
                });



                await File.WriteAllTextAsync(file.Path.LocalPath, json);

                if (DataContext is MainWindowViewModel vm)
                {
                    vm.StatusMessage = "Backup exported successfully";
                    vm.IsError = false;
                }
            }
            catch (Exception ex)
            {
                if (DataContext is MainWindowViewModel vm)
                {
                    vm.StatusMessage = $"Failed to export backup: {ex.Message}.";
                    vm.IsError = true;
                }
            }
        }

        private async void ImportBackup_Click(object? sender, RoutedEventArgs e)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Import Backup",
                AllowMultiple = false
            });

            var file = files.FirstOrDefault();

            if (file is null)
                return;

            try
            {
                string json;

                await using (var stream = await file.OpenReadAsync())
                using (var reader = new StreamReader(stream))
                {
                    json = await reader.ReadToEndAsync();
                }

                var backup = JsonSerializer.Deserialize<AppBackup>(json);

                if (backup is null)
                {
                    if (DataContext is MainWindowViewModel vmNull)
                    {
                        vmNull.StatusMessage = "Backup file was empty or invalid.";
                        vmNull.IsError = true;
                    }
                    return;
                }

                var providerService = new ProviderService();
                providerService.RestoreBackup(backup.Providers);

                if (DataContext is MainWindowViewModel vm)
                {
                    vm.RefreshAllData();
                    vm.StatusMessage = "Backup imported successfully.";
                    vm.IsError = false;
                }
            }
            catch (Exception ex)
            {
                if (DataContext is MainWindowViewModel vm)
                {
                    vm.StatusMessage = $"Failed to import backup: {ex.Message}";
                    vm.IsError = true;
                }
            }
        }

        private async void ExportCsvReport_Click(object? sender, RoutedEventArgs e)
        {
            var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Export CSV Report",
                SuggestedFileName = "kimmys-credentialing-report.csv",
                DefaultExtension = "csv"
            });

            if (file is null)
                return;

            try
            {
                var providerService = new ProviderService();
                var providers = providerService.GetAllProvidersWithCredentials();

                var csv = new StringBuilder();

                csv.AppendLine("Provider Name,NPI,Specialty, Email, Phone, Credential Name,Issue Date, Expiration Date, Status,Document, Path Notes");

                foreach ( var provider in providers)
                {
                    foreach ( var credential in provider.Credentials)
                    {
                        csv.AppendLine(string.Join(",",
                            EscapeCsv(provider.Name),
                            EscapeCsv(provider.NPI),
                            EscapeCsv(provider.Specialty),
                            EscapeCsv(provider.Email),
                            EscapeCsv(provider.Phone),
                            EscapeCsv(credential.Name),
                            EscapeCsv(credential.IssueDate?.ToShortDateString() ?? ""),
                            EscapeCsv(credential.ExpirationDate?.ToShortDateString() ?? ""),
                            EscapeCsv(credential.Status),
                            EscapeCsv(credential.FilePath),
                            EscapeCsv(credential.Notes)));
                    }
                }

                await using var stream = await file.OpenWriteAsync();
                using var writer = new StreamWriter(stream);
                await writer.WriteAsync(csv.ToString());

                if (DataContext is MainWindowViewModel vm)
                {
                    vm.StatusMessage = "CSV report exported successfully";
                    vm.IsError = false;
                }

            }catch(Exception ex)
            {
                if (DataContext is MainWindowViewModel vm)
                {
                    vm.StatusMessage = $"Failed to export CSV report: {ex.Message}";
                    vm.IsError= true;
                }
            }
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }

            return value ;
        } 
    }
}