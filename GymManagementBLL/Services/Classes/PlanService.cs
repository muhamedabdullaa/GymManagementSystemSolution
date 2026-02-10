using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
	public class PlanService : IPlanService
	{
		private readonly IUnitOfWork _unitOfWork;

		public PlanService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IEnumerable<PlanViewModel> GetAllPlans()
		{
			var plans = _unitOfWork.GetRepository<Plan>().GetAll();
			if (plans is null || !plans.Any()) return [];
			return plans.Select(p => new PlanViewModel()
			{
				Id = p.Id,
				Name = p.Name,
				Description = p.Description,
				DurationDays = p.DurationDays,
				Price = p.Price,
				IsActive = p.IsActive,
			});
		}

		public PlanViewModel? GetPLanById(int planId)
		{
			var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
			if (plan is null) return null;
			return new PlanViewModel() 
			{
				Id = plan.Id ,
			    Name = plan.Name, 
				Description = plan.Description,
				DurationDays = plan.DurationDays,
				Price = plan.Price,
				IsActive = plan.IsActive
			};
		}

		public UpdatePlanViewModel? GetPlanToUpdate(int planId)
		{
			var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
			if (plan is null || plan.IsActive == false || HasActiveMemberShip(planId)) return null;
			return new UpdatePlanViewModel()
			{
				PlanName = plan.Name,
				Description = plan.Description,
				DurationDays = plan.DurationDays,
				Price = plan.Price,
			};
		}

		public bool UpdatePlan(int planId, UpdatePlanViewModel updatePlan)
		{
			var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
			if(plan is null || HasActiveMemberShip(planId)) return false;
			try
			{
				(plan.Name, plan.Description , plan.Price , plan.CreatedAt) = 
					(updatePlan.PlanName,updatePlan.Description , updatePlan.Price ,DateTime.Now);
				_unitOfWork.GetRepository<Plan>().Update(plan);
				return _unitOfWork.SaveChanges() > 0;
			}
			catch (Exception)
			{

				return false;
			}
		}
		public bool ToggleStatus(int id)
		{
			var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
			if(plan is null || HasActiveMemberShip(id)) return false;
			try
			{
				plan.IsActive = plan.IsActive == true ? false : true;
				plan.UpdatedAt = DateTime.Now;
				_unitOfWork.GetRepository<Plan>().Update(plan);
				return _unitOfWork.SaveChanges() > 0;
			}
			catch (Exception)
			{

				return false;
			}
			
		}

		
		#region Helper Method
		bool HasActiveMemberShip(int planId)
		{
			var ActiveMemberShips = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.Id == planId && X.Status == "Active");
			return ActiveMemberShips.Any();
		}
		#endregion
	}
}
