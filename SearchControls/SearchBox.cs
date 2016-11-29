using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SearchControls
{
    public partial class SearchBox : UserControl
    {
        public SearchBox()
        {
            InitializeComponent();
        }

        public event Action<SearchItem> SelectionMade = i => { };

        [Browsable(false)]
        public SearchDataSource DataSource
        {
            get;
            set;
        }

        private void SearchBox_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(DataSource[""].ToArray());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.BackColor = Color.White;
                comboBox1.DroppedDown = false;
                return;
            }

            var results = Intersection(Tokenize(textBox1.Text)
                .Select(keyword => DataSource[keyword.ToLower()])
                .ToArray());

            if (results.Count > 0)
            {
                textBox1.BackColor = Color.White;
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(results.OrderBy(item => item.Title).ToArray());
                comboBox1.DroppedDown = true;
            }
            else
            {
                textBox1.BackColor = Color.LightPink;
                comboBox1.DroppedDown = false;
            }
        }

        private static string[] Tokenize(string text)
        {
            return text.Replace("-", "")
                .Replace(".", "")
                .Replace("&", " and ")
                .Split(" \t,".ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        private static HashSet<T> Intersection<T>(params HashSet<T>[] sets)
        {
            HashSet<T> result = new HashSet<T>();
            if (sets.Length > 0)
                result.UnionWith(sets[0]);

            foreach (var set in sets.Skip(1))
                result.IntersectWith(set);

            return result;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    up();
                    e.Handled = true;
                    break;
                case Keys.Down:
                    down();
                    e.Handled = true;
                    break;
                case Keys.Enter:
                    if (submit())
                        e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        private bool submit()
        {
            if (!comboBox1.DroppedDown)
                return false;
            if (comboBox1.SelectedIndex < 0)
                return false;

            SelectionMade(comboBox1.Items[comboBox1.SelectedIndex] as SearchItem);
            return true;
        }

        private void down()
        {
            if (!comboBox1.DroppedDown)
                return;

            if (comboBox1.SelectedIndex < 0)
                comboBox1.SelectedIndex = 0;
            else if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
                comboBox1.SelectedIndex = 0;
            else
                comboBox1.SelectedIndex++;
        }

        private void up()
        {
            if (!comboBox1.DroppedDown)
                return;

            if (comboBox1.SelectedIndex <= 0)
                comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            else
                comboBox1.SelectedIndex--;
        }
    }
}
