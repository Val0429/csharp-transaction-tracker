using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;

namespace TimeTrack
{
    public sealed partial class MiniTimeTrackController : UserControl
    {
        public event EventHandler OnPlay;
        public event EventHandler OnStop;

        private static Image _pause = Resources.GetResources(Properties.Resources.Pause, Properties.Resources.IMGPause);
        private static Image _play = Resources.GetResources(Properties.Resources.Play, Properties.Resources.IMGPlay);
        public MiniTimeTrackController()
        {
            InitializeComponent();

            Anchor = AnchorStyles.None;

            PauseButton.Visible = false;

            PauseButton.BackgroundImage = _pause;
            PlayButton.BackgroundImage = _play;
        }

        public void ActiveButton(String buttonName)
        {
            switch(buttonName)
            {
                case "Pause":
                    PlayButton.Visible = false;
                    PauseButton.Visible = true;
                    break;

                case "Play":
                    PlayButton.Visible = true;
                    PauseButton.Visible = false;
                    break;
            }
        }

        private void PlayButtonMouseUp(Object sender, MouseEventArgs e)
        {
            if (OnPlay != null)
                OnPlay(this, null);
        }

        private void PauseButtonMouseUp(Object sender, MouseEventArgs e)
        {
            if (OnStop != null)
                OnStop(this, null);
        }
    }
}
