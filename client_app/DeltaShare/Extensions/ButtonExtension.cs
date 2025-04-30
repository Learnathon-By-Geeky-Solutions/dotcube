namespace DeltaShare.Extensions;

public static class ButtonExtension
{
    public static void AddButtonTheme(this Button button)
    {

        LinearGradientBrush buttonGradient = new()
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1),
            GradientStops =
            [
                new GradientStop { Color = Color.FromArgb("#1f565e"), Offset = -2.0f },
                new GradientStop { Color = Color.FromArgb("#1f565e"), Offset = -1.0f },
                new GradientStop { Color = Color.FromArgb("#1a434e"), Offset = 0.0f },
                new GradientStop { Color = Color.FromArgb("#1a434e"), Offset = 1.0f },
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
        tapGesture.Tapped += async (s, e) =>
        {
            AnimateGradient(true, button);
            await button.ScaleTo(0.95, 125, Easing.SinOut);
            await button.ScaleTo(1.0, 125, Easing.SinOut);
            AnimateGradient(false, button);
            button.Command?.Execute(null);
        };

        button.GestureRecognizers.Add(pointerGesture);
#if ANDROID || IOS
        button.GestureRecognizers.Add(tapGesture);
#endif

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

        if (brush == null || brush.GradientStops.Count < 4)
            return;

        // Animate the gradient offsets
        Animation animation = new();

        if (!isHovered)
        {
            float currentVal1 = brush.GradientStops[0].Offset;
            float currentVal2 = brush.GradientStops[1].Offset;
            float currentVal3 = brush.GradientStops[2].Offset;
            float currentVal4 = brush.GradientStops[3].Offset;
            animation.Add(0, 1, new Animation(v => brush.GradientStops[0].Offset = (float)v, currentVal1, -2.0f));
            animation.Add(0, 1, new Animation(v => brush.GradientStops[1].Offset = (float)v, currentVal2, -1.0f));
            animation.Add(0, 1, new Animation(v => brush.GradientStops[2].Offset = (float)v, currentVal3, 0.0f));
            animation.Add(0, 1, new Animation(v => brush.GradientStops[3].Offset = (float)v, currentVal4, 1.0f));
        }
        else
        {
            float currentVal1 = brush.GradientStops[0].Offset;
            float currentVal2 = brush.GradientStops[1].Offset;
            float currentVal3 = brush.GradientStops[2].Offset;
            float currentVal4 = brush.GradientStops[3].Offset;
            animation.Add(0, 1, new Animation(v => brush.GradientStops[0].Offset = (float)v, currentVal1, 0.0));
            animation.Add(0, 1, new Animation(v => brush.GradientStops[1].Offset = (float)v, currentVal2, 1.0));
            animation.Add(0, 1, new Animation(v => brush.GradientStops[2].Offset = (float)v, currentVal3, 2.0));
            animation.Add(0, 1, new Animation(v => brush.GradientStops[3].Offset = (float)v, currentVal4, 3.0));
        }

        animation.Commit(button, $"GradientAnimation_{button.Id}", 16, 250, Easing.SinIn);
    }

}
