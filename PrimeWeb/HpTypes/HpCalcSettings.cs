using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeWeb.HpTypes;

[Serializable]
public struct HpCalcSettings
{
    //TODO: Type implementation - CALCSettings / CALCsettings
    public int angle_measure = 0;
    public int number_format = 0;
    public int precision = 5;
    public int digit_grouping = 0;
    public int entry = 2;
    public int integers = 3;
    public int bits = 32;
    public bool signed_int = false;
    public int complex = 0;
    public int language = 1;
    public int font_size = 2;
    public int theme = 1;
    public bool textbook = true;
    public bool menu = false;


    public HpCalcSettings(byte[] source)
	{
		DbgTools.PrintPacket(source,title: "HPSettings");
	}
   
}