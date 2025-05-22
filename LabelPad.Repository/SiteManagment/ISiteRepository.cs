using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.SiteManagment
{
    public interface ISiteRepository
    {
        Task<dynamic> GetAuditLogs(int PageNo, int PageSize, int RoleId, int SiteId);
        Task<dynamic> GetTenantLogs(int PageNo, int PageSize, int SiteId, int TenantId);
        Task<dynamic> GetNotificatiosList( int TenantId);
        Task<List<Country>> GetCountries();
        Task<dynamic> CloseTicket(int Id);
        Task<dynamic> GetSupportAdminById(int Id);
        Task<dynamic> GetVisitorParkings(string TenantId);
        Task<dynamic> GetVisitorParkingsById(string tenantid,string id);
        Task<dynamic> GetManageParkings(string TenantId);
        Task<dynamic> UpdateSupport(GetSupportCls obj);
        Task<dynamic> GetSupportListAdmin(int PageNo, int PageSize, int SiteId);
        Task<dynamic> GetSupportById(int Id);
        Task<dynamic> AddSite(AddSiteAc objsite);
        Task<dynamic> UpdateSite(AddSiteAc objsite);
        Task<dynamic> saveauditlog(Auditlog objsite);
        Task<dynamic> saveauditlogfornotification(Auditlog objsite);
        Task<dynamic> GetSites(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId);
        Task<dynamic> GetSitesbyoperatorid(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId,int OperatorId);

        Task<List<Site>> GetSiteslogin(int LoginId);
        Task<dynamic> GetSiteById(int Id);
        bool GetExistsSite(AddSiteAc objsite);
        Task<dynamic> DeleteSite(int Id);
        Task<dynamic> GetParkingBayNos(int SiteId, string Date, string EndDate);
        Task<dynamic> GetParkingBayNobysiteid(int SiteId);
        Task<dynamic> GetParkingBayNosEdit(int SiteId, int UserId);
        Task<List<VisitorBaySession>> GetVisitorBaySessions(int SiteId);
        Task<dynamic> BindTimeSlots(string duration, string sessionunit, DateTime date, string SiteId);
        Task<dynamic> DeleteManageParking(int Id, int Bayno);
        Task<dynamic> GetVisitorBayNo(string starttime, string siteid,string date);
        Task<dynamic> GetIsReadNotifications(int RoleId, int LoginId, int SiteId);
        Task<dynamic> GetSearchSites(int PageNo, int PageSize, string SiteName, string Email, string MobileNumber, int SiteId);
        Task<dynamic> GetSearchSupportList(int PageNo, int PageSize, int SiteId, string SiteName, string Name, string Email, string MobileNumber, string Subject);
        Task<dynamic> GetZatParkLogs(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId);
        Task<dynamic> GetSearchZatpark(int PageNo, int PageSize, string Tenant, string Sitename, string BayNo, string Fromdate, string Todate, int SiteId);
        Task<dynamic> GetSearchAuditReport(int PageNo, int PageSize, string Username, string Sitename, string Fromdate, string Todate, int SiteId);
    }
}
