using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ARKitDemo.Services;

namespace ARKitDemo.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    readonly IARService _arService;

    [ObservableProperty]
    public bool isARActive;

    [ObservableProperty]
    string statusMessage = "AR ready.";

    public MainPageViewModel(IARService arService)
    {
        _arService = arService;
    }

    [RelayCommand]
    void StartAR()
    {
        if (_arService.IsARSupported())
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