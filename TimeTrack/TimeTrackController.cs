using System;
using System.Drawing;
using System.Windows.Forms;
using Constant;
using Interface;

namespace TimeTrack
{
    public partial class TimeTrackController : UserControl
    {
        public event EventHandler<EventArgs<float>> OnPlayRateChanged;
        public event EventHandler OnPlay;
        public event EventHandler OnStop;
        public IServer Server;

        public readonly System.Timers.Timer _fasterBarTimer = new System.Timers.Timer();
        public readonly System.Timers.Timer _reverseBarTimer = new System.Timers.Timer();

        private static readonly Image _play = Resources.GetResources(Properties.Resources.Play, Properties.Resources.IMGPlay);
        private static readonly Image _playBlack = Resources.GetResources(Properties.Resources.PlayBlack, Properties.Resources.IMGPlayBlack);
        private static readonly Image _pause = Resources.GetResources(Properties.Resources.Pause, Properties.Resources.IMGPause);
        private static readonly Image _fastPlayBlack = Resources.GetResources(Properties.Resources.FastPlayBlack, Properties.Resources.IMGFastPlayBlack);
        private static readonly Image _fastReverseBlack = Resources.GetResources(Properties.Resources.FastReverseBlack, Properties.Resources.IMGFastReverseBlack);
        private static readonly Image _fastPlay = Resources.GetResources(Properties.Resources.FastPlay, Properties.Resources.IMGFastPlay);
        private static readonly Image _fastReverse = Resources.GetResources(Properties.Resources.FastReverse, Properties.Resources.IMGFastReverse);

        private PictureBox _activeButton;
        public TimeTrackController()
        {
            InitializeComponent();
            DoubleBuffered = true;

            Anchor = AnchorStyles.None;

            ActiveButton(PauseButton);

            FastReverseSpeedSelectorButton.BackgroundImage = Resources.GetResources(Properties.Resources.Black, Properties.Resources.IMGBlack);
            FastPlaySpeedSelectorButton.BackgroundImage = Resources.GetResources(Properties.Resources.Black, Properties.Resources.IMGBlack);

            FastPlayButton.BackgroundImage = _fastPlay;
            FastPlayList.BackgroundImage = Resources.GetResources(Properties.Resources.FastPlayList, Properties.Resources.IMGFastPlayList);

            FastReverseButton.BackgroundImage = _fastReverse;
            FastReverseList.BackgroundImage = Resources.GetResources(Properties.Resources.FastReverseList, Properties.Resources.IMGFastReverseList);

            PauseButton.BackgroundImage = _pause;
            PlayButton.BackgroundImage = _play;

            FastPlaySpeedSelectorButton.Location = new Point(FastSpeedPosition2, FastPlaySpeedSelectorButton.Location.Y);
            FastReverseSpeedSelectorButton.Location = new Point(ReverseSpeedPosition3, FastPlaySpeedSelectorButton.Location.Y);
        }

        public void Initialize()
        {
            _fasterBarTimer.Elapsed += ShowFastPlayList;
            _fasterBarTimer.Interval = 200;
            _fasterBarTimer.SynchronizingObject = Server.Form;

            _reverseBarTimer.Elapsed += ShowFastReverseList;
            _reverseBarTimer.Interval = 200;
            _reverseBarTimer.SynchronizingObject = Server.Form;
        }

        public void ActiveButton(String buttonName)
        {
            switch (buttonName)
            {
                case "Pause":
                    ActiveButton(PauseButton);
                    break;

                case "Play":
                    ActiveButton(PlayButton);
                    break;

                case "FastPlay":
                    ActiveButton(FastPlayButton);
                    break;

                case "FastReverse":
                    ActiveButton(FastReverseButton);
                    break;

                case "NextFrame":
                    ActiveButton(PauseButton);
                    fastSpeedLabel.Text = @"F";
                    reverseSpeedLabel.Text = "";
                    break;

                case "PreviousFrame":
                    ActiveButton(PauseButton);
                    fastSpeedLabel.Text = "";
                    reverseSpeedLabel.Text = @"F";
                    break;
            }
        }

        public void ActiveButton(PictureBox buttonObject)
        {
            if (_activeButton == buttonObject) return;

            DeactivateButton();
            _activeButton = buttonObject;

            if (_activeButton == PauseButton)
            {
                fastSpeedLabel.Text = reverseSpeedLabel.Text = "";

                PlayButton.Visible = true;
                PauseButton.Visible = false;
                PauseButton.BackgroundImage = _pause;
            }
            else if (_activeButton == PlayButton)
            {
                fastSpeedLabel.Text = @"1x";
                reverseSpeedLabel.Text = "";

                PlayButton.Visible = false;

                PauseButton.BackgroundImage = _pause;// _playBlack;
                PauseButton.Visible = true;
            }
            else if (_activeButton == FastPlayButton)
            {
                fastSpeedLabel.Text = _fastPlaySpeed + @"x";
                reverseSpeedLabel.Text = "";
                FastPlayButton.BackgroundImage = _fastPlayBlack;

                PlayButton.Visible = false;
                PauseButton.Visible = true;
                PauseButton.BackgroundImage = _pause;
            }
            else if (_activeButton == FastReverseButton)
            {
                fastSpeedLabel.Text = "";
                reverseSpeedLabel.Text = Math.Abs(_fastReverseSpeed) + @"x";
                FastReverseButton.BackgroundImage = _fastReverseBlack;

                PlayButton.Visible = false;
                PauseButton.Visible = true;
                PauseButton.BackgroundImage = _pause;
            }
        }

        private void DeactivateButton()
        {
            if (_activeButton == PlayButton)
            {
                PlayButton.Visible = true;
                PauseButton.Visible = false;
            }
            if (_activeButton == FastPlayButton)
                _activeButton.BackgroundImage = _fastPlay;
            else if (_activeButton == FastReverseButton)
                _activeButton.BackgroundImage = _fastReverse;

            _activeButton = null;
        }

        protected float _fastPlaySpeed = 2;
        protected float _fastReverseSpeed = -2;

        public void PlayButtonMouseUp(Object sender, MouseEventArgs e)
        {
            if (OnPlay != null)
                OnPlay(this, null);
        }

        public void PauseButtonMouseUp(Object sender, MouseEventArgs e)
        {
            if (OnStop != null)
                OnStop(this, null);
        }

        private void FastPlayButtonMouseDown(Object sender, MouseEventArgs e)
        {
            _fasterBarTimer.Enabled = true;
        }

        private void FastReverseButtonMouseDown(Object sender, MouseEventArgs e)
        {
            _reverseBarTimer.Enabled = true;
        }

        public void FastPlayButtonMouseUp(Object sender, MouseEventArgs e)
        {
            if (OnPlayRateChanged != null)
                OnPlayRateChanged(this, new EventArgs<float>(_fastPlaySpeed));

            _fasterBarTimer.Enabled = false;
            //SendToBack();
        }

        public void FastReverseButtonMouseUp(Object sender, MouseEventArgs e)
        {
            if (OnPlayRateChanged != null)
                OnPlayRateChanged(this, new EventArgs<float>(_fastReverseSpeed));

            _reverseBarTimer.Enabled = false;
            //SendToBack();
        }

        private void ShowFastPlayList(Object sender, EventArgs e)
        {
            //BringToFront();
            DeactivateButton();
            _fasterBarTimer.Enabled = false;
            FastPlayList.Visible = true;
            FastPlayList.Capture = true;
        }

        private void ShowFastReverseList(Object sender, EventArgs e)
        {
            //BringToFront();
            DeactivateButton();
            _reverseBarTimer.Enabled = false;
            FastReverseList.Visible = true;
            FastReverseList.Capture = true;
        }

        protected virtual void FastPlayListMouseUp(Object sender, MouseEventArgs e)
        {
            FastPlayList.Visible = false;
            FastPlayList.Capture = false;

            //back to default position
            FastPlaySpeedSelectorButton.Location = new Point(FastSpeedPosition2, FastPlaySpeedSelectorButton.Location.Y);

            FastPlayButtonMouseUp(this, e);
        }

        protected virtual void FastReverseListMouseUp(Object sender, MouseEventArgs e)
        {
            FastReverseList.Visible = false;
            FastReverseList.Capture = false;

            //back to default position
            FastReverseSpeedSelectorButton.Location = new Point(ReverseSpeedPosition3, FastPlaySpeedSelectorButton.Location.Y);

            FastReverseButtonMouseUp(this, e);
        }

        private Int32 _lastMovePosition;
        private const UInt16 SpeedDiff = 26;

        protected const UInt16 FastSpeedPosition1 = 0;
        protected const UInt16 FastSpeedPosition2 = 26;
        protected const UInt16 FastSpeedPosition3 = 52;
        protected const UInt16 FastSpeedPosition4 = 78;
        private void FastPlayListMouseMove(Object sender, MouseEventArgs e)
        {
            if (_lastMovePosition == e.X) return;

            Int32 newX = Math.Min(Math.Max(FastSpeedPosition1, e.X), FastSpeedPosition4 + SpeedDiff);

            if (newX <= FastSpeedPosition1 + SpeedDiff)
            {
                FastPlaySpeedSelectorButton.Location = new Point(FastSpeedPosition1, FastPlaySpeedSelectorButton.Location.Y);
                _fastPlaySpeed = GetFastPlaySpeed(0);
            }
            else if (newX > FastSpeedPosition2 && newX <= FastSpeedPosition2 + SpeedDiff)
            {
                FastPlaySpeedSelectorButton.Location = new Point(FastSpeedPosition2, FastPlaySpeedSelectorButton.Location.Y);
                _fastPlaySpeed = GetFastPlaySpeed(1);
            }
            else if (newX > FastSpeedPosition3 && newX <= FastSpeedPosition3 + SpeedDiff)
            {
                FastPlaySpeedSelectorButton.Location = new Point(FastSpeedPosition3, FastPlaySpeedSelectorButton.Location.Y);
                _fastPlaySpeed = GetFastPlaySpeed(2);
            }
            else
            {
                FastPlaySpeedSelectorButton.Location = new Point(FastSpeedPosition4, FastPlaySpeedSelectorButton.Location.Y);
                _fastPlaySpeed = GetFastPlaySpeed(3);
            }

            _lastMovePosition = e.X;
        }

        protected virtual float GetFastPlaySpeed(int selectorIndex)
        {
            switch (selectorIndex)
            {
                case 0:
                    return 0.5f;
                case 1:
                    return 2;
                case 2:
                    return 4;
                case 3:
                default:
                    return 8;
            }
        }

        private const UInt16 ReverseSpeedPosition1 = 0;
        protected const UInt16 ReverseSpeedPosition2 = 26;
        protected const UInt16 ReverseSpeedPosition3 = 52;
        protected const UInt16 ReverseSpeedPosition4 = 78;
        private void FastReverseListMouseMove(Object sender, MouseEventArgs e)
        {
            if (_lastMovePosition == e.X) return;

            Int32 newX = Math.Min(Math.Max(ReverseSpeedPosition1, e.X), ReverseSpeedPosition4 + SpeedDiff);

            if (newX <= ReverseSpeedPosition1 + SpeedDiff)
            {
                FastReverseSpeedSelectorButton.Location = new Point(ReverseSpeedPosition1, FastReverseSpeedSelectorButton.Location.Y);
                _fastReverseSpeed = GetReverseSpeed(0);
            }
            else if (newX > ReverseSpeedPosition2 && newX <= ReverseSpeedPosition2 + SpeedDiff)
            {
                FastReverseSpeedSelectorButton.Location = new Point(ReverseSpeedPosition2, FastReverseSpeedSelectorButton.Location.Y);
                _fastReverseSpeed = GetReverseSpeed(1);
            }
            else if (newX > ReverseSpeedPosition3 && newX <= ReverseSpeedPosition3 + SpeedDiff)
            {
                FastReverseSpeedSelectorButton.Location = new Point(ReverseSpeedPosition3, FastReverseSpeedSelectorButton.Location.Y);
                _fastReverseSpeed = GetReverseSpeed(2);
            }
            else
            {
                FastReverseSpeedSelectorButton.Location = new Point(ReverseSpeedPosition4, FastReverseSpeedSelectorButton.Location.Y);
                _fastReverseSpeed = GetReverseSpeed(3);
            }

            _lastMovePosition = e.X;
        }

        protected virtual float GetReverseSpeed(int selectorIndex)
        {
            switch (selectorIndex)
            {
                case 0:
                    return -8;
                case 1:
                    return -4;
                case 2:
                    return -2;
                case 3:
                default:
                    return -0.5f;
            }
        }
    }
}
