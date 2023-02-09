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

        public string PerseveranceOrientation
        {
            get => (string)GetValue(PerseveranceOrientationProperty);
            set => SetValue(PerseveranceOrientationProperty, value);
        }

        public Coordinate IngenuityPosition
        {
            get => (Coordinate)GetValue(IngenuityPositionProperty);
            set => SetValue(IngenuityPositionProperty, value);
        }

        public bool LockCursorSize
        {
            get => (bool)GetValue(LockSizeProperty);
            set => SetValue(LockSizeProperty, value);
        }

        public float CursorSize
        {
            get => (float)GetValue(CursorSizeProperty);
            set => SetValue(CursorSizeProperty, value);
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

        public static readonly BindableProperty PerseveranceOrientationProperty =
            BindableProperty.Create(nameof(PerseveranceOrientation), typeof(string), typeof(MapView));

        public static readonly BindableProperty LockSizeProperty =
            BindableProperty.Create(nameof(LockCursorSize), typeof(bool), typeof(MapView));

        public static readonly BindableProperty CursorSizeProperty =
            BindableProperty.Create(nameof(CursorSize), typeof(float), typeof(MapView));

        public static readonly BindableProperty IngenuityPositionProperty =
            BindableProperty.Create(nameof(IngenuityPosition), typeof(Coordinate), typeof(MapView));

        public MapView()
        {
            CursorSize = 25.0f;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var origin = (dirtyRect.Width / 2, dirtyRect.Height / 2);
            var zoomOffset = Zoom / 2;

            foreach (var cell in LowResolutionMap)
            {
                var col = origin.Item2 + (cell.LowerLeftRow - PositionOffset.Y) * Zoom - zoomOffset;
                var row = origin.Item1 - (cell.LowerLeftColumn - PositionOffset.X) * Zoom - (Zoom * 10 - zoomOffset);
                canvas.FillColor = cold.Lerp(hot, cell.ColorTemp);
                canvas.FillRectangle((float)col, (float)row, Zoom * 10, Zoom * 10);
            }

            foreach (var cell in HighResolutionMap)
            {
                var c = cell.Value;
                canvas.FillColor = cold.Lerp(hot, c.ColorTemp);
                var row = origin.Item1 - (c.Column - PositionOffset.X) * Zoom - zoomOffset;
                var col = origin.Item2 + (c.Row - PositionOffset.Y) * Zoom - zoomOffset;
                canvas.FillRectangle((float)col, (float)row, Zoom, Zoom);
            }

            var pRow = origin.Item1 - (PerseverancePosition.X - PositionOffset.X) * Zoom;
            var pCol = origin.Item2 + (PerseverancePosition.Y - PositionOffset.Y) * Zoom;
            DrawPerseverance(canvas, LockCursorSize ? CursorSize : Zoom, (float)pRow, (float)pCol);

            var iRow = origin.Item1 - (IngenuityPosition.X - PositionOffset.X) * Zoom;
            var iCol = origin.Item2 + (IngenuityPosition.Y - PositionOffset.Y) * Zoom;
            canvas.FillColor = Colors.Yellow;
            canvas.FillCircle((float)iCol, (float)iRow, LockCursorSize ? (CursorSize / 2) : zoomOffset);
        }

        private void DrawPerseverance(ICanvas canvas, float size, float pRow, float pCol)
        {
            PathF path = new PathF();
            canvas.FillColor = Colors.Green;
            float originOffset = size / 2;

            float left = pCol - originOffset;
            float right = pCol + originOffset;
            float bottom = pRow + originOffset;
            float top = pRow - originOffset;
            float horizontalMiddle = pCol;
            float verticalMiddle = pRow;    


            if (PerseveranceOrientation == "North")
            {
                path.MoveTo(left, bottom);
                path.LineTo(right, bottom);
                path.LineTo(horizontalMiddle, top);
            }
            else if (PerseveranceOrientation == "East")
            {
                path.MoveTo(left, bottom);
                path.LineTo(left, top);
                path.LineTo(right, verticalMiddle);
            }
            else if (PerseveranceOrientation == "South")
            {
                path.MoveTo(left, top);
                path.LineTo(right, top);
                path.LineTo(horizontalMiddle, bottom);
            }
            else if (PerseveranceOrientation == "West")
            {
                path.MoveTo(right, bottom);
                path.LineTo(right, top);
                path.LineTo(left, verticalMiddle);
            }
            canvas.FillPath(path);
        }
    }
}
