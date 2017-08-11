using System.Collections.Generic;
using System.Linq;
using Api;
using Api.Controllers;
using ApiTest.Common;
using AutoMapper;
using Core.Interface;
using Dto.Object;
using Moq;
using Xunit;

namespace ApiTest.Controllers
{
    [TestCaseOrderer("ApiTest.Common.PriorityOrderer", "ApiTest")]
    public class TaxSlabDetailControllerTest
    {
        private TaxSlabRepositoryTestImp dataModelRepositoryTest;
        private Mock<ITaxSlabBL> mockTaxSlabBL;
        private TaxSlabDetailController controller;

        public TaxSlabDetailControllerTest()
        {
            dataModelRepositoryTest = new TaxSlabRepositoryTestImp();
            SetUpTest();
        }

        [Theory]
        [InlineData(1, 4), TestPriority(0)]
        [InlineData(2, 5)]
        [InlineData(3, 6)]
        [InlineData(4, 7)]
        public void ListTaxSlabDetailTestForValidSlabId(int taxSlabId, int taxSlabDetailCount)
        {
            //Arrange
            mockTaxSlabBL.Setup(repo => repo.GetTaxSlabDetail(taxSlabId)).Returns(dataModelRepositoryTest.GetTaxSlabDetail(taxSlabId));

            // Act
            var result = controller.ListTaxSlabDetail(taxSlabId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FeaturedTaxSlabDetailViewModel>(result);
            Assert.IsType<TaxSlabViewModel>(result.TaxSlab);
            Assert.IsType<List<TaxSlabDetailViewModel>>(result.TaxSlabDetail);
            Assert.IsAssignableFrom<IEnumerable<TaxSlabDetailViewModel>>(result.TaxSlabDetail);
            Assert.Equal(taxSlabDetailCount, result.TaxSlabDetail.Count());

            foreach (var taxSlabDetail in result.TaxSlabDetail)
            {
                Assert.NotNull(taxSlabDetail);
            }
        }

        [Theory]
        [InlineData(5, 0), TestPriority(1)]
        public void ListTaxSlabDetailTestForInValidSlabId(int taxSlabId, int taxSlabDetailCount)
        {
            //Arrange
            mockTaxSlabBL.Setup(repo => repo.GetTaxSlabDetail(taxSlabId)).Returns(dataModelRepositoryTest.GetTaxSlabDetail(taxSlabId));

            // Act
            var result = controller.ListTaxSlabDetail(taxSlabId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FeaturedTaxSlabDetailViewModel>(result);
            Assert.Null(result.TaxSlab);
            Assert.IsType<List<TaxSlabDetailViewModel>>(result.TaxSlabDetail);
            Assert.IsAssignableFrom<IEnumerable<TaxSlabDetailViewModel>>(result.TaxSlabDetail);
            Assert.Equal(taxSlabDetailCount, result.TaxSlabDetail.Count());
        }

        [Theory]
        [InlineData(-1, 3), TestPriority(2)]
        public void ListTaxSlabDetailTestForAddMode(int taxSlabId, int taxSlabDetailCount)
        {
            //Arrange
            mockTaxSlabBL.Setup(repo => repo.GetTaxSlabDetail(taxSlabId)).Returns(dataModelRepositoryTest.GetTaxSlabDetail(taxSlabId));

            // Act
            var result = controller.ListTaxSlabDetail(taxSlabId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FeaturedTaxSlabDetailViewModel>(result);
            Assert.NotNull(result.TaxSlab);
            Assert.IsType<List<TaxSlabDetailViewModel>>(result.TaxSlabDetail);
            Assert.IsAssignableFrom<IEnumerable<TaxSlabDetailViewModel>>(result.TaxSlabDetail);
            Assert.Equal(taxSlabDetailCount, result.TaxSlabDetail.Count());

            foreach (var taxSlabDetail in result.TaxSlabDetail)
            {
                Assert.NotNull(taxSlabDetail);
            }
        }

        [Theory]
        [InlineData(1, 4), TestPriority(3)]
        public void DeleteTaxSlabTestValidTaxSlabId(int taxSlabId, int taxSlabDetailCount)
        {
            //Before Delete
            ListTaxSlabDetailTestForValidSlabId(taxSlabId, taxSlabDetailCount);
            
            //Arrange
            mockTaxSlabBL.Setup(repo => repo.DeleteTaxSlab(taxSlabId)).Returns(dataModelRepositoryTest.DeleteTaxSlab(taxSlabId));

            // Act
            var result = controller.DeleteTaxSlab(taxSlabId);

            // Asser Result
            Assert.NotNull(result);
            Assert.True(result);

            //After Delete
            ListTaxSlabDetailTestForInValidSlabId(taxSlabId, 0);
        }

        [Theory]
        [InlineData(10, 4), TestPriority(4)]
        public void DeleteTaxSlabTestInValidTaxSlabId(int taxSlabId, int taxSlabDetailCount)
        {
            //Arrange
            mockTaxSlabBL.Setup(repo => repo.DeleteTaxSlab(taxSlabId)).Returns(dataModelRepositoryTest.DeleteTaxSlab(taxSlabId));

            // Act
            var result = controller.DeleteTaxSlab(taxSlabId);

            // Asser Result
            Assert.NotNull(result);
            Assert.False(result);
        }

        private void SetUpTest()
        {
            //Arrange
            mockTaxSlabBL = new Mock<ITaxSlabBL>();
            mockTaxSlabBL.Setup(repo => repo.GetTaxSlabs()).Returns(dataModelRepositoryTest.GetTaxSlabs());
            Mapper.Initialize(m => m.AddProfile<CoreToDto>());
            controller = new TaxSlabDetailController(mockTaxSlabBL.Object, Mapper.Instance);
        }
    }
}
