﻿using FSH.BlazorWebAssembly.Client.Infrastructure.Preference;
using FSH.BlazorWebAssembly.Client.Infrastructure.Theme;
using Microsoft.AspNetCore.Components;

namespace FSH.BlazorWebAssembly.Client.Components.ThemeManager
{
    public partial class ThemeDrawer
    {

        [EditorRequired] [Parameter] public bool ThemeDrawerOpen { get; set; }
        [EditorRequired] [Parameter] public EventCallback<bool> ThemeDrawerOpenChanged { get; set; }
        [EditorRequired] [Parameter] public ClientPreference? ThemePreference { get; set; }
        [EditorRequired] [Parameter] public EventCallback<ClientPreference> ThemePreferenceChanged { get; set; }

        private readonly List<string> Colors = CustomColors.ThemeColors;

        private async Task UpdateThemePrimaryColor(string color)
        {
            if (ThemePreference is not null)
            {
                ThemePreference.PrimaryColor = color;
                await ThemePreferenceChanged.InvokeAsync(ThemePreference);
            }
        }
        private async Task UpdateThemeSecondaryColor(string color)
        {
            if (ThemePreference is not null)
            {
                ThemePreference.SecondaryColor = color;
                await ThemePreferenceChanged.InvokeAsync(ThemePreference);
            }
        }
        private async Task UpdateBorderRadius(double radius)
        {
            if (ThemePreference is not null)
            {
                ThemePreference.BorderRadius = radius;
                await ThemePreferenceChanged.InvokeAsync(ThemePreference);
            }
        }
        private async Task ToggleDarkLightMode(bool isDarkMode)
        {
            if (ThemePreference is not null)
            {
                ThemePreference.IsDarkMode = isDarkMode;
                await ThemePreferenceChanged.InvokeAsync(ThemePreference);
            }
        }
    }
}
