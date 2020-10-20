using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Net.Mail;
using Microsoft.Office.Interop.Word;
using System.Windows.Controls;


namespace Egyptian_FLS_code
{
    public partial class Form3 : Form
    {


        public static List<string> exp2 = Egyptian_FLS_code.Form1.exp;

        public static Microsoft.Office.Interop.Excel.Application excel;
        Microsoft.Office.Interop.Excel.Workbook worKbooK;
        Microsoft.Office.Interop.Excel.Worksheet worksheet;
        Microsoft.Office.Interop.Excel.Range celLrangE;

        string msg;
        public Form3()
        {
            InitializeComponent();
            excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = false;
            excel.DisplayAlerts = false;
            worKbooK = excel.Workbooks.Add(Type.Missing);

            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)worKbooK.ActiveSheet;

            //using (ExcelEngine excelEngine = new ExcelEngine())
            //{
            //    IApplication application = excelEngine.Excel;
            //    application.DefaultVersion = ExcelVersion.Excel2016;

            //    //Reads input Excel stream as a workbook
            //    IWorkbook workbook = application.Workbooks.Open(File.OpenRead(Path.GetFullPath(@"../../../Expenses.xlsx")));
            //    IWorksheet sheet = workbook.Worksheets[0];

            //    //Preparing first array with different data types
            //    object[] expenseArray = new object[14]
            //    {"Paul Pogba", 469.00d, 263.00d, 131.00d, 139.00d, 474.00d, 253.00d, 467.00d, 142.00d, 417.00d, 324.00d, 328.00d, 497.00d, "=SUM(B11:M11)"};

            //    //Inserting a new row by formatting as a previous row.
            //    sheet.InsertRow(11, 1, ExcelInsertOptions.FormatAsBefore);

            //    //Import Peter's expenses and fill it horizontally
            //    sheet.ImportArray(expenseArray, 11, 1, false);

            //    //Preparing second array with double data type
            //    double[] expensesOnDec = new double[6]
            //    {179.00d, 298.00d, 484.00d, 145.00d, 20.00d, 497.00d};

            //    //Modify the December month's expenses and import it vertically
            //    sheet.ImportArray(expensesOnDec, 6, 13, true);

            //    //Save the file in the given path
            //    Stream excelStream = File.Create(Path.GetFullPath(@"Output.xlsx"));
            //    workbook.SaveAs(excelStream);
            //    excelStream.Dispose();
            //}
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ExportToExcel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            //int i = 0;
            //foreach (string item in exp2)
            //{


            //    worksheet.Cells[i,0] = item;
            //    i++;
            //}
        
            //worKbooK.SaveAs(@"C:\Users\malbassel\Desktop\New folder\exp.xls"); ;
            //worKbooK.Close();
            //excel.Quit();
            //this.Close();


            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif |text file|*.txt";
            saveFileDialog1.Filter = "text file|*.txt";
            saveFileDialog1.ShowDialog();
            string path;
            path = saveFileDialog1.FileName;


            using (FileStream f = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                //bf.Serialize(f,bf );
                using (StreamWriter st = new StreamWriter(f))
                {

                    foreach (string item in exp2)
                    {
                        st.WriteLine(item+ "    "+Spelling.Correct(item));
                        
                       //st.WriteLine(Spelling.Correct(item));
                    }

                }

            }

            //try
            //{
            //    MailMessage mail = new MailMessage();
            //    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            //    mail.From = new MailAddress("moh.albassel@gmail.com");
            //    mail.To.Add("mohamed.albassel@dar.com");
            //    mail.Subject = "FLS";
                
            //    foreach (string item in exp2)
            //    {
            //         msg = string.Join(Environment.NewLine, item);
            //        //System.Windows.MessageBox.Show(msg);

            //    }
            //    mail.Body = msg;
            //   SmtpServer.Port = 8080;
            //    SmtpServer.Credentials = new System.Net.NetworkCredential("username", "password");
            //    SmtpServer.EnableSsl = true;

            //    SmtpServer.Send(mail);
            //    MessageBox.Show("mail Send");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}




            this.Close();
        }
        public System.Data.DataTable ExportToExcel()
        {
           

            System.Data.DataTable table = new System.Data.DataTable();
            //table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Room Name", typeof(string));
            table.Columns.Add("Correct Name", typeof(string));
         

            foreach (var item in exp2)
            {
               // Egyptian_FLS_code.Spelling spelling = new Egyptian_FLS_code.Spelling();
                object []o = { item, Spelling.Correct(item) };
                table.Rows.Add(o);
               // table.Rows.Add(Egyptian_FLS_code.Spelling.Correct(item));

            }
           

            return table;
        }
    }
}
