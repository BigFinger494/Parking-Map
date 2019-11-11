using System;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace ParkingMapbyBigFinger
{

    public class GeoData
    {
        public string type { get; set; }
        public List<List<List<double>>> coordinates { get; set; }
    }

    public class Cells
    {
        public int global_id { get; set; }
        public string PlaceID { get; set; }
        public string AdmArea { get; set; }
        public string District { get; set; }
        public string Name { get; set; }
        public GeoData geoData { get; set; }
    }

    public class RootObject
    {
        public int global_id { get; set; }
        public int Number { get; set; }
        public Cells Cells { get; set; }
    }
}
