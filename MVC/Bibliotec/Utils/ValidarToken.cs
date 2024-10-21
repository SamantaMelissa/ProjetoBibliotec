using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Bibliotec.Utils
{
    public static class ValidarToken
    {
        public static bool Validar(HttpContext httpContext)
        {

            // if (httpContext.Request.Cookies.ContainsKey("tokenBibliotec"))
            // {
            //     string valorCookie = httpContext.Request.Cookies["tokenBibliotec"]!;
            //     //Descriptografar
            //     try
            //     {
            //         var tokenHandler = new JwtSecurityTokenHandler();
            //         var validationParameters = new TokenValidationParameters
            //         {
            //             ValidateIssuerSigningKey = true,
            //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bibliotec-chave-autenticacao-dev")),
            //             ValidateIssuer = true,
            //             ValidateAudience = true,
            //             ValidateLifetime = true
            //             // ClockSkew = TimeSpan.Zero // Permite um pequeno desvio na validação da data de expiração
            //         };

            //         // Validar se o Token está com os requisitos validos

            //         tokenHandler.ValidateToken(valorCookie, validationParameters, out SecurityToken validatedToken);

            //         return Ok("");
            //     }
            //     catch (Exception e)
            //     {
            //         return BadRequest(e.Message);
            //     }
            // }else{
            //     return false;
            // }

            try
            {
                int id = Convert.ToInt32(httpContext.User.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value);
                Console.WriteLine(id + "fffff");

                string role = httpContext.User.Claims.First(x => x.Type == ClaimTypes.Role).Value;
                Console.WriteLine(role + "aaaa");

                if (role == "1")
                {
                    // return Ok(ConsultaRepository.ListarConsultas());
                    return true;
                }
                else
                {
                    // return Ok(ConsultaRepository.ListarConsultasMedico(id));
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}