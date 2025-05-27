using MediAssist.Application.Abstract.Entities;

namespace MediAssist.Application.Abstract.Services
{
    public interface IUserSessionService
    {
        public  Task<IUserSessionDetails> StartOrResumeSession(IStartSessionDetails startSessionDetails);

        public Task<IUserSessionDetails> StopUserSession(int sessionId, string userId, int totalToken, decimal totalCost);

        public Task<IUserSessionDetails> UpdateUserSession(int sessionId, string userId, int totalToken, decimal totalCost);

        public Task<IUserSessionDetails> ReportGenerated(int sessionId, string userId, int totalToken, decimal totalCost);

    }
}
