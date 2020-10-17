using System.Drawing;

namespace Captcha
{
    public class CaptchaOptions
    {
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int BackGroundMinBrightness { get; set; }
        public int BackGroundMaxBrightness { get; set; }
        public int EllipseAmount { get; set; }
        public float EllipseMinWidth { get; set; }
        public float EllipseMaxWidth { get; set; }
        public float EllipseMinHeight { get; set; }
        public float EllipseMaxHeight { get; set; }
        public int EllipseMinBrightness { get; set; }
        public int EllipseMaxBrightness { get; set; }
        public int ArcAmount { get; set; }
        public float ArcMinWidth { get; set; }
        public float ArcMaxWidth { get; set; }
        public float ArcMinHeight { get; set; }
        public float ArcMaxHeight { get; set; }
        public float ArcMinSweepAngle { get; set; }
        public float ArcMaxSweepAngle { get; set; }
        public int ArcMinBrightness { get; set; }
        public int ArcMaxBrightness { get; set; }
        public float ArcMinPenWidth { get; set; }
        public float ArcMaxPenWidth { get; set; }
        public string CharSets { get; set; }
        public int TextLength { get; set; }
        public float CharShift { get; set; }
        public float CharRotate { get; set; }
        public string FontFamily { get; set; }
        public int FontMinSize {get;set;}
        public int FontMaxSize {get;set;}
        public int FontMinBrightness { get; set; }
        public int FontMaxBrightness { get; set; }
    }
}