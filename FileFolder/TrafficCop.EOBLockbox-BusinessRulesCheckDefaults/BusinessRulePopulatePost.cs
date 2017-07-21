using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;

namespace TrafficCop.EOBLockbox
{
    [APIAttribute("EOB Lockbox Populate Post", "Contains business rules for EOB Lockbox Populate Post.")]

    public class BusinessRulePopulatePost : IBusinessRule
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
                    PopulatePost(form, xmlBatch);
                }
                else
                {
                    string[] imageFilenames = ab.fileName;
                    for (int j = 0; j < imageFilenames.Length; j++)
                    {
                        string fdfFilename = ab.GetFDFNameFromImageName(imageFilenames[j]);
                        IFormObject form = ab.GetForm(fdfFilename);
                        PopulatePost(form, xmlBatch);
                    }
                }

            }

            if (!args.ContainsKey("ObjectModel"))
                args.Add("ObjectModel", true);
            else
                args["ObjectModel"] = true;


        }

        public void PopulatePost(IFormObject form, IBatchConfigurationXml xmlBatch)
        {
            IField productTypeField = form.GetField("ProductType");
            IField postField = form.GetField("Post");
            if (postField != null && productTypeField != null)
            {
                if( productTypeField.GetCurrentValue().ToUpper().Trim().Equals("EOBFULL") && (form.FVFFileName.ToUpper().Contains("CHECK") || form.FVFFileName.ToUpper().Contains("PATIENTPAY") ) )
                    postField.SetCurrentValue("835");
                else
                    postField.SetCurrentValue("Manual");
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
