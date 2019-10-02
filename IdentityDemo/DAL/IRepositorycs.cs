using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using zAppDev.DotNet.Framework.Data.DAL;
using zAppDev.DotNet.Framework.Identity;

namespace IdentityDemo.DAL
{
    public interface IRepository : ICreateRepository, IIdentityRepository, IDeleteRepository, IRetrieveRepository, IUpdateRepository
    {
        void DeleteApplicationUserAction(zAppDev.DotNet.Framework.Identity.Model.ApplicationUserAction applicationuseraction, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteApplicationUserExternalProfile(zAppDev.DotNet.Framework.Identity.Model.ApplicationUserExternalProfile applicationuserexternalprofile, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteApplicationSetting(zAppDev.DotNet.Framework.Identity.Model.ApplicationSetting applicationsetting, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteApplicationUser(zAppDev.DotNet.Framework.Identity.Model.ApplicationUser applicationuser, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteApplicationRole(zAppDev.DotNet.Framework.Identity.Model.ApplicationRole applicationrole, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteApplicationOperation(zAppDev.DotNet.Framework.Identity.Model.ApplicationOperation applicationoperation, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteApplicationPermission(zAppDev.DotNet.Framework.Identity.Model.ApplicationPermission applicationpermission, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteApplicationClient(zAppDev.DotNet.Framework.Identity.Model.ApplicationClient applicationclient, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteApplicationUserLogin(zAppDev.DotNet.Framework.Identity.Model.ApplicationUserLogin applicationuserlogin, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteApplicationUserClaim(zAppDev.DotNet.Framework.Identity.Model.ApplicationUserClaim applicationuserclaim, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteProfileSetting(zAppDev.DotNet.Framework.Identity.Model.ProfileSetting profilesetting, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteProfile(zAppDev.DotNet.Framework.Identity.Model.Profile profile, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteApplicationLanguage(zAppDev.DotNet.Framework.Identity.Model.ApplicationLanguage applicationlanguage, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteDateTimeFormat(zAppDev.DotNet.Framework.Identity.Model.DateTimeFormat datetimeformat, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteApplicationTheme(zAppDev.DotNet.Framework.Identity.Model.ApplicationTheme applicationtheme, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteAuditEntityConfiguration(zAppDev.DotNet.Framework.Auditing.Model.AuditEntityConfiguration auditentityconfiguration, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteAuditPropertyConfiguration(zAppDev.DotNet.Framework.Auditing.Model.AuditPropertyConfiguration auditpropertyconfiguration, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteAuditLogEntry(zAppDev.DotNet.Framework.Auditing.Model.AuditLogEntry auditlogentry, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteAuditLogEntryType(zAppDev.DotNet.Framework.Auditing.Model.AuditLogEntryType auditlogentrytype, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteAuditLogPropertyActionType(zAppDev.DotNet.Framework.Auditing.Model.AuditLogPropertyActionType auditlogpropertyactiontype, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteWorkflowContextBase(zAppDev.DotNet.Framework.Workflow.WorkflowContextBase workflowcontextbase, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        void DeleteWorkflowSchedule(zAppDev.DotNet.Framework.Workflow.WorkflowSchedule workflowschedule, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null);
        List<T> Get<T>(Expression<Func<T, bool>> predicate, bool cacheQuery = true);
        List<T> Get<T>(Expression<Func<T, bool>> predicate,
                       int startRowIndex,
                       int pageSize,
                       Dictionary<Expression<Func<T, object>>, bool> orderBy,
                       out int totalRecords, bool cacheQuery = true);

        List<T> GetAll<T>(bool cacheQuery = true);
        List<T> GetAll<T>(int startRowIndex, int pageSize, out int totalRecords, bool cacheQuery = true);
    }
}
