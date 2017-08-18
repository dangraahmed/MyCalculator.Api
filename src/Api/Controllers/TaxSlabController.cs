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
        public FeaturedTaxSlabViewModel ListTaxSlabs()
        {
            var vmTaxSlab = new FeaturedTaxSlabViewModel();

            var taxSlabs = _taxSlabBL.GetTaxSlabs();

            foreach (var taxSlab in taxSlabs)
            {
                vmTaxSlab.TaxSlabs.Add(_mapper.Map<TaxSlabViewModel>(taxSlab));
            }

            return vmTaxSlab;
        }
    }
}
