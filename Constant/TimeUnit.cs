
namespace Constant
{
    public enum TimeScale : ushort
    {
        Scale1Second,
        Scale10Seconds,
        Scale1Minute,
        Scale10Minutes,
        Scale1Hour,
        Scale4Hours,
        Scale1Day,
    }

    //10000000 Ticks = 1 second
    public enum TimeUnit : ulong 
    {
        Unit1Senond = 10000000,
        Unit10Senonds = 100000000,
        Unit1Minute = 600000000,
        Unit10Minutes = 6000000000,
        Unit1Hour = 36000000000,
        Unit4Hours = 144000000000,
        Unit1Day = 864000000000,
    }

    //Pixel
    public enum ScaleUnit : ushort
    {
        Unit1Senond = 25,//20
        Unit10Senonds = 50,//40
        Unit1Minute = 125,//100
        Unit10Minutes = 300, 
        Unit1Hour = 1000, //600
        Unit4Hours = 160,
        Unit1Day = 800, 
    }

    public enum TicksPerPixel : uint
    {
        Unit1Senond = 400000, //10000000 / 25 //400000
        Unit10Senonds = 2000000, //100000000 / 50 //2500000
        Unit1Minute =   4800000, //600000000 / 125 //6000000
        Unit10Minutes = 20000000, //6000000000 / 300
        Unit1Hour = 36000000, //36000000000 / 1000 //60000000
        Unit4Hours = 900000000, //144000000000 / 160 
        Unit1Day = 1080000000, //864000000000 / 800 
    }
}
