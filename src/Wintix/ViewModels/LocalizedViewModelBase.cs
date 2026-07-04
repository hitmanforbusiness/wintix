using CommunityToolkit.Mvvm.ComponentModel;
using Wintix.Localization;

namespace Wintix.ViewModels;

public abstract partial class LocalizedViewModelBase : ObservableObject
{
    protected LocalizedViewModelBase()
    {
        LocalizationService.Instance.LanguageChanged += OnLanguageChanged;
    }

    protected string T(string key) => L.T(key);

    protected string T(string key, params object[] args) => L.T(key, args);

    protected virtual void OnLanguageChanged(object? sender, EventArgs e) => RefreshLocalizedStrings();

    protected abstract void RefreshLocalizedStrings();
}
