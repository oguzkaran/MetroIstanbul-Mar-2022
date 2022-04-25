using CSD.Util.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackAppRepoLib.Data.Entities;

namespace TrackAppRepoLib.Data.Repositories
{
    public interface ISwitchDatabaseRepo : ICrudRepository<SwitchDatabase, int>
    {
        Task<IEnumerable<SwitchDatabase>> FindByStationNameAsync(string name);
        //...
    }
}
