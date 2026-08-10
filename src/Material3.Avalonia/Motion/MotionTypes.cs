namespace Material3.Avalonia.Motion;

public enum MotionSpeed
{
    Fast,
    Default,
    Slow
}

public enum MotionStyle
{
    Spatial,
    Effects
}

public readonly record struct SpringToken(double Stiffness, double Damping);

public readonly record struct SpringParams(double Stiffness, double Damping, double InitialVelocity = 0);