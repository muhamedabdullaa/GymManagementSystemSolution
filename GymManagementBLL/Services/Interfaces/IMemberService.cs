using GymManagementBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
	public interface IMemberService
	{
	    IEnumerable<MemberViewModel> GetAllMembers();
		bool CreateMember(CreateMemberViewModel Createdmember);
		MemberViewModel? GetMemberDetails(int id);
		HealthRecordViewModel? GetHealthRecordDetails(int id);
		MemberToUpdateViewModel? GetMemberToUpdate(int memberId);
		bool UpdateMemberDetails(int id , MemberToUpdateViewModel UpdatedMember);
		bool RemoveMember(int MemberId);
	}
}
