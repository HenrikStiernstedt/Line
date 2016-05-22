using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using Wp7nl.Utilities;
using System;

namespace Liner.source.shapes
{
    class UnintersectingLine : System.Windows.Shapes.Shape
    {
        private Point p1;
        private Point p2;
        List<Point> points = new List<Point>();
        int counter = 0;

        public List<LineF> LineSegments { get; set; }
        public bool HasErrors {get; set;}

        public UnintersectingLine(Point point1, Point point2, List<UnintersectingLine> lines)
        {
            LineSegments = new List<LineF>();
            p1 = point1;
            p2 = point2;

            points.Add(point1);
            //points.Add(new Point(point1.X + 30, point2.Y - 30)); // Test. Fungerar bra att rita ut linjer som en path.

            // Enkel orekursiv version.
            LineF thisline = new LineF(p1, p2);

            Console.Out.WriteLine("Ny linje " + p1 + " till " + p2);
            counter = 0;
            LineSegments = FindPath(p1, p2, lines);
            if (LineSegments == null)
            {
                HasErrors = true;
                return;
            }
            /*
            foreach(UnintersectingLine otherUnintersectingLine in lines)
            {
                foreach (LineF otherLine in otherUnintersectingLine.LineSegments)
                {
                    Point? intersection = thisline.Intersection(otherLine);
                    if(intersection != null)
                    {
                        // Vi har en korsning. Lägg till ny punkt istället. 
                        points.Add(otherLine.To);
                    }
                }
            }
            */

            points.Add(point2);

            // Registrera alla segment för framtida kollisionsdetekteringar.
            Point lastPoint = p1;
            for (int i = 1; i < points.Count; i++)
            {
                LineSegments.Add(new LineF(lastPoint, points[i]));
                lastPoint = points[i];

            }
        }

        public List<LineF> FindPath(Point from, Point to, List<UnintersectingLine> existingLines)
        {
            counter++;
            if (counter == 100) return null;
 
            LineF thisline = new LineF(from, to);
            List<LineF> res = new List<LineF>();

            foreach (UnintersectingLine otherUnintersectingLine in existingLines)
            {
                foreach (LineF otherLine in otherUnintersectingLine.LineSegments)
                {
                    Point? intersection = thisline.Intersection(otherLine);
                    if (intersection != null)
                    {
                        
                        float dFrom = (float)Math.Sqrt(Math.Pow(intersection.Value.X - otherLine.From.X, 2) + Math.Pow(intersection.Value.Y - otherLine.From.Y, 2));
                        float dTo = (float)Math.Sqrt(Math.Pow(intersection.Value.X - otherLine.To.X, 2) + Math.Pow(intersection.Value.Y - otherLine.To.Y, 2));

                        // Specialare! Om vi är close enough, anse at det var ok.
                        if( dFrom < 1 || dTo < 1)
                        {
                            //Console.Out.WriteLine("Punkten (" + intersection.Value.X + ", " + intersection.Value.Y + ") anses ok.");
                            //res.Add(new LineF(from, to));
                            //   return res;
                            continue;
                        }
                        
                        // Vi har en korsning. Lägg till ny punkt istället. Kortast väg vinner.
                        Point p = (dFrom < dTo ? otherLine.From : otherLine.To);
                        
                     //   p.X += 5;
                     //   p.Y += 5;
                     //   Console.Out.WriteLine("Från " + from.ToString() + " till " + p.ToString());
                     //   Console.Out.WriteLine("Sen från " + p.ToString() + " till " + to.ToString());
                        res = FindPath(from, p, existingLines);
                        if(res != null)
                        {
                            List<LineF> part2 = FindPath(p, to, existingLines);
                            if(part2 != null)
                            {
                                res.AddRange(part2);
                            }
                        }
                        return res;

                    }
                    else
                    {
                        // Keep looking.
                        continue;
                    }
                }
            
            }

         //   Console.Out.WriteLine("Från " + from.ToString() + " till " + to.ToString());

            res.Add(new LineF(from, to));
            return res;
        }


        /// <summary>
        /// Denna anropas av WPF när linjen lagts till en Canvas.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                List<PathSegment> segments = new List<PathSegment>();
                segments.Add(new LineSegment(p1, true));
                foreach (LineF line in LineSegments)
                {
                    segments.Add(new LineSegment(line.To, true));
                }

                /*
                List<PathSegment> segments = new List<PathSegment>();
                segments.Add(new LineSegment(p1, true));
                foreach (Point p in points)
                {
                    segments.Add(new LineSegment(p, true));
                }
                //segments.Add(new LineSegment(p2, true));
                */
                List<PathFigure> figures = new List<PathFigure>(1);
                PathFigure pf = new PathFigure(p1, segments, false);
                figures.Add(pf);

                return new PathGeometry(figures, FillRule.EvenOdd, null);
            }
        }
    }

    
}
