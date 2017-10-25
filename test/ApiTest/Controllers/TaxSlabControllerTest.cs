using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api;
using Api.Controllers;
using ApiTest.Common;
using AutoMapper;
using Core.Interface;
using Core.Model;
using Dto.Object;
using Moq;
using Xunit;

namespace ApiTest.Controllers
{
    public class TaxSlabControllerTest
    {
        private TaxSlabRepositoryTestImp dataModelRepositoryTest;
        private Mock<ITaxSlabBL> mockTaxSlabBL;
        private TaxSlabController controller;
        public TaxSlabControllerTest()
        {
            dataModelRepositoryTest = new TaxSlabRepositoryTestImp();
            SetUpTest();
        }

        [Fact]
        public void ListTaxSlabsTest()
        {
            // Act
            var result = controller.ListTaxSlabs();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FeaturedTaxSlabListViewModel>(result);
            Assert.IsType<List<TaxSlabViewModel>>(result.TaxSlabs);
            Assert.IsAssignableFrom<IEnumerable<TaxSlabViewModel>>(result.TaxSlabs);
            Assert.Equal(dataModelRepositoryTest.TaxSlabTestData.Count, result.TaxSlabs.Count());

            foreach (var taxSlab in result.TaxSlabs)
            {
                Assert.IsType<TaxSlabViewModel>(taxSlab);
                Assert.NotNull(taxSlab);
            }

        }

        private void SetUpTest()
        {
            //Arrange
            mockTaxSlabBL = new Mock<ITaxSlabBL>();
            mockTaxSlabBL.Setup(repo => repo.GetTaxSlabs()).Returns(dataModelRepositoryTest.GetTaxSlabs());
            Mapper.Initialize(m => m.AddProfile<CoreToDto>());
            controller = new TaxSlabController(mockTaxSlabBL.Object, Mapper.Instance);
        }
    }
}
