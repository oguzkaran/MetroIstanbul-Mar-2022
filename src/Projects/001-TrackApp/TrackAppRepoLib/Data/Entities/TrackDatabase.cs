using System;
using System.Collections.Generic;

namespace TrackAppRepoLib.Data.Entities
{
    public partial class TrackDatabase
    {
        public int No { get; set; }
        public int? StationStartPosition { get; set; }
        public int? StationEndPosition { get; set; }
        public string? StationName { get; set; }
        public int? TrackId { get; set; }
        public int? LineId { get; set; }
        public int? TrackType { get; set; }
        public int? TrackStartPosition { get; set; }
        public int? TrackEndPosition { get; set; }
        public int? TrackLength { get; set; }
        public int? TrackSpeedLimit { get; set; }
        public int? StoppingPointPosition1 { get; set; }
        public int? StoppingPointType1 { get; set; }
        public int? StoppingPointPosition2 { get; set; }
        public int? StoppingPointType2 { get; set; }
        public int? TrackConnectionEntry1 { get; set; }
        public int? TrackConnectionEntry2 { get; set; }
        public int? TrackConnectionExit1 { get; set; }
        public int? TrackConnectionExit2 { get; set; }
        public int? X1Point { get; set; }
        public int? X2Point { get; set; }
        public int? Y1Point { get; set; }
        public int? Y2Point { get; set; }
        public int? SignalId1 { get; set; }
        public int? SignalId2 { get; set; }
        public int? PsdEsbId1 { get; set; }
        public int? PsdEsbId2 { get; set; }
        public int? WaysideId { get; set; }
    }
}
