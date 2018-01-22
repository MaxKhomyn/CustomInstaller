using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WIXInstaller.UIStyles.Controls
{
    public delegate void Complete();
    class Animation
    {
        public static void ButtonDownAnimation(Button button)
        {
            DoubleAnimation Animation = new DoubleAnimation();

            Animation.To = 500;
            Animation.Duration = TimeSpan.FromSeconds(1);
            Animation.AutoReverse = true;
            button.BeginAnimation(Canvas.TopProperty, Animation);
        }

        Complete completed { get; set; }
        Frame frame { get; set; }

        public void FrameInNextAnimation(Frame frame)
        {
            ThicknessAnimation Animation = new ThicknessAnimation()
            {
                From = new Thickness(-frame.ActualWidth + 200, 0, frame.ActualWidth + 200, 0),
                To = new Thickness(0, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(0.3),
            };
            frame.BeginAnimation(Frame.MarginProperty, Animation);
        }
        public void FrameOutNextAnimation(Frame _frame, Complete _completed)
        {
            completed = _completed;
            frame = _frame;
            ThicknessAnimation Animation = new ThicknessAnimation()
            {
                From = new Thickness(0, 0, 0, 0),
                To = new Thickness(-_frame.ActualWidth + 200, 0, _frame.ActualWidth + 200, 0),
                Duration = TimeSpan.FromSeconds(0.3)
            };
            Animation.Completed += BackAnimation_Completed;
            _frame.BeginAnimation(Frame.MarginProperty, Animation);
        }
        private void NextAnimation_Completed(object sender, EventArgs e)
        {
            completed();
            FrameInNextAnimation(frame);
        }

        public void FrameInBackAnimation(Frame frame)
        {
            ThicknessAnimation Animation = new ThicknessAnimation()
            {
                From = new Thickness(frame.ActualWidth, 0, -frame.ActualWidth, 0),
                To = new Thickness(0, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(0.3)
            };
            frame.BeginAnimation(Frame.MarginProperty, Animation);
        }
        public void FrameOutBackAnimation(Frame _frame, Complete _completed)
        {
            completed = _completed;
            frame = _frame;
            ThicknessAnimation Animation = new ThicknessAnimation()
            {
                From = new Thickness(0, 0, 0, 0),
                To = new Thickness(_frame.ActualWidth, 0, -frame.ActualWidth, 0),
                Duration = TimeSpan.FromSeconds(0.3)
            };
            Animation.Completed += NextAnimation_Completed;
            _frame.BeginAnimation(Frame.MarginProperty, Animation);
        }
        private void BackAnimation_Completed(object sender, EventArgs e)
        {
            completed();
            FrameInBackAnimation(frame);
        }

        public void IndicatorAnimation(int RowIndex, Border border)
        {
            ThicknessAnimation Animation0 = new ThicknessAnimation()
            {
                To = new Thickness(0, ((RowIndex++) * border.ActualHeight), 0, 0),
                Duration = TimeSpan.FromSeconds(0.3),
                AccelerationRatio = 0.1
            };
            DoubleAnimation Animation1 = new DoubleAnimation()
            {
                To = border.ActualHeight + (border.ActualHeight / 2),
                AutoReverse = true,
                Duration = TimeSpan.FromSeconds(0.15),
                AccelerationRatio = 0.07
            };

            border.BeginAnimation(Border.MarginProperty, Animation0);
            border.BeginAnimation(Border.HeightProperty, Animation1);
        }
        public void ImageAnimation(double? width, Image image)
        {
            double Width = 700;
            try
            {
                if(((double)width).ToString()!="NaN")
                    Width = (double)width;
            }
            catch { }
            ThicknessAnimation Animation0 = new ThicknessAnimation()
            {
                From = image.Margin,
                To = new Thickness(0, 0, -(image.ActualWidth - Width), 0),
                Duration = TimeSpan.FromSeconds((int)((image.ActualWidth - Width) / 10)),
                //Duration = TimeSpan.FromSeconds(60),
                AutoReverse = true
            };
            Animation0.RepeatBehavior = RepeatBehavior.Forever;
            image.BeginAnimation(Image.MarginProperty, Animation0);
        }
        //public void IndicatorAnimation(int RowIndex, Border border)
        //{
        //    Int32Animation Animation = new Int32Animation()
        //    {
        //        From = Grid.GetRow(border),
        //        To = RowIndex,
        //        Duration = TimeSpan.FromSeconds(0.3)
        //    };
        //    border.BeginAnimation(Grid.rowProperty, Animation);
        //}
    }
}