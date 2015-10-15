using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Web.Syndication;

namespace UwpDeepDive.MainApp.ViewModels
{
    public class XamlPageVm : VmBase
    {
        public XamlPageVm()
        {
            BindingTitle = "Binding";
            NewsItems = new ObservableCollection<NewsItemVmi>();
            //if (DesignMode.DesignModeEnabled)
            //{
                NewsItems.Add(new NewsItemVmi { Title = "first", Description = "description", ImageUri = "https://pbs.twimg.com/profile_images/508960761826131968/LnvhR8ED.png" });
                NewsItems.Add(new NewsItemVmi { Title = "second", Description = "description", ImageUri = "https://pbs.twimg.com/profile_images/508960761826131968/LnvhR8ED.png" });
            //}
        }
        public async Task Init()
        {
            var client = new SyndicationClient();
            var feed = await client.RetrieveFeedAsync(new Uri("http://rss.cnn.com/rss/edition_technology.rss", UriKind.Absolute));

            foreach (var item in feed.Items)
            {
                var element = item.
                    ElementExtensions.FirstOrDefault(e => e.NodeName == "thumbnail");
                var attribute = element.AttributeExtensions.FirstOrDefault(ae => ae.Name == "url");
                var uri = attribute?.Value;
                var imageUri = uri ?? "https://pbs.twimg.com/profile_images/508960761826131968/LnvhR8ED.png";
                NewsItems.Add(new NewsItemVmi
                {
                    Title = item.Title.Text,
                    Description = item.Summary.Text,
                    ImageUri = imageUri,
                    Date = item.PublishedDate.ToString("g")
                });
            }
        }
        public ObservableCollection<NewsItemVmi> NewsItems { get; set; }
        public string BindingTitle { get; set; }
    }
}
