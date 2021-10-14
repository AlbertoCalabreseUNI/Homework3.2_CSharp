using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homework3._2_CSharp
{
    public partial class Form1 : Form
    {
        public IEnumerable<String> book;
        public Dictionary<String, int> words;
        public IEnumerable<String> blacklist;
        public List<MyRectangle> rectangles;
        Timer timer;
        /*
         * My Frequencies: Word appearing once or twice, word appearing three or four times, words appearing 5+ times.
         */
        public int distribution2 = 0;
        public int distribution3_4 = 0;
        public int distribution5 = 0;
        public Form1()
        {
            InitializeComponent();
            book = File.ReadLines(@"Files\book.txt");
            blacklist = File.ReadLines(@"Files\stopwords.txt").ToList();
            words = new Dictionary<String, int>();

            timer = new Timer();
            rectangles = new List<MyRectangle>();


            myPictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.myPictureBox1_Paint);
            this.Controls.Add(this.myPictureBox1);

            timer.Tick += new EventHandler(timer1_Tick);
            timer.Interval = 50;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (String line in book)
            {
                var ws = line.Split();
                foreach(String word in ws)
                {
                    //With this we filter out all the words that are less than 2 chars long and we discard numbers. Additionaly you
                    // can use the populateBlacklist() function to populate the blacklist in order to discard specific words.
                    if (word.Length < 2 || int.TryParse(word, out int n) || blacklist.Contains(word.ToLower())) continue;
                    if (!words.ContainsKey(word))
                        words.Add(word, 1);
                    else
                        words[word] += 1;
                }
            }
            //LINQ to sort by value
            //We discard any word that has only happeared once.
            var sortedDict = from entry in words where entry.Value > 1 orderby entry.Value descending select entry;
            foreach(KeyValuePair<String,int> word in sortedDict)
            {
                if (word.Value == 2) distribution2++;
                if (word.Value == 3 || word.Value == 4) distribution3_4++;
                if (word.Value >= 5 ) distribution5++;
            }

            SolidBrush color = new SolidBrush(Color.FromArgb(255,0,0));
            rectangles.Add(new MyRectangle(20, distribution2/3, label1.Location.X, myPictureBox1.Height, color, myPictureBox1));
            rectangles.Add(new MyRectangle(20, distribution3_4/3, label2.Location.X, myPictureBox1.Height, color, myPictureBox1));
            rectangles.Add(new MyRectangle(20, distribution5/3, label3.Location.X, myPictureBox1.Height, color, myPictureBox1));

            timer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e) { this.myPictureBox1.Refresh(); }

        private void myPictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            foreach (MyRectangle rect in rectangles)
            {
                rect.Draw(g);
                rect.Update();
            }
        }
    }
}
