using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetRPKM
{
    public partial class Form1 : Form
    {

        Dictionary<string, gene> genes = new Dictionary<string, gene>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string fileName = FileString.OpenAs("Select the read count file with gene SYMBOLs", "Tab-delimited text file (*.txt;*.xls)|*.txt;*.xls");
            if (System.IO.File.Exists(fileName) == false) { return; }

            System.IO.StreamReader sf = null;
            System.IO.StreamWriter sw = null;

            try
            {
                genes = new Dictionary<string, gene>();

                sf = new System.IO.StreamReader(fileName);
                string line = "";
                string[] items = null;
                string titles = sf.ReadLine();
                while (sf.Peek()>0)
                {
                    line=sf.ReadLine();
                    items = line.Split('\t');
                    string key = items[items.Length-1];

                    if (genes.ContainsKey(key) == true)
                    {
                        genes[key].addTranscript(items);
                    }
                    else
                    {
                        gene newgene = new gene(items);
                        genes.Add(key, newgene);
                    }
                }

                string export = fileName.Substring(0, fileName.LastIndexOf('\\')) + "\\RPKM.xls";
                sw=new System.IO.StreamWriter(export);
                sw.Write("Name");
                List<double> totals = new List<double>();
                int length = items.Length - 9;

                items = titles.Split('\t');
                for (int index = 0; index < length; index++)
                { 
                    totals.Add(0.0d); 
                    sw.Write("\t" + items[index + 7]);
                }
                sw.Write('\n');

                foreach (gene g in genes.Values)
                { totals = g.AddCounts(totals); }

                for (int index = 0; index < length; index++)
                { totals[index] /= 1000000; }


                List<string> keys = new List<string>();
                foreach (string key in genes.Keys)
                { keys.Add(key); }

                keys.Sort();

                foreach (string key in keys)
                { genes[key].writeline(sw, totals); }

            }
            catch (Exception ex) { MessageBox.Show("Couldn't process file: " + ex.Message, "Error"); }
            finally
            {
                if (sf != null) { sf.Close(); }
                if (sw != null) { sw.Close(); }
            }

        }
    }
}
