using System;
using System.Collections.Generic;

namespace TrackAppRepoLib.Data.Entities
{
    public partial class SwitchDatabase
    {
        public int Id { get; set; }
        public int? SwitchId { get; set; }
        public int? EntryTrack { get; set; }
        public int? StationName { get; set; }
        public int? LeftTrack { get; set; }
        public int? RightTrack { get; set; }
        public int? NormalPosition { get; set; }
    }
}
