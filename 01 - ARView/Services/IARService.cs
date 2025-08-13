namespace ARKitDemo.Services;

public interface IARService
{
    bool IsARSupported();
    void StartARSession();
    void StopARSession();
}