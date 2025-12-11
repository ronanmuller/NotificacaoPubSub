using AutoMapper;
using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Models;
using System;

namespace NotificacaoPubSub.Domain.Profiles
{
    public class TopicoProfile : Profile
    {
        public TopicoProfile()
        {
            CreateMap<CadastrarTopicoRequest, Topico>()
                .ForMember(x => x.Id, src => src.MapFrom(x => Guid.NewGuid().ToString()));
        }
    }
}
