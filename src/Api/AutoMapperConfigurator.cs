using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Model;
using Dto.Object;

namespace Api
{
    public class CoreToDto : Profile
    {
        public CoreToDto()
        {
            CreateMap<TaxSlab, TaxSlabViewModel>();
            CreateMap<TaxSlabDetail, TaxSlabDetailViewModel>();
            CreateMap<UserMaster, UserMasterViewModel>();
        }
    }

    public class DtoToCore : Profile
    {
        public DtoToCore()
        {
            CreateMap<TaxSlabViewModel, TaxSlab>();
            CreateMap<TaxSlabDetailViewModel, TaxSlabDetail>();
            CreateMap<UserMasterViewModel, UserMaster>();
        }
    }
}
