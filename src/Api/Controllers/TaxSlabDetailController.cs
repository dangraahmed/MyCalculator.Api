using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dto.Object;
using Core;
using Core.Interface;
using AutoMapper;
using Core.Model;
using Api.Common;

namespace Api.Controllers
{
    [Route("[controller]")]
    public class TaxSlabDetailController : Controller
    {
        private readonly ITaxSlabBL _taxSlabBL;
        private readonly IMapper _mapper;

        public TaxSlabDetailController(ITaxSlabBL taxSlabBL, IMapper mapper)
        {
            if (taxSlabBL == null)
            {
                throw new ArgumentNullException(nameof(taxSlabBL));
            }

            _taxSlabBL = taxSlabBL;
            _mapper = mapper;
        }

        [HttpGet()]
        //[Auth.Authorize()]
        [Route("listTaxSlabDetail/{id}")]
        public FeaturedTaxSlabDetailListViewModel ListTaxSlabDetail(int id)
        {
            var vmTaxSlabDetail = new FeaturedTaxSlabDetailListViewModel();

            var taxSlabDetail = _taxSlabBL.GetTaxSlabDetail(id);

            foreach (var detail in taxSlabDetail)
            {
                vmTaxSlabDetail.TaxSlabDetail.Add(_mapper.Map<TaxSlabDetailViewModel>(detail));
            }

            if (id != -1) return vmTaxSlabDetail;
            vmTaxSlabDetail.TaxSlabDetail.Add(new TaxSlabDetailViewModel());
            vmTaxSlabDetail.TaxSlabDetail.Add(new TaxSlabDetailViewModel());
            vmTaxSlabDetail.TaxSlabDetail.Add(new TaxSlabDetailViewModel());

            return vmTaxSlabDetail;
        }

        [HttpPost()]
        [Auth.Authorize()]
        [Route("deleteTaxSlab/{id}")]
        public bool DeleteTaxSlab(int id)
        {
            return _taxSlabBL.DeleteTaxSlab(id);
        }

        [HttpPost()]
        //[Auth.Authorize()]
        [Route("insertUpdateTaxSlab")]
        public FeaturedTaxSlabViewModel InsertUpdateTaxSlab([FromBody] FeaturedTaxSlabViewModel featuredTaxSlabViewModel)
        {
            try
            {
                // TODO: -- have to remove this code (from here)
                TaxSlabViewModel taxSlabViewModel = new TaxSlabViewModel();
                taxSlabViewModel.Id = featuredTaxSlabViewModel.Id;
                taxSlabViewModel.FromYear = featuredTaxSlabViewModel.FromYear;
                taxSlabViewModel.ToYear = featuredTaxSlabViewModel.ToYear;
                taxSlabViewModel.Category = featuredTaxSlabViewModel.Category;
                // TODO: -- have to remove this code (till here)

                int taxSlabId = _taxSlabBL.InsertUpdateTaxSlab(_mapper.Map<TaxSlab>(taxSlabViewModel)
                                                    , _mapper.Map<IList<TaxSlabDetail>>(featuredTaxSlabViewModel.TaxSlabDetail));
                {
                    var taxSlab = _mapper.Map<TaxSlabViewModel>(_taxSlabBL.GetTaxSlabs().FirstOrDefault(slab => slab.Id == taxSlabId));
                    var taxSlabDetail = _mapper.Map<IList<TaxSlabDetailViewModel>>(this.ListTaxSlabDetail(taxSlabId).TaxSlabDetail);

                    var retTaxSlabViewModel = new FeaturedTaxSlabViewModel();
                    retTaxSlabViewModel.Id = taxSlab.Id;
                    retTaxSlabViewModel.FromYear = taxSlab.FromYear;
                    retTaxSlabViewModel.ToYear = taxSlab.ToYear;
                    retTaxSlabViewModel.Category = taxSlab.Category;

                    foreach (var detail in taxSlabDetail)
                    {
                        retTaxSlabViewModel.TaxSlabDetail.Add(detail);
                    }
                    return retTaxSlabViewModel;
                }
            }
            catch (Exception es)
            {
                throw;
            }
        }
    }
}