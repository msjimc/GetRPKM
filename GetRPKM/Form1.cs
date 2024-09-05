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

            try
            {
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

            }
            catch (Exception ex) { MessageBox.Show("Couldn't process file: " + ex.Message, "Error"); }
            finally
            {
                if (sf != null) { sf.Close(); }
            }

        }
    }
}
