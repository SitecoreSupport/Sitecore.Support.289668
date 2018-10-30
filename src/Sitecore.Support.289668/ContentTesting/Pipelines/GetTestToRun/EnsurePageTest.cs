
namespace Sitecore.Support.ContentTesting.Pipelines.GetTestToRun
{
  using Sitecore.ContentTesting.Pipelines.GetTestToRun;
  using Sitecore.ContentTesting.Model;
  using System.Collections.Specialized;
  using System.Web;
  public class EnsurePageTest : GetTestToRunProcessor
  {
    public override void Process(GetTestToRunArgs args)
    {
      //Fix for bug 289668 : added check for TestType.
      if (args.TestDefinition != null && args.TestDefinition.GetTestType() == TestType.Page)
      {
        string uri = args.HostItem.Uri.ToDataUri().ToString();
        uri = RemoveVersionFromUri(uri);
        string text = args.TestDefinition.ParseContentItem()?.ToString();
        if (text != null)
        {
          text = RemoveVersionFromUri(text);
          if (!text.Equals(uri))
          {
            args.TestDefinition = null;
          }
        }
      }
    }

    private string RemoveVersionFromUri(string uri)
    {
      if (string.IsNullOrEmpty(uri))
      {
        return uri;
      }
      NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(uri);
      nameValueCollection.Remove("ver");
      return nameValueCollection.ToString();
    }
  }
}