using Hairlytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Interfaces
{
    public interface IAuthRepository
    {
        //Task LoginUserAsync(string username, string password);
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> IsExitsEmailAsync(string email);
        Task<bool> IsExitsPhoneAsync(string phone);
        Task CreateUserAsync(User user);
        Task CreateRefreshToken(RefreshToken refreshToken);
        Task SaveChangesAsync();          
        Task<RefreshToken?> RefreshToken (int userId, string refreshToken);
        Task ForgotPassword(ForgotPassword forgotPassword);
        Task<ForgotPassword?> GetResetPasswordData(string email);
        Task RegisterPhoneNumber(RegisterPhoneNumber registerPhoneNumber);
        Task<EmailVerification?> CheckEmailVarificationExitAsync(string email);
        Task AddEmailVarificationOtpAsync(EmailVerification emailVerification);
       
       

    }
}
