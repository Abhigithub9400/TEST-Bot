using MediAssist.Application.Abstract.Entities;
using MediAssist.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Services
{
    public interface IUserService
    {
        Task<ILoginUserDetails> SignInWithEmailAndPasswordAsync(string email, string password);

        Task<HttpStatusCode> SignUpWithEmailAndPasswordAsync(ISignUpUserDetails signUpUserDetails);

        Task<bool> CheckWhetherEmailExistOrNot(string emailId);
       
        Task<bool> CheckWhetherCurrentPasswordCorrectOrNot(string userId, string password);

        Task<(int count, bool succeeded)> UpdateCounterAsync(string userId, ApplicationUser user);

        Task<IUserConfigurations> GetUserConfigurationAsync(string userId);

        Task<bool> SetUserConfiguration(string userId,int planid = 1);

    }
}
