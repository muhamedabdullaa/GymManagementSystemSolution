using AutoMapper;
using GymManagementBLL.Services.AttachmentService;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
	public class MemberService : IMemberService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper mapper;
		private readonly IAttachmentService attachmentService;

		public MemberService(IUnitOfWork unitOfWork , IMapper mapper ,IAttachmentService attachmentService)
		{
			_unitOfWork = unitOfWork;
			this.mapper = mapper;
			this.attachmentService = attachmentService;
		}

		public bool CreateMember(CreateMemberViewModel Createdmember)
		{
			try
			{
				//Check Email Exist


				if (IsEmailExist(Createdmember.Email) || IsPhoneExist(Createdmember.Phone)) return false;

				var PhotoName = attachmentService.Upload("members",Createdmember.PhotoFile);
				if (string.IsNullOrEmpty(PhotoName)) return false;

				var member = mapper.Map<Member>(Createdmember);
				member.Photo = PhotoName;
				_unitOfWork.GetRepository<Member>().Add(member);
				var IsCreated = _unitOfWork.SaveChanges() > 0;
				if (!IsCreated)
				{
					attachmentService.Delete(PhotoName,"members");
					return false;
				}
				else
				{
					return IsCreated;
				}
			}

			catch (Exception)
			{

				return false;
			}
		

		}

		public IEnumerable<MemberViewModel> GetAllMembers()
		{
			var Members = _unitOfWork.GetRepository<Member>().GetAll();
			if (Members is null && !Members.Any()) return [];
			//var MemberViewModel = Members.Select(X => new MemberViewModel
			//{
			//	Id = X.Id,
			//	Name = X.Name,
			//	Email = X.Email,
			//	Phone = X.Phone,
			//	photo = X.Photo,
			//	Gender = X.Gender.ToString()

			//});
			var MemberViewModel = mapper.Map<IEnumerable<MemberViewModel>>(Members);
			return MemberViewModel;  

		}

		public HealthRecordViewModel? GetHealthRecordDetails(int id)
		{
			var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(id);
			if(MemberHealthRecord is null) return null;
			var HealthRecordVm = mapper.Map<HealthRecordViewModel>(MemberHealthRecord);
			return HealthRecordVm;
			//return new HealthRecordViewModel()
			//{
			//	Height = MemberHealthRecord.Height,
			//	Weight = MemberHealthRecord.Weight,
			//	BloodType = MemberHealthRecord.BloodType,
			//	Note = MemberHealthRecord.Note
			//};
		}

		public MemberViewModel? GetMemberDetails(int id)
		{
			var member = _unitOfWork.GetRepository<Member>().GetById(id);
			if (member is null) return null;
			var memberVM = mapper.Map<MemberViewModel>(member);
			//var memberVM = new MemberViewModel()
			//{
			//	Name = member.Name,
			//	Email = member.Email,
			//	Phone = member.Phone,
			//	Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City} ",
			//	Gender = member.Gender.ToString(),
			//	DateOfBirth = member.DateOfBirth.ToShortDateString(),

			//};
			var ActiveMemberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.MemberId == id && X.Status == "Active").FirstOrDefault();
			if(ActiveMemberShip is not null)
			{
				memberVM.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
				memberVM.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
				var Plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
				
				memberVM.PlaneName = Plan?.Name;

				
			}
			return memberVM;


		}

		public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
		{
			var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
			if (member is null) return null;
			return new MemberToUpdateViewModel()
			{
				Name = member.Name,
				Email = member.Email,
				Phone = member.Phone,
				Photo = member.Photo,
				BuildingNumber = member.Address.BuildingNumber,
				City = member.Address.City,
				Street = member.Address.Street,
			};
		}

		public bool RemoveMember(int MemberId)
		{
			var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
			if (member is null) return false;
			var HasActiveMemberSession = _unitOfWork.GetRepository<MemberSession>().GetAll(X => X.MemberId == MemberId && X.Session.StartDate > DateTime.Now).Any();
			if (HasActiveMemberSession) return false;
			var memberShips = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.MemberId == MemberId);
			try
			{
				foreach (var item in memberShips)
				{
					_unitOfWork.GetRepository<MemberShip>().Delete(item);
				}
				 _unitOfWork.GetRepository<Member>().Delete(member) ;
				var IsDeleted = _unitOfWork.SaveChanges() > 0;
				if(IsDeleted)
				{
					attachmentService.Delete(member.Photo,"members");
				}
				return IsDeleted;
			}
			catch (Exception)
			{

				return false;
			}
		}

		public bool UpdateMemberDetails(int id, MemberToUpdateViewModel UpdatedMember)
		{
			try
			{
				
				var emailExist = _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == UpdatedMember.Email && X.Id != id);
				var phoneExist = _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == UpdatedMember.Phone && X.Id != id);
				if (emailExist.Any() || phoneExist.Any()) return false;
				var Member = _unitOfWork.GetRepository<Member>().GetById(id);
				if(Member is null) return false;
				Member.Email = UpdatedMember.Email;
				Member.Phone = UpdatedMember.Phone;
				Member.Address.BuildingNumber = UpdatedMember.BuildingNumber;
				Member.Address.City = UpdatedMember.City;
				Member.Address.Street = UpdatedMember.Street;
				Member.UpdatedAt = DateTime.Now;
				 _unitOfWork.GetRepository<Member>().Update(Member) ;
				return _unitOfWork.SaveChanges() > 0;


			}
			catch (Exception)
			{

				return false;
			}
		}

		#region Helper Method
		private bool IsEmailExist(string email)
		{
			return _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == email).Any();
		}
		private bool IsPhoneExist(string phone)
		{
			return _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == phone).Any();
		}
		#endregion
	}
}
