namespace SmashScheduler.Web.Services;

public enum ThemeMode
{
    System,
    Light,
    Dark
}

public interface IThemeService
{
    ThemeMode CurrentMode { get; }
    bool IsDarkMode { get; }
    event Action? OnChange;
    void SetThemeMode(ThemeMode mode);
    void SetSystemPreference(bool prefersDark);
}

public class ThemeService : IThemeService
{
    private ThemeMode _currentMode = ThemeMode.System;
    private bool _systemPrefersDark;

    public event Action? OnChange;

    public ThemeMode CurrentMode => _currentMode;

    public bool IsDarkMode => _currentMode switch
    {
        ThemeMode.Dark => true,
        ThemeMode.Light => false,
        ThemeMode.System => _systemPrefersDark,
        _ => false
    };

    public void SetThemeMode(ThemeMode mode)
    {
        if (_currentMode == mode) return;
        _currentMode = mode;
        NotifyStateChanged();
    }

    public void SetSystemPreference(bool prefersDark)
    {
        if (_systemPrefersDark == prefersDark) return;
        _systemPrefersDark = prefersDark;
        if (_currentMode == ThemeMode.System)
        {
            NotifyStateChanged();
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
