using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TasksApp.Infrastructure
{
    public class MultiTenantIssuerNameRegistry : ValidatingIssuerNameRegistry
    {
        private static readonly string filePath = HttpContext.Current.Server.MapPath("~/App_Data/tenants.xml");
        private static readonly XDocument doc = XDocument.Load(filePath);

        public static bool ContainsTenant(string tenantId)
        {
            return doc.Descendants("tenant").Any(x => x.Attribute("id").Value == tenantId);
        }

        public static bool ContainsKey(string thumbprint)
        {
            return doc.Descendants("key").Any(x => x.Attribute("id").Value == thumbprint);
        }

        protected override bool IsThumbprintValid(string thumbprint, string issuer)
        {
            var issuerID = issuer.TrimEnd('/').Split('/').Last();

            if (ContainsTenant(issuerID))
            {
                if (ContainsKey(thumbprint))
                    return true;
            }
            return false;
        }

        public static void RefreshKeys(string metadataAddress)
        {
            var ia = GetIssuingAuthority(metadataAddress);
            var newKeys = ia.Thumbprints.Any(thumbp => !ContainsKey(thumbp));

            if (!newKeys) return;
            var keysRoot = (from tt in doc.Descendants("keys") select tt).First();
            keysRoot.RemoveNodes();
            foreach (var node in ia.Thumbprints.Select(thumbp => new XElement("key", new XAttribute("id", thumbp))))
            {
                keysRoot.Add(node);
            }
            doc.Save(filePath);
        }


        public static void AddTenant(string tenantId)
        {
            if (ContainsTenant(tenantId)) return;
            var node = new XElement("tenant", new XAttribute("id", tenantId));
            var tenantsRoot = (from tt in doc.Descendants("tenants") select tt).First();

            tenantsRoot.Add(node);
            doc.Save(filePath);
        }
    }
}