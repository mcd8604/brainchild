using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WorldMaker
{
    public partial class PropertyEditor : Form
    {
        public PropertyEditor(Object o)
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = o;
            propertyGrid1.ExpandAllGridItems();
        }
    }
}