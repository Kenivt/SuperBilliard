using System;

namespace SuperBilliard
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class LoaclizationEnumTextAttribute : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class LoaclizationEnumAdditionalAttribute : Attribute
    {
        public string AdditionalText { get; private set; }

        public LoaclizationEnumAdditionalAttribute(string additionText)
        {
            AdditionalText = additionText;
        }
    }

    public enum wwdwd
    {
        [LoaclizationEnumAdditional("hhhh")]
        None = 0,
    }
}