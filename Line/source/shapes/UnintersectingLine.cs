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
        public Point From { get; set; }
        public Point To { get; set; }
        List<Point> points = new List<Point>();
        /// <summary>
        /// Max counter for recursion.
        /// </summary>
        int counter = 0;

        public LineF OriginalLine { get; set; }
        public List<LineF> LineSegments { get; set; }
        public LineF FirstSegment { get { return LineSegments[0]; } }
        public LineF LastSegment { get { return LineSegments[LineSegments.Count - 1]; } }
        public bool HasErrors {get; set;}

        public UnintersectingLine(Point point1, Point point2, List<UnintersectingLine> lines)
        {
            LineSegments = new List<LineF>();
            From = point1;
            To = point2;
            OriginalLine = new LineF(From, To);

            points.Add(point1);
            //points.Add(new Point(point1.X + 30, point2.Y - 30)); // Test. Fungerar bra att rita ut linjer som en path.

            LineF thisline = new LineF(From, To);
             
            Console.Out.WriteLine("Ny linje " + From + " till " + To);
            counter = 0;
            LineSegments = FindPath(From, To, lines);
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
            Point lastPoint = From;
            for (int i = 1; i < points.Count; i++)
            {
                LineSegments.Add(new LineF(lastPoint, points[i]));
                lastPoint = points[i];

            }
        }

        public List<LineF> FindPath(Point from, Point to, List<UnintersectingLine> existingLines)
        {
            counter++;
            if (counter > 20)
            {
                Console.Out.WriteLine("Out of moves!");
                return null;
            }
 
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

                        /*
                        // Specialare! Om vi är close enough, anse at det var ok.
                        if( dFrom < 1 || dTo < 1)
                        {
                            //Console.Out.WriteLine("Punkten (" + intersection.Value.X + ", " + intersection.Value.Y + ") anses ok.");
                            //res.Add(new LineF(from, to));
                            //   return res;
                            continue;
                        }
                         */
                        

                        Point candidate1 = (dFrom < dTo ? otherUnintersectingLine.FirstSegment.ExtendFrom(10) : otherUnintersectingLine.LastSegment.ExtendTo(10));
                        Point candidate2 = (dFrom >= dTo ? otherUnintersectingLine.FirstSegment.ExtendFrom(10) : otherUnintersectingLine.LastSegment.ExtendTo(10));

                        // Vi har en korsning. Lägg till ny punkt istället. Kortast väg vinner.
                        // Här verkar det bli avgörande att ibland testa ett andra alternativ. 
                        Point p = candidate1;
                       /*
                        if(TraversedPoints.Contains(candidate1))
                        {
                            p = candidate2;
                        }
                        */

                        
                        Console.Out.WriteLine("  Från " + from.ToString() + " till " + p.ToString());
                        Console.Out.WriteLine("  Sen från " + p.ToString() + " till " + to.ToString());
                        res = FindPath(from, p, existingLines);
                        if(res != null)
                        {
                            List<LineF> part2 = FindPath(p, to, existingLines);
                            if(part2 != null)
                            {
                                res.AddRange(part2);
                            }
                            else
                            {
                                // Om vi körde fast i förra spåret, testa att gå åt andra hållet. Starta om med 20 nya fräscha försök.
                                counter = 0;
                                Point p2 = (dFrom >= dTo ? otherUnintersectingLine.FirstSegment.ExtendFrom(10) : otherUnintersectingLine.LastSegment.ExtendTo(10));
                                List<LineF> part2_2 = FindPath(p, to, existingLines);
                                if(part2_2 != null)
                                {
                                    res.AddRange(part2_2);
                                }
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
                segments.Add(new LineSegment(From, true));
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
                //segments.Add(new LineSegment(P2, true));
                */
                List<PathFigure> figures = new List<PathFigure>(1);
                PathFigure pf = new PathFigure(From, segments, false);
                figures.Add(pf);

                return new PathGeometry(figures, FillRule.EvenOdd, null);
            }
        }
    }

    
}
