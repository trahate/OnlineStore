using SpreadsheetGear;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelDataRecorder.Common;
using TravelDataRecorder.Common.TravelDataEnum;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;
using TravelDataRecorder.Service.ITravelDataRecorderService;
using static TravelDataRecorder.Common.DownloadFileHelper;

namespace TravelDataRecorder.Controllers
{
    [Authorize]
    public class ReportController : TravelBaseController
    {
        private IWorksheet OSHEET;
        private IWorkbook oWB;
        public ReportController(IUserService _userRepository, IAdminService _adminRepository, ITravelService _TravelRepository, ITravelStateService _TravelStateRepository, ITravelCityService _TravelCityRepository, INotificationService _NotificationRepository, IProjectManagerService _objProjectManagerRepository, IDbErrorHandlingService _dbErrorHandlingService) : base(_userRepository, _adminRepository, _TravelRepository, _TravelStateRepository, _TravelCityRepository, _NotificationRepository, _objProjectManagerRepository, _dbErrorHandlingService)
        {

        }


        // GET: Report
        [HttpGet]
        public ActionResult Index()
        {
            FilterViewModel objTravelVM = new FilterViewModel();
            objTravelVM.TravelDetailViewModel = new TravelDetailViewModel();
            objTravelVM.TravelDetailViewModel.travelstate = objmanageCommon.GetAllStates();
            objTravelVM.IsExport = false;
            ViewBag.fileName = "";
            return View(objTravelVM);
        }

        [HttpPost]
        public ActionResult Index(FilterViewModel filterViewModel)
        {
            if (filterViewModel.TravelerCostCriteria == (int)CriteriaForNumericEnum.Select || filterViewModel.TravelerCost == null)
            {
                filterViewModel.TravelerCost = 0;
            }

            filterViewModel.lstTravelDetailModel = objmanageTravel.GetFilteredData(filterViewModel);

            filterViewModel.lstTravelDetailModel.ToList().ForEach(x => x.Cost_TotalExpense = Math.Round(x.Cost_TotalExpense.Value, 2));

            filterViewModel.TravelDetailViewModel = new TravelDetailViewModel();

            filterViewModel.TravelDetailViewModel.travelstate = objmanageCommon.GetAllStates();

            if (filterViewModel.TravelStateFrom != (int)CriteriaForStringEnum.Select)
            {
                filterViewModel.TravelDetailViewModel.travelCityFrom = objmanageCommon.GetallCity(filterViewModel.TravelStateFrom);
            }
            if (filterViewModel.TravelStateTo != (int)CriteriaForStringEnum.Select)
            {
                filterViewModel.TravelDetailViewModel.travelCityTo = objmanageCommon.GetallCity(filterViewModel.TravelStateFrom);
            }

            if (filterViewModel.IsExport)
            {
                string fileName = GenerateExcel(filterViewModel);
                ViewBag.fileName = fileName;

            }
            else
            {
                TempData["FilteredReportData"] = filterViewModel;
            }

            return View(filterViewModel);
        }
        private string GenerateExcel(FilterViewModel FilterViewModel)
        {


            this.oWB = Factory.GetWorkbook();
            this.OSHEET = this.oWB.Worksheets[0];
            this.OSHEET.Name = "Report Filters";
            this.OSHEET.Cells.WrapText = false;
            this.OSHEET.Cells.Font.Name = "Calibri";
            this.OSHEET.Cells.Font.Size = 11;

            AddExcelRow(this.OSHEET, "Sr No.", 0, 0, true, false, HAlign.Left, false, true, true, false, 10, "", "");
            AddExcelRow(this.OSHEET, "Project Name", 0, 1, true, false, HAlign.Center, false, true, true, false, 20, "", "");
            AddExcelRow(this.OSHEET, "Prime Contractor", 0, 2, true, false, HAlign.Center, false, true, true, false, 15, "", "");
            AddExcelRow(this.OSHEET, "Contract Number", 0, 3, true, false, HAlign.Center, false, true, true, false, 15, "", "");
            AddExcelRow(this.OSHEET, "Task Order", 0, 4, true, false, HAlign.Center, false, true, true, false, 15, "", "");
            AddExcelRow(this.OSHEET, "COR Name", 0, 5, true, false, HAlign.Center, false, true, true, false, 15, "", "");
            AddExcelRow(this.OSHEET, "Traveler Name", 0, 6, true, false, HAlign.Center, false, true, true, false, 15, "", "");
            AddExcelRow(this.OSHEET, "Traveler Cost", 0, 7, true, false, HAlign.Center, false, true, true, false, 15, "", "");
            AddExcelRow(this.OSHEET, "From State/City", 0, 8, true, false, HAlign.Center, false, true, true, false, 25, "", "");
            AddExcelRow(this.OSHEET, "To State/City", 0, 9, true, false, HAlign.Center, false, true, true, false, 25, "", "");
            int rowIndex = 0;
            foreach (var item in FilterViewModel.lstTravelDetailModel)
            {
                rowIndex++;
                AddExcelRow(this.OSHEET, rowIndex.ToString(), rowIndex, 0, false, false, HAlign.Left, false, false, true, false, 0, "", "");
                AddExcelRow(this.OSHEET, item.Summary_ProjectName, rowIndex, 1, false, false, HAlign.Left, false, false, true, false, 0, "", "");
                AddExcelRow(this.OSHEET, item.Summary_PrimeContractor, rowIndex, 2, false, false, HAlign.Left, false, false, true, false, 0, "", "");
                AddExcelRow(this.OSHEET, item.Summary_ContractNumber, rowIndex, 3, false, false, HAlign.Left, false, false, true, false, 0, "", "");
                AddExcelRow(this.OSHEET, item.Summary_TaskOrder, rowIndex, 4, false, false, HAlign.Left, false, false, true, false, 0, "", "");
                AddExcelRow(this.OSHEET, item.Summary_CorName, rowIndex, 5, false, false, HAlign.Left, false, false, true, false, 0, "", "");
                AddExcelRow(this.OSHEET, item.Summary_TravelerName, rowIndex, 6, false, false, HAlign.Left, false, false, true, false, 0, "", "");
                AddExcelRow(this.OSHEET, item.Cost_TotalExpense.ToString(), rowIndex, 7, false, false, HAlign.Left, false, false, true, false, 0, "", "");
                AddExcelRow(this.OSHEET, item.TravelState1.Name + "/" + item.TravelCity.Name, rowIndex, 8, false, false, HAlign.Left, false, false, true, false, 0, "", "");
                AddExcelRow(this.OSHEET, item.TravelState.Name + "/" + item.TravelCity1.Name, rowIndex, 9, false, false, HAlign.Left, false, false, true, false, 0, "", "");
            }

            IRange range = this.OSHEET.Range[FilterViewModel.lstTravelDetailModel.Count(), 0, FilterViewModel.lstTravelDetailModel.Count(), 9];
            IBorder border = range.Borders[SpreadsheetGear.BordersIndex.EdgeBottom];
            border.LineStyle = SpreadsheetGear.LineStyle.Continous;
            border.Color = SpreadsheetGear.Drawing.Color.GetSpreadsheetGearColor(System.Drawing.Color.Black);
            border.Weight = SpreadsheetGear.BorderWeight.Thin;

            DownloadFileHelper.CreateFolder("ReportData", "Excel"); 
            //delete old files
            DownloadFileHelper.DeleteOldExcelFiles(ExcelExportTypeEnum.ReportFilters);
            //delete old files
            //DownloadFileHelper.DeleteOldFiles();

            string path = System.Web.HttpContext.Current.Server.MapPath("~\\ReportData\\Excel");
            string timestamp = Guid.NewGuid().ToString();
            string fileName = "ReportFilters_" + timestamp + ".xlsx";
            string output = path + "\\" + fileName;
            this.oWB.SaveAs(output, SpreadsheetGear.FileFormat.OpenXMLWorkbook);
            string filePath = output;
            return fileName;
        }

        /// <summary>
        /// Function that will add a row to Excel based on the information passed
        /// </summary>
        /// <param name="OSHEET"></param>
        /// <param name="value"></param>
        /// <param name="stratIndex1"></param>
        /// <param name="lastIndex1"></param>
        /// <param name="isBold"></param>
        /// <param name="isItalic"></param>
        /// <param name="hAlignment"></param>
        /// <param name="isTopBorder"></param>
        /// <param name="isBottomBorder"></param>
        /// <param name="isRightBorder"></param>
        /// <param name="isLeftBorder"></param>
        /// <param name="width"></param>
        /// <param name="formula"></param>
        /// <param name="format"></param>
        private void AddExcelRow(IWorksheet OSHEET, string value, int stratIndex1, int lastIndex1, bool isBold, bool isItalic, HAlign hAlignment, bool isTopBorder, bool isBottomBorder, bool isRightBorder, bool isLeftBorder, float width, string formula, string format)
        {
            if (value != "")
            {
                this.OSHEET.Cells[stratIndex1, lastIndex1].Value = value;
            }
            if (formula != "")
                this.OSHEET.Cells[stratIndex1, lastIndex1].Formula = formula;

            if (format != "")
                this.OSHEET.Cells[stratIndex1, lastIndex1].NumberFormat = format;

            if (width != 0)
                this.OSHEET.Cells[stratIndex1, lastIndex1].ColumnWidth = width;
            if (isBold)
                this.OSHEET.Cells[stratIndex1, lastIndex1].Font.Bold = true;

            if (isItalic)
                this.OSHEET.Cells[stratIndex1, lastIndex1].Font.Italic = true;

            if (hAlignment != null)
                this.OSHEET.Cells[stratIndex1, lastIndex1].HorizontalAlignment = hAlignment;


            IRange range = this.OSHEET.Range[stratIndex1, lastIndex1];
            if (isTopBorder)
            {
                SetBorder(range, SpreadsheetGear.BordersIndex.EdgeTop);
            }
            if (isBottomBorder)
            {
                SetBorder(range, SpreadsheetGear.BordersIndex.EdgeBottom);
            }
            if (isLeftBorder)
            {
                SetBorder(range, SpreadsheetGear.BordersIndex.EdgeLeft);
            }
            if (isRightBorder)
            {
                SetBorder(range, SpreadsheetGear.BordersIndex.EdgeRight);
            }
        }

        /// <summary>
        /// Set Border in excel file
        /// </summary>
        /// <param name="range"></param>
        /// <param name="index"></param>
        private void SetBorder(IRange range, BordersIndex index)
        {
            IBorder border = range.Borders[index];
            border.LineStyle = SpreadsheetGear.LineStyle.Continous;
            border.Color = SpreadsheetGear.Drawing.Color.GetSpreadsheetGearColor(System.Drawing.Color.Black);
            border.Weight = SpreadsheetGear.BorderWeight.Thin;
        }


    }
}
