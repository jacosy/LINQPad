using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;
using FvTech.Data;
using System.Data;
using TrafficCop.Data;
using System.Globalization;
using System.Xml;
using System.IO;
using TrafficCop.Batch;
       
namespace TrafficCop.EOBLockbox
{
    public class BusinessRulePopulatePrintServicesID : IBusinessRule
    {
        #region IBusinessRule Members

        private IApiAgentBusiness ab = PluginAssemblyManager.Instance().GetInterface<IApiAgentBusiness>();

        public void Execute(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            IBatchConfigurationXml xmlBatch = ((IApiXmlNode)args["Batch"]).GetXmlNavigator<IBatchConfigurationXml>();
            
            if (args.ContainsKey("form"))
            {
                IFormObject form = (IFormObject)args["form"];
                PopulatePrintServicesID(form, xmlBatch);
            }
            else
            {
                string[] imageFilenames = ab.fileName;
                for (int j = 0; j < imageFilenames.Length; j++)
                {
                    string fdfFilename = ab.GetFDFNameFromImageName(imageFilenames[j]);
                    IFormObject form = ab.GetForm(fdfFilename);
                    PopulatePrintServicesID(form, xmlBatch);
                }
            }
            if (!args.ContainsKey("ObjectModel"))
                args.Add("ObjectModel", true);
            else
                args["ObjectModel"] = true;

        }

        private void PopulatePrintServicesID(IFormObject form, IBatchConfigurationXml xmlBatch)
        {
            string scanCenter = xmlBatch.GetBatchDataNode("ScanCenter");
            if (string.IsNullOrEmpty(scanCenter) || scanCenter != "ABF")
                return;

            IField PrintServicesIDField = form.GetField("PrintServicesID");
            if (PrintServicesIDField == null)
                return;
            string printServicesID = string.Empty;
            string originalFilename = Path.GetFileNameWithoutExtension(ab.GetOriginalFileName(form.GetImageHistoryFilename(TrafficCop.FDF.ImageChangeType.Original)));
            int underscoreIndex = originalFilename.LastIndexOf("_");
            int length = originalFilename.Length;
            if (underscoreIndex > -1 && (length - (underscoreIndex + 1)) < originalFilename.Length &&
               underscoreIndex - 1 < originalFilename.Length && (length - (underscoreIndex + 1)) > 0)
            {
                 printServicesID = originalFilename.Substring(underscoreIndex + 1, length - (underscoreIndex + 1));
            }
            PrintServicesIDField.SetCurrentValue(printServicesID);
        }

        public IConfigurationPage GetConfigurationPage(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            return null;
        }

        #endregion

    }
}
