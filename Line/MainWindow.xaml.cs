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
using Liner.Shapes;
using Liner.source.model;
using Liner.source.shapes;

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

        Lines lines = new Lines();
        Random rand = new Random();

        private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(lines.ActivePoint)
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
          /*
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
            */
        }

        private void Canvas_MouseUp_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // TODO: Remove Console.Out.WriteLine("UP " + (ActivePoint ? "t" : "f"));
            
            if(lines.ActivePoint)
            {
                // Finnish the line!
                // Remove temporary line.
                MyCanvas.Children.Remove(lines.CurrentLine);

                // Add the final line.
                Dot.paintDot(MyCanvas, e.GetPosition(this));

                UnintersectingLine line = new UnintersectingLine(lines.CurrentPoint, e.GetPosition(this), lines.AllUnintersectingLines);
                if(line.HasErrors)
                {
                    // Hittade ingen giltig path.
                }
                else
                {
                    lines.AllUnintersectingLines.Add(line);

                    line.Stroke = SystemColors.WindowFrameBrush;
                    line.Stroke = new SolidColorBrush(Color.FromRgb((byte)rand.Next(0, 200), (byte)rand.Next(0, 200), (byte)rand.Next(0, 200)));
                    /*
                    Line line = new Line();

                    line.Stroke = SystemColors.WindowFrameBrush;
                    line.X1 = lines.CurrentPoint.X;
                    line.Y1 = lines.CurrentPoint.Y;
                    line.X2 = e.GetPosition(this).X;
                    line.Y2 = e.GetPosition(this).Y;
                    */


                    MyCanvas.Children.Add(line);

                    // Test
                    Point extendedPoint = line.OriginalLine.ExtendLine(line.OriginalLine.To, 10);
                    Console.Out.WriteLine("Extended Point: " + extendedPoint);
                    Dot.paintDot(MyCanvas, extendedPoint);
                }

                lines.ActivePoint = false;
            }
            else
            {

                //Start a new line.
                lines.CurrentPoint = e.GetPosition(this);

                // Paint a point
                Dot.paintDot(MyCanvas, lines.CurrentPoint);
                lines.ActivePoint = true;
            }

        }

    }
}
