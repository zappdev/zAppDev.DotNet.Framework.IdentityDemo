using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zAppDev.DotNet.Framework.Identity.Model;
using IdentityDemo.DAL;
using System.Reflection;
using NHibernate;
using zAppDev.DotNet.Framework.Utilities;

namespace IdentityDemo
{
    public class DatabaseSeeder
    {
        private ConcurrentDictionary<string, ApplicationPermission> _applicationPermissionDictionary = null;

        private ISession _session;
        public DatabaseSeeder(ISessionFactory sessionFactory)
        {
            _session = sessionFactory.OpenSession();
        }

        private ApplicationPermission CreateOrUpdatePermission(string name, string description, bool isCustom, Repository repo)
        {
            if (_applicationPermissionDictionary == null)
            {
                _applicationPermissionDictionary = new ConcurrentDictionary<string, ApplicationPermission>();
                var applicationPermissions = repo.Get<ApplicationPermission>(a => a.Name != null);
                foreach (var item in applicationPermissions)
                {
                    if (!_applicationPermissionDictionary.ContainsKey(item.Name))
                    {
                        _applicationPermissionDictionary.TryAdd(item.Name, item);
                    }
                }
            }
            bool update = false;
            ApplicationPermission p = null;
            if (_applicationPermissionDictionary.ContainsKey(name))
            {
                p = _applicationPermissionDictionary[name];
                if (p.IsCustom != isCustom)
                {
                    p.IsCustom = isCustom;
                    update = true;
                }
                if (p.Description != description)
                {
                    p.Description = description;
                    update = true;
                }
            }
            else
            {
                update = true;
                p = new ApplicationPermission();
                p.Name = name;
                p.IsCustom = isCustom;
                p.Description = description;
            }
            if (update)
            {
                repo.Save(p);
            }
            return p;
        }

        private ConcurrentDictionary<string, ApplicationOperation> _applicationOperationDictionary = null;

        private string GetApplicationOperationKey(string name, string parent, string type)
        {
            return name + '|' + parent + '|' + type;
        }

        private ApplicationOperation CreateOrUpdateOperation(string name, string parent, string type,
                bool isAvailableToAllAuthorizedUsers,
                bool isAvailableToAnonymous, IEnumerable<ApplicationPermission> perms, Repository repo)
        {
            if (_applicationOperationDictionary == null)
            {
                _applicationOperationDictionary = new ConcurrentDictionary<string, ApplicationOperation>();
                var applicationOperations = repo.Get<ApplicationOperation>(a => a.Name != null);
                foreach (ApplicationOperation item in applicationOperations)
                {
                    string key = GetApplicationOperationKey(item.Name, item.ParentControllerName, item.Type);
                    if (!_applicationOperationDictionary.ContainsKey(key))
                    {
                        _applicationOperationDictionary.TryAdd(key, item);
                    }
                }
            }
            ApplicationOperation op = null;
            string currentKey = GetApplicationOperationKey(name, parent, type);
            bool update = false;
            if (_applicationOperationDictionary.ContainsKey(currentKey))
            {
                op = _applicationOperationDictionary[currentKey];
                if (op.IsAvailableToAllAuthorizedUsers != isAvailableToAllAuthorizedUsers)
                {
                    op.IsAvailableToAllAuthorizedUsers = isAvailableToAllAuthorizedUsers;
                    update = true;
                }
                if (op.IsAvailableToAnonymous != isAvailableToAnonymous)
                {
                    op.IsAvailableToAnonymous = isAvailableToAnonymous;
                    update = true;
                }
                List<ApplicationPermission> applicationPermissions = op.Permissions.Where(p => !p.IsCustom).ToList();
                HandleApplicationPermissions<ApplicationOperation>(op, applicationPermissions, perms, ref update);
            }
            else
            {
                update = true;
                op = new ApplicationOperation();
                op.Name = name;
                op.Type = type;
                op.IsAvailableToAllAuthorizedUsers = isAvailableToAllAuthorizedUsers;
                op.IsAvailableToAnonymous = isAvailableToAnonymous;
                op.ParentControllerName = parent;
                if (perms != null)
                {
                    foreach (var p in perms)
                    {
                        op.AddPermissions(p);
                    }
                }
            }
            if (update)
            {
                repo.Save(op);
            }
            return op;
        }



        private ConcurrentDictionary<string, ApplicationRole> _applicationRoleDictionary = null;
        private ApplicationRole CreateOrUpdateRole(string name, string description, IEnumerable<ApplicationPermission> perms, Repository repo)
        {
            if (_applicationRoleDictionary == null)
            {
                _applicationRoleDictionary = new ConcurrentDictionary<string, ApplicationRole>();
                var applicationRoles = repo.Get<ApplicationRole>(a => a.Name != null);
                foreach (ApplicationRole item in applicationRoles)
                {
                    if (!_applicationRoleDictionary.ContainsKey(item.Name))
                    {
                        _applicationRoleDictionary.TryAdd(item.Name, item);
                    }
                }
            }
            ApplicationRole role = null;
            bool update = false;
            if (_applicationRoleDictionary.ContainsKey(name))
            {
                role = _applicationRoleDictionary[name];
                if (role.Description != description)
                {
                    role.Description = description;
                    update = true;
                }
                if (role.IsCustom != false)
                {
                    role.IsCustom = false;
                    update = true;
                }
                List<ApplicationPermission> applicationPermissions = role.Permissions.Where(p => !p.IsCustom).ToList();
                HandleApplicationPermissions<ApplicationRole>(role, applicationPermissions, perms, ref update);
            }
            else
            {
                update = true;
                role = new ApplicationRole();
                role.Name = name;
                role.Description = description;
                role.IsCustom = false;
                if (perms != null)
                {
                    foreach (var p in perms)
                    {
                        role.AddPermissions(p);
                    }
                }
            }
            if (update)
            {
                repo.Save(role);
            }
            return role;
        }


        private void RemoveApplicationPermissions<T>(T objT, List<ApplicationPermission> applicationPermissions)
        {
            MethodInfo removePermissions = typeof(T).GetMethod("RemovePermissions");
            foreach (var p in applicationPermissions)
            {
                removePermissions.Invoke(objT, new object[] { p });
            }
        }



        private void AddApplicationPermissions<T>(T objT, IEnumerable<ApplicationPermission> perms)
        {
            MethodInfo addPermissions = typeof(T).GetMethod("AddPermissions", new[] { typeof(ApplicationPermission) });
            foreach (var p in perms)
            {
                addPermissions.Invoke(objT, new object[] { p });
            }
        }


        private void HandleApplicationPermissions<T>(T objT, List<ApplicationPermission> applicationPermissions,
                IEnumerable<ApplicationPermission> perms, ref bool update)
        {
            if (perms == null)
            {
                if (applicationPermissions != null && applicationPermissions.Count > 0)
                {
                    update = true;
                    RemoveApplicationPermissions<T>(objT, applicationPermissions);
                }
            }
            else if (perms.Any())
            {
                if (applicationPermissions == null || applicationPermissions.Count == 0)
                {
                    update = true;
                    AddApplicationPermissions<T>(objT, perms);
                }
                else
                {
                    if (perms.Count() != applicationPermissions.Count)
                    {
                        update = true;
                        RemoveApplicationPermissions<T>(objT, applicationPermissions);
                        AddApplicationPermissions<T>(objT, perms);
                    }
                    else
                    {
                        bool diffFound = false;
                        foreach (var p in perms)
                        {
                            //if (!applicationPermissions.Contains(p))
                            var checkApplicationPermission = applicationPermissions.FirstOrDefault(a => a.Name == p.Name);
                            if (checkApplicationPermission == null)
                            {
                                diffFound = true;
                                break;
                            }
                        }
                        if (diffFound)
                        {
                            update = true;
                            RemoveApplicationPermissions<T>(objT, applicationPermissions);
                            AddApplicationPermissions<T>(objT, perms);
                        }
                    }
                }
            }
        }



        public void UpdateAuthorizationTables()
        {
            bool seedSecurityTables = true;
            if (!seedSecurityTables) return;
            var repo = new Repository(_session);
            
            var allRoles = new List<ApplicationRole>();
            var manageUsersPermission = CreateOrUpdatePermission("ManageUsers", "Can Manage Users", false, repo);
            var manageRolesPermission = CreateOrUpdatePermission("ManageRoles", "Can Manage Roles", false, repo);
            var managePermissionsPermission = CreateOrUpdatePermission("ManagePermissions", "Can Manage Permissions", false, repo);
            var manageOperationsPermission = CreateOrUpdatePermission("ManageOperations", "Can Manage Operations", false, repo);
            var manageSettingsPermission = CreateOrUpdatePermission("ManageSettings", "Can Manage Settings", false, repo);
            var manageApplicationDataPermission = CreateOrUpdatePermission("ManageApplicationData", "Can Manage Application Data", false, repo);
            var customerSelectionAndAdminAddPermission = CreateOrUpdatePermission("CustomerSelectionAndAdmin_Add", "CustomerSelectionAndAdmin_Add", false, repo);
               
            allRoles = new List<ApplicationRole>
            {
                CreateOrUpdateRole("Administrator", "Administrator",
                new [] { manageUsersPermission, manageRolesPermission, managePermissionsPermission, manageOperationsPermission, manageSettingsPermission, manageApplicationDataPermission }, repo),
            };
            _session.Close();
            _session.Dispose();
        }
    }
}
