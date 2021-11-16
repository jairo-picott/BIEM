using System.Collections.Generic;
using System.IO;

namespace BIEM.Schedules
{
    public enum InterpolateTimestepEnum
    {
        Yes,
        No
    }
    public class ScheduleDayInterval
    {
        private string _name;
        private string _scheduleTypeLimitsName;
        private InterpolateTimestepEnum _interpolatetoTimestep = InterpolateTimestepEnum.No;
        private string _time1;
        private string _valueUntilTime1;
        private string _time2;
        private string _valueUntilTime2;
        private string _time3;
        private string _valueUntilTime3;
        private string _time4;
        private string _valueUntilTime4;
        private string _time5;
        private string _valueUntilTime5;
        private string _time6;
        private string _valueUntilTime6;
        private string _time7;
        private string _valueUntilTime7;
        private string _time8;
        private string _valueUntilTime8;
        private string _time9;
        private string _valueUntilTime9;
        private string _time10;
        private string _valueUntilTime10;
        private string _time11;
        private string _valueUntilTime11;
        private string _time12;
        private string _valueUntilTime12;
        private string _time13;
        private string _valueUntilTime13;
        private string _time14;
        private string _valueUntilTime14;
        private string _time15;
        private string _valueUntilTime15;
        private string _time16;
        private string _valueUntilTime16;
        private string _time17;
        private string _valueUntilTime17;
        private string _time18;
        private string _valueUntilTime18;
        private string _time19;
        private string _valueUntilTime19;
        private string _time20;
        private string _valueUntilTime20;
        private string _time21;
        private string _valueUntilTime21;
        private string _time22;
        private string _valueUntilTime22;
        private string _time23;
        private string _valueUntilTime23;
        private string _time24;
        private string _valueUntilTime24;
        private string _time25;
        private string _valueUntilTime25;
        private string _time26;
        private string _valueUntilTime26;
        private string _time27;
        private string _valueUntilTime27;
        private string _time28;
        private string _valueUntilTime28;
        private string _time29;
        private string _valueUntilTime29;
        private string _time30;
        private string _valueUntilTime30;
        private string _time31;
        private string _valueUntilTime31;
        private string _time32;
        private string _valueUntilTime32;
        private string _time33;
        private string _valueUntilTime33;
        private string _time34;
        private string _valueUntilTime34;
        private string _time35;
        private string _valueUntilTime35;
        private string _time36;
        private string _valueUntilTime36;
        private string _time37;
        private string _valueUntilTime37;
        private string _time38;
        private string _valueUntilTime38;
        private string _time39;
        private string _valueUntilTime39;
        private string _time40;
        private string _valueUntilTime40;
        private string _time41;
        private string _valueUntilTime41;
        private string _time42;
        private string _valueUntilTime42;
        private string _time43;
        private string _valueUntilTime43;
        private string _time44;
        private string _valueUntilTime44;
        private string _time45;
        private string _valueUntilTime45;
        private string _time46;
        private string _valueUntilTime46;
        private string _time47;
        private string _valueUntilTime47;
        private string _time48;
        private string _valueUntilTime48;
        private string _time49;
        private string _valueUntilTime49;
        private string _time50;
        private string _valueUntilTime50;

        public string Name { get => _name; set => _name = value; }
        public string ScheduleTypeLimitsName { get => _scheduleTypeLimitsName; set => _scheduleTypeLimitsName = value; }
        public InterpolateTimestepEnum InterpolatetoTimestep { get => _interpolatetoTimestep; set => _interpolatetoTimestep = value; }
        public string Time1 { get => _time1; set => _time1 = value; }
        public string ValueUntilTime1 { get => _valueUntilTime1; set => _valueUntilTime1 = value; }
        public string Time2 { get => _time2; set => _time2 = value; }
        public string ValueUntilTime2 { get => _valueUntilTime2; set => _valueUntilTime2 = value; }
        public string Time3 { get => _time3; set => _time3 = value; }
        public string ValueUntilTime3 { get => _valueUntilTime3; set => _valueUntilTime3 = value; }
        public string Time4 { get => _time4; set => _time4 = value; }
        public string ValueUntilTime4 { get => _valueUntilTime4; set => _valueUntilTime4 = value; }
        public string Time5 { get => _time5; set => _time5 = value; }
        public string ValueUntilTime5 { get => _valueUntilTime5; set => _valueUntilTime5 = value; }
        public string Time6 { get => _time6; set => _time6 = value; }
        public string ValueUntilTime6 { get => _valueUntilTime6; set => _valueUntilTime6 = value; }
        public string Time7 { get => _time7; set => _time7 = value; }
        public string ValueUntilTime7 { get => _valueUntilTime7; set => _valueUntilTime7 = value; }
        public string Time8 { get => _time8; set => _time8 = value; }
        public string ValueUntilTime8 { get => _valueUntilTime8; set => _valueUntilTime8 = value; }
        public string Time9 { get => _time9; set => _time9 = value; }
        public string ValueUntilTime9 { get => _valueUntilTime9; set => _valueUntilTime9 = value; }
        public string Time10 { get => _time10; set => _time10 = value; }
        public string ValueUntilTime10 { get => _valueUntilTime10; set => _valueUntilTime10 = value; }
        public string Time11 { get => _time11; set => _time11 = value; }
        public string ValueUntilTime11 { get => _valueUntilTime11; set => _valueUntilTime11 = value; }
        public string Time12 { get => _time12; set => _time12 = value; }
        public string ValueUntilTime12 { get => _valueUntilTime12; set => _valueUntilTime12 = value; }
        public string Time13 { get => _time13; set => _time13 = value; }
        public string ValueUntilTime13 { get => _valueUntilTime13; set => _valueUntilTime13 = value; }
        public string Time14 { get => _time14; set => _time14 = value; }
        public string ValueUntilTime14 { get => _valueUntilTime14; set => _valueUntilTime14 = value; }
        public string Time15 { get => _time15; set => _time15 = value; }
        public string ValueUntilTime15 { get => _valueUntilTime15; set => _valueUntilTime15 = value; }
        public string Time16 { get => _time16; set => _time16 = value; }
        public string ValueUntilTime16 { get => _valueUntilTime16; set => _valueUntilTime16 = value; }
        public string Time17 { get => _time17; set => _time17 = value; }
        public string ValueUntilTime17 { get => _valueUntilTime17; set => _valueUntilTime17 = value; }
        public string Time18 { get => _time18; set => _time18 = value; }
        public string ValueUntilTime18 { get => _valueUntilTime18; set => _valueUntilTime18 = value; }
        public string Time19 { get => _time19; set => _time19 = value; }
        public string ValueUntilTime19 { get => _valueUntilTime19; set => _valueUntilTime19 = value; }
        public string Time20 { get => _time20; set => _time20 = value; }
        public string ValueUntilTime20 { get => _valueUntilTime20; set => _valueUntilTime20 = value; }
        public string Time21 { get => _time21; set => _time21 = value; }
        public string ValueUntilTime21 { get => _valueUntilTime21; set => _valueUntilTime21 = value; }
        public string Time22 { get => _time22; set => _time22 = value; }
        public string ValueUntilTime22 { get => _valueUntilTime22; set => _valueUntilTime22 = value; }
        public string Time23 { get => _time23; set => _time23 = value; }
        public string ValueUntilTime23 { get => _valueUntilTime23; set => _valueUntilTime23 = value; }
        public string Time24 { get => _time24; set => _time24 = value; }
        public string ValueUntilTime24 { get => _valueUntilTime24; set => _valueUntilTime24 = value; }
        public string Time25 { get => _time25; set => _time25 = value; }
        public string ValueUntilTime25 { get => _valueUntilTime25; set => _valueUntilTime25 = value; }
        public string Time26 { get => _time26; set => _time26 = value; }
        public string ValueUntilTime26 { get => _valueUntilTime26; set => _valueUntilTime26 = value; }
        public string Time27 { get => _time27; set => _time27 = value; }
        public string ValueUntilTime27 { get => _valueUntilTime27; set => _valueUntilTime27 = value; }
        public string Time28 { get => _time28; set => _time28 = value; }
        public string ValueUntilTime28 { get => _valueUntilTime28; set => _valueUntilTime28 = value; }
        public string Time29 { get => _time29; set => _time29 = value; }
        public string ValueUntilTime29 { get => _valueUntilTime29; set => _valueUntilTime29 = value; }
        public string Time30 { get => _time30; set => _time30 = value; }
        public string ValueUntilTime30 { get => _valueUntilTime30; set => _valueUntilTime30 = value; }
        public string Time31 { get => _time31; set => _time31 = value; }
        public string ValueUntilTime31 { get => _valueUntilTime31; set => _valueUntilTime31 = value; }
        public string Time32 { get => _time32; set => _time32 = value; }
        public string ValueUntilTime32 { get => _valueUntilTime32; set => _valueUntilTime32 = value; }
        public string Time33 { get => _time33; set => _time33 = value; }
        public string ValueUntilTime33 { get => _valueUntilTime33; set => _valueUntilTime33 = value; }
        public string Time34 { get => _time34; set => _time34 = value; }
        public string ValueUntilTime34 { get => _valueUntilTime34; set => _valueUntilTime34 = value; }
        public string Time35 { get => _time35; set => _time35 = value; }
        public string ValueUntilTime35 { get => _valueUntilTime35; set => _valueUntilTime35 = value; }
        public string Time36 { get => _time36; set => _time36 = value; }
        public string ValueUntilTime36 { get => _valueUntilTime36; set => _valueUntilTime36 = value; }
        public string Time37 { get => _time37; set => _time37 = value; }
        public string ValueUntilTime37 { get => _valueUntilTime37; set => _valueUntilTime37 = value; }
        public string Time38 { get => _time38; set => _time38 = value; }
        public string ValueUntilTime38 { get => _valueUntilTime38; set => _valueUntilTime38 = value; }
        public string Time39 { get => _time39; set => _time39 = value; }
        public string ValueUntilTime39 { get => _valueUntilTime39; set => _valueUntilTime39 = value; }
        public string Time40 { get => _time40; set => _time40 = value; }
        public string ValueUntilTime40 { get => _valueUntilTime40; set => _valueUntilTime40 = value; }
        public string Time41 { get => _time41; set => _time41 = value; }
        public string ValueUntilTime41 { get => _valueUntilTime41; set => _valueUntilTime41 = value; }
        public string Time42 { get => _time42; set => _time42 = value; }
        public string ValueUntilTime42 { get => _valueUntilTime42; set => _valueUntilTime42 = value; }
        public string Time43 { get => _time43; set => _time43 = value; }
        public string ValueUntilTime43 { get => _valueUntilTime43; set => _valueUntilTime43 = value; }
        public string Time44 { get => _time44; set => _time44 = value; }
        public string ValueUntilTime44 { get => _valueUntilTime44; set => _valueUntilTime44 = value; }
        public string Time45 { get => _time45; set => _time45 = value; }
        public string ValueUntilTime45 { get => _valueUntilTime45; set => _valueUntilTime45 = value; }
        public string Time46 { get => _time46; set => _time46 = value; }
        public string ValueUntilTime46 { get => _valueUntilTime46; set => _valueUntilTime46 = value; }
        public string Time47 { get => _time47; set => _time47 = value; }
        public string ValueUntilTime47 { get => _valueUntilTime47; set => _valueUntilTime47 = value; }
        public string Time48 { get => _time48; set => _time48 = value; }
        public string ValueUntilTime48 { get => _valueUntilTime48; set => _valueUntilTime48 = value; }
        public string Time49 { get => _time49; set => _time49 = value; }
        public string ValueUntilTime49 { get => _valueUntilTime49; set => _valueUntilTime49 = value; }
        public string Time50 { get => _time50; set => _time50 = value; }
        public string ValueUntilTime50 { get => _valueUntilTime50; set => _valueUntilTime50 = value; }

        public ScheduleDayInterval() { }

        private static List<ScheduleDayInterval> list = new List<ScheduleDayInterval>();

        public static void Add(ScheduleDayInterval scheduleDayInterval)
        {
            list.Add(scheduleDayInterval);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Schedule:Day:Interval,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ScheduleTypeLimitsName}" + ",").PadRight(27, ' ') + " !-Zone or ZoneList Name\n" +
                    ($"  {list[i].InterpolatetoTimestep}" + ",").PadRight(27, ' ') + " !-Schedule Name\n" +
                    ($"  {list[i].Time1}" + ",").PadRight(27, ' ') + " !-Time 1\n" +
                    ($"  {list[i].ValueUntilTime1}" + ",").PadRight(27, ' ') + " !-Value Until Time 1\n" +
                    ($"  {list[i].Time2}" + ",").PadRight(27, ' ') + " !-Time 2\n" +
                    ($"  {list[i].ValueUntilTime2}" + ",").PadRight(27, ' ') + " !-Value Until Time 2\n" +
                    ($"  {list[i].Time3}" + ",").PadRight(27, ' ') + " !-Time 3\n" +
                    ($"  {list[i].ValueUntilTime3}" + ",").PadRight(27, ' ') + " !-Value Until Time 3\n" +
                    ($"  {list[i].Time4}" + ",").PadRight(27, ' ') + " !-Time 4\n" +
                    ($"  {list[i].ValueUntilTime4}" + ",").PadRight(27, ' ') + " !-Value Until Time 4\n" +
                    ($"  {list[i].Time5}" + ",").PadRight(27, ' ') + " !-Time 5\n" +
                    ($"  {list[i].ValueUntilTime5}" + ",").PadRight(27, ' ') + " !-Value Until Time 5\n" +
                    ($"  {list[i].Time6}" + ",").PadRight(27, ' ') + " !-Time 6\n" +
                    ($"  {list[i].ValueUntilTime6}" + ",").PadRight(27, ' ') + " !-Value Until Time 6\n" +
                    ($"  {list[i].Time7}" + ",").PadRight(27, ' ') + " !-Time 7\n" +
                    ($"  {list[i].ValueUntilTime7}" + ",").PadRight(27, ' ') + " !-Value Until Time 7\n" +
                    ($"  {list[i].Time8}" + ",").PadRight(27, ' ') + " !-Time 8\n" +
                    ($"  {list[i].ValueUntilTime8}" + ",").PadRight(27, ' ') + " !-Value Until Time 8\n" +
                    ($"  {list[i].Time9}" + ",").PadRight(27, ' ') + " !-Time 9\n" +
                    ($"  {list[i].ValueUntilTime9}" + ",").PadRight(27, ' ') + " !-Value Until Time 9\n" +
                    ($"  {list[i].Time10}" + ",").PadRight(27, ' ') + " !-Time 10\n" +
                    ($"  {list[i].ValueUntilTime10}" + ",").PadRight(27, ' ') + " !-Value Until Time 10\n" +
                    ($"  {list[i].Time11}" + ",").PadRight(27, ' ') + " !-Time 11\n" +
                    ($"  {list[i].ValueUntilTime11}" + ",").PadRight(27, ' ') + " !-Value Until Time 11\n" +
                    ($"  {list[i].Time12}" + ",").PadRight(27, ' ') + " !-Time 12\n" +
                    ($"  {list[i].ValueUntilTime12}" + ",").PadRight(27, ' ') + " !-Value Until Time 12\n" +
                    ($"  {list[i].Time13}" + ",").PadRight(27, ' ') + " !-Time 13\n" +
                    ($"  {list[i].ValueUntilTime13}" + ",").PadRight(27, ' ') + " !-Value Until Time 13\n" +
                    ($"  {list[i].Time14}" + ",").PadRight(27, ' ') + " !-Time 14\n" +
                    ($"  {list[i].ValueUntilTime14}" + ",").PadRight(27, ' ') + " !-Value Until Time 14\n" +
                    ($"  {list[i].Time15}" + ",").PadRight(27, ' ') + " !-Time 15\n" +
                    ($"  {list[i].ValueUntilTime15}" + ",").PadRight(27, ' ') + " !-Value Until Time 15\n" +
                    ($"  {list[i].Time16}" + ",").PadRight(27, ' ') + " !-Time 16\n" +
                    ($"  {list[i].ValueUntilTime16}" + ",").PadRight(27, ' ') + " !-Value Until Time 16\n" +
                    ($"  {list[i].Time17}" + ",").PadRight(27, ' ') + " !-Time 17\n" +
                    ($"  {list[i].ValueUntilTime17}" + ",").PadRight(27, ' ') + " !-Value Until Time 17\n" +
                    ($"  {list[i].Time18}" + ",").PadRight(27, ' ') + " !-Time 18\n" +
                    ($"  {list[i].ValueUntilTime18}" + ",").PadRight(27, ' ') + " !-Value Until Time 18\n" +
                    ($"  {list[i].Time19}" + ",").PadRight(27, ' ') + " !-Time 19\n" +
                    ($"  {list[i].ValueUntilTime19}" + ",").PadRight(27, ' ') + " !-Value Until Time 19\n" +
                    ($"  {list[i].Time20}" + ",").PadRight(27, ' ') + " !-Time 20\n" +
                    ($"  {list[i].ValueUntilTime20}" + ",").PadRight(27, ' ') + " !-Value Until Time 20\n" +
                    ($"  {list[i].Time21}" + ",").PadRight(27, ' ') + " !-Time 21\n" +
                    ($"  {list[i].ValueUntilTime21}" + ",").PadRight(27, ' ') + " !-Value Until Time 21\n" +
                    ($"  {list[i].Time22}" + ",").PadRight(27, ' ') + " !-Time 22\n" +
                    ($"  {list[i].ValueUntilTime22}" + ",").PadRight(27, ' ') + " !-Value Until Time 22\n" +
                    ($"  {list[i].Time23}" + ",").PadRight(27, ' ') + " !-Time 23\n" +
                    ($"  {list[i].ValueUntilTime23}" + ",").PadRight(27, ' ') + " !-Value Until Time 23\n" +
                    ($"  {list[i].Time24}" + ",").PadRight(27, ' ') + " !-Time 24\n" +
                    ($"  {list[i].ValueUntilTime24}" + ",").PadRight(27, ' ') + " !-Value Until Time 24\n" +
                    ($"  {list[i].Time25}" + ",").PadRight(27, ' ') + " !-Time 25\n" +
                    ($"  {list[i].ValueUntilTime25}" + ",").PadRight(27, ' ') + " !-Value Until Time 25\n" +
                    ($"  {list[i].Time26}" + ",").PadRight(27, ' ') + " !-Time 26\n" +
                    ($"  {list[i].ValueUntilTime26}" + ",").PadRight(27, ' ') + " !-Value Until Time 26\n" +
                    ($"  {list[i].Time27}" + ",").PadRight(27, ' ') + " !-Time 27\n" +
                    ($"  {list[i].ValueUntilTime27}" + ",").PadRight(27, ' ') + " !-Value Until Time 27\n" +
                    ($"  {list[i].Time28}" + ",").PadRight(27, ' ') + " !-Time 28\n" +
                    ($"  {list[i].ValueUntilTime28}" + ",").PadRight(27, ' ') + " !-Value Until Time 28\n" +
                    ($"  {list[i].Time29}" + ",").PadRight(27, ' ') + " !-Time 29\n" +
                    ($"  {list[i].ValueUntilTime29}" + ",").PadRight(27, ' ') + " !-Value Until Time 29\n" +
                    ($"  {list[i].Time30}" + ",").PadRight(27, ' ') + " !-Time 30\n" +
                    ($"  {list[i].ValueUntilTime30}" + ",").PadRight(27, ' ') + " !-Value Until Time 30\n" +
                    ($"  {list[i].Time31}" + ",").PadRight(27, ' ') + " !-Time 31\n" +
                    ($"  {list[i].ValueUntilTime31}" + ",").PadRight(27, ' ') + " !-Value Until Time 31\n" +
                    ($"  {list[i].Time32}" + ",").PadRight(27, ' ') + " !-Time 32\n" +
                    ($"  {list[i].ValueUntilTime32}" + ",").PadRight(27, ' ') + " !-Value Until Time 32\n" +
                    ($"  {list[i].Time33}" + ",").PadRight(27, ' ') + " !-Time 33\n" +
                    ($"  {list[i].ValueUntilTime33}" + ",").PadRight(27, ' ') + " !-Value Until Time 33\n" +
                    ($"  {list[i].Time34}" + ",").PadRight(27, ' ') + " !-Time 34\n" +
                    ($"  {list[i].ValueUntilTime34}" + ",").PadRight(27, ' ') + " !-Value Until Time 34\n" +
                    ($"  {list[i].Time35}" + ",").PadRight(27, ' ') + " !-Time 35\n" +
                    ($"  {list[i].ValueUntilTime35}" + ",").PadRight(27, ' ') + " !-Value Until Time 35\n" +
                    ($"  {list[i].Time36}" + ",").PadRight(27, ' ') + " !-Time 36\n" +
                    ($"  {list[i].ValueUntilTime36}" + ",").PadRight(27, ' ') + " !-Value Until Time 36\n" +
                    ($"  {list[i].Time37}" + ",").PadRight(27, ' ') + " !-Time 37\n" +
                    ($"  {list[i].ValueUntilTime37}" + ",").PadRight(27, ' ') + " !-Value Until Time 37\n" +
                    ($"  {list[i].Time38}" + ",").PadRight(27, ' ') + " !-Time 38\n" +
                    ($"  {list[i].ValueUntilTime38}" + ",").PadRight(27, ' ') + " !-Value Until Time 38\n" +
                    ($"  {list[i].Time39}" + ",").PadRight(27, ' ') + " !-Time 39\n" +
                    ($"  {list[i].ValueUntilTime39}" + ",").PadRight(27, ' ') + " !-Value Until Time 39\n" +
                    ($"  {list[i].Time40}" + ",").PadRight(27, ' ') + " !-Time 40\n" +
                    ($"  {list[i].ValueUntilTime40}" + ",").PadRight(27, ' ') + " !-Value Until Time 40\n" +
                    ($"  {list[i].Time41}" + ",").PadRight(27, ' ') + " !-Time 41\n" +
                    ($"  {list[i].ValueUntilTime41}" + ",").PadRight(27, ' ') + " !-Value Until Time 41\n" +
                    ($"  {list[i].Time42}" + ",").PadRight(27, ' ') + " !-Time 42\n" +
                    ($"  {list[i].ValueUntilTime42}" + ",").PadRight(27, ' ') + " !-Value Until Time 42\n" +
                    ($"  {list[i].Time43}" + ",").PadRight(27, ' ') + " !-Time 43\n" +
                    ($"  {list[i].ValueUntilTime43}" + ",").PadRight(27, ' ') + " !-Value Until Time 43\n" +
                    ($"  {list[i].Time44}" + ",").PadRight(27, ' ') + " !-Time 44\n" +
                    ($"  {list[i].ValueUntilTime44}" + ",").PadRight(27, ' ') + " !-Value Until Time 44\n" +
                    ($"  {list[i].Time45}" + ",").PadRight(27, ' ') + " !-Time 45\n" +
                    ($"  {list[i].ValueUntilTime45}" + ",").PadRight(27, ' ') + " !-Value Until Time 45\n" +
                    ($"  {list[i].Time46}" + ",").PadRight(27, ' ') + " !-Time 46\n" +
                    ($"  {list[i].ValueUntilTime46}" + ",").PadRight(27, ' ') + " !-Value Until Time 46\n" +
                    ($"  {list[i].Time47}" + ",").PadRight(27, ' ') + " !-Time 47\n" +
                    ($"  {list[i].ValueUntilTime47}" + ",").PadRight(27, ' ') + " !-Value Until Time 47\n" +
                    ($"  {list[i].Time48}" + ",").PadRight(27, ' ') + " !-Time 48\n" +
                    ($"  {list[i].ValueUntilTime48}" + ",").PadRight(27, ' ') + " !-Value Until Time 48\n" +
                    ($"  {list[i].Time49}" + ",").PadRight(27, ' ') + " !-Time 49\n" +
                    ($"  {list[i].ValueUntilTime49}" + ",").PadRight(27, ' ') + " !-Value Until Time 49\n" +
                    ($"  {list[i].Time50}" + ",").PadRight(27, ' ') + " !-Time 50\n" +
                    ($"  {list[i].ValueUntilTime50}" + ";").PadRight(27, ' ') + " !-Value Until Time50";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ScheduleDayInterval.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Clear()
        {
            list.Clear();
        }
    }
}
