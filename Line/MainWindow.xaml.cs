using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Liner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool ActivePoint = false;
        Point currentPoint = new Point();
        Line currentLine = null;

        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(ActivePoint)
            { 
                // Do nothing
            }
            else
            {
                // Do nothing
            }

        }

        private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (ActivePoint)
                {
                    Line line = new Line();


                    SolidColorBrush redBrush = new SolidColorBrush();
                    redBrush.Color = Colors.Red;

                    line.Stroke = redBrush;
                    line.X1 = currentPoint.X;
                    line.Y1 = currentPoint.Y;
                    line.X2 = e.GetPosition(this).X;
                    line.Y2 = e.GetPosition(this).Y;

                    //currentPoint = e.GetPosition(this);

                    MyCanvas.Children.Remove(currentLine);
                    MyCanvas.Children.Add(line);
                    currentLine = line;
                }
            }
            
        }

        private void Canvas_MouseUp_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Console.Out.WriteLine("UP " + (ActivePoint ? "t" : "f"));
            
            if(ActivePoint)
            {
                // Finnish the line!
                // Remove temporary line.
                MyCanvas.Children.Remove(currentLine);

                // Add the final line.
                Dot.paintDot(MyCanvas, e.GetPosition(this));

                Line line = new Line();

                line.Stroke = SystemColors.WindowFrameBrush;
                line.X1 = currentPoint.X;
                line.Y1 = currentPoint.Y;
                line.X2 = e.GetPosition(this).X;
                line.Y2 = e.GetPosition(this).Y;

                MyCanvas.Children.Add(line);

                ActivePoint = false;

            }
            else
            {

                //Start a new line.
                currentPoint = e.GetPosition(this);

                // Paint a point
                Dot.paintDot(MyCanvas, currentPoint);
                ActivePoint = true;

            }

          
           
        }

    }
}
