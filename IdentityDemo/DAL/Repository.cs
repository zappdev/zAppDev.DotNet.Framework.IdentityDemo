using NHibernate;
using NHibernate.Action;
using NHibernate.Engine;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using zAppDev.DotNet.Framework.Data;
using zAppDev.DotNet.Framework.Data.DAL;
using zAppDev.DotNet.Framework.Utilities;

namespace IdentityDemo.DAL
{
    public class Repository : IRepository
    {
        public static List<string> SystemClassNames = new List<string>()
        { "ApplicationUserAction","ApplicationUserExternalProfile","ApplicationSetting","ApplicationUser","ApplicationRole","ApplicationOperation","ApplicationPermission","ApplicationClient","ApplicationUserLogin","ApplicationUserClaim","ProfileSetting","Profile","ApplicationLanguage","DateTimeFormat","ApplicationTheme","FileData","StorageMedium","AuditEntityConfiguration","AuditPropertyConfiguration","AuditLogEntry","AuditLogEntryType","AuditLogPropertyActionType","WorkflowStatus","WorkflowExecutionResult","WorkflowContextBase","WorkflowSchedule"
        };

        private readonly ISession _currentSession;
        private readonly IMiniSessionService _sessionManager;

        public Repository(IMiniSessionService manager = null)
        {
            _sessionManager = manager ?? ServiceLocator.Current.GetInstance<IMiniSessionService>();
            // Make sure the session is open
            _sessionManager.OpenSession();
            _currentSession = _sessionManager.Session;
        }

        // For using without a MiniSessionManager
        public Repository(ISession session)
        {
            _sessionManager = new MiniSessionService(null);
            _currentSession = session;
        }

        private RepositoryAction? _prevAction;
        private void SetCurrentActionTo(RepositoryAction? action)
        {
            if (_sessionManager == null) return;
            _prevAction = _sessionManager.LastAction;
            _sessionManager.LastAction = (action ?? _prevAction ?? RepositoryAction.NONE);
        }

        private void RestoreLastAction()
        {
            SetCurrentActionTo(null);
        }

        public void DeleteApplicationClient(zAppDev.DotNet.Framework.Identity.Model.ApplicationClient applicationclient, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (applicationclient == null || applicationclient.IsTransient()) return;
            applicationclient.User = null;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.ApplicationClient>(applicationclient, isCascaded);
        }
        public void DeleteApplicationLanguage(zAppDev.DotNet.Framework.Identity.Model.ApplicationLanguage applicationlanguage, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (applicationlanguage == null || applicationlanguage.IsTransient()) return;
            if (applicationlanguage.DateTimeFormat != null)
            {
                var toDelete = applicationlanguage.DateTimeFormat;
                applicationlanguage.DateTimeFormat.ApplicationLanguage = null;
                applicationlanguage.DateTimeFormat = null;
                DeleteDateTimeFormat(toDelete, false, isCascaded, applicationlanguage);
            }
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.ApplicationLanguage>(applicationlanguage, isCascaded);
        }
        public void DeleteApplicationOperation(zAppDev.DotNet.Framework.Identity.Model.ApplicationOperation applicationoperation, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (applicationoperation == null || applicationoperation.IsTransient()) return;
            if (applicationoperation.Permissions.Count > 0)
            {
                var cs = new System.Data.ConstraintException("applicationoperation.Permissions elements are restricted and cannot be deleted");
                cs.Data["Entity"] = "ApplicationOperation";
                cs.Data["PropertyName"] = "Permissions";
                cs.Data["Multiplicity"] = "*";
                throw cs;
            }
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.ApplicationOperation>(applicationoperation, isCascaded);
        }
        public void DeleteApplicationPermission(zAppDev.DotNet.Framework.Identity.Model.ApplicationPermission applicationpermission, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (applicationpermission == null || applicationpermission.IsTransient()) return;
            if (applicationpermission.Roles.Count > 0)
            {
                var cs = new System.Data.ConstraintException("applicationpermission.Roles elements are restricted and cannot be deleted");
                cs.Data["Entity"] = "ApplicationPermission";
                cs.Data["PropertyName"] = "Roles";
                cs.Data["Multiplicity"] = "*";
                throw cs;
            }
            if (applicationpermission.Users.Count > 0)
            {
                var cs = new System.Data.ConstraintException("applicationpermission.Users elements are restricted and cannot be deleted");
                cs.Data["Entity"] = "ApplicationPermission";
                cs.Data["PropertyName"] = "Users";
                cs.Data["Multiplicity"] = "*";
                throw cs;
            }
            foreach (var toDissasociate in applicationpermission.Operations)
            {
                applicationpermission.RemoveOperations(toDissasociate);
            }
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.ApplicationPermission>(applicationpermission, isCascaded);
        }
        public void DeleteApplicationRole(zAppDev.DotNet.Framework.Identity.Model.ApplicationRole applicationrole, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (applicationrole == null || applicationrole.IsTransient()) return;
            if (applicationrole.Users.Count > 0)
            {
                var cs = new System.Data.ConstraintException("applicationrole.Users elements are restricted and cannot be deleted");
                cs.Data["Entity"] = "ApplicationRole";
                cs.Data["PropertyName"] = "Users";
                cs.Data["Multiplicity"] = "*";
                throw cs;
            }
            foreach (var toDissasociate in applicationrole.Permissions)
            {
                applicationrole.RemovePermissions(toDissasociate);
            }
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.ApplicationRole>(applicationrole, isCascaded);
        }
        public void DeleteApplicationSetting(zAppDev.DotNet.Framework.Identity.Model.ApplicationSetting applicationsetting, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (applicationsetting == null || applicationsetting.IsTransient()) return;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.ApplicationSetting>(applicationsetting, isCascaded);
        }
        public void DeleteApplicationTheme(zAppDev.DotNet.Framework.Identity.Model.ApplicationTheme applicationtheme, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (applicationtheme == null || applicationtheme.IsTransient()) return;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.ApplicationTheme>(applicationtheme, isCascaded);
        }
        public void DeleteApplicationUser(zAppDev.DotNet.Framework.Identity.Model.ApplicationUser applicationuser, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (applicationuser == null || applicationuser.IsTransient()) return;
            foreach (var toDelete in applicationuser.Clients)
            {
                applicationuser.RemoveClients(toDelete);
                DeleteApplicationClient(toDelete, false, isCascaded);
            }
            foreach (var toDelete in applicationuser.Logins)
            {
                applicationuser.RemoveLogins(toDelete);
                DeleteApplicationUserLogin(toDelete, false, isCascaded);
            }
            foreach (var toDelete in applicationuser.Claims)
            {
                applicationuser.RemoveClaims(toDelete);
                DeleteApplicationUserClaim(toDelete, false, isCascaded);
            }
            foreach (var toDissasociate in applicationuser.Permissions)
            {
                applicationuser.RemovePermissions(toDissasociate);
            }
            if (applicationuser.Profile != null)
            {
                var toDelete = applicationuser.Profile;
                applicationuser.Profile = null;
                DeleteProfile(toDelete, false, isCascaded, applicationuser);
            }
            foreach (var toDissasociate in applicationuser.Roles)
            {
                applicationuser.RemoveRoles(toDissasociate);
            }
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.ApplicationUser>(applicationuser, isCascaded);
        }
        public void DeleteApplicationUserAction(zAppDev.DotNet.Framework.Identity.Model.ApplicationUserAction applicationuseraction, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (applicationuseraction == null || applicationuseraction.IsTransient()) return;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.ApplicationUserAction>(applicationuseraction, isCascaded);
        }
        public void DeleteApplicationUserClaim(zAppDev.DotNet.Framework.Identity.Model.ApplicationUserClaim applicationuserclaim, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (applicationuserclaim == null || applicationuserclaim.IsTransient()) return;
            applicationuserclaim.User = null;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.ApplicationUserClaim>(applicationuserclaim, isCascaded);
        }
        public void DeleteApplicationUserExternalProfile(zAppDev.DotNet.Framework.Identity.Model.ApplicationUserExternalProfile applicationuserexternalprofile, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (applicationuserexternalprofile == null || applicationuserexternalprofile.IsTransient()) return;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.ApplicationUserExternalProfile>(applicationuserexternalprofile, isCascaded);
        }
        public void DeleteApplicationUserLogin(zAppDev.DotNet.Framework.Identity.Model.ApplicationUserLogin applicationuserlogin, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (applicationuserlogin == null || applicationuserlogin.IsTransient()) return;
            applicationuserlogin.User = null;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.ApplicationUserLogin>(applicationuserlogin, isCascaded);
        }
        public void DeleteAuditEntityConfiguration(zAppDev.DotNet.Framework.Auditing.Model.AuditEntityConfiguration auditentityconfiguration, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (auditentityconfiguration == null || auditentityconfiguration.IsTransient()) return;
            foreach (var toDelete in auditentityconfiguration.Properties)
            {
                auditentityconfiguration.RemoveProperties(toDelete);
                DeleteAuditPropertyConfiguration(toDelete, false, isCascaded);
            }
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Auditing.Model.AuditEntityConfiguration>(auditentityconfiguration, isCascaded);
        }
        public void DeleteAuditLogEntry(zAppDev.DotNet.Framework.Auditing.Model.AuditLogEntry auditlogentry, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (auditlogentry == null || auditlogentry.IsTransient()) return;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Auditing.Model.AuditLogEntry>(auditlogentry, isCascaded);
        }
        public void DeleteAuditLogEntryType(zAppDev.DotNet.Framework.Auditing.Model.AuditLogEntryType auditlogentrytype, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (auditlogentrytype == null || auditlogentrytype.IsTransient()) return;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Auditing.Model.AuditLogEntryType>(auditlogentrytype, isCascaded);
        }
        public void DeleteAuditLogPropertyActionType(zAppDev.DotNet.Framework.Auditing.Model.AuditLogPropertyActionType auditlogpropertyactiontype, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (auditlogpropertyactiontype == null || auditlogpropertyactiontype.IsTransient()) return;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Auditing.Model.AuditLogPropertyActionType>(auditlogpropertyactiontype, isCascaded);
        }
        public void DeleteAuditPropertyConfiguration(zAppDev.DotNet.Framework.Auditing.Model.AuditPropertyConfiguration auditpropertyconfiguration, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (auditpropertyconfiguration == null || auditpropertyconfiguration.IsTransient()) return;
            auditpropertyconfiguration.Entity = null;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Auditing.Model.AuditPropertyConfiguration>(auditpropertyconfiguration, isCascaded);
        }
        public void DeleteDateTimeFormat(zAppDev.DotNet.Framework.Identity.Model.DateTimeFormat datetimeformat, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (datetimeformat == null || datetimeformat.IsTransient()) return;
            datetimeformat.ApplicationLanguage = null;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.DateTimeFormat>(datetimeformat, isCascaded);
        }
       
        public void DeleteProfile(zAppDev.DotNet.Framework.Identity.Model.Profile profile, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (profile == null || profile.IsTransient()) return;
            foreach (var toDelete in profile.Settings)
            {
                profile.RemoveSettings(toDelete);
                DeleteProfileSetting(toDelete, false, isCascaded);
            }
            var _ApplicationUserApplicationUserProfilecount = this.Get<zAppDev.DotNet.Framework.Identity.Model.ApplicationUser>(ap => ap.Profile == profile).Count;
            if (
                (calledBy != null)
                &&
                (
                    (calledBy.GetType() == typeof(zAppDev.DotNet.Framework.Identity.Model.ApplicationUser) || calledBy.GetType().FullName == "ApplicationUserProxy")
                )
            ) _ApplicationUserApplicationUserProfilecount--;
            if (_ApplicationUserApplicationUserProfilecount > 0)
            {
                var cs = new System.Data.ConstraintException("At least one ApplicationUser exists so Profile cannot be deleted");
                cs.Data["Entity"] = "ApplicationUser";
                cs.Data["PropertyName"] = "Profile";
                cs.Data["Multiplicity"] = "0..1";
                throw cs;
            }
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.Profile>(profile, isCascaded);
        }
        public void DeleteProfileSetting(zAppDev.DotNet.Framework.Identity.Model.ProfileSetting profilesetting, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (profilesetting == null || profilesetting.IsTransient()) return;
            profilesetting.ParentProfile = null;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Identity.Model.ProfileSetting>(profilesetting, isCascaded);
        }
        public void DeleteWorkflowContextBase(zAppDev.DotNet.Framework.Workflow.WorkflowContextBase workflowcontextbase, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (workflowcontextbase == null || workflowcontextbase.IsTransient()) return;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Workflow.WorkflowContextBase>(workflowcontextbase, isCascaded);
        }
        public void DeleteWorkflowSchedule(zAppDev.DotNet.Framework.Workflow.WorkflowSchedule workflowschedule, bool doNotCallDeleteForThis = false, bool isCascaded = false, object calledBy = null)
        {
            if (workflowschedule == null || workflowschedule.IsTransient()) return;
            if (!doNotCallDeleteForThis) Delete<zAppDev.DotNet.Framework.Workflow.WorkflowSchedule>(workflowschedule, isCascaded);
        }

        public T GetById<T>(object id, bool throwIfNotFound = true) where T : class
        {
            SetCurrentActionTo(RepositoryAction.GET);
            var obj = _currentSession.Get<T>(id);

            if (throwIfNotFound && obj == null)
            {
                throw new ApplicationException($"No {typeof(T).Name} was found with key: {id}.");
            }

            if (!CanReadInstance(obj))
            {
                if (throwIfNotFound)
                {
                    throw new ApplicationException($"No Read Access for {typeof(T).Name} instance with key: {id}.");
                }
                else
                {
                    obj = null;
                }
            }

            RestoreLastAction();
            return obj;
        }

        public void SaveWithoutTransaction<T>(T entity) where T : class
        {
            try
            {
                _currentSession.SaveOrUpdate(entity);
            }
            catch (zAppDev.DotNet.Framework.Exceptions.BusinessException e)
            {
                throw;
            }
            catch (NHibernate.Exceptions.GenericADOException e)
            {
                throw;
            }
            catch (NonUniqueObjectException e)
            {
                throw;
            }
            catch (Exception e)
            {
                _currentSession.Merge(entity);
            }
        }

        public void Save<T>(T entity) where T : class
        {
            SetCurrentActionTo(RepositoryAction.SAVE);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "No " + typeof(T).Name + " was specified.");
            }

            SaveWithoutTransaction(entity);
            RestoreLastAction();
        }

        public void Insert<T>(T entity) where T : class
        {
            SetCurrentActionTo(RepositoryAction.INSERT);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "No " + typeof(T).Name + " was specified.");
            }

            try
            {
                _currentSession.Save(entity);
            }
            catch (zAppDev.DotNet.Framework.Exceptions.BusinessException e)
            {
                throw;
            }
            catch (NHibernate.Exceptions.GenericADOException e)
            {
                throw;
            }
            catch (NonUniqueObjectException e)
            {
                throw;
            }
            catch (Exception e)
            {
                _currentSession.Merge(entity);
            }

            RestoreLastAction();
        }

        public void Update<T>(T entity) where T : class
        {
            SetCurrentActionTo(RepositoryAction.UPDATE);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "No " + typeof(T).Name + " was specified.");
            }
            _currentSession.Update(entity);
            RestoreLastAction();
        }

        public void Delete<T>(T entity, bool isCascaded = false) where T : class
        {
            SetCurrentActionTo(RepositoryAction.DELETE);

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "No " + typeof(T).Name + " was specified.");
            }

            // var exists = GetMainQuery<T>().WithOptions(options => options.SetCacheable(true)).Any(a => a == entity);
            // if (!exists)
            // {
            //    RestoreLastAction();
            //    return;
            // }

            try
            {
                _currentSession.Delete(entity);
            }
            catch (Exception e)
            {
                throw;
            }

            RestoreLastAction();
        }

        public T Merge<T>(T entity) where T : class
        {
            return new ObjectGraphWalker().AssociateGraphWithSession(entity, MiniSessionManager.Instance);
        }

        public List<double> GeAggregates<T>(Expression<Func<T, bool>> predicate,
                                            Dictionary<Expression<Func<T, double>>, string> requestedAggregates)
        {
            var mainQuery = GetMainQuery<T>();
            List<double> aggregateValues = new List<double>();
            if (predicate == null)
            {
                predicate = a => true;
            }
            foreach (var entry in requestedAggregates)
            {
                try
                {
                    switch (entry.Value)
                    {
                        case "SUM":
                            aggregateValues.Add(mainQuery.Where(predicate).Sum(entry.Key));
                            break;
                        case "AVERAGE":
                            aggregateValues.Add(mainQuery.Where(predicate).Average(entry.Key));
                            break;
                        case "COUNT":
                            aggregateValues.Add(mainQuery.Where(predicate).Count());
                            break;
                    }
                }
                catch
                {
                    aggregateValues.Add(0);
                }
            }
            return aggregateValues;
        }

        private static IFutureValue<TResult> ToFutureValue<TSource, TResult>(IQueryable source, Expression<Func<IQueryable<TSource>, TResult>> selector)
        where TResult : struct
        {
            var provider = (DefaultQueryProvider)source.Provider;
            var method = ((MethodCallExpression)selector.Body).Method;
            var expression = Expression.Call(null, method, source.Expression);
            return provider.ExecuteFutureValue<TResult>(expression);
        }

        public List<T> Get<T>(Expression<Func<T, bool>> predicate,
                              int startRowIndex,
                              int pageSize,
                              Dictionary<Expression<Func<T, object>>, bool> orderBy,
                              out int totalRecords, bool cacheQuery = true)
        {
            if (orderBy == null)
            {
                orderBy = new Dictionary<Expression<Func<T, object>>, bool>();
            }
            if (predicate?.Body.NodeType == ExpressionType.Constant
                    && (((ConstantExpression)predicate.Body)).Value != null)
            {
                var val = (bool)(((ConstantExpression)predicate.Body)).Value;
                if (val)
                {
                    predicate = null;
                }
                else
                {
                    totalRecords = 0;
                    return new List<T>();
                }
            }
            var objects = GetMainQuery<T>();
            if (cacheQuery)
            {
                objects = objects.WithOptions(options => options.SetCacheable(true));
            }
            var futureCount = predicate == null
                              ? ToFutureValue<T, int>(objects, x => x.Count())
                              : ToFutureValue<T, int>(objects.Where(predicate), x => x.Count());
            var ordered = (IOrderedQueryable<T>)((predicate == null)
                                                 ? objects
                                                 : objects.Where(predicate));
            if (orderBy.Keys.Count > 0)
            {
                var first = orderBy.First();
                ordered = first.Value
                          ? ordered.OrderBy(first.Key)
                          : ordered.OrderByDescending(first.Key);
                foreach (var pair in orderBy.Skip(1))
                {
                    ordered = pair.Value
                              ? ordered.ThenBy(pair.Key)
                              : ordered.ThenByDescending(pair.Key);
                }
            }
            var paged = ordered.Skip(startRowIndex).Take(pageSize).ToFuture().ToList();
            totalRecords = futureCount.Value;
            return paged;
        }

        public IQueryable<T> GetAsQueryable<T>(Expression<Func<T, bool>> predicate = null, bool cacheQuery = true)
        {
            SetCurrentActionTo(RepositoryAction.GET);
            if (predicate?.Body.NodeType == ExpressionType.Constant
                    && (((ConstantExpression)predicate.Body)).Value != null)
            {
                var val = (bool)(((ConstantExpression)predicate.Body)).Value;
                if (val)
                {
                    predicate = null;
                }
                else
                {
                    // If the predicate returns false then return empty resultset
                    RestoreLastAction();
                    return Enumerable.Empty<T>().AsQueryable();
                }
            }
            var query = GetMainQuery<T>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (cacheQuery)
            {
                query = query.WithOptions(options => options.SetCacheable(true));
            }
            RestoreLastAction();
            return query;
        }

        public List<T> Get<T>(Expression<Func<T, bool>> predicate, bool cacheQuery = true)
        {
            var list = GetAsQueryable(predicate, cacheQuery).ToList();
            list = AppendPendingInsertions(predicate, list);
            list = RemovePendingDeletions(predicate, list);
            return list;
        }

        public int? GetCount<T>(Expression<Func<T, bool>> predicate = null, bool cacheQuery = true)
        {
            return GetAsQueryable(predicate, cacheQuery).Count()
                   + GetItemsToAppend(predicate).Count()
                   - GetItemsToRemove(predicate).Count();
        }

        public List<T> GetAll<T>(bool cacheQuery = true)
        {
            return Get<T>(null, cacheQuery);
        }

        public List<T> GetAll<T>(int startRowIndex, int pageSize, out int totalRecords, bool cacheQuery = true)
        {
            SetCurrentActionTo(RepositoryAction.GET);
            var items = GetMainQuery<T>();
            if (cacheQuery)
            {
                items = items.WithOptions(options => options.SetCacheable(true));
            }
            var futureCount = ToFutureValue<T, int>(items, x => x.Count());
            var paged = items.Skip(startRowIndex).Take(pageSize).ToFuture().ToList();
            totalRecords = futureCount.Value;
            RestoreLastAction();
            return paged;
        }

        public IQueryable<T> GetMainQuery<T>()
        {
            return ApplyReadFilter(_currentSession.Query<T>());
        }

        private static readonly FieldInfo _insertionsFieldInfo = typeof(ActionQueue).GetField("insertions", BindingFlags.NonPublic | BindingFlags.Instance);
        private List<T> AppendPendingInsertions<T>(Expression<Func<T, bool>> predicate, List<T> list = null)
        {
            if (list == null)
            {
                list = new List<T>();
            }
            if (!_sessionManager.WillFlush || !(((NHibernate.Impl.SessionImpl)_currentSession).ActionQueue).AreInsertionsOrDeletionsQueued)
                return list;
            var itemsToAppend = GetItemsToAppend(predicate);
            var items = itemsToAppend.ToList();
            if (!items.Any()) return list;
            if (!items.Any())
            {
                list = items;
            }
            if (list.Count > items.Count)
            {
                list.AddRange(items);
            }
            else
            {
                items.AddRange(list);
                list = items;
            }
            return list;
        }

        private static readonly FieldInfo _deletionsFieldInfo = typeof(ActionQueue).GetField("deletions", BindingFlags.NonPublic | BindingFlags.Instance);
        private List<T> RemovePendingDeletions<T>(Expression<Func<T, bool>> predicate, List<T> list = null)
        {
            if (list == null)
            {
                list = new List<T>();
            }
            if (list.Count == 0)
            {
                return list;
            }
            if (!_sessionManager.WillFlush || !(((NHibernate.Impl.SessionImpl)_currentSession).ActionQueue).AreInsertionsOrDeletionsQueued)
                return list;
            var itemsToRemove = GetItemsToRemove(predicate);
            foreach (var item in itemsToRemove)
            {
                if (list.Contains(item))
                {
                    list.Remove(item);
                }
            }
            return list;
        }

        private IQueryable<T> GetItemsToAppend<T>(Expression<Func<T, bool>> predicate)
        {
            if (_insertionsFieldInfo == null)
            {
                throw new ApplicationException("Could not find `insertions` field in NH Session's Action Queue!");
            }
            var insertions = (List<AbstractEntityInsertAction>)_insertionsFieldInfo.GetValue(((NHibernate.Impl.SessionImpl)_currentSession).ActionQueue);
            var itemsToAppend
                = insertions
                  .Cast<EntityInsertAction>()
                  .Where(a => a.EntityName == typeof(T).FullName)
                  .Select(a => (T)a.Instance)
                  .AsQueryable();
            if (predicate != null)
            {
                itemsToAppend = itemsToAppend.Where(predicate);
            }
            return itemsToAppend;
        }

        private IQueryable<T> GetItemsToRemove<T>(Expression<Func<T, bool>> predicate)
        {
            if (_deletionsFieldInfo == null)
            {
                throw new ApplicationException("Could not find `deletions` field in NH Session's Action Queue!");
            }
            var deletions = (List<EntityDeleteAction>)_deletionsFieldInfo.GetValue(((NHibernate.Impl.SessionImpl)_currentSession).ActionQueue);
            var itemsToRemove
                = deletions
                  .Where(a => a.EntityName == typeof(T).FullName)
                  .Select(a => (T)a.Instance)
                  .AsQueryable();
            if (predicate != null)
            {
                itemsToRemove = itemsToRemove.Where(predicate);
            }
            return itemsToRemove;
        }

        public void Evict(object obj)
        {
            _currentSession.Evict(obj);
        }

        private IQueryable<T> ApplyReadFilter<T>(IQueryable<T> query)
        {
            return query;
        }


        private bool CanReadInstance<T>(T instance)
        {
            if (instance == null) return true;
            var tmpList = new List<T> { instance };
            tmpList = ApplyReadFilter(tmpList.AsQueryable()).ToList();
            return tmpList.Any();
        }
    }
}
