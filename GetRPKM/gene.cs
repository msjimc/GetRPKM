using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace GetRPKM
{   
    internal class gene
    {
        List<double> counts = new List<double>();
        List<exon> exons = new List<exon>();
        string name = "";
        int originalLength = -1;

        public gene(string[] data)
        {
            name = data[data.Length - 1];
            
            originalLength = Convert.ToInt32(data[7]);
            for (int index = 8; index < data.Length - 1; index++)
            { counts.Add(0.0d); }

            exons = new List<exon>();
            addTranscript(data);
            int l = getGeneLength();
            
        }

        public void addTranscript(string[] data)
        {
            string[] starts = data[4].Split(';');
            string[] ends = data[5].Split(';');
            string[] name = data[3].Split(';');

            List<exon> theseExons = new List<exon>();

            for (int index = 0; index < starts.Length; index++)
            {              
                exon exon = new exon(name[index], Convert.ToInt32(starts[index]), Convert.ToInt32(ends[index]));
                theseExons.Add(exon);
            }             

            foreach (exon exon in theseExons)
            {                
                bool added = false;
                for (int index = 0; index < exons.Count; index++)
                {
                    exon thisExon = exons[index];
                    if (exon.Name == thisExon.Name)
                    { 
                        if (exon.X < thisExon.X && exon.Y > thisExon.Y)
                        {
                            thisExon.X = exon.X;
                            thisExon.Y = exon.Y;
                            exons[index] = thisExon;
                            added = true;
                        }
                        else if (exon.X >= thisExon.X && exon.Y <= thisExon.Y)
                        { added = true; }
                        else if (exon.X < thisExon.X && exon.Y >= thisExon.X && exon.Y <= thisExon.Y)
                        {
                            thisExon.X = exon.X;
                            exons[index] = thisExon;
                            added = true;
                        }
                        else if (exon.Y > thisExon.Y && exon.X <= thisExon.Y && exon.X >= thisExon.X)
                        {
                            thisExon.Y = exon.Y;
                            exons[index] = thisExon;
                            added = true;
                        }
                    } 
                }
                if (added == false)
                { exons.Add(exon); }

            }

            int counter = 0;
            for (int index = 8; index < data.Length - 1; index++)
            {
                counts[counter] += (Convert.ToDouble(data[index]));
                counter++;
            }
        }

        public int getSampleCount { get { return counts.Count; } }

        public List<double> AddCounts(List<double> countsTotals)
        {
            for(int index = 0; index < counts.Count; index++)
            { countsTotals[index] += counts[index]; }

            return countsTotals;
        }

        public int getGeneLength()
        {
            int length = 0;
            foreach (exon exon in exons)
            { length += exon.Y + 1 - exon.X; }
            return length;
        }
        public void writeline(System.IO.StreamWriter sw, List<double> totals)
        {
            double kb = (double)getGeneLength() / 1000;
            sw.Write(name);
            for (int index = 0; index < counts.Count; index++)
            {
                sw.Write("\t" + (counts[index] / (kb * totals[index])).ToString("N2"));
            }
            sw.Write("\n");
        }
    }
}
