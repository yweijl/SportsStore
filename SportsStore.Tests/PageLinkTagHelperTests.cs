using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Infrastructure;
using SportsStore.Views.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
    public class PageLinkTagHelperTests
    {
        [Fact]
        public void Can_Generate_Page_Links()
        {
            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2")
                .Returns("Test/Page3");

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            urlHelperFactory.Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(urlHelper.Object);

            var helper = new PageLinkTagHelper(urlHelperFactory.Object)
            {
                PageInfo = new PageInfo
                {
                    ItemsPerPage = 10,
                    CurrentPage = 2,
                    TotalItems = 28
                },
                PageAction = "Test"
            };

            var ctx = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "");

            var content = new Mock<TagHelperContent>();

            var output = new TagHelperOutput("div", new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(content.Object));

            helper.Process(ctx, output);

            var stringBuilder = new StringBuilder();
            stringBuilder
                .Append(CreateUrlElement("Test", "1"))
                .Append(CreateUrlElement("Test", "2"))
                .Append(CreateUrlElement("Test", "3"));

            Assert.Equal(
                stringBuilder.ToString(),
                output.Content.GetContent());
        }

        private static string CreateUrlElement(string url, string pageNumber)
        {
            return $@"<a href=""{url}/Page{pageNumber}"">{pageNumber}</a>";
        }
    }
}