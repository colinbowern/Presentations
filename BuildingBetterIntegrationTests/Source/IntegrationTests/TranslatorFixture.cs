using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Microsoft;

namespace Bits.IntegrationTests
{
    public class TranslatorFixture
    {
        private readonly Lazy<TranslatorContainer> container = new Lazy<TranslatorContainer>(CreateTranslatorContainer);

        public TranslatorContainer Container
        {
            get
            {
                return container.Value;
            }
        }

        private static TranslatorContainer CreateTranslatorContainer()
        {
            var serviceUri = new Uri(ConfigurationManager.ConnectionStrings["TranslationServices"].ConnectionString);
            var serviceCredentials = new NetworkCredential(ConfigurationManager.AppSettings["TranslationServicesKey"],
                ConfigurationManager.AppSettings["TranslationServicesKey"]);
            var result = new TranslatorContainer(serviceUri) { Credentials = serviceCredentials };
            return result;
        }
    }
}