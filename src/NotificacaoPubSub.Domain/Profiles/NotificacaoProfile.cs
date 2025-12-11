using AutoMapper;
using NotificacaoPubSub.Domain.Models;
using System;
using System.Text.Json;

namespace NotificacaoPubSub.Domain.Profiles
{
    public class NotificacaoProfile : Profile
    {
        public NotificacaoProfile()
        {
            CreateMap<MensagemNotificacao, HistoricoNotificacao>()
                .ForMember(x => x.DataInclusao, src => src.MapFrom(x => DateTime.Now))
                .ForMember(x => x.Id, src => src.MapFrom(x => x.Id))
                .ForMember(x => x.Payload, src => src.MapFrom(x => x.Payload));
        }
    }
}
