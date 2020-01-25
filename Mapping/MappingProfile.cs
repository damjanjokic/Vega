using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.Models;
using Vega.Controllers.Resources;
using Vega.Core.Models;
using System.Linq;
using System.Collections.Generic;

namespace Vega.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap(typeof(QueryResult<>), typeof(QueryResultResource<>));
            CreateMap<Make, MakeResource>().ReverseMap();
            CreateMap<Make, KeyValuePairResource>().ReverseMap();
            CreateMap<Model, KeyValuePairResource>().ReverseMap();
            CreateMap<Feature, KeyValuePairResource>().ReverseMap();
            CreateMap<Vehicle, SaveVehicleResource>()
                .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource{Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone}))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));
            CreateMap<Vehicle, VehicleResource>()
                .ForMember(vr => vr.Make, opt => opt.MapFrom(v => v.Model.Make))
                .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource{Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone}))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => new KeyValuePairResource{Id = vf.Feature.Id, Name = vf.Feature.Name})));


            CreateMap<VehicleQueryResource, VehicleQuery>();
            CreateMap<SaveVehicleResource, Vehicle>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
                .ForMember(v => v.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.Email))
                .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
                .ForMember(v => v.Features, opt => opt.Ignore())
                .AfterMap((vr, v ) => {
                    //REMOVE unselected features

                    // var rf = new List<VehicleFeature>();
                    // foreach(var f in v.Features)
                    //     if(!vr.Features.Contains(f.FeatureId))
                    //     rf.Add(f);
                    
                    // foreach(var f in rf)
                    //     v.Features.Remove(f);    

                    var removedFeatures = v.Features.Where(f => !vr.Features.Contains(f.FeatureId)).ToList();
                    foreach(var f in removedFeatures)
                        v.Features.Remove(f);

                    //ADD selected features

                    // foreach(var id in vr.Features)
                    //     if(v.Features.Any(f => f.FeatureId == id))
                    //         v.Features.Add(new VehicleFeature{ FeatureId = id});

                    var addedFeatures = vr.Features.Where(id => !v.Features.Any(f => f.FeatureId == id)).Select(id => new VehicleFeature{FeatureId = id}).ToList();
                    foreach(var f in addedFeatures)
                        v.Features.Add(f);
                });
    
        }
    }
}

