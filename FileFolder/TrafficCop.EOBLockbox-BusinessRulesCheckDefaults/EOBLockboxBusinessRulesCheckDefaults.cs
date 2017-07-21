using System;
using System.Collections.Generic;
using System.Text;
using FvTech.Api;

namespace TrafficCop.EOBLockbox
{
    [APIAttribute("EOB Lockbox Check Default Business Rules", "Contains business rules for EOB Lockbox Check Default.")]
    public class EOBLockboxBusinessRulesCheckQueueRouting : IBusinessRules
    {
        const string DEFAULT_CHECK_AMOUNT = "EOB Lockbox Default Check Amount";
        const string POPULATE_CHECK_TRANSACTIONID = "EOB Lockbox Populate Check TransactionID";
        const string POPULATE_PAYER_LIVE = "EOB Lockbox Populate Payer Live";
        const string POPULATE_CHECK_PAYMENT_TYPE = "EOB Lockbox Populate Check Payment Type";
        const string POPULATE_CHECK_FUNDED = "EOB Lockbox Populate Check Funded";
        const string POPULATE_CHECK_PROCESSING_TYPES = "EOB Lockbox Populate Check Processing Types";
        const string POPULATE_POST = "EOB Lockbox Populate Post";
        const string POPULATE_CLIENT_CODE = "EOB Lockbox Populate Client Code";
        const string POPULATE_BATCH_ID = "EOB Lockbox Populate Batch ID";
        const string POPULATE_FACILITY_ID = "EOB Lockbox Populate Facility ID";
        const string POPULATE_LOCKBOX_ID = "EOB Lockbox Populate Lockbox ID";
        const string POPULATE_PRINT_SERVICES_ID = "EOB Lockbox Populate Print Services ID";
        const string POPULATE_FV_FACILITY = "EOB Lockbox Populate FV Facility";
        const string POPULATE_NOCHECK = "EOB Lockbox Populate NoCheck Type";
        /// <summary>
        /// Returns information on the business rules in this assembly
        /// </summary>
        public IBusinessRuleInfo[] GetBusinessRulesInfo()
        {
            return new BusinessRuleInfo[] 
            {                 
                new BusinessRuleInfo(DEFAULT_CHECK_AMOUNT, "Execute business rule to default check amount." ),
                new BusinessRuleInfo(POPULATE_CHECK_TRANSACTIONID, "Execute business rule to populate TransactionID field." ),
                new BusinessRuleInfo(POPULATE_PAYER_LIVE, "Execute business rule to populate Payer Live"),
                new BusinessRuleInfo(POPULATE_CHECK_PAYMENT_TYPE, "Execute business rule to populate the check payment type"),
                new BusinessRuleInfo(POPULATE_CHECK_FUNDED, "Execute business rule to populate check funded"),
                new BusinessRuleInfo(POPULATE_CHECK_PROCESSING_TYPES, "Execute business rule to populate check processing types"),
                new BusinessRuleInfo(POPULATE_POST, "Executes business rule to populate the post field."),
                new BusinessRuleInfo(POPULATE_CLIENT_CODE, "Executes business rule to populate the client code"),
                new BusinessRuleInfo(POPULATE_BATCH_ID, "Executes business rule to populate the batch ID"),
                new BusinessRuleInfo(POPULATE_FACILITY_ID, "Executes business rule to populate the facility ID"),
                new BusinessRuleInfo(POPULATE_LOCKBOX_ID, "Executes business rule to populate the lockbox ID"),
                new BusinessRuleInfo(POPULATE_PRINT_SERVICES_ID, "Executes business rule to populate the print services ID"),
                new BusinessRuleInfo(POPULATE_FV_FACILITY, "EOB Lockbox Populate FV Facility"),
                new BusinessRuleInfo(POPULATE_NOCHECK, "EOB Lockbox Populate NoCheck Type")
			};
        }

        /// <summary>
        /// Retrieves a particular business rule by name.
        /// </summary>
        public IBusinessRule GetBusinessRule(string name)
        {
            IBusinessRule businessRule = null;
            switch (name)
            {
                case DEFAULT_CHECK_AMOUNT:
                    businessRule = new BusinessRuleDefaultCheckAmount();
                    break;
                case POPULATE_CHECK_TRANSACTIONID:
                    businessRule = new BusinessRulePopulateCheckTransactionID();
                    break;
                case POPULATE_PAYER_LIVE:
                    businessRule = new BusinessRulePopulatePayerLive();
                    break;
                case POPULATE_CHECK_FUNDED:
                    businessRule = new BusinessRulePopulateCheckFunded();
                    break;
                case POPULATE_CHECK_PAYMENT_TYPE:
                    businessRule = new BusinessRulePopulatePaymentType();
                    break;
                case POPULATE_CHECK_PROCESSING_TYPES:
                    businessRule = new BusinessRulePopulateCheckProcessingType();
                    break;
                case POPULATE_POST:
                    businessRule = new BusinessRulePopulatePost();
                    break;
                case POPULATE_CLIENT_CODE:
                    businessRule = new BusinessRulePopulateClientCode();
                    break;
                case POPULATE_BATCH_ID:
                    businessRule = new BusinessRulePopulateBatchID();
                    break;
                case POPULATE_FACILITY_ID:
                    businessRule = new BusinessRulePopulateFacilityID();
                    break;
                case POPULATE_LOCKBOX_ID:
                    businessRule = new BusinessRulePopulateLockboxID();
                    break;
                case POPULATE_PRINT_SERVICES_ID:
                    businessRule = new BusinessRulePopulatePrintServicesID();
                    break;
                case POPULATE_FV_FACILITY:
                    businessRule = new BusinessRulePopulateFVFacility();
                    break;
                case POPULATE_NOCHECK:
                    businessRule = new BusinessRulePopulateNoCheckType();
                    break;
            }
            return businessRule;
        }
    }
}
