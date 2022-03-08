using Application.Features.Products.Vms;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductVm>().ReverseMap();
        }
    }
}