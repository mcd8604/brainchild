using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WorldMakerDemo
{
    public partial class LevelEditor : Form
    {
        private Game1 m_Game;

        public LevelEditor(Game1 p_Game)
        {
            InitializeComponent();

            m_Game = p_Game;
        }
    }
}