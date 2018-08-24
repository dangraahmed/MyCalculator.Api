using AutoMapper;
using Core.Model;
using Dto.Object;

namespace Api.Common
{
    public class CoreToDto : Profile
    {
        public CoreToDto()
        {
            CreateMap<TaxSlab, TaxSlabViewModel>();
            CreateMap<TaxSlabDetail, TaxSlabDetailViewModel>();
            CreateMap<UserMaster, UserMasterViewModel>();
            CreateMap<TaxSlab, FeaturedTaxSlabViewModel>();
        }
    }

    public class DtoToCore : Profile
    {
        public DtoToCore()
        {
            CreateMap<TaxSlabViewModel, TaxSlab>();
            CreateMap<TaxSlabDetailViewModel, TaxSlabDetail>();
            CreateMap<UserMasterViewModel, UserMaster>();
            CreateMap<FeaturedTaxSlabViewModel, TaxSlab>();
        }
    }
}
