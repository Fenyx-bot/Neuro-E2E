namespace Messaging.Interfaces.Service;

public interface ICookieService
{
    public void Set(string token);
    public string Get();
    public void Remove();
}