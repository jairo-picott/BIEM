using System;
using System.Collections.Generic;
using System.IO;

namespace BIEM.OutputReporting
{
    public enum DrawingElementTypeEnum
    {
        Text,
        Walls,
        Windows,
        GlassDoors,
        Doors,
        Roofs,
        Floors,
        DetachedBuildingShades,
        DetachedFixedShades,
        AttachedBuildingShades,
        Photovoltaics,
        TubularDaylightDomes,
        TubularDaylightDiffusers,
        DaylightReferencePoint1,
        DaylightReferencePoint2
    }
    public class OutputControlSurfaceColorScheme
    {
        private string _name;
        private DrawingElementTypeEnum? _drawingElement1Type;
        private int _colorforDrawingElement1;
        private DrawingElementTypeEnum? _drawingElement2Type;
        private int _colorforDrawingElement2;
        private DrawingElementTypeEnum? _drawingElement3Type;
        private int _colorforDrawingElement3;
        private DrawingElementTypeEnum? _drawingElement4Type;
        private int _colorforDrawingElement4;
        private DrawingElementTypeEnum? _drawingElement5Type;
        private int _colorforDrawingElement5;
        private DrawingElementTypeEnum? _drawingElement6Type;
        private int _colorforDrawingElement6;
        private DrawingElementTypeEnum? _drawingElement7Type;
        private int _colorforDrawingElement7;
        private DrawingElementTypeEnum? _drawingElement8Type;
        private int _colorforDrawingElement8;
        private DrawingElementTypeEnum? _drawingElement9Type;
        private int _colorforDrawingElement9;
        private DrawingElementTypeEnum? _drawingElement10Type;
        private int _colorforDrawingElement10;
        private DrawingElementTypeEnum? _drawingElement11Type;
        private int _colorforDrawingElement11;
        private DrawingElementTypeEnum? _drawingElement12Type;
        private int _colorforDrawingElement12;
        private DrawingElementTypeEnum? _drawingElement13Type;
        private int _colorforDrawingElement13;
        private DrawingElementTypeEnum? _drawingElement14Type;
        private int _colorforDrawingElement14;
        private DrawingElementTypeEnum? _drawingElement15Type;
        private int _colorforDrawingElement15;

        public string Name { get => _name; set => _name = value; }
        public DrawingElementTypeEnum? DrawingElement1Type { get => _drawingElement1Type; set => _drawingElement1Type = value; }
        public int ColorforDrawingElement1 
        { 
            get => _colorforDrawingElement1;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement1 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement2Type { get => _drawingElement2Type; set => _drawingElement2Type = value; }
        public int ColorforDrawingElement2 
        { 
            get => _colorforDrawingElement2;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement2 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement3Type { get => _drawingElement3Type; set => _drawingElement3Type = value; }
        public int ColorforDrawingElement3 
        { 
            get => _colorforDrawingElement3;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement3 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement4Type { get => _drawingElement4Type; set => _drawingElement4Type = value; }
        public int ColorforDrawingElement4 
        { 
            get => _colorforDrawingElement4;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement4 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement5Type { get => _drawingElement5Type; set => _drawingElement5Type = value; }
        public int ColorforDrawingElement5 
        { 
            get => _colorforDrawingElement5;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement5 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement6Type { get => _drawingElement6Type; set => _drawingElement6Type = value; }
        public int ColorforDrawingElement6 
        { 
            get => _colorforDrawingElement6;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement6 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement7Type { get => _drawingElement7Type; set => _drawingElement7Type = value; }
        public int ColorforDrawingElement7 
        { 
            get => _colorforDrawingElement7;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement7 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement8Type { get => _drawingElement8Type; set => _drawingElement8Type = value; }
        public int ColorforDrawingElement8 
        { 
            get => _colorforDrawingElement8;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement8 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement9Type { get => _drawingElement9Type; set => _drawingElement9Type = value; }
        public int ColorforDrawingElement9 
        { 
            get => _colorforDrawingElement9;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement9 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement10Type { get => _drawingElement10Type; set => _drawingElement10Type = value; }
        public int ColorforDrawingElement10 
        { 
            get => _colorforDrawingElement10;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement10 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement11Type { get => _drawingElement11Type; set => _drawingElement11Type = value; }
        public int ColorforDrawingElement11 
        { 
            get => _colorforDrawingElement11;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement11 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement12Type { get => _drawingElement12Type; set => _drawingElement12Type = value; }
        public int ColorforDrawingElement12 
        { 
            get => _colorforDrawingElement12;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement12 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement13Type { get => _drawingElement13Type; set => _drawingElement13Type = value; }
        public int ColorforDrawingElement13 
        { 
            get => _colorforDrawingElement13;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement13 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement14Type { get => _drawingElement14Type; set => _drawingElement14Type = value; }
        public int ColorforDrawingElement14 
        { 
            get => _colorforDrawingElement14;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement14 = value;

                }
            }
        }
        public DrawingElementTypeEnum? DrawingElement15Type { get => _drawingElement15Type; set => _drawingElement15Type = value; }
        public int ColorforDrawingElement15 
        { 
            get => _colorforDrawingElement15;
            set
            {
                if (value >= 0 && value <= 255)
                {
                    _colorforDrawingElement15 = value;

                }
            }
        }

        public OutputControlSurfaceColorScheme() { }

        private static List<OutputControlSurfaceColorScheme> list = new List<OutputControlSurfaceColorScheme>();

        public static void Add(OutputControlSurfaceColorScheme output)
        {
            list.Add(output);

            Errors er = new Errors();
            if (output.Name == null)
            {
                er.Class = "OutputControl:SurfaceColorScheme";
                er.Field = "Name";
                Errors.Add(er);
            }
        }

        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"OutputControl:SurfaceColorScheme,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].DrawingElement1Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 1 Type\n" +
                    ($"  {list[i].ColorforDrawingElement1}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 1\n" +
                    ($"  {list[i].DrawingElement2Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 2 Type\n" +
                    ($"  {list[i].ColorforDrawingElement2}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 2\n" +
                    ($"  {list[i].DrawingElement3Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 3 Type\n" +
                    ($"  {list[i].ColorforDrawingElement3}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 3\n" +
                    ($"  {list[i].DrawingElement4Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 4 Type\n" +
                    ($"  {list[i].ColorforDrawingElement4}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 4\n" +
                    ($"  {list[i].DrawingElement5Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 5 Type\n" +
                    ($"  {list[i].ColorforDrawingElement5}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 5\n" +
                    ($"  {list[i].DrawingElement6Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 6 Type\n" +
                    ($"  {list[i].ColorforDrawingElement6}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 6\n" +
                    ($"  {list[i].DrawingElement7Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 7 Type\n" +
                    ($"  {list[i].ColorforDrawingElement7}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 7\n" +
                    ($"  {list[i].DrawingElement8Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 8 Type\n" +
                    ($"  {list[i].ColorforDrawingElement8}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 8\n" +
                    ($"  {list[i].DrawingElement9Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 9 Type\n" +
                    ($"  {list[i].ColorforDrawingElement9}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 9\n" +
                    ($"  {list[i].DrawingElement10Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 10 Type\n" +
                    ($"  {list[i].ColorforDrawingElement10}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 10\n" +
                    ($"  {list[i].DrawingElement11Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 11 Type\n" +
                    ($"  {list[i].ColorforDrawingElement11}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 11\n" +
                    ($"  {list[i].DrawingElement12Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 12 Type\n" +
                    ($"  {list[i].ColorforDrawingElement12}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 12\n" +
                    ($"  {list[i].DrawingElement13Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 13 Type\n" +
                    ($"  {list[i].ColorforDrawingElement13}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 13\n" +
                    ($"  {list[i].DrawingElement14Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 14 Type\n" +
                    ($"  {list[i].ColorforDrawingElement14}" + ",").PadRight(27, ' ') + " !-Color for Drawing Element 14\n" +
                    ($"  {list[i].DrawingElement15Type}" + ",").PadRight(27, ' ') + " !-Drawing Element 15 Type\n" +
                    ($"  {list[i].ColorforDrawingElement15}" + ";").PadRight(27, ' ') + " !-Color for Drawing Element 15";
                    
            }
            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in OutputControlSurfaceColorScheme.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Get()
        {

            foreach (string line in OutputControlSurfaceColorScheme.Read())
            {
                Console.WriteLine(line);
            }


        }
        public static void Clear()
        {
            list.Clear();
        }
    }
}
