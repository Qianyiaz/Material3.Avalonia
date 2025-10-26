using Avalonia.Controls;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;

namespace Material3.Avalonia.Tokens.System;

internal static class ColorResourceWriter
{
    private static string BuildSysColorKey(string roleName)
        => $"MdSys{roleName}Color";

    private static void UpsertColor(IResourceDictionary dict, string key, uint argb)
        => dict[key] = global::Avalonia.Media.Color.FromUInt32(argb);

    private static void SetRoleColor(IResourceDictionary dict, string roleName, uint argb)
        => UpsertColor(dict, BuildSysColorKey(roleName), argb);

    public static void Rebuild(IResourceDictionary dict, DynamicScheme scheme)
    {
        SetRoleColor(dict, nameof(DynamicScheme.Background), scheme.Background);
        SetRoleColor(dict, nameof(DynamicScheme.OnBackground), scheme.OnBackground);
        SetRoleColor(dict, nameof(DynamicScheme.Surface), scheme.Surface);
        SetRoleColor(dict, nameof(DynamicScheme.SurfaceDim), scheme.SurfaceDim);
        SetRoleColor(dict, nameof(DynamicScheme.SurfaceBright), scheme.SurfaceBright);
        SetRoleColor(dict, nameof(DynamicScheme.SurfaceContainerLowest), scheme.SurfaceContainerLowest);
        SetRoleColor(dict, nameof(DynamicScheme.SurfaceContainerLow), scheme.SurfaceContainerLow);
        SetRoleColor(dict, nameof(DynamicScheme.SurfaceContainer), scheme.SurfaceContainer);
        SetRoleColor(dict, nameof(DynamicScheme.SurfaceContainerHigh), scheme.SurfaceContainerHigh);
        SetRoleColor(dict, nameof(DynamicScheme.SurfaceContainerHighest), scheme.SurfaceContainerHighest);
        SetRoleColor(dict, nameof(DynamicScheme.OnSurface), scheme.OnSurface);
        SetRoleColor(dict, nameof(DynamicScheme.SurfaceVariant), scheme.SurfaceVariant);
        SetRoleColor(dict, nameof(DynamicScheme.OnSurfaceVariant), scheme.OnSurfaceVariant);
        SetRoleColor(dict, nameof(DynamicScheme.InverseSurface), scheme.InverseSurface);
        SetRoleColor(dict, nameof(DynamicScheme.InverseOnSurface), scheme.InverseOnSurface);
        SetRoleColor(dict, nameof(DynamicScheme.Outline), scheme.Outline);
        SetRoleColor(dict, nameof(DynamicScheme.OutlineVariant), scheme.OutlineVariant);
        SetRoleColor(dict, nameof(DynamicScheme.Shadow), scheme.Shadow);
        SetRoleColor(dict, nameof(DynamicScheme.Scrim), scheme.Scrim);
        SetRoleColor(dict, nameof(DynamicScheme.SurfaceTint), scheme.SurfaceTint);

        SetRoleColor(dict, nameof(DynamicScheme.Primary), scheme.Primary);
        SetRoleColor(dict, nameof(DynamicScheme.OnPrimary), scheme.OnPrimary);
        SetRoleColor(dict, nameof(DynamicScheme.PrimaryContainer), scheme.PrimaryContainer);
        SetRoleColor(dict, nameof(DynamicScheme.OnPrimaryContainer), scheme.OnPrimaryContainer);
        SetRoleColor(dict, nameof(DynamicScheme.InversePrimary), scheme.InversePrimary);

        SetRoleColor(dict, nameof(DynamicScheme.Secondary), scheme.Secondary);
        SetRoleColor(dict, nameof(DynamicScheme.OnSecondary), scheme.OnSecondary);
        SetRoleColor(dict, nameof(DynamicScheme.SecondaryContainer), scheme.SecondaryContainer);
        SetRoleColor(dict, nameof(DynamicScheme.OnSecondaryContainer), scheme.OnSecondaryContainer);

        SetRoleColor(dict, nameof(DynamicScheme.Tertiary), scheme.Tertiary);
        SetRoleColor(dict, nameof(DynamicScheme.OnTertiary), scheme.OnTertiary);
        SetRoleColor(dict, nameof(DynamicScheme.TertiaryContainer), scheme.TertiaryContainer);
        SetRoleColor(dict, nameof(DynamicScheme.OnTertiaryContainer), scheme.OnTertiaryContainer);

        SetRoleColor(dict, nameof(DynamicScheme.Error), scheme.Error);
        SetRoleColor(dict, nameof(DynamicScheme.OnError), scheme.OnError);
        SetRoleColor(dict, nameof(DynamicScheme.ErrorContainer), scheme.ErrorContainer);
        SetRoleColor(dict, nameof(DynamicScheme.OnErrorContainer), scheme.OnErrorContainer);

        SetRoleColor(dict, nameof(DynamicScheme.Warning), scheme.Warning);
        SetRoleColor(dict, nameof(DynamicScheme.OnWarning), scheme.OnWarning);
        SetRoleColor(dict, nameof(DynamicScheme.WarningContainer), scheme.WarningContainer);
        SetRoleColor(dict, nameof(DynamicScheme.OnWarningContainer), scheme.OnWarningContainer);

        SetRoleColor(dict, nameof(DynamicScheme.Info), scheme.Info);
        SetRoleColor(dict, nameof(DynamicScheme.OnInfo), scheme.OnInfo);
        SetRoleColor(dict, nameof(DynamicScheme.InfoContainer), scheme.InfoContainer);
        SetRoleColor(dict, nameof(DynamicScheme.OnInfoContainer), scheme.OnInfoContainer);

        SetRoleColor(dict, nameof(DynamicScheme.Success), scheme.Success);
        SetRoleColor(dict, nameof(DynamicScheme.OnSuccess), scheme.OnSuccess);
        SetRoleColor(dict, nameof(DynamicScheme.SuccessContainer), scheme.SuccessContainer);
        SetRoleColor(dict, nameof(DynamicScheme.OnSuccessContainer), scheme.OnSuccessContainer);

        SetRoleColor(dict, nameof(DynamicScheme.PrimaryFixed), scheme.PrimaryFixed);
        SetRoleColor(dict, nameof(DynamicScheme.PrimaryFixedDim), scheme.PrimaryFixedDim);
        SetRoleColor(dict, nameof(DynamicScheme.OnPrimaryFixed), scheme.OnPrimaryFixed);
        SetRoleColor(dict, nameof(DynamicScheme.OnPrimaryFixedVariant), scheme.OnPrimaryFixedVariant);

        SetRoleColor(dict, nameof(DynamicScheme.SecondaryFixed), scheme.SecondaryFixed);
        SetRoleColor(dict, nameof(DynamicScheme.SecondaryFixedDim), scheme.SecondaryFixedDim);
        SetRoleColor(dict, nameof(DynamicScheme.OnSecondaryFixed), scheme.OnSecondaryFixed);
        SetRoleColor(dict, nameof(DynamicScheme.OnSecondaryFixedVariant), scheme.OnSecondaryFixedVariant);

        SetRoleColor(dict, nameof(DynamicScheme.TertiaryFixed), scheme.TertiaryFixed);
        SetRoleColor(dict, nameof(DynamicScheme.TertiaryFixedDim), scheme.TertiaryFixedDim);
        SetRoleColor(dict, nameof(DynamicScheme.OnTertiaryFixed), scheme.OnTertiaryFixed);
        SetRoleColor(dict, nameof(DynamicScheme.OnTertiaryFixedVariant), scheme.OnTertiaryFixedVariant);
    }
}