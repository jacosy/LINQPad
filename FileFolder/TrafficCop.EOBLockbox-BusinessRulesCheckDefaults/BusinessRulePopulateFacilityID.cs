using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;

namespace TrafficCop.EOBLockbox
{
    class BusinessRulePopulateFacilityID : IBusinessRule
    {
        IBatchConfigurationXml xmlBatch;
        private IApiAgentBusiness ab = PluginAssemblyManager.Instance().GetInterface<IApiAgentBusiness>();

        #region Interface Members
        public void Execute(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            xmlBatch = ((IApiXmlNode)args["Batch"]).GetXmlNavigator<IBatchConfigurationXml>();

            if (args.ContainsKey("form"))
            {
                IFormObject form = (IFormObject)args["form"];
                PopulateFacilityID(form, xmlBatch);
            }
            else
            {
                foreach (string fileName in ab.fileName)
                {
                    string fdfFileName = ab.GetFDFNameFromImageName(fileName);
                    IFormObject form = ab.GetForm(fdfFileName);

                    PopulateFacilityID(form, xmlBatch);
                }

                if (!args.ContainsKey("ObjectModel"))
                    args.Add("ObjectModel", true);
                else
                    args["ObjectModel"] = true;
            }
        }
        public IConfigurationPage GetConfigurationPage(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            return null;
        }
        #endregion Interface Members

        private void PopulateFacilityID(IFormObject form, IBatchConfigurationXml xmlBatch)
        {
            IField facilityIDField = form.GetField("FacilityID");
            if (facilityIDField == null)
                return;

            string facilityIDType = xmlBatch.GetBatchDataNode("FacilityIDType").ToUpper();
            if (facilityIDType.Length > 0 && facilityIDType != "DEFAULTNPI")
                throw new Exception("FacilityIDType batch data node is not configured to a valid type.");
                        
                
            if (facilityIDType == "DEFAULTNPI" || string.IsNullOrEmpty(facilityIDType))
            {
                string defaultProviderNPI = xmlBatch.GetBatchDataNode("DefaultProviderNPI");
                facilityIDField.SetCurrentValue(defaultProviderNPI);
            }


        }
       
    }
}