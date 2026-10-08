using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Project.Helpers
{
    public class EmailService
    {
        public static async Task EnviarCodigoRecuperacao(string emailDestino, string codigo)
        {
            string SMTP_HOST = "smtp-agergs.pro.intra.rs.gov.br"; 
            int SMPT_PORT = 25; 
            string SMPT_USER = "bi-smtp@agergs.rs.gov.br"; 
            string SMTP_PASS = "T@1A07iKZ+Zo"; 
            
            string SENDER_DTI_EMAIL = "dti@agergs.rs.gov.br"; 

            using (var cliente = new SmtpClient(SMTP_HOST, SMPT_PORT))
            {
                cliente.Credentials = new NetworkCredential(SMPT_USER, SMTP_PASS);
                
                // CORREÇÃO: Desabilita o SSL, pois a porta 25 deste servidor não dá suporte a conexões seguras
                cliente.EnableSsl = false; 

                var mensagem = new MailMessage
                {
                    // Ajustado para o e-mail de envio oficial (DTI)
                    From = new MailAddress(SENDER_DTI_EMAIL, "Portal Gerencial - AGERGS"), 
                    Subject = "Código de Recuperação de Senha",
                    Body = $"<h3>Seu código de recuperação é: <b style='color:#0d6efd;'>{codigo}</b></h3><p>Este código expira em 15 minutos.</p>",
                    IsBodyHtml = true
                };
                mensagem.To.Add(emailDestino);

                await cliente.SendMailAsync(mensagem);
            }
        }
    }
}
