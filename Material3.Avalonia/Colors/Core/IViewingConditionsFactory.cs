namespace Material3.Avalonia.Colors.Core;

/// <summary>
/// Factory for viewing conditions (computes all derived constants).
/// </summary>
public interface IViewingConditionsFactory
{
    IViewingConditions Create(XyzColor white, double la, double yb, SurroundPreset surround);
    IViewingConditions SrgbAverage { get; }
}