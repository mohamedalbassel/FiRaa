#region Namespaces
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace Egyptian_FLS_code
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {

            a.CreateRibbonTab("FLS");
            a.CreateRibbonPanel("FLS", "Fire & Life Safety");
            RibbonPanel p = a.GetRibbonPanels("FLS")[0];
            string thisAssemblyPath1 = Assembly.GetExecutingAssembly().Location;
            PushButtonData pbdata1 = new PushButtonData("Fire & Life Safety", "Fire", thisAssemblyPath1, "Egyptian_FLS_code.CMDFLS");

            PushButton firebutton = p.AddItem(pbdata1) as PushButton;
            BitmapImage pbimg1 = new BitmapImage(new Uri("pack://application:,,,/Egyptian FLS code;component/Resources/applogo.png"));
            //button.LargeImage = largeImage;
            //button.LargeImage = imgSrc;
            firebutton.LargeImage = pbimg1;
            firebutton.Visible = true;

            a.ApplicationClosing += a_ApplicationClosing;

            //Set Application to Idling
            a.Idling += a_Idling;
            firebutton.Enabled = true;


            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
        void a_ApplicationClosing(object sender, Autodesk.Revit.UI.Events.ApplicationClosingEventArgs e)
        {
            throw new NotImplementedException();
        }
        void a_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {

        }

    }
}
