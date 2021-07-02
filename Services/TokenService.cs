using CeluGamaSystem.Dtos;
using CeluGamaSystem.Models;
using CeluGamaSystem.Platform.Clients;
using CeluGamaSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CeluGamaSystem.Services
{
    public class TokenService : ITokenService
    {
        private readonly CeluGamaDbContext _context;

        public TokenService(CeluGamaDbContext context)
        {
            _context = context;
        }
    }
}
