using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Captcha
{
    public class CapatchaService
    {
        private Random _random = new Random(Guid.NewGuid().GetHashCode());
        private readonly CaptchaOptions _options;
        private readonly CaptchaContext _context;

        public CapatchaService(IOptions<CaptchaOptions> options, CaptchaContext context)
        {
            _options = options.Value;
            _context = context;
        }

        private float RandomFloat(float min, float max)
        {
            return (float)((_random.NextDouble() * (max - min)) + min);
        }

        private Color RandomColor(int minBrightness, int maxBrightness)
        {
            var r = _random.Next(minBrightness, maxBrightness + 1);
            var g = _random.Next(minBrightness, maxBrightness + 1);
            var b = _random.Next(minBrightness, maxBrightness + 1);
            return Color.FromArgb(r, g, b);
        }

        public (string Text, string Image) GenerateCapatchaImage()
        {
            using var bitmap = new Bitmap(_options.ImageWidth, _options.ImageHeight);
            using var graph = Graphics.FromImage(bitmap);
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graph.Clear(RandomColor(_options.BackGroundMinBrightness,
             _options.BackGroundMaxBrightness));

            //隨機橢圓點
            for (int i = 0; i < _options.EllipseAmount; i++)
            {
                var ellipseWidth = RandomFloat(_options.EllipseMinWidth, _options.EllipseMaxWidth);
                var ellipseHeight = RandomFloat(_options.EllipseMinHeight, _options.EllipseMaxHeight);
                var x = RandomFloat(-ellipseWidth, _options.ImageWidth);
                var y = RandomFloat(-ellipseHeight, _options.ImageHeight);
                var color = RandomColor(_options.EllipseMinBrightness, _options.EllipseMaxBrightness);
                graph.FillEllipse(new SolidBrush(color), x, y, ellipseWidth, ellipseHeight);
            }

            //隨機曲線
            for (int i = 0; i < _options.ArcAmount; i++)
            {
                var arcWidth = RandomFloat(_options.ArcMinWidth, _options.ArcMaxWidth);
                var arcHeight = RandomFloat(_options.ArcMinHeight, _options.ArcMaxHeight);
                var arcX = RandomFloat(-arcWidth, _options.ImageWidth);
                var arcY = RandomFloat(-arcHeight, _options.ImageHeight);
                var startAngle = RandomFloat(0, 360);
                var sweepAngle = RandomFloat(_options.ArcMinSweepAngle, _options.ArcMaxSweepAngle);
                var color = RandomColor(_options.ArcMinBrightness, _options.ArcMaxBrightness);
                var penWidth = RandomFloat(_options.ArcMinPenWidth, _options.ArcMaxPenWidth);
                graph.DrawArc(new Pen(color, penWidth), arcX, arcY, arcWidth, arcHeight, startAngle, sweepAngle);
            }

            //隨機文字
            var text = "";
            for (int i = 0; i < _options.TextLength; i++)
            {
                var c = _options.CharSets[_random.Next(0, _options.CharSets.Length)];
                text += c;
            }

            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var charX = ((float)_options.ImageWidth / text.Length) * i + RandomFloat(-_options.CharShift, _options.CharShift);
                var charY = RandomFloat(-_options.CharShift, _options.CharShift);
                var rotate = RandomFloat(-_options.CharRotate, _options.CharRotate);
                graph.TranslateTransform(charX, charY);
                graph.RotateTransform(rotate);
                var fontSize = _random.Next(_options.FontMinSize, _options.FontMaxSize);
                var _font = new Font(_options.FontFamily, fontSize, FontStyle.Regular);
                var color = RandomColor(_options.FontMinBrightness, _options.FontMaxBrightness);
                graph.DrawString(c.ToString(), _font, new SolidBrush(color), 0, 0);
                graph.ResetTransform();
            }

            using var ms = new MemoryStream();

            bitmap.Save(ms, ImageFormat.Png);
            var image = Convert.ToBase64String(ms.ToArray());
            return (text, image);
        }

        public async Task<(int id, string image)> GenerateRecordAsync()
        {
            var (text, image) = GenerateCapatchaImage();

            var record = new CaptchaRecord
            {
                Text = text,
                CreatedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            _context.Records.Add(record);

            await _context.SaveChangesAsync();
            return (record.Id, image);
        }

        public async Task RemoveOldRecordsAsync()
        {
            var endTime = DateTimeOffset.UtcNow.AddMinutes(-5).ToUnixTimeSeconds();
            _context.Records.RemoveRange(_context.Records.Where(record => record.CreatedTime < endTime));
            await _context.SaveChangesAsync();
        }

        public async Task<bool> MatchRecord(int id, string text)
        {
            await RemoveOldRecordsAsync();
            var found = await _context.Records
                .SingleOrDefaultAsync(record => record.Id == id && record.Text == text);
            if (found == null) return false;
            _context.Records.Remove(found);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}