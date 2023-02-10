using MarsRover.Models;

namespace MarsRover.Components;

public class RouteView : MapView
{
    public LinkedList<Coordinate> Route
    {
        get => (LinkedList<Coordinate>)GetValue(RouteProperty);
        set => SetValue(RouteProperty, value);
    }

    public IEnumerable<Coordinate> ViableTargets
    {
        get => (IEnumerable<Coordinate>)GetValue(ViableTargetsProperty);
        set => SetValue(RouteProperty, value);
    }

    public static readonly BindableProperty RouteProperty =
        BindableProperty.Create(nameof(Route), typeof(LinkedList<Coordinate>), typeof(MapView));

    public static readonly BindableProperty ViableTargetsProperty =
        BindableProperty.Create(nameof(ViableTargets), typeof(IEnumerable<Coordinate>), typeof(MapView));

    public override void Draw(ICanvas canvas, RectF dirtyRect)
    {
        base.Draw(canvas, dirtyRect);

        var origin = new Coordinate(dirtyRect.Height / 2, dirtyRect.Width / 2);

        foreach (var target in ViableTargets)
        {
            var iRow = origin.X - (target.X - PositionOffset.X) * Zoom;
            var iCol = origin.Y + (target.Y - PositionOffset.Y) * Zoom;
            canvas.FillColor = Color.FromRgba(0, 255, 0, 100);
            canvas.FillCircle((float)iCol, (float)iRow, Zoom/2);
        }

        for (var curr = Route.First; curr.Next!= null; curr = curr.Next)
        {
            var from = curr.Value;
            var to = curr.Next.Value;

            var fromCol = origin.Y + (from.Y - PositionOffset.Y) * Zoom;
            var fromRow = origin.X - (from.X - PositionOffset.X) * Zoom;

            var toCol = origin.Y + (to.Y - PositionOffset.Y) * Zoom;
            var toRow = origin.X - (to.X - PositionOffset.X) * Zoom;

            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 3;
            canvas.DrawLine(fromCol, fromRow, toCol, toRow);
        }

    }
}
