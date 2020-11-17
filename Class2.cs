using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using System.ComponentModel;
using System.Data;

using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

//using System.Windows.Forms;
//using System.Diagnostics;
//using System.Collections;
//using System.Collections;
//using System.Collections.Generic;
using Autodesk.Revit.UI.Selection;
using System.Data.OleDb;
using System.Diagnostics;
using System.Net.Mail;
using System.Globalization;
using System.Threading;

using System.Security.Principal;
using Autodesk.Revit.DB.Analysis;


//using Microsoft.Office.Interop.Excel;
using Autodesk.Revit.DB.Architecture;
using System.Text.RegularExpressions;
using Egyptian_FLS_code;

namespace Egyptian_FLS_code
{


    public class emp : IComparable//<emp>
    {
        public PathOfTravel elements;
        public double lengths;
        //public int intlens;

        public emp()
        {

        }
        public emp(PathOfTravel element, double length)
        {

            elements = element;
            lengths = length;
            //intlens  = (int)length;

        }
        public class sortlength : IComparer
        {


            public int Compare(object x, object y)
            {
                emp comp1 = x as emp;
                emp comp2 = y as emp;
                return comp1.lengths.CompareTo(comp2.lengths);


            }
        }
        public int CompareTo(emp other)
        {
            if (this.lengths < other.lengths)
            {
                return -1;
            }
            if (this.lengths > other.lengths)
            {
                return 1;
            }
            else
            {

                return 0;
            }
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as emp);
        }


        ////show list in task dialog c#
        //var msg = string.Join(Environment.NewLine, item);
        //System.Windows.MessageBox.Show(msg);


        class CategoryComparer : IEqualityComparer<Category>
        {
            #region Implementation of IEqualityComparer<in Category>

            public bool Equals(Category x, Category y)
            {
                if (x == null || y == null) return false;

                return x.Id.Equals(y.Id);
            }

            public int GetHashCode(Category obj)
            {
                return obj.Id.IntegerValue;
            }

            #endregion
        }


    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    public class FiRaa : IExternalCommand
    {
        double travelpaths;
        ICollection<ElementId> selectedIds;

        XYZ q = new XYZ();
        List<XYZ> p = new List<XYZ>();
        List<Room> rms = null;
        // List<Room> selectedrms = null;
        List<IList<PathOfTravel>> routelist = new List<IList<PathOfTravel>>();
        public static int run = 0;
        public static Transaction tx;
        //public static Transaction documentTransaction;
        public static List<int> cnt = new List<int>();
        public static IList<IList<BoundarySegment>> cc = null;
        public static List<List<BoundarySegment>> flattensegment = new List<List<BoundarySegment>>();
        public static Transaction documentTransaction;

        public static List<XYZ> roomsXyz1 = new List<XYZ>();
        public static List<XYZ> ptsend = new List<XYZ>();
        public static List<List<XYZ>> ptclr = new List<List<XYZ>>();
        public static List<XYZ> ptclrr = new List<XYZ>();
        public static List<XYZ> ptt = new List<XYZ>();
        public static IList<PathOfTravel> routeMap = null;
        List<IList<PathOfTravel>> routelast = null;
        //List<IList<PathOfTravel>> routelast2 = null;
        public static IList<XYZ> closestroute = null;
        public static IList<PathOfTravel> closestrouteMap = null;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Result result = 0;
            try
            {


                DateTime thisDay = DateTime.Today;



                //if ((WindowsIdentity.GetCurrent().Name.ToUpper().Contains("DAR")) ^ (WindowsIdentity.GetCurrent().Name.ToUpper().Contains("DMSD")))
                //{
                UIDocument uiDocument = commandData.Application.ActiveUIDocument;
                Document doc = commandData.Application.ActiveUIDocument.Document;
                var version = uiDocument.Application.Application.VersionNumber;



                //Create a transaction

                using (Transaction documentTransaction = new Transaction(doc, "FLS"))
                {

                    documentTransaction.Start();


                    // Create a form to display the information of rooms

                    Egyptian_FLS_code.Form1 types = new Egyptian_FLS_code.Form1(doc, commandData, documentTransaction);

                    travelpaths = Form1.travelpath;

                    // Category category = Category.GetCategory(doc, BuiltInCategory.OST_Rooms);

                    //doc.FamilyManager.AddParameter("Building Type", BuiltInParameterGroup.PG_IDENTITY_DATA, category, true);
                    //doc.ParameterBindings.Insert()

                    documentTransaction.Commit();

                }





                if (Form1.travel == true)
                {
                    using (Transaction txr = new Transaction(doc, "new Travel Path"))
                    {
                        txr.Start();

                        //    //FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
                        //    //var categories =collector.ToElements().Select(x => x.Category).Distinct(new CategoryComparer()).ToList();

                        //    FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
                        //    IList<Element> elementsInView = collector.ToElements();
                        //    List<ElementId> allproject_ENTIRE = collector.ToElementIds().ToList<ElementId>();

                        //List<Element> zz = new List<Element>();

                        //foreach (var item in allproject_ENTIRE)
                        //{
                        //    Element yy = doc.GetElement(item);
                        //    zz.Add(yy);

                        //}

                        //int a = 0;
                        //foreach (Element item in zz)
                        //{


                        //        if (item.Category.Name== "Rooms")
                        //        {
                        //        a++;
                        //           // rms.Add(item as Room);
                        //        }
                        //    else
                        //    {
                        //        continue;
                        //    }

                        //}


                        selectedIds = uiDocument.Selection.GetElementIds();

                        try
                        {
                            try
                            {
                                q = uiDocument.Selection.PickPoint("Point 1");

                            }
                            catch (Exception)
                            {


                            }






                            p.Add(q);


                            //if (Form1.selrooms==true)
                            //{

                            //    foreach (ElementId item in selectedIds)
                            //    {
                            //        Element rr = doc.GetElement(item);


                            //            rms.Add(rr as Room);




                            //    }
                            //}
                            //else
                            //{
                            //    rms = new FilteredElementCollector(doc, doc.ActiveView.Id).WhereElementIsNotElementType().OfClass(typeof(SpatialElement)).Where(ed => ed.GetType() == typeof(Room)).Cast<Room>().ToList();
                            //}

                            if (rms == null)
                            {
                                rms = new FilteredElementCollector(doc, doc.ActiveView.Id).WhereElementIsNotElementType().OfClass(typeof(SpatialElement)).Where(ed => ed.GetType() == typeof(Room)).Cast<Room>().ToList();

                            }



                            foreach (Room rm in rms)
                            {

                                //Debugger.Launch();

                                SpatialElementBoundaryOptions vvv = new SpatialElementBoundaryOptions();
                                BoundingBoxXYZ ss = rm.get_BoundingBox(uiDocument.ActiveView);

                                cc = rm.GetBoundarySegments(vvv);

                                //List<List<Curve>> crvs = new LList<List<Curve>>();
                                //foreach (var item in cc)
                                //{
                                //    List< Curve > crv = new List<Curve>();
                                //    foreach (var i in item)
                                //    {
                                //        Curve cr = i.GetCurve();
                                //        crv.Add(cr);
                                //    }

                                //    crvs.Add(crv.Tessellate());
                                //}


                                List<BoundarySegment> peri = new List<BoundarySegment>();
                                foreach (var item in cc)
                                {


                                    //for Room Sides count
                                    foreach (BoundarySegment loop in item)
                                    {
                                        peri.Add(loop);

                                    }
                                    //if (item.Count() > 3)
                                    //{


                                    //     //vv.Tessellate();
                                    //}


                                }


                                flattensegment.Add(peri);



                                //Debugger.Launch();

                                XYZ mi = ss.Min;
                                // roomsXyz1.Add(mi);

                                Location x = rm.Location;
                                LocationPoint v = x as LocationPoint;
                                XYZ pp = v.Point;


                                //else
                                //{
                                //    continue;
                                //}
                                roomsXyz1.Add(pp);
                            }


                            foreach (var item in flattensegment)
                            {
                                List<XYZ> ptsend = new List<XYZ>();
                                List<Curve> crv = new List<Curve>();
                                foreach (var i in item)
                                {

                                    Curve xd = i.GetCurve();
                                    //doc.Create.NewModelCurve(xd, uiDocument.ActiveView.SketchPlane);
                                    XYZ gg = xd.Evaluate(0, true);
                                    ptsend.Add(gg);

                                }



                                ptclr.Add(ptsend);

                            }


                            cc.Count();
                            //Debugger.Launch();

                            foreach (var ii in ptclr)
                            {
                                cnt.Add(ii.ToArray().Count());
                                //ptt.Add(ii[3]);

                            }


                            //Debugger.Launch();





                            foreach (var ii in ptclr)
                            {

                                foreach (var n in ii)
                                {
                                    ptclrr.Add(n);
                                }

                            }

                            //Debugger.Launch();




                        }
                        catch (Exception e)
                        {

                            System.Windows.MessageBox.Show(e.Message + e.StackTrace);
                        }

                        try
                        {
                            foreach (var item in ptclr)
                            {

                                routeMap = PathOfTravel.CreateMapped(uiDocument.ActiveView, item, p);
                                routelist.Add(routeMap);
                            }

                        }
                        catch (Exception)
                        {


                        }

                        // Room centroid
                        //  routeMap = PathOfTravel.CreateMapped(uiDocument.ActiveView, roomsXyz1, p);


                        //longest path
                        closestroute = PathOfTravel.FindStartsOfLongestPathsFromRooms(uiDocument.ActiveView, p);
                        closestrouteMap = PathOfTravel.CreateMapped(uiDocument.ActiveView, closestroute, p);
                        // PathOfTravel.FindEndsOfShortestPaths()
                        //Debugger.Launch();



                        List<IList<PathOfTravel>> routeMap3 = new List<IList<PathOfTravel>>();

                        Dictionary<PathOfTravel, double> dictpath = new Dictionary<PathOfTravel, double>();
                        List<Dictionary<PathOfTravel, double>> dics = new List<Dictionary<PathOfTravel, double>>();





                        List<List<emp>> ells = new List<List<emp>>();
                        List<IList<PathOfTravel>> routeMapx = new List<IList<PathOfTravel>>();
                        List<IList<PathOfTravel>> routeMap3x = new List<IList<PathOfTravel>>();
                        List<List<double>> lnss = new List<List<double>>();
                        //Debugger.Launch();


                        List<double> lns = new List<double>();

                        try
                        {
                            foreach (var route in routelist)
                            {
                                List<emp> ell = new List<emp>();
                                foreach (var newroute in route)
                                {
                                    emp em = new emp();

                                    if (newroute != null)
                                    {
                                        Autodesk.Revit.DB.Parameter plengths = newroute.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                                        double lenghts = plengths.AsDouble();
                                        //lns.Add(lenghts);
                                        //dictpath[newroute] = lenghts;

                                        em.elements = newroute;
                                        em.lengths = lenghts;

                                        ell.Add(em);

                                    }
                                    else
                                    {
                                        em.elements = newroute;
                                        em.lengths = 0;

                                        ell.Add(em);
                                    }


                                }
                                ells.Add(ell);
                            }

                        }
                        catch (Exception)
                        {


                        }




                        Array lastsal = null;

                        List<Array> arrlast = new List<Array>();
                        Array arr = null;
                        lastsal = ells.ToArray();


                        foreach (var item in ells)
                        {
                            emp.sortlength sr = new emp.sortlength();
                            arr = item.ToArray();
                            Array.Sort(arr, sr);
                            arrlast.Add(arr);
                        }


                        // Debugger.Launch();




                        routelast = new List<IList<PathOfTravel>>();


                        foreach (Array i in arrlast)
                        {
                            List<PathOfTravel> routeMap2x = new List<PathOfTravel>();
                            foreach (emp item in i)
                            {
                                routeMap2x.Add(item.elements);
                            }
                            routelast.Add(routeMap2x);
                        }




                        txr.Commit();




                    }








                    using (tx = new Transaction(doc, "Travel Path Length"))
                    {
                        double ll;
                        tx.Start();

                        foreach (var route in routelist)
                        {


                            foreach (PathOfTravel item in route)
                            {
                                try
                                {
                                    Autodesk.Revit.DB.Parameter plength = item.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                                    ll = plength.AsDouble();
                                    double tr = ll * 304.8;
                                    if (travelpaths != 0)
                                    {
                                        if (tr > travelpaths)
                                        {
                                            var ogs = new OverrideGraphicSettings();
                                            Color colorRed = new Color(255, 0, 0);
                                            OverrideGraphicSettings overrides = uiDocument.ActiveView.GetElementOverrides(item.Id);
                                            overrides.SetProjectionLineColor(colorRed);
                                            uiDocument.ActiveView.SetElementOverrides(item.Id, overrides);
                                        }
                                    }


                                }
                                catch (Exception)
                                {

                                    //System.Windows.MessageBox.Show(exx.Message + exx.StackTrace);
                                }
                                if (Form1.tag == true)
                                {


                                    try
                                    {
                                        TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
                                        TagOrientation tagorn = TagOrientation.Horizontal;


                                        XYZ Mid = item.PathStart;

                                        Reference pathRef = new Reference(item);

                                        IndependentTag newTag = IndependentTag.Create(doc, uiDocument.ActiveView.Id, pathRef, true, tagMode, tagorn, Mid);
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }

                            }
                        }

                        //override
                        try
                        {
                            //Element longpath = closestrouteMap as Element;
                            var ogss = new OverrideGraphicSettings();
                            Color colorblue = new Color(0, 0, 255);
                            OverrideGraphicSettings overrides = uiDocument.ActiveView.GetElementOverrides(closestrouteMap[0].Id);
                            overrides.SetProjectionLineColor(colorblue);
                            uiDocument.ActiveView.SetElementOverrides(closestrouteMap[0].Id, overrides);
                        }
                        catch (Exception)
                        {


                        }

                        tx.Commit();
                        run++;
                    }





                    // Egyptian_FLS_code.Form1 nform = new Egyptian_FLS_code.Form1(doc, commandData, documentTransaction);



                    using (Transaction dl = new Transaction(doc, "delete Travel Path"))
                    {
                        dl.Start();

                        foreach (var item in routelast)
                        {

                            int ic = 0;

                            foreach (PathOfTravel ip in item)
                            {

                                if (ic < (item.Count() - 1))
                                {
                                    try
                                    {
                                        //doc.Regenerate();
                                        doc.Delete(ip.Id);
                                    }
                                    catch (Exception)
                                    {


                                    }



                                }
                                else
                                {
                                    continue;
                                }

                                ic++;

                            }




                        }


                        dl.Commit();

                    }





                    rms.Clear();



                    return Autodesk.Revit.UI.Result.Succeeded;




                }

                else
                {

                }
            }





            catch (Exception)
            {

            }



            return Result.Succeeded;


            

        }



    }
}







