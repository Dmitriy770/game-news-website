namespace GameNews.OAuth.Api;

public class Storage
{
    private Dictionary<string, string> _stateToUrl = new();

    public string GetAndDeleteUrl(string guid)
    {
        var url = _stateToUrl[guid];
        _stateToUrl.Remove(guid);
        
        return url;
    }

    public string AddUrl(string url)
    {
        var guid = Guid.NewGuid().ToString();
        _stateToUrl[guid] = url;
        return guid;
    }
}