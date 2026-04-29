using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using KimmysCredentialing.Models;

namespace KimmysCredentialing.Tests.Models
{
    public class CredentialTest
    {

        [Fact]
        public void Status_Returns_NoExpirationDate_When_ExpirationDate_Is_Null()
        {
            var credential = new Credential
            {
                Name = "DEA License",
                ExpirationDate = null
            };

            Assert.Equal("No Expiration Date", credential.Status);
        }

        [Fact]
        public void Status_Returns_Expired_When_ExpirationDate_Is_In_The_Past()
        {
            var credential = new Credential
            {
                Name = "DEA License",
                ExpirationDate = DateTime.Today.AddDays(-1)
            };

            Assert.Equal("Expired", credential.Status);
        }

        [Fact]
        public void Status_Returns_ExpiringSoon_When_ExpirationDate_Is_Within_30_Days()
        {
            var credential = new Credential
            {
                Name = "DEA License",
                ExpirationDate = DateTime.Today.AddDays(10)
            };

            Assert.Equal("Expiring Soon", credential.Status);
        }

        [Fact]
        public void Status_Returns_Active_When_ExpirationDate_Is_More_Than_30_Days()
        {
            var credential = new Credential
            {
                Name = "DEA License",
                ExpirationDate = DateTime.Today.AddDays(45)
            };

            Assert.Equal("Active", credential.Status);
        }

        [Fact]
        public void Status_Returns_ExpiringSoon_When_ExpirationDate_Is_Today()
        {
            var credential = new Credential
            {
                Name = "DEA License",
                ExpirationDate = DateTime.Today
            };

            Assert.Equal("Expiring Soon", credential.Status);
        }

        [Fact]
        public void Status_Returns_ExpiringSoon_Exactly_30_Days()
        {
            var credential = new Credential
            {
                Name = "DEA License",
                ExpirationDate = DateTime.Today.AddDays(30)
            };

            Assert.Equal("Expiring Soon", credential.Status);
        }

        [Fact]
        public void Status_Returns_Active_When_31_Days()
        {
            var credential = new Credential
            {
                Name = "DEA License",
                ExpirationDate = DateTime.Today.AddDays(31)
            };

            Assert.Equal("Active", credential.Status);
        }

        [Fact]
        public void Status_Returns_Expired_When_Very_Old()
        {
            var credential = new Credential
            {
                Name = "DEA License",
                ExpirationDate = DateTime.Today.AddYears(-2)
            };

            Assert.Equal("Expired", credential.Status);
        }
    }
}
