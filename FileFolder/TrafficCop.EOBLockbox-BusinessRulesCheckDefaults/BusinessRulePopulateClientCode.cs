using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;
namespace TrafficCop.EOBLockbox
{
    class BusinessRulePopulateClientCode : IBusinessRule
    {
        private IApiAgentBusiness ab = PluginAssemblyManager.Instance().GetInterface<IApiAgentBusiness>();
        IBatchConfigurationXml xmlBatch;

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
        #endregion Interface Members
        #region Business Rule Work
        private void DoRuleWork(IFormObject form, IApiXmlNode config)
        {
            string s_PopulateClientCodeBR_Value = xmlBatch.GetBatchFieldValue("PopulateClientCodeBR");
            if (s_PopulateClientCodeBR_Value == "True")
            {
                IField Fld_ClientCode = form.GetField("ClientCode");
                if (Fld_ClientCode != null)
                {
                    string s_PopulateClientCodeType_Value = xmlBatch.GetBatchFieldValue("PopulateClientCodeType");
                    if (s_PopulateClientCodeType_Value == "DEFAULTFIRST3CHAR")
                    {
                        Fld_ClientCode.SetCurrentValue(form.FDFFileName.Substring(0, 3));
                    }
                    else if (s_PopulateClientCodeType_Value == "DEFAULTBETWEEN1STAND2NDUNDERSCORE")
                    {
                        string[] dash = form.FDFFileName.Split('_');
                        Fld_ClientCode.SetCurrentValue(dash[1]);
                    }
                }
            }
        }
        #endregion Business Rule Work
    }
}
