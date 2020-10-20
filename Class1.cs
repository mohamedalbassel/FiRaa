using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dynamo.Graph.Nodes;
using Autodesk.DesignScript.Runtime;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.IO;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Architecture;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace Egyptian_FLS_code
{
    public sealed class SpellCheck
    {

    }
    

    [IsVisibleInDynamoLibrary(false)]
    public class Spelling
    {
        public static Dictionary<String, int> _dictionary = new Dictionary<String, int>();
        public static Regex _wordRegex = new Regex("[a-z]+", RegexOptions.Compiled);


        public static string Correct(string word)
        {
            
            
            string newpath = Properties.Resources.spelldata;

            //string path = @"D:\DAR-BIM STANDARD\ماجستير\BIM masters\C#\Egyptian FLS code\big.txt";
            //string fileContent = File.ReadAllText(path);
            List<string> wordList = newpath.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var i in wordList)
            {
                string trimmedWord = i.Trim().ToLower();
                if (_wordRegex.IsMatch(trimmedWord))
                {
                    if (_dictionary.ContainsKey(trimmedWord))
                        _dictionary[trimmedWord]++;
                    else
                        _dictionary.Add(trimmedWord, 1);
                }

            }
            if (string.IsNullOrEmpty(word))
                return word;

            word = word.ToLower();

            // known()
            if (_dictionary.ContainsKey(word))
                return word;

            List<String> list = Edits(word);
            Dictionary<string, int> candidates = new Dictionary<string, int>();

            foreach (string wordVariation in list)
            {
                if (_dictionary.ContainsKey(wordVariation) && !candidates.ContainsKey(wordVariation))
                    candidates.Add(wordVariation, _dictionary[wordVariation]);
            }

            if (candidates.Count > 0)
                return candidates.OrderByDescending(x => x.Value).First().Key;

            // known_edits2()
            foreach (string item in list)
            {
                foreach (string wordVariation in Edits(item))
                {
                    if (_dictionary.ContainsKey(wordVariation) && !candidates.ContainsKey(wordVariation))
                        candidates.Add(wordVariation, _dictionary[wordVariation]);
                }
            }

            return (candidates.Count > 0) ? candidates.OrderByDescending(x => x.Value).First().Key : word;

        }

        public static List<string> Edits(string word)
        {
            var splits = new List<Tuple<string, string>>();
            var transposes = new List<string>();
            var deletes = new List<string>();
            var replaces = new List<string>();
            var inserts = new List<string>();

            // Splits
            for (int i = 0; i < word.Length; i++)
            {
                var tuple = new Tuple<string, string>(word.Substring(0, i), word.Substring(i));
                splits.Add(tuple);
            }

            // Deletes
            for (int i = 0; i < splits.Count; i++)
            {
                string a = splits[i].Item1;
                string b = splits[i].Item2;
                if (!string.IsNullOrEmpty(b))
                {
                    deletes.Add(a + b.Substring(1));
                }
            }

            // Transposes
            for (int i = 0; i < splits.Count; i++)
            {
                string a = splits[i].Item1;
                string b = splits[i].Item2;
                if (b.Length > 1)
                {
                    transposes.Add(a + b[1] + b[0] + b.Substring(2));
                }
            }

            // Replaces
            for (int i = 0; i < splits.Count; i++)
            {
                string a = splits[i].Item1;
                string b = splits[i].Item2;
                if (!string.IsNullOrEmpty(b))
                {
                    for (char c = 'a'; c <= 'z'; c++)
                    {
                        replaces.Add(a + c + b.Substring(1));
                    }
                }
            }

            // Inserts
            for (int i = 0; i < splits.Count; i++)
            {
                string a = splits[i].Item1;
                
                string b = splits[i].Item2;
                for (char c = 'a'; c <= 'z'; c++)
                {
                    inserts.Add(a + c + b);
                }
            }

            return deletes.Union(transposes).Union(replaces).Union(inserts).ToList();
        }
    }
    public static class FLS
    {
        


        public static string SpellCheck(string RoomName)
        {
           
            return Spelling.Correct(RoomName);

     
        }

        public static string selectedItem;
        public static string Occupancy(string RoomName)
        {
            //foreach (string RoomName in roomrevit().ToString())
            //{

            //}



            if (RoomName.ToLower().Contains("prayer") || RoomName.ToLower().Contains("praying"))
            {

                RoomName = "Prayer";

            }

            else if (RoomName.ToLower().Contains("long period cryo") || RoomName.ToLower().Contains("store") || RoomName.ToLower().Contains("ahu room") || RoomName.ToLower().Contains("storage") || RoomName.ToLower().Contains("bms") ||
                RoomName.ToLower().Contains("supplies") || RoomName.ToLower().Contains("cassettes") || RoomName.ToLower().Contains("battery") || RoomName.ToLower().Contains("ups") || RoomName.ToLower().Contains("storage") || 
                RoomName.ToLower().Contains("lift") || RoomName.ToLower().Contains("closet") || RoomName.ToLower().Contains("compressor") || RoomName.ToLower().Contains("containers") || RoomName.ToLower().Contains("loading") || 
                RoomName.ToLower().Contains("disposal") || RoomName.ToLower().Contains("dry") || RoomName.ToLower().Contains("ecg / echo") || RoomName.ToLower().Contains("electrical") || RoomName.ToLower().Contains("mdb") || 
                RoomName.ToLower().Contains("disinfection") || RoomName.ToLower().Contains("receiving") || RoomName.ToLower().Contains("f&b") || RoomName.ToLower().Contains("flammable") || RoomName.ToLower().Contains("gas") || 
                RoomName.ToLower().Contains("waste") || RoomName.ToLower().Contains("electrical") || RoomName.ToLower().Contains("loading") || RoomName.ToLower().Contains("server") || RoomName.ToLower().Contains("me enclosure") || 
                RoomName.ToLower().Contains("mechanical") || RoomName.ToLower().Contains("chute") || RoomName.ToLower().Contains("mep") || RoomName.ToLower().Contains("mv") || RoomName.ToLower().Contains("narcotics") || 
                RoomName.ToLower().Contains("mdb") || RoomName.ToLower().Contains("novec") || RoomName.ToLower().Contains("blower") || RoomName.ToLower().Contains("dispatching") || RoomName.ToLower().Contains("pump") || 
                RoomName.ToLower().Contains("telecom") || RoomName.ToLower().Contains("firefighting") || RoomName.ToLower().Contains("shaft") || RoomName.ToLower().Contains("soft water") || RoomName.ToLower().Contains("soiled") ||
                RoomName.ToLower().Contains("transformer") || RoomName.ToLower().Contains("ups") || RoomName.ToLower().Contains("vacuum") || RoomName.ToLower().Contains("av equipment") || RoomName.ToLower().Contains("equipment alcove") 
                || RoomName.ToLower().Contains("equipment room") || RoomName.ToLower().Contains("utility") || RoomName.ToLower().Contains("data") || RoomName.ToLower().Contains("alarm") || RoomName.ToLower().Contains("noc room") || 
                RoomName.ToLower() == ("noc") || RoomName.ToLower().Contains("valve") || RoomName.ToLower().Contains("plenum") || RoomName.ToLower().Contains("tank") || RoomName.ToLower().Contains("cellular") || RoomName.ToLower().Contains("truck") 
                || RoomName.ToLower().Contains("dock") || RoomName.Contains("ETS") || RoomName.ToLower().Contains("fan") || RoomName.Contains("FTR") || RoomName.Contains("FCC") || RoomName.ToLower().Contains("generator") || RoomName.Contains("LV") ||
                RoomName.Contains("MCC") || RoomName.Contains("NOC") || RoomName.Contains("RSR") || RoomName.Contains("RMU") || RoomName.ToLower().Contains("pit") || RoomName.ToLower().Contains("vault") || RoomName.Contains("MTR") 
                || RoomName.ToLower().Contains("elec") || RoomName.ToLower().Contains("garbage") || RoomName.ToLower().Contains("warehouse") || RoomName.ToLower().Contains("stationary") || RoomName.Contains("PPI") || RoomName.Contains("MPH"))
            {

                RoomName = "MEP/Storage";

            }
            else if (RoomName.ToLower().Contains("amenity") || RoomName.ToLower().Contains("coffee shop") || RoomName.ToLower().Contains("common area") || RoomName.ToLower().Contains("conference") || RoomName.ToLower().Contains("meeting") ||
                RoomName.ToLower().Contains("lounge") || RoomName.ToLower().Contains("entrance") || RoomName.ToLower().Contains("waiting") || RoomName.ToLower().Contains("sub waiting") || RoomName.ToLower().Contains("dining hall") || 
                RoomName.ToLower() == ("bar") || RoomName.ToLower().Contains("rest") || RoomName.ToLower().Contains("lounge") || RoomName.ToLower().Contains("gym") || RoomName.ToLower().Contains("play") || RoomName.ToLower().Contains("seating") 
                || RoomName.ToLower().Contains("bsu") || RoomName.ToLower().Contains("caracass") || RoomName.ToLower().Contains("laboratory") || RoomName.ToLower().Contains("material decontamination") || RoomName.ToLower().Contains("procedure") 
                || RoomName.ToLower().Contains("simulation") || RoomName.ToLower().Contains("classroom") || RoomName.ToLower().Contains("lecture") || RoomName.ToLower().Contains("research Room") || RoomName.ToLower().Contains("teaching") 
                || RoomName.ToLower().Contains("training") || RoomName.ToLower().Contains("nursery") || RoomName.ToLower().Contains("cafe") || RoomName.ToLower().Contains("sitting") || RoomName.ToLower().Contains("break") || 
                RoomName.ToLower().Contains("majlis") || RoomName.ToLower().Contains("majles"))
            {

                RoomName = "Assembly";

            }
            else if (RoomName.ToLower().Contains("body loading") || RoomName.ToLower().Contains("body washing") || RoomName.ToLower().Contains("concealed body trolley park") || RoomName.ToLower().Contains("family viewing room") || 
                RoomName.ToLower().Contains("morgue") || RoomName.ToLower().Contains("refrigerated body storeroom"))
            {

                RoomName = "Morgue";

            }
            else if (RoomName.ToLower().Contains("accu") || RoomName.ToLower().Contains("adult double") || RoomName.ToLower().Contains("adult isolation") || RoomName.ToLower().Contains("adult single") || RoomName.ToLower().Contains("bariatric") || 
                RoomName.ToLower().Contains("cath interventional") || RoomName.ToLower().Contains("clean corridor") || RoomName.ToLower().Contains("sterile") || RoomName.ToLower().Contains("en-suite double") || 
                RoomName.ToLower().Contains("en-suite isolation") || RoomName.ToLower().Contains("en-suite single") || RoomName.ToLower().Contains("hybrid room cath/ot") || RoomName.ToLower().Contains("icu isolation airlock") ||
                RoomName.ToLower().Contains("icu") || RoomName.ToLower().Contains("nicu") || RoomName.ToLower().Contains("surgery") || RoomName.ToLower().Contains("isolation") || RoomName.ToLower().Contains("inpatient") || 
                RoomName.ToLower().Contains("in-patient") || RoomName.ToLower().Contains("peds") || RoomName.ToLower().Contains("perfusion") || RoomName.ToLower().Contains("pre-op") || RoomName.ToLower().Contains("preop cath") || 
                RoomName.ToLower().Contains("scrub-up") || RoomName.ToLower().Contains("x-ray") || RoomName.ToLower().Contains("xray") || RoomName.ToLower().Contains("resuscitation") || RoomName.ToLower().Contains("exam") || 
                RoomName.ToLower().Contains("angiography"))
            {

                RoomName = "Healthcare";

            }
            else if (RoomName.ToLower().Contains("bakery") || RoomName.ToLower().Contains("pastry") || RoomName.ToLower().Contains("beverag") || RoomName.ToLower().Contains("clean linen") || RoomName.ToLower().Contains("kitchen") ||
                RoomName.ToLower().Contains("fridge") || RoomName.ToLower().Contains("de-casing ingredient control") || RoomName.ToLower().Contains("detergent mixing room") || RoomName.ToLower().Contains("dietician") || 
                RoomName.ToLower().Contains("drycleaning ") || RoomName.ToLower().Contains("drying") || RoomName.ToLower().Contains("finishing area") || RoomName.ToLower().Contains("freezer") || RoomName.ToLower().Contains("fish prep") || 
                RoomName.ToLower().Contains("fitting room") || RoomName.ToLower().Contains("ironing") || RoomName.ToLower().Contains("gown") || RoomName.ToLower().Contains("washing") || RoomName.ToLower().Contains("laundry") || 
                RoomName.ToLower().Contains("linen") || RoomName.ToLower().Contains("loading - steam sterilizers") || RoomName.ToLower().Contains("meat") || RoomName.ToLower().Contains("packaging") || RoomName.ToLower().Contains("pick up") ||
                RoomName.ToLower().Contains("sanitation") || RoomName.ToLower().Contains("scrub") || RoomName.ToLower().Contains("steam") || RoomName.ToLower().Contains("sterile issue / dispatch") || RoomName.ToLower().Contains("tailor") || 
                RoomName.ToLower().Contains("tray") || RoomName.ToLower().Contains("trolley") || RoomName.ToLower().Contains("uniform") || RoomName.ToLower().Contains("veg") || RoomName.ToLower().Contains("fruits") || 
                RoomName.ToLower().Contains("wares") || RoomName.ToLower().Contains("pot") || RoomName.ToLower().Contains("washers") || RoomName.ToLower().Contains("washing") || RoomName.ToLower().Contains("weighting") || 
                RoomName.ToLower().Contains("freezers") || RoomName.ToLower().Contains("refrigerat") || RoomName.ToLower().Contains("poultry"))
            {

                RoomName = "kitchen/Laundry";

            }
            else if (RoomName.ToLower().Contains("sequencers") || RoomName.ToLower().Contains("print") || RoomName.ToLower().Contains("donation") || RoomName.ToLower().Contains("vr / ar") || RoomName.ToLower().Contains("artificial intelligence") || RoomName.ToLower().Contains("big data") || RoomName.ToLower().Contains("dark room") || RoomName.ToLower().Contains("biostatistics") || RoomName.ToLower().Contains("valve research") || RoomName.ToLower().Contains("proteomics") || RoomName.ToLower().Contains("cell biology functional genomics") || RoomName.ToLower().Contains("stem cell biology") || RoomName.ToLower().Contains("ablution") || RoomName.ToLower().Contains("workstation") || RoomName.ToLower().Contains("admission") || RoomName.ToLower().Contains("nursing") || RoomName.ToLower().Contains("ambulance") || RoomName.ToLower().Contains("airlock") || RoomName.ToLower().Contains("air lock") || RoomName.ToLower().Contains("alcove") || RoomName.ToLower().Contains("anaesthetist") || RoomName.ToLower().Contains("anesthesia") || RoomName.ToLower().Contains("apheresis") || RoomName.ToLower().Contains("archiving") || RoomName.ToLower().Contains("aseptic") || RoomName.ToLower().Contains("bathroom") || RoomName.ToLower().Contains("atm") || RoomName.ToLower().Contains("bag release") || RoomName.ToLower().Contains("counter") || RoomName.ToLower().Contains("office") || RoomName.ToLower().Contains("irradiator") || RoomName.ToLower().Contains("processing manufacturing") || RoomName.ToLower().Contains("bereavement") || RoomName.ToLower().Contains("beverage") || RoomName.ToLower().Contains("billing") || RoomName.ToLower().Contains("biobank") || RoomName.ToLower().Contains("biomedical workshop") || RoomName.ToLower().Contains("blood bank") || RoomName.ToLower().Contains("boh") || RoomName.ToLower().Contains("call center") || RoomName.ToLower().Contains("cart parking") || RoomName.ToLower().Contains("cashier") || RoomName.ToLower().Contains("cath") || RoomName.ToLower().Contains("boardroom") || RoomName.ToLower().Contains("pathologist") || RoomName.ToLower().Contains("chroma") || RoomName.ToLower().Contains("circulation") || RoomName.ToLower().Contains("corridor") || RoomName.ToLower().Contains("clean") || RoomName.ToLower().Contains("clinical") || RoomName.ToLower().Contains("cloak") || RoomName.ToLower().Contains("co2") || RoomName.ToLower().Contains("consult") || RoomName.ToLower().Contains("control room") || RoomName.ToLower().Contains("copy room") || RoomName.ToLower().Contains("corridor") || RoomName.ToLower().Contains("cpr") || RoomName.ToLower().Contains("crash cart alcove") || RoomName.ToLower().Contains("ct changing") || RoomName.ToLower().Contains("ct control") || RoomName.ToLower().Contains("ct equipment") || RoomName.ToLower().Contains("ct scan") || RoomName.ToLower().Contains("ct wc") || RoomName.ToLower().Contains("ct-mri prep/recov") || RoomName.ToLower().Contains("care") || RoomName.ToLower().Contains("decontam") || RoomName.ToLower().Contains("director") || RoomName.ToLower().Contains("wc") || RoomName.ToLower().Contains("toilet") || RoomName.ToLower().Contains("pharmacy") || RoomName.ToLower().Contains("dispensary") || RoomName.ToLower().Contains("disposal") || RoomName.ToLower().Contains("counter") || RoomName.ToLower().Contains("doctor") || RoomName.ToLower().Contains("resident") || RoomName.ToLower().Contains("ecg measurement") || RoomName.ToLower().Contains("echo") || RoomName.ToLower().Contains("em equipment") || RoomName.ToLower().Contains("embedding") || RoomName.ToLower().Contains("examination") || RoomName.ToLower().Contains("stress") || RoomName.ToLower().Contains("extemporaneous") || RoomName.ToLower().Contains("changing room") || RoomName.ToLower().Contains("lockers") || RoomName.ToLower().Contains("showers") || RoomName.ToLower().Contains("fire command center") || RoomName.ToLower().Contains("microscope") || RoomName.ToLower().Contains("office") || RoomName.ToLower().Contains("alcove") || RoomName.ToLower().Contains("future") || RoomName.ToLower().Contains("x-ray") || RoomName.ToLower().Contains("gamma") || RoomName.ToLower().Contains("gross") || RoomName.ToLower().Contains("security") || RoomName.ToLower().Contains("guards") || RoomName.ToLower().Contains("holter monitor room") || RoomName.ToLower().Contains("homeostasis coagulation") || RoomName.ToLower().Contains("hospital information") || RoomName.ToLower().Contains("hot patient") || RoomName.ToLower().Contains("house keeping") || RoomName.ToLower().Contains("housekeeping") || RoomName.ToLower().Contains("hplc") || RoomName.ToLower().Contains("hyperbaric chamber") || RoomName.ToLower().Contains("imaging viewing") || RoomName.ToLower().Contains("diagnostic") || RoomName.ToLower().Contains("reporting area") || RoomName.ToLower().Contains("immun") || RoomName.ToLower().Contains("vaccination") || RoomName.ToLower().Contains("labeling") || RoomName.ToLower().Contains("quarantine") || RoomName.ToLower().Contains("interview") || RoomName.ToLower().Contains("janitor") || RoomName.ToLower().Contains("laminar flow") || RoomName.ToLower().Contains("laser") || RoomName.ToLower().Contains("lift") || RoomName.ToLower().Contains("lobby") || RoomName.ToLower().Contains("switchboard") || RoomName.ToLower().Contains("operator") || RoomName.ToLower().Contains("maintenance workshop") || RoomName.ToLower().Contains("media preparation") || RoomName.ToLower().Contains("medical gas") || RoomName.ToLower().Contains("medication room") || RoomName.ToLower().Contains("microtom") || RoomName.ToLower().Contains("milk expressing room") || RoomName.ToLower().Contains("minor procedure") || RoomName.ToLower().Contains("assistant") || RoomName.ToLower().Contains("mri") || RoomName.ToLower().Contains("nourishment") || RoomName.ToLower().Contains("observation") || RoomName.ToLower().Contains("duty") || RoomName.ToLower().Contains("opd") || RoomName.ToLower().Contains("paitent") || RoomName.ToLower().Contains("peds") || RoomName.ToLower().Contains("pet ct") || RoomName.ToLower().Contains("measure") || RoomName.ToLower().Contains("physiotherapy") || RoomName.ToLower().Contains("point of care") || RoomName.ToLower().Contains("security") || RoomName.ToLower().Contains("porters") || RoomName.ToLower().Contains("post donation beverage") || RoomName.ToLower().Contains("drivers") || RoomName.ToLower().Contains("corridor") || RoomName.ToLower().Contains("pts") || RoomName.ToLower().Contains("toilets") || RoomName.ToLower().Contains("pulmonary function test") || RoomName.ToLower().Contains("purchasing") || RoomName.ToLower().Contains("quality control") || RoomName.ToLower().Contains("radio-diagnostic imaging") || RoomName.ToLower().Contains("radioactive") || RoomName.ToLower().Contains("recording area") || RoomName.ToLower().Contains("reception") || RoomName.ToLower().Contains("reviewing") || RoomName.ToLower().Contains("receiving") || RoomName.ToLower().Contains("records") || RoomName.ToLower().Contains("respiratory") || RoomName.ToLower().Contains("resusciation bay") || RoomName.ToLower().Contains("roof") || RoomName.ToLower().Contains("routine hematology") || RoomName.ToLower().Contains("scope disinfection") || RoomName.ToLower().Contains("scrub") || RoomName.ToLower().Contains("secretarial") || RoomName.ToLower().Contains("secretary") || RoomName.ToLower().Contains("seminar") || RoomName.ToLower().Contains("service counter") || RoomName.ToLower().Contains("firefighting") || RoomName.ToLower().Contains("serviced disinfected equipment") || RoomName.ToLower().Contains("soiled") || RoomName.ToLower().Contains("staff") || RoomName.ToLower().Contains("staining") || RoomName.ToLower().Contains("stair") || RoomName.ToLower().Contains("stationeries") || RoomName.ToLower().Contains("stretcher") || RoomName.ToLower().Contains("support") || RoomName.ToLower().Contains("wash") || RoomName.ToLower().Contains("tea") || RoomName.ToLower().Contains("team") || RoomName.ToLower().Contains("exit") || RoomName.ToLower().Contains("sterilization") || RoomName.ToLower().Contains("terrace") || RoomName.ToLower().Contains("therapist") || RoomName.ToLower().Contains("therapy") || RoomName.ToLower().Contains("tilt table room") || RoomName.ToLower().Contains("tpn") || RoomName.ToLower().Contains("translational space") || RoomName.ToLower().Contains("triage") || RoomName.ToLower().Contains("ultrasound") || RoomName.ToLower().Contains("universal") || RoomName.ToLower().Contains("urine & stool analysis") || RoomName.ToLower().Contains("vending machines") || RoomName.ToLower().Contains("venepuncture") || RoomName.ToLower().Contains("vestibule") || RoomName.ToLower().Contains("visitor") || RoomName.ToLower().Contains("wash") || RoomName.ToLower().Contains("waste") || RoomName.ToLower().Contains("wheelchair") || RoomName.ToLower().Contains("lab") || RoomName.ToLower().Contains("breavement") || RoomName.ToLower().Contains("library") || RoomName.Contains("MR") || RoomName == ("JC") || RoomName == ("J.C") || RoomName == ("J.C.") || RoomName.ToLower().Contains("locker") || RoomName.ToLower().Contains("mail") || RoomName.ToLower().Contains("manager") || RoomName.ToLower().Contains("nurse station") || RoomName.ToLower().Contains("pantry") || RoomName.ToLower().Contains("printer") || RoomName.ToLower().Contains("bath") || RoomName.ToLower().Contains("elevator") || RoomName.ToLower().Contains("call") || RoomName.ToLower().Contains("vend") || RoomName.ToLower().Contains("workshop") || RoomName.ToLower().Contains("workroom") || RoomName.ToLower().Contains("breast feeding") || RoomName.ToLower().Contains("hear") || RoomName.ToLower().Contains("work") || RoomName.ToLower().Contains("gift") || RoomName.ToLower().Contains("patient") || RoomName.ToLower().Contains("imaging") || RoomName.ToLower().Contains("mammography") || RoomName.ToLower().Contains("radiography") || RoomName.ToLower().Contains("fluoroscopy") || RoomName.ToLower().Contains("nuclear") || RoomName.ToLower().Contains("phlebotomy") || RoomName.ToLower().Contains("regist") || RoomName.ToLower().Contains("archive") || RoomName.ToLower().Contains("board") || RoomName.ToLower().Contains("cash") || RoomName.ToLower().Contains("money") || RoomName.ToLower().Contains("count") || RoomName.ToLower().Contains("customer") || RoomName.ToLower().Contains("deputy") || RoomName.ToLower().Contains("chairman") || RoomName.ToLower().Contains("emergency") || RoomName.ToLower().Contains("facility") || RoomName.ToLower().Contains("vistibule") || RoomName.Contains("GM") || RoomName.ToLower().Contains("g.m") || RoomName.ToLower().Contains("g.m.") || RoomName.ToLower().Contains("sec.") || RoomName.ToLower().Contains("secretary") || RoomName.ToLower().Contains("teller") || RoomName.ToLower().Contains("head") || RoomName.ToLower().Contains("paper shred") || RoomName.ToLower().Contains("bank") || RoomName.ToLower().Contains("commander") || RoomName.ToLower().Contains("financial") || RoomName.ToLower().Contains("foyer") || RoomName.ToLower().Contains("invest") || RoomName.ToLower().Contains("hold") || RoomName.ToLower().Contains("passport") || RoomName.ToLower().Contains("immigrat") || RoomName.ToLower().Contains("inspect") || RoomName.ToLower().Contains("operat") || RoomName.ToLower().Contains("detention") || RoomName.ToLower().Contains("respon") || RoomName.ToLower().Contains("customs") || RoomName.ToLower().Contains("shoe"))
            {

                RoomName = "Business";

            }
            else if (RoomName.ToLower().Contains("bedroom") || RoomName.ToLower().Contains("dinning") || RoomName.ToLower().Contains("ensuite") || RoomName.ToLower().Contains("family overnight stay") || RoomName.ToLower().Contains("hallway") || RoomName.ToLower().Contains("kitchenette") || RoomName.ToLower().Contains("living") || RoomName.ToLower().Contains("suite"))
            {

                RoomName = "Residential";

            }
            else if (RoomName.ToLower().Contains("park"))
            {

                RoomName = "Parking";

            }

            else
            {
                RoomName = null;
            }

            

            return RoomName;

        



        }
        public static double OccupantLoad_EGYPT(string calssification)
        {
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


            double x = dict[calssification.ToString()];


            return x;

        }
        public static double OccupantLoad_SAUDI(string calssification)
        {
            Dictionary<string, double> dict = new Dictionary<string, double>()
                {

                {"Accessory storage areas, mechanical equipment room",28},
                {"Agricultural building",28},
                {"Aircraft hangars",46},
                {"Baggage claim",1.9},
                {"Baggage handling",28},
                {"Concourse",9},
                {"Assembly",1.4},
                {"Gaming floors",1.02},
                {"Exhibit Gallery and Museum",2.8},
                {"Prayer Area",0.65},
                {"Bowling centers",0.65},
                {"Business areas",9},
                {"Courtrooms",3.7},
                {"Day care",3.3},
                {"Dormitories",4.6},
                {"Educational",1.9},
                {"Shops",4.6},
                {"Exercise rooms",4.6},
                {"H-5 Fabrication and manufacturing areas",19},
                {"Kitchens",19},
                {"Library",4.6},
                {"Mercantile, Areas on other floors",5.6},
                {"Stack area",9},
                {"Mercantile, Basement and grade floor areas Storage, stock, shipping areas",2.8},
                {"Parking garages",19},
                {"Residential",19},
                {"null",0}

                 };

              double y = dict[calssification.ToString()];


            return y;

        }

        public class egresscapicity
        {

            public static string Stairclass(bool run)
            {
                if (run==true)
                {
                    //Form1 f = new Form1();
                   
                    //if (f.ShowDialog() == DialogResult.OK)
                    //{

                    //}
                    
                    

                    //f.Owner = this;
                }
               
                List<string> x = new List<string>() { "A-1", "A-2", "A-3", "A-4", "I-1", "I-2", "R-1", "R-2", "B", "M-1", "M-2", "M-3", "F-1", "F-2", "F-3" };
               
                selectedItem = Form1.selectedItemm;
                return selectedItem;
            }
            public static double Stair(string egresscapicity)
            {
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


                double x = dict[egresscapicity.ToString()];


                return x;

            }

        }
        
    }

   
}
