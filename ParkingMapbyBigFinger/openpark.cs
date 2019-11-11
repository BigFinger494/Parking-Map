using System;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ParkingMapbyBigFinger
{
    class openpark
    {
        public string global_id { get; set; }
        public string Coordinates { get; set; }
        public string ID { get; set; }
        public string ParkingName { get; set; }
        public string ParkingZoneNumber { get; set; }
        public string AdmArea { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string WorkingHours { get; set; }
        public string Price { get; set; }
        public string CarCapacity { get; set; }
        public string Longitude_WGS84 { get; set; }
        public string Latitude_WGS84 { get; set; }
    }
}
