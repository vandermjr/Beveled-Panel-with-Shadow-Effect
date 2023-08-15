using System;
using System.Drawing;
using System.Windows.Forms;
using BevelPanel;

namespace PanelExample
{
    public partial class Form1 : Form
    {
        public Point Grid { get; private set; }
        public Form1()
        {
            InitializeComponent();
            Grid = new Point(4, 4);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateBlocks();
        }

        public Point SetBlockLocation(int position)
        {
            Point pos = GetBlockLocation(position, Grid.X, Grid.Y);
            return new Point(pos.X + Margin.Left, pos.Y + Margin.Top);
        }
        public static Point GetBlockLocation(int position, int columns, int rows)
        {
            return new Point((position - 1) % columns * 50, (position - 1) / rows * 50);
        }
        public void CreateBlocks()
        {
            for (int pos = 1; pos <= Grid.X * Grid.Y; pos++)
            {
                Point location = SetBlockLocation(pos);
                Block block = new Block
                {
                    Location = location,
                };
                Controls.Add(block);
            }
            ResumeLayout();
        }
    }
}
