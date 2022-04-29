using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrackAppRepoLib.Data.Contexts;
using TrackAppRepoLib.Data.Entities;

namespace TrackAppRepoLib.Data.Repositories
{
    public class SwitchDatabaseRepo : ISwitchDatabaseRepo 
    {
        private readonly AtsServerDbContext m_atsServerDbContext;

        public SwitchDatabaseRepo(AtsServerDbContext atsServerDbContext)
        {
            m_atsServerDbContext = atsServerDbContext;
        }

        public Task<IEnumerable<SwitchDatabase>> FindByStationNameAsync(int name)
        {
            var task = new Task<IEnumerable<SwitchDatabase>>(() => m_atsServerDbContext.SwitchDatabases.Where(sd => sd.StationName == name).ToList());

            task.Start();

            return task;
        }
        
        ////////////////////////////////////

        public IEnumerable<SwitchDatabase> All => throw new NotImplementedException();

        public long Count()
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync()
        {
            throw new NotImplementedException();
        }

        public void Delete(SwitchDatabase t)
        {
            throw new NotImplementedException();
        }

        public void DeleteAsync(SwitchDatabase t)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool ExistsById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SwitchDatabase>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SwitchDatabase> FindByFilter(Expression<Func<SwitchDatabase, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SwitchDatabase>> FindByFilterAsync(Expression<Func<SwitchDatabase, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public SwitchDatabase FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<SwitchDatabase> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SwitchDatabase> FindByIds(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SwitchDatabase>> FindByIdsAsync(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public SwitchDatabase Save(SwitchDatabase t)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SwitchDatabase> Save(IEnumerable<SwitchDatabase> entities)
        {
            throw new NotImplementedException();
        }

        public Task<SwitchDatabase> SaveAsync(SwitchDatabase t)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SwitchDatabase>> SaveAsync(IEnumerable<SwitchDatabase> entities)
        {
            throw new NotImplementedException();
        }

        //...
    }
}
