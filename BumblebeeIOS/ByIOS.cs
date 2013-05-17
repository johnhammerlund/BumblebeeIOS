using OpenQA.Selenium;

namespace BumblebeeIOS
{
    public class ByIOS
    {
        public static By Name(string name)
        {
            return By.LinkText("name=" + name);
        }

        public static By Value(string value)
        {
            return By.LinkText("value=" + value);
        }

        public static By Label(string label)
        {
            return By.LinkText("label=" + label);
        }

        public static By PartialName(string name)
        {
            return By.PartialLinkText("name=" + name);
        }

        public static By PartialValue(string value)
        {
            return By.PartialLinkText("value=" + value);
        }

        public static By PartialLabel(string label)
        {
            return By.PartialLinkText("label=" + label);
        }

        public static By LocalKey(string key)
        {
            return By.PartialLinkText("name=l10n('" + key + "')");
        }

        public static By Type(string key)
        {
            return By.ClassName(key);
        }
    }
}
