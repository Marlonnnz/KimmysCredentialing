using System;
using System.Collections.Generic;
using System.Text;
using KimmysCredentialing.Models;
using KimmysCredentialing.Tests.Fakes;
using KimmysCredentialing.ViewModels;

namespace KimmysCredentialing.Tests.ViewModels
{
    public class CredentialTest
    {
        [Fact]
        public void AddCredential_WithValidInput_CallsCredentialService_AndShowsSuccessMessage()
        {
            var providerService = new FakeProviderService();
            var credentialService = new FakeCredentialService();

            var vm = new MainWindowViewModel(providerService, credentialService, false);

            vm.SelectedProvider = new Provider
            {
                ProviderId = 1,
                Name = "Dr.Smith"
            };

            vm.CredentialName = "DEA License";
            vm.CredentialExpirationDate = DateTimeOffset.Now.AddDays(20);
            vm.CredentialNotes = "Renewal needed soon";

            vm.AddCredentialCommand.Execute(null);

            Assert.Single(credentialService.Credentials);
            Assert.Equal(1, credentialService.Credentials[0].ProviderId);
            Assert.Equal("DEA License", credentialService.Credentials[0].Name);
            Assert.Equal("Credential added successfully.", vm.StatusMessage);
            Assert.False(vm.IsError);

        }


    }
}
