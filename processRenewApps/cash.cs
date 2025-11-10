using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics; 

namespace processRenewApps
{
    public class cash
    {
        
        
        public static bool handleCash(List<RClassLibrary.renewal> renewals, logger log, itemCreator creator)
        {
            log.log("processRenewApps handleCash begin");

            decimal totalAmount = 0m;
            decimal orderTotal = 0m;
            decimal amount = 0m;
            string cshRevenueDefault = "";

            // display the dudes
            foreach (RClassLibrary.renewal renewal in renewals)
            {
                log.log(string.Format("Trace: {0}, late fee {1} amount due {2}", renewal.trace_number , renewal .lateFee.ToString("C"), renewal.totDue.ToString("C")));
            }

            try
            {
                //using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
                //{
                    // get sys cash param 
                    string dbname = System.Configuration.ConfigurationManager.AppSettings["linqDB"];
                    Tracerdb.SQLTracerDev db = new Tracerdb.SQLTracerDev(dbname);
                    Tracerdb.SYS_CASH_PARAM scp = DataAccess.getSCP(db);
                    log.log(string.Format("Last_bat_num {0}", scp.LAST_BAT_NUM.ToString()));

                    try
                    {
                        int temp = scp.LAST_BAT_NUM ?? 0;
                        temp = temp + 1;
                        log.log("New last bat num = " + temp.ToString());
                        scp.LAST_BAT_NUM = temp;
                    }
                    catch (Exception e)
                    {
                        log.log("Some problem with batch number! " + e.Message + " " + new StackFrame(0).GetMethod().Name);
                        return false;
                    }
                    //
                    //develop the last 4 chars of batch
                    string batchNum = scp.LAST_BAT_NUM.ToString();
                    if (batchNum.Length == 1) batchNum = "000" + batchNum;
                    else if (batchNum.Length == 2) batchNum = "00" + batchNum;
                    else if (batchNum.Length == 3) batchNum = "0" + batchNum;

                    Tracerdb.SYS_COMMON_PARAM common = DataAccess.getCommon(db);
                    log.log("fiscal year = " + common.CUR_FISCAL_YEAR);

                    batchNum = common.CUR_FISCAL_YEAR + batchNum;
                    log.log("batchNum = " + batchNum);

                    //mess with the last_cshrct_num - what is this even used for?
                    Tracerdb.SYS_TXONLINE_PARAM online = DataAccess.getOnline(db);
                    log.log("Last_cshrct_num: " + online.LAST_CSHRCT_NUM.ToString());

                    int last_cshrct_num = int.Parse(online.LAST_CSHRCT_NUM);
                    int first_cshrct_num = last_cshrct_num + 1;

                    //write stinking cash records
                    int count = 0;

                    //
                    foreach (RClassLibrary.renewal renewal in renewals)
                    {
                        //rebuild the lineitems
                        List<NICUSA.orderItem> orderItems = creator.createOrderItems(renewal);
                        //add up the money                
                        foreach (NICUSA.orderItem oi in orderItems) //yes there is only one as of 2_1_17
                        {
                            totalAmount = totalAmount + oi.unitprice;
                            log.log("Rev code: " + oi.sku + " unitprice: " + oi.unitprice.ToString("C"));
                            //amount = amount + oi.unitprice;
                        }
                        //continue
                        count = 0;
                        //calculate orderTotal
                        orderTotal = 0m;
                        foreach (NICUSA.orderItem oi in orderItems) //yes there is only one as of 2_1_17
                        {
                            orderTotal = orderTotal + oi.unitprice;
                            //amount = amount + oi.unitprice;
                        }


                        foreach (NICUSA.orderItem oi in orderItems)
                        {
                            cshRevenueDefault = oi.sku;   //yes, will be done many times - only needed once - but it don't care                    

                            if (orderItems.Count == 1)
                            {

                                Tracerdb.CASH cash = new Tracerdb.CASH();
                                cash.CSHBAT_NUM = batchNum;
                                cash.ID_NUM = renewal.id_num;
                                last_cshrct_num++;
                                cash.CSHRCT_NUM_MAIN = last_cshrct_num.ToString();
                                cash.CSHRCT_NUM_SUB = "0000";
                                cash.CSHAMT = oi.unitprice;                       //this works only if 1 orderItem
                                cash.CSHREVENUE_CD = oi.sku;
                                cash.CSHTAX_FLAG = "N";
                                cash.CSHTAX_AMT = 0M;
                                cash.CSHFLAG_MULT = "N";
                                cash.CSHRCT_TYPE = "W";
                                cash.OnlineTraceNum = renewal.trace_number;
                                cash.OnlineTraceType = renewal.LicType;

                                try
                                {
                                    cash.CSHRCT_DT = DateTime.Parse(renewal.TRANSACTION_DATE);
                                }
                                catch (Exception e)
                                {
                                    cash.CSHRCT_DT = DateTime.Now;
                                }

                                cash.CSHHB1009_AMT = 0m;
                                cash.CSHADJ_CD = "0";
                                cash.CSHRCT_FIRST_OPER = "processRenewApps";
                                cash.CSHRCT_FIRST_DT = DateTime.Now;
                                cash.CSHRCT_LAST_OPER = "processRenewApps";
                                cash.CSHRCT_LAST_DT = DateTime.Now;
                                db.CASH.InsertOnSubmit(cash);
                                count = 0; //
                            }
                            //
                            if (orderItems.Count > 1)
                            {
                                if (count == 0)
                                {
                                    //write main mrecord
                                    Tracerdb.CASH cash = new Tracerdb.CASH();
                                    cash.CSHBAT_NUM = batchNum;
                                    cash.ID_NUM = renewal.id_num;
                                    last_cshrct_num++;
                                    cash.CSHRCT_NUM_MAIN = last_cshrct_num.ToString();
                                    cash.CSHRCT_NUM_SUB = "0000";
                                    cash.CSHAMT = orderTotal;
                                    cash.CSHREVENUE_CD = oi.sku; //this is the revenue code
                                    cash.CSHTAX_FLAG = "N";
                                    cash.CSHTAX_AMT = 0M;
                                    cash.CSHFLAG_MULT = "Y";
                                    cash.CSHRCT_TYPE = "W";
                                    cash.OnlineTraceNum = renewal.trace_number;
                                    cash.OnlineTraceType = renewal.LicType;
                                    try
                                    {
                                        cash.CSHRCT_DT = DateTime.Parse(renewal.TRANSACTION_DATE).Date;
                                    }
                                    catch
                                    {
                                        cash.CSHRCT_DT = DateTime.Now;
                                    }
                                    cash.CSHHB1009_AMT = 0m;
                                    cash.CSHADJ_CD = "0";
                                    cash.CSHRCT_FIRST_OPER = "processRenewApps";
                                    cash.CSHRCT_FIRST_DT = DateTime.Now;
                                    cash.CSHRCT_LAST_OPER = "processRenewApps";
                                    cash.CSHRCT_LAST_DT = DateTime.Now;
                                    db.CASH.InsertOnSubmit(cash);


                                    //write first sub record
                                    cash = new Tracerdb.CASH();

                                    //write first sub
                                    cash.CSHBAT_NUM = batchNum;
                                    cash.ID_NUM = renewal.id_num;
                                    //cshrct_num is crazy complex, try to keep up  -  don't increment                        
                                    cash.CSHRCT_NUM_MAIN = (last_cshrct_num).ToString();

                                    cash.CSHRCT_NUM_SUB = "0001";
                                    cash.CSHAMT = oi.unitprice;             //cshamt
                                    cash.CSHREVENUE_CD = oi.sku;  // this is revenue code
                                    cash.CSHTAX_FLAG = "N";
                                    cash.CSHTAX_AMT = 0m;
                                    cash.CSHFLAG_MULT = "N";
                                    cash.CSHRCT_TYPE = "W";
                                    cash.OnlineTraceNum = renewal.trace_number;
                                    cash.OnlineTraceType = renewal.LicType;
                                    try
                                    {
                                        cash.CSHRCT_DT = DateTime.Parse(renewal.TRANSACTION_DATE).Date;
                                    }
                                    catch
                                    {
                                        log.log("First sub record transaction date corrupt: " + renewal.TRANSACTION_DATE);
                                        cash.CSHRCT_DT = DateTime.Now;
                                    }

                                    cash.CSHHB1009_AMT = 0m;
                                    cash.CSHADJ_CD = "0";
                                    cash.CSHRCT_FIRST_OPER = "processRenewApps";
                                    cash.CSHRCT_FIRST_DT = DateTime.Now;
                                    cash.CSHRCT_LAST_OPER = "processRenewApps";
                                    cash.CSHRCT_LAST_DT = DateTime.Now;

                                    db.CASH.InsertOnSubmit(cash);
                                    // bump sub
                                    count = 1;
                                } // end if sub = 0
                                else // count is greater than zero
                                {
                                    count++;
                                    Tracerdb.CASH cash = new Tracerdb.CASH();

                                    cash.CSHBAT_NUM = batchNum;
                                    cash.ID_NUM = renewal.id_num;

                                    //cshrct_num is crazy complex, try to keep up                           
                                    cash.CSHRCT_NUM_MAIN = last_cshrct_num.ToString();
                                    //
                                    if (count.ToString().Length == 1) cash.CSHRCT_NUM_SUB = "000" + count.ToString();
                                    else if (count.ToString().Length == 2) cash.CSHRCT_NUM_SUB = "00" + count.ToString();

                                    cash.CSHAMT = oi.unitprice;           //cshamt
                                    cash.CSHREVENUE_CD = oi.sku;
                                    cash.CSHTAX_FLAG = "N";
                                    cash.CSHTAX_AMT = 0m;              
                                    cash.CSHFLAG_MULT = "N";
                                    cash.CSHRCT_TYPE = "W";
                                    cash.OnlineTraceNum = renewal.trace_number;
                                    cash.OnlineTraceType = renewal.LicType;
                                    try
                                    {

                                        cash.CSHRCT_DT = DateTime.Parse(renewal.TRANSACTION_DATE).Date;
                                    }
                                    catch
                                    {
                                        cash.CSHRCT_DT = DateTime.Now;
                                    }

                                    cash.CSHHB1009_AMT = 0m;
                                    cash.CSHADJ_CD = "0";
                                    cash.CSHRCT_FIRST_OPER = "processRenewApps";
                                    cash.CSHRCT_FIRST_DT = DateTime.Now;
                                    cash.CSHRCT_LAST_OPER = "processRenewApps";
                                    cash.CSHRCT_LAST_DT = DateTime.Now;

                                    db.CASH.InsertOnSubmit(cash);
                                } // count is greater than zero
                            }
                        }
                    }// 

                    // need to write the Tracerdb.SYS_TXONLINE_PARAM online = phyAppCL.DataAccess.getOnline(db);
                    online.LAST_CSHRCT_NUM = last_cshrct_num.ToString();

                    // need batch dude
                    Tracerdb.BATCH_HEADER bh = new Tracerdb.BATCH_HEADER
                    {
                        CSHBAT_NUM = batchNum,
                        CSHBAT_DT = DateTime.Now,
                        CSHBATTAPETOT = totalAmount,
                        CSHDEPOSIT = totalAmount,
                        //CSHREVENUE_DEFAULT = "6101",
                        CSHREVENUE_DEFAULT = cshRevenueDefault,
                        CSHBATBAL_YN = "Y",
                        CSHBATBEGRCT_NUM = first_cshrct_num.ToString(),
                        CSHBATENDRCT_NUM = last_cshrct_num.ToString(),
                        CSHVOID = 0.0M,
                        CSHDEP_NUM = 0,
                        CSHBAT_FIRST_DT = DateTime.Now,
                        CSHBAT_FIRST_OPER = "processRenewApps",
                        CSHBAT_LAST_DT = DateTime.Now,
                        CSHBAT_LAST_OPER = "processRenewApps"

                    };
                    db.BATCH_HEADER.InsertOnSubmit(bh);
                    //
                    db.SubmitChanges();              //this acts like a transaction complete
                    log.log("success: write2DB");
                //    scope.Complete();
                //}//using 
                return true;
            }//try
            catch (Exception e)
            {
                log.log ("error: handle Cash: " + e.Message );
                return false;
            }
        }
        
    }
}
