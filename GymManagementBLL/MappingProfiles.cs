using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			MapSession();
			MapMember();
			MapTrainer();
		}
		private void MapTrainer()
		{
			CreateMap<Trainer, TrainerViewModel>();

			CreateMap<UpdateTrainerViewModel, Trainer>();
			CreateMap<CreateTrainerViewModel, Address>()
				.ForMember(X=>X.BuildingNumber , Options => Options.MapFrom(X=>X.BuildingNumber))
				.ForMember(X => X.Street, Options => Options.MapFrom(X => X.Street))
				.ForMember(X => X.City, Options => Options.MapFrom(X => X.City));
			CreateMap<CreateTrainerViewModel, Trainer>()
				.ForMember(dest=>dest.Address , Options => Options.MapFrom(Src=>Src));

			


		}
		private void MapSession()
		{
			CreateMap<Session, SessionViewModel>()
				.ForMember(dest => dest.CategoryName, Options => Options.MapFrom(src => src.Category.CategoryName))
				.ForMember(dest => dest.TrainerName, Options => Options.MapFrom(src => src.Trainer.Name))
				.ForMember(dest => dest.AvailableSlots, Options => Options.Ignore());
			CreateMap<Session, CreateSessionViewModel>().ReverseMap();
			CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
		}
		private void MapMember()
		{
			CreateMap<CreateMemberViewModel, Member>()
				.ForMember(dest => dest.Address, Options => Options.MapFrom(src => src));
			CreateMap<CreateMemberViewModel, Address>()
				.ForMember(dest => dest.BuildingNumber , Options =>Options.MapFrom(src => src.BuildingNumber))
				.ForMember(dest => dest.Street, Options => Options.MapFrom(src => src.Street))
				.ForMember(dest => dest.City, Options => Options.MapFrom(src => src.City));
			CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

			CreateMap<Member, MemberViewModel>()
				.ForMember(dest => dest.Gender, Options => Options.MapFrom(src => src.Gender.ToString()))
				.ForMember(dest => dest.Address , Options => Options.MapFrom(src => $"{src.Address.BuildingNumber}-{src.Address.Street}-{src.Address.City}"))
				.ForMember(dest => dest.DateOfBirth , Options => Options.MapFrom(src => src.DateOfBirth.ToShortDateString()));

			CreateMap<Member, MemberToUpdateViewModel>()
				.ForMember(dest => dest.BuildingNumber, Options => Options.MapFrom(src => src.Address.BuildingNumber))
				.ForMember(dest => dest.Street, Options => Options.MapFrom(src => src.Address.Street))
				.ForMember(dest => dest.City, Options => Options.MapFrom(src => src.Address.City));

			CreateMap<MemberToUpdateViewModel, Member>()
				.ForMember(dest => dest.Name, opt => opt.Ignore())
				.ForMember(dest => dest.Photo, opt => opt.Ignore())
				.AfterMap((src,dest) =>
				{
					dest.Address.BuildingNumber = src.BuildingNumber;
					dest.Address.Street = src.Street;
					dest.Address.City = src.City;
					dest.UpdatedAt = DateTime.Now;
				});
		}
	}
}
