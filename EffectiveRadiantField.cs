using System;

namespace BIEM
{
    public class EffectiveRadiantField
    {
        public static double feff { get; set; }
        static int find_span(double[] arr, double x)
        {
            // for ordered array arr and value x, find the left index
            // of the closed interval that the value falls in.
            for (var i = 0; i < arr.Length - 1; i++)
            {
                if (x <= arr[i + 1] && x >= arr[i])
                {
                    return i;
                }
            }
            return -1;
        }
        static double get_fp(double alt,double sharp, string posture)
        {
            // This function calculates the projected sunlit fraction (fp)
            // given a seated or standing posture, a solar altitude, and a
            // solar horizontal angle relative to the person (SHARP). fp
            // values are taken from Thermal Comfort, Fanger 1970, Danish
            // Technical Press.
            // alt : altitude of sun in degrees [0, 90] (beta) Integer
            // sharp : sun’s horizontal angle relative to person
            // in degrees [0, 180] Integer
            double fp;
            double[] alt_range = new double[7];
            alt_range[0] = 0; alt_range[1] = 15; alt_range[2] = 30; alt_range[3] = 45; alt_range[4] = 60; alt_range[5] = 75; alt_range[6] = 90;
            double[] sharp_range = new double[13];
            sharp_range[0] = 0; sharp_range[1] = 15; sharp_range[2] = 30; sharp_range[3] = 45; sharp_range[4] = 60; sharp_range[5] = 75; sharp_range[6] = 90;
            sharp_range[7] = 105; sharp_range[8] = 120; sharp_range[9] = 135; sharp_range[10] = 150; sharp_range[11] = 165; sharp_range[12] = 180;
            
            var alt_i = find_span(alt_range, alt);
            var sharp_i = find_span(sharp_range, sharp);
            double[,] fp_table = new double[7, 13];
            if (posture == "standing")
            {
                fp_table[0, 0] = 0.35; fp_table[1, 0] = 0.35; fp_table[2, 0] = 0.314; fp_table[3, 0] = 0.258; fp_table[4, 0] = 0.206; fp_table[5, 0] = 0.144; fp_table[6, 0] = 0.082;
                fp_table[0, 1] = 0.34; fp_table[1, 1] = 0.42; fp_table[2, 1] = 0.31; fp_table[3, 1] = 0.252; fp_table[4, 1] = 0.2; fp_table[5, 1] = 0.14; fp_table[6, 1] = 0.082;
                fp_table[0, 2] = 0.33; fp_table[1, 2] = 0.33; fp_table[2, 2] = 0.3; fp_table[3, 2] = 0.244; fp_table[4, 2] = 0.19; fp_table[5, 2] = 0.132; fp_table[6, 2] = 0.082;
                fp_table[0, 3] = 0.31; fp_table[1, 3] = 0.31; fp_table[2, 3] = 0.275; fp_table[3, 3] = 0.228; fp_table[4, 3] = 0.175; fp_table[5, 3] = 0.124; fp_table[6, 3] = 0.082;
                fp_table[0, 4] = 0.283; fp_table[1, 4] = 0.283; fp_table[2, 4] = 0.251; fp_table[3, 4] = 0.208; fp_table[4, 4] = 0.16; fp_table[5, 4] = 0.114; fp_table[6, 4] = 0.082;
                fp_table[0, 5] = 0.252; fp_table[1, 5] = 0.252; fp_table[2, 5] = 0.228; fp_table[3, 5] = 0.188; fp_table[4, 5] = 0.15; fp_table[5, 5] = 0.108; fp_table[6, 5] = 0.082;
                fp_table[0, 6] = 0.23; fp_table[1, 6] = 0.23; fp_table[2, 6] = 0.214; fp_table[3, 6] = 0.18; fp_table[4, 6] = 0.148; fp_table[5, 6] = 0.108; fp_table[6, 6] = 0.082;
                fp_table[0, 7] = 0.242; fp_table[1, 7] = 0.242; fp_table[2, 7] = 0.222; fp_table[3, 7] = 0.18; fp_table[4, 7] = 0.153; fp_table[5, 7] = 0.112; fp_table[6, 7] = 0.082;
                fp_table[0, 8] = 0.274; fp_table[1, 8] = 0.274; fp_table[2, 8] = 0.245; fp_table[3, 8] = 0.203; fp_table[4, 8] = 0.165; fp_table[5, 8] = 0.116; fp_table[6, 8] = 0.082;
                fp_table[0, 9] = 0.304; fp_table[1, 9] = 0.304; fp_table[2, 9] = 0.27; fp_table[3, 9] = 0.22; fp_table[4, 9] = 0.174; fp_table[5, 9] = 0.121; fp_table[6, 9] = 0.082;
                fp_table[0, 10] = 0.328; fp_table[1, 10] = 0.328; fp_table[2, 10] = 0.29; fp_table[3, 10] = 0.234; fp_table[4, 10] = 0.183; fp_table[5, 10] = 0.125; fp_table[6, 10] = 0.082;
                fp_table[0, 11] = 0.344; fp_table[1, 11] = 0.344; fp_table[2, 11] = 0.304; fp_table[3, 11] = 0.244; fp_table[4, 11] = 0.19; fp_table[5, 11] = 0.128; fp_table[6, 11] = 0.082;
                fp_table[0, 12] = 0.347; fp_table[1, 12] = 0.347; fp_table[2, 12] = 0.308; fp_table[3, 12] = 0.246; fp_table[4, 12] = 0.191; fp_table[5, 12] = 0.128; fp_table[6, 12] = 0.082;
                
                
            }
            else if (posture == "seated")
            {
                fp_table[0, 0] = 0.29; fp_table[1, 0] = 0.324; fp_table[2, 0] = 0.305; fp_table[3, 0] = 0.303; fp_table[4, 0] = 0.262; fp_table[5, 0] = 0.224; fp_table[6, 0] = 0.177;
                fp_table[0, 1] = 0.292; fp_table[1, 1] = 0.328; fp_table[2, 1] = 0.294; fp_table[3, 1] = 0.288; fp_table[4, 1] = 0.268; fp_table[5, 1] = 0.227; fp_table[6, 1] = 0.177;
                fp_table[0, 2] = 0.288; fp_table[1, 2] = 0.332; fp_table[2, 2] = 0.298; fp_table[3, 2] = 0.29; fp_table[4, 2] = 0.264; fp_table[5, 2] = 0.222; fp_table[6, 2] = 0.177;
                fp_table[0, 3] = 0.274; fp_table[1, 3] = 0.326; fp_table[2, 3] = 0.294; fp_table[3, 3] = 0.289; fp_table[4, 3] = 0.252; fp_table[5, 3] = 0.214; fp_table[6, 3] = 0.177;
                fp_table[0, 4] = 0.254; fp_table[1, 4] = 0.308; fp_table[2, 4] = 0.28; fp_table[3, 4] = 0.276; fp_table[4, 4] = 0.241; fp_table[5, 4] = 0.202; fp_table[6, 4] = 0.177;
                fp_table[0, 5] = 0.23; fp_table[1, 5] = 0.282; fp_table[2, 5] = 0.262; fp_table[3, 5] = 0.26; fp_table[4, 5] = 0.233; fp_table[5, 5] = 0.193; fp_table[6, 5] = 0.177;
                fp_table[0, 6] = 0.216; fp_table[1, 6] = 0.26; fp_table[2, 6] = 0.248; fp_table[3, 6] = 0.244; fp_table[4, 6] = 0.22; fp_table[5, 6] = 0.186; fp_table[6, 6] = 0.177;
                fp_table[0, 7] = 0.234; fp_table[1, 7] = 0.258; fp_table[2, 7] = 0.236; fp_table[3, 7] = 0.227; fp_table[4, 7] = 0.208; fp_table[5, 7] = 0.18; fp_table[6, 7] = 0.177;
                fp_table[0, 8] = 0.262; fp_table[1, 8] = 0.26; fp_table[2, 8] = 0.224; fp_table[3, 8] = 0.208; fp_table[4, 8] = 0.196; fp_table[5, 8] = 0.176; fp_table[6, 8] = 0.177;
                fp_table[0, 9] = 0.28; fp_table[1, 9] = 0.26; fp_table[2, 9] = 0.21; fp_table[3, 9] = 0.192; fp_table[4, 9] = 0.184; fp_table[5, 9] = 0.17; fp_table[6, 9] = 0.177;
                fp_table[0, 10] = 0.298; fp_table[1, 10] = 0.256; fp_table[2, 10] = 0.194; fp_table[3, 10] = 0.174; fp_table[4, 10] = 0.168; fp_table[5, 10] = 0.168; fp_table[6, 10] = 0.177;
                fp_table[0, 11] = 0.306; fp_table[1, 11] = 0.25; fp_table[2, 11] = 0.18; fp_table[3, 11] = 0.156; fp_table[4, 11] = 0.156; fp_table[5, 11] = 0.166; fp_table[6, 11] = 0.177;
                fp_table[0, 12] = 0.3; fp_table[1, 12] = 0.24; fp_table[2, 12] = 0.168; fp_table[3, 12] = 0.152; fp_table[4, 12] = 0.152; fp_table[5, 12] = 0.164; fp_table[6, 12] = 0.177;



            }
            double fp11 = fp_table[alt_i, sharp_i];
            double fp12 = fp_table[alt_i + 1, sharp_i];
            double fp21 = fp_table[alt_i, sharp_i + 1];
            double fp22 = fp_table[alt_i + 1, sharp_i + 1];
            double sharp1 = sharp_range[sharp_i];
            double sharp2 = sharp_range[sharp_i + 1];
            double alt1 = alt_range[alt_i];
            double alt2 = alt_range[alt_i + 1];
            // bilinear interpolation
            fp = fp11 * (sharp2 - sharp) * (alt2 - alt);
            fp += fp21 * (sharp - sharp1) * (alt2 - alt);
            fp += fp12 * (sharp2 - sharp) * (alt - alt1);
            fp += fp22 * (sharp - sharp1) * (alt - alt1);
            fp /= (sharp2 - sharp1) * (alt2 - alt1);
            return fp;
        }
        public static string ERF(double alt,double sharp, string posture, double Idir, double tsol, double fsvv, double fbes, double asa)
        {
            // ERF function to estimate the impact of solar
            // radiation on occupant comfort
            // INPUTS:
            // alt : altitude of sun in degrees [0, 90]
            // sharp : sun’s horizontal angle relative to person
            // in degrees [0, 180]
            // posture: posture of occupant ('seated' or 'standing')
            // Idir : direct beam intensity (normal)
            // tsol: total solar transmittance (SC * 0.87f)
            // fsvv : sky vault view fraction : fraction of sky vault
            // in occupant's view [0, 1]
            // fbes : fraction body exposed to sun [0, 1]
            // asa : average shortwave
            // absorptivity of body [0, 1] (alpha_sw)
            double DEG_TO_RAD = 0.0174532925f;
            double hr = 6;
            double Idiff = 0.175f * Idir * Math.Sin(alt * DEG_TO_RAD);
            double fp = get_fp(alt, sharp, posture);
            
            if (posture == "standing")
            {
                feff = 0.725;
            }
            else if (posture == "seated")
            {
                feff = 0.696;
            }
            else
            {
                Console.WriteLine("Invalid posture (choose seated or seated)");
                return null;
            }
            double sw_abs = asa;
            double lw_abs = 0.95;
            double E_diff = 0.5f * feff * fsvv * tsol * Idiff;
            double E_direct = fp * feff * fbes * tsol * Idir;
            double E_refl = 0.5f * feff * fsvv * tsol * (Idir * Math.Sin(alt * DEG_TO_RAD) + Idiff) * 0.6f;
            double E_solar = E_diff + E_direct + E_refl;
            double ERF = E_solar * (sw_abs / lw_abs);
            double trsw = ERF / (hr * feff);
            return "ERF:" + ERF + ", trsw:" + trsw;
        }
    }
}
