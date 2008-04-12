using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Project_blob;

namespace WorldMaker
{
    public partial class WarpSetter : Form
    {
        private WarpEvent _warp;
        public WarpEvent Warp { get { return _warp; } }

        public WarpSetter()
        {
            InitializeComponent();
        }

        private void okButton_Click( object sender, EventArgs e ) {
            if( !xPosText.Text.Equals( "" ) && !yPosText.Text.Equals( "" ) && !zPosText.Text.Equals( "" ) &&
                !xVelText.Text.Equals( "" ) && !yVelText.Text.Equals( "" ) && !zVelText.Text.Equals( "" ) ) {
                try {
                    _warp = new WarpEvent( float.Parse(xPosText.Text), float.Parse(yPosText.Text), float.Parse(zPosText.Text), 
                        float.Parse(xVelText.Text), float.Parse(yVelText.Text), float.Parse(zVelText.Text) );
                    this.Close( );
                } catch( Exception ) {
                }
            }
        }

        private void cancelButton_Click( object sender, EventArgs e ) {
            this.Close( );
        }

    }
}