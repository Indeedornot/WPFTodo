using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WPFTodo.Converters;
public static class EnumHelper
{
    public static string Description(this Enum value)
    {
        FieldInfo? enumField = value.GetType().GetField(value.ToString());
        IEnumerable<Attribute>? attributes = enumField?.GetCustomAttributes();
        DescriptionAttribute? descriptionAttr = attributes?.OfType<DescriptionAttribute>().FirstOrDefault();

        if (descriptionAttr != null)
        {
            return descriptionAttr.Description;
        }

        // If no description is found, the least we can do is replace underscores with spaces
        // You can add your own custom default formatting logic here
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        string dummyDescription = value.ToString().Replace("_", " ");
        return ti.ToTitleCase(dummyDescription);
    }

    public static IEnumerable<ValueDescription> GetAllValuesAndDescriptions(Type t)
    {
        if (!t.IsEnum)
        {
            throw new ArgumentException($"{nameof(t)} must be an enum type");
        }

        return Enum.GetValues(t).Cast<Enum>().Select(
            (e) => new ValueDescription() { Value = e, Description = e.Description() }).ToList();
    }
}

public class ValueDescription
{
    public object Value { get; set; }
    public string Description { get; set; }
}
