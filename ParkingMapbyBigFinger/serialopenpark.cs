using System;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace ParkingMapbyBigFinger
{

    public class GeoData_OpenPark
    {
        public string type { get; set; }
        public List<List<List<double>>> coordinates { get; set; }
    }

    public class Cells_OpenPark
    {
        public int global_id { get; set; }
        public string ParkingName { get; set; }
        public string ParkingZoneNumber { get; set; }
        public string AdmArea { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string WorkingHours { get; set; }
        public int Price { get; set; }
        public int CarCapacity { get; set; }
        public GeoData_OpenPark geoData { get; set; }
    }

    public class serialopenpark
    {
        public int global_id { get; set; }
        public int Number { get; set; }
        public Cells_OpenPark Cells { get; set; }
    }
}
