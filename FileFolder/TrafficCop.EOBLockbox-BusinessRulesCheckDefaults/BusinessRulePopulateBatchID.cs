using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;

namespace TrafficCop.EOBLockbox
{
    class BusinessRulePopulateBatchID : IBusinessRule
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
                DoRuleWork(form, xmlConfiguration);
            }
            else
            {
                foreach (string fileName in ab.fileName)
                {
                    string fdfFileName = ab.GetFDFNameFromImageName(fileName);
                    IFormObject form = ab.GetForm(fdfFileName);

                    DoRuleWork(form, xmlConfiguration);
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
        #region Business Rule Work
        private void DoRuleWork(IFormObject form, IApiXmlNode config)
        {
            string s_PopulateBatchIDBR_Value = xmlBatch.GetBatchFieldValue("PopulateBatchIDBR");
            if (s_PopulateBatchIDBR_Value == "True")
            {
                IField fld_BatchID = form.GetField("BatchID");
                if (fld_BatchID != null)
                {
                    string s_PopulateBatchIDType_Value = xmlBatch.GetBatchFieldValue("PopulateBatchIDType");
                    if (s_PopulateBatchIDType_Value == "DEFAULTVALUE")
                    {
                        fld_BatchID.SetCurrentValue(xmlBatch.GetBatchFieldValue("PopulateBatchIDDefault"));
                    }
                    else if (s_PopulateBatchIDType_Value.ToUpper() == "PACKAGEUNDERSCORE")
                    {
                        string packageName = xmlBatch.GetBatchDataNode("PackageName");
                        packageName = packageName.Substring(0, packageName.Length - 4);
                        string[] packageNameSplit = packageName.Split(new char[] { '_' });
                        int index = Convert.ToInt16(xmlBatch.GetBatchDataNode("PopulateBatchIDUnderscoreIndex"));
                        fld_BatchID.SetCurrentValue(packageNameSplit[index]);
                    }
                    else if (s_PopulateBatchIDType_Value.ToUpper() == "INTERNALBATCHID")
                    {
                        string batchIDValue = fld_BatchID.GetCurrentValue();
                        if (batchIDValue.Length == 0)
                        {
                            string batchIDNodeValue = xmlBatch.GetBatchDataNode("BatchID");
                            fld_BatchID.SetCurrentValue(batchIDNodeValue);
                        }
                    }
                }
            }

        }
        #endregion Business Rule Work
    }
}