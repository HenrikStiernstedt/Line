using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Liner.Shapes
{
    class Dot
    {

        public static void paintDot(Canvas canvas, Point p)
        {

            paintDot(canvas, p.X, p.Y);

        }
        public static void paintDot(Canvas canvas, double X, double Y)
        {

            Point startPoint = new Point(X-3, Y-3);

            Rectangle rectangle = new Rectangle();
            rectangle.Stroke = SystemColors.WindowFrameBrush;
            rectangle.Width = 6;
            rectangle.Height = 6;
            rectangle.Fill = SystemColors.WindowFrameBrush;

            Canvas.SetLeft(rectangle,startPoint.X);
            Canvas.SetTop(rectangle,startPoint.Y);
            canvas.Children.Add(rectangle);
        }
    }
}
