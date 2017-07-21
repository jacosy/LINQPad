using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;

namespace TrafficCop.EOBLockbox
{
    [APIAttribute("EOB Lockbox Populate FV Facility", "Contains business rules for EOB Lockbox Populate FV Facility.")]
    public class BusinessRulePopulateFVFacility : IBusinessRule
    {
        private IApiAgentBusiness ab = PluginAssemblyManager.Instance().GetInterface<IApiAgentBusiness>();

        public void Execute(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            IBatchConfigurationXml xmlBatch = ((IApiXmlNode)args["Batch"]).GetXmlNavigator<IBatchConfigurationXml>();

            if (args.ContainsKey("form"))
            {
                IFormObject form = (IFormObject)args["form"];

                if (form.GetField("FVFacility") != null)
                    SetFVFacility(form, xmlBatch);
            }
            else
            {
                string[] imageFilenames = ab.fileName;
                for (int j = 0; j < imageFilenames.Length; j++)
                {
                    string fdfFilename = ab.GetFDFNameFromImageName(imageFilenames[j]);
                    IFormObject form = ab.GetForm(fdfFilename);
                    if (form.GetField("FVFacility") != null)
                    {
                        SetFVFacility(form, xmlBatch);
                    }
                }
            }
            if (!args.ContainsKey("ObjectModel"))
                args.Add("ObjectModel", true);
            else
                args["ObjectModel"] = true;
        }

        public void SetFVFacility(IFormObject form, IBatchConfigurationXml xmlBatch)
        {
            IField fVFacilityField = form.GetField("FVFacility");
            if (fVFacilityField == null)
            {
                return;
            }

            if (xmlBatch.GetBatchDataNode("PopulateFVFacilityBR").ToUpper() != "TRUE")
            {
                return;
            }

            string facilityType = xmlBatch.GetBatchDataNode("PopulateFVFacilityType");
            string facilityDefault = xmlBatch.GetBatchDataNode("PopulateFVFacilityDefault");
            if (facilityType.Equals("PROVIDERNPI"))
            {
                string providerNpi = form.GetField("ProviderNPI").GetCurrentValue();
                    if( providerNpi.Length == 0 )
                        providerNpi = xmlBatch.GetBatchDataNode("DefaultProviderNPI");
                fVFacilityField.SetCurrentValue(providerNpi);
            }
            else if (facilityType.Equals("DEFAULT"))
            {
                fVFacilityField.SetCurrentValue(facilityDefault);
            }
            else if (facilityType.Equals("PROVIDERTAXIDMAP"))
            {
                bool match = false;
                string populateFVFacilityProviderTaxIDMapList = xmlBatch.GetBatchDataNode("PopulateFVFacilityProviderTaxIDMapList");
                IField providerTaxIDField = form.GetField("ProviderTaxID");
                string providerTaxIDToMatch = providerTaxIDField.GetCurrentValue();
                string[] providerTaxIDFacilityPair = populateFVFacilityProviderTaxIDMapList.Split('|');
                foreach (string pair in providerTaxIDFacilityPair)
                {
                    string[] providerTaxIDAndFacility = pair.Split(',');
                    if (providerTaxIDAndFacility.Length > 1)
                    {
                        string providerTaxID = providerTaxIDAndFacility[0];
                        string facility = providerTaxIDAndFacility[1];
                        if (providerTaxID == providerTaxIDToMatch)
                        {
                            fVFacilityField.SetCurrentValue(facility);
                            match = true;
                            return;
                        }
                    }
                }
                if (match == false)
                {
                    string defaultFacility = xmlBatch.GetBatchDataNode("PopulateFVFacilityProviderTaxIDMapDefault");
                    fVFacilityField.SetCurrentValue(defaultFacility);
                }
            }
            else if (facilityType.Equals("PROVIDERTAXIDNPIMAP"))
            {
                bool match = false;
                string populateFVFacilityProviderTaxIDNPIMapList = xmlBatch.GetBatchDataNode("PopulateFVFacilityProviderTaxIDNPIMapList");
                IField providerTaxIDField = form.GetField("ProviderTaxID");
                IField providerNPIField = form.GetField("ProviderNPI");
                string providerTaxIDToMatch = providerTaxIDField.GetCurrentValue();
                string providerNPIToMatch = providerNPIField.GetCurrentValue();
                string[] providerTaxIDNPIFacilityPair = populateFVFacilityProviderTaxIDNPIMapList.Split('|');
                foreach (string pair in providerTaxIDNPIFacilityPair)
                {
                    string[] providerTaxIDNPIFacility = pair.Split(',');
                    if (providerTaxIDNPIFacility.Length > 2)
                    {
                        string providerTaxID = providerTaxIDNPIFacility[0];
                        string providerNPI = providerTaxIDNPIFacility[1];
                        string facility = providerTaxIDNPIFacility[2];
                        if (providerTaxID == providerTaxIDToMatch && providerNPI == providerNPIToMatch)
                        {
                            fVFacilityField.SetCurrentValue(facility);
                            match = true;
                            return;
                        }
                    }
                }
                if (match == false)
                {
                    string defaultFacility = xmlBatch.GetBatchDataNode("PopulateFVFacilityProviderTaxIDNPIMapDefault");
                    fVFacilityField.SetCurrentValue(defaultFacility);
                }
            }
            else if (facilityType.Equals("BANKACCOUNTMAP"))
            {
                bool match = false;
                string populateFVFacilityBankAccountMapList = xmlBatch.GetBatchDataNode("PopulateFVFacilityBankAccountMapList");
                IField accountNumberField = form.GetField("AccountNumber");
                string accountNumberToMatch = accountNumberField.GetCurrentValue().TrimStart('0');
                string[] accountNumberFacilityPair = populateFVFacilityBankAccountMapList.Split('|');
                foreach (string pair in accountNumberFacilityPair)
                {
                    string[] accountNumberFacility = pair.Split(',');
                    if (accountNumberFacility.Length > 1)
                    {
                        string accountNumber = accountNumberFacility[0];
                        string facility = accountNumberFacility[1];
                        if (accountNumber == accountNumberToMatch)
                        {
                            fVFacilityField.SetCurrentValue(facility);
                            match = true;
                            return;
                        }
                    }
                }
                if (match == false)
                {
                    string defaultFacility = xmlBatch.GetBatchDataNode("PopulateFVFacilityBankAccountMapDefault");
                    fVFacilityField.SetCurrentValue(defaultFacility);
                }
            }
            else if (facilityType.Equals("LOCKBOXMAP"))
            {
                bool match = false;
                string populateFVFacilityLockboxMapList = xmlBatch.GetBatchDataNode("PopulateFVFacilityLockboxMapList");
                string planNameToMatch = xmlBatch.GetBatchDataNode("PlanName");
                string[] lockboxFacilityPair = populateFVFacilityLockboxMapList.Split('|');
                foreach (string pair in lockboxFacilityPair)
                {
                    string[] planNameFacility = pair.Split(',');
                    if (planNameFacility.Length > 1)
                    {
                        string planName = planNameFacility[0];
                        string facility = planNameFacility[1];
                        if (planName == planNameToMatch)
                        {
                            fVFacilityField.SetCurrentValue(facility);
                            match = true;
                            return;
                        }
                    }
                }
                if (match == false)
                    fVFacilityField.SetCurrentValue(facilityDefault);
            }
        }

        public IConfigurationPage GetConfigurationPage(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            return null;
        }
    }
}