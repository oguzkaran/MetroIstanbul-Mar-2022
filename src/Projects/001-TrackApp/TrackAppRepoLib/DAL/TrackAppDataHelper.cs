using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackAppRepoLib.Data.Repositories;

namespace TrackAppRepoLib.DAL
{
    public class TrackAppDataHelper
    {
        private readonly ISwitchDatabaseRepo m_switchDatabaseRepo;

        public TrackAppDataHelper(ISwitchDatabaseRepo switchDatabaseRepo)
        {
            m_switchDatabaseRepo = switchDatabaseRepo;
        }

        public Task<IEnumerable<SwitchDatabase>> FindSwitchDatabasesByStationName(string name)
        {
            return m_switchDatabaseRepo.FindByStationNameAsync(name);
        }



        //...
    }
}
