using ScriptCoreLib;
using ScriptCoreLib.Delegates;
using ScriptCoreLib.Extensions;
using ScriptCoreLib.JavaScript;
using ScriptCoreLib.JavaScript.Components;
using ScriptCoreLib.JavaScript.DOM;
using ScriptCoreLib.JavaScript.DOM.HTML;
using ScriptCoreLib.JavaScript.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AvalonPipeMania.Quattro.HTML.Pages;
using ScriptCoreLib.JavaScript.Windows.Forms;

namespace AvalonPipeMania.Quattro
{
    /// <summary>
    /// This type will run as JavaScript.
    /// </summary>
    internal sealed class Application
    {
        public readonly ApplicationWebService service = new ApplicationWebService();


        /// <summary>
        /// This is a javascript application.
        /// </summary>
        /// <param name="page">HTML document rendered by the web server which can now be enhanced.</param>
        public Application(IDefault page)
        {
            FormStyler.AtFormCreated =
            s =>
            {
                s.Context.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

                //var x = new ChromeTCPServerWithFrameNone.HTML.Pages.AppWindowDrag().AttachTo(s.Context.GetHTMLTarget());
                var x = new ChromeTCPServerWithFrameNone.HTML.Pages.AppWindowDragWithShadow().AttachTo(s.Context.GetHTMLTarget());
            };



            #region ChromeTCPServer
            dynamic self = Native.self;
            dynamic self_chrome = self.chrome;
            object self_chrome_socket = self_chrome.socket;

            if (self_chrome_socket != null)
            {
                Console.WriteLine("FlashHeatZeeker shall run as a chrome app as server");

                chrome.Notification.DefaultTitle = "AvalonPipeMania";
                //chrome.Notification.DefaultIconUrl = new HTML.Images.FromAssets.Preview().src;

                ChromeTCPServer.TheServerWithStyledForm.Invoke(
                    DefaultSource.Text,
                    AtFormCreated: FormStyler.AtFormCreated
                );

                return;
            }
            #endregion

            ApplicationSprite sprite = new ApplicationSprite();
            sprite.AutoSizeSpriteTo(page.ContentSize);
            sprite.AttachSpriteTo(page.Content);
          

        }

    }
}
