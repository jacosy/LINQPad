using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;

namespace TrafficCop.EOBLockbox
{
    class BusinessRulePopulateLockboxID : IBusinessRule
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
                PopulateLockboxID(form, xmlBatch);
            }
            else
            {
                foreach (string fileName in ab.fileName)
                {
                    string fdfFileName = ab.GetFDFNameFromImageName(fileName);
                    IFormObject form = ab.GetForm(fdfFileName);

                    PopulateLockboxID(form, xmlBatch);
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

        private void PopulateLockboxID(IFormObject form, IBatchConfigurationXml xmlBatch)
        {
            IField lockboxIDField = form.GetField("LockboxID");
            if (lockboxIDField == null)
                return; 
            
            string populateLockboxIDBR = xmlBatch.GetBatchDataNode("PopulateLockboxIDBR").ToUpper();
            if (populateLockboxIDBR != "TRUE")
            {
                if (lockboxIDField.GetCurrentValue() == "")
                    lockboxIDField.SetCurrentValue(xmlBatch.GetBatchDataNode("PlanName"));
                
                return;
            }
            string lockboxIDType = xmlBatch.GetBatchDataNode("PopulateLockboxIDType").ToUpper();
            if (lockboxIDType != "PLAN" && lockboxIDType != "PLANREMOVECHARS")
                throw new Exception("PopulateLockboxIDType batch data node is not configured to a valid type.");


            if (lockboxIDType == "PLAN")
            {
                string planName = xmlBatch.GetBatchDataNode("PlanName");
                lockboxIDField.SetCurrentValue(planName);
            }
            else if (lockboxIDType == "PLANREMOVECHARS")
            {
                string planName = xmlBatch.GetBatchDataNode("PlanName");
                string removeCharsBegin = xmlBatch.GetBatchDataNode("PopulateLockboxIDRemoveCharsBegin");
                string removeCharsEnd = xmlBatch.GetBatchDataNode("PopulateLockboxIDRemoveCharsEnd");

                if (planName.StartsWith(removeCharsBegin))
                    planName = planName.Substring(removeCharsBegin.Length, planName.Length - removeCharsBegin.Length);
                if (planName.EndsWith(removeCharsEnd))
                    planName = planName.Substring(0, planName.Length - removeCharsEnd.Length);

                lockboxIDField.SetCurrentValue(planName);
            }
        }
    }
}