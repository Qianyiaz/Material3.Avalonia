using Material3.Avalonia.Colors.Conversion;

namespace Material3.Avalonia.Colors.Core;

/// <summary>
/// Computes CAM16 viewing conditions from basic parameters.
/// </summary>
public sealed class ViewingConditionsFactory : IViewingConditionsFactory
{
    public IViewingConditions Create(XyzColor white, double la, double yb, SurroundPreset surround)
    {
        var (F, c, Nc) = surround switch
        {
            SurroundPreset.Average => (1.0, 0.69, 1.0),
            SurroundPreset.Dim     => (0.9, 0.59, 0.9),
            SurroundPreset.Dark    => (0.8, 0.525, 0.8),
            _ => (1.0, 0.69, 1.0)
        };

        // D = F[1 - 1/3.6 * exp(-(La+42)/92)], clamp to [0,1]
        var D = F * (1.0 - (1.0 / 3.6) * Math.Exp((-la - 42.0) / 92.0));
        D = Math.Clamp(D, 0.0, 1.0);

        // F_L (Li 2017)
        var k  = 1.0 / (5.0 * la + 1.0);
        var k4 = k * k; k4 *= k4;
        var t = 1 - k4;
        var FL = k4 * la + 0.1 * t * t * Math.Cbrt(5.0 * la);

        // n, Nbb, Ncb, z
        var n   = yb / white.Y;
        var Nbb = 0.725 / Math.Pow(n, 0.2);
        var Ncb = Nbb;
        var z   = 1.48 + Math.Sqrt(n);

        // White → RGB (M16), D-adapt, response compression, A_w
        var (Rw, Gw, Bw) = Cam16Math.M16_XyzToRgb(white.X, white.Y, white.Z);
        var DR = Cam16Math.DFactor(D, white.Y, Rw);
        var DG = Cam16Math.DFactor(D, white.Y, Gw);
        var DB = Cam16Math.DFactor(D, white.Y, Bw);
        var Rwc = DR * Rw; var Gwc = DG * Gw; var Bwc = DB * Bw;

        var Raw = Cam16Math.ResponseCompressionPositive(Rwc, FL);
        var Gaw = Cam16Math.ResponseCompressionPositive(Gwc, FL);
        var Baw = Cam16Math.ResponseCompressionPositive(Bwc, FL);
        var Aw = (2.0 * Raw + Gaw + 0.05 * Baw) * Nbb;

        return new ViewingConditions(white, la, yb, F, c, Nc, D, FL, n, Nbb, Ncb, z, (Raw, Gaw, Baw), Aw);
    }

    public IViewingConditions SrgbAverage =>
        Create(WhitePoint.D65, la: 64.0, yb: 20.0, surround: SurroundPreset.Average);
}