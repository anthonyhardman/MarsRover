using MarsRover.Models;
using Microsoft.Maui.Animations;

namespace MarsRover.Components;

public class MapView : BindableObject, IDrawable
{
    public GameData GameData
    {
        get => (GameData)GetValue(GameDataProperty);
        set => SetValue(GameDataProperty, value);
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

    private float ZoomOffset => Zoom / 2;

    public static readonly BindableProperty GameDataProperty =
        BindableProperty.Create(nameof(GameData), typeof(GameData), typeof(MapView));

    public static readonly BindableProperty ZoomProperty =
        BindableProperty.Create(nameof(Zoom), typeof(float), typeof(MapView));

    public static readonly BindableProperty PositionOffsetProperty =
        BindableProperty.Create(nameof(PositionOffset), typeof(Coordinate), typeof(MapView));

    public static readonly BindableProperty LockSizeProperty =
        BindableProperty.Create(nameof(LockCursorSize), typeof(bool), typeof(MapView));

    public static readonly BindableProperty CursorSizeProperty =
        BindableProperty.Create(nameof(CursorSize), typeof(float), typeof(MapView));

    private Color hot = Color.FromRgb(255, 0, 0);
    private Color cold = Color.FromRgb(0, 0, 255);


    public MapView()
    {
        CursorSize = 25.0f;
    }

    public virtual void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var origin = new Coordinate(dirtyRect.Height / 2, dirtyRect.Width / 2);

        DrawLowResolutionMap(canvas, GameData.LowResolutionMap, origin);
        DrawHighResolutionMap(canvas, GameData.HighResolutionMap, origin);
        DrawTarget(canvas, GameData.Target, origin);
        DrawIngenuity(canvas, GameData.IngenuityPosition, origin, LockCursorSize ? (CursorSize / 2) : ZoomOffset);
        DrawPerseverance(canvas, GameData.PerseverancePosition, origin, GameData.Orientation, LockCursorSize ? CursorSize : Zoom);
    }

    private void DrawLowResolutionMap(ICanvas canvas, IEnumerable<LowResolutionMap> map, Coordinate origin)
    {
        foreach (var cell in map)
        {
            var col = origin.Y + (cell.LowerLeftRow - PositionOffset.Y) * Zoom - ZoomOffset;
            var row = origin.X - (cell.LowerLeftColumn - PositionOffset.X) * Zoom - (Zoom * 10 - ZoomOffset);
            canvas.FillColor = cold.Lerp(hot, cell.ColorTemp);
            canvas.FillRectangle((float)col, (float)row, Zoom * 10, Zoom * 10);
        }
    }

    private void DrawHighResolutionMap(ICanvas canvas, Dictionary<long, Models.Cell> map, Coordinate origin)
    {
        foreach (var cell in map)
        {
            var c = cell.Value;
            canvas.FillColor = cold.Lerp(hot, c.ColorTemp);
            var row = origin.X - (c.Column - PositionOffset.X) * Zoom - ZoomOffset;
            var col = origin.Y + (c.Row - PositionOffset.Y) * Zoom - ZoomOffset;
            canvas.FillRectangle((float)col, (float)row, Zoom, Zoom);
        }
    }

    private void DrawTarget(ICanvas canvas, Coordinate target, Coordinate origin)
    {
        var tCol = origin.Y + (target.Y - PositionOffset.Y) * Zoom;
        var tRow = origin.X - (target.X - PositionOffset.X) * Zoom;
        canvas.FillColor = Color.FromArgb("#D2403D");
        canvas.FillCircle((float)tCol, (float)tRow, LockCursorSize ? (CursorSize / 2) : ZoomOffset);
        canvas.FillColor = Colors.White;
        canvas.FillCircle((float)tCol, (float)tRow, LockCursorSize ? (CursorSize / 2) * (3.0f / 4) : ZoomOffset);
        canvas.FillColor = Color.FromArgb("#D2403D");
        canvas.FillCircle((float)tCol, (float)tRow, LockCursorSize ? (CursorSize / 2) * (2.0f / 4) : ZoomOffset);
        canvas.FillColor = Colors.White;
        canvas.FillCircle((float)tCol, (float)tRow, LockCursorSize ? (CursorSize / 2) * (1.0f / 4) : ZoomOffset);
    }

    private void DrawPerseverance(ICanvas canvas, Coordinate perseverance, Coordinate origin, Orientation orientation, float size)
    {
        var pRow = origin.X - (perseverance.X - PositionOffset.X) * Zoom;
        var pCol = origin.Y + (perseverance.Y - PositionOffset.Y) * Zoom;

        PathF path = new PathF();
        canvas.FillColor = Colors.Green;
        float originOffset = size / 2;

        var left = pCol - originOffset;
        var right = pCol + originOffset;
        var bottom = pRow + originOffset;
        var top = pRow - originOffset;
        var horizontalMiddle = pCol;
        var verticalMiddle = pRow;


        if (orientation == Orientation.North)
        {
            path.MoveTo(left, bottom);
            path.LineTo(right, bottom);
            path.LineTo(horizontalMiddle, top);
        }
        else if (orientation == Orientation.East)
        {
            path.MoveTo(left, bottom);
            path.LineTo(left, top);
            path.LineTo(right, verticalMiddle);
        }
        else if (orientation == Orientation.South)
        {
            path.MoveTo(left, top);
            path.LineTo(right, top);
            path.LineTo(horizontalMiddle, bottom);
        }
        else if (orientation == Orientation.West)
        {
            path.MoveTo(right, bottom);
            path.LineTo(right, top);
            path.LineTo(left, verticalMiddle);
        }
        canvas.FillPath(path);
    }

    private void DrawIngenuity(ICanvas canvas, Coordinate ingenuity, Coordinate origin, float size)
    {
        var iRow = origin.X - (ingenuity.X - PositionOffset.X) * Zoom;
        var iCol = origin.Y + (ingenuity.Y - PositionOffset.Y) * Zoom;
        canvas.FillColor = Colors.Grey;
        canvas.FillCircle((float)iCol, (float)iRow, size);
    }
}
