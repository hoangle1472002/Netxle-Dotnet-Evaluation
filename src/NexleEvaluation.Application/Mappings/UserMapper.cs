using AutoMapper;
using NexleEvaluation.Application.Models.Responses.Auth;
using NexleEvaluation.Domain.Entities.Identity;

namespace NexleEvaluation.Application.Mappings
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, SignUpResponse>().ReverseMap();
        }
    }
}
