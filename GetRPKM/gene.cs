using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GetRPKM
{   
    internal class gene
    {
        List<double> counts = new List<double>();
        List<Point> exons = new List<Point>();
        string name = "";

        public gene(string[] data)
        {
            name = data[data.Length-1];
            
            for (int index = 8;index < data.Length-1;index++)
            { counts.Add(0.0d); }

            addTranscript(data);
            
        }

        public void addTranscript(string[] data)
        {
            string[] starts = data[4].Split(';');
            string[] ends = data[5].Split(';');

            List<Point> theseExons = new List<Point>();

            for (int index = 0; index < starts.Length; index++)
            {              
                Point exon = new Point(Convert.ToInt32(starts[index]), Convert.ToInt32(ends[index]));
                theseExons.Add(exon);
            }

            List<Point> toAdd = new List<Point>();
            foreach (Point exon in theseExons)
            {
                if (exons.Contains(exon) == false)
                {
                    bool added = false;
                    for (int index = 0; index < exons.Count; index++)
                    {
                        Point thisExon = exons[index];
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
                            added=true;
                        }
                    }
                    if (added==false)
                    { exons.Add(exon); }
                }
            }

            int counter = 0;
            for (int index = 8; index < data.Length - 1; index++)
            {
                counts[counter] += (Convert.ToDouble(data[index]));
                counter++;
            }
        }
    }
}
