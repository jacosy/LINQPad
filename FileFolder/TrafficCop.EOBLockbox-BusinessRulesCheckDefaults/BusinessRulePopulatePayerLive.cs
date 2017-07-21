using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;

namespace TrafficCop.EOBLockbox
{
    [APIAttribute("EOB Lockbox Populate Payer Live", "Contains business rules for EOB Lockbox Populate Payer Live.")]

    /// <summary>
    /// CR 22222. Runs by form or by batch. 
    /// Performs when 
    ///     PerformProductMapping BR batch data node exists and 
    ///     is set to “TRUE” and 
    ///     The form contains a field called “ProductType”. 
    /// 
    /// If the “ProductType” virtual field is set to “EOBLITE”
    /// then the PayerLive virtual field should be set to “EOB Lite”. 
    /// 
    /// If the “ProductType virtual field is set to “EOBFULL”
    /// then the PayerLive virtual field should be set to “EOB Capture”. 
    /// 
    /// If the ProductType virtual field is set to “BOTH”
    /// then the PayerLive virtual field should be set to “EOB Capture Test”. 
    /// 
    /// If the ProductType virtual field does not contain data
    /// then an error should be thrown.
    /// </summary>
    public class BusinessRulePopulatePayerLive : IBusinessRule
    {
        private IApiAgentBusiness ab = PluginAssemblyManager.Instance().GetInterface<IApiAgentBusiness>();

        public void Execute(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            IBatchConfigurationXml xmlBatch = ((IApiXmlNode)args["Batch"]).GetXmlNavigator<IBatchConfigurationXml>();


            if (xmlBatch.GetBatchFieldValue("PerformProductMappingBR") != null &&
                xmlBatch.GetBatchFieldValue("PerformProductMappingBR").ToUpper() == "TRUE")
            {

                if (args.ContainsKey("form"))
                {
                    IFormObject form = (IFormObject)args["form"];
                    SetProductType(form, xmlBatch);
                }
                else
                {
                    string[] imageFilenames = ab.fileName;
                    for (int j = 0; j < imageFilenames.Length; j++)
                    {
                        string fdfFilename = ab.GetFDFNameFromImageName(imageFilenames[j]);
                        IFormObject form = ab.GetForm(fdfFilename);
                        SetProductType(form, xmlBatch);
                    }
                }

            }

            if (!args.ContainsKey("ObjectModel"))
                args.Add("ObjectModel", true);
            else
                args["ObjectModel"] = true;


        }

        public void SetProductType(IFormObject form, IBatchConfigurationXml xmlBatch)
        {

            IField productTypeField = form.GetField("ProductType");
            IField payerLiveField = form.GetField("PayerLive");
            if (payerLiveField != null && productTypeField != null)
            {
                if( productTypeField.GetCurrentValue().ToUpper().Trim().Equals("BOTH") )
                    payerLiveField.SetCurrentValue("Test");
                else
                    payerLiveField.SetCurrentValue("Live");
            }
        }

        public IConfigurationPage GetConfigurationPage(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            return null;
        }

        private void ThrowErrorException(string message)
        {
            throw new Exception("Error: " + message);
        }

    }
}
