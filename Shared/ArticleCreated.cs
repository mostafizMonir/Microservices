namespace Shared;

public class ArticleCreated
{
    public string HelloFromAspNetCoreNet { get; set; }

    public ArticleCreated(string helloFromAspNetCoreNet)
    {
        HelloFromAspNetCoreNet = helloFromAspNetCoreNet;
    }
}
