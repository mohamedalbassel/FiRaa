using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.DesignScript.Runtime;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Events;
using Form = System.Windows.Forms.Form;
using View = Autodesk.Revit.DB.View;
//using System.Diagnostics;
using System.Runtime.InteropServices;
using ComponentManager = Autodesk.Windows.ComponentManager;
using IWin32Window = System.Windows.Forms.IWin32Window;
using Keys = System.Windows.Forms.Keys;
namespace Egyptian_FLS_code
{

    //[IsVisibleInDynamoLibrary(false)]
    public partial class Form1 : Form
    {
        public static double roomareas;
        public static List<string> eclas = new List<string>();
        public static List<string> exp = new List<string>();
        public static List<XYZ> p = new List<XYZ>();
        public static List<XYZ> roomsXyz1 = new List<XYZ>();

        public static bool travel = false;
        public static bool tag = false;
        //public static XYZ p ;
        public static XYZ q;
        public static View v;
        public static IEnumerable<Autodesk.Revit.DB.Architecture.Room> rooms;
        Autodesk.Revit.DB.Parameter com;
        List<string> s = new List<string>();
        
        Document doc2;
        UIDocument uidoc;
        ExternalCommandData com2;
        public static string selectedItemm;
        public static string selectedItemdoor;
        ICollection<ElementId> selectedIds;
        Selection sel;
        Transaction t2;
        // [IsVisibleInDynamoLibrary(false)]
        public Form1( Document doc, ExternalCommandData com, Transaction t3)
        {
            InitializeComponent();
            //IntPtr _revit_window;
            doc2 = doc;
            com2 = com;
            uidoc = com2.Application.ActiveUIDocument;
            sel = uidoc.Selection;
            UIApplication uiapp = com2.Application;
            rooms = new FilteredElementCollector(doc2).WhereElementIsNotElementType().OfClass(typeof(SpatialElement)).Where(ed => ed.GetType() == typeof(Room)).Cast<Room>();
            t2 = t3;
            //XYZ p = sel.PickPoint("Point 1");
            //XYZ q = sel.PickPoint("Point 2");
            //View v = doc2.ActiveView;
            //PathOfTravel.Create(v, p, q);

            this.ShowDialog();
            
            //if (p!=null)
            //{
            //    PathOfTravel.Create(v, p, q);
            //}
            //    IWin32Window revit_window = new JtWindowHandle(ComponentManager.ApplicationWindow); ;
            //    Process process = Process.GetCurrentProcess();
            //    IntPtr h = process.MainWindowHandle;
            //    this.ShowDialog(revit_window);
            //_revit_window = uiapp.MainWindowHandle;
        }
        //public static extern bool SetForegroundWindow(IntPtr hWnd);
        //public static bool ActivateWindow()
        //{
        //    Process p = Process.GetProcessesByName("Revit").FirstOrDefault();
        //    IntPtr ptr = p.MainWindowHandle;

        //    if (ptr != IntPtr.Zero)
        //    {
        //        return SetForegroundWindow(ptr);
        //    }

        //    return false;
        //}




        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
      
         
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
          

            selectedItemm= comboBox1.Text;
         
            Dictionary<string, double> dict = new Dictionary<string, double>()
                {

                {"A-1",60},
                {"A-2",60},
                {"A-3",60},
                {"A-4",500},
                {"I-1",30},
                {"I-2",30},
                {"R-1",30},
                {"R-2",30},
                {"B",60},
                {"M-1",90},
                {"M-2",90},
                {"M-3",90},
                {"F-1",30},
                {"F-2",60},
                {"F-3",60},
                {"null",0}

                 };

           

            double stair = dict[selectedItemm];

            //List<Element> st = new List<Element>();

            selectedIds = uidoc.Selection.GetElementIds();
            foreach (ElementId item in selectedIds)
            {
                Element st = doc2.GetElement(item);
           
                //get parameter of stair
                Autodesk.Revit.DB.Parameter comm = (st.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                string w = (comm.AsString()) ;
                double width = Convert.ToDouble(w);

                double egress_stair = ((width ) / 550) * stair;

                //set parameter of stair
                Autodesk.Revit.DB.Parameter mark = (st.get_Parameter(BuiltInParameter.DOOR_NUMBER));
                mark.Set(egress_stair.ToString());
                MessageBox.Show(egress_stair.ToString());

            }


            
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.radioButton1.Checked = true;
            this.checkBox1.Checked = true;
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> clas = new List<string>();
                foreach (Room r in rooms)
                {



                    Autodesk.Revit.DB.Parameter cm = (r.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                    string comments = getParameterText(cm);

                    clas.Add(comments);
                    roomareas += r.Area;

                }

                eclas = clas;


            }
            catch (Exception eex)
            {

                MessageBox.Show(eex.Message + eex.StackTrace);
            }
            //TransactionManager.Instance.TransactionTaskDone();
            //return s;


            Form2 f = new Form2();
            if (f.ShowDialog() == DialogResult.OK)
            {

            }

            f.Owner = this;




      
            
        }
        private string getParameterText(Autodesk.Revit.DB.Parameter p)
        {
            switch (p.StorageType)
            {
                case StorageType.String:
                    return p.AsString();

                case StorageType.Integer:
                    return p.AsInteger().ToString();

                case StorageType.Double:
                    // check to see if there's a value string first!
                    if (p.AsValueString() != null) return p.AsValueString();
                    return p.AsDouble().ToString();

                case StorageType.ElementId:
                    ElementId eid = p.AsElementId();
                    if (eid.IntegerValue < 0) return "(none)"; // blank
                    // get the element, return the name
                    Element eObj = p.Element.Document.GetElement(eid);
                    return eObj.Name;

                default:
                    return "N/A";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
         
             
               


            if (radioButton1.Checked)
            {
                foreach (Room r in rooms)
                {


                    //rrr = x.ToString();
                    //s.Add(r.Name);
                    //com = r.get_Parameter(BuiltInParameter.ROOM_NAME
                    string RoomName = r.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();
                    com = r.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);


                    if (RoomName.ToLower().Contains("pray") || RoomName.ToLower().Contains("praying"))
                    {

                        com.Set("Prayer");

                    }

                    else if (RoomName.ToLower().Contains("long period cryo") || RoomName.ToLower().Contains("store") || RoomName.ToLower().Contains("ahu room") || RoomName.ToLower().Contains("storage") || RoomName.ToLower().Contains("bms") || RoomName.ToLower().Contains("supplies") || RoomName.ToLower().Contains("cassettes") || RoomName.ToLower().Contains("battery") || RoomName.ToLower().Contains("ups") || RoomName.ToLower().Contains("storage") || RoomName.ToLower().Contains("lift") || RoomName.ToLower().Contains("closet") || RoomName.ToLower().Contains("compressor") || RoomName.ToLower().Contains("containers") || RoomName.ToLower().Contains("loading") || RoomName.ToLower().Contains("disposal") || RoomName.ToLower().Contains("dry") || RoomName.ToLower().Contains("ecg / echo") || RoomName.ToLower().Contains("electrical") || RoomName.ToLower().Contains("mdb") || RoomName.ToLower().Contains("disinfection") || RoomName.ToLower().Contains("receiving") || RoomName.ToLower().Contains("f&b") || RoomName.ToLower().Contains("flammable") || RoomName.ToLower().Contains("gas") || RoomName.ToLower().Contains("waste") || RoomName.ToLower().Contains("electrical") || RoomName.ToLower().Contains("loading") || RoomName.ToLower().Contains("server") || RoomName.ToLower().Contains("me enclosure") || RoomName.ToLower().Contains("mechanical") || RoomName.ToLower().Contains("chute") || RoomName.ToLower().Contains("mep") || RoomName.ToLower().Contains("mv") || RoomName.ToLower().Contains("narcotics") || RoomName.ToLower().Contains("mdb") || RoomName.ToLower().Contains("novec") || RoomName.ToLower().Contains("blower") || RoomName.ToLower().Contains("dispatching") || RoomName.ToLower().Contains("pump") || RoomName.ToLower().Contains("telecom") || RoomName.ToLower().Contains("firefighting") || RoomName.ToLower().Contains("shaft") || RoomName.ToLower().Contains("soft water") || RoomName.ToLower().Contains("soiled") || RoomName.ToLower().Contains("transformer") || RoomName.ToLower().Contains("ups") || RoomName.ToLower().Contains("vacuum") || RoomName.ToLower().Contains("av equipment") || RoomName.ToLower().Contains("equipment alcove") || RoomName.ToLower().Contains("equipment room") || RoomName.ToLower().Contains("utility") || RoomName.ToLower().Contains("data") || RoomName.ToLower().Contains("alarm") || RoomName.ToLower().Contains("noc room") || RoomName.ToLower() == ("noc") || RoomName.ToLower().Contains("valve") || RoomName.ToLower().Contains("plenum") || RoomName.ToLower().Contains("tank") || RoomName.ToLower().Contains("cellular") || RoomName.ToLower().Contains("truck") || RoomName.ToLower().Contains("dock") || RoomName.Contains("ETS") || RoomName.ToLower().Contains("fan") || RoomName.Contains("FTR") || RoomName.Contains("FCC") || RoomName.ToLower().Contains("generator") || RoomName.Contains("LV") || RoomName.Contains("MCC") || RoomName.Contains("NOC") || RoomName.Contains("RSR") || RoomName.Contains("RMU") || RoomName.ToLower().Contains("pit") || RoomName.ToLower().Contains("vault") || RoomName.Contains("MTR") || RoomName.ToLower().Contains("elec") || RoomName.ToLower().Contains("garbage") || RoomName.ToLower().Contains("warehouse") || RoomName.ToLower().Contains("stationary") || RoomName.Contains("PPI") || RoomName.Contains("MPH"))
                    {

                        com.Set("MEP/Storage");

                    }
                    else if (RoomName.ToLower().Contains("amenity") || RoomName.ToLower().Contains("coffee shop") || RoomName.ToLower().Contains("common area") || RoomName.ToLower().Contains("conference") || RoomName.ToLower().Contains("meeting") || RoomName.ToLower().Contains("lounge") || RoomName.ToLower().Contains("entrance") || RoomName.ToLower().Contains("waiting") || RoomName.ToLower().Contains("sub waiting") || RoomName.ToLower().Contains("dining hall") || RoomName.ToLower() == ("bar") || RoomName.ToLower().Contains("rest") || RoomName.ToLower().Contains("lounge") || RoomName.ToLower().Contains("gym") || RoomName.ToLower().Contains("play") || RoomName.ToLower().Contains("seating") || RoomName.ToLower().Contains("bsu") || RoomName.ToLower().Contains("caracass") || RoomName.ToLower().Contains("laboratory") || RoomName.ToLower().Contains("material decontamination") || RoomName.ToLower().Contains("procedure") || RoomName.ToLower().Contains("simulation") || RoomName.ToLower().Contains("classroom") || RoomName.ToLower().Contains("lecture") || RoomName.ToLower().Contains("research Room") || RoomName.ToLower().Contains("teaching") || RoomName.ToLower().Contains("training") || RoomName.ToLower().Contains("nursery") || RoomName.ToLower().Contains("cafe") || RoomName.ToLower().Contains("sitting") || RoomName.ToLower().Contains("break") || RoomName.ToLower().Contains("majlis") || RoomName.ToLower().Contains("majles"))
                    {

                        com.Set("Assembly");

                    }
                    else if (RoomName.ToLower().Contains("body loading") || RoomName.ToLower().Contains("body washing") || RoomName.ToLower().Contains("concealed body trolley park") || RoomName.ToLower().Contains("family viewing room") || RoomName.ToLower().Contains("morgue") || RoomName.ToLower().Contains("refrigerated body storeroom"))
                    {

                        com.Set("Morgue");

                    }
                    else if (RoomName.ToLower().Contains("accu") || RoomName.ToLower().Contains("adult double") || RoomName.ToLower().Contains("adult isolation") || RoomName.ToLower().Contains("adult single") || RoomName.ToLower().Contains("bariatric") || RoomName.ToLower().Contains("cath interventional") || RoomName.ToLower().Contains("clean corridor") || RoomName.ToLower().Contains("sterile") || RoomName.ToLower().Contains("en-suite double") || RoomName.ToLower().Contains("en-suite isolation") || RoomName.ToLower().Contains("en-suite single") || RoomName.ToLower().Contains("hybrid room cath/ot") || RoomName.ToLower().Contains("icu isolation airlock") || RoomName.ToLower().Contains("icu") || RoomName.ToLower().Contains("nicu") || RoomName.ToLower().Contains("surgery") || RoomName.ToLower().Contains("isolation") || RoomName.ToLower().Contains("inpatient") || RoomName.ToLower().Contains("in-patient") || RoomName.ToLower().Contains("peds") || RoomName.ToLower().Contains("perfusion") || RoomName.ToLower().Contains("pre-op") || RoomName.ToLower().Contains("preop cath") || RoomName.ToLower().Contains("scrub-up") || RoomName.ToLower().Contains("x-ray") || RoomName.ToLower().Contains("xray") || RoomName.ToLower().Contains("resuscitation") || RoomName.ToLower().Contains("exam") || RoomName.ToLower().Contains("angiography"))
                    {

                        com.Set("Healthcare");

                    }
                    else if (RoomName.ToLower().Contains("bakery") || RoomName.ToLower().Contains("pastry") || RoomName.ToLower().Contains("beverag") || RoomName.ToLower().Contains("clean linen") || RoomName.ToLower().Contains("kitchen") || RoomName.ToLower().Contains("fridge") || RoomName.ToLower().Contains("de-casing ingredient control") || RoomName.ToLower().Contains("detergent mixing room") || RoomName.ToLower().Contains("dietician") || RoomName.ToLower().Contains("drycleaning ") || RoomName.ToLower().Contains("drying") || RoomName.ToLower().Contains("finishing area") || RoomName.ToLower().Contains("freezer") || RoomName.ToLower().Contains("fish prep") || RoomName.ToLower().Contains("fitting room") || RoomName.ToLower().Contains("ironing") || RoomName.ToLower().Contains("gown") || RoomName.ToLower().Contains("washing") || RoomName.ToLower().Contains("laundry") || RoomName.ToLower().Contains("linen") || RoomName.ToLower().Contains("loading - steam sterilizers") || RoomName.ToLower().Contains("meat") || RoomName.ToLower().Contains("packaging") || RoomName.ToLower().Contains("pick up") || RoomName.ToLower().Contains("sanitation") || RoomName.ToLower().Contains("scrub") || RoomName.ToLower().Contains("steam") || RoomName.ToLower().Contains("sterile issue / dispatch") || RoomName.ToLower().Contains("tailor") || RoomName.ToLower().Contains("tray") || RoomName.ToLower().Contains("trolley") || RoomName.ToLower().Contains("uniform") || RoomName.ToLower().Contains("veg") || RoomName.ToLower().Contains("fruits") || RoomName.ToLower().Contains("wares") || RoomName.ToLower().Contains("pot") || RoomName.ToLower().Contains("washers") || RoomName.ToLower().Contains("washing") || RoomName.ToLower().Contains("weighting") || RoomName.ToLower().Contains("freezers") || RoomName.ToLower().Contains("refrigerat") || RoomName.ToLower().Contains("poultry"))
                    {

                        com.Set("kitchen/Laundry");

                    }
                    else if (RoomName.ToLower().Contains("sequencers") || RoomName.ToLower().Contains("print") || RoomName.ToLower().Contains("donation") || RoomName.ToLower().Contains("vr / ar") || RoomName.ToLower().Contains("artificial intelligence") || RoomName.ToLower().Contains("big data") || RoomName.ToLower().Contains("dark room") || RoomName.ToLower().Contains("biostatistics") || RoomName.ToLower().Contains("valve research") || RoomName.ToLower().Contains("proteomics") || RoomName.ToLower().Contains("cell biology functional genomics") || RoomName.ToLower().Contains("stem cell biology") || RoomName.ToLower().Contains("ablution") || RoomName.ToLower().Contains("workstation") || RoomName.ToLower().Contains("admission") || RoomName.ToLower().Contains("nursing") || RoomName.ToLower().Contains("ambulance") || RoomName.ToLower().Contains("airlock") || RoomName.ToLower().Contains("air lock") || RoomName.ToLower().Contains("alcove") || RoomName.ToLower().Contains("anaesthetist") || RoomName.ToLower().Contains("anesthesia") || RoomName.ToLower().Contains("apheresis") || RoomName.ToLower().Contains("archiving") || RoomName.ToLower().Contains("aseptic") || RoomName.ToLower().Contains("bathroom") || RoomName.ToLower().Contains("atm") || RoomName.ToLower().Contains("bag release") || RoomName.ToLower().Contains("counter") || RoomName.ToLower().Contains("office") || RoomName.ToLower().Contains("irradiator") || RoomName.ToLower().Contains("processing manufacturing") || RoomName.ToLower().Contains("bereavement") || RoomName.ToLower().Contains("beverage") || RoomName.ToLower().Contains("billing") || RoomName.ToLower().Contains("biobank") || RoomName.ToLower().Contains("biomedical workshop") || RoomName.ToLower().Contains("blood bank") || RoomName.ToLower().Contains("boh") || RoomName.ToLower().Contains("call center") || RoomName.ToLower().Contains("cart parking") || RoomName.ToLower().Contains("cashier") || RoomName.ToLower().Contains("cath") || RoomName.ToLower().Contains("boardroom") || RoomName.ToLower().Contains("pathologist") || RoomName.ToLower().Contains("chroma") || RoomName.ToLower().Contains("circulation") || RoomName.ToLower().Contains("corridor") || RoomName.ToLower().Contains("clean") || RoomName.ToLower().Contains("clinical") || RoomName.ToLower().Contains("cloak") || RoomName.ToLower().Contains("co2") || RoomName.ToLower().Contains("consult") || RoomName.ToLower().Contains("control room") || RoomName.ToLower().Contains("copy room") || RoomName.ToLower().Contains("corridor") || RoomName.ToLower().Contains("cpr") || RoomName.ToLower().Contains("crash cart alcove") || RoomName.ToLower().Contains("ct changing") || RoomName.ToLower().Contains("ct control") || RoomName.ToLower().Contains("ct equipment") || RoomName.ToLower().Contains("ct scan") || RoomName.ToLower().Contains("ct wc") || RoomName.ToLower().Contains("ct-mri prep/recov") || RoomName.ToLower().Contains("care") || RoomName.ToLower().Contains("decontam") || RoomName.ToLower().Contains("director") || RoomName.ToLower().Contains("wc") || RoomName.ToLower().Contains("toilet") || RoomName.ToLower().Contains("pharmacy") || RoomName.ToLower().Contains("dispensary") || RoomName.ToLower().Contains("disposal") || RoomName.ToLower().Contains("counter") || RoomName.ToLower().Contains("doctor") || RoomName.ToLower().Contains("resident") || RoomName.ToLower().Contains("ecg measurement") || RoomName.ToLower().Contains("echo") || RoomName.ToLower().Contains("em equipment") || RoomName.ToLower().Contains("embedding") || RoomName.ToLower().Contains("examination") || RoomName.ToLower().Contains("stress") || RoomName.ToLower().Contains("extemporaneous") || RoomName.ToLower().Contains("changing room") || RoomName.ToLower().Contains("lockers") || RoomName.ToLower().Contains("showers") || RoomName.ToLower().Contains("fire command center") || RoomName.ToLower().Contains("microscope") || RoomName.ToLower().Contains("office") || RoomName.ToLower().Contains("alcove") || RoomName.ToLower().Contains("future") || RoomName.ToLower().Contains("x-ray") || RoomName.ToLower().Contains("gamma") || RoomName.ToLower().Contains("gross") || RoomName.ToLower().Contains("security") || RoomName.ToLower().Contains("guards") || RoomName.ToLower().Contains("holter monitor room") || RoomName.ToLower().Contains("homeostasis coagulation") || RoomName.ToLower().Contains("hospital information") || RoomName.ToLower().Contains("hot patient") || RoomName.ToLower().Contains("house keeping") || RoomName.ToLower().Contains("housekeeping") || RoomName.ToLower().Contains("hplc") || RoomName.ToLower().Contains("hyperbaric chamber") || RoomName.ToLower().Contains("imaging viewing") || RoomName.ToLower().Contains("diagnostic") || RoomName.ToLower().Contains("reporting area") || RoomName.ToLower().Contains("immun") || RoomName.ToLower().Contains("vaccination") || RoomName.ToLower().Contains("labeling") || RoomName.ToLower().Contains("quarantine") || RoomName.ToLower().Contains("interview") || RoomName.ToLower().Contains("janitor") || RoomName.ToLower().Contains("laminar flow") || RoomName.ToLower().Contains("laser") || RoomName.ToLower().Contains("lift") || RoomName.ToLower().Contains("lobby") || RoomName.ToLower().Contains("switchboard") || RoomName.ToLower().Contains("operator") || RoomName.ToLower().Contains("maintenance workshop") || RoomName.ToLower().Contains("media preparation") || RoomName.ToLower().Contains("medical gas") || RoomName.ToLower().Contains("medication room") || RoomName.ToLower().Contains("microtom") || RoomName.ToLower().Contains("milk expressing room") || RoomName.ToLower().Contains("minor procedure") || RoomName.ToLower().Contains("assistant") || RoomName.ToLower().Contains("mri") || RoomName.ToLower().Contains("nourishment") || RoomName.ToLower().Contains("observation") || RoomName.ToLower().Contains("duty") || RoomName.ToLower().Contains("opd") || RoomName.ToLower().Contains("paitent") || RoomName.ToLower().Contains("peds") || RoomName.ToLower().Contains("pet ct") || RoomName.ToLower().Contains("measure") || RoomName.ToLower().Contains("physiotherapy") || RoomName.ToLower().Contains("point of care") || RoomName.ToLower().Contains("security") || RoomName.ToLower().Contains("porters") || RoomName.ToLower().Contains("post donation beverage") || RoomName.ToLower().Contains("drivers") || RoomName.ToLower().Contains("corridor") || RoomName.ToLower().Contains("pts") || RoomName.ToLower().Contains("toilets") || RoomName.ToLower().Contains("pulmonary function test") || RoomName.ToLower().Contains("purchasing") || RoomName.ToLower().Contains("quality control") || RoomName.ToLower().Contains("radio-diagnostic imaging") || RoomName.ToLower().Contains("radioactive") || RoomName.ToLower().Contains("recording area") || RoomName.ToLower().Contains("reception") || RoomName.ToLower().Contains("reviewing") || RoomName.ToLower().Contains("receiving") || RoomName.ToLower().Contains("records") || RoomName.ToLower().Contains("respiratory") || RoomName.ToLower().Contains("resusciation bay") || RoomName.ToLower().Contains("roof") || RoomName.ToLower().Contains("routine hematology") || RoomName.ToLower().Contains("scope disinfection") || RoomName.ToLower().Contains("scrub") || RoomName.ToLower().Contains("secretarial") || RoomName.ToLower().Contains("secretary") || RoomName.ToLower().Contains("seminar") || RoomName.ToLower().Contains("service counter") || RoomName.ToLower().Contains("firefighting") || RoomName.ToLower().Contains("serviced disinfected equipment") || RoomName.ToLower().Contains("soiled") || RoomName.ToLower().Contains("staff") || RoomName.ToLower().Contains("staining") || RoomName.ToLower().Contains("stair") || RoomName.ToLower().Contains("stationeries") || RoomName.ToLower().Contains("stretcher") || RoomName.ToLower().Contains("support") || RoomName.ToLower().Contains("wash") || RoomName.ToLower().Contains("tea") || RoomName.ToLower().Contains("team") || RoomName.ToLower().Contains("exit") || RoomName.ToLower().Contains("sterilization") || RoomName.ToLower().Contains("terrace") || RoomName.ToLower().Contains("therapist") || RoomName.ToLower().Contains("therapy") || RoomName.ToLower().Contains("tilt table room") || RoomName.ToLower().Contains("tpn") || RoomName.ToLower().Contains("translational space") || RoomName.ToLower().Contains("triage") || RoomName.ToLower().Contains("ultrasound") || RoomName.ToLower().Contains("universal") || RoomName.ToLower().Contains("urine & stool analysis") || RoomName.ToLower().Contains("vending machines") || RoomName.ToLower().Contains("venepuncture") || RoomName.ToLower().Contains("vestibule") || RoomName.ToLower().Contains("visitor") || RoomName.ToLower().Contains("wash") || RoomName.ToLower().Contains("waste") || RoomName.ToLower().Contains("wheelchair") || RoomName.ToLower().Contains("lab") || RoomName.ToLower().Contains("breavement") || RoomName.ToLower().Contains("library") || RoomName.Contains("MR") || RoomName == ("JC") || RoomName == ("J.C") || RoomName == ("J.C.") || RoomName.ToLower().Contains("locker") || RoomName.ToLower().Contains("mail") || RoomName.ToLower().Contains("manager") || RoomName.ToLower().Contains("nurse station") || RoomName.ToLower().Contains("pantry") || RoomName.ToLower().Contains("printer") || RoomName.ToLower().Contains("bath") || RoomName.ToLower().Contains("elevator") || RoomName.ToLower().Contains("call") || RoomName.ToLower().Contains("vend") || RoomName.ToLower().Contains("workshop") || RoomName.ToLower().Contains("workroom") || RoomName.ToLower().Contains("breast feeding") || RoomName.ToLower().Contains("hear") || RoomName.ToLower().Contains("work") || RoomName.ToLower().Contains("gift") || RoomName.ToLower().Contains("patient") || RoomName.ToLower().Contains("imaging") || RoomName.ToLower().Contains("mammography") || RoomName.ToLower().Contains("radiography") || RoomName.ToLower().Contains("fluoroscopy") || RoomName.ToLower().Contains("nuclear") || RoomName.ToLower().Contains("phlebotomy") || RoomName.ToLower().Contains("regist") || RoomName.ToLower().Contains("archive") || RoomName.ToLower().Contains("board") || RoomName.ToLower().Contains("cash") || RoomName.ToLower().Contains("money") || RoomName.ToLower().Contains("count") || RoomName.ToLower().Contains("customer") || RoomName.ToLower().Contains("deputy") || RoomName.ToLower().Contains("chairman") || RoomName.ToLower().Contains("emergency") || RoomName.ToLower().Contains("facility") || RoomName.ToLower().Contains("vistibule") || RoomName.Contains("GM") || RoomName.ToLower().Contains("g.m") || RoomName.ToLower().Contains("g.m.") || RoomName.ToLower().Contains("sec.") || RoomName.ToLower().Contains("secretary") || RoomName.ToLower().Contains("teller") || RoomName.ToLower().Contains("head") || RoomName.ToLower().Contains("paper shred") || RoomName.ToLower().Contains("bank") || RoomName.ToLower().Contains("commander") || RoomName.ToLower().Contains("financial") || RoomName.ToLower().Contains("foyer") || RoomName.ToLower().Contains("invest") || RoomName.ToLower().Contains("hold") || RoomName.ToLower().Contains("passport") || RoomName.ToLower().Contains("immigrat") || RoomName.ToLower().Contains("inspect") || RoomName.ToLower().Contains("operat") || RoomName.ToLower().Contains("detention") || RoomName.ToLower().Contains("respon") || RoomName.ToLower().Contains("customs") || RoomName.ToLower().Contains("shoe"))
                    {

                        com.Set("Business");

                    }
                    else if (RoomName.ToLower().Contains("bedroom") || RoomName.ToLower().Contains("dinning") || RoomName.ToLower().Contains("ensuite") || RoomName.ToLower().Contains("family overnight stay") || RoomName.ToLower().Contains("hallway") || RoomName.ToLower().Contains("kitchenette") || RoomName.ToLower().Contains("living") || RoomName.ToLower().Contains("suite"))
                    {

                        com.Set("Residential");

                    }
                    else if (RoomName.ToLower().Contains("park"))
                    {

                        com.Set("Parking");

                    }

                    else
                    {
                        com.Set("null");
                        exp.Add(RoomName);
                    }

                }



                Dictionary<string, double> dict = new Dictionary<string, double>()
                {

                {"Business",10},
                {"Prayer",0.65},
                {"Assembly",1.2},
                {"MEP/Storage",30},
                {"kitchen/Laundry",10},
                {"Healthcare",10},
                {"Morgue",10},
                {"Residential",2},
                {"Parking",30},
                {"null",0}

                };

               

                foreach (Room r in rooms)
                {



                    Autodesk.Revit.DB.Parameter cm = (r.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                    string comments = getParameterText(cm);
                    double occ = dict[comments];
                    Autodesk.Revit.DB.Parameter occupantfactor = r.get_Parameter(BuiltInParameter.ROOM_OCCUPANCY);
                    occupantfactor.Set(occ.ToString());

                    double area = r.Area;
                    double occupancy = (area / 10.764) / occ;
                    Autodesk.Revit.DB.Parameter basefinish = r.get_Parameter(BuiltInParameter.ROOM_FINISH_BASE);
                    basefinish.Set(occupancy.ToString());
                    
                    //clas.Add(comments);


                }
            }

            if (radioButton2.Checked)
            {

            }
            if (radioButton3.Checked)
            {

            }

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            travel = true;
            if (this.checkBox1.Checked)
            {
                tag = true;
            }
            else
            {
                tag = false;
            }
            //Debugger.Launch();
            
            //Hide();
            ////using (Transaction tr = new Transaction(doc2, "Path"))
            ////{
            ////    tr.Start();

               
            //    try
            //    {
            //        XYZ q = uidoc.Selection.PickPoint("Point 2");

            //        List<Room> rms = new FilteredElementCollector(doc2).WhereElementIsNotElementType().OfClass(typeof(SpatialElement)).Where(ed => ed.GetType() == typeof(Room)).Cast<Room>().ToList();
            //        foreach (Room rm in rms)
            //        {
            //            Location x = rm.Location;
            //            LocationPoint v = x as LocationPoint;
            //            XYZ pp = v.Point;
            //            roomsXyz1.Add(pp);
            //        }


            //        p.Add(q);
            //    }
            //    catch (Exception er)
            //    {

            //        System.Windows.MessageBox.Show(er.Message + er.StackTrace);
            //    }

            //Show();
            //    IList<PathOfTravel> routeMap = PathOfTravel.CreateMapped(uidoc.ActiveView, roomsXyz1, p);
          

            //    tr.Commit();

            //}


            //this.Hide();
            //try
            //{

            //Reference selection = uidoc.Selection.PickObject(ObjectType.Element);      //Select Element 1

            //ElementId eleId = selection.ElementId;
            //LocationPoint eleLocPt = doc2.GetElement(eleId).Location as LocationPoint;
            //XYZ roomXyzPoint1 = eleLocPt.Point;


            //Reference selection2 = uidoc.Selection.PickObject(ObjectType.Element);     //Select Element 2
            //ElementId eleId2 = selection2.ElementId;
            //LocationPoint eleLocPt2 = doc2.GetElement(eleId2).Location as LocationPoint;
            //XYZ roomXyzPoint2 = eleLocPt2.Point;



            //XYZ P1 = new XYZ(0,0,0);
            //XYZ P2 = new XYZ(5,5,0);
            //List<XYZ> pts = new List<XYZ>();


            //for (int i = 0; i < 2; i++)
            //{
            //    pts.Add(uidoc.Selection.PickPoint());
            //    //Autodesk.Revit.DB.XYZ PICKEDXY = uidoc.Selection.PickPoint();


            //}
            //P1 = pts[0];
            //P2 = pts[1];
            //MessageBox.Show("2p");

            //bjectSnapTypes snapTypes = ObjectSnapTypes.Endpoints | ObjectSnapTypes.Intersections;
            //XYZ point = uidoc.Selection.PickPoint(snapTypes, "Select an end point or intersection");


            //this.OnActivated(e);
            //this.Show();

            //Element selection_PICK = doc2.GetElement(hasPickOne);


            //Autodesk.Revit.DB.XYZ PICKEDXY2 = new Autodesk.Revit.DB.XYZ(hasPickOne.GlobalPoint.X, hasPickOne.GlobalPoint.Y, 0);


            // View CurrentActiveView { get; }

            // P2 = PICKEDXY2;


            //, out PathOfTravelCalculationStatus resultStatus
            //MessageBox.Show(p.ToString() + q.ToString());

            //}
            //catch (Exception eex)
            //{

            //    MessageBox.Show(eex.Message + eex.StackTrace);
            //}

            this.Close();
            //this.Show();
            //Egyptian_FLS_code.CMDFLS.documentTransaction.Commit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //UIApplication uiApp = new UIApplication(com2.Application.Application);
            ////UIApplication uiApp = sender as UIApplication;
            //Document doc = uiApp.ActiveUIDocument.Document;
            //using (Transaction transaction = new Transaction(doc, "Text Note Update"))
            //{
            //    XYZ P1 = Form1.p;
            //    XYZ P2 = Form1.q;

            //    transaction.Start();
            //    PathOfTravel.Create(v, p, q);
            //    transaction.Commit();
            //}

            this.Close();
            // PathOfTravel.Create(v, p, q);
           
        }

        private void button5_Click_1(object sender, EventArgs e)
        {

            Form3 f3 = new Form3();
            if (f3.ShowDialog() == DialogResult.OK)
            {

            }

            f3.Owner = this;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            roomareas = 0;
            foreach (Room r in rooms)
            {


                roomareas += r.Area;

            }
            Form4 f4 = new Form4(doc2, com2, t2);
            if (f4.ShowDialog() == DialogResult.OK)
            {

            }

            f4.Owner = this;
            
           // System.Windows.MessageBox.Show(roomareas.ToString());
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            selectedItemdoor = comboBox2.Text;

            Dictionary<string, double> dictdoor = new Dictionary<string, double>()
                {

           
                {"A-3",110},
                {"B",36},
                {"C",45},
                {"D",75},
                {"H",110},
                {"W-1",45},
                {"W-2",75},
                {"W-3",75},
                {"null",0}

                 };

            double door = dictdoor[selectedItemdoor];

            //List<Element> st = new List<Element>();
            
            selectedIds = uidoc.Selection.GetElementIds();
            foreach (ElementId item in selectedIds)
            {
                try
                {
                   
                    Element dr = doc2.GetElement(item);
                    FamilyInstance family = dr as FamilyInstance;
                    //get parameter of stair
                    Autodesk.Revit.DB.Parameter wid = (family.Symbol.get_Parameter(BuiltInParameter.DOOR_WIDTH));

                    double width = wid.AsDouble()/ 0.00328084;

                    double egress_door = (width / door);
               
                    //set parameter of stair
                    Autodesk.Revit.DB.Parameter com = (dr.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS));
                    com.Set(egress_door.ToString());
                    MessageBox.Show(egress_door.ToString());
                }
                catch (Exception err)
                {

                    MessageBox.Show(err.Message + err.StackTrace);
                }
               

            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"D:\DAR-BIM STANDARD\ماجستير\BIM masters\Fire\EGY code.pdf");
            }
            catch (Exception)
            {

                MessageBox.Show("File is not Exist");
            }
           
            //System.Diagnostics.Process.Start("pack://application:,,,/Egyptian FLS code;component/Resources/EGY code.pdf");

            //Form5 f5 = new Form5();
            //if (f5.ShowDialog() == DialogResult.OK)
            //{

            //}

            //f5.Owner = this;



            //var dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.Filter = "PDF files (*.pdf)|*.pdf";
            //if (dlg.ShowDialog().Value)
            //    _c1pdf.LoadDocument(dlg.FileName);

            //var code = Properties.Resources.EGY_code;

            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    axAcroPDF1.src = openFileDialog1.FileName;

            //  Help.ShowHelp(this, "pack://application:,,,/Egyptian FLS code;component/Resources/EGY code.pdf", HelpNavigator.TableOfContents, "");
           // System.IO.FileMode mode = new System.IO.FileMode();
            //var file = (byte[])reader["pack://application:,,,/Egyptian FLS code;component/Resources/EGY code.pdf"];

           

        }
    }

    //public class JtWindowHandle : IWin32Window
    //{
    //    IntPtr _hwnd;

    //    public JtWindowHandle(IntPtr h)
    //    {
    //        Debug.Assert(IntPtr.Zero != h, "expected non-null window handle");

    //        _hwnd = h;
    //    }

    //    public IntPtr Handle
    //    {
    //        get
    //        {
    //            return _hwnd;
    //        }
    //    }
    //}

}
