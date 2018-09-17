using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Tutorial.Models;
using Tutorial.Utils;
using System.Data.Common;
using System.Data.SqlClient;

namespace Tutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : MyBaseClass
    {
        public UserController(DataContext ctx, IConfiguration config)
        {
            context = ctx;
            configuration = config;
        }

        [HttpPost("authenticate")]
        public Message Authenticate([FromBody] UserAuth auth)
        {
            mess = new Message(1);

            try
            {
                int exists = 0;

                string procedure = "EXEC dbo.AUTH_USER @USERNAME, @PASSWD, @IT_EXISTS OUT";
                SqlParameter user = new SqlParameter("@USERNAME", auth.Username);
                SqlParameter pass = new SqlParameter("@PASSWD", auth.Password);
                SqlParameter exi = new SqlParameter("@IT_EXISTS", 0);
                exi.Direction = System.Data.ParameterDirection.Output;
                context.Database.ExecuteSqlCommand(procedure, user, pass, exi);

                Int32.TryParse(exi.Value.ToString(), out exists);

                if (exists > 0)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(configuration["Security:Secret"].ToString());
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, auth.Username)
                        }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);
                    mess = new Message(3) { Id = tokenString };
                }
                else
                {
                    mess.Description = "Usuario y/o contraseña erróneos";
                }
            }
            catch (Exception err)
            {
                mess.Description = configuration["Messages:ErrGeneric"].ToString() + err.Message;
            }

            return mess;
        }
    }
}