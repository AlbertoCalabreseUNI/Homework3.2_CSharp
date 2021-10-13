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
        public List<String> blacklist;
        public Form1()
        {
            InitializeComponent();
            book = File.ReadLines(@"Files\book.txt");
            words = new Dictionary<String, int>();
            blacklist = new List<String>();
            populateBlacklist();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (String line in book)
            {
                var ws = line.Split();
                foreach(String word in ws)
                {
                    if (word.Length < 1 ) continue;
                    if (!words.ContainsKey(word))
                        words.Add(word, 1);
                    else
                        words[word] += 1;
                }
            }
            //LINQ to sort by value
            var sortedDict = from entry in words where entry.Value > 1 orderby entry.Value descending select entry;
            foreach(KeyValuePair<String,int> word in sortedDict.Take(10))
            {
                textBox1.AppendText(word.Key + " - word count: " + word.Value);
                textBox1.AppendText(Environment.NewLine);
            }
        }

        public void populateBlacklist()
        {
            blacklist.Add("the");
            blacklist.Add("and");
            blacklist.Add("for");
            blacklist.Add("not");
            blacklist.Add("has");
            blacklist.Add("are");
            blacklist.Add("have");
            blacklist.Add("that");
            blacklist.Add("with");
            blacklist.Add("but");
            blacklist.Add("was");
            blacklist.Add("were");
            blacklist.Add("this");
            blacklist.Add("from");
            blacklist.Add("its");
            blacklist.Add("his");
            blacklist.Add("her");
        }
    }
}
