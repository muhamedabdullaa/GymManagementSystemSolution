using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
	[Authorize(Roles = "SuperAdmin")]
	public class MemberController : Controller
	{
		private readonly IMemberService memberService;

		public MemberController(IMemberService memberService)
		{
			this.memberService = memberService;
		}
		#region GetAll Members
		public ActionResult Index()
		{
			var members = memberService.GetAllMembers();
			return View(members);
		}
		#endregion
		#region Get Member Details
		public ActionResult MemberDetails(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative";
				return RedirectToAction(nameof(Index));
			}
				
			var member = memberService.GetMemberDetails(id);
			if(member == null)
			{
				TempData["ErrorMessage"] = "Member Is Not Found";
				return RedirectToAction(nameof(Index));
			}
				

			return View(member);

		}
		#endregion
		#region Get Health Record Details
		public ActionResult HealthRecordDetails(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative";
				return RedirectToAction(nameof(Index));
			}
			var HealthRecord = memberService.GetHealthRecordDetails(id);
			if(HealthRecord is null)
			{
				TempData["ErrorMessage"] = "Health Record  Is Not Found";
				return RedirectToAction(nameof(Index));
			}
				
			return View(HealthRecord);
		}
		#endregion
		#region Create Member
		public ActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Create(CreateMemberViewModel createMember)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
				return View(nameof(Create),createMember);
			}
			bool Result = memberService.CreateMember(createMember);
			if (Result) 
			{
				TempData["Success Message"] = "Member Created Successfully";
			}
			else
				TempData["Error Message"] = "Member Failed To Create , Check Email And Phone";
			return RedirectToAction(nameof(Index));
		}
		#endregion
		#region Edit Member
		public ActionResult MemberEdit(int id)
		{
			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative";
				return RedirectToAction(nameof(Index));
			}
			var member = memberService.GetMemberToUpdate(id);
			if (member is null)
			{
				TempData["ErrorMessage"] = "Member  Is Not Found";
				return RedirectToAction(nameof(Index));
			}
			return View(member);
		}
		[HttpPost]
		public ActionResult MemberEdit([FromRoute] int id , MemberToUpdateViewModel MemberToEdit)
		{
			if (!ModelState.IsValid)
				return View(MemberToEdit);
			var Result = memberService.UpdateMemberDetails(id, MemberToEdit);
			if (Result)
			{
				TempData["SuccessMessage"] = "Member Updated Successfully";
			}
			else
			{
				TempData["ErrorMessage"] = "Member Failed To Update";
			}
			return RedirectToAction(nameof(Index));

		}
		#endregion
		#region Delete Member
		public ActionResult Delete(int id)
		{

			if (id <= 0)
			{
				TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative";
				return RedirectToAction(nameof(Index));
			}
			var member = memberService.GetMemberDetails(id);
			if (member is null)
			{
				TempData["ErrorMessage"] = "Member  Is Not Found";
				return RedirectToAction(nameof(Index));
			}
			ViewBag.MemberId = id;
			return View();
		}
		[HttpPost]
		public ActionResult DeleteConfirmed([FromForm] int id)
		{
			var Result = memberService.RemoveMember(id);
			if (Result)
			{
				TempData["SuccessMessage"] = "Member  Is Deleted Successfully";
				return RedirectToAction(nameof(Index));
			}
			else
			{
				TempData["SuccessMessage"] = "Member Can Bot Be  Deleted ";
				return RedirectToAction(nameof(Index));
			}
			
		}
		#endregion

	}
}
