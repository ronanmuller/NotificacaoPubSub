using AutoMapper;
using NotificacaoPubSub.Domain.Dtos;
using NotificacaoPubSub.Domain.Models;
using System;

namespace NotificacaoPubSub.Domain.Profiles
{
    public class AssinaturaTopicoProfile : Profile
    {
        public AssinaturaTopicoProfile()
        {
            CreateMap<CadastrarAssinaturaTopicoRequest, AssinaturaTopico>()
                .ForMember(x => x.Id, src => src.MapFrom(x => Guid.NewGuid().ToString()))
                .ForMember(x => x.Ativo, src => src.MapFrom(x => true));
        }
    }
}
