using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Events;
using Form = System.Windows.Forms.Form;
using View = Autodesk.Revit.DB.View;
using System.Windows.Controls;
namespace Egyptian_FLS_code
{
    public partial class Form4 : Form
    {
        Document doc3;
        UIDocument uidoc;
        ExternalCommandData com3;

        public Form4(Document doc, ExternalCommandData com, Transaction t3)
        {
            InitializeComponent();
            doc3 = doc;
            com3 = com;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(this.textBox1, "Insert Building Height");
            //System.Windows.MessageBoxImage.Information.ToString();
            this.textBox3.Text = ((Egyptian_FLS_code.Form1.roomareas)/10.7639).ToString();


        }

        private void label8_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, @"C:\Users\malbassel\Desktop\فيلا 30\sticker.pdf", HelpNavigator.TableOfContents, "");


            //string path = Egyptian_FLS_code.Properties.Resources.sticker.ToString();
            //Debugger.Launch();
            //Help.ShowHelp(this, path, HelpNavigator.TableOfContents, "");

            // System.IO.File.WriteAllBytes("hello.pdf", Egyptian_FLS_code.Properties.Resources.sticker.ToArray());
            //Help.ShowHelp(this, "hello.pdf", HelpNavigator.TableOfContents, "");

            try
            {
                var bt = doc3.ProjectInformation.LookupParameter("Building Type");

       
            List<object> pi = new List<object>();

            //doc3.ProjectInformation.get_Parameter("Building Type");
          
            //foreach (var item in (doc3.ProjectInformation.Parameters))
            //{

            //    pi.Add(item);
            //}
            //var ob=pi[0].GetType();
            //string na = ob.Name;
            //Debugger.Launch();
            if (checkBox1.Checked==true)
            {

                //A1
                if (comboBox1.Text == ("A-1"))
                {
                    if (((int.Parse(textBox1.Text) > 54)) || ((int.Parse(textBox1.Text) <= 54) && (double.Parse(textBox1.Text) > 25.5)) || (double.Parse(textBox3.Text) > 4320))
                       
                    {
                        if (double.Parse(textBox2.Text) > 6)
                        {
                            bt.Set("Type1-A");
                        }
                        else if ((int.Parse(textBox2.Text) == 6) || (int.Parse(textBox2.Text) == 5))
                        {
                            bt.Set("Type1-B");
                        }

                    }

                  
                    
                    else if (((double.Parse(textBox1.Text) <= 25.5) && (double.Parse(textBox1.Text) > 22.5)))
                    {
                        if ((int.Parse(textBox2.Text) == 4))
                        {
                            bt.Set("Type2-A");



                        }
                       

                    }
                    else if (((double.Parse(textBox1.Text) < 22.5)) || (double.Parse(textBox3.Text) < 2369))
                    {
                        if ((int.Parse(textBox2.Text) <= 3))
                        {
                            bt.Set("Type2-B");
                        }

                    }

                }



                //A2

                else if ((comboBox1.Text == ("A-2")) && ((int.Parse(textBox1.Text) > 54)) && (double.Parse(textBox3.Text) > 4320))
                {
                    if (double.Parse(textBox2.Text) > 12)
                    {
                        bt.Set("Type1-A");
                    }


                }
                else if ((comboBox1.Text == ("A-2")) && ((int.Parse(textBox1.Text) <= 54) && (double.Parse(textBox1.Text) > 25.5)) && (double.Parse(textBox3.Text) > 4320))
                {
                    if ((int.Parse(textBox2.Text) <=12 ) && (int.Parse(textBox2.Text) > 4))
                    {
                        bt.Set("Type1-B");
                    }

                }

                else if ((comboBox1.Text == ("A-2")) && ((double.Parse(textBox1.Text) <= 25.5) && (double.Parse(textBox1.Text) > 22.5)) && ((double.Parse(textBox3.Text) <= 4320) && (double.Parse(textBox3.Text) > 2369)))
                {
                    if ((int.Parse(textBox2.Text) == 4))
                    {
                        bt.Set("Type2-A");
                    }

                }

                else if ((comboBox1.Text == ("A-2")) && ((double.Parse(textBox1.Text) < 22.5)) && (double.Parse(textBox3.Text) < 2369))
                {
                    if ((int.Parse(textBox2.Text) <= 3))
                    {
                        bt.Set("Type2-B");
                    }

                }



            }
            else if(checkBox1.Checked == false)
            {
                //A1

                if ((comboBox1.Text == ("A-1")) && ((int.Parse(textBox1.Text) > 48)) && (double.Parse(textBox3.Text) > 1440))
                {
                    if (double.Parse(textBox2.Text) > 5)
                    {
                        bt.Set("Type1-A");
                    }


                }
                else if ((comboBox1.Text == ("A-1")) && ((int.Parse(textBox1.Text) <= 48) && (double.Parse(textBox1.Text) > 19.5)) && (double.Parse(textBox3.Text) > 1440))
                {
                    if ((int.Parse(textBox2.Text) == 5) || (int.Parse(textBox2.Text) == 4))
                    {
                        bt.Set("Type1-B");
                    }

                }

                else if ((comboBox1.Text == ("A-1")) && ((double.Parse(textBox1.Text) <= 25.5) && (double.Parse(textBox1.Text) > 22.5)) && ((double.Parse(textBox3.Text) <= 1440) && (double.Parse(textBox3.Text) > 790)))
                {
                    if ((int.Parse(textBox2.Text) == 4))
                    {
                        bt.Set("Type2-A");
                    }

                }

                else if ((comboBox1.Text == ("A-1")) && ((double.Parse(textBox1.Text) < 22.5)) && (double.Parse(textBox3.Text) < 790))
                {
                    if ((int.Parse(textBox2.Text) <= 3))
                    {
                        bt.Set("Type2-B");
                    }

                }

                //A2

                else if ((comboBox1.Text == ("A-2")) && ((int.Parse(textBox1.Text) > 54)) && (double.Parse(textBox3.Text) > 1440))
                {
                    if (double.Parse(textBox2.Text) > 11)
                    {
                        bt.Set("Type1-A");
                    }


                }
                else if ((comboBox1.Text == ("A-2")) && ((int.Parse(textBox1.Text) <= 54) && (double.Parse(textBox1.Text) > 25.5)) && (double.Parse(textBox3.Text) > 1440))
                {
                    if ((int.Parse(textBox2.Text) <= 11) && (int.Parse(textBox2.Text) > 3))
                    {
                        bt.Set("Type1-B");
                    }

                }

                else if ((comboBox1.Text == ("A-2")) && ((double.Parse(textBox1.Text) <= 25.5) && (double.Parse(textBox1.Text) > 22.5)) && ((double.Parse(textBox3.Text) <= 1440) && (double.Parse(textBox3.Text) > 883)))
                {
                    if ((int.Parse(textBox2.Text) == 3))
                    {
                        bt.Set("Type2-A");
                    }

                }

                else if ((comboBox1.Text == ("A-2")) && ((double.Parse(textBox1.Text) < 22.5)) && (double.Parse(textBox3.Text) < 883))
                {
                    if ((int.Parse(textBox2.Text) <= 2))
                    {
                        bt.Set("Type2-B");
                    }

                }



            }


            MessageBox.Show(bt.AsString());
            }
            catch (Exception)
            {

                MessageBox.Show("add parameter: 'Building Type'");
               
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"D:\DAR-BIM STANDARD\ماجستير\BIM masters\Fire\Saudi Building Code-General-SBC_Code_201.pdf");
            }
            catch (Exception)
            {

                MessageBox.Show("File is not Exist");
            }
            
        }
    }
}
