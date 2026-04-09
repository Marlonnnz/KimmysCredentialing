using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using KimmysCredentialing.ViewModels;
using System.Linq;
using System.Diagnostics;
using System.IO;

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

            if(DataContext is MainWindowViewModel vm)
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
    }
}