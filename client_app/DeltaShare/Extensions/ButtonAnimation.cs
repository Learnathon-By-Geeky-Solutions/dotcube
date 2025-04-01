namespace DeltaShare.Extensions;

public static class ButtonExtensions
{
    public static void AddButtonTheme(this Button button)
    {

        LinearGradientBrush buttonGradient = new()
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1),
            GradientStops =
            [
                new GradientStop { Color = Color.FromArgb("#1400FE"), Offset = -1.0f },
        new GradientStop { Color = Color.FromArgb("#A100FE"), Offset = 0.0f },
        new GradientStop { Color = Color.FromArgb("#1400FE"), Offset = 1.0f },
    ]
        };

        button.Background = buttonGradient;

        PointerGestureRecognizer pointerGesture = new();
        pointerGesture.PointerEntered += (s, e) =>
        {
            button.ScaleTo(1.05, 250, Easing.SinOut);
            AnimateGradient(true, button);
        };
        pointerGesture.PointerExited += (s, e) =>
        {
            button.ScaleTo(1.0, 250, Easing.SinOut);
            AnimateGradient(false, button);
        };

        TapGestureRecognizer tapGesture = new();
        tapGesture.Tapped += (s, e) =>
        {
            button.Command?.Execute(null);
        };

        button.GestureRecognizers.Add(pointerGesture);
        button.GestureRecognizers.Add(tapGesture);

        button.Pressed += async (s, e) =>
        {
            await button.ScaleTo(0.95, 50, Easing.SinOut);
        };

        button.Released += async (s, e) =>
        {
            await button.ScaleTo(1.0, 50, Easing.SinOut);
        };
    }
    private static void AnimateGradient(bool isHovered, Button button)
    {
        LinearGradientBrush brush = (LinearGradientBrush)button.Background;

        if (brush == null || brush.GradientStops.Count < 3)
            return;

        // Animate the gradient offsets
        Animation animation = new();

        if (!isHovered)
        {
            float currentVal1 = brush.GradientStops[0].Offset;
            float currentVal2 = brush.GradientStops[1].Offset;
            float currentVal3 = brush.GradientStops[2].Offset;
            animation.Add(0, 1, new Animation(v => brush.GradientStops[0].Offset = (float)v, currentVal1, -1.0f));
            animation.Add(0, 1, new Animation(v => brush.GradientStops[1].Offset = (float)v, currentVal2, 0.0f));
            animation.Add(0, 1, new Animation(v => brush.GradientStops[2].Offset = (float)v, currentVal3, 1.0f));
        }
        else
        {
            float currentVal1 = brush.GradientStops[0].Offset;
            float currentVal2 = brush.GradientStops[1].Offset;
            float currentVal3 = brush.GradientStops[2].Offset;
            animation.Add(0, 1, new Animation(v => brush.GradientStops[0].Offset = (float)v, currentVal1, 0.0));
            animation.Add(0, 1, new Animation(v => brush.GradientStops[1].Offset = (float)v, currentVal2, 1.0));
            animation.Add(0, 1, new Animation(v => brush.GradientStops[2].Offset = (float)v, currentVal3, 2.0));
        }

        animation.Commit(button, $"GradientAnimation_{button.Id}", 16, 250, Easing.SinIn);
    }

}
