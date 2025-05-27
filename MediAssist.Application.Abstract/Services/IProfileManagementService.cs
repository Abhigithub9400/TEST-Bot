using MediAssist.Application.Abstract.Entities;
using MediAssist.DbContext;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MediAssist.Application.Abstract.Services
{
    public interface IProfileManagementService
    {
        Task<ApplicationUser> FindUserAsync(string userId);
        Task<DoctorProfile> FindUserDetailsAsync(string userId);
        Task<(HttpStatusCode HttpStatusCode, UserTitle? UserTitle)> UpdateProfileAsync(string userId, IUpdateUserDetails updateUserDetails);
        Task<IdentityResult> DeleteUserAccount(ApplicationUser user);
    }
}
