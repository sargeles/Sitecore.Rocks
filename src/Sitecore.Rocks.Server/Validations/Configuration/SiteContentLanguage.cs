// � 2015-2016 Sitecore Corporation A/S. All rights reserved.

using Sitecore.Configuration;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;

namespace Sitecore.Rocks.Server.Validations.Configuration
{
    [Validation("Site content language", "Configuration")]
    public class SiteContentLanguage : Validation
    {
        public override bool CanCheck(string contextName)
        {
            Assert.ArgumentNotNull(contextName, nameof(contextName));

            return contextName == "Site";
        }

        public override void Check(ValidationWriter output)
        {
            Assert.ArgumentNotNull(output, nameof(output));

            foreach (var siteName in Factory.GetSiteNames())
            {
                var site = Factory.GetSite(siteName);

                if (site.ContentDatabase == null || site.ContentLanguage == null)
                {
                    continue;
                }

                if (LanguageManager.IsLanguageNameDefined(site.ContentDatabase, site.ContentLanguage.Name))
                {
                    continue;
                }

                output.Write(SeverityLevel.Error, "Site content language", string.Format("The content language \"{0}\" is specified on the site \"{1}\" but is not defined in the site content database \"{2}\".", site.ContentLanguage.Name, site.Name, site.ContentDatabase.Name), string.Format("Set the \"ContentLanguage\" attribute on the site \"{0}\" to an existing language.", site.Name));
            }
        }
    }
}
