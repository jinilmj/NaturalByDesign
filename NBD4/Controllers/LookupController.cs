using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NBD4.CustomControllers;
using NBD4.Data;

namespace NBD4.Controllers
{
	public class LookupController : CognizantController
	{
		private readonly NBDContext _context;

		public LookupController(NBDContext context)
		{
			_context = context;
		}
        public IActionResult Index(string Tab = "City-Tab")
        {
            //Note: select the tab you want to load by passing in
            //the ID of the tab such as Inventory-Tab
            ViewData["Tab"] = Tab;
            return View();
        }
        public PartialViewResult MaterialType()
		{
			ViewData["MaterialTypeID"] = new
				SelectList(_context.MaterialTypes
				.OrderBy(a => a.MaterialTypeName), "ID", "MaterialTypeName");
			return PartialView("_MaterialType");
		}
		public PartialViewResult City()
		{
			ViewData["CityID"] = new SelectList(_context.Cities
				.OrderBy(c => c.Name), "ID", "Name");
			return PartialView("_City");
		}
		public PartialViewResult Province()
		{
			ViewData["ProvinceID"] = new SelectList(_context.Provinces
				.OrderBy(p => p.Name), "ID", "Name");
			return PartialView("_Province");
		}
		
		public PartialViewResult StaffRole()
		{
			ViewData["StaffRoleID"] = new SelectList(_context.StaffRoles
				.OrderBy(s => s.StaffRoleName), "ID", "StaffRoleName");
			return PartialView("_StaffRole");
		}
		public PartialViewResult LabourTypeInfo()
		{
			ViewData["LabourTypeInfoID"] = new SelectList(_context.LabourTypeInfos
				.OrderBy(l => l.LabourTypeName), "ID", "LabourTypeName");
			return PartialView("_LabourTypeInfo");
		}
		public PartialViewResult Inventory()
		{
			ViewData["InventoryID"] = new SelectList(_context.Inventories
				.OrderBy(i => i.Description), "ID", "Description");
			return PartialView("_Inventory");
		}
        //public PartialViewResult Staff()
        //{
        //    ViewData["StaffID"] = new SelectList(_context.Staffs
        //        .OrderBy(i => i.StaffFirstName), "ID", "StaffFirstName");
        //    return PartialView("_Staff");
        //}

        public PartialViewResult Staff()
        {
            var staffList = _context.Staffs.OrderBy(i => i.StaffFirstName).Select(s => new SelectListItem
            {
                Value = s.ID.ToString(),
                Text = $"{s.FullName} - {s.Email} - {s.PhoneFormatted} - {s.StaffRole.StaffRoleName}"
            });

            ViewData["StaffID"] = new SelectList(staffList, "Value", "Text");
            return PartialView("_Staff");
        }

    }

}
