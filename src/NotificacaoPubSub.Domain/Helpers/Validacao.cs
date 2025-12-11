using System;
using System.IdentityModel.Tokens.Jwt;

namespace NotificacaoPubSub.Domain.Helpers
{
    public class Validacao
    {
        public static void ValidarToken(string token)
        {
            try
            {
                var validarToken = new JwtSecurityToken(token);
            }
            catch (Exception)
            {
                //throw new BancoMasterException("Token inválido");
            }
        }
    }
}
