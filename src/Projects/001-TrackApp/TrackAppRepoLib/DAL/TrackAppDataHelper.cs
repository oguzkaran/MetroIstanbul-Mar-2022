using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackAppRepoLib.Data.Entities;
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

        public async Task<IEnumerable<SwitchDatabase>> FindSwitchDatabasesByStationNameAsync(int name)
        {
            try
            {
                return await m_switchDatabaseRepo.FindByStationNameAsync(name);
            }
            catch (Exception)
            {
                //...
                throw;
            }                   
        }



        //...
    }
}
