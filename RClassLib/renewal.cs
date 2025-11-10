using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RClassLibrary
{
    [Serializable]
    public class renewal
    {
        public string status { get; set; }
        public string methodOfPayment { get; set; }
        //
        public int id_num { get; set; }
        public string trace_number { get; set; }
        //public int FeeCode { get; set; }
        public string TRANSACTION_DATE { get; set; }
        public decimal AmtDue { get; set; }
        public decimal totDue { get; set; }

        // 4 new dq project
        public decimal lateFee { get; set; }     
        public decimal backFee { get; set; }
        // chg confirm login fee                     4
        // chg disclaimer fee display and logic     16 hours
        // chg to load to add all the fees           4
        // receipt page changes                      4

        public string Gender { get; set; }
        public DateTime LicenseIssueDate { get; set; }
        public DateTime LicenseExpirationDate { get; set; }
        public DateTime ThroughDate { get; set; }
        public string LicNum { get; set; }
        public string LicType { get; set; }
        public string SSN { get; set; }
        public string PAname { get; set; }
        public DateTime ExpireDt { get; set; }
        public string Other1 { get; set; }
        public string Other2 { get; set; }
        public string Other3 { get; set; }
        public string FeeCol { get; set; }
        public string FeeLicNum { get; set; }
        public string RenewalCol { get; set; }
        public decimal CURRFEE { get; set; }
        public string FeeCode { get; set; }
        public string RequiredNot { get; set; }
        public string Address1Col { get; set; }
        public string Address1LicNum { get; set; }
        public string MailingCol { get; set; }
        public string MailingAddressLine1 { get; set; }
        public string MailingAddressLine2 { get; set; }
        public string MailingAddressCity { get; set; }
        public string MailingAddressState { get; set; }
        public string MailingAddressZIP { get; set; }
        public string MailingAddressCountry { get; set; }
        public string PhoneNum { get; set; }
        public string EMailAddr { get; set; }
        public string FaxNum { get; set; }
        public string Address2Col { get; set; }
        public string Address2LicNum { get; set; }
        public string PracticeCol { get; set; }
        public string PracticeAddressLine1 { get; set; }
        public string PracticeAddressLine2 { get; set; }
        public string PracticeAddressCity { get; set; }
        public string PracticeAddressState { get; set; }
        public string PracticeAddressZIP { get; set; }
        public string PracticeAddressCountry { get; set; }
        public string PPhoneNum { get; set; }
        public string PEMailAddr { get; set; }
        public string PFaxNum { get; set; }
        public string arrested { get; set; }
        public string cited_charged { get; set; }
        public string crim_investigation { get; set; }
        public string convicted { get; set; }
        public string investigation { get; set; }
        public string depressed { get; set; }
        public string alcohol { get; set; }
        public string impairment { get; set; }
        public string sexual { get; set; }
        public string malpractice { get; set; }
        public string CME { get; set; }
        public string total_hrs { get; set; }
        public string primary_hrs { get; set; }
        public string primCounty { get; set; }
        public string primSetting { get; set; }
        public string primGrpNbr { get; set; }
        public string SecPracZip { get; set; }
        public string SecPracCounty { get; set; }
        public string SecPracSetting { get; set; }
        public string SecPracGrpNbr { get; set; }
        public string Race { get; set; }
        public string Hispanic_flg { get; set; }
        public string TxHScounty { get; set; }
        public bool DRP { get; set; }
        public bool MNP { get; set; }
        public bool TRP { get; set; }
        public bool MHP { get; set; }         

        public renewal()
        {
            status = "";
            methodOfPayment = "";
            this.id_num = 0;
            trace_number = "";
            LicNum = "";
            LicType = "";
            SSN = "";
            TRANSACTION_DATE = "";
            PAname = "";
            CURRFEE = 0m;
            MailingAddressLine1 = "";
            MailingAddressLine2 = "";
            MailingAddressCity = "";
            MailingAddressState = "";
            MailingAddressZIP = "";
            MailingAddressCountry = "";
            PhoneNum = "";
            EMailAddr = "";
            FaxNum = "";
            PracticeAddressLine1 = "";
            PracticeAddressLine2 = "";
            PracticeAddressCity = "";
            PracticeAddressState = "";
            PracticeAddressZIP = "";
            PracticeAddressCountry = "";
            PPhoneNum = "";
            PEMailAddr = "";
            PFaxNum = "";
            arrested = "";
            cited_charged = "";
            crim_investigation = "";
            convicted = "";
            investigation = ""; //5
            depressed = "";
            alcohol = "";
            impairment = "";
            sexual = "";
            malpractice = ""; //10
            CME = "";
            total_hrs = "";
            primary_hrs = "";
            primCounty = "";
            primSetting = ""; //15
            primGrpNbr = "";
            SecPracZip = "";
            SecPracCounty = "";
            SecPracSetting = "";
            SecPracGrpNbr = ""; //20
            Race = "";
            Hispanic_flg = "";
            TxHScounty = "";  //23 
            DRP = false ;
            MNP = false ;
            TRP = false;
            MHP = false;

    }

    
   
    

    //[Serializable]
    //public class renewalAC : renewal
    //{
    //    public string ac_regstat_cd { get; set; }
    //    public string ac_bad_ck_flag { get; set; }
    //    public string ac_tgsl_flag { get; set; }
    //    public bool ac_renew_lack_dt_criteria { get; set; }

    //    public renewalAC()
    //    {
    //        ac_regstat_cd = "";
    //        ac_bad_ck_flag = "";
    //        ac_tgsl_flag = "";
    //        ac_renew_lack_dt_criteria = false;
    //    }
    //    //
    //    public renewalAC(int id_num)
    //    {
    //        //creade D record all info is ultimately based on individual and annreg           
    //        try
    //        {
    //            this.id_num = id_num;
    //            ac_regstat_cd = "";
    //            ac_bad_ck_flag = "";
    //            ac_tgsl_flag = "";
    //            ac_renew_lack_dt_criteria = true; // tru is bad - no eligible to renew 
    //            //get individual
    //            Tracerdb.INDIVIDUAL ind = DataAccess.getIndividual(id_num);
    //            Tracerdb.ACUPUNCTURE ac = DataAccess.getAC(id_num);

    //            //elig
    //            ac_regstat_cd = ac.AC_REGSTAT_CD;
    //            ac_bad_ck_flag = ac.AC_BAD_CK_FLAG;
    //            ac_tgsl_flag = ac.AC_TGSL_FLAG;
    //            ac_renew_lack_dt_criteria = DataAccess.getLackInfo(id_num); //false is good
    //            //
    //            if ((ind != null) && (ac != null))
    //            {
    //                Race = ind.Race_cd;
    //                Hispanic_flg = ind.Hispanic_origin_flg;
    //                TxHScounty = ind.County_cd_high_school;
    //                LicenseExpirationDate = ac.AC_LIC_EXP_DT ?? DateTime.MinValue;
    //                LicNum = ac.AC_LIC_NUM.Trim();
    //                if (ind.SSN.Length > 4) SSN = ind.SSN.Substring(ind.SSN.Length - 4, 4);
    //                PAname = ind.FN + " " + ind.LN;

    //                CURRFEE = decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["ACFEE"]);
    //                FeeCode = "";

    //                //addresses                    
    //                MailingAddressLine1 = ind.MADD1;
    //                MailingAddressLine2 = ind.MADD2;
    //                MailingAddressCity = ind.MCITY;
    //                MailingAddressState = DataAccess.getState(ind);
    //                if ((ind.MZIP != null) && (ind.MZIP.Length > 4)) MailingAddressZIP = ind.MZIP.Substring(0, 5);
    //                MailingAddressCountry = DataAccess.getCountry(ind);
    //                PhoneNum = ind.PhoneNum;
    //                EMailAddr = ind.EMailAddr;
    //                FaxNum = ind.FaxNum;
    //                //
    //                if ((ind.PADD1 != null) && (ind.PZIP != null))
    //                {
    //                    PracticeAddressLine1 = ind.PADD1;
    //                    PracticeAddressLine2 = ind.PADD2;
    //                    PracticeAddressCity = ind.PCITY;
    //                    if (ind.PZIP != "") PracticeAddressState = ind.PST;
    //                    else PracticeAddressState = DataAccess.getPState(ind);
    //                    if (ind.PZIP.Length > 4) PracticeAddressZIP = ind.PZIP.Substring(0, 5);
    //                    if (ind.PZIP != "") PracticeAddressCountry = "USA";
    //                    else PracticeAddressCountry = DataAccess.getPState(ind);
    //                    PPhoneNum = ind.PPhoneNum ?? "";
    //                    PEMailAddr = ind.PEMailAddr ?? "";
    //                    PFaxNum = ind.PFaxNum ?? "";
    //                }
    //            }


    //        }
    //        catch (Exception e)
    //        {
    //            string s = e.Message;
    //        }
    //    }// constructor
    }
    

    
}

    

