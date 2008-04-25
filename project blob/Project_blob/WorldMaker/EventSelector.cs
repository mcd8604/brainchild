using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Project_blob;

namespace WorldMaker {
    public partial class EventSelector : Form {
        private enum Events { AREATRANSITION, PLAYERWARP, SWITCHACTIVATION, CAMERAPAN, DELTA };

        private WarpSetter _warpSetter;
        private SwitchSetter _switchSetter;
        private CameraPanSetter _cameraPanSetter;
        private TransitionSetter _transitionSetter;
        private DeltaSetter _deltaSetter;

        private EventTrigger _eventTrigger;
        public EventTrigger EventTrigger { get { return _eventTrigger; } }

        public EventSelector( ) {
            InitializeComponent( );

			System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom("Project_blob.exe");
			List<Type> EventSubClasses = new List<Type>();
			foreach (Type t in asm.GetTypes())
			{
				if(typeof(Project_blob.EventTrigger).IsAssignableFrom(t))
				{
					Console.WriteLine(t + " is a Event Type.");
					EventSubClasses.Add(t);
				}
			}
		
        }

        private void cancelButton_Click( object sender, EventArgs e ) {
            this.DialogResult = DialogResult.Cancel;
            this.Close( );
        }

        private void selectButton_Click( object sender, EventArgs e ) {
            if( eventBox.SelectedIndex != -1 ) {
                switch( (Events)eventBox.SelectedIndex ) {
                    case Events.AREATRANSITION:
                        _transitionSetter = new TransitionSetter( );
                        _transitionSetter.ShowDialog( );
                        _eventTrigger = _transitionSetter.Transition;
                        break;
                    case Events.PLAYERWARP:
                        _warpSetter = new WarpSetter( );
                        _warpSetter.ShowDialog( );
                        _eventTrigger = _warpSetter.Warp;
                        break;
                    case Events.SWITCHACTIVATION:
                        _switchSetter = new SwitchSetter( );
                        _switchSetter.ShowDialog( );
                        //_eventTrigger = _switchSetter;
                        break;
                    case Events.CAMERAPAN:
                        _cameraPanSetter = new CameraPanSetter( );
                        _cameraPanSetter.ShowDialog( );
                        _eventTrigger = _cameraPanSetter.CameraPan;
                        break;
                    case Events.DELTA:
                        _deltaSetter = new DeltaSetter();
                        _deltaSetter.ShowDialog();
                        _eventTrigger = _deltaSetter.Delta;
                        break;
                };
                if( _eventTrigger != null ) {
                    this.DialogResult = DialogResult.OK;
                    this.Close( );
                }
            }
        }
    }
}