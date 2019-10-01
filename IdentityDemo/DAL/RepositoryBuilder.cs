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
        private readonly ISessionFactory _factory;

        public RepositoryBuilder(ISessionFactory factory)
        {
            _factory = factory;
        }

        public IAuditingRepository CreateAuditingRepository(IMiniSessionService sessionManager = null)
        {
            throw new NotImplementedException();
        }

        public ICreateRepository CreateCreateRepository(MiniSessionService manager = null)
        {
            return CreateCreateRepository((IMiniSessionService)manager);
        }

        public ICreateRepository CreateCreateRepository(IMiniSessionService sessionManager)
        {
            return new Repository(sessionManager);
        }

        public IDeleteRepository CreateDeleteRepository(MiniSessionService manager = null)
        {
            throw new NotImplementedException();
        }

        public IDeleteRepository CreateDeleteRepository(IMiniSessionService manager)
        {
            throw new NotImplementedException();
        }

        public IIdentityRepository CreateIdentityRepository(MiniSessionService sessionManager = null)
        {
            throw new NotImplementedException();
        }

        public IIdentityRepository CreateIdentityRepository(IMiniSessionService sessionManager)
        {
            throw new NotImplementedException();
        }

        public IRetrieveRepository CreateRetrieveRepository(MiniSessionService manager = null)
        {
            return CreateCreateRepository((IMiniSessionService)manager);
        }

        public IRetrieveRepository CreateRetrieveRepository(IMiniSessionService manager)
        {
            return CreateCreateRepository(manager);
        }

        public IUpdateRepository CreateUpdateRepository(MiniSessionService manager = null)
        {
            throw new NotImplementedException();
        }

        public IUpdateRepository CreateUpdateRepository(IMiniSessionService manager)
        {
            throw new NotImplementedException();
        }

        public IWorkflowRepository CreateWorkflowRepository(MiniSessionService manager = null)
        {
            throw new NotImplementedException();
        }

        public IWorkflowRepository CreateWorkflowRepository(IMiniSessionService manager)
        {
            throw new NotImplementedException();
        }

        IIdentityRepository IRepositoryBuilder.CreateIdentityRepository(MiniSessionService sessionManager)
        {
            throw new NotImplementedException();
        }

        IIdentityRepository IRepositoryBuilder.CreateIdentityRepository(IMiniSessionService sessionManager)
        {
            throw new NotImplementedException();
        }

        IWorkflowRepository IRepositoryBuilder.CreateWorkflowRepository(MiniSessionService manager)
        {
            throw new NotImplementedException();
        }

        IWorkflowRepository IRepositoryBuilder.CreateWorkflowRepository(IMiniSessionService manager)
        {
            throw new NotImplementedException();
        }
    }
}
