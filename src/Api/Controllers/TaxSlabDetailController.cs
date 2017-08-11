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
        [Auth.Authorize()]
        [Route("listTaxSlabDetail/{id}")]
        public FeaturedTaxSlabDetailViewModel ListTaxSlabDetail(int id)
        {
            var vmTaxSlabDetail = new FeaturedTaxSlabDetailViewModel();

            vmTaxSlabDetail.TaxSlab = _mapper.Map<TaxSlabViewModel>(_taxSlabBL.GetTaxSlabs().FirstOrDefault(slab => slab.Id == id));

            var taxSlabDetail = _taxSlabBL.GetTaxSlabDetail(id);

            foreach (var detail in taxSlabDetail)
            {
                vmTaxSlabDetail.TaxSlabDetail.Add(_mapper.Map<TaxSlabDetailViewModel>(detail));
            }

            if (id != -1) return vmTaxSlabDetail;
            vmTaxSlabDetail.TaxSlab = new TaxSlabViewModel() { Id = id};
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
        [Auth.Authorize()]
        [Route("insertUpdateTaxSlab")]
        public bool InsertUpdateTaxSlab([FromBody] FeaturedTaxSlabDetailViewModel featuredTaxSlabDetailViewModel)
        {
            try
            {
                return _taxSlabBL.InsertUpdateTaxSlab(_mapper.Map<TaxSlab>(featuredTaxSlabDetailViewModel.TaxSlab), _mapper.Map<IList<TaxSlabDetail>>(featuredTaxSlabDetailViewModel.TaxSlabDetail));
            }
            catch (Exception es)
            {

                throw;
            }
            
        }
    }
}
