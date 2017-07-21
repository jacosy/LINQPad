using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;
using TrafficCop.Api;
using TrafficCop.Form;
using FvTech.Data;
using System.Data;
using TrafficCop.Data;
using System.Globalization;
using System.Xml;
using System.IO;


namespace TrafficCop.EOBLockbox
{
    public class BusinessRulePopulateCheckTransactionID : IBusinessRule
    {
        #region IBusinessRule Members

        private IApiAgentBusiness ab = PluginAssemblyManager.Instance().GetInterface<IApiAgentBusiness>();

        public void Execute(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            IBatchConfigurationXml xmlBatch = ((IApiXmlNode)args["Batch"]).GetXmlNavigator<IBatchConfigurationXml>();
            if (args.ContainsKey("form"))
            {
                IFormObject form = (IFormObject)args["form"];
                PopulateTransactionID(form, xmlBatch);
            }
            else
            {
                string[] imageFilenames = ab.fileName;
                for (int j = 0; j < imageFilenames.Length; j++)
                {
                    string fdfFilename = ab.GetFDFNameFromImageName(imageFilenames[j]);
                    IFormObject form = ab.GetForm(fdfFilename);
                    PopulateTransactionID(form, xmlBatch);
                }
            }
            if (!args.ContainsKey("ObjectModel"))
                args.Add("ObjectModel", true);
            else
                args["ObjectModel"] = true;

        }

        private void PopulateTransactionID(IFormObject form, IBatchConfigurationXml xmlBatch)
        {
            IField transactionIDField = form.GetField("TransactionID");
            IField PrintServicesIDField = form.GetField("PrintServicesID");
            if (transactionIDField != null && string.IsNullOrEmpty(transactionIDField.GetCurrentValue()))
            {
                if (PrintServicesIDField != null && PrintServicesIDField.GetCurrentValue().Length > 0 )
                {
                    transactionIDField.SetCurrentValue(PrintServicesIDField.GetCurrentValue());
                }
                else
                {
                    string sqlConnectionString = xmlBatch.GetBatchFieldValue("EOBLockboxDatabase");
                    SqlDatabaseConnection conn = new SqlDatabaseConnection(sqlConnectionString);
                    List<SerializableSqlParameter> sqlParams = new List<SerializableSqlParameter>();
                    DataTable table = conn.RunStoredProcedure("GenerateTransactionID", sqlParams).Tables[0];
                    if (table.Rows.Count > 0)
                        transactionIDField.SetCurrentValue(table.Rows[0][0].ToString());
                    else
                        throw new Exception("No data returned from 'GenerateTransactionID' stored proc");
                }
            }

            IField PerformSecondPSDBInsert = form.GetField("PerformSecondPSDBInsert");
            if (PerformSecondPSDBInsert != null && PerformSecondPSDBInsert.GetCurrentValue() != null && PerformSecondPSDBInsert.GetCurrentValue().ToUpper() == "TRUE")
            {
                IField transactionID2Field = form.GetField("TransactionID2");

                if (transactionID2Field != null && string.IsNullOrEmpty(transactionID2Field.GetCurrentValue()))
                {
                    string sqlConnectionString = xmlBatch.GetBatchFieldValue("EOBLockboxDatabase");
                    SqlDatabaseConnection conn = new SqlDatabaseConnection(sqlConnectionString);
                    List<SerializableSqlParameter> sqlParams = new List<SerializableSqlParameter>();
                    DataTable table = conn.RunStoredProcedure("GenerateTransactionID", sqlParams).Tables[0];
                    if (table.Rows.Count > 0)
                        transactionID2Field.SetCurrentValue(table.Rows[0][0].ToString());
                    else
                        throw new Exception("No data returned from 'GenerateTransactionID' stored proc");
                }
            }

            IField ERADuplicate = form.GetField("ERADuplicate");
            if (ERADuplicate != null && ERADuplicate.GetCurrentValue().ToUpper().Equals("TRUE") && form.FVFFileName.ToUpper().Contains("SINGLECHECK"))
            {
                IField transactionID2Field = form.GetField("TransactionID2");

                if (transactionID2Field != null && transactionIDField != null && string.IsNullOrEmpty(transactionID2Field.GetCurrentValue()))
                {

                    transactionID2Field.SetCurrentValue(transactionIDField.GetCurrentValue());
                    string sqlConnectionString = xmlBatch.GetBatchFieldValue("EOBLockboxDatabase");
                    SqlDatabaseConnection conn = new SqlDatabaseConnection(sqlConnectionString);
                    List<SerializableSqlParameter> sqlParams = new List<SerializableSqlParameter>();
                    DataTable table = conn.RunStoredProcedure("GenerateTransactionID", sqlParams).Tables[0];
                    if (table.Rows.Count > 0)
                        transactionIDField.SetCurrentValue(table.Rows[0][0].ToString());
                    else
                        throw new Exception("No data returned from 'GenerateTransactionID' stored proc");
                }

            }
        }

        public IConfigurationPage GetConfigurationPage(IApiXmlNode xmlConfiguration, EventArgsDictionary args)
        {
            return null;
        }

        #endregion

    }
}
