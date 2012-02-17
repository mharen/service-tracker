using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Collections;
using System.Text;
using System.Globalization;

namespace service_tracker_mvc
{
    public class ExtendedSelectListItem : SelectListItem
    {
        public object htmlAttributes { get; set; }
    }

    public static partial class HtmlHelperExtensions
    {
        static object GetModelStateValue(HtmlHelper htmlHelper, string key, Type destinationType)
        {
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
            {
                if (modelState.Value != null)
                {
                    return modelState.Value.ConvertTo(destinationType, null /* culture */);
                }
            }
            return null;
        }

        public static MvcHtmlString ExtendedDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, 
            IEnumerable<ExtendedSelectListItem> selectList, 
            string optionLabel, 
            object htmlAttributes,
            object selectedValue)
        {
            return SelectInternal(
                htmlHelper, 
                optionLabel, 
                ExpressionHelper.GetExpressionText(expression), 
                selectList, 
                false /* allowMultiple */, 
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes),
                selectedValue);
        }

        private static MvcHtmlString SelectInternal(
            this HtmlHelper htmlHelper, 
            string optionLabel, 
            string name, 
            IEnumerable<ExtendedSelectListItem> selectList, 
            bool allowMultiple, 
            IDictionary<string, object> htmlAttributes,
            object selectedValue)
        {
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
                throw new ArgumentException("No name");

            if (selectList == null)
                throw new ArgumentException("No selectlist");

            object defaultValue = (allowMultiple) ? GetModelStateValue(htmlHelper, fullName, typeof(string[])) : GetModelStateValue(htmlHelper, fullName, typeof(string));

            // If we haven't already used ViewData to get the entire list of items then we need to
            // use the ViewData-supplied value before using the parameter-supplied value.
            if (defaultValue == null)
                defaultValue = htmlHelper.ViewData.Eval(fullName);

            // If we still don't have the value, use what was passed in
            if (defaultValue == null)
                defaultValue = selectedValue;

            if (defaultValue != null)
            {
                IEnumerable defaultValues = (allowMultiple) ? defaultValue as IEnumerable : new[] { defaultValue };
                IEnumerable<string> values = from object value in defaultValues select Convert.ToString(value, CultureInfo.CurrentCulture);
                HashSet<string> selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
                List<ExtendedSelectListItem> newSelectList = new List<ExtendedSelectListItem>();

                foreach (ExtendedSelectListItem item in selectList)
                {
                    item.Selected = (item.Value != null) ? selectedValues.Contains(item.Value) : selectedValues.Contains(item.Text);
                    newSelectList.Add(item);
                }
                selectList = newSelectList;
            }
            else
            {
                selectList.Where(li => li.Selected).ToList().ForEach(li => li.Selected = false);
            }

            // Convert each ListItem to an <option> tag
            StringBuilder listItemBuilder = new StringBuilder();

            // Make optionLabel the first item that gets rendered.
            if (optionLabel != null)
                listItemBuilder.Append(ListItemToOption(new ExtendedSelectListItem() { Text = optionLabel, Value = String.Empty, Selected = false }));

            foreach (ExtendedSelectListItem item in selectList)
            {
                listItemBuilder.Append(ListItemToOption(item));
            }
            
            TagBuilder tagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("name", fullName, true /* replaceExisting */);
            tagBuilder.GenerateId(fullName);
            if (allowMultiple)
                tagBuilder.MergeAttribute("multiple", "multiple");

            // If there are any errors for a named field, we add the css attribute.
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            tagBuilder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name));

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        internal static string ListItemToOption(ExtendedSelectListItem item)
        {
            TagBuilder builder = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode(item.Text)
            };
            if (item.Value != null)
            {
                builder.Attributes["value"] = item.Value;
            }
            if (item.Selected)
            {
                builder.Attributes["selected"] = "selected";
            }
            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(item.htmlAttributes));
            return builder.ToString(TagRenderMode.Normal);
        }
    }
}