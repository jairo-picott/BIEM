using System;

namespace BIEM
{
    public class StandardEffectiveTemperature
    {
        
        public static double ICL { get; set; }
        public static double WCRIT { get; set; }
        public static double DRY { get; set; }
        public static double HFCS { get; set; }
        public static double ERES { get; set; }
        public static double CRES { get; set; }
        public static double SCR { get; set; }
        public static double SSK { get; set; }
        public static double TCSK { get; set; }
        public static double TCCR { get; set; }
        public static double DTSK { get; set; }
        public static double DTCR { get; set; }
        public static double TB { get; set; }
        public static double SKSIG { get; set; }
        public static double WARMS { get; set; }
        public static double COLDS { get; set; }
        public static double CRSIG { get; set; }
        public static double WARMC { get; set; }
        public static double COLDC { get; set; }
        public static double BDSIG { get; set; }
        public static double WARMB { get; set; }
        public static double COLDB { get; set; }
        public static double REGSW { get; set; }
        public static double ERSW { get; set; }
        public static double REA { get; set; }
        public static double RECL { get; set; }
        public static double EMAX { get; set; }
        public static double PRSW { get; set; }
        public static double PWET { get; set; }
        public static double EDIF { get; set; }
        public static double CHCS { get; set; }
        public static double SET { get; set; }
        public static double ERR1 { get; set; }
        public static double ERR2 { get; set; }

        public static double FindSaturatedVaporPressureTorr (double T)
        {
            //Helper function for pierceSET calculates Saturated Vapor Pressure (Torr) at Temperature T (°C)
            return Math.Exp(18.6686 - 4030.183 / (T + 235.0));
        }
        public static double pierceSET (double TA, double TR, double VEL, double RH, double MET, double CLO, double WME, double PATM)
        {
            //Input variables – TA (air temperature): °C, TR (mean radiant temperature): °C, VEL (air velocity): m/s,
            //RH (relative humidity): %, MET: met unit, CLO: clo unit, WME (external work): W/m2, PATM (atmospheric pressure): kPa
            double KCLO = 0.25;
            double BODYWEIGHT = 69.9; //kg
            double BODYSURFACEAREA = 1.8258; //m2
            double METFACTOR = 58.2; //W/m2
            double SBC = 0.000000056697; //Stefan-Boltzmann constant (W/m2K4)
            double CSW = 170.0;
            double CDIL = 120.0;
            double CSTR = 0.5;
            double LTIME = 60.0;
            double VaporPressure = RH * FindSaturatedVaporPressureTorr(TA) / 100.0;
            double AirVelocity = Math.Max(VEL, 0.1);
            double TempSkinNeutral = 33.7;
            double TempCoreNeutral = 36.8;
            double TempBodyNeutral = 36.49;
            double SkinBloodFlowNeutral = 6.3;
            double TempSkin = TempSkinNeutral; //Initial values
            double TempCore = TempCoreNeutral;
            double SkinBloodFlow = SkinBloodFlowNeutral;
            double MSHIV = 0.0;
            double ALFA = 0.1;
            double ESK = 0.1 * MET;
            double PressureInAtmospheres = PATM * 0.009869;
            double RCL = 0.155 * CLO;
            double FACL = 1.0 + 0.15 * CLO;
            double LR = 2.2 / PressureInAtmospheres; //Lewis Relation is 2.2f at sea level
            double RM = MET * METFACTOR;
            double M = MET * METFACTOR;
            if (CLO <= 0)
            {
                WCRIT = 0.38 * Math.Pow(AirVelocity, -0.29);
                ICL = 1.0;
            }
            else
            {
                WCRIT = 0.59 * Math.Pow(AirVelocity, -0.08);
                ICL = 0.45;
            }
            double CHC = 3.0 * Math.Pow(PressureInAtmospheres, 0.53);
            double CHCV = 8.600001 * Math.Pow((AirVelocity * PressureInAtmospheres), 0.53);
            CHC = Math.Max(CHC, CHCV);
            double CHR = 4.7;
            double CTC = CHR + CHC;
            double RA = 1.0 / (FACL * CTC); //Resistance of air layer to dry heat transfer
            double TOP = (CHR * TR + CHC * TA) / CTC;
            double TCL = TOP + (TempSkin - TOP) / (CTC * (RA + RCL));
            //TCL and CHR are solved iteratively using: H(Tsk – TOP) = CTC(TCL – TOP),
            //where H = 1/(RA + RCL) and RA = 1/FACL*CTC
            double TCL_OLD = TCL;
            bool flag = true;
            
            for (var TIM = 1; TIM <= LTIME; TIM++)
            { //Begin iteration
                do
                {
                    if (flag)
                    {
                        TCL_OLD = TCL;
                        CHR = 4.0 * SBC * Math.Pow(((TCL + TR) / 2.0 + 273.15), 3.0) * 0.72;
                        CTC = CHR + CHC;
                        RA = 1.0 / (FACL * CTC); //Resistance of air layer to dry heat transfer
                        TOP = (CHR * TR + CHC * TA) / CTC;
                    }
                    TCL = (RA * TempSkin + RCL * TOP) / (RA + RCL);
                    flag = true;
                } while (Math.Abs(TCL - TCL_OLD) > 0.01);
                flag = false;
                DRY = (TempSkin - TOP) / (RA + RCL);
                HFCS = (TempCore - TempSkin) *(5.28 + 1.163 * SkinBloodFlow);
                ERES = 0.0023 * M * (44.0 - VaporPressure);
                CRES = 0.0014 * M * (34.0 - TA);
                SCR = M - HFCS - ERES - CRES - WME;
                SSK = HFCS - DRY - ESK;
                TCSK = 0.97 * ALFA * BODYWEIGHT;
                TCCR = 0.97 * (1 - ALFA) *BODYWEIGHT;
                DTSK = (SSK * BODYSURFACEAREA) / (TCSK * 60.0f); //°C/min
                DTCR = SCR * BODYSURFACEAREA / (TCCR * 60.0f); //°C/min
                TempSkin = TempSkin + DTSK;
                TempCore = TempCore + DTCR;
                TB = ALFA * TempSkin + (1 - ALFA) *TempCore;
                SKSIG = TempSkin - TempSkinNeutral;
                if (SKSIG > 0)
                {
                    WARMS = SKSIG;
                    COLDS = 0.0;
                }
                else
                {
                    WARMS = 0.0;
                    COLDS = -1.0 * SKSIG;
                }
                CRSIG = (TempCore - TempCoreNeutral);
                if (CRSIG > 0)
                {
                    WARMC = CRSIG;
                    COLDC = 0.0;
                }
                else
                {
                    WARMC = 0.0;
                    COLDC = -1.0 * CRSIG;
                }
                BDSIG = TB - TempBodyNeutral;
                if (BDSIG > 0)
                {
                    WARMB = BDSIG;
                }
                else
                {
                    WARMB = 0;
                }
                SkinBloodFlow = (SkinBloodFlowNeutral + CDIL * WARMC) / (1 + CSTR * COLDS);
                SkinBloodFlow = Math.Max(0.5, Math.Min(90.0, SkinBloodFlow));
                REGSW = CSW * WARMB * Math.Exp(WARMS / 10.7);
                REGSW = Math.Min(REGSW, 500.0);
                ERSW = 0.68 * REGSW;
                REA = 1.0 / (LR * FACL * CHC); //Evaporative resistance of air layer
                RECL = RCL / (LR * ICL); //Evaporative resistance of clothing (icl=.45)
                EMAX = (FindSaturatedVaporPressureTorr(TempSkin) - VaporPressure)/ (REA + RECL);
                PRSW = ERSW / EMAX;
                PWET = 0.06 + 0.94 * PRSW;
                EDIF = PWET * EMAX - ERSW;
                ESK = ERSW + EDIF;
                if (PWET > WCRIT)
                {
                    PWET = WCRIT;
                    PRSW = WCRIT / 0.94;
                    ERSW = PRSW * EMAX;
                    EDIF = 0.06 * (1.0 - PRSW) *EMAX;
                    ESK = ERSW + EDIF;
                }
                if (EMAX < 0)
                {
                    EDIF = 0;
                    ERSW = 0;
                    PWET = WCRIT;
                    PRSW = WCRIT;
                    ESK = EMAX;
                }
                ESK = ERSW + EDIF;
                MSHIV = 19.4 * COLDS * COLDC;
                M = RM + MSHIV;
                ALFA = 0.0417737 + 0.7451833 / (SkinBloodFlow + 0.585417);
            } //End iteration
            double HSK = DRY + ESK; //Total heat loss from skin
            double RN = M - WME; //Net metabolic heat production
            double ECOMF = 0.42f * (RN - (1 * METFACTOR));
            if (ECOMF < 0.0) ECOMF = 0.0; //From Fanger
            EMAX = EMAX * WCRIT;
            double W = PWET;
            double PSSK = FindSaturatedVaporPressureTorr(TempSkin);
            double CHRS = CHR; //Definition of ASHRAE standard environment
            //... denoted “S”
            if (MET< 0.85) 
            {
                CHCS = 3.0f;
            } 
            else 
            {
                CHCS = 5.66 * Math.Pow(((MET - 0.85)), 0.39);
                CHCS = Math.Max(CHCS, 3.0);
            }       
            double CTCS = CHCS + CHRS;
            double RCLOS = 1.52 / ((MET - WME/METFACTOR) + 0.6944) - 0.1835;
            double RCLS = 0.155 * RCLOS;
            double FACLS = 1.0 + KCLO * RCLOS;
            double FCLS = 1.0 / (1.0 + 0.155 * FACLS * CTCS * RCLOS);
            double IMS = 0.45;
            double ICLS = IMS * CHCS / CTCS * (1 - FCLS)/(CHCS/CTCS - FCLS* IMS);
            double RAS = 1.0 / (FACLS * CTCS);
            double REAS = 1.0 / (LR * FACLS * CHCS);
            double RECLS = RCLS / (LR * ICLS);
            double HD_S = 1.0 / (RAS + RCLS);
            double HE_S = 1.0 / (REAS + RECLS);
            //SET determined using Newton’s iterative solution
            double DELTA = 0.0001;
            double dx = 100.0;
            
            double SET_OLD = TempSkin - HSK/HD_S; //Lower bound for SET
            while (Math.Abs(dx) > .01) 
            {
                ERR1 = (HSK - HD_S* (TempSkin - SET_OLD) - W* HE_S * (PSSK - 0.5 * FindSaturatedVaporPressureTorr(SET_OLD)));
                ERR2 = (HSK - HD_S* (TempSkin - (SET_OLD + DELTA)) - W* HE_S * (PSSK - 0.5 *FindSaturatedVaporPressureTorr((SET_OLD + DELTA))));
                SET = SET_OLD - DELTA* ERR1/(ERR2 - ERR1);
                dx = SET - SET_OLD;
                SET_OLD = SET;
            }
            return SET;
        }
    }
}
