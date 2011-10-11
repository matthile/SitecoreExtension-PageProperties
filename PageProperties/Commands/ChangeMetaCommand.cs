using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using GNReSound.BL.Controls.Seo;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Resources;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.XamlSharp.Continuations;

namespace GNReSound.BL.Commands
{
    class ChangeMetaCommand : WebEditCommand, ISupportsContinuation
    {
        /// <summary>
        /// Executes the command in the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull((object)context, "context");
            if (context.Items.Length != 1)
                return;
            NameValueCollection parameters = new NameValueCollection();

            string itemID = context.Parameters["itemid"] ?? context.Parameters["id"];
            ItemUri itemUri = ItemUri.ParseQueryString();
            if (string.IsNullOrEmpty(itemID) && itemUri != (ItemUri)null)
                itemID = itemUri.ItemID.ToString();

            parameters["itemid"] = itemID;

            string language = context.Parameters["language"];
            if (string.IsNullOrEmpty(language))
                language = itemUri.Language.ToString();

            parameters["language"] = language;
            parameters["navigate"] = context.Parameters["navigate"];
            Context.ClientPage.Start((object)this, "Run", parameters);
        }

        protected void Run(ClientPipelineArgs args)
        {
            Item itemNotNull = Sitecore.Client.GetItemNotNull(args.Parameters["itemid"], Language.Parse(args.Parameters["language"]));
            UrlString urlString = ResourceUri.Parse("Control:Seo.Edit").ToUrlString();
            itemNotNull.Uri.AddToUrlString(urlString);
            SheerResponse.ShowModalDialog(urlString.ToString(), false);
        }
    }
}
