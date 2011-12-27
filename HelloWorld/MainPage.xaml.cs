using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace HelloWorld
{
    public partial class MainPage : UserControl
    {
        private static int _index = 0;
        private double _startPos = 0;
        private double _lastPos = 0;
        private double _between = 0;
        private int _timerInterval = 3000; 
        private List<UIElement> _controlsList = new List<UIElement>();
        

        public MainPage()
        {
            InitializeComponent();
            _controlsList.Add(C1);
            _controlsList.Add(C2);
            _controlsList.Add(C3);
            _controlsList.Add(C4);
            _controlsList.Add(C5);
            _controlsList.Add(C6);
            StartTimer();
            _startPos = Canvas.GetLeft(C1);
            _between = Canvas.GetLeft(C2) - _startPos;
            _lastPos = Canvas.GetLeft(C6);
            sb_Completed(null, null);

        }


        private void StartTimer()
        {
            System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, _timerInterval);
            myDispatcherTimer.Tick += new EventHandler(TimeUp);
            myDispatcherTimer.Start();
        }

        private void TimeUp(object o, EventArgs sender)
        {
            int count = 0;
            foreach (UIElement c in _controlsList)
            {
                count++;
                if (count == _controlsList.Count)
                {
                    MoveOneControl(c, true);
                }
                else
                {
                    MoveOneControl(c, false);
                }
            }


            
           
            
        }




        private void MoveOneControl(UIElement c, bool setHandle)
        {
            double current = Canvas.GetLeft(c);
            double future = current + _between;
            if (future > _lastPos)
            {
                future = _startPos;
            }

            // Set up the animation to layout in grid
            Storyboard moveStoryboardX = new Storyboard();

            // Create Animation
            DoubleAnimationUsingKeyFrames moveAnimation = new DoubleAnimationUsingKeyFrames();
            moveAnimation.Duration = TimeSpan.FromSeconds(1);

            // Create Keyframe
            SplineDoubleKeyFrame startKeyframe = new SplineDoubleKeyFrame();
            startKeyframe.Value = current;
            startKeyframe.KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero);
            moveAnimation.KeyFrames.Add(startKeyframe);

            startKeyframe = new SplineDoubleKeyFrame();
            if (future > current)
            {
                startKeyframe.Value = current + (future - current) / 2;
            }
            else
            {
                startKeyframe.Value = future + (current - future) / 2;

            }

            startKeyframe.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(500));

            KeySpline ks = new KeySpline();
            ks.ControlPoint1 = new Point(0, 1);
            ks.ControlPoint2 = new Point(1, 1);
            startKeyframe.KeySpline = ks;
            moveAnimation.KeyFrames.Add(startKeyframe);

            startKeyframe = new SplineDoubleKeyFrame();
           
            startKeyframe.Value = future;
            
            startKeyframe.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(800));
            moveAnimation.KeyFrames.Add(startKeyframe);
            
            Storyboard.SetTarget(moveAnimation, (UIElement)c);
            Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("(Canvas.Left)"));
                  
            moveStoryboardX.Children.Add(moveAnimation);
            canvasCts.Resources.Add(Convert.ToString(_index), moveStoryboardX);
            _index++;
            // Play Storyboard
            moveStoryboardX.Begin();

            if (setHandle)
            {
                moveStoryboardX.Completed += new EventHandler(sb_Completed);
            }
           
            canvasCts.Resources.Clear();
        }

        void sb_Completed(object sender, EventArgs e)
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 1.0;// 1.0;
            animation.To = 0.1;
            animation.Duration = TimeSpan.FromMilliseconds(_timerInterval/2);
            animation.AutoReverse = true;
            Random rd = new Random();
            int i = rd.Next(_controlsList.Count);
            Storyboard.SetTarget(animation, (UIElement)_controlsList[i]);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
            sb.Children.Add(animation);
            canvasCts.Resources.Add(Convert.ToString(_index++), sb);
            sb.Begin();
            canvasCts.Resources.Clear();
            //MessageBox.Show("Completed");
        }
    }
}
