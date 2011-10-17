namespace PageProperties.Controls.HtmlControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Sitecore.Web.UI.HtmlControls;

    class Section : Control
    {
        #region Properties

        public string Header
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        protected override void DoRender(System.Web.UI.HtmlTextWriter output)
        {
            this.SetWidthAndHeightStyle();
            output.Write("<div class=\"Section\">");
            output.Write("<h2>" + this.Header + "</h2>");
            this.RenderChildren(output);
            output.Write("</div>");
        }

        #endregion Methods
    }
}