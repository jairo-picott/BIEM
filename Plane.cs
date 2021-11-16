

using System;

namespace BIEM
{
    public class Plane
    {
        private double _x;
        private double _y;
        private double _z;
        private double _c;

        public double X { get => _x; set => _x = value; }
        public double Y { get => _y; set => _y = value; }
        public double Z { get => _z; set => _z = value; }
        public double C { get => _c; set => _c = value; }

        public Plane() { }

        public static string PrintPlaneEquation(Plane plane)
        {
            string planeEQ = $"EQ: {plane.X}x + {plane.Y}y + {plane.Z}z+ {plane.C} = 0";
            return planeEQ;
        }
        public static Plane EquationOfPlane3D(double x1, double y1, double z1, double x2, double y2, double z2, double x3, double y3, double z3)
        {
            double[] vector1to2 = new double[3];
            vector1to2[0] = x2 - x1;
            vector1to2[1] = y2 - y1;
            vector1to2[2] = z2 - z1;


            double[] vector1to3 = new double[3];
            vector1to3[0] = x3 - x1;
            vector1to3[1] = y3 - y1;
            vector1to3[2] = z3 - z1;



            double[] CrossVector = new double[3];
            CrossVector[0] = vector1to2[1] * vector1to3[2] - vector1to2[2] * vector1to3[1];
            CrossVector[1] = -1*(vector1to2[0] * vector1to3[2] - vector1to2[2] * vector1to3[0]);
            CrossVector[2] = vector1to2[0] * vector1to3[1] - vector1to2[1] * vector1to3[0];

            //eq plane a(x-x0) + b(y-y0) + c(z-z0) = 0 a, b and c are the components of CrossVector.

            Plane plane = new Plane();
            plane.X = CrossVector[0];
            plane.Y = CrossVector[1];
            plane.Z = CrossVector[2];
            plane.C = -1 * (CrossVector[0] * x1) - 1 * (CrossVector[1] * y1)  -1 * (CrossVector[2] * z1);

            return plane;


        }

        public static bool PlanesParallel(Plane p1, Plane p2)
        {
            var p12 = DistanceBetween2PlanesTry(p1, p2);
            var p21 = DistanceBetween2PlanesTry(p2, p1);
            if (p12 == p21)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static double DistanceBetween2PlanesTry(Plane p1, Plane p2)
        {
            //getting point in plane 1 assuming y = 0 and z = 0....
            double x1 = -1 * p1.C / p1.X;

            //formula for distance between plane and point....
            double distance = Math.Abs(p2.X * x1 + p2.C) / Math.Sqrt(Math.Pow(p2.X, 2) + Math.Pow(p2.Y, 2) + Math.Pow(p2.Z, 2));


            return distance;

        }
        public static double DistanceBetween2Planes(Plane p1, Plane p2)
        {
            if (PlanesParallel(p1,p2))
            {
                double x1 = -1 * p1.C / p1.X;

                //formula for distance between plane and point....
                double distance = Math.Abs(p2.X * x1 + p2.C) / Math.Sqrt(Math.Pow(p2.X, 2) + Math.Pow(p2.Y, 2) + Math.Pow(p2.Z, 2));


                return distance;
            }
            else
            {
                return -1;
            }
            //getting point in plane 1 assuming y = 0 and z = 0....
            

        }

    }


}
