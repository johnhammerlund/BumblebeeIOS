using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace BumblebeeIOS
{
    class IOSDriver : RemoteWebDriver
    {
        public IOSDriver(ICommandExecutor commandExecutor, ICapabilities desiredCapabilities) : base(commandExecutor, desiredCapabilities)
        {
        }

        public IOSDriver(ICapabilities desiredCapabilities) : base(desiredCapabilities)
        {
        }

        public IOSDriver(Uri remoteAddress, ICapabilities desiredCapabilities) : base(remoteAddress, desiredCapabilities)
        {
        }

        public IOSDriver(Uri remoteAddress, ICapabilities desiredCapabilities, TimeSpan commandTimeout) : base(remoteAddress, desiredCapabilities, commandTimeout)
        {
        }

        protected override Response Execute(string driverCommandToExecute, Dictionary<string, object> parameters)
        {
            Command commandToExecute = new Command(SessionId, driverCommandToExecute, parameters);

            Response commandResponse = new Response();

           try
            {
                commandResponse = CommandExecutor.Execute(commandToExecute);
            }
            catch (System.Net.WebException e)
            {
                commandResponse.Status = WebDriverResult.UnhandledError;
                commandResponse.Value = e;
            }

            if (commandResponse.Status != WebDriverResult.Success)
            {
                UnpackAndThrowOnError(commandResponse);
            }

            if (commandResponse.Value as Dictionary<string, object> != null)
                (commandResponse.Value as Dictionary<string, object>)["javascriptEnabled"] = true;

            return commandResponse;
        }

        private static void UnpackAndThrowOnError(Response errorResponse)
        {
            // Check the status code of the error, and only handle if not success.
            if (errorResponse.Status != WebDriverResult.Success)
            {
                Dictionary<string, object> errorAsDictionary = errorResponse.Value as Dictionary<string, object>;
                if (errorAsDictionary != null)
                {
                    ErrorResponse errorResponseObject = new ErrorResponse(errorAsDictionary);
                    string errorMessage = errorResponseObject.Message;
                    switch (errorResponse.Status)
                    {
                        case WebDriverResult.NoSuchElement:
                            throw new NoSuchElementException(errorMessage);

                        case WebDriverResult.NoSuchFrame:
                            throw new NoSuchFrameException(errorMessage);

                        case WebDriverResult.UnknownCommand:
                            throw new NotImplementedException(errorMessage);

                        case WebDriverResult.ObsoleteElement:
                            throw new StaleElementReferenceException(errorMessage);

                        case WebDriverResult.ElementNotDisplayed:
                            throw new ElementNotVisibleException(errorMessage);

                        case WebDriverResult.InvalidElementState:
                        case WebDriverResult.ElementNotSelectable:
                            throw new InvalidElementStateException(errorMessage);

                        case WebDriverResult.UnhandledError:
                            throw new InvalidOperationException(errorMessage);

                        case WebDriverResult.NoSuchDocument:
                            throw new NoSuchElementException(errorMessage);

                        case WebDriverResult.Timeout:
                            throw new WebDriverTimeoutException(errorMessage);

                        case WebDriverResult.NoSuchWindow:
                            throw new NoSuchWindowException(errorMessage);

                        case WebDriverResult.InvalidCookieDomain:
                        case WebDriverResult.UnableToSetCookie:
                            throw new WebDriverException(errorMessage);

                        case WebDriverResult.AsyncScriptTimeout:
                            throw new WebDriverTimeoutException(errorMessage);

                        case WebDriverResult.UnexpectedAlertOpen:
                            // TODO(JimEvans): Handle the case where the unexpected alert setting
                            // has been set to "ignore", so there is still a valid alert to be
                            // handled.
                            string alertText = string.Empty;
                            if (errorAsDictionary.ContainsKey("alert"))
                            {
                                Dictionary<string, object> alertDescription = errorAsDictionary["alert"] as Dictionary<string, object>;
                                if (alertDescription != null && alertDescription.ContainsKey("text"))
                                {
                                    alertText = alertDescription["text"].ToString();
                                }
                            }

                            throw new UnhandledAlertException(errorMessage, alertText);

                        case WebDriverResult.NoAlertPresent:
                            throw new NoAlertPresentException(errorMessage);

                        case WebDriverResult.InvalidSelector:
                            throw new InvalidSelectorException(errorMessage);

                        default:
                            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "{0} ({1})", errorMessage, errorResponse.Status));
                    }
                }
                else
                {
                    throw new WebDriverException("Unexpected error. " + errorResponse.Value.ToString());
                }
            }
        }
    }
}
