using MarsRover.Models;
using Microsoft.Maui.Animations;
using Microsoft.Maui.Graphics;
using System.Reflection;
using System.Resources;

namespace MarsRover.Components
{
    public class MapView : BindableObject, IDrawable
    {
        public Dictionary<long, Models.Cell> HighResolutionMap
        {
            get => (Dictionary<long, Models.Cell>)GetValue(HighResolutionMapProperty);
            set => SetValue(HighResolutionMapProperty, value);
        }

        public IEnumerable<LowResolutionMap> LowResolutionMap
        {
            get => (IEnumerable<LowResolutionMap>)GetValue(LowResolutionMapProperty);
            set => SetValue(LowResolutionMapProperty, value);
        }

        public float Zoom
        {
            get => (float)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        public Coordinate PositionOffset
        {
            get => (Coordinate)GetValue(PositionOffsetProperty);
            set => SetValue(PositionOffsetProperty, value);
        }

        public Coordinate PerseverancePosition
        {
            get => (Coordinate)GetValue(PerseverancePositionProperty);
            set => SetValue(PerseverancePositionProperty, value);   
        }

        private Color hot = Color.FromRgb(255, 0, 0);
        private Color cold = Color.FromRgb(0, 0, 255);

        public static readonly BindableProperty HighResolutionMapProperty =
            BindableProperty.Create(nameof(HighResolutionMap), typeof(Dictionary<long, Models.Cell>), typeof(MapView));

        public static readonly BindableProperty LowResolutionMapProperty =
            BindableProperty.Create(nameof(LowResolutionMap), typeof(IEnumerable<LowResolutionMap>), typeof(MapView));

        public static readonly BindableProperty ZoomProperty =
            BindableProperty.Create(nameof(Zoom), typeof(float), typeof(MapView));

        public static readonly BindableProperty PositionOffsetProperty = 
            BindableProperty.Create(nameof(PositionOffset), typeof(Coordinate), typeof(MapView));

        public static readonly BindableProperty PerseverancePositionProperty =
            BindableProperty.Create(nameof(PerseverancePosition), typeof(Coordinate), typeof(MapView));

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var origin = (dirtyRect.Width / 2, dirtyRect.Height / 2);

            foreach (var cell in LowResolutionMap)
            {
                var col = origin.Item2 + (cell.LowerLeftRow - PositionOffset.Y) * Zoom - (Zoom / 2);
                var row = origin.Item1 - (cell.LowerLeftColumn - PositionOffset.X) * Zoom - (Zoom * 10 - Zoom / 2);
                canvas.FillColor = cold.Lerp(hot, cell.ColorTemp);
                canvas.FillRectangle((float)col, (float)row, Zoom * 10, Zoom * 10);
            }

            foreach (var cell in HighResolutionMap)
            {
                var c = cell.Value;
                canvas.FillColor = cold.Lerp(hot, c.ColorTemp);
                var col = origin.Item1 - (c.Column - PositionOffset.X) * Zoom - (Zoom / 2);
                var row = origin.Item2 + (c.Row - PositionOffset.Y) * Zoom - (Zoom / 2);
                canvas.FillRectangle((float)row, (float)col, Zoom, Zoom);
            }
            
            canvas.FillColor = Colors.Black;
            var col1 = origin.Item1 - (PerseverancePosition.X - PositionOffset.X) * Zoom;
            var row1 = origin.Item2 + (PerseverancePosition.Y - PositionOffset.Y) * Zoom;
            canvas.FillCircle((float)row1, (float)col1, Zoom / 2);
        }
    }
}
