using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
	public interface ISessionService
	{
		UpdateSessionViewModel? GetSessionToUpdate(int sessionId);
		bool UpdateSession(int sessionId, UpdateSessionViewModel UpdateSession);
		IEnumerable<SessionViewModel> GatAllSessions();
		SessionViewModel? GetSessionById(int sessionId);
		bool CreateSession(CreateSessionViewModel createSession);
		bool RemoveSession(int sessionId);
		
	}
}
