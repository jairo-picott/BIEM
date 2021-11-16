using System;

namespace BIEM
{
    public class AnalyticalComfortZoneMethod
    {
        private double _TA;
        private double _TR;
        private double _VEL;
        private double _RH;
        private double _CLO;
        private double _MET;
        private double _WME;
        private double _FNPS;
        private double _PA;
        private double _ICL;
        private double _M;
        private double _W;
        private double _MW;
        private double _FCL;
        private double _HCF;
        private double _TAA;
        private double _TRA;
        private double _TCLA;
        private double _P1;
        private double _P2;
        private double _P3;
        private double _P4;
        private double _P5;
        private double _XN;
        private double _XF;
        private double _HCN;
        private double _HC;
        private double _TCL;
        private double _HL1;
        private double _HL2;
        private double _HL3;
        private double _HL4;
        private double _HL5;
        private double _HL6;
        private double _TS;


        public AnalyticalComfortZoneMethod() { }

        public double TA { get => _TA; set => _TA = value; }
        public double TR { get => _TR; set => _TR = value; }
        public double VEL { get => _VEL; set => _VEL = value; }
        public double RH { get => _RH; set => _RH = value; }
        public double CLO { get => _CLO; set => _CLO = value; }
        public double MET { get => _MET; set => _MET = value; }
        public double WME { get => _WME; set => _WME = value; }
        public double FNPS { get => _FNPS; set => _FNPS = value; }
        public double PA { get => _PA; set => _PA = value; }
        public double ICL { get => _ICL; set => _ICL = value; }
        public double M { get => _M; set => _M = value; }
        public double W { get => _W; set => _W = value; }
        public double MW { get => _MW; set => _MW = value; }
        public double FCL { get => _FCL; set => _FCL = value; }
        public double HCF { get => _HCF; set => _HCF = value; }
        public double TAA { get => _TAA; set => _TAA = value; }
        public double TRA { get => _TRA; set => _TRA = value; }
        public double TCLA { get => _TCLA; set => _TCLA = value; }
        public double P1 { get => _P1; set => _P1 = value; }
        public double P2 { get => _P2; set => _P2 = value; }
        public double P3 { get => _P3; set => _P3 = value; }
        public double P4 { get => _P4; set => _P4 = value; }
        public double P5 { get => _P5; set => _P5 = value; }
        public double XN { get => _XN; set => _XN = value; }
        public double XF { get => _XF; set => _XF = value; }
        public double HCN { get => _HCN; set => _HCN = value; }
        public double HC { get => _HC; set => _HC = value; }
        public double TCL { get => _TCL; set => _TCL = value; }
        public double HL1 { get => _HL1; set => _HL1 = value; }
        public double HL2 { get => _HL2; set => _HL2 = value; }
        public double HL3 { get => _HL3; set => _HL3 = value; }
        public double HL4 { get => _HL4; set => _HL4 = value; }
        public double HL5 { get => _HL5; set => _HL5 = value; }
        public double HL6 { get => _HL6; set => _HL6 = value; }
        public double TS { get => _TS; set => _TS = value; }

        public static double GetPMVPPD(double clo, double met,double wme, double ta, double tr, double vel, double rh, string PMVPPD)
        {
            AnalyticalComfortZoneMethod analytical = new AnalyticalComfortZoneMethod();
            analytical.TA = ta;// Convert.ToDouble(data[ZoneOutdoorAirDrybulbTemperature]);
            analytical.TR = tr; // Convert.ToDouble(data[ZoneThermostatAirTemperature]);
            analytical.VEL = vel; // Convert.ToDouble(data[ZoneOutdoorAirWindSpeed]);
            analytical.RH = rh; //Convert.ToDouble(data[ZoneAirRelativeHumidity]);
            analytical.CLO = clo;
            analytical.MET = met;
            analytical.WME = wme;
            analytical.FNPS = Math.Exp(16.6536 - 4030.183 / (analytical.TA + 235));
            analytical.PA = analytical.RH * 10 * analytical.FNPS;
            analytical.ICL = 0.155 * clo;
            analytical.M = analytical.MET * 58.15;
            analytical.W = analytical.WME * 58.15;
            bool outOfRange = false;
            //Console.WriteLine("Calculating for Elevated Air Speed");
            if (met < 1 || met > 2)
            {
                outOfRange = true;
                goto Out;
            }
            if (analytical.VEL > 0.2) 
            {
                
                double SET1 = StandardEffectiveTemperature.pierceSET(analytical.TA, analytical.TR, analytical.VEL, analytical.RH, analytical.MET, analytical.CLO, analytical.WME, 100);
                
                double delta = 0.1;
                bool again = true;
                while (again)
                {
                    
                    double deltai = delta;
                    double TADelta = analytical.TA - deltai ;
                    double TRDelta = analytical.TR - deltai ;
                    double SET2 = StandardEffectiveTemperature.pierceSET(TADelta, TRDelta, 0.1, analytical.RH, analytical.MET, analytical.CLO, analytical.WME, 100);
                    if (Math.Abs(SET1-SET2) < 0.1)
                    {
                        analytical.TA = TADelta;
                        analytical.TR = TRDelta;
                        analytical.VEL = 0.1;
                        again = false;
                        
                        break;
                    }

                    
                    delta = delta + 0.1;
                }
            }
            

            analytical.MW = analytical.M - analytical.W;

            if (analytical.ICL < 0.078)
            {
                analytical.FCL = 1 + 1.29 * analytical.ICL;
            }
            else
            {
                analytical.FCL = 1.05 + 0.645 * analytical.ICL;
            }
            analytical.HCF = 12.1 * Math.Sqrt(analytical.VEL);
            analytical.TAA = analytical.TA + 273;
            analytical.TRA = analytical.TR + 273;
            analytical.TCLA = analytical.TAA + (35.5 - analytical.TA) / (3.5 * (6.45 * analytical.ICL + 0.1));
            analytical.P1 = analytical.ICL * analytical.FCL;
            analytical.P2 = analytical.P1 * 3.96;
            analytical.P3 = analytical.P1 * 100;
            analytical.P4 = analytical.P1 * analytical.TAA;
            analytical.P5 = 308.7 - 0.028 * analytical.MW + analytical.P2 * Math.Pow((analytical.TRA / 100), 4);
            double _XN = analytical.TCLA / 100;
            double _XF = _XN;
            int n = 0;
            double EPS = 0.00015;
            analytical.XF = (_XF + _XN / 2);

            analytical.HCN = 2.38 * Math.Pow(Math.Abs(100 * analytical.XF - analytical.TAA), 0.25);

            if (analytical.HCF > analytical.HCN)
            {
                analytical.HC = analytical.HCF;
            }
            else
            {
                analytical.HC = analytical.HCN;
            }
            analytical.XN = (analytical.P5 + analytical.P4 * analytical.HC - analytical.P2 * Math.Pow(analytical.XF, 4)) / (100 + analytical.P3 * analytical.HC);
            while (Math.Abs(analytical.XN - analytical.XF) > EPS + 0.00002 || (Math.Abs(analytical.XN - analytical.XF) < EPS - 0.00002))
            {
                analytical.XF = ((_XF + _XN) / 2);

                analytical.HCN = 2.38 * Math.Pow(Math.Abs(100 * analytical.XF - analytical.TAA), 0.25);

                if (analytical.HCF > analytical.HCN)
                {
                    analytical.HC = analytical.HCF;
                }
                else
                {
                    analytical.HC = analytical.HCN;
                }
                analytical.XN = (analytical.P5 + analytical.P4 * analytical.HC - analytical.P2 * Math.Pow(analytical.XF, 4)) / (100 + analytical.P3 * analytical.HC);
                n++;
                if (Math.Abs(analytical.XN - analytical.XF) < 0.00015)
                {
                    break;
                }

                _XN = analytical.XN;
                _XF = _XN;
            }

            analytical.TCL = 100 * analytical.XN - 273;

            analytical.HL1 = 3.05 * 0.001 * (5733 - 6.99 * analytical.MW - analytical.PA);

            if (analytical.MW > 58.15)
            {
                analytical.HL2 = 0.42 * (analytical.MW - 58.15);
            }
            else
            {
                analytical.HL2 = 0;
            }
            analytical.HL3 = 1.7 * 0.00001 * analytical.M * (5867 - analytical.PA);
            analytical.HL4 = 0.0014 * analytical.M * (34 - analytical.TA);
            analytical.HL5 = 3.96 * analytical.FCL * (Math.Pow(analytical.XN, 4) - (Math.Pow((analytical.TRA / 100), 4)));
            analytical.HL6 = analytical.FCL * analytical.HC * (analytical.TCL - analytical.TA);

            analytical.TS = 0.303 * Math.Exp(-0.036 * analytical.M) + 0.028;
            double PMV = analytical.TS * (analytical.MW - analytical.HL1 - analytical.HL2 - analytical.HL3 - analytical.HL4 - analytical.HL5 - analytical.HL6);
            double PPD = 100 - 95 * Math.Exp(-0.03353 * Math.Pow(PMV, 4) - 0.2179 * Math.Pow(PMV, 2));
            switch (PMVPPD)
            {
                case "PMV":
                    return PMV;

                case "PPD":
                    return PPD;
                default:
            
                    break;
            }
            return 999;

            Out:
            
            if (outOfRange)
            {
                Console.WriteLine("Values are not into the limits of the ASHRAE Standard 55-2017");
                return 999;
            }
            return 999;
        }

        
    }
}
