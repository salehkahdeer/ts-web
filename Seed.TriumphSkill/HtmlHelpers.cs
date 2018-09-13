using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Seed.TriumphSkill
{
    public static class HtmlHelpers
    {
        public static string GetInputName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (expression.Body.NodeType == ExpressionType.Call)
            {
                MethodCallExpression methodCallExpression = (MethodCallExpression)expression.Body;
                string name = GetInputName(methodCallExpression);
                return name.Substring(expression.Parameters[0].Name.Length + 1);

            }
            return expression.Body.ToString().Substring(expression.Parameters[0].Name.Length + 1);
        }

        private static string GetInputName(MethodCallExpression expression)
        {
            // p => p.Foo.Bar().Baz.ToString() => p.Foo OR throw...
            MethodCallExpression methodCallExpression = expression.Object as MethodCallExpression;
            if (methodCallExpression != null)
            {
                return GetInputName(methodCallExpression);
            }
            return expression.Object.ToString();
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes) where TModel : class
        {
            string inputName = GetInputName(expression);
            var value = htmlHelper.ViewData.Model == null
                ? default(TProperty)
                : expression.Compile()(htmlHelper.ViewData.Model);

            return htmlHelper.DropDownList(inputName, ToSelectList(typeof(TProperty), value.ToString()), htmlAttributes);
        }

        public static SelectList ToSelectList(Type enumType, string selectedItem)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(enumType))
            {
                FieldInfo fi = enumType.GetField(item.ToString());
                var attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
                var title = attribute == null ? item.ToString() : ((DescriptionAttribute)attribute).Description;
                int val;
                var listItem = new SelectListItem
                {
                    Value = (Int32.TryParse(selectedItem, out val) ? ((int)item).ToString() : item.ToString()),
                    Text = title,
                    Selected = selectedItem == (Int32.TryParse(selectedItem, out val) ? ((int)item).ToString() : item.ToString())
                };
                items.Add(listItem);
            }

            return new SelectList(items, "Value", "Text", selectedItem);
        }

        public static MvcHtmlString SuccessNotification(this HtmlHelper helper)
        {
            if (helper.ViewBag.Success == null)
                return MvcHtmlString.Empty;
             

            string success = @"<div class='alert alert-dismissible alert-success'><button type='button' class='close' data-dismiss='alert'>x</button><h4>Success!</h4>";
            success += HttpUtility.UrlDecode(helper.ViewBag.Success);
            success += @"</div>
                                <script type='text/javascript'>
                                $(function () {setTimeout(function(){ $('.alert-success').fadeOut(); },3000)});
                                </script>";

            return MvcHtmlString.Create(success);
        }

        public static MvcHtmlString WarningNotification(this HtmlHelper helper)
        {
            if (helper.ViewBag.Warning == null)
                return MvcHtmlString.Empty;

            string marutiWarning = @"<div class='alert alert-block'> <a class='close' data-dismiss='alert' href='#'>×</a>
                                        <h4 class='alert-heading'>Warning!</h4>";
            marutiWarning += HttpUtility.UrlDecode(helper.ViewBag.Warning);
            marutiWarning += "</div>";

            return MvcHtmlString.Create(marutiWarning);
        }

        public static MvcHtmlString InfoNotification(this HtmlHelper helper)
        {
            if (helper.ViewBag.Info == null)
                return MvcHtmlString.Empty;

            string marutiInfo = @"<div class='alert alert-info alert-block'> <a class='close' data-dismiss='alert' href='#'>×</a>
                                        <h4 class='alert-heading'>Info!</h4>";
            marutiInfo += HttpUtility.UrlDecode(helper.ViewBag.Info);
            marutiInfo += @"</div>
                                <script type='text/javascript'>
                                $(function () {setTimeout(function(){ $('.alert-info').fadeOut(); },15000)});
                                </script>";


            return MvcHtmlString.Create(marutiInfo);
        }

        public static MvcHtmlString ErrorNotification(this HtmlHelper helper)
        {
            if (helper.ViewBag.Error == null)
                return MvcHtmlString.Empty;

            string marutiWarning = @"<div class='alert alert-error alert-block'> <a class='close' data-dismiss='alert' href='#'>×</a>
                                        <h4 class='alert-heading'>Error!</h4>";
            marutiWarning += HttpUtility.UrlDecode(helper.ViewBag.Error);
            marutiWarning += "</div>";

            return MvcHtmlString.Create(marutiWarning);
        }

        /// <summary>
        /// Ref: http://blog.stevensanderson.com/2010/01/28/editing-a-variable-length-list-aspnet-mvc-2-style/
        /// </summary>
        private const string idsToReuseKey = "__htmlPrefixScopeExtensions_IdsToReuse_";

        public static IDisposable BeginCollectionItem(this HtmlHelper html, string collectionName)
        {
            //nested list support
            //http://www.joe-stevens.com/2011/06/06/editing-and-binding-nested-lists-with-asp-net-mvc-2/
            if (html.ViewData["ContainerPrefix"] != null)
            {
                collectionName = string.Concat(html.ViewData["ContainerPrefix"], ".", collectionName);
            }
            else
            {
                collectionName = string.Concat(html.ViewData.TemplateInfo.HtmlFieldPrefix, ".", collectionName).Trim('.');
            }

            var idsToReuse = GetIdsToReuse(html.ViewContext.HttpContext, collectionName);
            string itemIndex = idsToReuse.Count > 0 ? idsToReuse.Dequeue() : Guid.NewGuid().ToString();

            var htmlFieldPrefix = string.Format("{0}[{1}]", collectionName, itemIndex);
            html.ViewData["ContainerPrefix"] = htmlFieldPrefix;

            // autocomplete="off" is needed to work around a very annoying Chrome behaviour whereby it reuses old values after the user clicks "Back", which causes the xyz.index and xyz[...] values to get out of sync.
            html.ViewContext.Writer.WriteLine(string.Format("<input type=\"hidden\" name=\"{0}.index\" autocomplete=\"off\" value=\"{1}\" />", collectionName, html.Encode(itemIndex)));

            return BeginHtmlFieldPrefixScope(html, string.Format("{0}[{1}]", collectionName, itemIndex));
        }

        public static IDisposable BeginHtmlFieldPrefixScope(this HtmlHelper html, string htmlFieldPrefix)
        {
            return new HtmlFieldPrefixScope(html.ViewData.TemplateInfo, htmlFieldPrefix);
        }

        private static Queue<string> GetIdsToReuse(HttpContextBase httpContext, string collectionName)
        {
            // We need to use the same sequence of IDs following a server-side validation failure,  
            // otherwise the framework won't render the validation error messages next to each item.
            string key = idsToReuseKey + collectionName;
            var queue = (Queue<string>)httpContext.Items[key];
            if (queue == null)
            {
                httpContext.Items[key] = queue = new Queue<string>();
                var previouslyUsedIds = httpContext.Request[collectionName + ".index"];
                if (!string.IsNullOrEmpty(previouslyUsedIds))
                    foreach (string previouslyUsedId in previouslyUsedIds.Split(','))
                        queue.Enqueue(previouslyUsedId);
            }
            return queue;
        }

        private class HtmlFieldPrefixScope : IDisposable
        {
            private readonly TemplateInfo templateInfo;
            private readonly string previousHtmlFieldPrefix;

            public HtmlFieldPrefixScope(TemplateInfo templateInfo, string htmlFieldPrefix)
            {
                this.templateInfo = templateInfo;

                previousHtmlFieldPrefix = templateInfo.HtmlFieldPrefix;
                templateInfo.HtmlFieldPrefix = htmlFieldPrefix;
            }

            public void Dispose()
            {
                templateInfo.HtmlFieldPrefix = previousHtmlFieldPrefix;
            }
        }
    }
}