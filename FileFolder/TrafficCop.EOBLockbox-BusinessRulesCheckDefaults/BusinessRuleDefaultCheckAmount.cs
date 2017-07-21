using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;

namespace TrafficCop.EOBLockbox
{
    [APIAttribute("EOB Lockbox Default Check Amount", "EOB Lockbox Default Check Amount.")]

    /// <summary>
    /// CR 24118
    /// </summary>
    public class BusinessRuleDefaultCheckAmount : IBusinessRule
    {
        private IApiAgentBusiness ab = PluginAssemblyManager.Instance().GetInterface<IApiAgentBusiness>();

        public void Execute(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            IBatchConfigurationXml xmlBatch = ((IApiXmlNode)args["Batch"]).GetXmlNavigator<IBatchConfigurationXml>();


            if (xmlBatch.GetBatchFieldValue("CheckAmountDefaultBR") != null &&
                xmlBatch.GetBatchFieldValue("CheckAmountDefaultBR").ToUpper().Equals("TRUE"))
            {

                if (args.ContainsKey("form"))
                {
                    IFormObject form = (IFormObject)args["form"];
                    DefaultCheckAmount(form, xmlBatch);
                }
                else
                {
                    string[] imageFilenames = ab.fileName;
                    for (int j = 0; j < imageFilenames.Length; j++)
                    {
                        string fdfFilename = ab.GetFDFNameFromImageName(imageFilenames[j]);
                        IFormObject form = ab.GetForm(fdfFilename);
                        DefaultCheckAmount(form, xmlBatch);
                    }
                }

            }

            if (!args.ContainsKey("ObjectModel"))
                args.Add("ObjectModel", true);
            else
                args["ObjectModel"] = true;


        }

        public void DefaultCheckAmount(IFormObject form, IBatchConfigurationXml xmlBatch)
        {
            if (!form.FVFFileName.ToUpper().Contains("NOCHECK"))
            {
                return;
            }
            IField checkAmountField = form.GetField("Check_Amount");
            if (checkAmountField != null)
            {
                string checkAmountDefaultType = xmlBatch.GetBatchFieldValue("CheckAmountDefaultType");
                if (checkAmountDefaultType.Trim().ToUpper().Equals("ZERO"))
                {
                    string checkAmountDefaultSkipReview = xmlBatch.GetBatchFieldValue("CheckAmountDefaultSkipReview");
                    if (checkAmountDefaultSkipReview.Trim().ToUpper().Equals("TRUE"))
                    {
                        SetPrepopulatedCheckFields(form);
                    }
                }
            }
        }

        public void SetPrepopulatedCheckFields(IFormObject form)
        {
            string prepopulatedCheckFields = form.GetField("PrepopulatedCheckFields").GetCurrentValue();
            if (prepopulatedCheckFields.Length > 0)
            {
                if (prepopulatedCheckFields.EndsWith(","))
                {
                    prepopulatedCheckFields = prepopulatedCheckFields + "Check_Amount";
                }
                else
                {
                    prepopulatedCheckFields = prepopulatedCheckFields + ",Check_Amount";
                }
            }
            else
            {
                prepopulatedCheckFields = "Check_Amount";
            }
            form.GetField("PrepopulatedCheckFields").SetCurrentValue(prepopulatedCheckFields);
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
