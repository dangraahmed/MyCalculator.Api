using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Dto.Object;
using Dto.Common;
using Core;
using Core.Model;
using Core.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Api.Controllers
{
    [Route("[controller]")]
    public class TaxSlabController : Controller
    {

        private readonly ITaxSlabBL _taxSlabBL;
        private readonly IMapper _mapper;

        public TaxSlabController(ITaxSlabBL taxSlabBL, IMapper mapper)
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
        [Route("listTaxSlabs")]
        public FeaturedTaxSlabListViewModel ListTaxSlabs()
        {
            var vmTaxSlab = new FeaturedTaxSlabListViewModel();

            var taxSlabs = _taxSlabBL.GetTaxSlabs();

            foreach (var taxSlab in taxSlabs)
            {
                vmTaxSlab.TaxSlabs.Add(_mapper.Map<TaxSlabViewModel>(taxSlab));
            }

            return vmTaxSlab;
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

            return vmTaxSlabDetail;
        }

        [HttpPost()]
        //[Auth.Authorize()]
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
                // TODO: -- have to remove this  code (from here)
                // TODO: refactoring required remove duplicate code
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
