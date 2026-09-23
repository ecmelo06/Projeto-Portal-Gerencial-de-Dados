using Microsoft.AspNetCore.Identity;
using Project.Models;

namespace Project.Helpers
{
    public static class HashHelper{
        public static string GerarHash(string senha){
            var hasher = new PasswordHasher<CadastroModel>();

            return hasher.HashPassword(null!, senha);
        }

        
        public static bool VerificarHash(string hashSalvo, string senhaDigitada){
            var hasher = new PasswordHasher<CadastroModel>();

            var resultado = hasher.VerifyHashedPassword(
                null!,
                hashSalvo,
                senhaDigitada);

            return resultado == PasswordVerificationResult.Success;
        }
    }
}