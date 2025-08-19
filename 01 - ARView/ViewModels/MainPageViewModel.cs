using ARKitDemo.Platforms.iOS;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ARKitDemo.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    public bool isARActive;

    [ObservableProperty]
    string statusMessage = "AR ready.";

    [RelayCommand]
    void StartAR()
    {
        if (MauiARView.IsARSupported())
        {
            IsARActive = true;
            StatusMessage = "AR session started.";
        }
        else
        {
            StatusMessage = "AR unsupported on this device.";
        }
    }

    [RelayCommand]
    void StopAR()
    {
        IsARActive = false;
        StatusMessage = "AR session stopped.";
    }
}