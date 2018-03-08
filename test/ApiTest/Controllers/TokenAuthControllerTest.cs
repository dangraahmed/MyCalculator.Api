using System;
using Api;
using Api.Auth;
using Api.Controllers;
using ApiTest.Common;
using AutoMapper;
using Core.Interface;
using Dto.Object;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace ApiTest.Controllers
{
    public class TokenAuthControllerTest
    {
        private UserRepositoryTestImp dataModelRepositoryTest;
        private Mock<IUserBL> mockUserBL;
        private Mock<ITokenAuthOptionWrapper> mockTokenAuthOption;
        private TokenAuthController controller;

        public TokenAuthControllerTest()
        {
            dataModelRepositoryTest = new UserRepositoryTestImp();
            SetUpTest();
        }

        [Fact]
        public void CheckTokenAuthControllerGaurdClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TokenAuthController(null, null));
        }

        [Fact]
        public void GetAuthTokenTestForValidUser()
        {
            //Arrange
            UserMasterViewModel userMasterViewModel = new UserMasterViewModel()
            {
                UserId = "a",
                UserPassword = "s"
            };


            mockTokenAuthOption.Setup(rep => rep.Audience).Returns("ExampleAudience");
            mockTokenAuthOption.Setup(rep => rep.Issuer).Returns("ExampleIssuer");
            mockTokenAuthOption.Setup(rep => rep.SigningCredentials).Returns(new SigningCredentials(new RsaSecurityKey(RSAKeyHelper.GenerateKey()), SecurityAlgorithms.RsaSha256Signature));
            mockTokenAuthOption.Setup(rep => rep.ExpiresSpan).Returns(TimeSpan.FromSeconds(6000));
            mockTokenAuthOption.Setup(rep => rep.TokenType).Returns("Bearer");

            //// Act
            var tokenValue = controller.GetAuthToken(userMasterViewModel);

            //// Asser Result
            Assert.Contains("Bearer", tokenValue);
        }

        [Fact]
        public void GetAuthTokenTestForInValidUser()
        {
            //Arrange
            UserMasterViewModel userMasterViewModel = new UserMasterViewModel()
            {
                UserId = "xyz",
                UserPassword = "abc"
            };

            // Act
            var tokenValue = controller.GetAuthToken(userMasterViewModel);

            // Asser Result
            Assert.Contains("Username or password is invalid", tokenValue);
        }

        #region private method
        private void SetUpTest()
        {
            //Arrange
            mockUserBL = new Mock<IUserBL>();
            mockTokenAuthOption = new Mock<ITokenAuthOptionWrapper>();
            mockUserBL.Setup(repo => repo.GetUsers()).Returns(dataModelRepositoryTest.GetUsers());
            controller = new TokenAuthController(mockUserBL.Object, mockTokenAuthOption.Object);
        }

        #endregion
    }
}
