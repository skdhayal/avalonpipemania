using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using android.app;
using android.os;
using android.view;
using android.widget;
using ScriptCoreLib;
using ScriptCoreLib.Android.Extensions;
using xavalon.net;

namespace AvalonPipeManiaActivity.Activities
{
    public class ApplicationActivity : WebServiceServerActivity
    {


        protected override void onCreate(Bundle savedInstanceState)
        {
            // jsc.meta generates a different version than jsc
            this.ApplicationFile = "index.htm";

            base.onCreate(savedInstanceState);

            
        }


    }


}
