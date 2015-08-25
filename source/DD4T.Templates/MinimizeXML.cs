﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using System.Reflection;
using Tridion.ContentManager.Templating;
using DD4T.Templates.Base;
using DD4T.Templates.Base.Utils;
using DD4T.ContentModel;

namespace DD4T.Templates.XML
{

    /// <summary>
    /// Minimizes the XML data by removing all whitespace between elements.
    /// </summary>
    /// <remarks>Only use this template if the output format is XML (not for JSON)</remarks>
    /// [Tridion.ContentManager.Templating.Assembly.TcmTemplateTitle("Minimize XML")] // QS: disabling all XML functionality
    public class MinimizeXML : Tridion.ContentManager.Templating.Assembly.ITemplate
    {
        protected TemplatingLogger log = TemplatingLogger.GetLogger(typeof(MinimizeXML));
        protected Package package;
        protected Engine engine;


        public void Transform(Engine engine, Package package)
        {
            this.package = package;
            this.engine = engine;

            if (engine.PublishingContext.RenderContext != null && engine.PublishingContext.RenderContext.ContextVariables.Contains(BasePageTemplate.VariableNameCalledFromDynamicDelivery))
            {
                if (engine.PublishingContext.RenderContext.ContextVariables[BasePageTemplate.VariableNameCalledFromDynamicDelivery].Equals(BasePageTemplate.VariableValueCalledFromDynamicDelivery))
                {
                    log.Debug("template is rendered by a DynamicDelivery page template, will not convert from XML to java");
                    return;
                }
            }

            Item outputItem = package.GetByName("Output");
            String inputValue = package.GetValue("Output");

            if (inputValue == null || inputValue.Length == 0)
            {
                log.Warning("Could not find 'Output' in the package, nothing to transform");
                return;
            }

          


            string outputValue = XmlMinimizer.Convert(inputValue);            

            // replace the Output item in the package
            package.Remove(outputItem);
            outputItem.SetAsString(outputValue);
            package.PushItem("Output", outputItem);
        }

    }
}
