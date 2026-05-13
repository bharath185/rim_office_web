using OfficeConnect_Web.Controllers;
using OfficeConnect_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace OfficeConnect_Web.Models
{
    public class PayrollModel
    {
        DB_Offc_ConEntities DB = new DB_Offc_ConEntities();
        ClsAuthentication ObjAuth = new ClsAuthentication();

        public List<PayrollSymbolMasterViewModel> DDPayrollSymbols(PayrolAccessViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var Symboldetails = (from pay in DB.PayrollSymbolMasters
                                      where pay.IsActive == true 
                                      select pay).OrderByDescending(x => x.SymbolId).ToList();

                if (loginId != 0)
                {
                    if (Symboldetails != null)
                    {
                        List<PayrollSymbolMasterViewModel> lstofpaytype = new List<PayrollSymbolMasterViewModel>();

                        for (int i = 0; i < Symboldetails.Count(); i++)
                        {
                            PayrollSymbolMasterViewModel ltvm = new PayrollSymbolMasterViewModel();
                            ltvm.SymbolId = Symboldetails[i].SymbolId;
                            ltvm.Symbol = Symboldetails[i].Symbol;
                            lstofpaytype.Add(ltvm);

                        }
                        return lstofpaytype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Symbol Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<PayrollFrequencyMasterViewModel> DDPayrollFrequency(PayrolAccessViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var Symboldetails = (from pay in DB.PayrollFrequencyMasters
                                     where pay.IsActive == true
                                     select pay).OrderByDescending(x => x.FrequencyId).ToList();

                if (loginId != 0)
                {
                    if (Symboldetails != null)
                    {
                        List<PayrollFrequencyMasterViewModel> lstofpaytype = new List<PayrollFrequencyMasterViewModel>();

                        for (int i = 0; i < Symboldetails.Count(); i++)
                        {
                            PayrollFrequencyMasterViewModel ltvm = new PayrollFrequencyMasterViewModel();
                            ltvm.FrequencyId = Symboldetails[i].FrequencyId;
                            ltvm.Frequency = Symboldetails[i].Frequency;
                            lstofpaytype.Add(ltvm);

                        }
                        return lstofpaytype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Frequency Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<DDPayrollPayoutTypeViewModel> DDPayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var Paydetails = (from pay in DB.PayrollPayoutTypes
                                  where pay.IsActive == true && pay.IsDeleted == false
                                  select pay).OrderByDescending(x => x.PayoutTypeId).ToList();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        List<DDPayrollPayoutTypeViewModel> lstofpaytype = new List<DDPayrollPayoutTypeViewModel>();

                        for (int i = 0; i < Paydetails.Count(); i++)
                        {
                            DDPayrollPayoutTypeViewModel ltvm = new DDPayrollPayoutTypeViewModel();
                            ltvm.PayoutTypeId = Paydetails[i].PayoutTypeId;
                            ltvm.PayoutTypeName = Paydetails[i].PayoutTypeName;
                            ltvm.Frequency = Paydetails[i].Frequency;
                            lstofpaytype.Add(ltvm);

                        }
                        return lstofpaytype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout Type Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<PayrollPayoutTypeViewModel> GetAllPayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var Paydetails = (from pay in DB.PayrollPayoutTypes
                                  where pay.IsDeleted == false
                                  select pay).OrderByDescending(x => x.PayoutTypeId).ToList();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        List<PayrollPayoutTypeViewModel> lstofpaytype = new List<PayrollPayoutTypeViewModel>();

                        for (int i = 0; i < Paydetails.Count(); i++)
                        {
                            PayrollPayoutTypeViewModel ltvm = new PayrollPayoutTypeViewModel();
                            ltvm.PayoutTypeId = Paydetails[i].PayoutTypeId;
                            ltvm.PayoutTypeName = Paydetails[i].PayoutTypeName;
                            int? payouttypeid = Paydetails[i].PayoutTypeId;
                            ltvm.PayoutTypeName = DB.PayrollPayoutTypes.Where(x => x.PayoutTypeId == payouttypeid && x.IsActive == true && x.IsDeleted == false).Select(x => x.PayoutTypeName).FirstOrDefault();
                            ltvm.Frequency = Paydetails[i].Frequency;
                            ltvm.CreatedBy = Paydetails[i].CreatedBy;
                            ltvm.CreatedDate = Paydetails[i].CreatedDate;
                            ltvm.LastUpdatedBy = Paydetails[i].LastUpdatedBy;
                            ltvm.LastUpdatedDate = Paydetails[i].LastUpdatedDate;
                            ltvm.IsActive = Paydetails[i].IsActive;
                            ltvm.IsUpdated = Paydetails[i].IsUpdated;
                            ltvm.IsDeleted = Paydetails[i].IsDeleted;
                            lstofpaytype.Add(ltvm);

                        }
                        return lstofpaytype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout Type Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollPayoutTypeViewModel GetPayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Paydetails = (from pay in DB.PayrollPayoutTypes
                                  where pay.PayoutTypeId == model.PayoutTypeId && pay.IsDeleted == false
                                  select pay).OrderByDescending(x => x.PayoutTypeId).FirstOrDefault();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        PayrollPayoutTypeViewModel ltvm = new PayrollPayoutTypeViewModel();
                        ltvm.PayoutTypeId = Paydetails.PayoutTypeId;
                        ltvm.PayoutTypeName = Paydetails.PayoutTypeName;
                        ltvm.Frequency = Paydetails.Frequency;
                        ltvm.CreatedBy = Paydetails.CreatedBy;
                        ltvm.CreatedDate = Paydetails.CreatedDate;
                        ltvm.LastUpdatedBy = Paydetails.LastUpdatedBy;
                        ltvm.LastUpdatedDate = Paydetails.LastUpdatedDate;
                        ltvm.IsActive = Paydetails.IsActive;
                        ltvm.IsUpdated = Paydetails.IsUpdated;
                        ltvm.IsDeleted = Paydetails.IsDeleted;
                        return ltvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout Type Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel AddPayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Paydetails = (from pay in DB.PayrollPayoutTypes
                                  where pay.PayoutTypeName == model.PayoutTypeName
                                  && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).ToList();

                if (loginId != 0)
                {
                    if (Paydetails.Count() == 0)
                    {
                        PayrollPayoutType ltm = new PayrollPayoutType();
                        //em.EmpId = model.modelId;
                        ltm.PayoutTypeName = model.PayoutTypeName;
                        ltm.Frequency = model.Frequency;
                        ltm.IsActive = true;
                        ltm.IsUpdated = false;
                        ltm.IsDeleted = false;
                        ltm.CreatedBy = model.LoginId;
                        ltm.CreatedDate = DateTime.Now;
                        ltm.LastUpdatedBy = model.LoginId;
                        ltm.LastUpdatedDate = DateTime.Now;
                        DB.PayrollPayoutTypes.Add(ltm);
                        DB.SaveChanges();

                        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Added";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout Type Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel UpdatePayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.PayoutTypeId != 0) ? model.PayoutTypeId : 0;

                var Paydetails = (from acc in DB.PayrollPayoutTypes
                                  where acc.PayoutTypeId == id && acc.IsActive == true && acc.IsDeleted == false
                                  select acc).FirstOrDefault();

                if (loginId != 0)
                {
                    if (id != 0)
                    {
                        if (Paydetails != null)
                        {
                            Paydetails.PayoutTypeName = model.PayoutTypeName;
                            Paydetails.Frequency = model.Frequency;
                            Paydetails.IsActive = true;
                            Paydetails.IsUpdated = true;
                            Paydetails.IsDeleted = false;
                            Paydetails.LastUpdatedBy = model.LoginId;
                            Paydetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();

                            PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                            emvm.Status = 200;
                            emvm.msg = "Updated";

                            return emvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Payout Type Details Not Found");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout Type Id is Mismatching");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel DeletePayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.PayoutTypeId != 0) ? model.PayoutTypeId : 0;

                var Paydetails = (from pay in DB.PayrollPayoutTypes
                                  where pay.PayoutTypeId == id && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).FirstOrDefault();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        Paydetails.IsActive = true;
                        Paydetails.IsUpdated = true;
                        Paydetails.IsDeleted = true;
                        Paydetails.LastUpdatedBy = model.LoginId;
                        Paydetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Deleted";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout Type Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel ActivatePayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.PayoutTypeId != 0) ? model.PayoutTypeId : 0;

                var Paydetails = (from pay in DB.PayrollPayoutTypes
                                  where pay.PayoutTypeId == id && pay.IsActive == false && pay.IsDeleted == false
                                  select pay).FirstOrDefault();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        Paydetails.IsActive = true;
                        Paydetails.IsUpdated = true;
                        Paydetails.IsDeleted = false;
                        Paydetails.LastUpdatedBy = model.LoginId;
                        Paydetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Activated";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout Type Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel DeactivatePayrollPayoutType(PayrollPayoutTypeViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.PayoutTypeId != 0) ? model.PayoutTypeId : 0;

                var Paydetails = (from pay in DB.PayrollPayoutTypes
                                  where pay.PayoutTypeId == id && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).FirstOrDefault();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        Paydetails.IsActive = false;
                        Paydetails.IsUpdated = true;
                        Paydetails.IsDeleted = false;
                        Paydetails.LastUpdatedBy = model.LoginId;
                        Paydetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Deactivated";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout Type Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }

        }
        public List<DDPayrollSegmentViewModel> DDPayrollSegment(PayrollSegmentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? payouttypeid = (model.PayoutTypeId != 0) ? model.PayoutTypeId : 0;

                var Paydetails = (from pay in DB.PayrollSegments
                                  where pay.PayoutTypeId == model.PayoutTypeId && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).OrderByDescending(x => x.SegmentId).ToList();

                if (payouttypeid == 0)
                {
                    Paydetails = (from pay in DB.PayrollSegments
                                      where pay.IsActive == true && pay.IsDeleted == false
                                      select pay).OrderByDescending(x => x.SegmentId).ToList();
                }

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        List<DDPayrollSegmentViewModel> lstofpaytype = new List<DDPayrollSegmentViewModel>();

                        for (int i = 0; i < Paydetails.Count(); i++)
                        {
                            DDPayrollSegmentViewModel ltvm = new DDPayrollSegmentViewModel();
                            ltvm.SegmentId = Paydetails[i].SegmentId;
                            ltvm.SegmentName = Paydetails[i].SegmentName;
                            lstofpaytype.Add(ltvm);

                        }
                        return lstofpaytype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Segment Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<PayrollSegmentViewModel> GetAllPayrollPayoutTypeSegment(PayrollSegmentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? PayouttypeId = (model.PayoutTypeId != 0) ? model.PayoutTypeId : 0;

                var Paydetails = (from pay in DB.PayrollSegments
                                  where pay.PayoutTypeId == PayouttypeId && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).OrderByDescending(x => x.SegmentId).ToList();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        List<PayrollSegmentViewModel> lstofpaytype = new List<PayrollSegmentViewModel>();

                        for (int i = 0; i < Paydetails.Count(); i++)
                        {
                            PayrollSegmentViewModel ltvm = new PayrollSegmentViewModel();
                            ltvm.PayoutTypeId = Paydetails[i].PayoutTypeId;
                            int? payouttypeid = Paydetails[i].PayoutTypeId;
                            ltvm.PayoutTypeName = DB.PayrollPayoutTypes.Where(x => x.PayoutTypeId == payouttypeid && x.IsActive == true && x.IsDeleted == false).Select(x => x.PayoutTypeName).FirstOrDefault();
                            ltvm.SegmentId = Paydetails[i].SegmentId;
                            ltvm.SegmentName = Paydetails[i].SegmentName;
                            ltvm.CreatedBy = Paydetails[i].CreatedBy;
                            ltvm.CreatedDate = Paydetails[i].CreatedDate;
                            ltvm.LastUpdatedBy = Paydetails[i].LastUpdatedBy;
                            ltvm.LastUpdatedDate = Paydetails[i].LastUpdatedDate;
                            ltvm.IsActive = Paydetails[i].IsActive;
                            ltvm.IsUpdated = Paydetails[i].IsUpdated;
                            ltvm.IsDeleted = Paydetails[i].IsDeleted;
                            lstofpaytype.Add(ltvm);

                        }
                        return lstofpaytype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Segment Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<PayrollSegmentViewModel> GetAllPayrollSegment(PayrollSegmentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var Paydetails = (from pay in DB.PayrollSegments
                                  where pay.IsActive == true && pay.IsDeleted == false
                                  select pay).OrderByDescending(x => x.SegmentId).ToList();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        List<PayrollSegmentViewModel> lstofpaytype = new List<PayrollSegmentViewModel>();

                        for (int i = 0; i < Paydetails.Count(); i++)
                        {
                            PayrollSegmentViewModel ltvm = new PayrollSegmentViewModel();
                            ltvm.PayoutTypeId = Paydetails[i].PayoutTypeId;
                            int? payouttypeid = Paydetails[i].PayoutTypeId;
                            ltvm.PayoutTypeName = DB.PayrollPayoutTypes.Where(x => x.PayoutTypeId == payouttypeid && x.IsActive == true && x.IsDeleted == false).Select(x => x.PayoutTypeName).FirstOrDefault();
                            ltvm.SegmentId = Paydetails[i].SegmentId;
                            ltvm.SegmentName = Paydetails[i].SegmentName;
                            ltvm.CreatedBy = Paydetails[i].CreatedBy;
                            ltvm.CreatedDate = Paydetails[i].CreatedDate;
                            ltvm.LastUpdatedBy = Paydetails[i].LastUpdatedBy;
                            ltvm.LastUpdatedDate = Paydetails[i].LastUpdatedDate;
                            ltvm.IsActive = Paydetails[i].IsActive;
                            ltvm.IsUpdated = Paydetails[i].IsUpdated;
                            ltvm.IsDeleted = Paydetails[i].IsDeleted;
                            lstofpaytype.Add(ltvm);

                        }
                        return lstofpaytype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Segment Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollSegmentViewModel GetPayrollSegment(PayrollSegmentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Paydetails = (from pay in DB.PayrollSegments
                                  where pay.SegmentId == model.SegmentId && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).OrderByDescending(x => x.SegmentId).FirstOrDefault();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        PayrollSegmentViewModel ltvm = new PayrollSegmentViewModel();
                        ltvm.PayoutTypeId = Paydetails.PayoutTypeId;
                        int? payouttypeid = Paydetails.PayoutTypeId;
                        ltvm.PayoutTypeName = DB.PayrollPayoutTypes.Where(x => x.PayoutTypeId == payouttypeid && x.IsActive == true && x.IsDeleted == false).Select(x => x.PayoutTypeName).FirstOrDefault();
                        ltvm.SegmentId = Paydetails.SegmentId;
                        ltvm.SegmentName = Paydetails.SegmentName;
                        ltvm.CreatedBy = Paydetails.CreatedBy;
                        ltvm.CreatedDate = Paydetails.CreatedDate;
                        ltvm.LastUpdatedBy = Paydetails.LastUpdatedBy;
                        ltvm.LastUpdatedDate = Paydetails.LastUpdatedDate;
                        ltvm.IsActive = Paydetails.IsActive;
                        ltvm.IsUpdated = Paydetails.IsUpdated;
                        ltvm.IsDeleted = Paydetails.IsDeleted;
                        return ltvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Segment Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel AddPayrollSegment(PayrollSegmentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Paydetails = (from pay in DB.PayrollSegments
                                  where pay.PayoutTypeId == model.PayoutTypeId && pay.SegmentName == model.SegmentName
                                  && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).ToList();

                if (loginId != 0)
                {
                    if (Paydetails.Count() == 0)
                    {
                        PayrollSegment ltm = new PayrollSegment();
                        //em.EmpId = model.modelId;
                        ltm.PayoutTypeId = model.PayoutTypeId;
                        ltm.SegmentName = model.SegmentName;
                        ltm.IsActive = true;
                        ltm.IsUpdated = false;
                        ltm.IsDeleted = false;
                        ltm.CreatedBy = model.LoginId;
                        ltm.CreatedDate = DateTime.Now;
                        ltm.LastUpdatedBy = model.LoginId;
                        ltm.LastUpdatedDate = DateTime.Now;
                        DB.PayrollSegments.Add(ltm);
                        DB.SaveChanges();

                        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Added";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Segment Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel UpdatePayrollSegment(PayrollSegmentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? ptid = (model.PayoutTypeId != 0) ? model.PayoutTypeId : 0;
                int? segid = (model.SegmentId != 0) ? model.SegmentId : 0;

                var Paydetails = (from acc in DB.PayrollSegments
                                  where acc.PayoutTypeId == ptid && acc.SegmentId == segid && acc.IsActive == true && acc.IsDeleted == false
                                  select acc).FirstOrDefault();

                if (loginId != 0)
                {
                    if (ptid != 0)
                    {
                        if (Paydetails != null)
                        {
                            Paydetails.SegmentName = model.SegmentName;
                            Paydetails.IsActive = true;
                            Paydetails.IsUpdated = true;
                            Paydetails.IsDeleted = false;
                            Paydetails.LastUpdatedBy = model.LoginId;
                            Paydetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();

                            PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                            emvm.Status = 200;
                            emvm.msg = "Updated";

                            return emvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Segment Details Not Found");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Segment Id is Mismatching");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel DeletePayrollSegment(PayrollSegmentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? ptid = (model.PayoutTypeId != 0) ? model.PayoutTypeId : 0;
                int? segid = (model.SegmentId != 0) ? model.SegmentId : 0;

                var Paydetails = (from acc in DB.PayrollSegments
                                  where acc.PayoutTypeId == ptid && acc.SegmentId == segid && acc.IsActive == true && acc.IsDeleted == false
                                  select acc).FirstOrDefault();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        Paydetails.IsActive = true;
                        Paydetails.IsUpdated = true;
                        Paydetails.IsDeleted = true;
                        Paydetails.LastUpdatedBy = model.LoginId;
                        Paydetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Deleted";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Segment Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<DDPayrollEmpListViewModel> DDPayrollEmpList(PayrollALLComponentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var empdetails = (from emp in DB.EmployeeMasters
                                  join ctc in DB.EmployeeSalaryDetails on emp.EmpCode.ToUpper() equals ctc.EmpCode.ToUpper()
                                  where emp.IsActive == true && emp.IsDeleted == false && emp.EmpStatus.ToUpper() == "ACTIVE" &&
                                  ctc.RecordStatus == true && ctc.IsActive == true && ctc.IsDeleted == false
                                  select emp).OrderByDescending(x => x.EmpId).ToList();

                if (loginId != 0)
                {
                    if (empdetails != null)
                    {
                        List<DDPayrollEmpListViewModel> lstofemplist = new List<DDPayrollEmpListViewModel>();

                        for (int i = 0; i < empdetails.Count(); i++)
                        {
                            DDPayrollEmpListViewModel ltvm = new DDPayrollEmpListViewModel();
                            ltvm.EmpId = empdetails[i].EmpId;
                            ltvm.EmpName = empdetails[i].FirstName + empdetails[i].MiddleName + empdetails[i].LastName + " (" + empdetails[i].EmpCode + ")";
                            ltvm.EmpCode = empdetails[i].EmpCode;
                            lstofemplist.Add(ltvm);

                        }
                        return lstofemplist;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Component Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<DDPayrollComponentViewModel> DDPayrollComponent(PayrollALLComponentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? payoutid = (model.PayoutTypeId != null) ? model.PayoutTypeId : 0;

                var comdetails = (from pay in DB.PayrollComponents
                                  where pay.PayoutTypeId == payoutid && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).OrderByDescending(x => x.SegmentId).ToList();

                if (payoutid == 0)
                {
                    comdetails = (from pay in DB.PayrollComponents
                                      where pay.IsActive == true && pay.IsDeleted == false
                                      select pay).OrderByDescending(x => x.SegmentId).ToList();
                }

                if (loginId != 0)
                {
                    if (comdetails != null)
                    {
                        List<DDPayrollComponentViewModel> lstofpaytype = new List<DDPayrollComponentViewModel>();

                        for (int i = 0; i < comdetails.Count(); i++)
                        {
                            DDPayrollComponentViewModel ltvm = new DDPayrollComponentViewModel();
                            ltvm.ComponentId = comdetails[i].ComponentId;
                            ltvm.ComponentName = comdetails[i].ComponentCode;
                            lstofpaytype.Add(ltvm);

                        }
                        return lstofpaytype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Component Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        ////public PayrollResponseViewModel AddComponent(PayrollALLComponentViewModel model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        ////        int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

        ////        var Componentdetails = (from pay in DB.PayrollComponents
        ////                                where pay.PayoutTypeId == model.PayoutTypeId && pay.SegmentId == model.SegmentId
        ////                                && pay.ComponentName == model.ComponentName
        ////                                && pay.IsActive == true && pay.IsDeleted == false
        ////                                select pay).ToList();

        ////        var ComponentCodedetails = (from pay in DB.PayrollComponents
        ////                                    where pay.PayoutTypeId == model.PayoutTypeId && pay.SegmentId == model.SegmentId
        ////                                    && pay.ComponentCode == model.ComponentCode
        ////                                    && pay.IsActive == true && pay.IsDeleted == false
        ////                                    select pay).ToList();

        ////        if (model.ComponentId1 != null)
        ////        {
        ////            int comppayouttypeId = (int)DB.PayrollComponents.Where(x => x.ComponentId == model.ComponentId1 && x.IsActive == true && x.IsDeleted == false).Select(x => x.PayoutTypeId).FirstOrDefault();

        ////            if (comppayouttypeId != model.PayoutTypeId)
        ////            {
        ////                throw new CustomApiException(HttpStatusCode.NotFound, "The selected payout does not match the payout components referenced in the logic. Kindly choose the correct payout components in the payout logic to proceed.");
        ////            }
        ////        }

        ////        if (Componentdetails.Count() > 0)
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "Component Name is Already Exists");
        ////        }
        ////        if (ComponentCodedetails.Count() > 0)
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "Component Code is Already Exists");
        ////        }

        ////        //var LogCaldetails = (from pay in DB.PayrollComponentLogics
        ////        //                     where pay.PayoutTypeId == model.PayoutTypeId && pay.SegmentName == model.SegmentName
        ////        //                     && pay.IsActive == true && pay.IsDeleted == false
        ////        //                     select pay).ToList();

        ////        //var Conditiondetails = (from pay in DB.PayrollComponentConditions
        ////        //                        where pay.PayoutTypeId == model.PayoutTypeId && pay.SegmentName == model.SegmentName
        ////        //                        && pay.IsActive == true && pay.IsDeleted == false
        ////        //                        select pay).ToList();

        ////        if (loginId != 0)
        ////        {
        ////            if (Componentdetails.Count() == 0)
        ////            {
        ////                PayrollComponent prc = new PayrollComponent();
        ////                //em.EmpId = model.modelId;
        ////                prc.PayoutTypeId = model.PayoutTypeId;
        ////                prc.SegmentId = model.SegmentId;
        ////                prc.ComponentName = model.ComponentName;
        ////                prc.ComponentCode = model.ComponentCode;
        ////                prc.IsActive = true;
        ////                prc.IsUpdated = false;
        ////                prc.IsDeleted = false;
        ////                prc.CreatedBy = model.LoginId;
        ////                prc.CreatedDate = DateTime.Now;
        ////                prc.LastUpdatedBy = model.LoginId;
        ////                prc.LastUpdatedDate = DateTime.Now;
        ////                DB.PayrollComponents.Add(prc);
        ////                DB.SaveChanges();
        ////                int? componentid = prc.ComponentId;

        ////                PayrollComponentLogic prcl = new PayrollComponentLogic();
        ////                //em.EmpId = model.modelId;
        ////                prcl.ComponentId = componentid;
        ////                prcl.Value = (model.Value != 0) ? model.Value : 0;
        ////                prcl.Percentage = (model.Percentage != 0) ? model.Percentage : 0;
        ////                prcl.ComponentId1 = (model.ComponentId1 != 0) ? model.ComponentId1 : 0;
        ////                prcl.ComponentName1 = (model.ComponentName1 != "") ? model.ComponentName1 : "";
        ////                prcl.EffectiveFrom = (model.EffectiveFrom != null) ? model.EffectiveFrom : null;
        ////                prcl.EffectiveTo = (model.EffectiveTo != null) ? model.EffectiveTo : null;
        ////                prcl.IsActive = true;
        ////                prcl.IsUpdated = false;
        ////                prcl.IsDeleted = false;
        ////                prcl.CreatedBy = model.LoginId;
        ////                prcl.CreatedDate = DateTime.Now;
        ////                prcl.LastUpdatedBy = model.LoginId;
        ////                prcl.LastUpdatedDate = DateTime.Now;
        ////                DB.PayrollComponentLogics.Add(prcl);
        ////                DB.SaveChanges();

        ////                PayrollComponentCondition prcc = new PayrollComponentCondition();
        ////                //em.EmpId = model.modelId;
        ////                prcc.ComponentId = componentid;
        ////                prcc.ConditionExpression = (model.ConditionExpression != "") ? model.ConditionExpression : ""; 
        ////                prcc.ConditionResultPFESI = (model.ConditionResultPFESI != null) ? model.ConditionResultPFESI : null; 
        ////                prcc.IsActive = true;
        ////                prcc.IsUpdated = false;
        ////                prcc.IsDeleted = false;
        ////                prcc.CreatedBy = model.LoginId;
        ////                prcc.CreatedDate = DateTime.Now;
        ////                prcc.LastUpdatedBy = model.LoginId;
        ////                prcc.LastUpdatedDate = DateTime.Now;
        ////                DB.PayrollComponentConditions.Add(prcc);
        ////                DB.SaveChanges();


        ////                PayrollResponseViewModel emvm = new PayrollResponseViewModel();
        ////                emvm.Status = 200;
        ////                emvm.msg = "Added";

        ////                return emvm;
        ////            }
        ////            else
        ////            {
        ////                throw new CustomApiException(HttpStatusCode.NotFound, "Component Details Already Exists");
        ////            }
        ////        }
        ////        else
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}
        public PayrollResponseViewModel AddComponent(PayrollALLComponentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Componentdetails = (from pay in DB.PayrollComponents
                                        where pay.PayoutTypeId == model.PayoutTypeId && pay.SegmentId == model.SegmentId
                                        && pay.ComponentName == model.ComponentName
                                        && pay.IsActive == true && pay.IsDeleted == false
                                        select pay).ToList();

                var ComponentCodedetails = (from pay in DB.PayrollComponents
                                            where pay.PayoutTypeId == model.PayoutTypeId && pay.SegmentId == model.SegmentId
                                            && pay.ComponentCode == model.ComponentCode
                                            && pay.IsActive == true && pay.IsDeleted == false
                                            select pay).ToList();

                ////for (int i = 0; i < model.lstofLC.Count(); i++)
                ////{
                ////    if (model.lstofLC[i].ComponentId1 != null)
                ////    {
                ////        int comppayouttypeId = (int)DB.PayrollComponents.Where(x => x.ComponentId == model.lstofLC[i].ComponentId1 && x.IsActive == true && x.IsDeleted == false).Select(x => x.PayoutTypeId).FirstOrDefault();

                ////        if (comppayouttypeId != model.PayoutTypeId)
                ////        {
                ////            throw new CustomApiException(HttpStatusCode.NotFound, "The selected payout does not match the payout components referenced in the logic. Kindly choose the correct payout components in the payout logic to proceed.");
                ////        }
                ////    }
                ////}
                
                for (int i = 0; i < model.lstofLC.Count(); i++)
                {
                    if (model.lstofLC[i].ComponentId1.HasValue && model.lstofLC[i].ComponentId1.Value != 0)
                    {
                        int componentId1Value = model.lstofLC[i].ComponentId1.Value;

                        var component = DB.PayrollComponents
                            .Where(x => x.ComponentId == componentId1Value
                                && x.IsActive == true
                                && x.IsDeleted == false)
                            .Select(x => x.PayoutTypeId)
                            .FirstOrDefault();

                        if (component == 0)
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound,
                                $"Component ID {model.lstofLC[i].ComponentId1.Value} not found.");
                        }

                        if (component != model.PayoutTypeId)
                        {
                            throw new CustomApiException(HttpStatusCode.BadRequest,
                                "Payout type mismatch in component logic.");
                        }
                    }
                }

                if (Componentdetails.Count() > 0)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Component Name is Already Exists");
                }
                if (ComponentCodedetails.Count() > 0)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Component Code is Already Exists");
                }

                if (loginId != 0)
                {
                    if (Componentdetails.Count() == 0)
                    {
                        PayrollComponent prc = new PayrollComponent();
                        //em.EmpId = model.modelId;
                        prc.PayoutTypeId = model.PayoutTypeId;
                        prc.SegmentId = model.SegmentId;
                        prc.ComponentName = model.ComponentName;
                        prc.ComponentCode = model.ComponentCode;
                        prc.IsActive = true;
                        prc.IsUpdated = false;
                        prc.IsDeleted = false;
                        prc.CreatedBy = model.LoginId;
                        prc.CreatedDate = DateTime.Now;
                        prc.LastUpdatedBy = model.LoginId;
                        prc.LastUpdatedDate = DateTime.Now;
                        DB.PayrollComponents.Add(prc);
                        DB.SaveChanges();
                        int? componentid = prc.ComponentId;

                        for (int i = 0; i < model.lstofLC.Count(); i++)
                        {
                            int sno = i + 1;
                            PayrollComponentLogic prcl = new PayrollComponentLogic();
                            //em.EmpId = model.modelId;
                            prcl.ComponentId = componentid;
                            prcl.SNo = sno;
                            prcl.Value = (model.lstofLC[i].Value != 0) ? model.lstofLC[i].Value : null;
                            prcl.Percentage = (model.lstofLC[i].Percentage != 0) ? model.lstofLC[i].Percentage : null ;
                            prcl.ComponentId1 = (model.lstofLC[i].ComponentId1 != 0) ? model.lstofLC[i].ComponentId1 : null;
                            prcl.ComponentName1 = (model.lstofLC[i].ComponentName1 != "") ? model.lstofLC[i].ComponentName1 : null;
                            prcl.EffectiveFrom = (model.lstofLC[i].EffectiveFrom != null) ? model.lstofLC[i].EffectiveFrom : null;
                            prcl.EffectiveTo = (model.lstofLC[i].EffectiveTo != null) ? model.lstofLC[i].EffectiveTo : null;
                            prcl.IsActive = true;
                            prcl.IsUpdated = false;
                            prcl.IsDeleted = false;
                            prcl.CreatedBy = model.LoginId;
                            prcl.CreatedDate = DateTime.Now;
                            prcl.LastUpdatedBy = model.LoginId;
                            prcl.LastUpdatedDate = DateTime.Now;
                            DB.PayrollComponentLogics.Add(prcl);
                            DB.SaveChanges();

                            PayrollComponentCondition prcc = new PayrollComponentCondition();
                            //em.EmpId = model.modelId;
                            prcc.ComponentId = componentid;
                            prcc.SNo = sno;
                            prcc.ConditionExpression = (model.lstofLC[i].ConditionExpression != "") ? model.lstofLC[i].ConditionExpression : null;
                            prcc.ConditionResultPFESI = (model.lstofLC[i].ConditionResultPFESI != null) ? model.lstofLC[i].ConditionResultPFESI : null;
                            prcc.IsActive = true;
                            prcc.IsUpdated = false;
                            prcc.IsDeleted = false;
                            prcc.CreatedBy = model.LoginId;
                            prcc.CreatedDate = DateTime.Now;
                            prcc.LastUpdatedBy = model.LoginId;
                            prcc.LastUpdatedDate = DateTime.Now;
                            DB.PayrollComponentConditions.Add(prcc);
                            DB.SaveChanges();
                        }

                        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Added";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Component Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        ////public List<PayrollResponseModel> GetAllComponentDetails(PayrollALLComponentViewModel model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

        ////        if (loginId != 0)
        ////        {
        ////            var result = DB.PayrollPayoutTypes
        ////            .Where(p => p.IsActive == true && p.IsDeleted == false)
        ////            .Select(payout => new PayrollResponseModel
        ////            {
        ////                PayoutId = payout.PayoutTypeId,
        ////                PayoutName = payout.PayoutTypeName,

        ////                Segments = DB.PayrollSegments
        ////                    .Where(s => s.PayoutTypeId == payout.PayoutTypeId
        ////                                && s.IsActive == true && s.IsDeleted == false)
        ////                    .Select(segment => new SegmentResponseModel
        ////                    {
        ////                        SegmentId = segment.SegmentId,
        ////                        SegmentName = segment.SegmentName,

        ////                        Components = (from comp in DB.PayrollComponents
        ////                                      join logic in DB.PayrollComponentLogics
        ////                                         on comp.ComponentId equals logic.ComponentId into logicGroup
        ////                                      from logic in logicGroup.DefaultIfEmpty()

        ////                                      join cond in DB.PayrollComponentConditions
        ////                                         on comp.ComponentId equals cond.ComponentId into condGroup
        ////                                      from cond in condGroup.DefaultIfEmpty()

        ////                                      where comp.PayoutTypeId == payout.PayoutTypeId
        ////                                            && comp.SegmentId == segment.SegmentId
        ////                                            && comp.IsActive == true
        ////                                            && comp.IsDeleted == false
        ////                                            && (logic == null || (logic.IsActive == true && logic.IsDeleted == false))
        ////                                            && (cond == null || (cond.IsActive == true && cond.IsDeleted == false))

        ////                                      select new ComponentResponseModel
        ////                                      {
        ////                                          // Component master
        ////                                          ComponentId = comp.ComponentId,
        ////                                          ComponentName = comp.ComponentName,
        ////                                          ComponentValue = "",



        ////                                          // Logic table
        ////                                          LogicId = logic.LogicId,
        ////                                          Percentage = logic.Percentage,
        ////                                          Value = logic.Value,
        ////                                          ComponentId1 = logic.ComponentId1,
        ////                                          ComponentName1 = logic.ComponentName1,

        ////                                          // Condition table
        ////                                          ConditionId = cond.ConditionId,
        ////                                          ConditionExpression = cond.ConditionExpression,
        ////                                          ConditionResultPFESI = cond.ConditionResultPFESI,
        ////                                      })
        ////                            .OrderBy(c => c.ComponentId)
        ////                            .ToList()
        ////                    })
        ////                    .OrderBy(s => s.SegmentId)
        ////                    .ToList()

        ////            })
        ////            .OrderBy(p => p.PayoutId)
        ////            .ToList();

        ////            return result;
        ////        }
        ////        else
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}
        public List<PayrollResponseModel> GetAllComponentDetails(PayrollALLComponentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                if (loginId != 0)
                {
                    var result = DB.PayrollPayoutTypes
                        .Where(p => p.IsActive == true && p.IsDeleted == false)
                        .Select(payout => new PayrollResponseModel
                        {
                            PayoutId = payout.PayoutTypeId,
                            PayoutName = payout.PayoutTypeName,

                            Segments = DB.PayrollSegments
                                .Where(s =>
                                    s.PayoutTypeId == payout.PayoutTypeId &&
                                    s.IsActive == true && s.IsDeleted == false)
                                .Select(segment => new SegmentResponseModel
                                {
                                    SegmentId = segment.SegmentId,
                                    SegmentName = segment.SegmentName,

                                    Components = DB.PayrollComponents
                                        .Where(comp =>
                                            comp.PayoutTypeId == payout.PayoutTypeId &&
                                            comp.SegmentId == segment.SegmentId &&
                                            comp.IsActive == true && comp.IsDeleted == false)
                                        .Select(comp => new ComponentResponseModel
                                        {
                                            ComponentId = comp.ComponentId,
                                            ComponentName = comp.ComponentName,
                                            ComponentCode = comp.ComponentCode,
                                            ComponentValue = "",

                                            LogicConditions = (
                                                from logic in DB.PayrollComponentLogics
                                                    .Where(l =>
                                                        l.ComponentId == comp.ComponentId &&
                                                        l.IsActive == true && l.IsDeleted == false)

                                                join cond in DB.PayrollComponentConditions
                                                    .Where(c =>
                                                        c.IsActive == true && c.IsDeleted == false)
                                                    on new { logic.ComponentId, logic.SNo }
                                                    equals new { cond.ComponentId, cond.SNo }
                                                    into condGroup

                                                from cond in condGroup.DefaultIfEmpty()

                                                select new LogicConditionResponseModel
                                                {
                                                    ComponentId = comp.ComponentId,

                                                    // Logic fields
                                                    LogicId = logic.LogicId,
                                                    Percentage = logic.Percentage,
                                                    Value = logic.Value,
                                                    ComponentId1 = logic.ComponentId1,
                                                    ComponentName1 = logic.ComponentName1,

                                                    // Condition fields
                                                    ConditionId = cond != null ? cond.ConditionId : 0,
                                                    ConditionExpression = cond != null ? cond.ConditionExpression : null,
                                                    ConditionResultPFESI = cond != null ? cond.ConditionResultPFESI : null
                                                }
                                            ).ToList()
                                        })
                                        .OrderBy(c => c.ComponentId)
                                        .ToList()
                                })
                                .OrderBy(s => s.SegmentId)
                                .ToList()
                        })
                        .OrderBy(p => p.PayoutId)
                        .ToList();

                    return result;
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        //////public List<PayrollALLComponentCompactViewModel> EmpCTCCalculation(PayrollALLComponentViewModel model)
        //////{
        //////    try
        //////    {
        //////        string msg = "";
        //////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        //////        int? empId = (model.EmpId != 0) ? model.EmpId : 0;

        //////        double? empCTC = 0.00;

        //////        if (loginId == null)
        //////        {
        //////            if (loginId == 0)
        //////            {
        //////                throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
        //////            }
        //////            else
        //////            {
        //////                throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
        //////            }
        //////        }

        //////        DateTime Today = DateTime.Now;

        //////        var empSaldetails = (from emp in DB.EmployeeMasters
        //////                              join sal in DB.EmployeeSalaryDetails
        //////                                on emp.EmpCode.ToUpper() equals sal.EmpCode.ToUpper()
        //////                              where emp.EmpId == empId && sal.EmpId == empId && emp.EmpStatus.ToUpper() == "ACTIVE"
        //////                                 && sal.EffectiveFromDate <= Today && sal.EffectiveToDate >= Today
        //////                                 && emp.IsActive == true && emp.IsDeleted == false
        //////                                 && sal.IsActive == true && sal.IsDeleted == false
        //////                              orderby sal.SalaryId descending
        //////                              select sal).FirstOrDefault();

        //////        if (empSaldetails == null)
        //////        {
        //////            var empSaldetails1 = (from emp in DB.EmployeeMasters
        //////                                   join sal in DB.EmployeeSalaryDetails
        //////                                     on emp.EmpCode.ToUpper() equals sal.EmpCode.ToUpper()
        //////                                   where emp.EmpId == empId && sal.EmpId == empId && emp.EmpStatus.ToUpper() == "ACTIVE"
        //////                                      && emp.IsActive == true && emp.IsDeleted == false
        //////                                      && sal.IsActive == true && sal.IsDeleted == false
        //////                                   orderby sal.SalaryId descending
        //////                                   select sal).FirstOrDefault();

        //////            if (empSaldetails1 != null)
        //////            {
        //////                throw new CustomApiException(HttpStatusCode.NotFound, "The effective dates for the employee’s salary details have expired.");
        //////            }
        //////            else
        //////            {
        //////                throw new CustomApiException(HttpStatusCode.NotFound, "Salary details (CTC) for the selected employee were not found.");
        //////            }
        //////        }

        //////        empCTC = (double?)empSaldetails.CTC; // emp CTC

        //////        var empdetails = (from emp in DB.EmployeeMasters 
        //////                          where emp.EmpId == empId && emp.EmpStatus.ToUpper() == "ACTIVE"
        //////                             && emp.IsActive == true && emp.IsDeleted == false
        //////                          orderby emp.EmpId descending
        //////                          select emp).FirstOrDefault();

        //////        //emp details
        //////        string EmpCode = empdetails.EmpCode;
        //////        string FirstName = empdetails.FirstName;
        //////        string MiddleName = empdetails.MiddleName;
        //////        string LastName = empdetails.LastName;

        //////        var Componentdetails = (from com in DB.PayrollComponents
        //////                                join cal in DB.PayrollComponentLogics on com.ComponentId equals cal.ComponentId
        //////                                join con in DB.PayrollComponentConditions on com.ComponentId equals con.ComponentId
        //////                                join pay in DB.PayrollPayoutTypes on com.PayoutTypeId equals pay.PayoutTypeId
        //////                                join seg in DB.PayrollSegments on com.SegmentId equals seg.SegmentId
        //////                                where com.IsActive == true && com.IsDeleted == false
        //////                                && cal.IsActive == true && cal.IsDeleted == false
        //////                                && con.IsActive == true && con.IsDeleted == false
        //////                                && pay.IsActive == true && pay.IsDeleted == false
        //////                                && seg.IsActive == true && seg.IsDeleted == false
        //////                                orderby pay.PayoutTypeId ascending, seg.SegmentId ascending
        //////                                select new PayrollALLComponentCompactViewModel
        //////                                {
        //////                                    //Payout
        //////                                    PayoutTypeId = pay.PayoutTypeId,
        //////                                    PayoutTypeName = pay.PayoutTypeName,
        //////                                    FrequencyId = 0,
        //////                                    Frequency = pay.Frequency,
        //////                                    //Segment
        //////                                    SegmentId = seg.SegmentId,
        //////                                    SegmentName = seg.SegmentName,
        //////                                    //Component
        //////                                    ComponentId = com.ComponentId,
        //////                                    ComponentName = com.ComponentName,
        //////                                    ComponentCode = com.ComponentCode,
        //////                                    ComponentValue = "0.00",
        //////                                    //Logic & Calculation
        //////                                    LogicId = cal.LogicId,
        //////                                    Percentage = cal.Percentage,
        //////                                    Value = cal.Value,
        //////                                    ComponentId1 = cal.ComponentId1,
        //////                                    ComponentName1 = cal.ComponentName1,
        //////                                    EffectiveFrom = cal.EffectiveFrom,
        //////                                    EffectiveTo = cal.EffectiveTo,
        //////                                    //Condition
        //////                                    ConditionId = con.ConditionId,
        //////                                    ConditionExpression = con.ConditionExpression,
        //////                                    ConditionResultPFESI = con.ConditionResultPFESI,
        //////                                }).ToList();

        //////        if (Componentdetails.Count() == 0)
        //////        {
        //////            throw new CustomApiException(HttpStatusCode.NotFound, "Component details are not found");
        //////        }
        //////        else
        //////        {
        //////            if (Componentdetails != null)
        //////            {
        //////                List<PayrollALLComponentCompactViewModel> lstofCompvalue = new List<PayrollALLComponentCompactViewModel>();

        //////                for (int i = 0; i < Componentdetails.Count(); i++)
        //////                {
        //////                    PayrollALLComponentCompactViewModel pacvm = new PayrollALLComponentCompactViewModel();
        //////                    //Emp Details
        //////                    pacvm.EmpId = (int)empId;
        //////                    pacvm.EmpCode = EmpCode;
        //////                    pacvm.FirstName = FirstName;
        //////                    pacvm.MiddleName = MiddleName;
        //////                    pacvm.LastName = LastName;
        //////                    pacvm.LoginId = (int)loginId;
        //////                    //Payout
        //////                    pacvm.PayoutTypeId = Componentdetails[i].PayoutTypeId;
        //////                    pacvm.PayoutTypeName = Componentdetails[i].PayoutTypeName;
        //////                    pacvm.FrequencyId = 0;
        //////                    pacvm.Frequency = Componentdetails[i].Frequency;
        //////                    //Segment
        //////                    pacvm.SegmentId = Componentdetails[i].SegmentId;
        //////                    pacvm.SegmentName = Componentdetails[i].SegmentName;
        //////                    //Component
        //////                    pacvm.ComponentId = Componentdetails[i].ComponentId;
        //////                    pacvm.ComponentName = Componentdetails[i].ComponentName;
        //////                    pacvm.ComponentCode = Componentdetails[i].ComponentCode;
        //////                    pacvm.ComponentValue = "0.00";
        //////                    //Logic & Calculation
        //////                    pacvm.LogicId = Componentdetails[i].LogicId;
        //////                    pacvm.Percentage = Componentdetails[i].Percentage;
        //////                    pacvm.Value = Componentdetails[i].Value;
        //////                    pacvm.ComponentId1 = Componentdetails[i].ComponentId1;
        //////                    pacvm.ComponentName1 = Componentdetails[i].ComponentName1;
        //////                    pacvm.EffectiveFrom = Componentdetails[i].EffectiveFrom;
        //////                    pacvm.EffectiveTo = Componentdetails[i].EffectiveTo;
        //////                    //Condition
        //////                    pacvm.ConditionId = Componentdetails[i].ConditionId;
        //////                    pacvm.ConditionExpression = Componentdetails[i].ConditionExpression;
        //////                    pacvm.ConditionResultPFESI = Componentdetails[i].ConditionResultPFESI;

        //////                    if (pacvm.Value != null)
        //////                    {
        //////                        pacvm.ComponentValue = Convert.ToString(pacvm.Value);
        //////                    }
        //////                    else if (pacvm.Percentage != null)
        //////                    {
        //////                        decimal? percentage = pacvm.Percentage;
        //////                        string newcomponentcode = pacvm.ComponentName1;
        //////                        decimal? newcomponentvalue = 0;
        //////                        string conditionexp = pacvm.ConditionExpression;

        //////                        for (int j = 0; j < lstofCompvalue.Count(); j++)
        //////                        {
        //////                            if (newcomponentcode.ToUpper() == pacvm.ComponentName1.ToUpper())
        //////                            {
        //////                                newcomponentvalue = Convert.ToDecimal(lstofCompvalue[j].ComponentValue);
        //////                            }
        //////                        }

        //////                        if (newcomponentvalue > 0)
        //////                        {
        //////                            if (conditionexp != null)
        //////                            {
        //////                                //var expression = "PF + GI + ESI + Grat";
        //////                                var expression = conditionexp;

        //////                                // Replace all keys with their values
        //////                                foreach (var item in lstofCompvalue)
        //////                                {
        //////                                    expression = expression.Replace(item.ComponentCode, item.ComponentValue.ToString());
        //////                                }

        //////                                // Evaluate expression
        //////                                var result = new DataTable().Compute(expression, null);
        //////                            }
        //////                        }
        //////                    }

        //////                    lstofCompvalue.Add(pacvm);

        //////                }
        //////                return lstofCompvalue;
        //////            }
        //////            else
        //////            {
        //////                throw new CustomApiException(HttpStatusCode.NotFound, "Component Details Not Found");
        //////            }
        //////        }
        //////    }
        //////    catch (CustomApiException ex)
        //////    {
        //////        throw new CustomApiException(ex.StatusCode, ex.Message);
        //////    }
        //////}

        ///----02.03.2026 revert the code for calculation removed multi login and calculation 


        ///////// ------ 20.04.2026 Fully working code (For variable pay hide this)
        public PayrollALLFULLComponentCompactViewModel EmpCTCCalculation(PayrollALLComponentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;
                double cvalue = 0;

                int? payouttypeid = 0;

                int year = Convert.ToInt32(model.Year);
                int month = model.MonthNo;

                decimal? totalDays = DateTime.DaysInMonth(year, month);

                // start & end dates
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

                var lop = (from lev in DB.EmpLeaveApplications
                           where lev.EmpId == loginId
                              && lev.LeaveTypeId == 0
                              && lev.StartDate >= startDate
                              && lev.EndDate <= endDate
                              && lev.IsActive == true
                              && lev.IsDeleted == false
                           orderby lev.StartDate descending
                           select lev).ToList();

                decimal? lopDuration = (from lev in DB.EmpLeaveApplications
                                        where lev.EmpId == loginId
                                           && lev.LeaveTypeId == 0
                                           && lev.StartDate >= startDate
                                           && lev.EndDate <= endDate
                                           && lev.IsActive == true
                                           && lev.IsDeleted == false
                                        select lev.Duration)
                                       .DefaultIfEmpty(0)           // avoid null result
                                       .Sum();

                decimal? workingdays = totalDays - lopDuration;

                if (loginId == null || loginId == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");

                DateTime Today = DateTime.Now;

                var gradedetails = (from emp in DB.EmployeeMasters
                                    join deg in DB.DesignationMasters
                                      on emp.DesignationId equals deg.DesignationId
                                    where emp.EmpId == empId && emp.EmpStatus.ToUpper() == "ACTIVE"
                                       && emp.IsActive == true && emp.IsDeleted == false
                                       && deg.IsActive == true && deg.IsDeleted == false
                                    orderby deg.DesignationId descending
                                    select deg).FirstOrDefault();

                if (gradedetails == null)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "User designation not found.");
                }
                if (gradedetails.Grade == null)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "User designation does not have a grade mapping. Kindly map the designation to the appropriate grade to continue.");
                }
                else
                {
                    var gradepayout = (from emp in DB.EmployeeMasters
                                       join deg in DB.DesignationMasters
                                         on emp.DesignationId equals deg.DesignationId
                                       join gpo in DB.PayoutMappingMasters
                                          on deg.GradeId equals gpo.GradeId
                                       where emp.EmpId == empId && emp.EmpStatus.ToUpper() == "ACTIVE"
                                           && emp.IsActive == true && emp.IsDeleted == false
                                           && deg.IsActive == true && deg.IsDeleted == false
                                           && gpo.IsActive == true && gpo.IsDeleted == false
                                       orderby deg.DesignationId descending
                                       select gpo).FirstOrDefault();

                    if (gradepayout == null)
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "User Grade does not have a Payout mapping. Kindly map the Grade to the appropriate Payout to continue.");
                    }

                    payouttypeid = gradepayout.PayoutTypeId;
                }

                // Get employee salary details (primary)
                var empSaldetails = (from emp in DB.EmployeeMasters
                                     join sal in DB.EmployeeSalaryDetails
                                       on emp.EmpCode.ToUpper() equals sal.EmpCode.ToUpper()
                                     where emp.EmpId == empId && sal.EmpId == empId && emp.EmpStatus.ToUpper() == "ACTIVE"
                                        && sal.EffectiveFromDate <= Today //&& sal.EffectiveToDate >= Today
                                        && emp.IsActive == true && emp.IsDeleted == false
                                        && sal.IsActive == true && sal.IsDeleted == false
                                     orderby sal.SalaryId descending
                                     select sal).FirstOrDefault();

                if (empSaldetails == null)
                {
                    var empSaldetails1 = (from emp in DB.EmployeeMasters
                                          join sal in DB.EmployeeSalaryDetails
                                            on emp.EmpCode.ToUpper() equals sal.EmpCode.ToUpper()
                                          where emp.EmpId == empId && sal.EmpId == empId && emp.EmpStatus.ToUpper() == "ACTIVE"
                                             && emp.IsActive == true && emp.IsDeleted == false
                                             && sal.IsActive == true && sal.IsDeleted == false
                                          orderby sal.SalaryId descending
                                          select sal).FirstOrDefault();

                    if (empSaldetails1 != null)
                        throw new CustomApiException(HttpStatusCode.NotFound, "The effective dates for the employee’s salary details have expired.");
                    else
                        throw new CustomApiException(HttpStatusCode.NotFound, "Salary details (CTC) for the selected employee were not found.");
                }

                bool? variable = empSaldetails.IsVariable;
                bool? cleararrear = empSaldetails.IsClearArrear;
                //DateTime? effectivedate = empSaldetails.CreatedDate;
                int? effectivemonth = empSaldetails.ArrearMonth ?? 0;
                int? effectiveyear = empSaldetails.ArrearYear ?? 0;

                bool? arrear = empSaldetails.IsArrear;
                double? arrearamt = Convert.ToDouble(empSaldetails.ArrearAmt);

                if (cleararrear == false)
                {
                    if (effectivemonth == month && effectiveyear == year)
                    {
                        arrear = true;
                    }
                    else
                    {
                        arrear = false;
                    }
                }
                else
                {
                    if (effectivemonth == month && effectiveyear == year)
                    {
                        arrear = true;
                    }
                    else
                    {
                        arrear = false;
                    }
                }

                // Helper: try to read numeric properties from empSaldetails into dictionary
                var salaryVars = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

                // Put CTC into map (you used CTC earlier)
                double ctcValue = empSaldetails.CTC != null ? Convert.ToDouble(empSaldetails.CTC) : 0.0;
                salaryVars["CTC"] = ctcValue;

                // Try to fill other known variable names by reflection (if properties exist)
                //var possibleNames = new[] { "MCTC", "GS", "BS", "HRA", "Con", "PF", "GI", "ESI", "Grat", "SB", "TD", "PT" };
                var possibleNames = new[] { "MCTC", "BS", "HRA", "Con", "ESIB", "PFB", "GI", "Grat", "SB", "GS", "PFB", "ESIB", "PT", "TD", "IA", "NS" };

                foreach (var name in possibleNames)
                {
                    var prop = empSaldetails.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (prop != null)
                    {
                        object val = prop.GetValue(empSaldetails, null);
                        double d = 0;
                        if (val != null && double.TryParse(val.ToString(), out d))
                            salaryVars[name] = d;
                        else
                            salaryVars[name] = 0.0;
                    }
                    else
                    {
                        if (!salaryVars.ContainsKey(name))
                            salaryVars[name] = 0.0;
                    }
                }

                // If MCTC not present, attempt derive from CTC (you can change logic as per your actual rules)
                if (!salaryVars.ContainsKey("MCTC") || salaryVars["MCTC"] == 0)
                    salaryVars["MCTC"] = ctcValue; // fallback - adjust as necessary

                // Get emp basic meta
                var empdetails = (from emp in DB.EmployeeMasters
                                  where emp.EmpId == empId && emp.EmpStatus.ToUpper() == "ACTIVE"
                                     && emp.IsActive == true && emp.IsDeleted == false
                                  orderby emp.EmpId descending
                                  select emp).FirstOrDefault();

                string EmpCode = empdetails.EmpCode;
                string FirstName = empdetails.FirstName;
                string MiddleName = empdetails.MiddleName;
                string LastName = empdetails.LastName;

                // Read components and related logic & condition as before
                var Componentdetails = (from com in DB.PayrollComponents
                                        join cal in DB.PayrollComponentLogics on com.ComponentId equals cal.ComponentId
                                        join con in DB.PayrollComponentConditions on com.ComponentId equals con.ComponentId
                                        join pay in DB.PayrollPayoutTypes on com.PayoutTypeId equals pay.PayoutTypeId
                                        join seg in DB.PayrollSegments on com.SegmentId equals seg.SegmentId
                                        where cal.SNo == con.SNo
                                        && com.IsActive == true && com.IsDeleted == false
                                        && cal.IsActive == true && cal.IsDeleted == false
                                        && con.IsActive == true && con.IsDeleted == false
                                        && pay.IsActive == true && pay.IsDeleted == false
                                        && seg.IsActive == true && seg.IsDeleted == false
                                        && pay.PayoutTypeId == payouttypeid
                                        orderby pay.PayoutTypeId ascending, seg.SegmentId ascending
                                        select new PayrollALLComponentCompactViewModel
                                        {
                                            PayoutTypeId = pay.PayoutTypeId,
                                            PayoutTypeName = pay.PayoutTypeName,
                                            FrequencyId = 0,
                                            Frequency = pay.Frequency,
                                            SegmentId = seg.SegmentId,
                                            SegmentName = seg.SegmentName,
                                            ComponentId = com.ComponentId,
                                            ComponentName = com.ComponentName,
                                            ComponentCode = com.ComponentCode,
                                            ComponentValue = "0.00",
                                            LogicId = cal.LogicId,
                                            Percentage = cal.Percentage,
                                            Value = cal.Value,
                                            ComponentId1 = cal.ComponentId1,
                                            ComponentName1 = cal.ComponentName1,
                                            EffectiveFrom = cal.EffectiveFrom,
                                            EffectiveTo = cal.EffectiveTo,
                                            ConditionId = con.ConditionId,
                                            ConditionExpression = con.ConditionExpression,
                                            ConditionResultPFESI = con.ConditionResultPFESI,
                                            LCtrue = 0,
                                        }).ToList();

                if (Componentdetails == null || Componentdetails.Count == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "Component details are not found");

                // Result list
                List<PayrollALLComponentCompactViewModel> lstofCompvalue = new List<PayrollALLComponentCompactViewModel>();

                // Keep a map of computed component values by name (so other components can reference them)
                var computedValues = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

                // Seed computedValues with salaryVars
                foreach (var kv in salaryVars)
                    if (!computedValues.ContainsKey(kv.Key))
                        computedValues[kv.Key] = kv.Value;

                // Helper: evaluate arithmetic expression (with variables replaced)
                Func<string, double> EvaluateArithmetic = expr =>
                {
                    if (string.IsNullOrWhiteSpace(expr)) return 0.0;
                    // Replace any multiple spaces with single space
                    expr = Regex.Replace(expr, @"\s+", " ").Trim();

                    // Replace variable tokens with numeric values from computedValues or salaryVars
                    // Tokenize by words matching letters/numbers/underscore
                    var varPattern = new Regex(@"\b[A-Za-z_][A-Za-z0-9_]*\b");
                    string replaced = varPattern.Replace(expr, match =>
                    {
                        string token = match.Value;
                        double v = 0.0;
                        if (computedValues.TryGetValue(token, out v))
                            return v.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        if (salaryVars.TryGetValue(token, out v))
                            return v.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        // else treat as 0
                        return "0";
                    });

                    // Validate: allow only digits, decimal, operators, parentheses, spaces and minus
                    if (!Regex.IsMatch(replaced, @"^[0-9\.\-\+\*\/\(\)\s]+$"))
                        throw new Exception("Invalid characters in expression after substitution.");

                    // Use DataTable.Compute to evaluate arithmetic
                    try
                    {
                        var dt = new DataTable();
                        var valObj = dt.Compute(replaced, "");
                        double val = 0.0;
                        double.TryParse(Convert.ToString(valObj), out val);
                        return val;
                    }
                    catch
                    {
                        return 0.0;
                    }
                };

                // Helper: evaluate condition expression. Supports:
                // - single comparisons (A > 10)
                // - range (10 <= A <= 20)
                // - OR separated subconditions using "OR" or "(OR)"
                Func<string, bool> EvaluateCondition = condExpr =>
                {
                    if (string.IsNullOrWhiteSpace(condExpr)) return true; // no condition => pass

                    // Normalize OR tokens
                    condExpr = condExpr.Replace("(OR)", " OR ").Replace("(or)", " OR ").Replace("||", " OR ");
                    // Split on OR (top-level)
                    var orParts = Regex.Split(condExpr, @"\s+OR\s+", RegexOptions.IgnoreCase).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                    foreach (var part in orParts)
                    {
                        string p = part.Trim();

                        // Range form? detect "a <= VAR <= b" or "a <= VAR" style
                        // Pattern: number <op> VAR <op> number  (e.g., "15000 <= MCTC <= 21000")
                        var rangeMatch = Regex.Match(p, @"^\s*(?<left>[-\d\.]+)\s*(<=|<)\s*(?<var>[A-Za-z_][A-Za-z0-9_]*)\s*(<=|<)\s*(?<right>[-\d\.]+)\s*$");
                        if (rangeMatch.Success)
                        {
                            double left = Convert.ToDouble(rangeMatch.Groups["left"].Value);
                            string varName = rangeMatch.Groups["var"].Value;
                            double right = Convert.ToDouble(rangeMatch.Groups["right"].Value);
                            double varVal = 0;
                            if (!computedValues.TryGetValue(varName, out varVal)) computedValues.TryGetValue(varName, out varVal);
                            if (varVal >= left && varVal <= right) return true;
                            else continue;
                        }

                        // Alternative range form "VAR >= x AND VAR <= y" or "x <= VAR <= y" handled above partially
                        // Try generic comparison: leftOperator right (like MCTC > 26000 or 26000 <= MCTC)
                        var compMatch = Regex.Match(p, @"^\s*(?<left>[A-Za-z0-9\.\-\+\s\(\)]+)\s*(?<op>>=|<=|>|<|==|=|!=)\s*(?<right>[A-Za-z0-9\.\-\+\s\(\)]+)\s*$");
                        if (compMatch.Success)
                        {
                            string leftToken = compMatch.Groups["left"].Value.Trim();
                            string op = compMatch.Groups["op"].Value.Trim();
                            string rightToken = compMatch.Groups["right"].Value.Trim();

                            // Determine numeric values for left and right (either variable or arithmetic)
                            double leftVal = 0, rightVal = 0;
                            // If leftToken is a variable or expression
                            leftVal = EvaluateArithmetic(leftToken);
                            rightVal = EvaluateArithmetic(rightToken);

                            bool result = false;
                            switch (op)
                            {
                                case ">": result = leftVal > rightVal; break;
                                case "<": result = leftVal < rightVal; break;
                                case ">=": result = leftVal >= rightVal; break;
                                case "<=": result = leftVal <= rightVal; break;
                                case "==":
                                case "=": result = Math.Abs(leftVal - rightVal) < 0.000001; break;
                                case "!=": result = Math.Abs(leftVal - rightVal) > 0.000001; break;
                                default: result = false; break;
                            }

                            if (result) return true;
                            else continue;
                        }

                        // If not matched above, as a last attempt evaluate whole expression as boolean by replacing variables and checking >0
                        try
                        {
                            double val = EvaluateArithmetic(p);
                            if (val != 0)
                                cvalue = val;
                            return true;
                        }
                        catch
                        {
                            // ignore
                        }
                    }

                    // none of OR parts returned true
                    return false;
                };

                // Main loop — compute each component
                for (int i = 0; i < Componentdetails.Count(); i++)
                {
                    var cd = Componentdetails[i];

                    PayrollALLComponentCompactViewModel pacvm = new PayrollALLComponentCompactViewModel();

                    // Copy metadata
                    pacvm.EmpId = (int)empId;
                    pacvm.EmpCode = EmpCode;
                    pacvm.FirstName = FirstName;
                    pacvm.MiddleName = MiddleName;
                    pacvm.LastName = LastName;
                    pacvm.LoginId = (int)loginId;

                    pacvm.PayoutTypeId = cd.PayoutTypeId;
                    pacvm.PayoutTypeName = cd.PayoutTypeName;
                    pacvm.FrequencyId = 0;
                    pacvm.Frequency = cd.Frequency;

                    pacvm.SegmentId = cd.SegmentId;
                    pacvm.SegmentName = cd.SegmentName;

                    pacvm.ComponentId = cd.ComponentId;
                    pacvm.ComponentName = cd.ComponentName;
                    pacvm.ComponentCode = cd.ComponentCode;

                    // Copy logic/condition metadata back into VM so caller has it
                    pacvm.LogicId = cd.LogicId;
                    pacvm.Percentage = cd.Percentage;
                    pacvm.Value = cd.Value;
                    pacvm.ComponentId1 = cd.ComponentId1;
                    pacvm.ComponentName1 = cd.ComponentName1;
                    pacvm.EffectiveFrom = cd.EffectiveFrom;
                    pacvm.EffectiveTo = cd.EffectiveTo;
                    pacvm.ConditionId = cd.ConditionId;
                    pacvm.ConditionExpression = cd.ConditionExpression;
                    pacvm.ConditionResultPFESI = cd.ConditionResultPFESI;

                    // We compute a numeric value, then format as string
                    double computed = 0.0;

                    // 1) If Value present (explicit value) -> use it directly
                    double valueParsed = 0;
                    bool hasValue = cd.Value.HasValue;   // checking decimal?
                    if (hasValue)
                    {
                        valueParsed = Convert.ToDouble(cd.Value.Value);
                    }
                    //bool hasValue = !string.IsNullOrWhiteSpace(cd.Value) && double.TryParse(cd.Value, out valueParsed);

                    // 2) If percentage present -> compute percent of the referenced component
                    double percentageParsed = 0;
                    bool hasPercentage = cd.Percentage.HasValue;   // checking decimal?
                    if (hasPercentage)
                    {
                        percentageParsed = Convert.ToDouble(cd.Percentage.Value);
                    }
                    //bool hasPercentage = !string.IsNullOrWhiteSpace(cd.Percentage) && double.TryParse(cd.Percentage, out percentageParsed);

                    // Determine operand variable name (ComponentName1 is preferred per your notes)
                    string operandName = !string.IsNullOrWhiteSpace(cd.ComponentName1) ? cd.ComponentName1 : cd.ComponentName1;
                    // If operandName includes spaces like "MCTC (something)" we take token before space
                    if (!string.IsNullOrWhiteSpace(operandName))
                        operandName = operandName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

                    double operandValue = 0;
                    ////if (!string.IsNullOrWhiteSpace(operandName))
                    ////{
                    ////    if (!computedValues.TryGetValue(operandName, out operandValue))
                    ////    {
                    ////        // fallback to salaryVars
                    ////        salaryVars.TryGetValue(operandName, out operandValue);
                    ////    }
                    ////}


                    if (!string.IsNullOrWhiteSpace(operandName))
                    {
                        for (int j = 0; j < lstofCompvalue.Count(); j++)
                        {
                            if (operandName.ToUpper() == lstofCompvalue[j].ComponentCode.ToUpper())
                            {
                                operandValue = Convert.ToDouble(lstofCompvalue[j].ComponentValue);
                                salaryVars.TryGetValue(operandName, out operandValue);
                            }
                        }
                    }

                    if (hasValue)
                    {
                        computed = valueParsed;
                    }
                    else if (hasPercentage)
                    {
                        computed = (percentageParsed / 100.0) * operandValue;
                    }
                    ////else
                    ////{
                    ////    // If neither value nor percentage given, try to evaluate a formula if present in ComponentName1 (like "PF + GI + ESI + Grat")
                    ////    // Use ConditionExpression or ComponentName1 as formula candidate
                    ////    if (!string.IsNullOrWhiteSpace(cd.ComponentName1))
                    ////    {
                    ////        try
                    ////        {
                    ////            computed = EvaluateArithmetic(cd.ComponentName1);
                    ////        }
                    ////        catch
                    ////        {
                    ////            computed = 0.0;
                    ////        }
                    ////    }
                    ////    else
                    ////    {
                    ////        computed = 0.0;
                    ////    }
                    ////}

                    // Now evaluate the ConditionExpression (if any) to verify whether to accept the computed value
                    bool condOk = true;
                    if (!string.IsNullOrWhiteSpace(cd.ConditionExpression))
                    {
                        try
                        {
                            condOk = EvaluateCondition(cd.ConditionExpression);
                        }
                        catch
                        {
                            condOk = false;
                        }
                    }

                    if (!condOk)
                    {
                        // Condition failed -> set to 0
                        computed = 0.0;
                    }
                    else
                    {
                        if (cvalue != 0)
                        {
                            computed = cvalue;
                            cvalue = 0;
                        }
                    }

                    if (cd.ComponentCode.ToUpper() == "CTC")
                    {
                        computed = ctcValue;
                    }
                    ////if (cd.ComponentCode.ToUpper() == "MCTC")
                    ////{
                    ////    double mctc = computed;

                    ////    double PayableSalary = mctc * ((double)workingdays / (double)totalDays);

                    ////    computed = PayableSalary;
                    ////}

                    ////// Save computed value into maps for other components referencing it
                    ////if (!computedValues.ContainsKey(cd.ComponentName))
                    ////    computedValues[cd.ComponentName] = computed;
                    ////else
                    ////    computedValues[cd.ComponentName] = computed; // overwrite latest

                    // Also store by ComponentCode key for convenience
                    if (!string.IsNullOrWhiteSpace(cd.ComponentCode))
                    {
                        if (!computedValues.ContainsKey(cd.ComponentCode))
                            computedValues[cd.ComponentCode] = computed;
                        else
                            computedValues[cd.ComponentCode] = computed;
                    }

                    if (!string.IsNullOrWhiteSpace(cd.ComponentCode))
                    {
                        if (!salaryVars.ContainsKey(cd.ComponentCode))
                            salaryVars[cd.ComponentCode] = computed;
                        else
                            salaryVars[cd.ComponentCode] = computed;
                    }

                    // Format component value for response
                    pacvm.ComponentValue = computed.ToString("0.##"); // you can change formatting

                    lstofCompvalue.Add(pacvm);
                }

                List<PayrollALLComponentCompactViewModel> lstofArrearComponentDetails = new List<PayrollALLComponentCompactViewModel>();

                if (arrear == true)
                {
                    // Get MCT value from ArrearAmt
                    arrearamt = empSaldetails.ArrearAmt != null ? Convert.ToDouble(empSaldetails.ArrearAmt) : 0;

                    // Calculate arrear components using existing database logic
                    var arrearComponents = CalculateArrearComponents(
                        (int)empId,
                        year,
                        month,
                        (int)loginId,
                        EmpCode,
                        FirstName,
                        MiddleName,
                        LastName,
                        arrearamt,
                        payouttypeid ?? 0
                    );

                    lstofArrearComponentDetails = arrearComponents;
                }
                else
                {
                    lstofArrearComponentDetails = new List<PayrollALLComponentCompactViewModel>();
                }

                PayrollALLFULLComponentCompactViewModel pafccvm = new PayrollALLFULLComponentCompactViewModel();
                pafccvm.lstofComponentDetails = lstofCompvalue;
                pafccvm.lstofArrearComponentDetails = lstofArrearComponentDetails;

                return pafccvm;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
            ////catch (Exception ex)
            ////{
            ////    // Log and wrap
            ////    logger.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            ////    throw new CustomApiException(HttpStatusCode.InternalServerError, "Error while calculating components: " + ex.Message);
            ////}
        }
        // ========== ADD THIS METHOD INSIDE YOUR CLASS ==========
        private List<PayrollALLComponentCompactViewModel> GetVariablePayComponents(int empId, int year, int month, int loginId, string empCode, string firstName, string middleName, string lastName, 
            int varpayouttypeId, string varpayouttypeName, int varsegmentId, string varsegmentName)
        {
            List<PayrollALLComponentCompactViewModel> variableComponents = new List<PayrollALLComponentCompactViewModel>();

            try
            {
                // Get employee salary details to check if variable pay is enabled
                var empSalaryDetail = DB.EmployeeSalaryDetails
                    .FirstOrDefault(sal => sal.EmpId == empId &&
                                           sal.IsActive == true &&
                                           sal.IsDeleted == false &&
                                           sal.IsVariable == true);

                if (empSalaryDetail == null || empSalaryDetail.IsVariable != true)
                    return variableComponents; // No variable pay for this employee

                // Get active variable pay definitions from PayrollVariable table
                var activeVariables = DB.PayrollVariables
                    .Where(v => v.IsActive == true && v.IsDeleted == false && v.Status == true)
                    .ToList();

                if (activeVariables == null || !activeVariables.Any())
                    return variableComponents;

                // Get variable history for current employee, year and month
                var variableHistory = DB.VariableHistories
                    .Where(vh => vh.EmpId == empId &&
                                vh.Year == year &&
                                vh.Month == month &&
                                vh.IsActive == true &&
                                vh.IsDeleted == false)
                    .ToDictionary(vh => vh.VariableId, vh => vh);

                // Create component for each active variable
                foreach (var variable in activeVariables)
                {
                    PayrollALLComponentCompactViewModel varComponent = new PayrollALLComponentCompactViewModel();

                    // Set employee basic info
                    varComponent.EmpId = empId;
                    varComponent.EmpCode = empCode;
                    varComponent.FirstName = firstName;
                    varComponent.MiddleName = middleName;
                    varComponent.LastName = lastName;
                    varComponent.LoginId = loginId;

                    // Set component metadata
                    varComponent.PayoutTypeId = varpayouttypeId;
                    varComponent.PayoutTypeName = varpayouttypeName;
                    varComponent.FrequencyId = 0;
                    varComponent.Frequency = "Monthly";
                    varComponent.SegmentId = varsegmentId; // High number to ensure it comes last
                    varComponent.SegmentName = varsegmentName;

                    varComponent.ComponentId = variable.VariableId;
                    varComponent.ComponentName = variable.VariableName;
                    varComponent.ComponentCode = variable.VariableCode;

                    // Check if variable exists in history for current month
                    if (variableHistory.ContainsKey(variable.VariableId))
                    {
                        var history = variableHistory[variable.VariableId];

                        if (decimal.TryParse(history.VariableAmt?.ToString(), out var value))
                        {
                            varComponent.ComponentValue = value.ToString("0.##");
                        }
                        else
                        {
                            varComponent.ComponentValue = "0.00";
                        }
                    }
                    else
                    {
                        varComponent.ComponentValue = "0.00";
                    }

                    // Set default values for other properties
                    varComponent.LogicId = 0;
                    varComponent.Percentage = 0;
                    varComponent.Value = 0;
                    varComponent.ComponentId1 = 0;
                    varComponent.ComponentName1 = "";
                    varComponent.EffectiveFrom = null;
                    varComponent.EffectiveTo = null;
                    varComponent.ConditionId = 0;
                    varComponent.ConditionExpression = "";
                    varComponent.ConditionResultPFESI = null;
                    varComponent.LCtrue = 0;

                    variableComponents.Add(varComponent);
                }
            }
            catch (Exception ex)
            {
                // If variable pay fails, return empty list to not break main flow
                return new List<PayrollALLComponentCompactViewModel>();
            }

            return variableComponents;
        }
        private List<PayrollALLComponentCompactViewModel> CalculateArrearComponents(
    int empId,
    int year,
    int month,
    int loginId,
    string empCode,
    string firstName,
    string middleName,
    string lastName,
    double? arrearAmt,
    int payouttypeId)
        {
            List<PayrollALLComponentCompactViewModel> arrearComponents = new List<PayrollALLComponentCompactViewModel>();

            try
            {
                if (arrearAmt == null || arrearAmt == 0)
                    return arrearComponents;

                // MCT value from ArrearAmt
                double mctValue = Convert.ToDouble(arrearAmt);

                // Get the components with their logic and conditions from database
                // Only for components: BS, IA, HRA, CA, GS
                var arrearComponentDetails = (from com in DB.PayrollComponents
                                              join cal in DB.PayrollComponentLogics on com.ComponentId equals cal.ComponentId
                                              join con in DB.PayrollComponentConditions on com.ComponentId equals con.ComponentId
                                              where (com.ComponentCode == "BS" || com.ComponentCode == "IA" ||
                                                     com.ComponentCode == "HRA" || com.ComponentCode == "Con" ||
                                                     com.ComponentCode == "GS")
                                              && com.IsActive == true && com.IsDeleted == false
                                              && cal.IsActive == true && cal.IsDeleted == false
                                              && con.IsActive == true && con.IsDeleted == false
                                              && com.PayoutTypeId == payouttypeId
                                              select new PayrollALLComponentCompactViewModel
                                              {
                                                  ComponentId = com.ComponentId,
                                                  ComponentName = com.ComponentName,
                                                  ComponentCode = com.ComponentCode,
                                                  LogicId = cal.LogicId,
                                                  Percentage = cal.Percentage,
                                                  Value = cal.Value,
                                                  ComponentId1 = cal.ComponentId1,
                                                  ComponentName1 = cal.ComponentName1,
                                                  EffectiveFrom = cal.EffectiveFrom,
                                                  EffectiveTo = cal.EffectiveTo,
                                                  ConditionId = con.ConditionId,
                                                  ConditionExpression = con.ConditionExpression,
                                                  ConditionResultPFESI = con.ConditionResultPFESI,
                                              }).ToList();

                if (arrearComponentDetails == null || !arrearComponentDetails.Any())
                    return arrearComponents;

                // Create a dictionary to store computed values (similar to your main logic)
                var computedValues = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

                // Set MCTC value from arrear amount
                computedValues["MCTC"] = mctValue;
                computedValues["CTC"] = mctValue;

                // Store computed component values by ComponentCode
                var componentResults = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

                // Helper: evaluate arithmetic expression (same as your main logic)
                Func<string, double> EvaluateArithmetic = expr =>
                {
                    if (string.IsNullOrWhiteSpace(expr)) return 0.0;
                    expr = Regex.Replace(expr, @"\s+", " ").Trim();

                    var varPattern = new Regex(@"\b[A-Za-z_][A-Za-z0-9_]*\b");
                    string replaced = varPattern.Replace(expr, match =>
                    {
                        string token = match.Value;
                        double v = 0.0;
                        if (computedValues.TryGetValue(token, out v))
                            return v.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        if (componentResults.TryGetValue(token, out v))
                            return v.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        return "0";
                    });

                    if (!Regex.IsMatch(replaced, @"^[0-9\.\-\+\*\/\(\)\s]+$"))
                        throw new Exception("Invalid characters in expression after substitution.");

                    try
                    {
                        var dt = new DataTable();
                        var valObj = dt.Compute(replaced, "");
                        double val = 0.0;
                        double.TryParse(Convert.ToString(valObj), out val);
                        return val;
                    }
                    catch
                    {
                        return 0.0;
                    }
                };

                // Helper: evaluate condition (same as your main logic)
                Func<string, bool> EvaluateCondition = condExpr =>
                {
                    if (string.IsNullOrWhiteSpace(condExpr)) return true;

                    condExpr = condExpr.Replace("(OR)", " OR ").Replace("(or)", " OR ").Replace("||", " OR ");
                    var orParts = Regex.Split(condExpr, @"\s+OR\s+", RegexOptions.IgnoreCase).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

                    foreach (var part in orParts)
                    {
                        string p = part.Trim();

                        var rangeMatch = Regex.Match(p, @"^\s*(?<left>[-\d\.]+)\s*(<=|<)\s*(?<var>[A-Za-z_][A-Za-z0-9_]*)\s*(<=|<)\s*(?<right>[-\d\.]+)\s*$");
                        if (rangeMatch.Success)
                        {
                            double left = Convert.ToDouble(rangeMatch.Groups["left"].Value);
                            string varName = rangeMatch.Groups["var"].Value;
                            double right = Convert.ToDouble(rangeMatch.Groups["right"].Value);
                            double varVal = 0;
                            if (!computedValues.TryGetValue(varName, out varVal))
                                componentResults.TryGetValue(varName, out varVal);
                            if (varVal >= left && varVal <= right) return true;
                            else continue;
                        }

                        var compMatch = Regex.Match(p, @"^\s*(?<left>[A-Za-z0-9\.\-\+\s\(\)]+)\s*(?<op>>=|<=|>|<|==|=|!=)\s*(?<right>[A-Za-z0-9\.\-\+\s\(\)]+)\s*$");
                        if (compMatch.Success)
                        {
                            string leftToken = compMatch.Groups["left"].Value.Trim();
                            string op = compMatch.Groups["op"].Value.Trim();
                            string rightToken = compMatch.Groups["right"].Value.Trim();

                            double leftVal = EvaluateArithmetic(leftToken);
                            double rightVal = EvaluateArithmetic(rightToken);

                            bool result = false;
                            switch (op)
                            {
                                case ">": result = leftVal > rightVal; break;
                                case "<": result = leftVal < rightVal; break;
                                case ">=": result = leftVal >= rightVal; break;
                                case "<=": result = leftVal <= rightVal; break;
                                case "==":
                                case "=": result = Math.Abs(leftVal - rightVal) < 0.000001; break;
                                case "!=": result = Math.Abs(leftVal - rightVal) > 0.000001; break;
                                default: result = false; break;
                            }
                            if (result) return true;
                            else continue;
                        }

                        try
                        {
                            double val = EvaluateArithmetic(p);
                            if (val != 0) return true;
                        }
                        catch { }
                    }
                    return false;
                };

                // Calculate each component in specific order: BS, Con, HRA, then IA, then GS
                var orderedComponents = arrearComponentDetails
                    .OrderBy(c => c.ComponentCode == "GS" ? 3 :
                                  c.ComponentCode == "IA" ? 2 :
                                  c.ComponentCode == "HRA" ? 1 :
                                  c.ComponentCode == "Con" ? 0 : -1)
                    .ToList();

                foreach (var component in orderedComponents)
                {
                    double computed = 0.0;
                    double percentage = component.Percentage != null ? Convert.ToDouble(component.Percentage) : 0;
                    double fixedValue = component.Value != null ? Convert.ToDouble(component.Value) : 0;

                    // Get operand component value if ComponentId1 exists
                    double operandValue = 0;
                    if (component.ComponentId1 > 0 && !string.IsNullOrWhiteSpace(component.ComponentName1))
                    {
                        string operandName = component.ComponentName1.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

                        // Check if operand is already computed
                        if (componentResults.TryGetValue(operandName, out operandValue))
                        {
                            // Use computed value
                        }
                        else if (computedValues.TryGetValue(operandName, out operandValue))
                        {
                            // Use from computed values
                        }
                    }

                    // HARDCODED LOGIC FOR IA AND GS
                    if (component.ComponentCode.ToUpper() == "GS")
                    {
                        // GS = MCTC (arrear amount)
                        computed = mctValue;
                    }
                    else if (component.ComponentCode.ToUpper() == "IA")
                    {
                        // IA = MCTC - (BS + HRA + Con)
                        double bsVal = componentResults.ContainsKey("BS") ? componentResults["BS"] : 0;
                        double hraVal = componentResults.ContainsKey("HRA") ? componentResults["HRA"] : 0;
                        double conVal = componentResults.ContainsKey("Con") ? componentResults["Con"] : 0;
                        computed = mctValue - (bsVal + hraVal + conVal);
                    }
                    else
                    {
                        // Calculate based on Percentage or Value for other components (BS, HRA, Con)
                        if (fixedValue > 0)
                        {
                            computed = fixedValue;
                        }
                        else if (percentage > 0)
                        {
                            computed = (percentage / 100.0) * mctValue;
                        }
                        else if (!string.IsNullOrWhiteSpace(component.ComponentName1))
                        {
                            // Try to evaluate as formula
                            try
                            {
                                computed = EvaluateArithmetic(component.ComponentName1);
                            }
                            catch
                            {
                                computed = 0.0;
                            }
                        }
                    }

                    // Evaluate condition
                    bool condOk = true;
                    if (!string.IsNullOrWhiteSpace(component.ConditionExpression))
                    {
                        try
                        {
                            condOk = EvaluateCondition(component.ConditionExpression);
                        }
                        catch
                        {
                            condOk = false;
                        }
                    }

                    if (!condOk)
                    {
                        computed = 0.0;
                    }

                    // Store computed value for reference by other components
                    if (!componentResults.ContainsKey(component.ComponentCode))
                        componentResults[component.ComponentCode] = computed;
                    else
                        componentResults[component.ComponentCode] = computed;

                    // Create component view model
                    PayrollALLComponentCompactViewModel arrearComp = new PayrollALLComponentCompactViewModel();

                    // Employee basic info
                    arrearComp.EmpId = empId;
                    arrearComp.EmpCode = empCode;
                    arrearComp.FirstName = firstName;
                    arrearComp.MiddleName = middleName;
                    arrearComp.LastName = lastName;
                    arrearComp.LoginId = loginId;

                    // Component metadata
                    arrearComp.PayoutTypeId = payouttypeId;
                    arrearComp.PayoutTypeName = "Arrear Adjustment";
                    arrearComp.FrequencyId = 0;
                    arrearComp.Frequency = "Monthly";
                    arrearComp.SegmentId = 0; // Arrear segment
                    arrearComp.SegmentName = "Arrear Adjustment";

                    arrearComp.ComponentId = component.ComponentId;
                    arrearComp.ComponentName = component.ComponentName;
                    arrearComp.ComponentCode = component.ComponentCode;
                    arrearComp.ComponentValue = computed.ToString("0.##");

                    // Logic details
                    arrearComp.LogicId = component.LogicId;
                    arrearComp.Percentage = component.Percentage ?? 0;
                    arrearComp.Value = component.Value ?? 0;
                    arrearComp.ComponentId1 = component.ComponentId1 ?? 0;
                    arrearComp.ComponentName1 = component.ComponentName1 ?? "";
                    arrearComp.EffectiveFrom = component.EffectiveFrom;
                    arrearComp.EffectiveTo = component.EffectiveTo;
                    arrearComp.ConditionId = component.ConditionId;
                    arrearComp.ConditionExpression = component.ConditionExpression ?? "";
                    arrearComp.ConditionResultPFESI = component.ConditionResultPFESI;

                    arrearComponents.Add(arrearComp);
                }
            }
            catch (Exception ex)
            {
                // Log error if needed
                return new List<PayrollALLComponentCompactViewModel>();
            }

            return arrearComponents;
        }

        ///
        ///// ------ 20.04.2026 Fully working code (For variable pay hide this)

        //////// ------ 20.04.2026 with variable pay
        //////public List<PayrollALLComponentCompactViewModel> EmpCTCCalculation(PayrollALLComponentViewModel model)
        //////{
        //////    try
        //////    {
        //////        string msg = "";
        //////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        //////        int? empId = (model.EmpId != 0) ? model.EmpId : 0;
        //////        double cvalue = 0;

        //////        int? payouttypeid = 0;

        //////        int year = Convert.ToInt32(model.Year);
        //////        int month = model.MonthNo;

        //////        decimal? totalDays = DateTime.DaysInMonth(year, month);

        //////        // start & end dates
        //////        DateTime startDate = new DateTime(year, month, 1);
        //////        DateTime endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

        //////        var lop = (from lev in DB.EmpLeaveApplications
        //////                   where lev.EmpId == loginId
        //////                      && lev.LeaveTypeId == 0
        //////                      && lev.StartDate >= startDate
        //////                      && lev.EndDate <= endDate
        //////                      && lev.IsActive == true
        //////                      && lev.IsDeleted == false
        //////                   orderby lev.StartDate descending
        //////                   select lev).ToList();

        //////        decimal? lopDuration = (from lev in DB.EmpLeaveApplications
        //////                                where lev.EmpId == loginId
        //////                                   && lev.LeaveTypeId == 0
        //////                                   && lev.StartDate >= startDate
        //////                                   && lev.EndDate <= endDate
        //////                                   && lev.IsActive == true
        //////                                   && lev.IsDeleted == false
        //////                                select lev.Duration)
        //////                               .DefaultIfEmpty(0)           // avoid null result
        //////                               .Sum();

        //////        decimal? workingdays = totalDays - lopDuration;

        //////        if (loginId == null || loginId == 0)
        //////            throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");

        //////        DateTime Today = DateTime.Now;

        //////        var gradedetails = (from emp in DB.EmployeeMasters
        //////                            join deg in DB.DesignationMasters
        //////                              on emp.DesignationId equals deg.DesignationId
        //////                            where emp.EmpId == empId && emp.EmpStatus.ToUpper() == "ACTIVE"
        //////                               && emp.IsActive == true && emp.IsDeleted == false
        //////                               && deg.IsActive == true && deg.IsDeleted == false
        //////                            orderby deg.DesignationId descending
        //////                            select deg).FirstOrDefault();

        //////        if (gradedetails == null)
        //////        {
        //////            throw new CustomApiException(HttpStatusCode.NotFound, "User designation not found.");
        //////        }
        //////        if (gradedetails.Grade == null)
        //////        {
        //////            throw new CustomApiException(HttpStatusCode.NotFound, "User designation does not have a grade mapping. Kindly map the designation to the appropriate grade to continue.");
        //////        }
        //////        else
        //////        {
        //////            var gradepayout = (from emp in DB.EmployeeMasters
        //////                               join deg in DB.DesignationMasters
        //////                                 on emp.DesignationId equals deg.DesignationId
        //////                               join gpo in DB.PayoutMappingMasters
        //////                                  on deg.GradeId equals gpo.GradeId
        //////                               where emp.EmpId == empId && emp.EmpStatus.ToUpper() == "ACTIVE"
        //////                                   && emp.IsActive == true && emp.IsDeleted == false
        //////                                   && deg.IsActive == true && deg.IsDeleted == false
        //////                                   && gpo.IsActive == true && gpo.IsDeleted == false
        //////                               orderby deg.DesignationId descending
        //////                               select gpo).FirstOrDefault();

        //////            if (gradepayout == null)
        //////            {
        //////                throw new CustomApiException(HttpStatusCode.NotFound, "User Grade does not have a Payout mapping. Kindly map the Grade to the appropriate Payout to continue.");
        //////            }

        //////            payouttypeid = gradepayout.PayoutTypeId;
        //////        }

        //////        // Get employee salary details (primary)
        //////        var empSaldetails = (from emp in DB.EmployeeMasters
        //////                             join sal in DB.EmployeeSalaryDetails
        //////                               on emp.EmpCode.ToUpper() equals sal.EmpCode.ToUpper()
        //////                             where emp.EmpId == empId && sal.EmpId == empId && emp.EmpStatus.ToUpper() == "ACTIVE"
        //////                                && sal.EffectiveFromDate <= Today //&& sal.EffectiveToDate >= Today
        //////                                && emp.IsActive == true && emp.IsDeleted == false
        //////                                && sal.IsActive == true && sal.IsDeleted == false
        //////                             orderby sal.SalaryId descending
        //////                             select sal).FirstOrDefault();

        //////        if (empSaldetails == null)
        //////        {
        //////            var empSaldetails1 = (from emp in DB.EmployeeMasters
        //////                                  join sal in DB.EmployeeSalaryDetails
        //////                                    on emp.EmpCode.ToUpper() equals sal.EmpCode.ToUpper()
        //////                                  where emp.EmpId == empId && sal.EmpId == empId && emp.EmpStatus.ToUpper() == "ACTIVE"
        //////                                     && emp.IsActive == true && emp.IsDeleted == false
        //////                                     && sal.IsActive == true && sal.IsDeleted == false
        //////                                  orderby sal.SalaryId descending
        //////                                  select sal).FirstOrDefault();

        //////            if (empSaldetails1 != null)
        //////                throw new CustomApiException(HttpStatusCode.NotFound, "The effective dates for the employee’s salary details have expired.");
        //////            else
        //////                throw new CustomApiException(HttpStatusCode.NotFound, "Salary details (CTC) for the selected employee were not found.");
        //////        }

        //////        // Helper: try to read numeric properties from empSaldetails into dictionary
        //////        var salaryVars = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

        //////        // Put CTC into map (you used CTC earlier)
        //////        double ctcValue = empSaldetails.CTC != null ? Convert.ToDouble(empSaldetails.CTC) : 0.0;
        //////        salaryVars["CTC"] = ctcValue;

        //////        // Try to fill other known variable names by reflection (if properties exist)
        //////        //var possibleNames = new[] { "MCTC", "GS", "BS", "HRA", "Con", "PF", "GI", "ESI", "Grat", "SB", "TD", "PT" };
        //////        var possibleNames = new[] { "MCTC", "BS", "HRA", "Con", "ESIB", "PFB", "GI", "Grat", "SB", "GS", "PFB", "ESIB", "PT", "TD", "IA", "NS" };

        //////        foreach (var name in possibleNames)
        //////        {
        //////            var prop = empSaldetails.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        //////            if (prop != null)
        //////            {
        //////                object val = prop.GetValue(empSaldetails, null);
        //////                double d = 0;
        //////                if (val != null && double.TryParse(val.ToString(), out d))
        //////                    salaryVars[name] = d;
        //////                else
        //////                    salaryVars[name] = 0.0;
        //////            }
        //////            else
        //////            {
        //////                if (!salaryVars.ContainsKey(name))
        //////                    salaryVars[name] = 0.0;
        //////            }
        //////        }

        //////        // If MCTC not present, attempt derive from CTC (you can change logic as per your actual rules)
        //////        if (!salaryVars.ContainsKey("MCTC") || salaryVars["MCTC"] == 0)
        //////            salaryVars["MCTC"] = ctcValue; // fallback - adjust as necessary

        //////        // Get emp basic meta
        //////        var empdetails = (from emp in DB.EmployeeMasters
        //////                          where emp.EmpId == empId && emp.EmpStatus.ToUpper() == "ACTIVE"
        //////                             && emp.IsActive == true && emp.IsDeleted == false
        //////                          orderby emp.EmpId descending
        //////                          select emp).FirstOrDefault();

        //////        string EmpCode = empdetails.EmpCode;
        //////        string FirstName = empdetails.FirstName;
        //////        string MiddleName = empdetails.MiddleName;
        //////        string LastName = empdetails.LastName;

        //////        // Read components and related logic & condition as before
        //////        var Componentdetails = (from com in DB.PayrollComponents
        //////                                join cal in DB.PayrollComponentLogics on com.ComponentId equals cal.ComponentId
        //////                                join con in DB.PayrollComponentConditions on com.ComponentId equals con.ComponentId
        //////                                join pay in DB.PayrollPayoutTypes on com.PayoutTypeId equals pay.PayoutTypeId
        //////                                join seg in DB.PayrollSegments on com.SegmentId equals seg.SegmentId
        //////                                where cal.SNo == con.SNo
        //////                                && com.IsActive == true && com.IsDeleted == false
        //////                                && cal.IsActive == true && cal.IsDeleted == false
        //////                                && con.IsActive == true && con.IsDeleted == false
        //////                                && pay.IsActive == true && pay.IsDeleted == false
        //////                                && seg.IsActive == true && seg.IsDeleted == false
        //////                                && pay.PayoutTypeId == payouttypeid
        //////                                orderby pay.PayoutTypeId ascending, seg.SegmentId ascending
        //////                                select new PayrollALLComponentCompactViewModel
        //////                                {
        //////                                    PayoutTypeId = pay.PayoutTypeId,
        //////                                    PayoutTypeName = pay.PayoutTypeName,
        //////                                    FrequencyId = 0,
        //////                                    Frequency = pay.Frequency,
        //////                                    SegmentId = seg.SegmentId,
        //////                                    SegmentName = seg.SegmentName,
        //////                                    ComponentId = com.ComponentId,
        //////                                    ComponentName = com.ComponentName,
        //////                                    ComponentCode = com.ComponentCode,
        //////                                    ComponentValue = "0.00",
        //////                                    LogicId = cal.LogicId,
        //////                                    Percentage = cal.Percentage,
        //////                                    Value = cal.Value,
        //////                                    ComponentId1 = cal.ComponentId1,
        //////                                    ComponentName1 = cal.ComponentName1,
        //////                                    EffectiveFrom = cal.EffectiveFrom,
        //////                                    EffectiveTo = cal.EffectiveTo,
        //////                                    ConditionId = con.ConditionId,
        //////                                    ConditionExpression = con.ConditionExpression,
        //////                                    ConditionResultPFESI = con.ConditionResultPFESI,
        //////                                    LCtrue = 0,
        //////                                }).ToList();

        //////        // ========== START: VARIABLE PAY LOGIC ==========
        //////        // Get Variable Pay components
        //////        var variablePayComponents = GetVariablePayComponents(
        //////            (int)empId,
        //////            year,
        //////            month,
        //////            (int)loginId,
        //////            EmpCode,
        //////            FirstName,
        //////            MiddleName,
        //////            LastName
        //////        );

        //////        // Merge variable pay components with regular components
        //////        if (variablePayComponents != null && variablePayComponents.Any())
        //////        {
        //////            // Add variable pay components to the main list
        //////            Componentdetails.AddRange(variablePayComponents);

        //////            // Reorder to ensure variable pay components appear last within "Salary Benefits" segment
        //////            Componentdetails = Componentdetails
        //////                .OrderBy(c => c.SegmentName == "Salary Benefits" ? 1 : 0) // Non-Salary Benefits first
        //////                .ThenBy(c => c.SegmentId) // Then by original segment order
        //////                .ThenBy(c => c.ComponentName) // Then alphabetically within segment
        //////                .ToList();
        //////        }
        //////        // ========== END: VARIABLE PAY LOGIC ==========

        //////        if (Componentdetails == null || Componentdetails.Count == 0)
        //////            throw new CustomApiException(HttpStatusCode.NotFound, "Component details are not found");

        //////        // Result list
        //////        List<PayrollALLComponentCompactViewModel> lstofCompvalue = new List<PayrollALLComponentCompactViewModel>();

        //////        // Keep a map of computed component values by name (so other components can reference them)
        //////        var computedValues = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

        //////        // Seed computedValues with salaryVars
        //////        foreach (var kv in salaryVars)
        //////            if (!computedValues.ContainsKey(kv.Key))
        //////                computedValues[kv.Key] = kv.Value;

        //////        // Helper: evaluate arithmetic expression (with variables replaced)
        //////        Func<string, double> EvaluateArithmetic = expr =>
        //////        {
        //////            if (string.IsNullOrWhiteSpace(expr)) return 0.0;
        //////            // Replace any multiple spaces with single space
        //////            expr = Regex.Replace(expr, @"\s+", " ").Trim();

        //////            // Replace variable tokens with numeric values from computedValues or salaryVars
        //////            // Tokenize by words matching letters/numbers/underscore
        //////            var varPattern = new Regex(@"\b[A-Za-z_][A-Za-z0-9_]*\b");
        //////            string replaced = varPattern.Replace(expr, match =>
        //////            {
        //////                string token = match.Value;
        //////                double v = 0.0;
        //////                if (computedValues.TryGetValue(token, out v))
        //////                    return v.ToString(System.Globalization.CultureInfo.InvariantCulture);
        //////                if (salaryVars.TryGetValue(token, out v))
        //////                    return v.ToString(System.Globalization.CultureInfo.InvariantCulture);
        //////                // else treat as 0
        //////                return "0";
        //////            });

        //////            // Validate: allow only digits, decimal, operators, parentheses, spaces and minus
        //////            if (!Regex.IsMatch(replaced, @"^[0-9\.\-\+\*\/\(\)\s]+$"))
        //////                throw new Exception("Invalid characters in expression after substitution.");

        //////            // Use DataTable.Compute to evaluate arithmetic
        //////            try
        //////            {
        //////                var dt = new DataTable();
        //////                var valObj = dt.Compute(replaced, "");
        //////                double val = 0.0;
        //////                double.TryParse(Convert.ToString(valObj), out val);
        //////                return val;
        //////            }
        //////            catch
        //////            {
        //////                return 0.0;
        //////            }
        //////        };

        //////        // Helper: evaluate condition expression. Supports:
        //////        // - single comparisons (A > 10)
        //////        // - range (10 <= A <= 20)
        //////        // - OR separated subconditions using "OR" or "(OR)"
        //////        Func<string, bool> EvaluateCondition = condExpr =>
        //////        {
        //////            if (string.IsNullOrWhiteSpace(condExpr)) return true; // no condition => pass

        //////            // Normalize OR tokens
        //////            condExpr = condExpr.Replace("(OR)", " OR ").Replace("(or)", " OR ").Replace("||", " OR ");
        //////            // Split on OR (top-level)
        //////            var orParts = Regex.Split(condExpr, @"\s+OR\s+", RegexOptions.IgnoreCase).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        //////            foreach (var part in orParts)
        //////            {
        //////                string p = part.Trim();

        //////                // Range form? detect "a <= VAR <= b" or "a <= VAR" style
        //////                // Pattern: number <op> VAR <op> number  (e.g., "15000 <= MCTC <= 21000")
        //////                var rangeMatch = Regex.Match(p, @"^\s*(?<left>[-\d\.]+)\s*(<=|<)\s*(?<var>[A-Za-z_][A-Za-z0-9_]*)\s*(<=|<)\s*(?<right>[-\d\.]+)\s*$");
        //////                if (rangeMatch.Success)
        //////                {
        //////                    double left = Convert.ToDouble(rangeMatch.Groups["left"].Value);
        //////                    string varName = rangeMatch.Groups["var"].Value;
        //////                    double right = Convert.ToDouble(rangeMatch.Groups["right"].Value);
        //////                    double varVal = 0;
        //////                    if (!computedValues.TryGetValue(varName, out varVal)) computedValues.TryGetValue(varName, out varVal);
        //////                    if (varVal >= left && varVal <= right) return true;
        //////                    else continue;
        //////                }

        //////                // Alternative range form "VAR >= x AND VAR <= y" or "x <= VAR <= y" handled above partially
        //////                // Try generic comparison: leftOperator right (like MCTC > 26000 or 26000 <= MCTC)
        //////                var compMatch = Regex.Match(p, @"^\s*(?<left>[A-Za-z0-9\.\-\+\s\(\)]+)\s*(?<op>>=|<=|>|<|==|=|!=)\s*(?<right>[A-Za-z0-9\.\-\+\s\(\)]+)\s*$");
        //////                if (compMatch.Success)
        //////                {
        //////                    string leftToken = compMatch.Groups["left"].Value.Trim();
        //////                    string op = compMatch.Groups["op"].Value.Trim();
        //////                    string rightToken = compMatch.Groups["right"].Value.Trim();

        //////                    // Determine numeric values for left and right (either variable or arithmetic)
        //////                    double leftVal = 0, rightVal = 0;
        //////                    // If leftToken is a variable or expression
        //////                    leftVal = EvaluateArithmetic(leftToken);
        //////                    rightVal = EvaluateArithmetic(rightToken);

        //////                    bool result = false;
        //////                    switch (op)
        //////                    {
        //////                        case ">": result = leftVal > rightVal; break;
        //////                        case "<": result = leftVal < rightVal; break;
        //////                        case ">=": result = leftVal >= rightVal; break;
        //////                        case "<=": result = leftVal <= rightVal; break;
        //////                        case "==":
        //////                        case "=": result = Math.Abs(leftVal - rightVal) < 0.000001; break;
        //////                        case "!=": result = Math.Abs(leftVal - rightVal) > 0.000001; break;
        //////                        default: result = false; break;
        //////                    }

        //////                    if (result) return true;
        //////                    else continue;
        //////                }

        //////                // If not matched above, as a last attempt evaluate whole expression as boolean by replacing variables and checking >0
        //////                try
        //////                {
        //////                    double val = EvaluateArithmetic(p);
        //////                    if (val != 0)
        //////                        cvalue = val;
        //////                    return true;
        //////                }
        //////                catch
        //////                {
        //////                    // ignore
        //////                }
        //////            }

        //////            // none of OR parts returned true
        //////            return false;
        //////        };

        //////        // Main loop — compute each component
        //////        for (int i = 0; i < Componentdetails.Count(); i++)
        //////        {
        //////            var cd = Componentdetails[i];

        //////            PayrollALLComponentCompactViewModel pacvm = new PayrollALLComponentCompactViewModel();

        //////            // Copy metadata
        //////            pacvm.EmpId = (int)empId;
        //////            pacvm.EmpCode = EmpCode;
        //////            pacvm.FirstName = FirstName;
        //////            pacvm.MiddleName = MiddleName;
        //////            pacvm.LastName = LastName;
        //////            pacvm.LoginId = (int)loginId;

        //////            pacvm.PayoutTypeId = cd.PayoutTypeId;
        //////            pacvm.PayoutTypeName = cd.PayoutTypeName;
        //////            pacvm.FrequencyId = 0;
        //////            pacvm.Frequency = cd.Frequency;

        //////            pacvm.SegmentId = cd.SegmentId;
        //////            pacvm.SegmentName = cd.SegmentName;

        //////            pacvm.ComponentId = cd.ComponentId;
        //////            pacvm.ComponentName = cd.ComponentName;
        //////            pacvm.ComponentCode = cd.ComponentCode;

        //////            // Copy logic/condition metadata back into VM so caller has it
        //////            pacvm.LogicId = cd.LogicId;
        //////            pacvm.Percentage = cd.Percentage;
        //////            pacvm.Value = cd.Value;
        //////            pacvm.ComponentId1 = cd.ComponentId1;
        //////            pacvm.ComponentName1 = cd.ComponentName1;
        //////            pacvm.EffectiveFrom = cd.EffectiveFrom;
        //////            pacvm.EffectiveTo = cd.EffectiveTo;
        //////            pacvm.ConditionId = cd.ConditionId;
        //////            pacvm.ConditionExpression = cd.ConditionExpression;
        //////            pacvm.ConditionResultPFESI = cd.ConditionResultPFESI;

        //////            // We compute a numeric value, then format as string
        //////            double computed = 0.0;

        //////            // 1) If Value present (explicit value) -> use it directly
        //////            double valueParsed = 0;
        //////            bool hasValue = cd.Value.HasValue;   // checking decimal?
        //////            if (hasValue)
        //////            {
        //////                valueParsed = Convert.ToDouble(cd.Value.Value);
        //////            }
        //////            //bool hasValue = !string.IsNullOrWhiteSpace(cd.Value) && double.TryParse(cd.Value, out valueParsed);

        //////            // 2) If percentage present -> compute percent of the referenced component
        //////            double percentageParsed = 0;
        //////            bool hasPercentage = cd.Percentage.HasValue;   // checking decimal?
        //////            if (hasPercentage)
        //////            {
        //////                percentageParsed = Convert.ToDouble(cd.Percentage.Value);
        //////            }
        //////            //bool hasPercentage = !string.IsNullOrWhiteSpace(cd.Percentage) && double.TryParse(cd.Percentage, out percentageParsed);

        //////            // Determine operand variable name (ComponentName1 is preferred per your notes)
        //////            string operandName = !string.IsNullOrWhiteSpace(cd.ComponentName1) ? cd.ComponentName1 : cd.ComponentName1;
        //////            // If operandName includes spaces like "MCTC (something)" we take token before space
        //////            if (!string.IsNullOrWhiteSpace(operandName))
        //////                operandName = operandName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

        //////            double operandValue = 0;
        //////            ////if (!string.IsNullOrWhiteSpace(operandName))
        //////            ////{
        //////            ////    if (!computedValues.TryGetValue(operandName, out operandValue))
        //////            ////    {
        //////            ////        // fallback to salaryVars
        //////            ////        salaryVars.TryGetValue(operandName, out operandValue);
        //////            ////    }
        //////            ////}


        //////            if (!string.IsNullOrWhiteSpace(operandName))
        //////            {
        //////                for (int j = 0; j < lstofCompvalue.Count(); j++)
        //////                {
        //////                    if (operandName.ToUpper() == lstofCompvalue[j].ComponentCode.ToUpper())
        //////                    {
        //////                        operandValue = Convert.ToDouble(lstofCompvalue[j].ComponentValue);
        //////                        salaryVars.TryGetValue(operandName, out operandValue);
        //////                    }
        //////                }
        //////            }

        //////            if (hasValue)
        //////            {
        //////                computed = valueParsed;
        //////            }
        //////            else if (hasPercentage)
        //////            {
        //////                computed = (percentageParsed / 100.0) * operandValue;
        //////            }
        //////            ////else
        //////            ////{
        //////            ////    // If neither value nor percentage given, try to evaluate a formula if present in ComponentName1 (like "PF + GI + ESI + Grat")
        //////            ////    // Use ConditionExpression or ComponentName1 as formula candidate
        //////            ////    if (!string.IsNullOrWhiteSpace(cd.ComponentName1))
        //////            ////    {
        //////            ////        try
        //////            ////        {
        //////            ////            computed = EvaluateArithmetic(cd.ComponentName1);
        //////            ////        }
        //////            ////        catch
        //////            ////        {
        //////            ////            computed = 0.0;
        //////            ////        }
        //////            ////    }
        //////            ////    else
        //////            ////    {
        //////            ////        computed = 0.0;
        //////            ////    }
        //////            ////}

        //////            // Now evaluate the ConditionExpression (if any) to verify whether to accept the computed value
        //////            bool condOk = true;
        //////            if (!string.IsNullOrWhiteSpace(cd.ConditionExpression))
        //////            {
        //////                try
        //////                {
        //////                    condOk = EvaluateCondition(cd.ConditionExpression);
        //////                }
        //////                catch
        //////                {
        //////                    condOk = false;
        //////                }
        //////            }

        //////            if (!condOk)
        //////            {
        //////                // Condition failed -> set to 0
        //////                computed = 0.0;
        //////            }
        //////            else
        //////            {
        //////                if (cvalue != 0)
        //////                {
        //////                    computed = cvalue;
        //////                    cvalue = 0;
        //////                }
        //////            }

        //////            if (cd.ComponentCode.ToUpper() == "CTC")
        //////            {
        //////                computed = ctcValue;
        //////            }
        //////            ////if (cd.ComponentCode.ToUpper() == "MCTC")
        //////            ////{
        //////            ////    double mctc = computed;

        //////            ////    double PayableSalary = mctc * ((double)workingdays / (double)totalDays);

        //////            ////    computed = PayableSalary;
        //////            ////}

        //////            ////// Save computed value into maps for other components referencing it
        //////            ////if (!computedValues.ContainsKey(cd.ComponentName))
        //////            ////    computedValues[cd.ComponentName] = computed;
        //////            ////else
        //////            ////    computedValues[cd.ComponentName] = computed; // overwrite latest

        //////            // Also store by ComponentCode key for convenience
        //////            if (!string.IsNullOrWhiteSpace(cd.ComponentCode))
        //////            {
        //////                if (!computedValues.ContainsKey(cd.ComponentCode))
        //////                    computedValues[cd.ComponentCode] = computed;
        //////                else
        //////                    computedValues[cd.ComponentCode] = computed;
        //////            }

        //////            if (!string.IsNullOrWhiteSpace(cd.ComponentCode))
        //////            {
        //////                if (!salaryVars.ContainsKey(cd.ComponentCode))
        //////                    salaryVars[cd.ComponentCode] = computed;
        //////                else
        //////                    salaryVars[cd.ComponentCode] = computed;
        //////            }

        //////            // Format component value for response
        //////            pacvm.ComponentValue = computed.ToString("0.##"); // you can change formatting

        //////            lstofCompvalue.Add(pacvm);
        //////        }

        //////        return lstofCompvalue;
        //////    }
        //////    catch (CustomApiException ex)
        //////    {
        //////        throw new CustomApiException(ex.StatusCode, ex.Message);
        //////    }
        //////}

        //////// ========== ADD THIS METHOD INSIDE YOUR CLASS ==========
        //////private List<PayrollALLComponentCompactViewModel> GetVariablePayComponents(int empId, int year, int month, int loginId, string empCode, string firstName, string middleName, string lastName)
        //////{
        //////    List<PayrollALLComponentCompactViewModel> variableComponents = new List<PayrollALLComponentCompactViewModel>();

        //////    try
        //////    {
        //////        // Get employee salary details to check if variable pay is enabled
        //////        var empSalaryDetail = DB.EmployeeSalaryDetails
        //////            .FirstOrDefault(sal => sal.EmpId == empId &&
        //////                                   sal.IsActive == true &&
        //////                                   sal.IsDeleted == false &&
        //////                                   sal.IsVariable == true);

        //////        if (empSalaryDetail == null || empSalaryDetail.IsVariable != true)
        //////            return variableComponents; // No variable pay for this employee

        //////        // Get active variable pay definitions from PayrollVariable table
        //////        var activeVariables = DB.PayrollVariables
        //////            .Where(v => v.IsActive == true && v.IsDeleted == false && v.Status == true)
        //////            .ToList();

        //////        if (activeVariables == null || !activeVariables.Any())
        //////            return variableComponents;

        //////        // Get variable history for current employee, year and month
        //////        var variableHistory = DB.VariableHistories
        //////            .Where(vh => vh.EmpId == empId &&
        //////                        vh.Year == year &&
        //////                        vh.Month == month &&
        //////                        vh.IsActive == true &&
        //////                        vh.IsDeleted == false)
        //////            .ToDictionary(vh => vh.VariableId, vh => vh);

        //////        // Create component for each active variable
        //////        foreach (var variable in activeVariables)
        //////        {
        //////            PayrollALLComponentCompactViewModel varComponent = new PayrollALLComponentCompactViewModel();

        //////            // Set employee basic info
        //////            varComponent.EmpId = empId;
        //////            varComponent.EmpCode = empCode;
        //////            varComponent.FirstName = firstName;
        //////            varComponent.MiddleName = middleName;
        //////            varComponent.LastName = lastName;
        //////            varComponent.LoginId = loginId;

        //////            // Set component metadata
        //////            varComponent.PayoutTypeId = 0;
        //////            varComponent.PayoutTypeName = "Variable Pay";
        //////            varComponent.FrequencyId = 0;
        //////            varComponent.Frequency = "Monthly";
        //////            varComponent.SegmentId = 999; // High number to ensure it comes last
        //////            varComponent.SegmentName = "Salary Benefits";

        //////            varComponent.ComponentId = variable.VariableId;
        //////            varComponent.ComponentName = variable.VariableName;
        //////            varComponent.ComponentCode = variable.VariableCode;

        //////            // Check if variable exists in history for current month
        //////            if (variableHistory.ContainsKey(variable.VariableId))
        //////            {
        //////                var history = variableHistory[variable.VariableId];

        //////                if (decimal.TryParse(history.VariableAmt?.ToString(), out var value))
        //////                {
        //////                    varComponent.ComponentValue = value.ToString("0.##");
        //////                }
        //////                else
        //////                {
        //////                    varComponent.ComponentValue = "0.00";
        //////                }
        //////            }
        //////            else
        //////            {
        //////                varComponent.ComponentValue = "0.00";
        //////            }

        //////            // Set default values for other properties
        //////            varComponent.LogicId = 0;
        //////            varComponent.Percentage = 0;
        //////            varComponent.Value = 0;
        //////            varComponent.ComponentId1 = 0;
        //////            varComponent.ComponentName1 = "";
        //////            varComponent.EffectiveFrom = null;
        //////            varComponent.EffectiveTo = null;
        //////            varComponent.ConditionId = 0;
        //////            varComponent.ConditionExpression = "";
        //////            varComponent.ConditionResultPFESI = null;
        //////            varComponent.LCtrue = 0;

        //////            variableComponents.Add(varComponent);
        //////        }
        //////    }
        //////    catch (Exception ex)
        //////    {
        //////        // If variable pay fails, return empty list to not break main flow
        //////        return new List<PayrollALLComponentCompactViewModel>();
        //////    }

        //////    return variableComponents;
        //////}
        //Payslip Generation
        public PayslipResponseViewModel EmpPayslipGeneration(PayslipRequestViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                //int? empId = (model.EmpId != 0) ? model.EmpId : 0;
                double cvalue = 0;
                int? payouttypeid = 0;

                int year = Convert.ToInt32(model.Year);
                int month = model.MonthNo;

                decimal? totalDays = DateTime.DaysInMonth(year, month);

                // start & end dates
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

                var lop = (from lev in DB.EmpLeaveApplications
                           where lev.EmpId == loginId
                              && lev.LeaveTypeId == 0
                              && lev.StartDate >= startDate
                              && lev.EndDate <= endDate
                              && lev.IsActive == true
                              && lev.IsDeleted == false
                           orderby lev.StartDate descending
                           select lev).ToList();

                decimal? lopDuration = (from lev in DB.EmpLeaveApplications
                                       where lev.EmpId == loginId
                                          && lev.LeaveTypeId == 0
                                          && lev.StartDate >= startDate
                                          && lev.EndDate <= endDate
                                          && lev.IsActive == true
                                          && lev.IsDeleted == false
                                       select lev.Duration)
                                       .DefaultIfEmpty(0)           // avoid null result
                                       .Sum();

                decimal? workingdays = totalDays - lopDuration;

                if (loginId == null || loginId == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");

                DateTime Today = DateTime.Now;

                var gradedetails = (from emp in DB.EmployeeMasters
                                    join deg in DB.DesignationMasters
                                      on emp.DesignationId equals deg.DesignationId
                                    where emp.EmpId == loginId && emp.EmpStatus.ToUpper() == "ACTIVE"
                                       && emp.IsActive == true && emp.IsDeleted == false
                                       && deg.IsActive == true && deg.IsDeleted == false
                                    orderby deg.DesignationId descending
                                    select deg).FirstOrDefault();

                if (gradedetails == null)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Your designation not found. Please contact the HR team.");
                }

                if (gradedetails.Grade == null)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Grade mapping for your designation is missing. Please contact the HR team to map your designation to the correct grade and try again.");
                }
                else
                {
                    var gradepayout = (from emp in DB.EmployeeMasters
                                       join deg in DB.DesignationMasters
                                         on emp.DesignationId equals deg.DesignationId
                                       join gpo in DB.PayoutMappingMasters
                                          on deg.GradeId equals gpo.GradeId
                                       where emp.EmpId == loginId && emp.EmpStatus.ToUpper() == "ACTIVE"
                                           && emp.IsActive == true && emp.IsDeleted == false
                                           && deg.IsActive == true && deg.IsDeleted == false
                                           && gpo.IsActive == true && gpo.IsDeleted == false
                                       orderby deg.DesignationId descending
                                       select gpo).FirstOrDefault();

                    if (gradepayout == null)
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout mapping for your grade is missing. Please contact the HR team to map the grade to the correct payout and try again.");
                    }

                    payouttypeid = gradepayout.PayoutTypeId;
                }



                // Get employee salary details (primary)
                var empSaldetails = (from emp in DB.EmployeeMasters
                                     join sal in DB.EmployeeSalaryDetails
                                       on emp.EmpCode.ToUpper() equals sal.EmpCode.ToUpper()
                                     where emp.EmpId == loginId && sal.EmpId == loginId && emp.EmpStatus.ToUpper() == "ACTIVE"
                                        && sal.EffectiveFromDate <= Today //&& sal.EffectiveToDate >= Today
                                        && emp.IsActive == true && emp.IsDeleted == false
                                        && sal.IsActive == true && sal.IsDeleted == false
                                     orderby sal.SalaryId descending
                                     select sal).FirstOrDefault();

                if (empSaldetails == null)
                {
                    var empSaldetails1 = (from emp in DB.EmployeeMasters
                                          join sal in DB.EmployeeSalaryDetails
                                            on emp.EmpCode.ToUpper() equals sal.EmpCode.ToUpper()
                                          where emp.EmpId == loginId && sal.EmpId == loginId && emp.EmpStatus.ToUpper() == "ACTIVE"
                                             && emp.IsActive == true && emp.IsDeleted == false
                                             && sal.IsActive == true && sal.IsDeleted == false
                                          orderby sal.SalaryId descending
                                          select sal).FirstOrDefault();

                    if (empSaldetails1 != null)
                        throw new CustomApiException(HttpStatusCode.NotFound, "The effective dates for the employee’s salary details have expired.");
                    else
                        throw new CustomApiException(HttpStatusCode.NotFound, "Salary details (CTC) for your profile were not found. Kindly contact the HR team to update your salary information in the portal.");
                }

                bool? variable = empSaldetails.IsVariable;
                bool? cleararrear = empSaldetails.IsClearArrear;
                //DateTime? effectivedate = empSaldetails.CreatedDate;
                int? effectivemonth = empSaldetails.ArrearMonth ?? 0;
                int? effectiveyear = empSaldetails.ArrearYear ?? 0;

                bool? arrear = empSaldetails.IsArrear;
                double? arrearamt = Convert.ToDouble(empSaldetails.ArrearAmt);

                if (cleararrear == false)
                {
                    if (effectivemonth == month && effectiveyear == year)
                    {
                        arrear = true;
                    }
                    else
                    {
                        arrear = false;
                    }
                }
                else
                {
                    if (effectivemonth == month && effectiveyear == year)
                    {
                        arrear = true;
                    }
                    else
                    {
                        arrear = false;
                    }
                }

                // Helper: try to read numeric properties from empSaldetails into dictionary
                var salaryVars = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

                // Put CTC into map (you used CTC earlier)
                double ctcValue = empSaldetails.CTC != null ? Convert.ToDouble(empSaldetails.CTC) : 0.0;
                salaryVars["CTC"] = ctcValue;

                // Try to fill other known variable names by reflection (if properties exist)
                //var possibleNames = new[] { "MCTC", "GS", "BS", "HRA", "Con", "PF", "GI", "ESI", "Grat", "SB", "TD", "PT" };
                var possibleNames = new[] { "MCTC", "BS", "HRA", "Con", "ESIB", "PFB", "GI", "Grat", "SB", "GS", "PFB", "ESIB", "PT", "TD", "IA", "NS" };

                foreach (var name in possibleNames)
                {
                    var prop = empSaldetails.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (prop != null)
                    {
                        object val = prop.GetValue(empSaldetails, null);
                        double d = 0;
                        if (val != null && double.TryParse(val.ToString(), out d))
                            salaryVars[name] = d;
                        else
                            salaryVars[name] = 0.0;
                    }
                    else
                    {
                        if (!salaryVars.ContainsKey(name))
                            salaryVars[name] = 0.0;
                    }
                }

                // If MCTC not present, attempt derive from CTC (you can change logic as per your actual rules)
                if (!salaryVars.ContainsKey("MCTC") || salaryVars["MCTC"] == 0)
                    salaryVars["MCTC"] = ctcValue; // fallback - adjust as necessary

                // Get emp basic meta
                var empdetails = (from emp in DB.EmployeeMasters
                                  where emp.EmpId == loginId && emp.EmpStatus.ToUpper() == "ACTIVE"
                                     && emp.IsActive == true && emp.IsDeleted == false
                                  orderby emp.EmpId descending
                                  select emp).FirstOrDefault();

                string EmpCode = empdetails.EmpCode;
                string FirstName = empdetails.FirstName;
                string MiddleName = empdetails.MiddleName;
                string LastName = empdetails.LastName;

                // Read components and related logic & condition as before
                var Componentdetails = (from com in DB.PayrollComponents
                                        join cal in DB.PayrollComponentLogics on com.ComponentId equals cal.ComponentId
                                        join con in DB.PayrollComponentConditions on com.ComponentId equals con.ComponentId
                                        join pay in DB.PayrollPayoutTypes on com.PayoutTypeId equals pay.PayoutTypeId
                                        join seg in DB.PayrollSegments on com.SegmentId equals seg.SegmentId
                                        where com.IsActive == true && com.IsDeleted == false
                                        && cal.IsActive == true && cal.IsDeleted == false
                                        && con.IsActive == true && con.IsDeleted == false
                                        && pay.IsActive == true && pay.IsDeleted == false
                                        && seg.IsActive == true && seg.IsDeleted == false
                                        && pay.PayoutTypeId == payouttypeid
                                        orderby pay.PayoutTypeId ascending, seg.SegmentId ascending
                                        select new PayrollALLComponentCompactViewModel
                                        {
                                            PayoutTypeId = pay.PayoutTypeId,
                                            PayoutTypeName = pay.PayoutTypeName,
                                            FrequencyId = 0,
                                            Frequency = pay.Frequency,
                                            SegmentId = seg.SegmentId,
                                            SegmentName = seg.SegmentName,
                                            ComponentId = com.ComponentId,
                                            ComponentName = com.ComponentName,
                                            ComponentCode = com.ComponentCode,
                                            ComponentValue = "0.00",
                                            LogicId = cal.LogicId,
                                            Percentage = cal.Percentage,
                                            Value = cal.Value,
                                            ComponentId1 = cal.ComponentId1,
                                            ComponentName1 = cal.ComponentName1,
                                            EffectiveFrom = cal.EffectiveFrom,
                                            EffectiveTo = cal.EffectiveTo,
                                            ConditionId = con.ConditionId,
                                            ConditionExpression = con.ConditionExpression,
                                            ConditionResultPFESI = con.ConditionResultPFESI,
                                        }).ToList();

                if (Componentdetails == null || Componentdetails.Count == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "Pay slip Component details are not found");

                // Result list
                List<PayrollALLComponentCompactViewModel> lstofCompvalue = new List<PayrollALLComponentCompactViewModel>();

                // Keep a map of computed component values by name (so other components can reference them)
                var computedValues = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);

                // Seed computedValues with salaryVars
                foreach (var kv in salaryVars)
                    if (!computedValues.ContainsKey(kv.Key))
                        computedValues[kv.Key] = kv.Value;

                // Helper: evaluate arithmetic expression (with variables replaced)
                Func<string, double> EvaluateArithmetic = expr =>
                {
                    if (string.IsNullOrWhiteSpace(expr)) return 0.0;
                    // Replace any multiple spaces with single space
                    expr = Regex.Replace(expr, @"\s+", " ").Trim();

                    // Replace variable tokens with numeric values from computedValues or salaryVars
                    // Tokenize by words matching letters/numbers/underscore
                    var varPattern = new Regex(@"\b[A-Za-z_][A-Za-z0-9_]*\b");
                    string replaced = varPattern.Replace(expr, match =>
                    {
                        string token = match.Value;
                        double v = 0.0;
                        if (computedValues.TryGetValue(token, out v))
                            return v.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        if (salaryVars.TryGetValue(token, out v))
                            return v.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        // else treat as 0
                        return "0";
                    });

                    // Validate: allow only digits, decimal, operators, parentheses, spaces and minus
                    if (!Regex.IsMatch(replaced, @"^[0-9\.\-\+\*\/\(\)\s]+$"))
                        throw new Exception("Invalid characters in expression after substitution.");

                    // Use DataTable.Compute to evaluate arithmetic
                    try
                    {
                        var dt = new DataTable();
                        var valObj = dt.Compute(replaced, "");
                        double val = 0.0;
                        double.TryParse(Convert.ToString(valObj), out val);
                        return val;
                    }
                    catch
                    {
                        return 0.0;
                    }
                };

                // Helper: evaluate condition expression. Supports:
                // - single comparisons (A > 10)
                // - range (10 <= A <= 20)
                // - OR separated subconditions using "OR" or "(OR)"
                Func<string, bool> EvaluateCondition = condExpr =>
                {
                    if (string.IsNullOrWhiteSpace(condExpr)) return true; // no condition => pass

                    // Normalize OR tokens
                    condExpr = condExpr.Replace("(OR)", " OR ").Replace("(or)", " OR ").Replace("||", " OR ");
                    // Split on OR (top-level)
                    var orParts = Regex.Split(condExpr, @"\s+OR\s+", RegexOptions.IgnoreCase).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                    foreach (var part in orParts)
                    {
                        string p = part.Trim();

                        // Range form? detect "a <= VAR <= b" or "a <= VAR" style
                        // Pattern: number <op> VAR <op> number  (e.g., "15000 <= MCTC <= 21000")
                        var rangeMatch = Regex.Match(p, @"^\s*(?<left>[-\d\.]+)\s*(<=|<)\s*(?<var>[A-Za-z_][A-Za-z0-9_]*)\s*(<=|<)\s*(?<right>[-\d\.]+)\s*$");
                        if (rangeMatch.Success)
                        {
                            double left = Convert.ToDouble(rangeMatch.Groups["left"].Value);
                            string varName = rangeMatch.Groups["var"].Value;
                            double right = Convert.ToDouble(rangeMatch.Groups["right"].Value);
                            double varVal = 0;
                            if (!computedValues.TryGetValue(varName, out varVal)) computedValues.TryGetValue(varName, out varVal);
                            if (varVal >= left && varVal <= right) return true;
                            else continue;
                        }

                        // Alternative range form "VAR >= x AND VAR <= y" or "x <= VAR <= y" handled above partially
                        // Try generic comparison: leftOperator right (like MCTC > 26000 or 26000 <= MCTC)
                        var compMatch = Regex.Match(p, @"^\s*(?<left>[A-Za-z0-9\.\-\+\s\(\)]+)\s*(?<op>>=|<=|>|<|==|=|!=)\s*(?<right>[A-Za-z0-9\.\-\+\s\(\)]+)\s*$");
                        if (compMatch.Success)
                        {
                            string leftToken = compMatch.Groups["left"].Value.Trim();
                            string op = compMatch.Groups["op"].Value.Trim();
                            string rightToken = compMatch.Groups["right"].Value.Trim();

                            // Determine numeric values for left and right (either variable or arithmetic)
                            double leftVal = 0, rightVal = 0;
                            // If leftToken is a variable or expression
                            leftVal = EvaluateArithmetic(leftToken);
                            rightVal = EvaluateArithmetic(rightToken);

                            bool result = false;
                            switch (op)
                            {
                                case ">": result = leftVal > rightVal; break;
                                case "<": result = leftVal < rightVal; break;
                                case ">=": result = leftVal >= rightVal; break;
                                case "<=": result = leftVal <= rightVal; break;
                                case "==":
                                case "=": result = Math.Abs(leftVal - rightVal) < 0.000001; break;
                                case "!=": result = Math.Abs(leftVal - rightVal) > 0.000001; break;
                                default: result = false; break;
                            }

                            if (result) return true;
                            else continue;
                        }

                        // If not matched above, as a last attempt evaluate whole expression as boolean by replacing variables and checking >0
                        try
                        {
                            double val = EvaluateArithmetic(p);
                            if (val != 0)
                                cvalue = val;
                            return true;
                        }
                        catch
                        {
                            // ignore
                        }
                    }

                    // none of OR parts returned true
                    return false;
                };

                // Main loop — compute each component
                for (int i = 0; i < Componentdetails.Count(); i++)
                {
                    var cd = Componentdetails[i];

                    PayrollALLComponentCompactViewModel pacvm = new PayrollALLComponentCompactViewModel();

                    // Copy metadata
                    pacvm.EmpId = (int)loginId;
                    pacvm.EmpCode = EmpCode;
                    pacvm.FirstName = FirstName;
                    pacvm.MiddleName = MiddleName;
                    pacvm.LastName = LastName;
                    pacvm.LoginId = (int)loginId;

                    pacvm.PayoutTypeId = cd.PayoutTypeId;
                    pacvm.PayoutTypeName = cd.PayoutTypeName;
                    pacvm.FrequencyId = 0;
                    pacvm.Frequency = cd.Frequency;

                    pacvm.SegmentId = cd.SegmentId;
                    pacvm.SegmentName = cd.SegmentName;

                    pacvm.ComponentId = cd.ComponentId;
                    pacvm.ComponentName = cd.ComponentName;
                    pacvm.ComponentCode = cd.ComponentCode;

                    // Copy logic/condition metadata back into VM so caller has it
                    pacvm.LogicId = cd.LogicId;
                    pacvm.Percentage = cd.Percentage;
                    pacvm.Value = cd.Value;
                    pacvm.ComponentId1 = cd.ComponentId1;
                    pacvm.ComponentName1 = cd.ComponentName1;
                    pacvm.EffectiveFrom = cd.EffectiveFrom;
                    pacvm.EffectiveTo = cd.EffectiveTo;
                    pacvm.ConditionId = cd.ConditionId;
                    pacvm.ConditionExpression = cd.ConditionExpression;
                    pacvm.ConditionResultPFESI = cd.ConditionResultPFESI;

                    // We compute a numeric value, then format as string
                    double computed = 0.0;

                    // 1) If Value present (explicit value) -> use it directly
                    double valueParsed = 0;
                    bool hasValue = cd.Value.HasValue;   // checking decimal?
                    if (hasValue)
                    {
                        valueParsed = Convert.ToDouble(cd.Value.Value);
                    }
                    //bool hasValue = !string.IsNullOrWhiteSpace(cd.Value) && double.TryParse(cd.Value, out valueParsed);

                    // 2) If percentage present -> compute percent of the referenced component
                    double percentageParsed = 0;
                    bool hasPercentage = cd.Percentage.HasValue;   // checking decimal?
                    if (hasPercentage)
                    {
                        percentageParsed = Convert.ToDouble(cd.Percentage.Value);
                    }
                    //bool hasPercentage = !string.IsNullOrWhiteSpace(cd.Percentage) && double.TryParse(cd.Percentage, out percentageParsed);

                    // Determine operand variable name (ComponentName1 is preferred per your notes)
                    string operandName = !string.IsNullOrWhiteSpace(cd.ComponentName1) ? cd.ComponentName1 : cd.ComponentName1;
                    // If operandName includes spaces like "MCTC (something)" we take token before space
                    if (!string.IsNullOrWhiteSpace(operandName))
                        operandName = operandName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

                    double operandValue = 0;
                    ////if (!string.IsNullOrWhiteSpace(operandName))
                    ////{
                    ////    if (!computedValues.TryGetValue(operandName, out operandValue))
                    ////    {
                    ////        // fallback to salaryVars
                    ////        salaryVars.TryGetValue(operandName, out operandValue);
                    ////    }
                    ////}


                    if (!string.IsNullOrWhiteSpace(operandName))
                    {
                        for (int j = 0; j < lstofCompvalue.Count(); j++)
                        {
                            if (operandName.ToUpper() == lstofCompvalue[j].ComponentCode.ToUpper())
                            {
                                operandValue = Convert.ToDouble(lstofCompvalue[j].ComponentValue);
                                salaryVars.TryGetValue(operandName, out operandValue);
                            }
                        }
                    }

                    if (hasValue)
                    {
                        computed = valueParsed;
                    }
                    else if (hasPercentage)
                    {
                        computed = (percentageParsed / 100.0) * operandValue;
                    }
                    ////else
                    ////{
                    ////    // If neither value nor percentage given, try to evaluate a formula if present in ComponentName1 (like "PF + GI + ESI + Grat")
                    ////    // Use ConditionExpression or ComponentName1 as formula candidate
                    ////    if (!string.IsNullOrWhiteSpace(cd.ComponentName1))
                    ////    {
                    ////        try
                    ////        {
                    ////            computed = EvaluateArithmetic(cd.ComponentName1);
                    ////        }
                    ////        catch
                    ////        {
                    ////            computed = 0.0;
                    ////        }
                    ////    }
                    ////    else
                    ////    {
                    ////        computed = 0.0;
                    ////    }
                    ////}

                    // Now evaluate the ConditionExpression (if any) to verify whether to accept the computed value
                    bool condOk = true;
                    if (!string.IsNullOrWhiteSpace(cd.ConditionExpression))
                    {
                        try
                        {
                            condOk = EvaluateCondition(cd.ConditionExpression);
                        }
                        catch
                        {
                            condOk = false;
                        }
                    }

                    if (!condOk)
                    {
                        // Condition failed -> set to 0
                        computed = 0.0;
                    }
                    else
                    {
                        if (cvalue != 0)
                        {
                            computed = cvalue;
                            cvalue = 0;
                        }
                    }

                    if (cd.ComponentCode.ToUpper() == "CTC")
                    {
                        computed = ctcValue;
                    }

                    if (cd.ComponentCode.ToUpper() == "MCTC")
                    {
                        double mctc = computed;

                        double PayableSalary = mctc * ((double)workingdays / (double)totalDays);

                        computed = PayableSalary;
                    }

                    ////// Save computed value into maps for other components referencing it
                    ////if (!computedValues.ContainsKey(cd.ComponentName))
                    ////    computedValues[cd.ComponentName] = computed;
                    ////else
                    ////    computedValues[cd.ComponentName] = computed; // overwrite latest

                    // Also store by ComponentCode key for convenience
                    if (!string.IsNullOrWhiteSpace(cd.ComponentCode))
                    {
                        if (!computedValues.ContainsKey(cd.ComponentCode))
                            computedValues[cd.ComponentCode] = computed;
                        else
                            computedValues[cd.ComponentCode] = computed;
                    }

                    if (!string.IsNullOrWhiteSpace(cd.ComponentCode))
                    {
                        if (!salaryVars.ContainsKey(cd.ComponentCode))
                            salaryVars[cd.ComponentCode] = computed;
                        else
                            salaryVars[cd.ComponentCode] = computed;
                    }

                    // Format component value for response
                    pacvm.ComponentValue = computed.ToString("0.##"); // you can change formatting

                    lstofCompvalue.Add(pacvm);
                }

                // Declare the variable outside the if block with a default value
                List<OfficeConnect_Web.ViewModel.PayrollALLComponentCompactViewModel> variablePayComponents = null;

                if (variable == true)
                {
                    var ESIdata = lstofCompvalue.Where(x => x.ComponentCode == "ESIB").FirstOrDefault();


                    int? varpayouttypeId = ESIdata.PayoutTypeId;
                    string varpayouttypeName = ESIdata.PayoutTypeName;
                    int? varsegmentId = ESIdata.SegmentId;
                    string varsegmentName = ESIdata.SegmentName;

                    // ========== START: VARIABLE PAY LOGIC ==========
                    // Get Variable Pay components
                    variablePayComponents = GetVariablePayComponents(
                        (int)loginId,
                        year,
                        month,
                        (int)loginId,
                        EmpCode,
                        FirstName,
                        MiddleName,
                        LastName,
                        (int)varpayouttypeId,
                        varpayouttypeName,
                        (int)varsegmentId,
                        varsegmentName
                    );

                    ////// Merge variable pay components with regular components
                    ////if (variablePayComponents != null && variablePayComponents.Any())
                    ////{
                    ////    // Add variable pay components to the main list
                    ////    lstofCompvalue.AddRange(variablePayComponents);

                    ////    ////// Reorder to ensure lstofCompvalue pay components appear last within "Salary Benefits" segment
                    ////    ////lstofCompvalue = Componentdetails
                    ////    ////    .OrderBy(c => c.SegmentName == "Salary Benefits" ? 1 : 0) // Non-Salary Benefits first
                    ////    ////    .ThenBy(c => c.SegmentId) // Then by original segment order
                    ////    ////    .ThenBy(c => c.ComponentName) // Then alphabetically within segment
                    ////    ////    .ToList();
                    ////}
                    // ========== END: VARIABLE PAY LOGIC ==========

                }

                // Declare the variable outside the if block with a default value
                List<OfficeConnect_Web.ViewModel.PayrollALLComponentCompactViewModel> arrearComponents = null;

                if (arrear == true)
                {
                    // Get MCT value from ArrearAmt
                    arrearamt = empSaldetails.ArrearAmt != null ? Convert.ToDouble(empSaldetails.ArrearAmt) : 0;

                    // Calculate arrear components using existing database logic
                   arrearComponents = CalculateArrearComponents(
                        (int)loginId,
                        year,
                        month,
                        (int)loginId,
                        EmpCode,
                        FirstName,
                        MiddleName,
                        LastName,
                        arrearamt,
                        payouttypeid ?? 0
                    );
                }

                //return lstofCompvalue;

                ////var payslip = DB.PayslipSections
                ////                .Where(s => s.IsActive == true && s.IsDeleted == false)
                ////                .Select(section => new SectionResponseViewModel
                ////                {
                ////                    SectionId = section.SectionId,
                ////                    SectionName = section.SectionName,

                ////                    Components =
                ////                        (from sec in DB.PayslipSectionComponents
                ////                         join comp in lstofCompvalue
                ////                             on sec.ComponentId equals comp.ComponentId into compJoin
                ////                         from comp in compJoin.DefaultIfEmpty()

                ////                         where sec.PayoutTypeId == 1
                ////                               && sec.SectionId == section.SectionId
                ////                               && sec.IsActive == true
                ////                               && sec.IsDeleted == false
                ////                         // && (comp == null || (comp.IsActive == true && comp.IsDeleted == false))

                ////                         select new SalaryComponentViewModel
                ////                         {
                ////                             SectionComponentId = sec.SectionComponentId,
                ////                             ComponentId = sec.ComponentId.HasValue ? sec.ComponentId.Value : 0,
                ////                             ComponentName = comp != null ? comp.ComponentName : null,
                ////                             ComponentCode = comp != null ? comp.ComponentCode : null,
                ////                             SequenceNo = sec.SequenceNo.HasValue ? sec.SequenceNo.Value : 0
                ////                         })
                ////                        .OrderBy(c => c.SequenceNo)
                ////                        .ToList()
                ////                })
                ////                .OrderBy(s => s.SectionId)
                ////                .ToList();

                ////// Get emp basic meta
                ////var empdetails = (from emp in DB.EmployeeMasters
                ////                  where emp.EmpId == loginId && emp.EmpStatus.ToUpper() == "ACTIVE"
                ////                     && emp.IsActive == true && emp.IsDeleted == false
                ////                  orderby emp.EmpId descending
                ////                  select emp).FirstOrDefault();
                ///

                var sections = DB.PayslipSections
                                .Where(s => s.IsActive == true && s.IsDeleted == false)
                                .ToList();

                var sectionComponents = DB.PayslipSectionComponents
                                        .Where(x => x.PayoutTypeId == payouttypeid && x.IsActive == true && x.IsDeleted == false)
                                        .ToList();

                var payslip = sections
                                .Select(section => new SectionResponseViewModel
                                {
                                    SectionId = section.SectionId,
                                    SectionName = section.SectionName,

                                    Components = sectionComponents
                                        .Where(sec => sec.SectionId == section.SectionId
                                                   && sec.PayoutTypeId == payouttypeid)
                                        .Select(sec =>
                                        {
                                            var comp = lstofCompvalue
                                                .FirstOrDefault(c => c.ComponentId == sec.ComponentId);

                                            return new SalaryComponentViewModel
                                            {
                                                SectionComponentId = sec.SectionComponentId,
                                                ComponentId = sec.ComponentId ?? 0,
                                                ComponentName = comp?.ComponentName,
                                                ComponentCode = comp?.ComponentCode,
                                                SequenceNo = sec.SequenceNo ?? 0,
                                                ComponentValue = comp?.ComponentValue
                                            };
                                        })
                                        .OrderBy(c => c.SequenceNo)
                                        .ToList()
                                })
                                .OrderBy(s => s.SectionId)
                                .ToList();

                var ArrearSec = new List<SectionResponseViewModel>
                                {
                                    new SectionResponseViewModel
                                    {
                                        SectionId = 0,
                                        SectionName = "Arrear Adjustment",
                                        Components = arrearComponents
                                            .Where(comp => sectionComponents.Any(sec => sec.ComponentId == comp.ComponentId
                                                                                        && sec.PayoutTypeId == payouttypeid))
                                            .Select(comp =>
                                            {
                                                var sec = sectionComponents.FirstOrDefault(s => s.ComponentId == comp.ComponentId
                                                                                                && s.PayoutTypeId == payouttypeid);
                                                return new SalaryComponentViewModel
                                                {
                                                    SectionComponentId = sec?.SectionComponentId ?? 0,
                                                    ComponentId = comp.ComponentId,
                                                    ComponentName = comp.ComponentName,
                                                    ComponentCode = comp.ComponentCode,
                                                    SequenceNo = sec?.SequenceNo ?? 0,
                                                    ComponentValue = comp.ComponentValue
                                                };
                                            })
                                            .OrderBy(c => c.SequenceNo)
                                            .ToList()
                                    }
                                };

                // Get Company basic meta
                var compdetails = (from comp in DB.CompanyMasters
                                   join le in DB.LegalEntityMasters on comp.CompId equals le.CompId
                                   //join bu in DB.BusinessUnitMasters on comp.CompId equals bu.CompId
                                   join loc in DB.LocationMasters on comp.CompId equals loc.CompId
                                   where comp.CompId == empdetails.CompId && loc.LocationId == empdetails.LocationId
                                   && le.LEId == loc.LEId
                                      //&& bu.BUId == empdetails.BUId && le.LEId == empdetails.LEId
                                      && comp.IsActive == true && comp.IsDeleted == false
                                      && le.IsActive == true && le.IsDeleted == false
                                      //&& bu.IsActive == true && bu.IsDeleted == false
                                      && loc.IsActive == true && loc.IsDeleted == false
                                   orderby loc.LocationId descending
                                   select new CompanyInfoViewModel
                                   {
                                       CompanyName = le.LegalEntity,
                                       CompanyAddress = loc.Address + ", " + loc.City + ", " + loc.State + ", " + loc.Country + ", Postal Code - " + loc.PostalCode,
                                       CompanyPhoneNo = "+91 90350 60961",  //Need to add location master
                                       CompanyFax = "",
                                       CompanyEmail = "india@3dcad-global.com",
                                   }).FirstOrDefault();

                if (compdetails == null)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Company details are missing. Kindly reach out to the HR team for assistance.");
                }

                string salarymonth = model.Month + " - " + model.Year;

                // Get EMP ACC basic meta
                var empinfordetails = (from emp in DB.EmployeeMasters
                                            join acc in DB.EmployeeAccDetails on emp.EmpId equals acc.EmpId
                                            where emp.EmpId == loginId
                                                  && emp.EmpStatus.ToUpper() == "ACTIVE"
                                                  && emp.IsActive == true
                                                  && emp.IsDeleted == false
                                                  && acc.IsActive == true
                                                  && acc.IsDeleted == false
                                            orderby emp.EmpId descending
                                            select new EmployeeInfoDetailsViewModel
                                            {
                                                Name = emp.FirstName + " " + (emp.MiddleName ?? "") + " " + emp.LastName,
                                                Designation = emp.DesignationName,
                                                EmpCode = emp.EmpCode,

                                                // Location handling
                                                Location = emp.LocationId == 0
                                                            ? "Bangalore"
                                                            : DB.LocationMasters
                                                                .Where(x => x.LocationId == emp.LocationId
                                                                        && x.IsActive == true
                                                                        && x.IsDeleted == false)
                                                                .Select(x => x.Location)
                                                                .FirstOrDefault(),

                                                BankName = acc.BankName,
                                                BranchName = acc.BranchName,
                                                IFSCCode = acc.IFSCCode,
                                                BankAccNo = acc.AccNo,
                                                PanNo = acc.PANNo,
                                                PFNo = acc.PFNo,
                                                DaysPaid = workingdays ?? 0,
                                                UANNo = acc.UANNo,
                                                LOP = lopDuration ?? 0,
                                                ESINo = acc.ESIInsuranceNo
                                            }
                                        ).FirstOrDefault();

                if (empinfordetails == null)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Employee Account details are missing. Kindly reach out to the HR team for assistance.");
                }

                PayslipResponseViewModel resmodel = new PayslipResponseViewModel();
                resmodel.Company = compdetails;
                resmodel.SalaryMonth = salarymonth;
                resmodel.EmployeeDetails = empinfordetails;
                resmodel.PayslipSections = payslip;
                resmodel.VariableSections = variablePayComponents;
                resmodel.ArrearSections = ArrearSec;
                resmodel.DescriptionforArrear = empSaldetails.DescriptionforArrear;

                return resmodel;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
            ////catch (Exception ex) 
            ////{
            ////    // Log and wrap
            ////    logger.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            ////    throw new CustomApiException(HttpStatusCode.InternalServerError, "Error while calculating components: " + ex.Message);
            ////}
        }
        public List<DDPayslipSectionViewModel> DDPayslipSection(PayslipSectionViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var comdetails = (from pay in DB.PayslipSections
                                  where pay.IsActive == true && pay.IsDeleted == false
                                  select pay).OrderBy(x => x.SectionId).ToList();

                if (loginId != 0)
                {
                    if (comdetails != null)
                    {
                        List<DDPayslipSectionViewModel> lstofpaytype = new List<DDPayslipSectionViewModel>();

                        for (int i = 0; i < comdetails.Count(); i++)
                        {
                            DDPayslipSectionViewModel ltvm = new DDPayslipSectionViewModel();
                            ltvm.SectionId = comdetails[i].SectionId;
                            ltvm.SectionName = comdetails[i].SectionName; 
                            lstofpaytype.Add(ltvm);

                        }
                        return lstofpaytype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Section Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<PayslipSectionViewModel> GetAllPayslipSection(PayslipSectionViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var Paydetails = (from pay in DB.PayslipSections
                                  where pay.IsActive == false && pay.IsDeleted == false
                                  select pay).OrderByDescending(x => x.SectionId).ToList();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        List<PayslipSectionViewModel> lstofpaytype = new List<PayslipSectionViewModel>();

                        for (int i = 0; i < Paydetails.Count(); i++)
                        {
                            PayslipSectionViewModel ltvm = new PayslipSectionViewModel();
                            ltvm.SectionId = Paydetails[i].SectionId;
                            ltvm.SectionName = Paydetails[i].SectionName;
                            ltvm.SequenceNo = Paydetails[i].SequenceNo;
                            ltvm.CreatedBy = Paydetails[i].CreatedBy;
                            ltvm.CreatedDate = Paydetails[i].CreatedDate;
                            ltvm.LastUpdatedBy = Paydetails[i].LastUpdatedBy;
                            ltvm.LastUpdatedDate = Paydetails[i].LastUpdatedDate;
                            ltvm.IsActive = Paydetails[i].IsActive;
                            ltvm.IsUpdated = Paydetails[i].IsUpdated;
                            ltvm.IsDeleted = Paydetails[i].IsDeleted;
                            lstofpaytype.Add(ltvm);

                        }
                        return lstofpaytype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Section Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayslipSectionViewModel GetPayslipSection(PayslipSectionViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Paydetails = (from pay in DB.PayslipSections
                                  where pay.SectionId == model.SectionId && pay.IsActive == false && pay.IsDeleted == false
                                  select pay).OrderByDescending(x => x.SectionId).FirstOrDefault();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        PayslipSectionViewModel ltvm = new PayslipSectionViewModel();
                        ltvm.SectionId = Paydetails.SectionId;
                        ltvm.SectionName = Paydetails.SectionName;
                        ltvm.SequenceNo = Paydetails.SequenceNo;
                        ltvm.CreatedBy = Paydetails.CreatedBy;
                        ltvm.CreatedDate = Paydetails.CreatedDate;
                        ltvm.LastUpdatedBy = Paydetails.LastUpdatedBy;
                        ltvm.LastUpdatedDate = Paydetails.LastUpdatedDate;
                        ltvm.IsActive = Paydetails.IsActive;
                        ltvm.IsUpdated = Paydetails.IsUpdated;
                        ltvm.IsDeleted = Paydetails.IsDeleted;
                        return ltvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Section Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel AddPayslipSection(PayslipSectionViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                //int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Paydetails = (from pay in DB.PayslipSections
                                  where pay.SectionName == model.SectionName
                                  && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).ToList();

                if (loginId != 0)
                {
                    if (Paydetails.Count() == 0)
                    {
                        PayslipSection ltm = new PayslipSection();
                        //em.EmpId = model.modelId;
                        ltm.SectionName = model.SectionName;
                        ltm.SequenceNo = 0;
                        ltm.IsActive = true;
                        ltm.IsUpdated = false;
                        ltm.IsDeleted = false;
                        ltm.CreatedBy = model.LoginId;
                        ltm.CreatedDate = DateTime.Now;
                        ltm.LastUpdatedBy = model.LoginId;
                        ltm.LastUpdatedDate = DateTime.Now;
                        DB.PayslipSections.Add(ltm);
                        DB.SaveChanges();

                        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Added";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Section Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel UpdatePayslipSection(PayslipSectionViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                //int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.SectionId != 0) ? model.SectionId : 0;

                var Paydetails = (from acc in DB.PayslipSections
                                  where acc.SectionId == id && acc.IsActive == true && acc.IsDeleted == false
                                  select acc).FirstOrDefault();

                if (loginId != 0)
                {
                    if (id != 0)
                    {
                        if (Paydetails != null)
                        {
                            Paydetails.SectionName = model.SectionName;
                            Paydetails.SequenceNo = model.SequenceNo;
                            Paydetails.IsActive = true;
                            Paydetails.IsUpdated = true;
                            Paydetails.IsDeleted = false;
                            Paydetails.LastUpdatedBy = model.LoginId;
                            Paydetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();

                            PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                            emvm.Status = 200;
                            emvm.msg = "Updated";

                            return emvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Section Details Not Found");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Section Id is Mismatching");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel DeletePayslipSection(PayslipSectionViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                //int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.SectionId != 0) ? model.SectionId : 0;

                var Paydetails = (from pay in DB.PayslipSections
                                  where pay.SectionId == id && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).FirstOrDefault();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        Paydetails.IsActive = true;
                        Paydetails.IsUpdated = true;
                        Paydetails.IsDeleted = true;
                        Paydetails.LastUpdatedBy = model.LoginId;
                        Paydetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Deleted";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Section Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<PayoutRequestViewModel> GetAllPayslipSectionComponent(PayslipSectionComponentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var result = DB.PayrollPayoutTypes
                            .Where(p => p.IsActive == true && p.IsDeleted == false)
                            .Select(payout => new PayoutRequestViewModel
                            {
                                PayoutTypeId = payout.PayoutTypeId,
                                PayoutTypeName = payout.PayoutTypeName,

                                Sections = DB.PayslipSections
                                    .Where(s => s.IsActive == true && s.IsDeleted == false)
                                    .Select(section => new SectionRequestViewModel
                                    {
                                        SectionId = section.SectionId,
                                        SectionName = section.SectionName,

                                        Components =
                                            (from sec in DB.PayslipSectionComponents
                                             join comp in DB.PayrollComponents
                                                on sec.ComponentId equals comp.ComponentId into compJoin
                                             from comp in compJoin.DefaultIfEmpty()

                                             where sec.PayoutTypeId == payout.PayoutTypeId
                                                   && sec.SectionId == section.SectionId
                                                   && sec.IsActive == true && sec.IsDeleted == false
                                                   && (comp == null || (comp.IsActive == true && comp.IsDeleted == false))

                                             select new ComponentRequestViewModel
                                             {
                                                 SectionComponentId = sec.SectionComponentId,
                                                 ComponentId = sec.ComponentId,
                                                 ComponentName = comp != null ? comp.ComponentName : null,
                                                 ComponentCode = comp != null ? comp.ComponentCode : null,
                                                 SequenceNo = sec.SequenceNo,
                                                 EffectiveFrom = sec.EffectiveFrom,
                                                 EffectiveTo = sec.EffectiveTo,
                                                 RecordStatus = sec.RecordStatus
                                             })
                                             .OrderBy(c => c.SequenceNo)
                                             .ToList()
                                    })
                                    .OrderBy(s => s.SectionId)
                                    .ToList()
                            })
                            .OrderBy(p => p.PayoutTypeId)
                            .ToList();

                if (loginId != 0)
                {
                    
                        return result;
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<PayoutRequestViewModel> GetPayslipSectionComponent(PayslipSectionComponentViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                //int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? payoutId = (model.PayoutTypeId != 0) ? model.PayoutTypeId : 0;

                var result = DB.PayrollPayoutTypes
                            .Where(p => p.IsActive == true && p.IsDeleted == false && p.PayoutTypeId == payoutId)
                            .Select(payout => new PayoutRequestViewModel
                            {
                                PayoutTypeId = payout.PayoutTypeId,
                                PayoutTypeName = payout.PayoutTypeName,

                                Sections = DB.PayslipSections
                                    .Where(s => s.IsActive == true && s.IsDeleted == false)
                                    .Select(section => new SectionRequestViewModel
                                    {
                                        SectionId = section.SectionId,
                                        SectionName = section.SectionName,

                                        Components =
                                            (from sec in DB.PayslipSectionComponents
                                             join comp in DB.PayrollComponents
                                                on sec.ComponentId equals comp.ComponentId into compJoin
                                             from comp in compJoin.DefaultIfEmpty()

                                             where sec.PayoutTypeId == payout.PayoutTypeId
                                                   && sec.SectionId == section.SectionId
                                                   //&& sec.EffectiveFrom <= model.EffectiveFrom
                                                   //&& sec.EffectiveTo >= model.EffectiveTo
                                                   && sec.IsActive == true && sec.IsDeleted == false
                                                   && (comp == null || (comp.IsActive == true && comp.IsDeleted == false))

                                             select new ComponentRequestViewModel
                                             {
                                                 SectionComponentId = sec.SectionComponentId,
                                                 ComponentId = sec.ComponentId,
                                                 ComponentName = comp != null ? comp.ComponentName : null,
                                                 ComponentCode = comp != null ? comp.ComponentCode : null,
                                                 SequenceNo = sec.SequenceNo,
                                                 EffectiveFrom = sec.EffectiveFrom,
                                                 EffectiveTo = sec.EffectiveTo,
                                                 RecordStatus = sec.RecordStatus
                                             })
                                             .OrderBy(c => c.SequenceNo)
                                             .ToList()
                                    })
                                    .OrderBy(s => s.SectionId)
                                    .ToList()
                            })
                            .OrderBy(p => p.PayoutTypeId)
                            .ToList();

                if (loginId != 0)
                {

                    return result;
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        ////public PayrollResponseViewModel AddPayslipSectionComponent(List<PayslipSectionComponentViewModel> model)
        ////{
        ////    try
        ////    {
        ////        for (int i = 0; i < model.Count(); i++)
        ////        {
        ////            string msg = "";
        ////            int? loginId = (model[i].LoginId != 0) ? model[i].LoginId : 0;
        ////            //int? EmpId = (model[i].EmpId != 0) ? model[i].EmpId : 0;
        ////            int? payoutId = (model[i].PayoutTypeId != 0) ? model[i].PayoutTypeId : 0;

        ////            var Paydetails = (from psc in DB.PayslipSectionComponents
        ////                              join pay in DB.PayrollPayoutTypes on psc.PayoutTypeId equals pay.PayoutTypeId
        ////                              join sec in DB.PayslipSections on psc.SectionId equals sec.SectionId
        ////                              where pay.IsActive == true && pay.IsDeleted == false &&
        ////                                      sec.IsActive == true && sec.IsDeleted == false &&
        ////                                      psc.IsActive == true && psc.IsDeleted == false && psc.RecordStatus == true
        ////                                      && psc.EffectiveFrom <= model[i].EffectiveFrom && psc.EffectiveTo <= model[i].EffectiveFrom
        ////                              select pay).ToList();

        ////            if (loginId != 0)
        ////            {
        ////                if (Paydetails.Count() == 0)
        ////                {
        ////                    PayslipSectionComponent ltm = new PayslipSectionComponent();
        ////                    //em.EmpId = model[i].modelId;
        ////                    ltm.PayoutTypeId = model[i].PayoutTypeId;
        ////                    ltm.SectionId = model[i].SectionId;
        ////                    ltm.ComponentId = model[i].ComponentId;
        ////                    ltm.SequenceNo = model[i].SequenceNo;
        ////                    ltm.EffectiveFrom = model[i].EffectiveFrom;
        ////                    ltm.EffectiveTo = model[i].EffectiveTo;
        ////                    ltm.RecordStatus = model[i].RecordStatus;
        ////                    ltm.IsActive = true;
        ////                    ltm.IsUpdated = false;
        ////                    ltm.IsDeleted = false;
        ////                    ltm.CreatedBy = model[i].LoginId;
        ////                    ltm.CreatedDate = DateTime.Now;
        ////                    ltm.LastUpdatedBy = model[i].LoginId;
        ////                    ltm.LastUpdatedDate = DateTime.Now;
        ////                    DB.PayslipSectionComponents.Add(ltm);
        ////                    DB.SaveChanges();

        ////                }
        ////                else
        ////                {
        ////                    throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Sequence Details Already Exists - Check the Effective From and To Data");
        ////                }
        ////            }
        ////            else
        ////            {
        ////                throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
        ////            }
        ////        }

        ////        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
        ////        emvm.Status = 200;
        ////        emvm.msg = "Added";

        ////        return emvm;
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}

        ////public PayrollResponseViewModel AddPayslipSectionComponent(List<PayslipSectionComponentViewModel> model)
        ////{
        ////    try
        ////    {
        ////        foreach (var item in model)
        ////        {
        ////            if (item.LoginId == 0)
        ////                throw new CustomApiException(HttpStatusCode.Unauthorized, "Invalid LoginId");

        ////            // Check Date Overlap
        ////            var existing = (from psc in DB.PayslipSectionComponents
        ////                            where psc.PayoutTypeId == item.PayoutTypeId &&
        ////                                  psc.SectionId == item.SectionId &&
        ////                                  psc.ComponentId == item.ComponentId &&
        ////                                  psc.IsActive == true && psc.IsDeleted == false && psc.RecordStatus == true && 
        ////                                 (
        ////                                     psc.EffectiveFrom <= item.EffectiveTo &&
        ////                                     psc.EffectiveTo >= item.EffectiveFrom
        ////                                 )
        ////                            select psc).Any();

        ////            if (existing)
        ////                throw new CustomApiException(HttpStatusCode.BadRequest,
        ////                    "Payslip sequence details already exist in the selected effective date range.");

        ////            // Insert
        ////            var newData = new PayslipSectionComponent
        ////            {
        ////                PayoutTypeId = item.PayoutTypeId,
        ////                SectionId = item.SectionId,
        ////                ComponentId = item.ComponentId,
        ////                SequenceNo = item.SequenceNo,
        ////                EffectiveFrom = item.EffectiveFrom,
        ////                EffectiveTo = item.EffectiveTo,
        ////                RecordStatus = true,
        ////                IsActive = true,
        ////                IsUpdated = false,
        ////                IsDeleted = false,
        ////                CreatedBy = item.LoginId,
        ////                CreatedDate = DateTime.Now,
        ////                LastUpdatedBy = item.LoginId,
        ////                LastUpdatedDate = DateTime.Now
        ////            };

        ////            DB.PayslipSectionComponents.Add(newData);
        ////        }

        ////        DB.SaveChanges();

        ////        return new PayrollResponseViewModel
        ////        {
        ////            Status = 200,
        ////            msg = "Payslip section components added successfully"
        ////        };
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}

        public PayrollResponseViewModel AddPayslipSectionComponent(PayslipPayloadRequest request)
        {
            try
            {
                // Validate Login
                if (request.LoginId == 0)
                    throw new CustomApiException(HttpStatusCode.Unauthorized, "Invalid LoginId");

                foreach (var section in request.Sections)
                {
                    // Get SectionId from SectionName
                    var sectionMaster = DB.PayslipSections
                        .FirstOrDefault(x => x.SectionName.ToUpper() == section.SectionName.ToUpper() &&
                                             x.IsActive == true && x.IsDeleted == false);

                    if (sectionMaster == null)
                        throw new CustomApiException(HttpStatusCode.BadRequest, $"Invalid Section: {section.SectionName}");

                    foreach (var comp in section.Components)
                    {
                        // Check Date Overlap
                        bool exists = DB.PayslipSectionComponents.Any(x =>
                            x.PayoutTypeId == request.PayoutTypeId &&
                            x.SectionId == sectionMaster.SectionId &&
                            x.ComponentId == comp.ComponentId &&
                            x.IsActive == true && x.IsDeleted == false && x.RecordStatus == true &&
                            (
                                x.EffectiveFrom <= request.EffectiveDateTo &&
                                x.EffectiveTo >= request.EffectiveDateFrom
                            )
                        );

                        if (exists)
                            throw new CustomApiException(HttpStatusCode.BadRequest,
                                $"Component {comp.ComponentId} already exists in section {section.SectionName} for the given date range.");

                        // Insert New Record
                        var newData = new PayslipSectionComponent
                        {
                            PayoutTypeId = request.PayoutTypeId,
                            SectionId = sectionMaster.SectionId,
                            ComponentId = comp.ComponentId,
                            SequenceNo = comp.SequenceNo,
                            EffectiveFrom = request.EffectiveDateFrom,
                            EffectiveTo = request.EffectiveDateTo,
                            RecordStatus = true,
                            IsActive = true,
                            IsUpdated = false,
                            IsDeleted = false,
                            CreatedBy = request.LoginId,
                            CreatedDate = DateTime.Now,
                            LastUpdatedBy = request.LoginId,
                            LastUpdatedDate = DateTime.Now
                        };

                        DB.PayslipSectionComponents.Add(newData);
                    }
                }

                DB.SaveChanges();

                return new PayrollResponseViewModel
                {
                    Status = 200,
                    msg = "Payslip components added successfully"
                };
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel UpdatePayslipSectionComponent(UpdatePayslipPayload request)
        {
            try
            {
                // Basic validation
                if (request == null)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid payload");

                if (request.LoginId == 0)
                    throw new CustomApiException(HttpStatusCode.Unauthorized, "Invalid LoginId");

                if (request.Sections == null || !request.Sections.Any())
                    throw new CustomApiException(HttpStatusCode.BadRequest, "No section data provided");


                foreach (var section in request.Sections)
                {
                    // Resolve SectionId using SectionName
                    var sectionMaster = DB.PayslipSections
                        .FirstOrDefault(x => x.SectionName.ToUpper() == section.SectionName.ToUpper()
                                             && x.IsActive == true && x.IsDeleted == false);

                    if (sectionMaster == null)
                        throw new CustomApiException(HttpStatusCode.BadRequest,
                            $"Invalid section name: {section.SectionName}");

                    foreach (var comp in section.Components)
                    {
                        if (comp.SectionComponentId == 0)
                            throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid SectionComponentId");

                        // Fetch existing data
                        var existing = DB.PayslipSectionComponents
                            .FirstOrDefault(x => x.SectionComponentId == comp.SectionComponentId &&
                                                 x.IsActive == true && x.IsDeleted == false && x.RecordStatus == true);

                        if (existing == null)
                            throw new CustomApiException(HttpStatusCode.NotFound,
                                $"Component not found for SectionComponentId: {comp.SectionComponentId}");

                        // Update record
                        existing.PayoutTypeId = request.PayoutTypeId;
                        existing.SectionId = sectionMaster.SectionId;
                        existing.ComponentId = comp.ComponentId;
                        existing.SequenceNo = comp.SequenceNo;
                        existing.EffectiveFrom = request.EffectiveDateFrom;
                        existing.EffectiveTo = request.EffectiveDateTo;
                        existing.RecordStatus = true;
                        existing.IsActive = true;
                        existing.IsUpdated = true;
                        existing.IsDeleted = false;
                        existing.LastUpdatedBy = request.LoginId;
                        existing.LastUpdatedDate = DateTime.Now;
                    }
                }

                DB.SaveChanges();

                return new PayrollResponseViewModel
                {
                    Status = 200,
                    msg = "Payslip components updated successfully"
                };
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel DeletePayslipSectionComponent(DeletePayslipPayload request)
        {
            try
            {
                if (request == null)
                    throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid payload");

                if (request.LoginId == 0)
                    throw new CustomApiException(HttpStatusCode.Unauthorized, "Invalid LoginId");

                if (request.Sections == null || !request.Sections.Any())
                    throw new CustomApiException(HttpStatusCode.BadRequest, "No section data provided");


                foreach (var section in request.Sections)
                {
                    foreach (var comp in section.Components)
                    {
                        if (comp.SectionComponentId == 0)
                            throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid SectionComponentId");

                        // Fetch the record
                        var existing = DB.PayslipSectionComponents
                            .FirstOrDefault(x => x.SectionComponentId == comp.SectionComponentId &&
                                                 x.IsActive == true &&
                                                 x.IsDeleted == false &&
                                                 x.RecordStatus == true);

                        if (existing == null)
                            throw new CustomApiException(HttpStatusCode.NotFound,
                                $"Component not found for SectionComponentId: {comp.SectionComponentId}");

                        // Soft Delete
                        existing.RecordStatus = false;
                        existing.IsActive = false;
                        existing.IsDeleted = true;
                        existing.IsUpdated = true;
                        existing.LastUpdatedBy = request.LoginId;
                        existing.LastUpdatedDate = DateTime.Now;
                    }
                }

                DB.SaveChanges();

                return new PayrollResponseViewModel
                {
                    Status = 200,
                    msg = "Payslip components deleted successfully"
                };
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

        ////public PayrollResponseViewModel UpdatePayslipSectionComponent(List<PayslipSectionComponentViewModel> model)
        ////{
        ////    try
        ////    {
        ////        if (model == null || !model.Any())
        ////            throw new CustomApiException(HttpStatusCode.BadRequest, "No data provided");

        ////        foreach (var item in model)
        ////        {
        ////            if (item.LoginId == 0)
        ////                throw new CustomApiException(HttpStatusCode.Unauthorized, "Invalid LoginId");

        ////            if (item.SectionComponentId == 0)
        ////                throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid SectionComponentId");

        ////            // Fetch existing record
        ////            var existing = DB.PayslipSectionComponents
        ////                .FirstOrDefault(x => x.SectionComponentId == item.SectionComponentId &&
        ////                                     x.IsActive == true && x.IsDeleted == false && x.RecordStatus == true);

        ////            if (existing == null)
        ////                throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Section Component not found");

        ////            // Update record
        ////            existing.PayoutTypeId = item.PayoutTypeId;
        ////            existing.SectionId = item.SectionId;
        ////            existing.ComponentId = item.ComponentId;
        ////            existing.SequenceNo = item.SequenceNo;
        ////            existing.EffectiveFrom = item.EffectiveFrom;
        ////            existing.EffectiveTo = item.EffectiveTo;
        ////            existing.RecordStatus = true;
        ////            existing.IsActive = true;
        ////            existing.IsUpdated = true;
        ////            existing.IsDeleted = false;
        ////            existing.LastUpdatedBy = item.LoginId;
        ////            existing.LastUpdatedDate = DateTime.Now;
        ////        }

        ////        DB.SaveChanges();

        ////        return new PayrollResponseViewModel
        ////        {
        ////            Status = 200,
        ////            msg = "Updated successfully"
        ////        };
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}
        ////public PayrollResponseViewModel DeletePayslipSectionComponent(List<PayslipSectionComponentViewModel> model)
        ////{
        ////    try
        ////    {
        ////        if (model == null || !model.Any())
        ////            throw new CustomApiException(HttpStatusCode.BadRequest, "No data provided");

        ////        foreach (var item in model)
        ////        {
        ////            if (item.LoginId == 0)
        ////                throw new CustomApiException(HttpStatusCode.Unauthorized, "Invalid LoginId");

        ////            if (item.SectionComponentId == 0)
        ////                throw new CustomApiException(HttpStatusCode.BadRequest, "Invalid SectionComponentId");

        ////            var existing = DB.PayslipSectionComponents
        ////                .FirstOrDefault(x => x.SectionComponentId == item.SectionComponentId &&
        ////                                     x.IsActive == true && x.IsDeleted == false && x.RecordStatus == true);

        ////            if (existing == null)
        ////                throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Section Component not found");

        ////            // Soft Delete
        ////            existing.RecordStatus = false;
        ////            existing.IsActive = false;
        ////            existing.IsDeleted = true;
        ////            existing.IsUpdated = true;
        ////            existing.LastUpdatedBy = item.LoginId;
        ////            existing.LastUpdatedDate = DateTime.Now;
        ////        }

        ////        DB.SaveChanges();

        ////        return new PayrollResponseViewModel
        ////        {
        ////            Status = 200,
        ////            msg = "Deleted successfully"
        ////        };
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}


        ////public PayrollResponseViewModel UpdatePayslipSectionComponent(List<PayslipSectionComponentViewModel> model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        ////        //int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
        ////        int? id = (model.SectionComponentId != 0) ? model.SectionComponentId : 0;

        ////        var Paydetails = (from acc in DB.PayslipSectionComponents
        ////                          where acc.SectionId == id && acc.IsActive == true && acc.IsDeleted == false
        ////                          select acc).FirstOrDefault();

        ////        if (loginId != 0)
        ////        {
        ////            if (id != 0)
        ////            {
        ////                if (Paydetails != null)
        ////                {
        ////                    Paydetails.PayoutTypeId = model.PayoutTypeId;
        ////                    Paydetails.SectionId = model.SectionId;
        ////                    Paydetails.ComponentId = model.ComponentId;
        ////                    Paydetails.SequenceNo = model.SequenceNo;
        ////                    Paydetails.EffectiveFrom = model.EffectiveFrom;
        ////                    Paydetails.EffectiveTo = model.EffectiveTo;
        ////                    Paydetails.RecordStatus = model.RecordStatus;
        ////                    Paydetails.SequenceNo = model.SequenceNo;
        ////                    Paydetails.IsActive = true;
        ////                    Paydetails.IsUpdated = true;
        ////                    Paydetails.IsDeleted = false;
        ////                    Paydetails.LastUpdatedBy = model.LoginId;
        ////                    Paydetails.LastUpdatedDate = DateTime.Now;
        ////                    DB.SaveChanges();

        ////                    PayrollResponseViewModel emvm = new PayrollResponseViewModel();
        ////                    emvm.Status = 200;
        ////                    emvm.msg = "Updated";

        ////                    return emvm;
        ////                }
        ////                else
        ////                {
        ////                    throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Sequence Details Not Found");
        ////                }
        ////            }
        ////            else
        ////            {
        ////                throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Component Id is Mismatching");
        ////            }
        ////        }
        ////        else
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}
        ////public PayrollResponseViewModel DeletePayslipSectionComponent(List<PayslipSectionComponentViewModel> model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
        ////        //int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
        ////        int? id = (model.SectionId != 0) ? model.SectionId : 0;

        ////        var Paydetails = (from pay in DB.PayslipSectionComponents
        ////                          where pay.SectionId == id && pay.IsActive == true && pay.IsDeleted == false
        ////                          select pay).FirstOrDefault();

        ////        if (loginId != 0)
        ////        {
        ////            if (Paydetails != null)
        ////            {
        ////                Paydetails.IsActive = true;
        ////                Paydetails.IsUpdated = true;
        ////                Paydetails.IsDeleted = true;
        ////                Paydetails.LastUpdatedBy = model.LoginId;
        ////                Paydetails.LastUpdatedDate = DateTime.Now;
        ////                DB.SaveChanges();

        ////                PayrollResponseViewModel emvm = new PayrollResponseViewModel();
        ////                emvm.Status = 200;
        ////                emvm.msg = "Deleted";

        ////                return emvm;
        ////            }
        ////            else
        ////            {
        ////                throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Sequence Details Not Found");
        ////            }
        ////        }
        ////        else
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}
        ////public List<EmployeeSalaryDetailViewModel> GetAllEmployeeSalaryDetails(EmployeeSalaryDetailViewModel model)
        ////{
        ////    try
        ////    {
        ////        string msg = "";
        ////        int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

        ////        var Paydetails = (from pay in DB.EmployeeSalaryDetails
        ////                          where pay.IsActive == true && pay.IsDeleted == false
        ////                          select pay).OrderByDescending(x => x.SalaryId).ToList();

        ////        if (loginId != 0)
        ////        {
        ////            if (Paydetails != null)
        ////            {
        ////                List<EmployeeSalaryDetailViewModel> lstofpaytype = new List<EmployeeSalaryDetailViewModel>();

        ////                for (int i = 0; i < Paydetails.Count(); i++)
        ////                {
        ////                    EmployeeSalaryDetailViewModel ltvm = new EmployeeSalaryDetailViewModel();
        ////                    ltvm.SalaryId = Paydetails[i].SalaryId;
        ////                    ltvm.EmpId = Paydetails[i].EmpId;
        ////                    int? empid = Paydetails[i].EmpId;
        ////                    ltvm.FirstName = DB.EmployeeMasters.Where(x => x.EmpId == empid && x.IsActive == true && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault();
        ////                    ltvm.MiddleName = DB.EmployeeMasters.Where(x => x.EmpId == empid && x.IsActive == true && x.IsDeleted == false).Select(x => x.MiddleName).FirstOrDefault();
        ////                    ltvm.LastName = DB.EmployeeMasters.Where(x => x.EmpId == empid && x.IsActive == true && x.IsDeleted == false).Select(x => x.LastName).FirstOrDefault();
        ////                    ltvm.EmpCode = Paydetails[i].EmpCode;
        ////                    ltvm.CTC = Paydetails[i].CTC;
        ////                    ltvm.MCTC = Paydetails[i].MCTC;
        ////                    ltvm.EffectiveFromDate = Paydetails[i].EffectiveFromDate;
        ////                    ltvm.EffectiveToDate = Paydetails[i].EffectiveToDate;
        ////                    ltvm.IsAppraised = Paydetails[i].IsAppraised;
        ////                    ltvm.RecordStatus = Paydetails[i].RecordStatus;
        ////                    ltvm.CreatedBy = Paydetails[i].CreatedBy;
        ////                    ltvm.CreatedDate = Paydetails[i].CreatedDate;
        ////                    ltvm.LastUpdatedBy = Paydetails[i].LastUpdatedBy;
        ////                    ltvm.LastUpdatedDate = Paydetails[i].LastUpdatedDate;
        ////                    ltvm.IsActive = Paydetails[i].IsActive;
        ////                    ltvm.IsUpdated = Paydetails[i].IsUpdated;
        ////                    ltvm.IsDeleted = Paydetails[i].IsDeleted;
        ////                    lstofpaytype.Add(ltvm);

        ////                }
        ////                return lstofpaytype;
        ////            }
        ////            else
        ////            {
        ////                throw new CustomApiException(HttpStatusCode.NotFound, "Payslip Section Details Not Found");
        ////            }
        ////        }
        ////        else
        ////        {
        ////            throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
        ////        }
        ////    }
        ////    catch (CustomApiException ex)
        ////    {
        ////        throw new CustomApiException(ex.StatusCode, ex.Message);
        ////    }
        ////}
        public List<EmployeeSalaryDetailViewModel> GetAllEmployeeSalaryDetails(EmployeeSalaryDetailViewModel model)
        {
            try
            {
                // ✅ Validate required parameter
                if (model?.LoginId == null || model.LoginId == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");

                int loginId = model.LoginId;

                // ✅ Optional filter parameters (only LEId is required)
                int? leId = (model.LEId > 0) ? model.LEId : null;
                int? compId = (model.CompId > 0) ? model.CompId : null;
                int? buId = (model.BUId > 0) ? model.BUId : null;
                int? locId = (model.LocId > 0) ? model.LocId : null;
                int? deptId = (model.DeptId > 0) ? model.DeptId : null;
                int? designationId = (model.DesignationId > 0) ? model.DesignationId : null;
                int? reportId = (model.ReportId > 0) ? model.ReportId : null;
                int? empId = (model.EmpId > 0) ? model.EmpId : null;

                // ✅ Build query with dynamic filters
                var query = from pay in DB.EmployeeSalaryDetails
                            join emp in DB.EmployeeMasters on pay.EmpId equals emp.EmpId
                            where pay.IsActive == true && pay.IsDeleted == false
                                  && emp.IsActive == true && emp.IsDeleted == false
                            select new { pay, emp };

                // ✅ Apply filters dynamically (only if values are provided)
                if (leId.HasValue)
                    query = query.Where(x => x.emp.LEId == leId);

                //if (compId.HasValue)
                //query = query.Where(x => x.pay.CompId == compId);

                if (buId.HasValue)
                    query = query.Where(x => x.emp.BUId == buId);

                if (locId.HasValue)
                    query = query.Where(x => x.emp.LocationId == locId);

                if (deptId.HasValue)
                    query = query.Where(x => x.emp.CategoryId == deptId);

                if (designationId.HasValue)
                    query = query.Where(x => x.emp.DesignationId == designationId);

                if (reportId.HasValue)
                    query = query.Where(x => x.emp.ReportId == reportId);

                if (empId.HasValue)
                    query = query.Where(x => x.emp.EmpId == empId);

                // ✅ Execute query
                var payDetails = query
                    .OrderByDescending(x => x.pay.SalaryId)
                    .ToList();

                // ✅ Check if any records found
                if (!payDetails.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "Salary Details Not Found");

                // ✅ Map results efficiently
                var result = new List<EmployeeSalaryDetailViewModel>();

                foreach (var item in payDetails)
                {
                    var vm = new EmployeeSalaryDetailViewModel
                    {
                        SalaryId = item.pay.SalaryId,
                        EmpId = item.pay.EmpId,
                        FirstName = item.emp.FirstName ?? "",
                        MiddleName = item.emp.MiddleName ?? "",
                        LastName = item.emp.LastName ?? "",
                        EmpCode = item.pay.EmpCode ?? "",
                        CTC = item.pay.CTC,
                        MCTC = item.pay.MCTC,
                        PerviousCTC = item.pay.PerviousCTC,
                        IncrementPercent = item.pay.IncrementPercent,
                        EffectiveFromDate = item.pay.EffectiveFromDate,
                        EffectiveToDate = item.pay.EffectiveToDate,
                        IsAppraised = item.pay.IsAppraised,
                        RecordStatus = item.pay.RecordStatus,
                        IsFixed = item.pay.IsFixed,
                        IsVariable = item.pay.IsVariable,
                        Period = item.pay.Period,
                        VariableId = item.pay.VariableId,
                        VariableName = item.pay.VariableName,
                        VariableCode = item.pay.VariableCode,
                        VariableAmt = item.pay.VariableAmt,
                        IsArrear = item.pay.IsArrear,
                        ArrearAmt = item.pay.ArrearAmt,
                        IsClearArrear = item.pay.IsClearArrear,
                        PendingMonth = item.pay.PendingMonth,
                        DescriptionforArrear = item.pay.DescriptionforArrear,
                        ArrearYear = item.pay.ArrearYear,
                        ArrearMonth = item.pay.ArrearMonth,
                        CreatedDate = item.pay.CreatedDate,
                        LastUpdatedBy = item.pay.LastUpdatedBy,
                        LastUpdatedDate = item.pay.LastUpdatedDate,
                        IsActive = item.pay.IsActive,
                        IsUpdated = item.pay.IsUpdated,
                        IsDeleted = item.pay.IsDeleted
                    };

                    result.Add(vm);
                }

                return result;
            }
            catch (CustomApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomApiException(HttpStatusCode.InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }
        public List<EmployeeSalaryDetailViewModel> GetEmployeeSalaryDetails(EmployeeSalaryDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;

                var Paydetails = (from pay in DB.EmployeeSalaryDetails
                                  where pay.EmpId == empId && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).OrderByDescending(x => x.SalaryId).ToList();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        List<EmployeeSalaryDetailViewModel> lstofpaytype = new List<EmployeeSalaryDetailViewModel>();

                        for (int i = 0; i < Paydetails.Count(); i++)
                        {
                            EmployeeSalaryDetailViewModel ltvm = new EmployeeSalaryDetailViewModel();
                            ltvm.SalaryId = Paydetails[i].SalaryId;
                            ltvm.EmpId = Paydetails[i].EmpId;
                            int? empid = Paydetails[i].EmpId;
                            ltvm.FirstName = DB.EmployeeMasters.Where(x => x.EmpId == empid && x.IsActive == true && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault();
                            ltvm.MiddleName = DB.EmployeeMasters.Where(x => x.EmpId == empid && x.IsActive == true && x.IsDeleted == false).Select(x => x.MiddleName).FirstOrDefault();
                            ltvm.LastName = DB.EmployeeMasters.Where(x => x.EmpId == empid && x.IsActive == true && x.IsDeleted == false).Select(x => x.LastName).FirstOrDefault();
                            ltvm.EmpCode = Paydetails[i].EmpCode;
                            ltvm.CTC = Paydetails[i].CTC;
                            ltvm.MCTC = Paydetails[i].MCTC;
                            ltvm.EffectiveFromDate = Paydetails[i].EffectiveFromDate;
                            ltvm.EffectiveToDate = Paydetails[i].EffectiveToDate;
                            ltvm.IsAppraised = Paydetails[i].IsAppraised;
                            ltvm.RecordStatus = Paydetails[i].RecordStatus;
                            ltvm.IsFixed = Paydetails[i].IsFixed;
                            ltvm.IsVariable = Paydetails[i].IsVariable;
                            ltvm.Period = Paydetails[i].Period;
                            ltvm.VariableId = Paydetails[i].VariableId;
                            ltvm.VariableName = Paydetails[i].VariableName;
                            ltvm.VariableCode = Paydetails[i].VariableCode;
                            ltvm.VariableAmt = Paydetails[i].VariableAmt;
                            ltvm.IsArrear = Paydetails[i].IsArrear;
                            ltvm.ArrearAmt = Paydetails[i].ArrearAmt;
                            ltvm.IsClearArrear = Paydetails[i].IsClearArrear;
                            ltvm.PendingMonth = Paydetails[i].PendingMonth;
                            ltvm.ArrearYear = Paydetails[i].ArrearYear;
                            ltvm.ArrearMonth = Paydetails[i].ArrearMonth;
                            ltvm.DescriptionforArrear = Paydetails[i].DescriptionforArrear;
                            ltvm.CreatedBy = Paydetails[i].CreatedBy;
                            ltvm.CreatedDate = Paydetails[i].CreatedDate;
                            ltvm.LastUpdatedBy = Paydetails[i].LastUpdatedBy;
                            ltvm.LastUpdatedDate = Paydetails[i].LastUpdatedDate;
                            ltvm.IsActive = Paydetails[i].IsActive;
                            ltvm.IsUpdated = Paydetails[i].IsUpdated;
                            ltvm.IsDeleted = Paydetails[i].IsDeleted;
                            lstofpaytype.Add(ltvm);
                        }
                        return lstofpaytype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Salary Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel AddEmployeeSalaryDetails(EmployeeSalaryDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Paydetails = (from pay in DB.EmployeeSalaryDetails
                                  where pay.EmpId == EmpId && pay.RecordStatus == true
                                  && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).ToList();

                DateTime? Today = DateTime.Now;
                DateTime? effstartdate = model.EffectiveFromDate;
                DateTime? effenddate = model.EffectiveFromDate;

                if (effstartdate.HasValue)
                {
                    ////effenddate = effstartdate.Value.AddDays(1);
                    effenddate = model.EffectiveFromDate?.AddDays(-1);
                }

                if (loginId != 0)
                {
                    if (model.IsAppraised == true)
                    {
                        var Saldetails = (from pay in DB.EmployeeSalaryDetails
                                          where pay.EmpId == EmpId && pay.EffectiveFromDate <= effstartdate //&& pay.EffectiveToDate >= effstartdate
                                          && pay.IsActive == true && pay.IsDeleted == false && pay.RecordStatus == true
                                          select pay).OrderByDescending(x => x.SalaryId).ToList();

                        if (Saldetails.Count() > 0)
                        {
                            Saldetails[0].EffectiveToDate = effenddate;
                            Saldetails[0].RecordStatus = false;
                            Saldetails[0].IsActive = true;
                            Saldetails[0].IsUpdated = true;
                            Saldetails[0].IsDeleted = false;
                            Saldetails[0].LastUpdatedBy = model.LoginId;
                            Saldetails[0].LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();
                        }

                        ////EmployeeSalaryDetail ltm = new EmployeeSalaryDetail();
                        ////ltm.EmpId = model.EmpId;
                        ////ltm.EmpCode = model.EmpCode;
                        ////ltm.CTC = model.CTC;
                        ////ltm.MCTC = model.MCTC;
                        ////ltm.PerviousCTC = Saldetails[0].CTC;
                        ////ltm.IncrementPercent = ltm.PerviousCTC > 0 ? ((ltm.CTC - ltm.PerviousCTC) / ltm.PerviousCTC) * 100 : 0;
                        ////ltm.EffectiveFromDate = model.EffectiveFromDate;
                        ////ltm.EffectiveToDate = model.EffectiveToDate;
                        ////ltm.IsAppraised = model.IsAppraised;
                        ////ltm.RecordStatus = true;
                        ////ltm.IsActive = true;
                        ////ltm.IsUpdated = false;
                        ////ltm.IsDeleted = false;
                        ////ltm.CreatedBy = model.LoginId;
                        ////ltm.CreatedDate = DateTime.Now;
                        ////ltm.LastUpdatedBy = model.LoginId;
                        ////ltm.LastUpdatedDate = DateTime.Now;
                        ////DB.EmployeeSalaryDetails.Add(ltm);
                        ////DB.SaveChanges();
                        ///

                        EmployeeSalaryDetail ltm = new EmployeeSalaryDetail();

                        ltm.EmpId = model.EmpId;
                        ltm.EmpCode = model.EmpCode;
                        ltm.CTC = model.CTC;
                        ltm.MCTC = model.MCTC;

                        // ✅ Safe previous CTC
                        decimal previousCTC = Saldetails != null && Saldetails.Count > 0 && Saldetails[0].CTC.HasValue
                            ? Saldetails[0].CTC.Value
                            : 0;

                        ltm.PerviousCTC = previousCTC;

                        // ✅ Safe Increment Calculation
                        if (ltm.CTC.HasValue && previousCTC > 0)
                        {
                            ltm.IncrementPercent = ((ltm.CTC.Value - previousCTC) / previousCTC) * 100;
                        }
                        else
                        {
                            ltm.IncrementPercent = 0;
                        }

                        ltm.EffectiveFromDate = model.EffectiveFromDate;
                        ltm.EffectiveToDate = model.EffectiveToDate;
                        ltm.IsAppraised = model.IsAppraised;
                        ltm.RecordStatus = true;
                        ltm.IsFixed = model.IsFixed;
                        ltm.IsVariable = model.IsVariable;
                        ltm.Period = model.Period;
                        ltm.VariableId = model.VariableId;
                        ltm.VariableName = model.VariableName;
                        ltm.VariableCode = model.VariableCode;
                        ltm.VariableAmt = model.VariableAmt;
                        ltm.IsArrear = model.IsArrear;
                        ltm.ArrearAmt = model.ArrearAmt;
                        ltm.IsClearArrear = false;
                        ltm.PendingMonth = model.PendingMonth;
                        ltm.ArrearYear = model.ArrearYear;
                        ltm.ArrearMonth = model.ArrearMonth;
                        ltm.DescriptionforArrear = model.DescriptionforArrear;
                        ltm.IsActive = true;
                        ltm.IsUpdated = false;
                        ltm.IsDeleted = false;
                        ltm.CreatedBy = model.LoginId;
                        ltm.CreatedDate = DateTime.Now;
                        ltm.LastUpdatedBy = model.LoginId;
                        ltm.LastUpdatedDate = DateTime.Now;

                        DB.EmployeeSalaryDetails.Add(ltm);
                        DB.SaveChanges();

                        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Appraisal Added";

                        return emvm;
                    }
                    else
                    {
                        if (Paydetails.Count() == 0)
                        {
                            EmployeeSalaryDetail ltm = new EmployeeSalaryDetail();
                            ltm.EmpId = model.EmpId;
                            ltm.EmpCode = model.EmpCode;
                            ltm.CTC = model.CTC;
                            ltm.MCTC = model.MCTC;
                            ltm.PerviousCTC = 0;
                            ltm.IncrementPercent = 0;
                            ltm.EffectiveFromDate = model.EffectiveFromDate;
                            ltm.EffectiveToDate = model.EffectiveToDate;
                            ltm.IsAppraised = model.IsAppraised;
                            ltm.RecordStatus = true;
                            ltm.IsFixed = model.IsFixed;
                            ltm.IsVariable = model.IsVariable;
                            ltm.Period = model.Period;
                            ltm.VariableId = model.VariableId;
                            ltm.VariableName = model.VariableName;
                            ltm.VariableCode = model.VariableCode;
                            ltm.VariableAmt = model.VariableAmt;
                            ltm.IsArrear = model.IsArrear;
                            ltm.ArrearAmt = model.ArrearAmt;
                            ltm.IsClearArrear = false;
                            ltm.PendingMonth = model.PendingMonth;
                            ltm.ArrearYear = model.ArrearYear;
                            ltm.ArrearMonth = model.ArrearMonth;
                            ltm.DescriptionforArrear = model.DescriptionforArrear;
                            ltm.IsActive = true;
                            ltm.IsUpdated = false;
                            ltm.IsDeleted = false;
                            ltm.CreatedBy = model.LoginId;
                            ltm.CreatedDate = DateTime.Now;
                            ltm.LastUpdatedBy = model.LoginId;
                            ltm.LastUpdatedDate = DateTime.Now;
                            DB.EmployeeSalaryDetails.Add(ltm);
                            DB.SaveChanges();

                            PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                            emvm.Status = 200;
                            emvm.msg = "CTC Added";

                            return emvm;
                        }
                        else if (Paydetails.Count() > 0)
                        {
                            var effFrom = Paydetails[0].EffectiveFromDate is DateTime dt
                                            ? dt.ToString("yyyy-MM-dd")
                                            : Paydetails[0].EffectiveFromDate?.ToString();
                            ////    throw new CustomApiException(HttpStatusCode.NotFound, "A CTC record for EmpCode " + Paydetails[0].EmpCode + " is already active for the period" +
                            ////        Paydetails[0].EffectiveFromDate.ToString("YYYY-MM-DD") + " to till date " with a CTC of " + Paydetails[0].CTC + ".To proceed with changes, " +
                            ////        "please select the Appraisal checkbox.");
                            throw new CustomApiException(HttpStatusCode.NotFound, "An active CTC record already exists for employee code " + Paydetails[0].EmpCode +
                                " for the period " + effFrom + " to till date, with a CTC of " + Paydetails[0].CTC +
                                ". To proceed with changes, please select the Appraisal checkbox.");
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Employee Salary Details Already Exists");
                        }
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel UpdateEmployeeSalaryDetails(EmployeeSalaryDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                //int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.SalaryId != 0) ? model.SalaryId : 0;

                var Paydetails = (from acc in DB.EmployeeSalaryDetails
                                  where acc.SalaryId == id && acc.IsActive == true && acc.IsDeleted == false && acc.RecordStatus == true
                                  select acc).FirstOrDefault();

                if (loginId != 0)
                {
                    if (id != 0)
                    {
                        if (Paydetails != null)
                        {
                            Paydetails.EmpCode = model.EmpCode;
                            Paydetails.CTC = model.CTC;
                            Paydetails.MCTC = model.MCTC;
                            Paydetails.PerviousCTC = model.CTC;
                            Paydetails.IncrementPercent = Paydetails.PerviousCTC > 0 ? ((Paydetails.CTC - Paydetails.PerviousCTC) / Paydetails.PerviousCTC) * 100 : 0;

                            // ✅ Safe previous CTC
                            decimal previousCTC = Paydetails != null  && Paydetails.CTC.HasValue
                                ? Paydetails.CTC.Value
                                : 0;

                            Paydetails.PerviousCTC = previousCTC;

                            // ✅ Safe Increment Calculation
                            if (Paydetails.CTC.HasValue && previousCTC > 0)
                            {
                                Paydetails.IncrementPercent = ((Paydetails.CTC.Value - previousCTC) / previousCTC) * 100;
                            }
                            else
                            {
                                Paydetails.IncrementPercent = 0;
                            }

                            Paydetails.EffectiveFromDate = model.EffectiveFromDate;
                            Paydetails.EffectiveToDate = model.EffectiveToDate;
                            Paydetails.IsAppraised = model.IsAppraised;
                            Paydetails.RecordStatus = model.RecordStatus;
                            Paydetails.IsFixed = model.IsFixed;
                            Paydetails.IsVariable = model.IsVariable;
                            Paydetails.Period = model.Period;
                            Paydetails.VariableId = model.VariableId;
                            Paydetails.VariableName = model.VariableName;
                            Paydetails.VariableCode = model.VariableCode;
                            Paydetails.VariableAmt = model.VariableAmt;
                            Paydetails.IsArrear = model.IsArrear;
                            Paydetails.ArrearAmt = model.ArrearAmt;
                            Paydetails.PendingMonth = model.PendingMonth;
                            Paydetails.ArrearYear = model.ArrearYear;
                            Paydetails.ArrearMonth = model.ArrearMonth;
                            Paydetails.DescriptionforArrear = model.DescriptionforArrear;
                            Paydetails.IsActive = true;
                            Paydetails.IsUpdated = true;
                            Paydetails.IsDeleted = false;
                            Paydetails.LastUpdatedBy = model.LoginId;
                            Paydetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();

                            PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                            emvm.Status = 200;
                            emvm.msg = "Updated";

                            return emvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Employee Salary Details Not Found");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Salary Id is Mismatching");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel DeleteEmployeeSalaryDetails(EmployeeSalaryDetailViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                //int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.SalaryId != 0) ? model.SalaryId : 0;

                var Paydetails = (from acc in DB.EmployeeSalaryDetails
                                  where acc.SalaryId == id && acc.IsActive == true && acc.IsDeleted == false && acc.RecordStatus == true
                                  select acc).FirstOrDefault();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        Paydetails.RecordStatus = false;
                        Paydetails.IsActive = true;
                        Paydetails.IsUpdated = true;
                        Paydetails.IsDeleted = true;
                        Paydetails.LastUpdatedBy = model.LoginId;
                        Paydetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Deleted";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Salary Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<PayoutMappingMasterViewModel> GetAllPayoutMappingMaster(PayoutMappingMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var Paydetails = (from pay in DB.PayoutMappingMasters
                                  where pay.IsActive == true && pay.IsDeleted == false
                                  select pay).OrderByDescending(x => x.MapId).ToList();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        List<PayoutMappingMasterViewModel> lstofpaytype = new List<PayoutMappingMasterViewModel>();

                        for (int i = 0; i < Paydetails.Count(); i++)
                        {
                            PayoutMappingMasterViewModel ltvm = new PayoutMappingMasterViewModel();
                            ltvm.LoginId = (int)loginId;
                            ltvm.MapId = Paydetails[i].MapId;
                            ltvm.GradeId = Paydetails[i].GradeId;
                            ltvm.Grade = Paydetails[i].Grade;
                            ltvm.PayoutTypeId = Paydetails[i].PayoutTypeId;
                            ltvm.PayoutTypeName = Paydetails[i].PayoutTypeName;
                            ltvm.CreatedBy = Paydetails[i].CreatedBy;
                            ltvm.CreatedDate = Paydetails[i].CreatedDate;
                            ltvm.LastUpdatedBy = Paydetails[i].LastUpdatedBy;
                            ltvm.LastUpdatedDate = Paydetails[i].LastUpdatedDate;
                            ltvm.IsActive = Paydetails[i].IsActive;
                            ltvm.IsUpdated = Paydetails[i].IsUpdated;
                            ltvm.IsDeleted = Paydetails[i].IsDeleted;
                            lstofpaytype.Add(ltvm);

                        }
                        return lstofpaytype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout-Employee Grade Mapping Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayoutMappingMasterViewModel GetPayoutMappingMaster(PayoutMappingMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                //int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Paydetails = (from pay in DB.PayoutMappingMasters
                                  where pay.MapId == model.MapId && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).OrderByDescending(x => x.MapId).FirstOrDefault();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        PayoutMappingMasterViewModel ltvm = new PayoutMappingMasterViewModel();
                        ltvm.LoginId = (int)loginId;
                        ltvm.MapId = Paydetails.MapId;
                        ltvm.GradeId = Paydetails.GradeId;
                        ltvm.Grade = Paydetails.Grade;
                        ltvm.PayoutTypeId = Paydetails.PayoutTypeId;
                        ltvm.PayoutTypeName = Paydetails.PayoutTypeName;
                        ltvm.CreatedBy = Paydetails.CreatedBy;
                        ltvm.CreatedDate = Paydetails.CreatedDate;
                        ltvm.LastUpdatedBy = Paydetails.LastUpdatedBy;
                        ltvm.LastUpdatedDate = Paydetails.LastUpdatedDate;
                        ltvm.IsActive = Paydetails.IsActive;
                        ltvm.IsUpdated = Paydetails.IsUpdated;
                        ltvm.IsDeleted = Paydetails.IsDeleted;
                        return ltvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout-Employee Grade Mapping Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel AddPayoutMappingMaster(PayoutMappingMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                //int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var Paydetails = (from pay in DB.PayoutMappingMasters
                                  where pay.GradeId == model.GradeId
                                  && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).ToList();

                if (loginId != 0)
                {
                    if (Paydetails.Count() == 0)
                    {
                        PayoutMappingMaster pmm = new PayoutMappingMaster();
                        //em.EmpId = model.modelId;
                        pmm.GradeId = model.GradeId;
                        pmm.Grade = model.Grade;
                        pmm.PayoutTypeId = model.PayoutTypeId;
                        pmm.PayoutTypeName = model.PayoutTypeName;
                        pmm.IsActive = true;
                        pmm.IsUpdated = false;
                        pmm.IsDeleted = false;
                        pmm.CreatedBy = model.LoginId;
                        pmm.CreatedDate = DateTime.Now;
                        pmm.LastUpdatedBy = model.LoginId;
                        pmm.LastUpdatedDate = DateTime.Now;
                        DB.PayoutMappingMasters.Add(pmm);
                        DB.SaveChanges();

                        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Added";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout-Employee Grade Mapping Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel UpdatePayoutMappingMaster(PayoutMappingMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                //int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.MapId != 0) ? model.MapId : 0;

                var Paydetails = (from acc in DB.PayoutMappingMasters
                                  where acc.MapId == id && acc.IsActive == true && acc.IsDeleted == false
                                  select acc).FirstOrDefault();

                if (loginId != 0)
                {
                    if (id != 0)
                    {
                        if (Paydetails != null)
                        {
                            Paydetails.GradeId = model.GradeId;
                            Paydetails.Grade = model.Grade;
                            Paydetails.PayoutTypeId = model.PayoutTypeId;
                            Paydetails.PayoutTypeName = model.PayoutTypeName;
                            Paydetails.IsActive = true;
                            Paydetails.IsUpdated = true;
                            Paydetails.IsDeleted = false;
                            Paydetails.LastUpdatedBy = model.LoginId;
                            Paydetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();

                            PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                            emvm.Status = 200;
                            emvm.msg = "Updated";

                            return emvm;
                        }
                        else
                        {
                            throw new CustomApiException(HttpStatusCode.NotFound, "Payout-Employee Grade Mapping Details Not Found");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout Mapping Id is Mismatching");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollResponseViewModel DeletePayoutMappingMaster(PayoutMappingMasterViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                //int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? id = (model.MapId != 0) ? model.MapId : 0;

                var Paydetails = (from pay in DB.PayoutMappingMasters
                                  where pay.MapId == id && pay.IsActive == true && pay.IsDeleted == false
                                  select pay).FirstOrDefault();

                if (loginId != 0)
                {
                    if (Paydetails != null)
                    {
                        Paydetails.IsActive = true;
                        Paydetails.IsUpdated = true;
                        Paydetails.IsDeleted = true;
                        Paydetails.LastUpdatedBy = model.LoginId;
                        Paydetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        PayrollResponseViewModel emvm = new PayrollResponseViewModel();
                        emvm.Status = 200;
                        emvm.msg = "Deleted";

                        return emvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payout-Employee Grade Mapping Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<PayrollReportViewModel> PayrollReportforALL(PayrollReportViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;
                //int? compId = (model.CompId != 0) ? model.CompId : 0;
                int? leId = (model.LEId != 0) ? model.LEId : 0;
                //int? buId = (model.BUId != 0) ? model.BUId : 0;
                int? locationId = (model.LocationId != 0) ? model.LocationId : 0;
                int? deptId = (model.DeptId != 0) ? model.DeptId : 0;
                int? desigId = (model.DesignationId != 0) ? model.DesignationId : 0;
                int year = model.Year != 0 ? model.Year : DateTime.Now.Year;
                int monthNo = model.MonthNo != 0 ? model.MonthNo : DateTime.Now.AddMonths(-1).Month;

                int currentmonth = DateTime.Now.Month;
                int currentyear = DateTime.Now.Year;

                if (currentmonth == monthNo && currentyear == year)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Unable to load data for the current month. Loading current-month records is not supported on this page.");
                }

                string month = new DateTime(1, monthNo, 1).ToString("MMM");

                DateTime Today = DateTime.Now;
                // total days
                int totalDays = DateTime.DaysInMonth(year, monthNo);

                // start & end dates
                DateTime startDate = new DateTime(year, monthNo, 1);
                DateTime endDate = new DateTime(year, monthNo, totalDays);

                ////var lop = (from lev in DB.EmpLeaveApplications
                ////           where lev.EmpId == loginId
                ////              && lev.LeaveTypeId == 0
                ////              && lev.StartDate >= startDate
                ////              && lev.EndDate <= endDate
                ////              && lev.IsActive == true
                ////              && lev.IsDeleted == false
                ////           orderby lev.StartDate descending
                ////           select lev).ToList();

                ////decimal? lopDuration = (from lev in DB.EmpLeaveApplications
                ////                        where lev.EmpId == loginId
                ////                           && lev.LeaveTypeId == 0
                ////                           && lev.StartDate >= startDate
                ////                           && lev.EndDate <= endDate
                ////                           && lev.IsActive == true
                ////                           && lev.IsDeleted == false
                ////                        select lev.Duration)
                ////                       .DefaultIfEmpty(0)           // avoid null result
                ////                       .Sum();

                ////decimal? workingdays = totalDays - lopDuration;

                ////decimal? ELDuration = (from lev in DB.EmpLeaveApplications
                ////                       join lty in DB.LeaveTypeMasters on lev.LeaveTypeId equals lty.LeaveTypeId
                ////                        where lty.ShortName.ToUpper() == "EL" &&lev.EmpId == loginId
                ////                           && lev.StartDate >= startDate
                ////                           && lev.EndDate <= endDate
                ////                           && lev.IsActive == true
                ////                           && lev.IsDeleted == false
                ////                           && lty.IsActive == true
                ////                           && lty.IsDeleted == false
                ////                       select lev.Duration)
                ////                       .DefaultIfEmpty(0)           // avoid null result
                ////                       .Sum();

                ////decimal? CLDuration = (from lev in DB.EmpLeaveApplications
                ////                       join lty in DB.LeaveTypeMasters on lev.LeaveTypeId equals lty.LeaveTypeId
                ////                       where lty.ShortName.ToUpper() == "CL" && lev.EmpId == loginId
                ////                          && lev.StartDate >= startDate
                ////                          && lev.EndDate <= endDate
                ////                          && lev.IsActive == true
                ////                          && lev.IsDeleted == false
                ////                          && lty.IsActive == true
                ////                          && lty.IsDeleted == false
                ////                       select lev.Duration)
                ////                       .DefaultIfEmpty(0)           // avoid null result
                ////                       .Sum();


                var empDetails = (from emp in DB.EmployeeMasters
                                  join sal in DB.EmployeeSalaryDetails on emp.EmpId equals sal.EmpId
                                  join comp in DB.CompanyMasters on emp.CompId equals comp.CompId
                                  join dept in DB.DeptMasters on emp.CategoryId equals dept.DeptId
                                  join des in DB.DesignationMasters on emp.DesignationId equals des.DesignationId
                                  //where emp.EmpCode.Contains("3DCAD-")
                                  where emp.EmpStatus.ToUpper() == "ACTIVE"
                                  && emp.IsActive == true && emp.IsDeleted == false
                                  && sal.EffectiveFromDate >= startDate && sal.EffectiveFromDate <= endDate //&& sal.EffectiveToDate == null && sal.IsAppraised == true 
                                  && sal.IsActive == true && sal.IsDeleted == false
                                  && comp.IsActive == true && comp.IsDeleted == false
                                  && dept.IsActive == true && dept.IsDeleted == false
                                  && des.IsActive == true && des.IsDeleted == false
                                  select new
                                  {
                                      emp.EmpId,
                                      emp.EmpCode,
                                      EmpName = emp.FirstName + " " + emp.MiddleName + " " + emp.LastName,
                                      emp.OldEmp_ID,
                                      emp.CompId,
                                      emp.LEId,
                                      emp.BUId,
                                      emp.LocationId,
                                      CompName = comp.Company,
                                      DesignationName = des.Designation,
                                      DeptName = dept.DeptName,
                                      emp.CategoryId,
                                      emp.DesignationId,
                                      sal.CTC
                                  }).ToList();

                //if (compId != 0)
                //{
                //    var compfilter = empDetails.Where(x => x.CompId == compId).ToList();
                //    empDetails = compfilter.ToList();
                //}
                if (leId != 0)
                {
                    var lefilter = empDetails.Where(x => x.LEId == leId).ToList();
                    empDetails = lefilter.ToList();
                }
                //if (buId != 0)
                //{
                //    var bufilter = empDetails.Where(x => x.BUId == buId).ToList();
                //    empDetails = bufilter.ToList();
                //}
                if (locationId != 0)
                {
                    var locfilter = empDetails.Where(x => x.LocationId == locationId).ToList();
                    empDetails = locfilter.ToList();
                }
                if (deptId != 0)
                {
                    var deptfilter = empDetails.Where(x => x.CategoryId == deptId).ToList();
                    empDetails = deptfilter.ToList();
                }
                if (desigId != 0)
                {
                    var desgfilter = empDetails.Where(x => x.DesignationId == desigId).ToList();
                    empDetails = desgfilter.ToList();
                }
                if (empId != 0)
                {
                    var empfilter = empDetails.Where(x => x.EmpId == empId).ToList();
                    empDetails = empfilter.ToList();
                }

                if (loginId != 0)
                {
                    if (empDetails != null)
                    {
                        List<PayrollReportViewModel> lstofpaytype = new List<PayrollReportViewModel>();

                        for (int i = 0; i < empDetails.Count(); i++)
                        {
                            int? EmpId = empDetails[i].EmpId;

                            var lop = (from lev in DB.EmpLeaveApplications
                                       where lev.EmpId == EmpId
                                          && lev.LeaveTypeId == 0
                                          && lev.StartDate >= startDate
                                          && lev.EndDate <= endDate
                                          && lev.IsActive == true
                                          && lev.IsDeleted == false
                                       orderby lev.StartDate descending
                                       select lev).ToList();

                            decimal? lopDuration = (from lev in DB.EmpLeaveApplications
                                                    where lev.EmpId == EmpId
                                                       && lev.LeaveTypeId == 0
                                                       && lev.StartDate >= startDate
                                                       && lev.EndDate <= endDate
                                                       && lev.IsActive == true
                                                       && lev.IsDeleted == false
                                                    select lev.Duration)
                                                   .DefaultIfEmpty(0)           // avoid null result
                                                   .Sum();

                            decimal? workingdays = totalDays - lopDuration;

                            decimal? ELDuration = (from lev in DB.EmpLeaveApplications
                                                   join lty in DB.LeaveTypeMasters on lev.LeaveTypeId equals lty.LeaveTypeId
                                                   where lty.ShortName.ToUpper() == "EL" && lev.EmpId == EmpId
                                                      && lev.StartDate >= startDate
                                                      && lev.EndDate <= endDate
                                                      && lev.IsActive == true
                                                      && lev.IsDeleted == false
                                                      && lty.IsActive == true
                                                      && lty.IsDeleted == false
                                                   select lev.Duration)
                                                   .DefaultIfEmpty(0)           // avoid null result
                                                   .Sum();

                            decimal? CLDuration = (from lev in DB.EmpLeaveApplications
                                                   join lty in DB.LeaveTypeMasters on lev.LeaveTypeId equals lty.LeaveTypeId
                                                   where lty.ShortName.ToUpper() == "CL" && lev.EmpId == EmpId
                                                      && lev.StartDate >= startDate
                                                      && lev.EndDate <= endDate
                                                      && lev.IsActive == true
                                                      && lev.IsDeleted == false
                                                      && lty.IsActive == true
                                                      && lty.IsDeleted == false
                                                   select lev.Duration)
                                                   .DefaultIfEmpty(0)           // avoid null result
                                                   .Sum();

                            PayrollReportViewModel ltvm = new PayrollReportViewModel();
                            ltvm.LoginId = (int)loginId;
                            ltvm.Month = month;
                            ltvm.MonthNo = monthNo;
                            ltvm.Year = year;
                            ltvm.EmpId = empDetails[i].EmpId;
                            ltvm.EmpCode = empDetails[i].EmpCode;
                            ltvm.EmpName = empDetails[i].EmpName;
                            ltvm.CompId = empDetails[i].CompId;
                            ltvm.Company = empDetails[i].CompName;
                            ltvm.LEId = empDetails[i].LEId;
                            int? leid = empDetails[i].LEId;
                            ltvm.LegalEntity = DB.LegalEntityMasters.Where(x => x.LEId == leid && x.IsActive == true && x.IsDeleted == false).Select(x => x.LegalEntity).FirstOrDefault();
                            ltvm.BUId = empDetails[i].BUId;
                            int? buid = empDetails[i].BUId;
                            ltvm.BusinessUnit = DB.BusinessUnitMasters.Where(x => x.BUId == buid && x.IsActive == true && x.IsDeleted == false).Select(x => x.BusinessUnit).FirstOrDefault();
                            ltvm.LocationId = empDetails[i].LocationId;
                            int? locid = empDetails[i].LocationId;
                            ltvm.Location = DB.LocationMasters.Where(x => x.LocationId == locid && x.IsActive == true && x.IsDeleted == false).Select(x => x.Location).FirstOrDefault();
                            ltvm.DeptId = empDetails[i].CategoryId;
                            ltvm.Department = empDetails[i].DeptName;
                            ltvm.DesignationId = empDetails[i].DesignationId;
                            ltvm.Designation = empDetails[i].DesignationName;
                            ltvm.TotalDays = Convert.ToDecimal(totalDays);
                            ltvm.WorkingDays = Convert.ToDecimal(workingdays);
                            ltvm.PaidLeaveDaysEL = Convert.ToDecimal(ELDuration);
                            ltvm.PaidLeaveDaysCL = Convert.ToDecimal(CLDuration);
                            ltvm.LOPDays = Convert.ToDecimal(lopDuration);
                            ltvm.Arrears = Convert.ToDecimal(0);
                            decimal? ctc = empDetails[i].CTC;
                            decimal? perday = (ctc / totalDays);
                            decimal? lopday = (perday * lopDuration);
                            ltvm.LOPAmt = Convert.ToDecimal(lopday);
                            lstofpaytype.Add(ltvm);

                        }

                        return lstofpaytype;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payroll Report Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<DDLegalEntityPayrollViewModel> GetDDLegalEntity(DDLegalEntityPayrollViewModel model)
        {
            try
            {
                string msg = "";
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                //int? CompId = (model.CompId != 0) ? model.CompId : 0;

                var Legaldetails = (from le in DB.LegalEntityMasters
                                    where le.IsActive == true && le.IsDeleted == false
                                    select new DDLegalEntityPayrollViewModel
                                    {
                                        LEId = le.LEId,
                                        LegalEntity = le.LegalEntity,
                                    }).ToList();

                if (LoginId != 0)
                {
                    if (Legaldetails != null)
                    {
                        return Legaldetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Legal Entity Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<DDLocationPayrollViewModel> GetDDLocation(DDLocationPayrollViewModel model)
        {
            try
            {
                string msg = "";
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                string authorisedEntity = (model.AuthorisedEntity != null) ? model.AuthorisedEntity : null;

                var authorisedEntities = model.AuthorisedEntity?
                                            .Split(',')
                                            .Select(x => int.Parse(x.Trim()))
                                            .ToList();

                var Locationdetails = (from lm in DB.LocationMasters
                                       where lm.LEId.HasValue
                                            && authorisedEntities.Contains(lm.LEId.Value)
                                          && lm.IsActive == true && lm.IsDeleted == false
                                       select new DDLocationPayrollViewModel
                                       {
                                           LocationId = lm.LocationId,
                                           Location = lm.Location,
                                       }).ToList();

                if (LoginId != 0)
                {
                    if (Locationdetails != null)
                    {
                        return Locationdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Location Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<PayrollVariableViewModel> GetAllPayrollVariable(PayrollVariableViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;

                var PVdetails = (from fin in DB.PayrollVariables
                                 where fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (PVdetails != null)
                    {
                        List<PayrollVariableViewModel> lstofPV = new List<PayrollVariableViewModel>();

                        for (int i = 0; i < PVdetails.Count(); i++)
                        {
                            PayrollVariableViewModel pvvm = new PayrollVariableViewModel();
                            pvvm.LoginId = loginId;
                            pvvm.VariableId = PVdetails[i].VariableId;
                            pvvm.VariableName = PVdetails[i].VariableName;
                            pvvm.VariableCode = PVdetails[i].VariableCode;
                            pvvm.Status = PVdetails[i].Status;
                            pvvm.CreatedBy = PVdetails[i].CreatedBy;
                            pvvm.CreatedDate = PVdetails[i].CreatedDate;
                            pvvm.LastUpdatedBy = PVdetails[i].LastUpdatedBy;
                            pvvm.LastUpdatedDate = PVdetails[i].LastUpdatedDate;
                            pvvm.IsActive = PVdetails[i].IsActive;
                            pvvm.IsUpdated = PVdetails[i].IsUpdated;
                            pvvm.IsDeleted = PVdetails[i].IsDeleted;
                            lstofPV.Add(pvvm);
                        }

                        return lstofPV;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payroll Variable Detail Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public PayrollVariableViewModel GetPayrollVariable(PayrollVariableViewModel model)
        {
            try
            {

                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? Id = (model.VariableId != 0) ? model.VariableId : 0;

                var PVdetails = (from fin in DB.PayrollVariables
                                 where fin.VariableId == Id && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (PVdetails != null)
                    {
                        PayrollVariableViewModel pvvm = new PayrollVariableViewModel();
                        pvvm.LoginId = loginId;
                        pvvm.VariableId = PVdetails.VariableId;
                        pvvm.VariableName = PVdetails.VariableName;
                        pvvm.VariableCode = PVdetails.VariableCode;
                        pvvm.Status = PVdetails.Status;
                        pvvm.CreatedBy = PVdetails.CreatedBy;
                        pvvm.CreatedDate = PVdetails.CreatedDate;
                        pvvm.LastUpdatedBy = PVdetails.LastUpdatedBy;
                        pvvm.LastUpdatedDate = PVdetails.LastUpdatedDate;
                        pvvm.IsActive = PVdetails.IsActive;
                        pvvm.IsUpdated = PVdetails.IsUpdated;
                        pvvm.IsDeleted = PVdetails.IsDeleted;
                        return pvvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payroll Variable Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel AddPayrollVariable(PayrollVariableViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                string VariableCode = (model.VariableCode != "") ? model.VariableCode : "";

                var FyDetails = (from fin in DB.PayrollVariables
                                 where fin.VariableCode == VariableCode && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).ToList();

                if (loginId != 0)
                {
                    if (FyDetails.Count() == 0)
                    {
                        PayrollVariable pv = new PayrollVariable();
                        //fm.LoginId = loginId;
                        pv.VariableName = model.VariableName;
                        pv.VariableCode = model.VariableCode;
                        pv.Status = true;
                        pv.CreatedBy = model.LoginId;
                        pv.CreatedDate = DateTime.Now;
                        pv.LastUpdatedBy = model.LoginId;
                        pv.LastUpdatedDate = DateTime.Now;
                        pv.IsActive = true;
                        pv.IsUpdated = false;
                        pv.IsDeleted = false;
                        DB.PayrollVariables.Add(pv);
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Added";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payroll Variable Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel UpdatePayrollVariable(PayrollVariableViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? Id = (model.VariableId != 0) ? model.VariableId : 0;

                var FyDetails = (from fin in DB.PayrollVariables
                                 where fin.VariableId == Id && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (FyDetails != null)
                    {
                        FyDetails.VariableName = model.VariableName;
                        FyDetails.VariableCode = model.VariableCode;
                        FyDetails.Status = true;
                        FyDetails.LastUpdatedBy = model.LoginId;
                        FyDetails.LastUpdatedDate = DateTime.Now;
                        FyDetails.IsUpdated = true;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Updated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payroll Variable Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel DeletePayrollVariable(PayrollVariableViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? Id = (model.VariableId != 0) ? model.VariableId : 0;

                var FyDetails = (from fin in DB.PayrollVariables
                                 where fin.VariableId == Id && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (FyDetails != null)
                    {
                        FyDetails.IsUpdated = true;
                        FyDetails.IsDeleted = true;
                        FyDetails.LastUpdatedBy = model.LoginId;
                        FyDetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Deleted";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payroll Variable Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<DDPayrollVariableViewModel> GetDDPayrollVariable(DDPayrollVariableViewModel model)
        {
            try
            {
                string msg = "";
                int? LoginId = (model.LoginId != 0) ? model.LoginId : 0;
                string authorisedEntity = (model.AuthorisedEntity != null) ? model.AuthorisedEntity : null;

                var authorisedEntities = model.AuthorisedEntity?
                                            .Split(',')
                                            .Select(x => int.Parse(x.Trim()))
                                            .ToList();

                var Variabledetails = (from lm in DB.PayrollVariables
                                       where lm.IsActive == true && lm.IsDeleted == false && lm.Status == true
                                       select new DDPayrollVariableViewModel
                                       {
                                           VariableId = lm.VariableId,
                                           VariableName = lm.VariableName,
                                           VariableCode = lm.VariableCode,
                                       }).ToList();

                if (LoginId != 0)
                {
                    if (Variabledetails != null)
                    {
                        return Variabledetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payroll Variable Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<VariableHistoryViewModel> PayrollVariableHistory(VariableHistoryViewModel model)
        {
            try
            {
                int? loginId = (model.LoginId != 0) ? model.LoginId : null;
                int? CompId = (model.CompId != 0) ? model.CompId : null;
                int? LEId = (model.LEId != 0) ? model.LEId : null;
                int? BUId = (model.BUId != 0) ? model.BUId : null;
                int? LocationId = (model.LocationId != 0) ? model.LocationId : null;
                int? DeptId = (model.DeptId != 0) ? model.DeptId : null;
                int? DesignationId = (model.DesignationId != 0) ? model.DesignationId : null;
                int? ReporterId = (model.ReporterId != 0) ? model.ReporterId : null;
                int? EmpId = (model.EmpId != 0) ? model.EmpId : null;
                int? Year = (model.Year != 0) ? model.Year : null;
                int? Month = (model.Month != 0) ? model.Month : null;

                // Validate LoginId
                if (loginId == null || loginId == 0)
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");

                // Get logged-in employee
                var empdetails = DB.EmployeeMasters
                    .FirstOrDefault(emp => emp.EmpId == loginId &&
                                           emp.IsActive == true &&
                                           emp.IsDeleted == false &&
                                           emp.EmpStatus == "ACTIVE");

                if (empdetails == null)
                    throw new CustomApiException(HttpStatusCode.NotFound,
                        $"Employee with ID {loginId} not found or is not active");

                int? logindeptId = empdetails.CategoryId;
                const int HR_DEPT_ID = 1;

                // Build query with joins
                var query = from fin in DB.VariableHistories
                            join emp in DB.EmployeeMasters on fin.EmpId equals emp.EmpId
                            where fin.IsActive == true && fin.IsDeleted == false && fin.Status == true &&
                                  emp.IsActive == true && emp.IsDeleted == false
                            select new { VariableHistory = fin, Employee = emp };

                // Apply filters - using the nullable variables
                if (CompId.HasValue)
                    query = query.Where(x => x.Employee.CompId == CompId.Value);

                if (LEId.HasValue)
                    query = query.Where(x => x.Employee.LEId == LEId.Value);

                if (BUId.HasValue)
                    query = query.Where(x => x.Employee.BUId == BUId.Value);

                if (LocationId.HasValue)
                    query = query.Where(x => x.Employee.LocationId == LocationId.Value);

                if (ReporterId.HasValue)
                {
                    query = query.Where(x => x.Employee.ReportId == ReporterId.Value);
                }
                else
                {
                    if (DeptId.HasValue)
                        query = query.Where(x => x.Employee.CategoryId == DeptId.Value);

                    if (DesignationId.HasValue)
                        query = query.Where(x => x.Employee.DesignationId == DesignationId.Value);
                }

                if (Year.HasValue)
                    query = query.Where(x => x.VariableHistory.Year == Year.Value);

                if (Month.HasValue)
                    query = query.Where(x => x.VariableHistory.Month == Month.Value);

                if (EmpId.HasValue)
                    query = query.Where(x => x.VariableHistory.EmpId == EmpId.Value);

                // Role-based filter
                if (logindeptId != HR_DEPT_ID)
                    query = query.Where(x => x.Employee.ReportId == loginId);

                // Execute query and order results
                var allRecords = query
                    .OrderByDescending(x => x.VariableHistory.CreatedDate)
                    .ToList();

                if (!allRecords.Any())
                    throw new CustomApiException(HttpStatusCode.NotFound, "Payroll Variable Detail Not Found");

                // Map results to ViewModel
                List<VariableHistoryViewModel> lstofPV = new List<VariableHistoryViewModel>();

                foreach (var record in allRecords)
                {
                    VariableHistoryViewModel pvvm = new VariableHistoryViewModel
                    {
                        LoginId = loginId.Value,
                        CompId = record.Employee.CompId,
                        LEId = record.Employee.LEId,
                        BUId = record.Employee.BUId,
                        LocationId = record.Employee.LocationId,
                        DeptId = record.Employee.CategoryId,
                        DesignationId = record.Employee.DesignationId,
                        ReporterId = record.Employee.ReportId,
                        VariableHistoryId = record.VariableHistory.VariableHistoryId,
                        EmpId = record.VariableHistory.EmpId,
                        EmpCode = record.VariableHistory.EmpCode,
                        EmpName = record.Employee.FirstName + " " + record.Employee.MiddleName + " " + record.Employee.LastName,
                        EmpCTC = record.VariableHistory.EmpCTC,
                        VariableId = record.VariableHistory.VariableId,
                        VariableName = record.VariableHistory.VariableName,
                        VariableCode = record.VariableHistory.VariableCode,
                        VariableAmt = record.VariableHistory.VariableAmt,
                        Year = record.VariableHistory.Year,
                        Month = record.VariableHistory.Month,
                        Status = record.VariableHistory.Status,
                        CreatedBy = record.VariableHistory.CreatedBy,
                        CreatedDate = record.VariableHistory.CreatedDate,
                        LastUpdatedBy = record.VariableHistory.LastUpdatedBy,
                        LastUpdatedDate = record.VariableHistory.LastUpdatedDate,
                        IsActive = record.VariableHistory.IsActive,
                        IsUpdated = record.VariableHistory.IsUpdated,
                        IsDeleted = record.VariableHistory.IsDeleted
                    };
                    lstofPV.Add(pvvm);
                }

                return lstofPV;
            }
            catch (CustomApiException)
            {
                throw; // Re-throw CustomApiException as-is
            }
            catch (Exception ex)
            {
                // Log the exception here if you have logging
                throw new CustomApiException(HttpStatusCode.InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }
        public BusinessEntityResponseViewModel AddPayrollVariableHistory(VariableHistoryViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;
                int? year = (model.Year != 0) ? model.Year : 0;
                int? month = (model.Month != 0) ? model.Month : 0;

                var FyDetails = (from fin in DB.VariableHistories
                                 where fin.EmpId == empId && fin.Year == year && fin.Month == month
                                 && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).ToList();

                var empctcdetails = (from fin in DB.EmployeeSalaryDetails
                                     where fin.EmpId == empId && fin.RecordStatus == true
                                     && fin.IsActive == true && fin.IsDeleted == false 
                                     select fin).OrderByDescending(x => x.SalaryId).FirstOrDefault();

                if (empctcdetails == null)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Employee salary details not found.Contact HR department.");
                }

                if (loginId != 0)
                {
                    if (FyDetails.Count() == 0)
                    {
                        VariableHistory vh = new VariableHistory();
                        //fm.LoginId = loginId;
                        vh.EmpId = model.EmpId;
                        vh.EmpCode = model.EmpCode;
                        vh.EmpCTC = Convert.ToString(empctcdetails.CTC);
                        vh.VariableId = model.VariableId;
                        vh.VariableName = model.VariableName;
                        vh.VariableCode = model.VariableCode;
                        vh.VariableAmt = model.VariableAmt;
                        vh.Year = model.Year;
                        vh.Month = model.Month;
                        vh.Status = true;
                        vh.CreatedBy = model.LoginId;
                        vh.CreatedDate = DateTime.Now;
                        vh.LastUpdatedBy = model.LoginId;
                        vh.LastUpdatedDate = DateTime.Now;
                        vh.IsActive = true;
                        vh.IsUpdated = false;
                        vh.IsDeleted = false;
                        DB.VariableHistories.Add(vh);
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Added";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payroll Variable Details Already Exists");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel UpdatePayrollVariableHistory(VariableHistoryViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? Id = (model.VariableHistoryId != 0) ? model.VariableHistoryId : 0;
                int? empId = (model.EmpId != 0) ? model.EmpId : 0;

                var FyDetails = (from fin in DB.VariableHistories
                                 where fin.VariableHistoryId == Id && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                var empctcdetails = (from fin in DB.EmployeeSalaryDetails
                                     where fin.EmpId == empId && fin.RecordStatus == true
                                     && fin.IsActive == true && fin.IsDeleted == false
                                     select fin).OrderByDescending(x => x.SalaryId).FirstOrDefault();

                if (empctcdetails == null)
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Employee salary details not found.Contact HR department.");
                }


                if (loginId != 0)
                {
                    if (FyDetails != null)
                    {
                        FyDetails.EmpId = model.EmpId;
                        FyDetails.EmpCode = model.EmpCode;
                        FyDetails.EmpCTC = Convert.ToString(empctcdetails.CTC);
                        FyDetails.VariableId = model.VariableId;
                        FyDetails.VariableName = model.VariableName;
                        FyDetails.VariableCode = model.VariableCode;
                        FyDetails.VariableAmt = model.VariableAmt;
                        FyDetails.Year = model.Year;
                        FyDetails.Month = model.Month;
                        FyDetails.Status = true;
                        FyDetails.LastUpdatedBy = model.LoginId;
                        FyDetails.LastUpdatedDate = DateTime.Now;
                        FyDetails.IsUpdated = true;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Updated";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payroll Variable Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public BusinessEntityResponseViewModel DeletePayrollVariableHistory(VariableHistoryViewModel model)
        {
            try
            {
                string msg = "";
                int? loginId = (model.LoginId != 0) ? model.LoginId : 0;
                int? Id = (model.VariableHistoryId != 0) ? model.VariableHistoryId : 0;
                //int? empId = (model.EmpId != 0) ? model.EmpId : 0;

                var FyDetails = (from fin in DB.VariableHistories
                                 where fin.VariableHistoryId == Id && fin.IsActive == true && fin.IsDeleted == false && fin.Status == true
                                 select fin).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (loginId != 0)
                {
                    if (FyDetails != null)
                    {
                        FyDetails.IsUpdated = true;
                        FyDetails.IsDeleted = true;
                        FyDetails.LastUpdatedBy = model.LoginId;
                        FyDetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        BusinessEntityResponseViewModel bervm = new BusinessEntityResponseViewModel();
                        bervm.Status = 200;
                        bervm.msg = "Deleted";

                        return bervm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Payroll Variable Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "LoginId is Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
    }
}