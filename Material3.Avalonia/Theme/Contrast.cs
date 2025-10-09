using System.ComponentModel;
using System.Globalization;

namespace Material3.Avalonia.Theme;

[TypeConverter(typeof(ContrastTypeConverter))]
public readonly struct Contrast : IEquatable<Contrast>
{
    public double Level { get; }
    
    public Contrast(double level)
    {
        Level = level;
    }

    public static Contrast Reduced => new(-1.0);
    public static Contrast Standard => new(0.0);
    public static Contrast Medium => new(0.5);
    public static Contrast High => new(1.0);
    
    public enum Preset { Reduced, Standard, Medium, High }
    
    public static implicit operator Contrast(double level) => new(level);
    public static implicit operator Contrast(Preset preset) => preset switch
    {
        Preset.Reduced => Reduced,
        Preset.Standard => Standard,
        Preset.Medium => Medium,
        Preset.High => High,
        _ => Standard
    };
    
    public static implicit operator double(Contrast contrast) => contrast.Level;
    
    public override string ToString() => Level.ToString(CultureInfo.InvariantCulture);
    public bool Equals(Contrast other) => Level.Equals(other.Level);
    public override bool Equals(object? obj) => obj is Contrast other && Equals(other);
    public override int GetHashCode() => Level.GetHashCode();
}