using Liner.source.shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Liner.source.model
{
    class Lines
    {

        
        /// <summary>
        /// Are we currently having a single point, waiting for the last to complete an unintersecting line.
        /// </summary>
        public bool ActivePoint {get; set;}
        bool activePoint = false;

        Point currentPoint = new Point();

        /// <summary>
        /// The first point in the line currently beeing drawn.
        /// </summary>
        public Point CurrentPoint {get; set;}

        Line currentLine = null;
        /// <summary>
        /// The current line if drawing on MouseMove is enabled.
        /// </summary>
        public Line CurrentLine {get; set;}

        private List<Line> allLines;

        public List<Line> AllLines {get; set;}


        private List<UnintersectingLine> allUnintersectingLines = new List<UnintersectingLine>();

        internal List<UnintersectingLine> AllUnintersectingLines
        {
            get { return allUnintersectingLines; }
            set { allUnintersectingLines = value; }
        }


        public bool AddUnintersectingLine(Point p1, Point p2)
        {

            UnintersectingLine newLine = new UnintersectingLine(p1, p2, this.AllUnintersectingLines);
            

            return false;
        }

    }
}
