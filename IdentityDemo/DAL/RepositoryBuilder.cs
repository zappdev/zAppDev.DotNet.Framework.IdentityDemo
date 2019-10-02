using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zAppDev.DotNet.Framework.Auditing;
using zAppDev.DotNet.Framework.Data;
using zAppDev.DotNet.Framework.Data.DAL;
using zAppDev.DotNet.Framework.Identity;
using zAppDev.DotNet.Framework.Workflow;

namespace IdentityDemo.DAL
{
    public class RepositoryBuilder: IRepositoryBuilder
    {
        public IUpdateRepository CreateUpdateRepository(MiniSessionService manager = null) => new Repository(manager);

        public IWorkflowRepository CreateWorkflowRepository(MiniSessionService manager = null)
        {
            throw new NotImplementedException();
        }

        public ICreateRepository CreateCreateRepository(MiniSessionService manager = null) => new Repository(manager);

        public IDeleteRepository CreateDeleteRepository(MiniSessionService manager = null) => new Repository(manager);

        public IIdentityRepository CreateIdentityRepository(MiniSessionService manager = null) => new Repository(manager);

        public IRetrieveRepository CreateRetrieveRepository(MiniSessionService manager = null) => new Repository(manager);

        public IDeleteRepository CreateDeleteRepository(IMiniSessionService manager) => new Repository(manager);

        public IIdentityRepository CreateIdentityRepository(IMiniSessionService manager) => new Repository(manager);

        public IRetrieveRepository CreateRetrieveRepository(IMiniSessionService manager) => new Repository(manager);

        public ICreateRepository CreateCreateRepository(IMiniSessionService manager) => new Repository(manager);

        public IUpdateRepository CreateUpdateRepository(IMiniSessionService manager) => new Repository(manager);

        public IWorkflowRepository CreateWorkflowRepository(IMiniSessionService manager) => throw new NotImplementedException();

        public IAuditingRepository CreateAuditingRepository(MiniSessionService manager) => throw new NotImplementedException();

        public IAuditingRepository CreateAuditingRepository(IMiniSessionService manager) => throw new NotImplementedException();
    }
}
