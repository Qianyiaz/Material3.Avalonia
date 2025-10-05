namespace Material3.Avalonia.ColorScience.Conversion;

/// <summary>
/// Hue quadrature & composition helpers for CAM16 Step 5.
/// Implements Table 3 anchors and Li et al. formulas.
/// </summary>
internal static class Cam16Hue
{
    // Table 3: unique hues (i = 1..5, with 5 wrapping to red again)
    //   i :   1 (Red)   2 (Yellow)   3 (Green)    4 (Blue)     5 (Red wrap)
    //  h_i: 20.14      90.00        164.25       237.53       380.14
    //  e_i:  0.8        0.7           1.0          1.2          0.8
    //  H_i:  0.0      100.0         200.0        300.0        400.0
    private static readonly double[] hAnchors = { 20.14, 90.00, 164.25, 237.53, 380.14 };
    private static readonly double[] eAnchors = {  0.80,  0.70,   1.00,   1.20,   0.80 };
    private static readonly double[] HAnchors = {  0.0,  100.0,  200.0,  300.0,  400.0 };
    private static readonly string[] names    = { "R", "Y", "G", "B", "R" };

    /// <summary>
    /// Calculates hue quadrature H (0..400) per CAM16 Step 5.
    /// Also returns the segment index i (1..4) such that h is between h_i and h_{i+1}.
    /// </summary>
    /// <param name="hDegrees">Hue angle h in degrees [0,360).</param>
    /// <param name="H">Hue quadrature in 0..400.</param>
    /// <param name="segmentIndex">Segment i in 1..4 where h lies between h_i and h_{i+1}.</param>
    public static void ComputeHueQuadrature(double hDegrees, out double H, out int segmentIndex)
    {
        // Make h' in the [h_i, h_{i+1}) interval by possibly adding 360°
        double h = hDegrees;
        // Find i so that h_i <= h' < h_{i+1}. Note that h_5 wraps to 380.14 (> 360).
        int i = 0; // 0-based working index (0..3)
        // If h < h1, we let h' = h + 360 and i=4→wrap; else find the place.
        if (h < hAnchors[0])
        {
            h += 360.0;
            i = 3; // between h4 and h5 (blue-red wrap)
        }
        else
        {
            // find largest i s.t. h_i <= h
            for (int k = 0; k < 4; k++)
            {
                if (h >= hAnchors[k] && h < hAnchors[k + 1])
                {
                    i = k;
                    break;
                }
            }
        }

        int iNext = i + 1;
        double hi   = hAnchors[i];
        double hi1  = hAnchors[iNext];
        double ei   = eAnchors[i];
        double ei1  = eAnchors[iNext];
        double Hi   = HAnchors[i];
        double Hi1  = HAnchors[iNext];

        // H = H_i + 100 * e_{i+1} * (h' - h_i) / ( e_{i+1}(h' - h_i) + e_i(h_{i+1} - h') )
        double dhL = h - hi;      // (h' - h_i)
        double dhR = hi1 - h;     // (h_{i+1} - h')
        double denom = (ei1 * dhL) + (ei * dhR);
        double frac  = denom != 0.0 ? (ei1 * dhL) / denom : 0.0;
        H = Hi + 100.0 * frac;

        // Return i as 1-based like in the paper (1..4)
        segmentIndex = i + 1;
    }

    /// <summary>
    /// Computes hue composition (Hc) as percentages of adjacent unique hue sectors.
    /// Returns the left and right unique hue names and integer percentages P_L / P_R summing to 100.
    /// Example: H between H3 and H4 → "G" and "B", e.g. 59G41B.
    /// </summary>
    public static void ComputeHueComposition(double H, int segmentIndex,
        out string leftUnique, out string rightUnique, out int PL, out int PR)
    {
        // H_i and H_{i+1} are 100 apart (0,100,…,400).
        // By definition: P_L = H_{i+1} - H;  P_R = H - H_i
        int i = segmentIndex - 1;        // 0-based
        double Hi  = HAnchors[i];
        double Hi1 = HAnchors[i + 1];

        double pL = Hi1 - H;
        double pR = H - Hi;
        // Round to integers as in the paper’s example
        PL = (int)Math.Round(pL, MidpointRounding.AwayFromZero);
        PR = (int)Math.Round(pR, MidpointRounding.AwayFromZero);

        leftUnique  = names[i];       // e.g. between G (i=2) and B (i=3) → left=G, right=B
        rightUnique = names[i + 1];
    }

    /// <summary>
    /// Eccentricity factor e_t used elsewhere in CAM16 (also Step 5).
    /// </summary>
    public static double ComputeEccentricity(double hDegrees)
    {
        // e_t = 1/4 * (cos(h*pi/180 + 2) + 3.8)
        // (NB: +2 is in radians; that’s the published form.)
        return 0.25 * (Math.Cos(hDegrees * Math.PI / 180.0 + 2.0) + 3.8);
    }
}