using MudBlazor;

namespace SmashScheduler.Web.Themes;

public static class SmashSchedulerTheme
{
    public static MudTheme Theme => new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#2ECC71",
            PrimaryDarken = "#229954",
            PrimaryLighten = "#58D68D",
            PrimaryContrastText = "#FFFFFF",

            Secondary = "#3498DB",
            SecondaryDarken = "#2471A3",
            SecondaryLighten = "#5DADE2",
            SecondaryContrastText = "#FFFFFF",

            Tertiary = "#9B59B6",
            TertiaryDarken = "#7D3C98",
            TertiaryLighten = "#AF7AC5",
            TertiaryContrastText = "#FFFFFF",

            Info = "#3498DB",
            InfoDarken = "#2980B9",
            InfoLighten = "#5DADE2",
            InfoContrastText = "#FFFFFF",

            Success = "#27AE60",
            SuccessDarken = "#1E8449",
            SuccessLighten = "#52BE80",
            SuccessContrastText = "#FFFFFF",

            Warning = "#F39C12",
            WarningDarken = "#D68910",
            WarningLighten = "#F8C471",
            WarningContrastText = "#FFFFFF",

            Error = "#E74C3C",
            ErrorDarken = "#C0392B",
            ErrorLighten = "#EC7063",
            ErrorContrastText = "#FFFFFF",

            Dark = "#2C3E50",
            DarkDarken = "#1A252F",
            DarkLighten = "#34495E",
            DarkContrastText = "#FFFFFF",

            TextPrimary = "#2C3E50",
            TextSecondary = "#7F8C8D",
            TextDisabled = "#BDC3C7",

            Background = "#F8F9FA",
            BackgroundGray = "#ECF0F1",
            Surface = "#FFFFFF",

            DrawerBackground = "#FFFFFF",
            DrawerText = "#2C3E50",
            DrawerIcon = "#7F8C8D",

            AppbarBackground = "#FFFFFF",
            AppbarText = "#2C3E50",

            LinesDefault = "#E8ECEF",
            LinesInputs = "#BDC3C7",

            Divider = "#E8ECEF",
            DividerLight = "#F4F6F7",

            HoverOpacity = 0.06,
            RippleOpacity = 0.1,

            ActionDefault = "#7F8C8D",
            ActionDisabled = "#BDC3C7",
            ActionDisabledBackground = "#E8ECEF"
        },

        PaletteDark = new PaletteDark
        {
            Primary = "#2ECC71",
            PrimaryDarken = "#229954",
            PrimaryLighten = "#58D68D",
            PrimaryContrastText = "#FFFFFF",

            Secondary = "#3498DB",
            SecondaryDarken = "#2471A3",
            SecondaryLighten = "#5DADE2",
            SecondaryContrastText = "#FFFFFF",

            Tertiary = "#9B59B6",
            TertiaryDarken = "#7D3C98",
            TertiaryLighten = "#AF7AC5",
            TertiaryContrastText = "#FFFFFF",

            Info = "#3498DB",
            InfoDarken = "#2980B9",
            InfoLighten = "#5DADE2",
            InfoContrastText = "#FFFFFF",

            Success = "#27AE60",
            SuccessDarken = "#1E8449",
            SuccessLighten = "#52BE80",
            SuccessContrastText = "#FFFFFF",

            Warning = "#F39C12",
            WarningDarken = "#D68910",
            WarningLighten = "#F8C471",
            WarningContrastText = "#000000",

            Error = "#E74C3C",
            ErrorDarken = "#C0392B",
            ErrorLighten = "#EC7063",
            ErrorContrastText = "#FFFFFF",

            Dark = "#1A1A2E",
            DarkDarken = "#0F0F1A",
            DarkLighten = "#2D2D44",
            DarkContrastText = "#FFFFFF",

            TextPrimary = "#E8E8E8",
            TextSecondary = "#A0A0A0",
            TextDisabled = "#5C5C5C",

            Background = "#121212",
            BackgroundGray = "#1E1E1E",
            Surface = "#1E1E1E",

            DrawerBackground = "#1A1A1A",
            DrawerText = "#E8E8E8",
            DrawerIcon = "#A0A0A0",

            AppbarBackground = "#1A1A1A",
            AppbarText = "#E8E8E8",

            LinesDefault = "#2D2D2D",
            LinesInputs = "#3D3D3D",

            Divider = "#2D2D2D",
            DividerLight = "#252525",

            HoverOpacity = 0.1,
            RippleOpacity = 0.15,

            ActionDefault = "#A0A0A0",
            ActionDisabled = "#5C5C5C",
            ActionDisabledBackground = "#2D2D2D"
        },

        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "12px",
            DrawerWidthLeft = "260px",
            DrawerWidthRight = "260px"
        }
    };
}
