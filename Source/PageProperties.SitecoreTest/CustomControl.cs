using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Web.UI.HtmlControls;

namespace PageProperties.SitecoreTest
{
    class CustomControl : Input
    {

        protected override void DoRender(System.Web.UI.HtmlTextWriter output)
        {
            output.Write("<input type=\"input\" value=\"Custom sjover" + this.Value + "\"/>");
        }
        
    }
}
