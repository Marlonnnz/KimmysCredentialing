using KimmysCredentialing.Services;
using KimmysCredentialing.ViewModels;
using Xunit;
using KimmysCredentialing.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimmysCredentialing.Tests.ViewModels
{
    public class ProviderTest
    {

        private static MainWindowViewModel CreateViewModel()
        {
            var providerService = new ProviderService();
            var credentialService = new CredentialService();
            return new MainWindowViewModel(providerService, credentialService, false);
        }

        [Fact]
        public void AddProvider_WithBlankName_SetsErrorMessage()
        {
            var vm = CreateViewModel();

            vm.Name = "";
            vm.AddProviderCommand.Execute(null);

            Assert.Equal("Provider name is required.", vm.StatusMessage);
            Assert.True(vm.IsError);

        }

        [Fact]
        public void AddCredential_With_No_Provider_Selected_SetsErrorMessage()
        {
            var vm = CreateViewModel();

            vm.SelectedProvider = null;
            vm.AddCredentialCommand.Execute(null);

            Assert.Equal("Select a provider before adding a credential.", vm.StatusMessage);
            Assert.True(vm.IsError);
        }

        [Fact]
        public void DeleteCredential_With_No_Selection()
        {
            var vm = CreateViewModel();

            vm.SelectedCredential = null;
            vm.DeleteCredentialCommand.Execute(null);

            Assert.Equal("Select a credential to delete.", vm.StatusMessage);
            Assert.True(vm.IsError);
        }

        [Fact]
        public void UpdateProvider_With_No_Selection()
        {
            var vm = CreateViewModel();

            vm.SelectedProvider = null;
            vm.UpdateProviderCommand.Execute(null);

            Assert.Equal("Select a provider to update.", vm.StatusMessage);
            Assert.True(vm.IsError);

        }

        [Fact]
        public void DeleteProvider_With_No_Selection()
        {
            var vm = CreateViewModel();

            vm.SelectedProvider = null;
            vm.DeleteProviderCommand.Execute(null);

            Assert.Equal("Select a provider to delete.", vm.StatusMessage);
            Assert.True(vm.IsError);
        }
    }
}
