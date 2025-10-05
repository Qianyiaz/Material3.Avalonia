using Material3.Avalonia.ColorScience.Models;

namespace Material3.Avalonia.ColorScience.Viewing;

/// <summary>
/// Factory for viewing conditions (computes all derived constants).
/// </summary>
public interface IViewingConditionsFactory
{
    IViewingConditions Create(XyzColor white, double la, double yb, SurroundPreset surround);
    IViewingConditions SrgbAverage { get; }
}