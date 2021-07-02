using System;
using CeluGamaSystem.Models;
using CeluGamaSystem.Dtos;

namespace CeluGamaSystem.Services
{
    public interface ILoginService
    {
        User tryLogin(User user);
        JWToken generateToken(string username);
        void changePassword(User user);
    }
}
