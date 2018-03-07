using System;
using System.Collections.Generic;
using System.Linq;
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
    [TestCaseOrderer("ApiTest.Common.PriorityOrderer", "ApiTest")]
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
        [TestPriority(0)]
        public void CheckTaxSlabControllerGaurdClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TaxSlabController(null, null));
        }

        [Fact]
        [TestPriority(1)]
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

        [Theory]
        [InlineData(1, 4), TestPriority(2)]
        //[InlineData(2, 5)]
        //[InlineData(3, 6)]
        //[InlineData(4, 7)]
        public void ListTaxSlabDetailTestForValidSlabId(int taxSlabId, int taxSlabDetailCount)
        {
            //Arrange
            mockTaxSlabBL.Setup(repo => repo.GetTaxSlabDetail(taxSlabId)).Returns(dataModelRepositoryTest.GetTaxSlabDetail(taxSlabId));

            // Act
            var result = controller.ListTaxSlabDetail(taxSlabId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FeaturedTaxSlabDetailListViewModel>(result);
            Assert.IsType<List<TaxSlabDetailViewModel>>(result.TaxSlabDetail);
            Assert.IsAssignableFrom<IEnumerable<TaxSlabDetailViewModel>>(result.TaxSlabDetail);

            Assert.Equal("", result.TaxSlabDetail[0].SSlabFromAmount);
            Assert.Equal("101", result.TaxSlabDetail[1].SSlabFromAmount);
            Assert.Equal("", result.TaxSlabDetail[result.TaxSlabDetail.Count - 1].SSlabToAmount);
            Assert.Equal("300", result.TaxSlabDetail[result.TaxSlabDetail.Count - 2].SSlabToAmount);

            Assert.Equal("Income upto Rs. 100", result.TaxSlabDetail[0].Slab);
            Assert.Equal("Income between Rs. 201 - 300", result.TaxSlabDetail[result.TaxSlabDetail.Count - 2].Slab);
            Assert.Equal("Income above Rs. 301", result.TaxSlabDetail[result.TaxSlabDetail.Count - 1].Slab);

            result.TaxSlabDetail[result.TaxSlabDetail.Count - 1].SlabFromAmount = null;
            result.TaxSlabDetail[result.TaxSlabDetail.Count - 1].SlabToAmount = null;

            Assert.Null(result.TaxSlabDetail[result.TaxSlabDetail.Count - 1].Slab);

            Assert.Equal(taxSlabDetailCount, result.TaxSlabDetail.Count());

            foreach (var taxSlabDetail in result.TaxSlabDetail)
            {
                Assert.NotNull(taxSlabDetail);
            }
        }

        [Theory]
        [InlineData(5, 0), TestPriority(3)]
        public void ListTaxSlabDetailTestForInValidSlabId(int taxSlabId, int taxSlabDetailCount)
        {
            //Arrange
            mockTaxSlabBL.Setup(repo => repo.GetTaxSlabDetail(taxSlabId)).Returns(dataModelRepositoryTest.GetTaxSlabDetail(taxSlabId));

            // Act
            var result = controller.ListTaxSlabDetail(taxSlabId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FeaturedTaxSlabDetailListViewModel>(result);
            Assert.IsType<List<TaxSlabDetailViewModel>>(result.TaxSlabDetail);
            Assert.IsAssignableFrom<IEnumerable<TaxSlabDetailViewModel>>(result.TaxSlabDetail);
            Assert.Equal(taxSlabDetailCount, result.TaxSlabDetail.Count());
        }

        [Theory]
        [InlineData(1, 4), TestPriority(4)]
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
        [InlineData(-10, 4), TestPriority(5)]
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

        [Fact]
        public void InsertTaxSlabTest()
        {
            //Arrange

            var featuredTaxSlabViewModel = GetFeaturedTaxSlabViewModelData(-1);

            mockTaxSlabBL.Setup(repo => repo.GetTaxSlabDetail(It.IsAny<int>()))
                .Returns(dataModelRepositoryTest.GetTaxSlabDetail(It.IsAny<int>())); // TODO: -- have to remove this code from here

            mockTaxSlabBL.Setup(repo => repo.InsertUpdateTaxSlab(It.IsAny<TaxSlab>(), It.IsAny<IList<TaxSlabDetail>>()))
                .Returns(dataModelRepositoryTest.InsertUpdateTaxSlab(Mapper.Map<TaxSlab>(featuredTaxSlabViewModel), Mapper.Map<IList<TaxSlabDetail>>(featuredTaxSlabViewModel.TaxSlabDetail)));

            // Act
            var result = controller.InsertUpdateTaxSlab(featuredTaxSlabViewModel);

            // Asser Result
            Assert.NotNull(result);
        }

        [Fact]
        public void InsertTaxSlabInvalidTest()
        {
            // Asser Result
            Assert.IsType(typeof(NullReferenceException), Record.Exception(() => controller.InsertUpdateTaxSlab(null)));
        }

        #region private method
        private void SetUpTest()
        {
            //Arrange
            mockTaxSlabBL = new Mock<ITaxSlabBL>();
            mockTaxSlabBL.Setup(repo => repo.GetTaxSlabs()).Returns(dataModelRepositoryTest.GetTaxSlabs());
            Mapper.Initialize(m =>
            {
                m.AddProfile<CoreToDto>();
                m.AddProfile<DtoToCore>();
            });
            controller = new TaxSlabController(mockTaxSlabBL.Object, Mapper.Instance);
        }


        private FeaturedTaxSlabViewModel GetFeaturedTaxSlabViewModelData(int Id)
        {
            var fts = new FeaturedTaxSlabViewModel()
            {
                Id = Id,
                FromYear = 2000 + Id,
                ToYear = 2001 + Id,
                Category = "A",
            };

            fts.TaxSlabDetail.Add(new TaxSlabDetailViewModel() { Id = 1, SlabToAmount = 100, Percentage = 110 });
            fts.TaxSlabDetail.Add(new TaxSlabDetailViewModel() { Id = 2, SlabFromAmount = 101, SlabToAmount = 200, Percentage = 110 });
            fts.TaxSlabDetail.Add(new TaxSlabDetailViewModel() { Id = 3, SlabFromAmount = 201, SlabToAmount = 300, Percentage = 110 });
            fts.TaxSlabDetail.Add(new TaxSlabDetailViewModel() { Id = 4, SlabFromAmount = 301, Percentage = 110 });

            return fts;
        }

        #endregion
    }
}
