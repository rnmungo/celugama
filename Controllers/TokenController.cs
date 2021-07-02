using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using RestSharp;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CeluGamaSystem.Models;
using CeluGamaSystem.Dtos;
using CeluGamaSystem.Platform.Clients;

namespace CeluGamaSystem.Controllers
{
    [Route("api/tokens")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly CeluGamaDbContext _context;

        public TokenController(CeluGamaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Token>> GetToken()
        {
            Token token = await _context.Tokens.FirstOrDefaultAsync();
            return token;
        }

        [HttpPost]
        public async Task<ActionResult<Token>> PostToken([FromBody] TokenCode Tokenizer)
        {
            string AppID = "";
            string SecretKey = "";
            Token meliToken = _context.Tokens.FirstOrDefault();
            if (meliToken != null)
            {
                AppID = meliToken.AppID;
                SecretKey = meliToken.SecretKey;
            }

            MercadoLibreClient client = new MercadoLibreClient(AppID, SecretKey);
            IRestResponse Response = client.Authorize(Tokenizer.Code);

            if (Response.StatusCode.Equals(HttpStatusCode.OK))
            {
                TokenMeLi TokenMeLi = JsonConvert.DeserializeObject<TokenMeLi>(Response.Content);
                bool exists = ExistAnyToken();
                Token Token = exists ? await _context.Tokens.FirstAsync() : new Token();
                Token.AccessToken = TokenMeLi.AccessToken;
                Token.RefreshToken = TokenMeLi.RefreshToken;
                Token.DueDateTime = DateTimeOffset.UtcNow;
                _context.Entry(Token).State = exists ? EntityState.Modified : EntityState.Added;

                try
                {
                    await _context.SaveChangesAsync();
                    return Ok(Token);
                }
                catch (Exception Error)
                {
                    await _context.Logs.AddAsync(new Log()
                    {
                        StatusCode = 500,
                        Error = "internal_server_error",
                        Message = Error.Message,
                        StackTrace = Error.StackTrace,
                        Resource = "/oauth/token?grant_type=authorization_code",
                        Level = "Error",
                        CreatedAt = DateTime.UtcNow
                    });
                    await _context.SaveChangesAsync();
                    return StatusCode(500, new ApiError()
                    {
                        StatusCode = 500,
                        Message = Error.Message
                    });
                }
            }

            try
            {
                await _context.Logs.AddAsync(new Log()
                {
                    StatusCode = (int)Response.StatusCode,
                    Error = Response.StatusDescription,
                    Message = Response.Content,
                    StackTrace = "",
                    Resource = "/oauth/token?grant_type=authorization_code",
                    Level = "Error",
                    CreatedAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }
            catch(Exception Error)
            {
                return StatusCode(500, new ApiError()
                {
                    StatusCode = 500,
                    Message = Error.Message
                });
            }

            return BadRequest(new { Message = Response.Content });
        }

        private bool ExistAnyToken()
        {
            return _context.Tokens.ToList().Count > 0;
        }

    }
}