using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Microsoft;

namespace TodoMVC.Tests
{
    public class TranslationFixture
    {
        private static readonly Lazy<TranslatorContainer> translatorContainer = new Lazy<TranslatorContainer>(CreateTranslatorContainer);

        public TranslatorContainer TranslatorContainer
        {
            get { return translatorContainer.Value;  }
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
