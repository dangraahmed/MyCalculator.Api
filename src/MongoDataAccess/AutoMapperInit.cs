using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Model;
using MongoDataAccess.Model;

namespace MongoDataAccess
{
    public class CoreToMongoModel : Profile
    {
        public CoreToMongoModel()
        {
            CreateMap<TaxSlab, MdTaxSlab>().ForMember(mem => mem._id, opt => opt.Ignore());
            CreateMap<TaxSlabDetail, MdTaxSlabDetail>().ForMember(mem => mem._id, opt => opt.Ignore());
        }
    }

    public class MongoModelToCore : Profile
    {
        public MongoModelToCore()
        {
            CreateMap<MdTaxSlab, TaxSlab>();
            CreateMap<MdTaxSlabDetail, TaxSlabDetail>();
        }
    }

    public static class AutoMapperInit
    {
        public static IMapper Configuration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CoreToMongoModel>();
                cfg.AddProfile<MongoModelToCore>();
            });

            return config.CreateMapper();
        }
    }
}
