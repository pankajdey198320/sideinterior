using System.Text;
using System.Web;
using System.Web.Mvc;


public static class DemoHelper
{
    public static HtmlString SimpleTextBox(this HtmlHelper helper, string id)
    {
        StringBuilder htmlString = new StringBuilder();
        htmlString.Append("<input type='text'");
        htmlString.Append(" id='" + id+"'");
        htmlString.Append("/>");
        return new HtmlString(htmlString.ToString());

    }
    public static HtmlString BootstrapTextBox(this HtmlHelper helper, string id,string placeholder)
    {
        StringBuilder htmlString = new StringBuilder();
        htmlString.Append("<div class='input-group'>");
        htmlString.AppendFormat("<span class='input-group-addon fix-Label-width'>{0}</span>",placeholder);
        htmlString.AppendFormat("<input class='form-control' id='{0}' name='{1}' placeholder='{2}' required type='text' />", id, id, placeholder);
        htmlString.Append("</div>");
       
        return new HtmlString(htmlString.ToString());

    }

}
