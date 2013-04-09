using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace BumblebeeIOS.Extensions
{
    static class Nonstandard
    {

        public static TResult GetAttribute<TResult>(this IWebElement element, string attributeName)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            MethodInfo[] executeInfos = typeof(RemoteWebElement).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);


            MethodInfo elementIdInfo = executeInfos.FirstOrDefault(e => e.Name.Contains("get_InternalElementId"));
            MethodInfo executeInfo = executeInfos.FirstOrDefault(e => e.Name.Contains("Execute"));


            if (elementIdInfo != null) parameters.Add("id", elementIdInfo.Invoke(element, null));
            parameters.Add("name", attributeName);

            TResult attributeValue = default(TResult);

            if (executeInfo != null)
            {
                Response commandResponse = (Response)executeInfo.Invoke(element, new object[] { DriverCommand.GetElementAttribute, parameters });
                if (commandResponse.Value != null)
                {
                    attributeValue = (TResult)commandResponse.Value;
                }
            }
            return attributeValue;
        }
    }
}
