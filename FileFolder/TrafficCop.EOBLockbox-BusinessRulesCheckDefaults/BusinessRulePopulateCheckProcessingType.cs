using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;

namespace TrafficCop.EOBLockbox
{
    [APIAttribute("EOB Lockbox Populate Check Processing Type", "Contains business rules for EOB Lockbox Populate Check Processing Type.")]
    public class BusinessRulePopulateCheckProcessingType : IBusinessRule
    {
        private IApiAgentBusiness ab = PluginAssemblyManager.Instance().GetInterface<IApiAgentBusiness>();

        public void Execute(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            IBatchConfigurationXml xmlBatch = ((IApiXmlNode)args["Batch"]).GetXmlNavigator<IBatchConfigurationXml>();

            if (args.ContainsKey("form"))
            {
                IFormObject form = (IFormObject)args["form"];
                IField temp = form.GetField("ProcessingType");
                if (temp != null)
                    SetCheckProcessingType(form, xmlBatch);
            }
            else
            {
                string[] imageFilenames = ab.fileName;
                for (int j = 0; j < imageFilenames.Length; j++)
                {

                    string fdfFilename = ab.GetFDFNameFromImageName(imageFilenames[j]);
                    IFormObject form = ab.GetForm(fdfFilename);
                    IField temp = form.GetField("ProcessingType");
                    if (temp != null)
                    {
                        SetCheckProcessingType(form, xmlBatch);
                    }
                }
            }
            if (!args.ContainsKey("ObjectModel"))
                args.Add("ObjectModel", true);
            else
                args["ObjectModel"] = true;

        }

        public void SetCheckProcessingType(IFormObject form, IBatchConfigurationXml xmlBatch)
        {
            IField fldProcessingType = form.GetField("ProcessingType");
            if (fldProcessingType == null)
            {
                ThrowErrorException("ProcessingType virtual field missing");
            }

            string classification = form.FVFFileName;
            string batchDataNodeProcessingTypeDefault = xmlBatch.GetBatchDataNode("ProcessingTypeDefault");

            if (!String.IsNullOrEmpty(batchDataNodeProcessingTypeDefault))
            {
                fldProcessingType.SetCurrentValue(batchDataNodeProcessingTypeDefault);
            }

            IField fldProductType = form.GetField("ProductType");
            if (classification.Contains("_PatientPay"))
            {
                if (BatchIdIsSpecial(form.GetField("BatchID")))
                {
                    fldProcessingType.SetCurrentValue("PATPAYNC");
                }
                else
                {
                    fldProcessingType.SetCurrentValue("PATPAY");
                }
            }
            else
                if (classification.Contains("_Correspondence"))
                {
                    fldProcessingType.SetCurrentValue("CORR");
                }
                else
                    if (fldProductType != null && fldProductType.GetCurrentValue().ToUpper().Equals("EOBLITE"))
                    {
                        fldProcessingType.SetCurrentValue("EOBL");
                    }
                    else
                    {
                        fldProcessingType.SetCurrentValue("EOBC");
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

        private bool BatchIdIsSpecial(IField batchIDField)
        {
            if (batchIDField != null)
            {
                string batchIDValue = batchIDField.GetCurrentValue().Trim();
                int batchID;
                if (int.TryParse(batchIDValue, out batchID))
                {
                    if (batchID >= 90000 && batchID <= 909999)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
