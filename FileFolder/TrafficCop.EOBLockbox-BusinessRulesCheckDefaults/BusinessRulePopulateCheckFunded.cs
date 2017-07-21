using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;

namespace TrafficCop.EOBLockbox
{
    [APIAttribute("EOB Lockbox Populate Check Funded", "Contains business rules for EOB Lockbox Populate Check Funded.")]
    public class BusinessRulePopulateCheckFunded : IBusinessRule
    {
        private IApiAgentBusiness ab = PluginAssemblyManager.Instance().GetInterface<IApiAgentBusiness>();

        public void Execute(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            IBatchConfigurationXml xmlBatch = ((IApiXmlNode)args["Batch"]).GetXmlNavigator<IBatchConfigurationXml>();

            if (args.ContainsKey("form"))
            {
                IFormObject form = (IFormObject)args["form"];

                if (form.GetField("Funded") != null)
                    SetCheckFunded(form, xmlBatch);
            }
            else
            {
                string[] imageFilenames = ab.fileName;
                for (int j = 0; j < imageFilenames.Length; j++)
                {
                    string fdfFilename = ab.GetFDFNameFromImageName(imageFilenames[j]);
                    IFormObject form = ab.GetForm(fdfFilename);

                    if (form.GetField("Funded") != null)
                    {
                        SetCheckFunded(form, xmlBatch);
                    }
                }
            }
            if (!args.ContainsKey("ObjectModel"))
                args.Add("ObjectModel", true);
            else
                args["ObjectModel"] = true;
        }

        public void SetCheckFunded(IFormObject form, IBatchConfigurationXml xmlBatch)
        {

            IField fundedField = form.GetField("Funded");
            if (fundedField == null)
            {
                ThrowErrorException("Funded virtual field missing");
            }

            if (xmlBatch.GetBatchDataNode("PerformEFTVerificationBR").ToUpper() != "TRUE"
                || form.FVFFileName.Contains("Correspondence"))
            {
                fundedField.SetCurrentValue("");
                return;
            }


            IField productTypeField = form.GetField("ProductType");
            IField checkAmountField = form.GetField("Check_Amount");
            string bString2 = xmlBatch.GetBatchDataNode("PerformEFTVerificationNonProcessedPayerIDList");
            string bString3 = xmlBatch.GetBatchDataNode("PerformEFTVerificationProcessedPayerIDList");
            IField performFundsVerificationField = form.GetField("PerformFundsVerification");

            double checkAmount = 0.0;
            Double.TryParse(checkAmountField.GetCurrentValue(), out checkAmount);

            if (form.FVFFileName.Contains("_Check"))
            {

                if (bString2.Length > 0 && bString2.Contains(form.GetField("PayerID").GetCurrentValue()))
                {
                    fundedField.SetCurrentValue("FVoff");
                }
                else if (bString3.Length > 0 && !bString3.Contains(form.GetField("PayerID").GetCurrentValue()))
                {
                    fundedField.SetCurrentValue("FVoff");
                }
                else if (productTypeField != null && (productTypeField.GetCurrentValue() == "EOBLITE" || productTypeField.GetCurrentValue() == "BOTH"))
                {
                    fundedField.SetCurrentValue("Nolink");
                }
                else
                {
                    fundedField.SetCurrentValue("Automatch");
                }

            }
            else if (bString2.Length > 0 && bString2.Contains(form.GetField("PayerID").GetCurrentValue()))
            {
                fundedField.SetCurrentValue("FVoff");

            }
            else if (bString3.Length > 0 && !bString3.Contains(form.GetField("PayerID").GetCurrentValue()))
            {
                fundedField.SetCurrentValue("FVoff");

            }
            else if (productTypeField != null && (productTypeField.GetCurrentValue() == "EOBLITE" || productTypeField.GetCurrentValue() == "BOTH"))
            {
                fundedField.SetCurrentValue("Nolink");

            }
            else if (form.FVFFileName.Contains("SingleCheck"))
            {
                fundedField.SetCurrentValue("Nolink");

            }
            else if (checkAmountField != null && form.FVFFileName.Contains("NoCheck") && checkAmount.Equals(0.00) )
            {
                fundedField.SetCurrentValue("Nolink");
            }
            else if (form.FVFFileName.Contains("NoCheck"))
            {
                fundedField.SetCurrentValue("Unfunded");
            }
            else
            {
                fundedField.SetCurrentValue("Funded");
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
