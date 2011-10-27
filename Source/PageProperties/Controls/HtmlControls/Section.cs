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
            get;
            set;
        }

        public int Order
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        protected override void DoRender(System.Web.UI.HtmlTextWriter output)
        {
            this.SetWidthAndHeightStyle();
            output.Write("<div class=\"wrapper\">");
            output.Write("<div class=\"Section\">");
            output.Write("<div class=\"scEditorSectionCaptionExpanded\">" + this.Header + "</div>");
            this.RenderChildren(output);
            output.Write("</div>");
            output.Write("</div>");
        }

        #endregion Methods
    }
}