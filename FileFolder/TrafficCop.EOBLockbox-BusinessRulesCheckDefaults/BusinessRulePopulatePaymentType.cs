using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;
using TrafficCop.Batch;

namespace TrafficCop.EOBLockbox
{
    [APIAttribute("EOB Lockbox Populate Payer Live", "Contains business rules for EOB Lockbox Payment Type.")]

    /// <summary>
    /// CR 22222. Runs by form or by batch. 
    ///called: "Populate Check Payment Type"    
    ///Onlyy runs if CheckPaymentType exists.
    ///If the form classification contains "_Check" or _SingleCheck" AND CheckType virtual field is not set to "ACH"
    /// Set the CheckPaymentType virtual field to "CHK"
    ///If the form classificaiton contains "_PatientPay
    /// Set the CheckPaymenType virtual field to "PatPay"
    ///ELSE
    /// set CheckPaymentType to "NON"

    /// </summary>
    public class BusinessRulePopulatePaymentType : IBusinessRule
    {
        private IApiAgentBusiness ab = PluginAssemblyManager.Instance().GetInterface<IApiAgentBusiness>();

        public void Execute(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            IBatchConfigurationXml xmlBatch = ((IApiXmlNode)args["Batch"]).GetXmlNavigator<IBatchConfigurationXml>();

            if (args.ContainsKey("form"))
            {
                IFormObject form = (IFormObject)args["form"];

                ///Onlyy runs if CheckPaymentType exists.
                if (form.GetField("CheckPaymentType") != null)
                {
                    SetPaymentType(form, xmlBatch);
                }
            }
            else
            {
                string[] imageFilenames = ab.fileName;
                for (int j = 0; j < imageFilenames.Length; j++)
                {
                    string fdfFilename = ab.GetFDFNameFromImageName(imageFilenames[j]);
                    IFormObject form = ab.GetForm(fdfFilename);

                    ///Onlyy runs if CheckPaymentType exists.
                    if (form.GetField("CheckPaymentType") != null)
                    {
                        SetPaymentType(form, xmlBatch);
                    }
                }
            }
            if (!args.ContainsKey("ObjectModel"))
                args.Add("ObjectModel", true);
            else
                args["ObjectModel"] = true;

        }

        public void SetPaymentType(IFormObject form, IBatchConfigurationXml xmlBatch)
        {
            IField paymentTypeField = form.GetField("CheckPaymentType");
            int batchID = -1;
            Int32.TryParse(form.GetField("BatchID").GetCurrentValue(), out batchID);

            if (paymentTypeField.GetCurrentValue().ToUpper().Contains("ACH")) return;
            if (form.FVFFileName.Contains("_Check") | form.FVFFileName.Contains("_SingleCheck"))
            {
                SetTypeToCHKorECL(paymentTypeField, xmlBatch);
            }
            else if (form.FVFFileName.Contains("_PatientPay"))
            {
                if (batchID >= 90000 && batchID <= 99999)
                {
                    paymentTypeField.SetCurrentValue("NON");
                }
                else if (batchID >= 14000 && batchID <= 14999)
                {
                    SetTypeToCHKorECL(paymentTypeField, xmlBatch);
                }
                else
                {
                    paymentTypeField.SetCurrentValue("PatPay");
                }
            }
            else
            {
                paymentTypeField.SetCurrentValue("NON");
            }
        }

        public IConfigurationPage GetConfigurationPage(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            return null;
        }


        private void SetTypeToCHKorECL(IField paymentTypeField, IBatchConfigurationXml xmlBatch)
        {
            string scanCenter = xmlBatch.GetBatchDataNode("ScanCenter");
            if (scanCenter.Trim().ToUpper().Equals("ABF"))
            {
                paymentTypeField.SetCurrentValue("ECL");
            }
            else
            {
                paymentTypeField.SetCurrentValue("CHK");
            }
        }
    }
}
