using System.ComponentModel;
using System.Reflection;

namespace SharedKernel.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());

        if (field == null)
        {
            return string.Empty;
        }

        return field.GetCustomAttribute<DescriptionAttribute>() is not { } attribute
            ? value.ToString()
            : attribute.Description;
    }
}
