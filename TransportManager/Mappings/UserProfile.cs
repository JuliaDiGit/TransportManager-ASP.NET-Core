using AutoMapper;
using Domain;
using Domain.Authorization;
using Entities;
using Models;
using Models.Authorization;

namespace TransportManager.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRequestModel, UserRequest>();
            CreateMap<UserRequest, UserEntity>();
            CreateMap<UserEntity, UserResponse>();
            CreateMap<UserResponse, UserResponseModel>();

            // регистрация и авторизация
            CreateMap<UserRegistrationRequestModel, UserRegistrationRequest>();
            CreateMap<UserRegistrationRequest, UserRequest>();
            CreateMap<UserEntity, UserAuthenticateResponse>();
            CreateMap<UserAuthenticateResponse, UserAuthenticateResponseModel>();

        }
    }
}
