namespace ARKitDemo.Controls;

public class ARView : View
{
    public static readonly BindableProperty IsSessionRunningProperty =
        BindableProperty.Create(nameof(IsSessionRunning), typeof(bool), typeof(ARView), false);

    public bool IsSessionRunning
    {
        get => (bool)GetValue(IsSessionRunningProperty);
        set => SetValue(IsSessionRunningProperty, value);
    }
}