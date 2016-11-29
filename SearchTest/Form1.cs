using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SearchControls;

namespace SearchTest
{
    public partial class Form1 : Form
    {
        private SearchDataSource _data;

        public Form1()
        {
            InitializeComponent();

            _data = new SearchDataSource();
            _data.Add(new SearchItem("Launch -> Basemap", null, "launch", "basemap", "map"));
            _data.Add(new SearchItem("Launch -> 3D Visualizer", null, "launch", "3d", "viz", "visualizer"));
            _data.Add(new SearchItem("Grid -> Properties", null, "grid", "properties"));
            _data.Add(new SearchItem("Grid -> Merge Grids", null, "grid", "merge"));
            _data.Add(new SearchItem("Horizon -> Properties", null, "horizon", "properties"));
            _data.Add(new SearchItem("Grid -> Grid and Contour", null, "grid", "horizon", "contour"));

            searchBox1.DataSource = _data;
            searchBox1.SelectionMade += new Action<SearchItem>(searchBox1_SelectionMade);
        }

        void searchBox1_SelectionMade(SearchItem obj)
        {
            MessageBox.Show(obj.Title);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
