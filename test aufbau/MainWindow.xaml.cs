using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;



namespace test_aufbau
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void auswählen(object sender, RoutedEventArgs e)
        {
            //beim aufruf des buttons werden im Drop Down alle Baugruppen der Jeweiligen Auftagsnummer angezeigt 
            Fehlerabfrage.fehlerabfrage_autragsnummer(Aufgragsnummer.Text);


            //InitializeComponent();
            using (SqlConnection conn = new SqlConnection(@"server=vmsql01\prod;database=schnupp; trusted_connection=yes"))
            {
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("Select Count(*)  from tbl_Stücklistenf where Auftragsnummer Like '" + Aufgragsnummer.Text + "'", conn);
                int anzahl = (Int32)cmd1.ExecuteScalar();

                if (anzahl < 1)
                {

                    MessageBox.Show("Es wurde keine Auftragsnummer augewählt oder eine Fehlerhafte Auftragsnummer eingetragen");

                }
                else
                {
                    SqlCommand cmd = new SqlCommand("Select Distinct Baugruppe  from tbl_Stücklistenf where Auftragsnummer Like '" + Aufgragsnummer.Text + "'", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Baugruppe.Items.Add(reader[0].ToString());

                    }
                    reader.Close();

                }

            }
        }
        /// <summary>
        /// KEY ENTER                  https://stackoverflow.com/questions/19975617/press-enter-in-textbox-to-and-execute-button-command
        ///
        private void auswählen_baugruppe(object sender, RoutedEventArgs e)
        {

            if (Baugruppe.SelectedItem == null)
            {
                MessageBox.Show("Es wurde keine Baugruppe ausgewählt");
            }
            else
            {
                using (SqlConnection conn1 = new SqlConnection(@"server=vmsql01\prod;database=schnupp; trusted_connection=yes"))
                {
                    SqlCommand cmd1 = new SqlCommand("Select Count(*)  from tbl_Stücklistenf where  Baugruppe Like '" + Baugruppe.SelectedItem + "' AND Auftragsnummer like '" + Aufgragsnummer.Text + "'", conn1);
                    conn1.Open();
                    int anzahl = (Int32)cmd1.ExecuteScalar();


                    SqlCommand cmd2 = new SqlCommand("Select Positionsnr , Positionsnr2,   Positionsnr3, benennung, RohmaßBestellbezeichnung , BemerkungFirma , TeilGekommenAm , StückGezeichnet from tbl_Stücklistenf where Baugruppe Like '" + Baugruppe.SelectedItem + "' AND Auftragsnummer like '" + Aufgragsnummer.Text + "'", conn1);
                    // bemerkungen Montage fehlt !!

                    SqlDataReader reader2 = cmd2.ExecuteReader();

                    string[] Positionsnr = new string[anzahl];
                    string[] Positionsnr2 = new string[anzahl];
                    string[] Positionsnr3 = new string[anzahl];
                    string[] benennung = new string[anzahl];
                    string[] RohmaßBestellbezeichnung = new string[anzahl];
                    string[] BemerkungFirma = new string[anzahl];
                    string[] TeilDa = new string[anzahl];
                    string[] StückGezeichnet = new string[anzahl];
                    //string[] Montagebemerkungen = new string[anzahl];
                    for (int i = 0; reader2.Read(); i++)
                    {
                        Positionsnr[i] = reader2[0].ToString();
                        Positionsnr2[i] = reader2[1].ToString();
                        Positionsnr3[i] = reader2[2].ToString();
                        benennung[i] = reader2[3].ToString();
                        RohmaßBestellbezeichnung[i] = reader2[4].ToString();
                        BemerkungFirma[i] = reader2[5].ToString();
                        TeilDa[i] = reader2[6].ToString();
                        StückGezeichnet[i] = reader2[7].ToString();
                        // Montagebemerkungen[i] = reader2[4].ToString();
                    }

                    reader2.Close();
                    //MessageBox.Show("ERster");








                    Microsoft.Office.Interop.Excel.Application xlApp;
                 
                    Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
               

                    Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
               
                    object misValue = System.Reflection.Missing.Value;

                    Microsoft.Office.Interop.Excel.Range chartRange;
                


                    xlApp = new Microsoft.Office.Interop.Excel.Application();
               
                    xlWorkBook = xlApp.Workbooks.Add(misValue);
                  
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);









                    //Microsoft.Office.Interop.Excel.Application xlApp;
                    //Workbook xlWorkBook;
                    //Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;

                    //object misValue = System.Reflection.Missing.Value;
                    //Microsoft.Office.Interop.Excel.Range chartRange;






                    //xlApp = new Microsoft.Office.Interop.Excel.Application();
                    //xlWorkBook = xlApp.Workbooks.Add(misValue);
                    //xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);







                    //string[] NachVor = new string[anzahl];
                    //for (int i = 0; i < anzahl; i++)
                    //{
                    //    NachVor[i] = nachname[i] + " " + vorname[i];
                    //}
                    MessageBox.Show("nach");
                    try
                    {
                        //fehler
                        xlWorkSheet.Cells[1, 1] = "Pos 1: ";
                        xlWorkSheet.Cells[1, 2] = "Pos 2:";
                        xlWorkSheet.Cells[1, 3] = "Pos 2:";
                        xlWorkSheet.Cells[1, 4] = "Benennung";
                        xlWorkSheet.Cells[1, 5] = "Rohmaß Bestellbezeichnung";
                        xlWorkSheet.Cells[1, 6] = "Firma ";
                        xlWorkSheet.Cells[1, 7] = "Teil da";
                        xlWorkSheet.Cells[1, 8] = "Stück Gesamt";
                        xlWorkSheet.Cells[1, 9] = "Montagebemerkungen ";

                        for (int i = 0; i < anzahl; i++)
                        {
                            xlWorkSheet.Cells[i + 2, 1] = Positionsnr[i];
                            xlWorkSheet.Cells[i + 2, 2] = Positionsnr2[i];
                            xlWorkSheet.Cells[i + 2, 3] = Positionsnr3[i];
                            xlWorkSheet.Cells[i + 2, 5] = benennung[i];
                            xlWorkSheet.Cells[i + 2, 4] = RohmaßBestellbezeichnung[i];
                            xlWorkSheet.Cells[i + 2, 6] = BemerkungFirma[i];
                            xlWorkSheet.Cells[i + 2, 7] = TeilDa[i];
                            xlWorkSheet.Cells[i + 2, 8] = StückGezeichnet[i];
                            // xlWorkSheet.Cells[i + 2, 7] = Montagebemerkungen[i];

                        }
                        anzahl++;
                        string anzahl_string = Convert.ToString(anzahl);
                        string test = "i" + anzahl_string;
                        chartRange = xlWorkSheet.get_Range("a1", "j1");
                        chartRange = xlWorkSheet.get_Range("a1", "j1");
                        chartRange.Font.Bold = true;
                        int rand = anzahl + 1;
                        //chartRange = xlWorkSheet.get_Range("a1", "g" + (anzahl + 1));
                        chartRange = xlWorkSheet.get_Range("a1", test);
                        chartRange.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic);
                        foreach (Microsoft.Office.Interop.Excel.Range cell in chartRange.Rows[1].Cells)
                        {
                            cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                            cell.Font.Bold = true;
                        }
                        chartRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        xlApp.DisplayAlerts = false;
                        //excel spalten zentieren
                        xlWorkSheet.Columns["A:j"].AutoFit();

                        string auftrag = Aufgragsnummer.Text;
                        //string gruppe = Baugruppe.SelectedItem.;
                        string bau_gruppe = Convert.ToString(Baugruppe.SelectedItem);

                        //   string speicherort = @"M:\Kollegen\Auftragsnummer\" +auftrag+@"_"+bau_gruppe+@".xls"; 
                        string username = Environment.UserName;
                        // string speicherort = @"C:\Users\c.fitz\OneDrive - Schnupp GmbH & Co.Hydraulik KG\Microsoft Teams-Chatdateien\Baugruppe\" + auftrag + @"_" + bau_gruppe + @".xls";
                        string speicherort = @"C:\Users\" + username + @"\Desktop\" + auftrag + @"_" + bau_gruppe + @".xls";


                        xlWorkBook.SaveAs(speicherort, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                        // xlWorkBook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, @"M:\Kollegen\Telefonlisten\TelefonSchmal_carina.pdf");
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();
                        releaseObject(xlApp);
                        releaseObject(xlWorkBook);
                        releaseObject(xlWorkSheet); var p = new System.Diagnostics.Process();

                        //"M:\Kollegen\Auftragsnummer\Auftragsnummer_Baugruppe.xls
                        p.StartInfo = new ProcessStartInfo(speicherort)
                        {
                            UseShellExecute = true
                        };
                        p.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Die Excel Datei ist gerade noch geöffnet, bitte probieren sie es zu einem Spätern Zeitpunkt erneut oder schließen Sie die geöffnete Excel Datei");
                    }

                }
                void releaseObject(object obj)
                {
                    try
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                        obj = null;
                    }
                    catch (Exception ex)
                    {
                        obj = null;
                    }
                    finally
                    {
                        GC.Collect();
                        Aufgragsnummer.Text = "";
                        Baugruppe.Items.Clear();
                    }
                }

            }
        }

    }
}
