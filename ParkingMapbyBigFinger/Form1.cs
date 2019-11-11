using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Linq;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace ParkingMapbyBigFinger
{
    public partial class ParkingMapByGCorp : Form
    {
        GMap.NET.WindowsForms.GMapMarker marker;
        GMap.NET.WindowsForms.GMapMarker Person;
        GMapRoute road;
        MapRoute route;
        bool RequestIsOver = false;
        bool LoadIsOver = false;
        bool LoadingIsOver = false;
        GMap.NET.WindowsForms.GMapOverlay markers = new GMap.NET.WindowsForms.GMapOverlay("markers");
        GMapOverlay polygons = new GMapOverlay("polygons");
        public bool isOpen = false;
       public  List<RootObject> data = new List<RootObject>();
        public List<serialopenpark> DataOpenPark = new List<serialopenpark>();
        public bool isHere = false;
         GMapOverlay routesOverlay = new GMapOverlay("path");
        public Bitmap openparkmarker = new Bitmap(Properties.Resources.marker2);
        public Bitmap mainmarker = new Bitmap(Properties.Resources.marker1);
        public string buf = "";
        public string buf_streetparkings = "";


        public void LoadJson()
        {
     
            string json_content = buf;
            string json_content2 = buf_streetparkings;
            data = JsonConvert.DeserializeObject<List<RootObject>>(json_content);
            DataOpenPark = JsonConvert.DeserializeObject<List<serialopenpark>>(json_content2);
            textBox3.Text = data.Count.ToString();
            LoadIsOver = true;
          
            GoStoopid();
        }
        
        bool rdy = false;
        public void RequestData()
        {
           WebRequest request = WebRequest.Create("https://apidata.mos.ru/v1/datasets/1682/rows?api_key=f87a0d8a5117e5d94ece5fbeb75d882b");
           WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    buf =reader.ReadToEnd();
                }
            }
            response.Close();

             request = WebRequest.Create("https://apidata.mos.ru/v1/datasets/623/rows?$top=2000&api_key=f87a0d8a5117e5d94ece5fbeb75d882b");
             response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    buf_streetparkings = reader.ReadToEnd();
                }
            }
            response.Close();
            rdy = true;
            RequestIsOver = true;
            LoadJson();
        }
        public void mapControl1_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            double lat = 0;
            double lng = 0;
            var point = gMapControl1.FromLocalToLatLng(e.X, e.Y);
            lat = point.Lat;
            lng = point.Lng;
           
            if (e.Button == MouseButtons.Left)
            {
                if (gMapControl1.Zoom < 14)
                    gMapControl1.Zoom = 14;
                gMapControl1.Position = new PointLatLng(item.Position.Lat, item.Position.Lng);
            }
            else if (e.Button == MouseButtons.Right && markers.Markers.Contains(Person))
            {
                if (isHere)
                {
                    routesOverlay.Clear();
                    gMapControl1.Overlays.Remove(routesOverlay);
                    route.Clear();
                }
               
                PointLatLng end = new PointLatLng(item.Position.Lat, item.Position.Lng);
                PointLatLng start = new PointLatLng(Person.Position.Lat, Person.Position.Lng);

                route = OpenStreetMapProvider.Instance.GetRoute(start, end, false, false, 10);
                road = new GMapRoute(route.Points, "My path");
                road.Stroke.Width = 2;
                road.Stroke.Color = Color.OrangeRed;

                routesOverlay.Routes.Add(road);
                gMapControl1.Overlays.Add(routesOverlay);
                isHere = true;
                textBox2.Text = route.Distance.ToString();
                gMapControl1.Position = new PointLatLng(lat, lng);




            }
        }

        public ParkingMapByGCorp()
        {
            InitializeComponent();
            progressBar1.Maximum = 100;
            progressBar1.Minimum = 0;
            progressBar1.Step = 10;
           
        }

        public void gMapControl1_Load(object sender, EventArgs e)
        {
            gMapControl1.Bearing = 0;
            gMapControl1.CanDragMap = true; 
            gMapControl1.DragButton = MouseButtons.Left; 
            gMapControl1.GrayScaleMode = true;
            gMapControl1.MaxZoom = 17;
            gMapControl1.MinZoom = 2; 
            gMapControl1.MouseWheelZoomType = MouseWheelZoomType.ViewCenter; 
            gMapControl1.ShowCenter = false;
            gMapControl1.PolygonsEnabled = true; 
            gMapControl1.MarkersEnabled = true;
            gMapControl1.NegativeMode = false; 
            gMapControl1.ShowTileGridLines = false; 
            

            gMapControl1.Zoom = 10;
            gMapControl1.MapProvider = GMapProviders.YandexMap; 
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gMapControl1.SetPositionByKeywords("Moscow, Russia");
        }
         async void Async()
        {

            await Task.Run(() => {
                RequestData();
                


            }); // вызов асинхронной операции


            if (LoadingIsOver)
            {
                gMapControl1.Zoom++;
                gMapControl1.Zoom--;
            }
        }

        void GoStoopid()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            List<PointLatLng> points = new List<PointLatLng>();
            List<PointLatLng> pointsforopen = new List<PointLatLng>();
            for (int i = 0; i < data.Count; i++)
            {
                double lat = data[i].Cells.geoData.coordinates[0][0][1];
                double lng = data[i].Cells.geoData.coordinates[0][0][0];
                marker =
          new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
              new GMap.NET.PointLatLng(lat, lng), mainmarker
              );

                String parking_info = data[i].global_id + "\n" + data[i].Cells.Name + "\n" + data[i].Cells.District + "\n" + "\n" + data[i].Cells.AdmArea + " \n" /* data[i].Address +"\n Максимальное количество парковочных мест: " + data[i].CountSpaces*/ ;
                marker.ToolTipText = parking_info;
                marker.ToolTip.Fill = Brushes.Black;
                marker.ToolTip.Foreground = Brushes.White;
                marker.ToolTip.Stroke = Pens.Black;
                marker.ToolTip.TextPadding = new Size(20, 30);
                marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                marker.IsVisible = true;
                markers.Markers.Add(marker);

            }

            for (int i = 0; i < DataOpenPark.Count; i++)
            {
                marker =
       new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
           new GMap.NET.PointLatLng(DataOpenPark[i].Cells.geoData.coordinates[0][0][1], DataOpenPark[i].Cells.geoData.coordinates[0][0][0]), openparkmarker
          );
                marker.ToolTipText = DataOpenPark[i].Cells.ParkingName + "\n" + DataOpenPark[i].Cells.AdmArea;
                markers.Markers.Add(marker);
            }
            gMapControl1.Overlays.Add(polygons);
            gMapControl1.Overlays.Add(markers);


            gMapControl1.OnMarkerClick +=
       new MarkerClick(mapControl1_OnMarkerClick);

            this.KeyDown += new KeyEventHandler(OKP);

            
            LoadingIsOver = true;
        }
      

        double speed = 0;
        private void OKP(object sender, KeyEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());
            double speed;
            if (gMapControl1.Zoom > 12)
            {
               speed = 0.001;
            }
            else speed = 0.01;
                if (e.KeyCode == Keys.Left)
            {
                gMapControl1.Position = new PointLatLng(gMapControl1.Position.Lat, gMapControl1.Position.Lng - speed);
            }
            if(e.KeyCode == Keys.Right)
            {
                gMapControl1.Position = new PointLatLng(gMapControl1.Position.Lat, gMapControl1.Position.Lng + speed);
            }
            if(e.KeyCode == Keys.Up)
            {
                gMapControl1.Position = new PointLatLng(gMapControl1.Position.Lat + speed, gMapControl1.Position.Lng);
            }
            if (e.KeyCode == Keys.Down)
            {
                gMapControl1.Position = new PointLatLng(gMapControl1.Position.Lat - speed, gMapControl1.Position.Lng);
            }
            }
        

        private void gMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!markers.Markers.Contains(Person))
                {
                    PointLatLng point = gMapControl1.FromLocalToLatLng(e.X, e.Y);

                    Person = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                         point, GMarkerGoogleType.arrow);
                    markers.Markers.Add(Person);
                }
                else
                {
                    markers.Markers.Remove(Person);
                    PointLatLng point = gMapControl1.FromLocalToLatLng(e.X, e.Y);

                    Person = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                         point, GMarkerGoogleType.arrow);
                    markers.Markers.Add(Person);
                }
            } 
           
        }
        public bool fuck = false;


        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
         

               PointLatLng end = new PointLatLng(37.4340760558763, 55.7382816934103);
               PointLatLng start = new PointLatLng(55.7390378802488, 37.4324790462873);
            MapRoute route = GoogleMapProvider.Instance.GetRoute(start, end, false, false, 14);
            GMapRoute road = new GMapRoute(route.Points, "My path");
            GMapOverlay routesOverlay = new GMapOverlay("path");
            routesOverlay.Routes.Add(road);
            gMapControl1.Overlays.Add(routesOverlay);
            //    //}
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Loading newForm = new Loading();
           
           
            Async();
           
                
            
            //RequestData();

           

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
