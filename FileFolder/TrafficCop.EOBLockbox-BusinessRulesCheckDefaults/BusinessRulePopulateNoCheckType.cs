using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;

namespace TrafficCop.EOBLockbox
{
     [APIAttribute("EOB Lockbox Populate NoCheck Type", "Contains business rules for EOB Lockbox Populate NoCheck Type.")]
    public class BusinessRulePopulateNoCheckType : IBusinessRule
    {
        private IApiAgentBusiness ab = PluginAssemblyManager.Instance().GetInterface<IApiAgentBusiness>();

        public void Execute(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            IBatchConfigurationXml xmlBatch = ((IApiXmlNode)args["Batch"]).GetXmlNavigator<IBatchConfigurationXml>();

            if (args.ContainsKey("form"))
            {
                IFormObject form = (IFormObject)args["form"];
                if (form.FVFFileName.ToUpper().Contains("NOCHECK"))
                {
                    PopulateNoCheck(form, xmlBatch);
                }
            }
            else
            {
                string[] imageFilenames = ab.fileName;
                for (int j = 0; j < imageFilenames.Length; j++)
                {
                    string fdfFilename = ab.GetFDFNameFromImageName(imageFilenames[j]);
                    IFormObject form = ab.GetForm(fdfFilename);
                    if (form.FVFFileName.ToUpper().Contains("NOCHECK"))
                    {
                        PopulateNoCheck(form, xmlBatch);
                    }
                }
            }

            if (!args.ContainsKey("ObjectModel"))
                args.Add("ObjectModel", true);
            else
                args["ObjectModel"] = true;

        }


        public IConfigurationPage GetConfigurationPage(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            return null;
        }

        public void PopulateNoCheck(IFormObject form, IBatchConfigurationXml xmlBatch)
        {
            IField noCheckTypeField = form.GetField("NoCheckType");
            IField vccItemField = form.GetField("VCCItem");
            IField checkAmountField = form.GetField("Check_Amount");

            if (vccItemField != null && vccItemField.GetCurrentValue().ToUpper().Equals("TRUE"))
            {
                noCheckTypeField.SetCurrentValue("VCC");
            }
            else if (checkAmountField.GetCurrentValue().Equals("0.00") || !(checkAmountField.GetCurrentValue().Length > 0))
            {
                noCheckTypeField.SetCurrentValue("ZEROPAY");
            }
            else
            {
                noCheckTypeField.SetCurrentValue("NOCHECK");
            }
      
        }
    }
}
