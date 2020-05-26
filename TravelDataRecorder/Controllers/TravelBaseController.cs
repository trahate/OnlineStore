using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using TravelDataRecorder.APIControllers;
using TravelDataRecorder.Common;
using TravelDataRecorder.Model;
using TravelDataRecorder.Service.ITravelDataRecorderService;


namespace TravelDataRecorder.Controllers
{
    public abstract class TravelBaseController : Controller
    {

        public ManageUserAPIController objManageUser;
        public ManageAdminAPIController objmanageAdmin;
        public ManageTravelAPIController objmanageTravel;
        public CommonAPIController objmanageCommon;
        public ManageProjectManagerAPIController objProjectManager;
        private IUserService _userRepository;
        private IAdminService _adminRepository;
        private ITravelService _TravelRepository;
        public INotificationService _NotificationRepository;
        public IProjectManagerService _ProjectManagerRepository;
        public IDbErrorHandlingService _dbErrorHandlingService;
        public TravelBaseController(IUserService _userRepository, IAdminService _adminRepository, ITravelService _TravelRepository, ITravelStateService _TravelStateRepository, ITravelCityService _TravelCityRepository, INotificationService _NotificationRepository, IProjectManagerService _ProjectManagerRepository, IDbErrorHandlingService dbErrorHandlingService)
        {
            objManageUser = new ManageUserAPIController(_userRepository);
            objmanageAdmin = new ManageAdminAPIController(_adminRepository);
            objmanageTravel = new ManageTravelAPIController(_TravelRepository);
            objmanageCommon = new CommonAPIController(_TravelStateRepository, _TravelCityRepository, _NotificationRepository);
            objProjectManager = new ManageProjectManagerAPIController(_ProjectManagerRepository);
            _dbErrorHandlingService = dbErrorHandlingService;
        }
        // GET: Common
        [HttpGet]
        public CustomUserModel CurrentUserDetail()
        {
            FormsAuthenticationTicket retVal = null;
            HttpCookie curCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (curCookie != null && !String.IsNullOrWhiteSpace(curCookie.Value))
            {
                retVal = FormsAuthentication.Decrypt(curCookie.Value);
                var jsonInput = retVal.UserData;
                CustomUserModel user = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomUserModel>(jsonInput);
                return user;
            }
            else
            {
                return null;
            }

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userManager = CurrentUserDetail();

            if (userManager != null)
            {
                ViewBag.Name = userManager.UserName;
                ViewBag.UserId = userManager.ID;
                ViewBag.Role = userManager.RoleId;
                ViewBag.FullName = userManager.FullName;
                ViewBag.RoleName = userManager.RoleName;
            }

        }
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            //var errorLog = DependencyResolver.Current.GetService<IErrorLogRepository>();
            //CommonHelper.AddErrorLog(errorLog, ex, ErrorTypeEnum.Website.ToString(), "");

            EmailHelper objEmailHelper = new EmailHelper(_dbErrorHandlingService);

            ErrorLogModel objErrorLog = new ErrorLogModel();
            objErrorLog.ErrorInnerException = ex.InnerException != null ? ex.InnerException.ToString() : "";
            objErrorLog.ErrorInnerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message.ToString() : "";
            objErrorLog.ErrorMessage = ex.Message.ToString();
            objErrorLog.ErrorTimeStamp = DateTime.Now;
            objErrorLog.StackTrace = ex.StackTrace.ToString();

            EmailHelper.AddErrorLog(objErrorLog);
            TempData["error"] = ex;
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Error" }));
        }

    }
}