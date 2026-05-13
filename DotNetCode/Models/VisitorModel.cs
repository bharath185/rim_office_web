using OfficeConnect_Web.Controllers;
using OfficeConnect_Web.ViewModel;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace OfficeConnect_Web.Models
{
    public class VisitorModel
    {
        DB_Offc_ConEntities DB = new DB_Offc_ConEntities();

        public FileUploadAPIViewModel UploadFileVisitor(FileUploadAPIViewModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new CustomApiException(HttpStatusCode.BadRequest, "No file uploaded");
                }
                var path = "~/Uploads/Images/Visitor";
                var httpRequest = HttpContext.Current.Request;
                var docfiles = new List<string>();
                var uploadDir = HttpContext.Current.Server.MapPath("~/Uploads/Images/Visitor");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        var docName = Path.GetFileName(postedFile.FileName);
                        var extension = Path.GetExtension(docName).ToLower();
                        var ImgName = "VISITOR_" + model.Visitor.ToUpper() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        path = Path.Combine(uploadDir, ImgName);

                        postedFile.SaveAs(path);
                        docfiles.Add(path);
                    }
                }

                FileUploadAPIViewModel dmvm = new FileUploadAPIViewModel();
                dmvm.msg = "Visitor Picture Uploaded";
                dmvm.path = path;

                return dmvm;
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public VisitorManagementViewModel InviteVisit(VisitorManagementViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                if (EmpId != 0)
                {
                    VisitorManagement ivm = new VisitorManagement();
                    ivm.RegNo = (model.RegNo != "" && model.RegNo != null) ? model.RegNo : "";
                    ivm.QR = (model.QR != "" && model.QR != null) ? model.QR : "";
                    ivm.Name = model.Name;
                    ivm.Designation = (model.Designation != "" && model.Designation != null) ? model.Designation : "";
                    ivm.Company = (model.Company != "" && model.Company != null) ? model.Company : "";
                    ivm.Purpose = (model.Purpose != "" && model.Purpose != null) ? model.Purpose : "";
                    ivm.PMail = (model.PMail != "" && model.PMail != null) ? model.PMail : "";
                    ivm.OMail = (model.OMail != "" && model.OMail != null) ? model.OMail : "";
                    ivm.Mobile = (model.Mobile != "" && model.Mobile != null) ? model.Mobile : "";
                    ivm.AMobile = (model.AMobile != "" && model.AMobile != null) ? model.AMobile : "";
                    ivm.Photo = (model.Photo != "" && model.Photo != null) ? model.Photo : "";
                    ivm.CompId = (model.CompId != "") ? model.CompId : "";
                    ivm.WhomtoMeet = (model.WhomtoMeet != 0) ? model.WhomtoMeet : 0;
                    ivm.Date = model.Date;
                    ivm.Time = model.Time;
                    ivm.Invited = true;
                    ivm.Accept = false;
                    ivm.Approved = false;
                    ivm.Expired = false;
                    ivm.Accessories = "";
                    //ivm.CheckIn = model.CheckIn;
                    //ivm.CheckOut = model.CheckOut;
                    ivm.DirectCheckIn = false;
                    ivm.IdCard = "";
                    ivm.IsActive = true;
                    ivm.IsUpdated = false;
                    ivm.IsDeleted = false;
                    ivm.CreatedBy = EmpId;
                    ivm.CreatedDate = DateTime.Now;
                    ivm.LastUpdatedBy = EmpId;
                    ivm.LastUpdatedDate = DateTime.Now;
                    DB.VisitorManagements.Add(ivm);
                    DB.SaveChanges();
                    int visitorid = ivm.VisitId;  // Pending - Add Reg No, CompId, QR

                    string InviteCode = GenerateSecureOTP();

                    byte[] QRCode = GenerateQRCode("REG3DCADV0001");


                    //string link = "http://newofficeconnect.rim-global.com/UAT_OfficeConnect/#/verify_otp";
                    string link = DB.ServiceMasters.Where(x => x.ServiceName == "VisitorLink").Select(x => x.ServiceLink).FirstOrDefault();
                    string subject = "Invitation to Visit 3DCAD";
                    //                    string body = "Dear " + model.Name + ", " + Environment.NewLine + Environment.NewLine;
                    //                    body += "       We are pleased to invite you to visit 3DCAD. We believe this visit will provide you with valuable insights into our operations and allow us to explore potential collaborations." + Environment.NewLine + Environment.NewLine;
                    //                    body += "To facilitate your visit, please fill out the form at the following link: " + link + "." + Environment.NewLine;
                    //                    body += "**Invitation Code: ** " + InviteCode + Environment.NewLine + Environment.NewLine;
                    //                    body += "This will help us ensure a smooth and productive experience for you." + Environment.NewLine + Environment.NewLine;
                    //                    body += "We look forward to welcoming you to 3DCAD." + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                    //                    body += @"
                    //<p>Dear model.Name  </p>
                    //        < p>Please find the QR code below:</p>
                    //        <img src='cid:QRCodeImage' alt='QR Code' style='width:50px; height:50px;' />";
                    //                    body += "Best regards," + Environment.NewLine + "3DCAD";


                    string body = $@"
                                <p>Dear {model.Name},</p>
                                <p>We are pleased to invite you to visit 3DCAD. We believe this visit will provide you with valuable insights into our operations and allow us to explore potential collaborations.</p>
                                <p>To facilitate your visit, please fill out the form at the following link: <a href='{link}'>InviteLink</a>.</p>
                                <p><strong>Invitation Code: </strong> {InviteCode}</p>
                                <p>This will help us ensure a smooth and productive experience for you.</p>
                                <p>We look forward to welcoming you to 3DCAD.</p>
                                <p>Please find the QR code below:</p>
                                <img src='cid:QRCodeImage' alt='QR Code' style='width:50px; height:50px;' />
                                <p>Best regards,</p>
                                <p>3DCAD</p>";

                    
                    //SendEmail(model.OMail, subject, body);

                    VisitorInviteHistory vih = new VisitorInviteHistory();
                    vih.VisitorId = visitorid;
                    vih.InviteCode = InviteCode;
                    vih.CheckInCode = "";
                    vih.Mail = true;
                    vih.Mobile = false;
                    vih.CheckIn = false;
                    vih.CheckOut = false;
                    vih.IsActive = true;
                    vih.IsUpdated = false;
                    vih.IsDeleted = false;
                    vih.CreatedBy = EmpId;
                    vih.CreatedDate = DateTime.Now;
                    vih.LastUpdatedBy = EmpId;
                    vih.LastUpdatedDate = DateTime.Now;
                    DB.VisitorInviteHistories.Add(vih);
                    DB.SaveChanges();

                    VisitorManagementViewModel ivmvm = new VisitorManagementViewModel();
                    ivmvm.msg = "Invite Created";
                    ivmvm.InviteCode = InviteCode;

                    // Fire-and-forget email sending task
                    Task.Run(() => SendEmailWithQRCode(model.OMail, subject, body, QRCode, model.Date, model.Time));
                    //SendEmailWithQRCode(model.OMail, subject, body, QRCode);

                    return ivmvm;
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Mismatching");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public VisitorManagementViewModel VerifyOTP(VisitorManagementViewModel model)
        {
            try
            {
                string msg = "";
                string OTP = (model.otp != "" || model.otp != null) ? model.otp : "";
                DateTime tdydate = DateTime.Now;

                var Visitordetails = (from vih in DB.VisitorInviteHistories
                                      join vm in DB.VisitorManagements on vih.VisitorId equals vm.VisitId
                                      where vih.InviteCode == OTP && vm.IsDeleted == false && vm.Invited == true && vm.Accept == false && vm.Expired == false
                                      select vm).FirstOrDefault();


                if (OTP != "")
                {
                    if (Visitordetails != null)
                    {
                        VisitorManagementViewModel vmvm = new VisitorManagementViewModel();
                        vmvm.RegNo = Visitordetails.RegNo;
                        vmvm.QR = Visitordetails.QR;
                        vmvm.VisitId = Visitordetails.VisitId;
                        vmvm.Name = Visitordetails.Name;
                        vmvm.Designation = Visitordetails.Designation;
                        vmvm.Company = Visitordetails.Company;
                        vmvm.Purpose = Visitordetails.Purpose;
                        vmvm.PMail = Visitordetails.PMail;
                        vmvm.OMail = Visitordetails.OMail;
                        vmvm.Mobile = Visitordetails.Mobile;
                        vmvm.AMobile = Visitordetails.AMobile;
                        vmvm.Photo = Visitordetails.Photo;

                        if (vmvm.Photo != "")
                        {
                            string[] stringSeparators = new string[] { "Uploads" };
                            string[] firstNames = vmvm.Photo.Split(stringSeparators, StringSplitOptions.None);
                            string lnkval = firstNames[1];
                            vmvm.Photo = "Uploads" + lnkval;
                        }

                        vmvm.CompId = Visitordetails.CompId;
                        vmvm.CompName = Visitordetails.CompId;
                        vmvm.Accessories = Visitordetails.Accessories;
                        vmvm.WhomtoMeet = Visitordetails.WhomtoMeet;
                        vmvm.WName = DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.FirstName).FirstOrDefault() +
                            DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.MiddleName).FirstOrDefault() +
                            DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.LastName).FirstOrDefault();
                        vmvm.WEmpCode = DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.UserName).FirstOrDefault();
                        vmvm.Date = Visitordetails.Date;
                        vmvm.Time = Visitordetails.Time;
                        vmvm.Invited = Visitordetails.Invited;
                        vmvm.Accept = Visitordetails.Accept;
                        vmvm.Approved = Visitordetails.Approved;
                        vmvm.Expired = Visitordetails.Expired;
                        vmvm.DirectCheckIn = Visitordetails.DirectCheckIn;
                        vmvm.IdCard = Visitordetails.IdCard;
                        vmvm.CheckIn = Visitordetails.CheckIn;
                        vmvm.CheckOut = Visitordetails.CheckOut;
                        vmvm.VisitorCheckIn = DB.VisitorInviteHistories.Where(x => x.InviteCode == OTP).Select(x => x.CheckIn).FirstOrDefault();
                        vmvm.VisitorCheckOut = DB.VisitorInviteHistories.Where(x => x.InviteCode == OTP).Select(x => x.CheckOut).FirstOrDefault();
                        vmvm.CreatedBy = Visitordetails.CreatedBy;
                        vmvm.CreatedDate = Visitordetails.CreatedDate;
                        vmvm.LastUpdatedBy = Visitordetails.LastUpdatedBy;
                        vmvm.LastUpdatedDate = Visitordetails.LastUpdatedDate;
                        vmvm.IsActive = Visitordetails.IsActive;
                        vmvm.IsUpdated = Visitordetails.IsUpdated;
                        vmvm.IsDeleted = Visitordetails.IsDeleted;
                        vmvm.InviteCode = OTP;
                        vmvm.msg = "OTP Verified Successfully";

                        return vmvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "OTP Invalid");
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
        public VisitorManagementViewModel AcceptInvite(VisitorManagementViewModel model)
        {
            try
            {
                string msg = "";
                int? VisitId = (model.VisitId != 0) ? model.VisitId : 0;
                string EmpMail = "", EmpName = "", VisitorMail = "", VisitorName = "", date = "", time = "", locationMap = "";
                int ecount = 0, vcount = 0;

                var Visitordetails = (from vm in DB.VisitorManagements
                                      where vm.VisitId == VisitId && vm.IsDeleted == false && vm.Invited == true && vm.Accept == false && vm.Expired == false
                                      select vm).FirstOrDefault();

                if (Visitordetails != null)
                {
                    Visitordetails.RegNo = (model.RegNo != "" && model.RegNo != null) ? model.RegNo : "";
                    Visitordetails.QR = (model.QR != "" && model.QR != null) ? model.QR : "";
                    Visitordetails.Name = model.Name;
                    Visitordetails.Designation = (model.Designation != "" && model.Designation != null) ? model.Designation : "";
                    Visitordetails.Company = (model.Company != "" && model.Company != null) ? model.Company : "";
                    Visitordetails.Purpose = (model.Purpose != "" && model.Purpose != null) ? model.Purpose : "";
                    Visitordetails.PMail = (model.PMail != "" && model.PMail != null) ? model.PMail : "";
                    Visitordetails.OMail = (model.OMail != "" && model.OMail != null) ? model.OMail : "";
                    Visitordetails.Mobile = (model.Mobile != "" && model.Mobile != null) ? model.Mobile : "";
                    Visitordetails.AMobile = (model.AMobile != "" && model.AMobile != null) ? model.AMobile : "";
                    Visitordetails.Photo = (model.Photo != "" && model.Photo != null) ? model.Photo : "";

                    if (Visitordetails.Photo != "")
                    {
                        string[] stringSeparators = new string[] { "Uploads" };
                        string[] firstNames = Visitordetails.Photo.Split(stringSeparators, StringSplitOptions.None);
                        string lnkval = firstNames[1];
                        Visitordetails.Photo = "Uploads" + lnkval;
                    }

                    Visitordetails.CompId = (model.CompId != "") ? model.CompId : "";
                    Visitordetails.WhomtoMeet = (model.WhomtoMeet != 0) ? model.WhomtoMeet : 0;
                    Visitordetails.Date = model.Date;
                    Visitordetails.Time = model.Time;
                    Visitordetails.Invited = true;
                    Visitordetails.Accept = true;
                    Visitordetails.Approved = false;
                    Visitordetails.Expired = false;
                    Visitordetails.Accessories = "";
                    Visitordetails.DirectCheckIn = false;
                    Visitordetails.IdCard = "";
                    //Visitordetails.CheckIn = model.CheckIn;
                    //Visitordetails.CheckOut = model.CheckOut;
                    Visitordetails.IsActive = true;
                    Visitordetails.IsUpdated = false;
                    Visitordetails.IsDeleted = false;
                    Visitordetails.LastUpdatedBy = VisitId;
                    Visitordetails.LastUpdatedDate = DateTime.Now;
                    DB.SaveChanges();

                    string InviteCode = GenerateSecureOTP();

                    var VHistorydetails = (from vh in DB.VisitorInviteHistories
                                          where vh.VisitorId == VisitId && vh.IsActive == true && vh.IsDeleted == false && vh.CheckInCode == ""
                                          select vh).FirstOrDefault();

                    if (VHistorydetails != null)
                    {
                        VHistorydetails.CheckInCode = InviteCode;
                        VHistorydetails.CheckIn = false;
                        VHistorydetails.CheckOut = false;
                        VHistorydetails.IsUpdated = true;
                        VHistorydetails.LastUpdatedBy = VisitId;
                        VHistorydetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();
                    }

                    EmpMail = DB.EmployeeMasters.Where(x => x.EmpId == model.WhomtoMeet).Select(x => x.EmailId).FirstOrDefault();
                    EmpName = DB.EmployeeMasters.Where(x => x.EmpId == model.WhomtoMeet).Select(x => x.FirstName).FirstOrDefault();
                    VisitorMail = Visitordetails.OMail;
                    VisitorName = Visitordetails.Name;
                    DateTime date1 = Convert.ToDateTime(model.Date);
                    date = date1.ToString("dd-MM-yyyy");
                    time = Convert.ToString(model.Time);
                    string input = model.CompId;
                    string[] parts = input.Split(new string[] { " - " }, StringSplitOptions.None);
                    string loc = parts[1];

                    locationMap = DB.LocationMasters.Where(x => x.Location.ToUpper() == loc.ToUpper()).Select(x => x.LocationMap).FirstOrDefault();
                    if (locationMap == null)
                    {
                        locationMap = "https://www.google.com/maps/place/RIM+INDIA+PVT+LTD/@12.9765095,77.5295906,15z/data=!4m2!3m1!1s0x0:0x255305137c820daa?sa=X&ved=1t:2428&ictx=111";
                    }

                    if (EmpMail != "")
                    {
                        // Fire-and-forget email sending task
                        Task.Run(() => SendEmailEmp(EmpMail, EmpName, VisitorName, InviteCode, date, time, locationMap));
                        //SendEmailEmp(EmpMail, EmpName, VisitorName, InviteCode, date, time, locationMap);
                        ecount = 1;
                    }
                    if (VisitorMail != "")
                    {
                        // Fire-and-forget email sending task
                        Task.Run(() => SendEmailVisitor(VisitorMail, VisitorName, InviteCode, date, time, locationMap));
                        //SendEmailVisitor(VisitorMail, VisitorName, InviteCode, date, time, locationMap);
                        vcount = 1;
                    }

                    VisitorManagementViewModel ivmvm = new VisitorManagementViewModel();
                    if (ecount == 1 && vcount == 1)
                    {
                        ivmvm.msg = "Invite Accepted by Visitor";
                        ivmvm.Name = model.Name;
                        ivmvm.InviteCode = InviteCode;
                    }
                    else if (ecount == 0 && vcount == 1)
                    {
                        ivmvm.msg = "Employee mail is not send";
                        ivmvm.Name = model.Name;
                        ivmvm.InviteCode = InviteCode;
                    }
                    else if (ecount == 0 && vcount == 1)
                    {
                        ivmvm.msg = "Visitor mail is not send";
                        ivmvm.Name = model.Name;
                        ivmvm.InviteCode = InviteCode;
                    }
                    else
                    {
                        ivmvm.msg = "Confirmation mail is not send";
                        ivmvm.Name = model.Name;
                        ivmvm.InviteCode = InviteCode;
                    }

                    return ivmvm;
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Visitor Details Not Found");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<VisitorManagementViewModel> GetAllInvite(VisitorManagementViewModel model)
        {
            try
            {
                Task.Run(() => ExpireInvites());
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var today = DateTime.Today;

                var Visitordetails = (from vm in DB.VisitorManagements
                                      where vm.IsDeleted == false
                                      select vm).OrderByDescending(vm => vm.Date >= today && vm.Date <= today)  // Fetch today's records first
                                                  .ThenByDescending(vm => vm.Date)  // Order other dates after today
                                                  .ThenByDescending(vm => vm.VisitId)  // Sort by VisitId for records on the same date
                                                  .ToList();

                if (EmpId != 0)
                {
                    if (Visitordetails != null)
                    {
                        List<VisitorManagementViewModel> lstofVisitors = new List<VisitorManagementViewModel>();

                        for (int i = 0; i < Visitordetails.Count(); i++)
                        {
                            VisitorManagementViewModel vmvm = new VisitorManagementViewModel();
                            vmvm.RegNo = Visitordetails[i].RegNo;
                            vmvm.QR = Visitordetails[i].QR;
                            vmvm.VisitId = Visitordetails[i].VisitId;
                            vmvm.Name = Visitordetails[i].Name;
                            vmvm.Designation = Visitordetails[i].Designation;
                            vmvm.Company = Visitordetails[i].Company;
                            vmvm.Purpose = Visitordetails[i].Purpose;
                            vmvm.PMail = Visitordetails[i].PMail;
                            vmvm.OMail = Visitordetails[i].OMail;
                            vmvm.Mobile = Visitordetails[i].Mobile;
                            vmvm.AMobile = Visitordetails[i].AMobile;
                            vmvm.Photo = Visitordetails[i].Photo;

                            if (vmvm.Photo != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = vmvm.Photo.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                vmvm.Photo = "Uploads" + lnkval;
                            }

                            vmvm.CompId = Visitordetails[i].CompId;
                            vmvm.CompName = Visitordetails[i].CompId;
                            vmvm.Accessories = Visitordetails[i].Accessories;
                            vmvm.WhomtoMeet = Visitordetails[i].WhomtoMeet;
                            vmvm.WName = DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.FirstName).FirstOrDefault() +
                                DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.MiddleName).FirstOrDefault() +
                                DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.LastName).FirstOrDefault();
                            vmvm.WEmpCode = DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.UserName).FirstOrDefault();
                            vmvm.Date = Visitordetails[i].Date;
                            vmvm.Time = Visitordetails[i].Time;
                            vmvm.Invited = Visitordetails[i].Invited;
                            vmvm.Accept = Visitordetails[i].Accept;
                            vmvm.Approved = Visitordetails[i].Approved;
                            vmvm.Expired = Visitordetails[i].Expired;
                            vmvm.DirectCheckIn = Visitordetails[i].DirectCheckIn;
                            vmvm.CheckIn = Visitordetails[i].CheckIn;
                            vmvm.CheckOut = Visitordetails[i].CheckOut;
                            vmvm.IdCard = Visitordetails[i].IdCard;
                            vmvm.VisitorCheckOut = DB.VisitorInviteHistories.Where(x => x.VisitorId == vmvm.VisitId).Select(x => x.CheckIn).FirstOrDefault();
                            vmvm.VisitorCheckOut = DB.VisitorInviteHistories.Where(x => x.VisitorId == vmvm.VisitId).Select(x => x.CheckOut).FirstOrDefault();
                            vmvm.CreatedBy = Visitordetails[i].CreatedBy;
                            vmvm.CreatedDate = Visitordetails[i].CreatedDate;
                            vmvm.LastUpdatedBy = Visitordetails[i].LastUpdatedBy;
                            vmvm.LastUpdatedDate = Visitordetails[i].LastUpdatedDate;
                            vmvm.IsActive = Visitordetails[i].IsActive;
                            vmvm.IsUpdated = Visitordetails[i].IsUpdated;
                            vmvm.IsDeleted = Visitordetails[i].IsDeleted;
                            lstofVisitors.Add(vmvm);

                        }
                        return lstofVisitors;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Invite Details Not Found");
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
        public void ExpireInvites()
        {
            try
            {
                var date = DateTime.Today;
                DateTime today = date;

                //var Visitordetails = (from vm in DB.VisitorManagements
                //                      where vm.IsDeleted == false && vm.Date < today && 
                //                      (vm.Invited == true && vm.Accept == false && vm.Approved == false) || (vm.Invited == true && vm.Accept == true && vm.Approved == false) ||
                //                      (vm.CheckIn < today && vm.CheckOut == null)
                //                      select vm).ToList();

                using (var context = new DB_Offc_ConEntities())
                {
                    var Visitordetails = (from vm in context.VisitorManagements
                                          where vm.IsDeleted == false && vm.Date < today && vm.Expired == false &&
                                          vm.Invited == true && vm.Accept == false && vm.Approved == false 
                                          select vm).ToList();

                    var Visitordetails1 = (from vm in context.VisitorManagements
                                           where vm.IsDeleted == false && vm.Date < today && vm.Expired == false &&
                                           vm.Invited == true && vm.Accept == true && vm.Approved == false
                                           select vm).ToList();

                    var Visitordetails2 = (from vm in context.VisitorManagements
                                           where vm.IsDeleted == false && vm.Date < today && vm.Expired == false &&
                                           vm.CheckIn < today && vm.CheckOut == null
                                           select vm).ToList();

                    if (Visitordetails.Count != 0)
                    {
                        for (int i = 0; i < Visitordetails.Count(); i++)
                        {
                            Visitordetails[i].Expired = true;
                            Visitordetails[i].IsActive = true;
                            Visitordetails[i].IsUpdated = true;
                            Visitordetails[i].IsDeleted = false;
                            Visitordetails[i].LastUpdatedBy = 0;
                            Visitordetails[i].LastUpdatedDate = DateTime.Now;
                            context.SaveChanges();
                        }
                    }
                    if (Visitordetails1.Count != 0)
                    {
                        for (int i = 0; i < Visitordetails1.Count(); i++)
                        {
                            Visitordetails1[i].Expired = true;
                            Visitordetails1[i].IsActive = true;
                            Visitordetails1[i].IsUpdated = true;
                            Visitordetails1[i].IsDeleted = false;
                            Visitordetails1[i].LastUpdatedBy = 0;
                            Visitordetails1[i].LastUpdatedDate = DateTime.Now;
                            context.SaveChanges();
                        }
                    }
                    if (Visitordetails2.Count != 0)
                    {
                        for (int i = 0; i < Visitordetails2.Count(); i++)
                        {
                            Visitordetails2[i].Expired = true;
                            Visitordetails2[i].IsActive = true;
                            Visitordetails2[i].IsUpdated = true;
                            Visitordetails2[i].IsDeleted = false;
                            Visitordetails2[i].LastUpdatedBy = 0;
                            Visitordetails2[i].LastUpdatedDate = DateTime.Now;
                            context.SaveChanges();
                        }
                    }
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<VisitorManagementViewModel> GetAllEmployeeInvite(VisitorManagementViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;

                var today = DateTime.Today;

                var Visitordetails = (from vm in DB.VisitorManagements
                                      where vm.CreatedBy == EmpId && vm.IsDeleted == false
                                      select vm).OrderByDescending(vm => vm.Date >= today && vm.Date <= today)  // Fetch today's records first
                                                  .ThenByDescending(vm => vm.Date)  // Order other dates after today
                                                  .ThenByDescending(vm => vm.VisitId)  // Sort by VisitId for records on the same date
                                                  .ToList();

                if (EmpId != 0)
                {
                    if (Visitordetails != null)
                    {
                        List<VisitorManagementViewModel> lstofVisitors = new List<VisitorManagementViewModel>();

                        for (int i = 0; i < Visitordetails.Count(); i++)
                        {
                            VisitorManagementViewModel vmvm = new VisitorManagementViewModel();
                            vmvm.RegNo = Visitordetails[i].RegNo;
                            vmvm.QR = Visitordetails[i].QR;
                            vmvm.VisitId = Visitordetails[i].VisitId;
                            vmvm.Name = Visitordetails[i].Name;
                            vmvm.Designation = Visitordetails[i].Designation;
                            vmvm.Company = Visitordetails[i].Company;
                            vmvm.Purpose = Visitordetails[i].Purpose;
                            vmvm.PMail = Visitordetails[i].PMail;
                            vmvm.OMail = Visitordetails[i].OMail;
                            vmvm.Mobile = Visitordetails[i].Mobile;
                            vmvm.AMobile = Visitordetails[i].AMobile;
                            vmvm.Photo = Visitordetails[i].Photo;

                            if (vmvm.Photo != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = vmvm.Photo.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                vmvm.Photo = "Uploads" + lnkval;
                            }

                            vmvm.CompId = Visitordetails[i].CompId;
                            vmvm.CompName = Visitordetails[i].CompId;
                            vmvm.Accessories = Visitordetails[i].Accessories;
                            vmvm.WhomtoMeet = Visitordetails[i].WhomtoMeet;
                            vmvm.WName = DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.FirstName).FirstOrDefault() +
                                DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.MiddleName).FirstOrDefault() +
                                DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.LastName).FirstOrDefault();
                            vmvm.WEmpCode = DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.UserName).FirstOrDefault();
                            vmvm.Date = Visitordetails[i].Date;
                            vmvm.Time = Visitordetails[i].Time;
                            vmvm.Invited = Visitordetails[i].Invited;
                            vmvm.Accept = Visitordetails[i].Accept;
                            vmvm.Approved = Visitordetails[i].Approved;
                            vmvm.Expired = Visitordetails[i].Expired;
                            vmvm.DirectCheckIn = Visitordetails[i].DirectCheckIn;
                            vmvm.CheckIn = Visitordetails[i].CheckIn;
                            vmvm.CheckOut = Visitordetails[i].CheckOut;
                            vmvm.IdCard = Visitordetails[i].IdCard;
                            vmvm.VisitorCheckOut = DB.VisitorInviteHistories.Where(x => x.VisitorId == vmvm.VisitId).Select(x => x.CheckIn).FirstOrDefault();
                            vmvm.VisitorCheckOut = DB.VisitorInviteHistories.Where(x => x.VisitorId == vmvm.VisitId).Select(x => x.CheckOut).FirstOrDefault();
                            vmvm.CreatedBy = Visitordetails[i].CreatedBy;
                            vmvm.CreatedDate = Visitordetails[i].CreatedDate;
                            vmvm.LastUpdatedBy = Visitordetails[i].LastUpdatedBy;
                            vmvm.LastUpdatedDate = Visitordetails[i].LastUpdatedDate;
                            vmvm.IsActive = Visitordetails[i].IsActive;
                            vmvm.IsUpdated = Visitordetails[i].IsUpdated;
                            vmvm.IsDeleted = Visitordetails[i].IsDeleted;
                            lstofVisitors.Add(vmvm);

                        }
                        return lstofVisitors;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Invite Details Not Found");
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
        public VisitorManagementViewModel CancelInvite(VisitorManagementViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? VisitId = (model.VisitId != 0) ? model.VisitId : 0;

                var today = DateTime.Today;

                var Visitordetails = (from vm in DB.VisitorManagements
                                      where vm.VisitId == VisitId && vm.IsActive == true && vm.IsDeleted == false
                                      select vm).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Visitordetails != null)
                    {
                        Visitordetails.IsActive = true;
                        Visitordetails.IsUpdated = true;
                        Visitordetails.IsDeleted = true;
                        Visitordetails.LastUpdatedBy = EmpId;
                        Visitordetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        VisitorManagementViewModel vm = new VisitorManagementViewModel();
                        vm.msg = "Invite Cancelled";

                        return vm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Invite Details Not Found");
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
        public VisitorManagementViewModel DirectCheckIn(VisitorManagementViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                string EmpMail = "", EmpName = "", VisitorMail = "", VisitorName = "", date = "", time = "", locationMap = "";
                int ecount = 0, vcount = 0, flag = 0;

                if (EmpId != 0)
                {
                    VisitorManagement ivm = new VisitorManagement();
                    ivm.RegNo = (model.RegNo != "" && model.RegNo != null) ? model.RegNo : "";
                    ivm.QR = (model.QR != "" && model.QR != null) ? model.QR : "";
                    ivm.Name = model.Name;
                    ivm.Designation = (model.Designation != "" && model.Designation != null) ? model.Designation : "";
                    ivm.Company = (model.Company != "" && model.Company != null) ? model.Company : "";
                    ivm.Purpose = (model.Purpose != "" && model.Purpose != null) ? model.Purpose : "";
                    ivm.PMail = (model.PMail != "" && model.PMail != null) ? model.PMail : "";
                    ivm.OMail = (model.OMail != "" && model.OMail != null) ? model.OMail : "";
                    ivm.Mobile = (model.Mobile != "" && model.Mobile != null) ? model.Mobile : "";
                    ivm.AMobile = (model.AMobile != "" && model.AMobile != null) ? model.AMobile : "";
                    ivm.Photo = (model.Photo != "" && model.Photo != null) ? model.Photo : "";
                    ivm.CompId = (model.CompId != "") ? model.CompId : "";
                    ivm.WhomtoMeet = (model.WhomtoMeet != 0) ? model.WhomtoMeet : 0;
                    ivm.Date = model.Date;
                    ivm.Time = model.Time;
                    ivm.Invited = true;
                    ivm.Accept = true;
                    ivm.Approved = true;
                    ivm.Expired = false;
                    ivm.Accessories = model.Accessories;
                    ivm.CheckIn = DateTime.Now;
                    //ivm.CheckOut = model.CheckOut;
                    ivm.DirectCheckIn = true;
                    ivm.IdCard = model.IdCard;
                    ivm.IsActive = true;
                    ivm.IsUpdated = false;
                    ivm.IsDeleted = false;
                    ivm.CreatedBy = EmpId;
                    ivm.CreatedDate = DateTime.Now;
                    ivm.LastUpdatedBy = EmpId;
                    ivm.LastUpdatedDate = DateTime.Now;
                    DB.VisitorManagements.Add(ivm);
                    DB.SaveChanges();
                    int visitorid = ivm.VisitId;  // Pending - Add Reg No, CompId, QR

                    //SendEmail(model.OMail, subject, body);

                    string InviteCode = GenerateSecureOTP();

                    VisitorInviteHistory vih = new VisitorInviteHistory();
                    vih.VisitorId = visitorid;
                    vih.InviteCode = "0";
                    vih.CheckInCode = InviteCode;
                    vih.Mail = true;
                    vih.Mobile = false;
                    vih.CheckIn = true;
                    vih.CheckOut = false;
                    vih.IsActive = true;
                    vih.IsUpdated = false;
                    vih.IsDeleted = false;
                    vih.CreatedBy = EmpId;
                    vih.CreatedDate = DateTime.Now;
                    vih.LastUpdatedBy = EmpId;
                    vih.LastUpdatedDate = DateTime.Now;
                    DB.VisitorInviteHistories.Add(vih);
                    DB.SaveChanges();

                    var Visitordetails = (from vm in DB.VisitorManagements
                                          where vm.VisitId == visitorid && vm.IsDeleted == false && vm.Invited == true && vm.Accept == true && vm.Expired == false
                                          select vm).FirstOrDefault();

                    model.Date = Visitordetails.Date;
                    model.Time = Visitordetails.Time;
                    model.CompId = Visitordetails.CompId;
                    int? wtm = Visitordetails.WhomtoMeet;
                    EmpMail = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.EmailId).FirstOrDefault();
                    EmpName = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.FirstName).FirstOrDefault();
                    VisitorMail = Visitordetails.OMail;
                    VisitorName = Visitordetails.Name;
                    DateTime date1 = Convert.ToDateTime(model.Date);
                    date = date1.ToString("dd-MM-yyyy");
                    time = Convert.ToString(model.Time);
                    string input = model.CompId;
                    string[] parts = input.Split(new string[] { " - " }, StringSplitOptions.None);
                    string loc = parts[1];

                    locationMap = DB.LocationMasters.Where(x => x.Location.ToUpper() == loc.ToUpper()).Select(x => x.LocationMap).FirstOrDefault();
                    if (locationMap == null)
                    {
                        locationMap = "https://www.google.com/maps/place/RIM+INDIA+PVT+LTD/@12.9765095,77.5295906,15z/data=!4m2!3m1!1s0x0:0x255305137c820daa?sa=X&ved=1t:2428&ictx=111";
                    }


                    string invitecode = InviteCode;

                    if (EmpMail != "")
                    {
                        // Fire-and-forget email sending task
                        Task.Run(() => SendEmailEmpCheckIn(EmpMail, EmpName, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn));
                        ecount = 1;
                    }
                    if (VisitorMail != "")
                    {
                        // Fire-and-forget email sending task
                        Task.Run(() => SendEmailVisitorCheckIn(VisitorMail, VisitorName, date, time, invitecode, locationMap, flag, Visitordetails.CheckIn));
                        vcount = 1;
                    }

                    VisitorManagementViewModel ivmvm = new VisitorManagementViewModel();
                    if (ecount == 1 && vcount == 1)
                    {
                        ivmvm.msg = "Visitor Checked In Successfully";
                        ivmvm.Name = model.Name;
                        ivmvm.InviteCode = "0";
                    }
                    else if (ecount == 0 && vcount == 1)
                    {
                        ivmvm.msg = "Employee Check in mail is not send";
                        ivmvm.Name = model.Name;
                        ivmvm.InviteCode = "0";
                    }
                    else if (ecount == 0 && vcount == 1)
                    {
                        ivmvm.msg = "Visitor Check in mail is not send";
                        ivmvm.Name = model.Name;
                        ivmvm.InviteCode = "0";
                    }
                    else
                    {
                        ivmvm.msg = "Check in mail is not send";
                        ivmvm.Name = model.Name;
                        ivmvm.InviteCode = "0";
                    }

                    return ivmvm;
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
        public VisitorManagementViewModel CheckIn(VisitorManagementViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? VisitId = (model.VisitId != 0) ? model.VisitId : 0;
                string EmpMail = "", EmpName = "", VisitorMail = "", VisitorName = "", date = "", time = "", locationMap = "";
                int ecount = 0, vcount = 0, flag = 0;

                var Visitordetails = (from vm in DB.VisitorManagements
                                      where vm.VisitId == VisitId && vm.IsDeleted == false && vm.Invited == true && vm.Accept == true && vm.Expired == false
                                      select vm).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Visitordetails != null)
                    {
                        Visitordetails.IdCard = model.IdCard;
                        Visitordetails.Accessories = model.Accessories;
                        Visitordetails.Approved = true;
                        Visitordetails.CheckIn = DateTime.Now;
                        Visitordetails.IsUpdated = true;
                        Visitordetails.LastUpdatedBy = VisitId;
                        Visitordetails.LastUpdatedDate = DateTime.Now;
                        Visitordetails.Photo = (model.Photo != "" && model.Photo != null) ? model.Photo : Visitordetails.Photo;
                        DB.SaveChanges();

                        var VHistorydetails = (from vh in DB.VisitorInviteHistories
                                               where vh.VisitorId == VisitId && vh.IsActive == true && vh.IsDeleted == false && vh.CheckInCode != "" 
                                               && vh.CheckIn == false && vh.CheckOut == false
                                               select vh).FirstOrDefault();

                        if (VHistorydetails != null)
                        {
                            VHistorydetails.CheckIn = true;
                            VHistorydetails.CheckOut = false;
                            VHistorydetails.IsUpdated = true;
                            VHistorydetails.LastUpdatedBy = VisitId;
                            VHistorydetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();
                        }

                        model.Date = Visitordetails.Date;
                        model.Time = Visitordetails.Time;
                        model.CompId = Visitordetails.CompId;
                        int? wtm = Visitordetails.WhomtoMeet;
                        EmpMail = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.EmailId).FirstOrDefault();
                        EmpName = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.FirstName).FirstOrDefault();
                        VisitorMail = Visitordetails.OMail;
                        VisitorName = Visitordetails.Name;
                        DateTime date1 = Convert.ToDateTime(model.Date);
                        date = date1.ToString("dd-MM-yyyy");
                        time = Convert.ToString(model.Time);
                        string input = model.CompId;
                        string[] parts = input.Split(new string[] { " - " }, StringSplitOptions.None);
                        string loc = parts[1];

                        locationMap = DB.LocationMasters.Where(x => x.Location.ToUpper() == loc.ToUpper()).Select(x => x.LocationMap).FirstOrDefault();
                        if (locationMap == null)
                        {
                            locationMap = "https://www.google.com/maps/place/RIM+INDIA+PVT+LTD/@12.9765095,77.5295906,15z/data=!4m2!3m1!1s0x0:0x255305137c820daa?sa=X&ved=1t:2428&ictx=111";
                        }

                        // loading issues ----
                        ////model.Date = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.Date).FirstOrDefault();
                        ////model.Time = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.Time).FirstOrDefault();
                        ////model.CompId = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.CompId).FirstOrDefault();
                        ////int? wtm = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.WhomtoMeet).FirstOrDefault();
                        ////EmpMail = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.EmailId).FirstOrDefault();
                        ////EmpName = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.FirstName).FirstOrDefault();
                        ////VisitorMail = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.OMail).FirstOrDefault();
                        ////VisitorName = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.Name).FirstOrDefault();
                        ////date = Convert.ToString(model.Date);
                        ////time = Convert.ToString(model.Time);
                        ////locationMap = DB.CompanyMasters.Where(x => x.CompId == model.CompId).Select(x => x.LocationMap).FirstOrDefault();

                        string invitecode = VHistorydetails.CheckInCode;

                        if (EmpMail != "")
                        {
                            // Fire-and-forget email sending task
                            Task.Run(() => SendEmailEmpCheckIn(EmpMail, EmpName, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn));
                            //SendEmailEmpCheckIn(EmpMail, EmpName, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn);
                            ecount = 1;
                        }
                        if (VisitorMail != "")
                        {
                            // Fire-and-forget email sending task
                            Task.Run(() => SendEmailVisitorCheckIn(VisitorMail, VisitorName, date, time, invitecode, locationMap, flag, Visitordetails.CheckIn));
                            //SendEmailVisitorCheckIn(VisitorMail, VisitorName, date, time, invitecode, locationMap, flag, Visitordetails.CheckIn);
                            vcount = 1;
                        }

                        VisitorManagementViewModel ivmvm = new VisitorManagementViewModel();
                        if (ecount == 1 && vcount == 1)
                        {
                            ivmvm.msg = "Visitor Checked In Successfully";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }
                        else if (ecount == 0 && vcount == 1)
                        {
                            ivmvm.msg = "Employee Check in mail is not send";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }
                        else if (ecount == 0 && vcount == 1)
                        {
                            ivmvm.msg = "Visitor Check in mail is not send";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }
                        else
                        {
                            ivmvm.msg = "Check in mail is not send";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }

                        return ivmvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Visitor Details Not Found");
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
        public VisitorManagementViewModel CheckOut(VisitorManagementViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                int? VisitId = (model.VisitId != 0) ? model.VisitId : 0;
                string EmpMail = "", EmpName = "", VisitorMail = "", VisitorName = "", date = "", time = "", locationMap = "";
                int ecount = 0, vcount = 0, flag = 0;

                var Visitordetails = (from vm in DB.VisitorManagements
                                      where vm.VisitId == VisitId && vm.IsDeleted == false && vm.Invited == true && vm.Accept == true 
                                      && vm.CheckIn != null && vm.Expired == false
                                      select vm).FirstOrDefault();

                if (EmpId != 0)
                {
                    if (Visitordetails != null)
                    {
                        Visitordetails.IdCard = model.IdCard;
                        Visitordetails.Accessories = model.Accessories;
                        Visitordetails.CheckOut = DateTime.Now;
                        Visitordetails.IsUpdated = true;
                        Visitordetails.LastUpdatedBy = VisitId;
                        Visitordetails.LastUpdatedDate = DateTime.Now;
                        Visitordetails.Photo = (model.Photo != "" && model.Photo != null) ? model.Photo : Visitordetails.Photo;
                        DB.SaveChanges();

                        var VHistorydetails = (from vh in DB.VisitorInviteHistories
                                               where vh.VisitorId == VisitId && vh.IsActive == true && vh.IsDeleted == false && vh.CheckInCode != "" && vh.CheckIn == true && vh.CheckOut == false
                                               select vh).FirstOrDefault();

                        if (VHistorydetails != null)
                        {
                            VHistorydetails.CheckIn = true;
                            VHistorydetails.CheckOut = true;
                            VHistorydetails.IsUpdated = true;
                            VHistorydetails.LastUpdatedBy = VisitId;
                            VHistorydetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();
                        }

                        model.Date = Visitordetails.Date;
                        model.Time = Visitordetails.Time;
                        model.CompId = Visitordetails.CompId;
                        int? wtm = Visitordetails.WhomtoMeet;
                        EmpMail = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.EmailId).FirstOrDefault();
                        EmpName = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.FirstName).FirstOrDefault();
                        VisitorMail = Visitordetails.OMail;
                        VisitorName = Visitordetails.Name;
                        DateTime date1 = Convert.ToDateTime(model.Date);
                        date = date1.ToString("dd-MM-yyyy");
                        time = Convert.ToString(model.Time);
                        string input = model.CompId;
                        string[] parts = input.Split(new string[] { " - " }, StringSplitOptions.None);
                        string loc = parts[1];

                        locationMap = DB.LocationMasters.Where(x => x.Location.ToUpper() == loc.ToUpper()).Select(x => x.LocationMap).FirstOrDefault();
                        if (locationMap == null)
                        {
                            locationMap = "https://www.google.com/maps/place/RIM+INDIA+PVT+LTD/@12.9765095,77.5295906,15z/data=!4m2!3m1!1s0x0:0x255305137c820daa?sa=X&ved=1t:2428&ictx=111";
                        }

                        ////model.Date = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.Date).FirstOrDefault();
                        ////model.Time = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.Time).FirstOrDefault();
                        ////model.CompId = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.CompId).FirstOrDefault();
                        ////int? wtm = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.WhomtoMeet).FirstOrDefault();
                        ////EmpMail = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.EmailId).FirstOrDefault();
                        ////EmpName = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.FirstName).FirstOrDefault();
                        ////VisitorMail = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.OMail).FirstOrDefault();
                        ////VisitorName = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.Name).FirstOrDefault();
                        ////date = Convert.ToString(model.Date);
                        ////time = Convert.ToString(model.Time);
                        ////locationMap = DB.CompanyMasters.Where(x => x.CompId == model.CompId).Select(x => x.LocationMap).FirstOrDefault();

                        if (EmpMail != "")
                        {
                            // Fire-and-forget email sending task
                            Task.Run(() => SendEmailEmpCheckOut(EmpMail, EmpName, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn, Visitordetails.CheckOut));
                            //SendEmailEmpCheckOut(EmpMail, EmpName, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn, Visitordetails.CheckOut);
                            ecount = 1;
                        }
                        if (VisitorMail != "")
                        {
                            // Fire-and-forget email sending task
                            Task.Run(() => SendEmailVisitorCheckOut(VisitorMail, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn, Visitordetails.CheckOut));
                            //SendEmailVisitorCheckOut(VisitorMail, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn, Visitordetails.CheckOut);
                            vcount = 1;
                        }

                        VisitorManagementViewModel ivmvm = new VisitorManagementViewModel();
                        if (ecount == 1 && vcount == 1)
                        {
                            ivmvm.msg = "Visitor Checked out Successfully";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }
                        else if (ecount == 0 && vcount == 1)
                        {
                            ivmvm.msg = "Employee Check out mail is not send";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }
                        else if (ecount == 0 && vcount == 1)
                        {
                            ivmvm.msg = "Visitor Check out mail is not send";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }
                        else
                        {
                            ivmvm.msg = "Check out mail is not send";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }

                        return ivmvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Visitor Details Not Found");
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
        public VisitorManagementViewModel VerifyOTPCheckIn(VisitorManagementViewModel model)
        {
            try
            {
                string msg = "";
                string OTP = (model.otp != "" || model.otp != null) ? model.otp : "";
                DateTime tdydate = DateTime.Now;

                var Visitordetails = (from vih in DB.VisitorInviteHistories
                                      join vm in DB.VisitorManagements on vih.VisitorId equals vm.VisitId
                                      where vih.CheckInCode == OTP && vm.IsDeleted == false && vm.Invited == true && vm.Accept == true && vm.Expired == false
                                      select vm).FirstOrDefault();


                if (OTP != "")
                {
                    if (Visitordetails != null)
                    {
                        var Visitordetails1 = (from vih in DB.VisitorInviteHistories
                                               join vm in DB.VisitorManagements on vih.VisitorId equals vm.VisitId
                                               where vih.CheckInCode == OTP && vm.Date <= tdydate && vm.IsDeleted == false 
                                               && vm.Invited == true && vm.Accept == true && vm.Expired == false
                                               select vm).FirstOrDefault();

                        if (Visitordetails1 != null)
                        {
                            VisitorManagementViewModel vmvm = new VisitorManagementViewModel();
                            vmvm.RegNo = Visitordetails.RegNo;
                            vmvm.QR = Visitordetails.QR;
                            vmvm.VisitId = Visitordetails.VisitId;
                            vmvm.Name = Visitordetails.Name;
                            vmvm.Designation = Visitordetails.Designation;
                            vmvm.Company = Visitordetails.Company;
                            vmvm.Purpose = Visitordetails.Purpose;
                            vmvm.PMail = Visitordetails.PMail;
                            vmvm.OMail = Visitordetails.OMail;
                            vmvm.Mobile = Visitordetails.Mobile;
                            vmvm.AMobile = Visitordetails.AMobile;
                            vmvm.Photo = Visitordetails.Photo;

                            if (Visitordetails.Photo != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = Visitordetails.Photo.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                Visitordetails.Photo = "Uploads" + lnkval;
                            }

                            vmvm.CompId = Visitordetails.CompId;
                            vmvm.CompName = Visitordetails.CompId;
                            vmvm.Accessories = Visitordetails.Accessories;
                            vmvm.WhomtoMeet = Visitordetails.WhomtoMeet;
                            vmvm.WName = DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.FirstName).FirstOrDefault() +
                                DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.MiddleName).FirstOrDefault() +
                                DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.LastName).FirstOrDefault();
                            vmvm.WEmpCode = DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.UserName).FirstOrDefault();
                            vmvm.Date = Visitordetails.Date;
                            vmvm.Time = Visitordetails.Time;
                            vmvm.Invited = Visitordetails.Invited;
                            vmvm.Accept = Visitordetails.Accept;
                            vmvm.Approved = Visitordetails.Approved;
                            vmvm.Expired = Visitordetails.Expired;
                            vmvm.DirectCheckIn = Visitordetails.DirectCheckIn;
                            vmvm.CheckIn = Visitordetails.CheckIn;
                            vmvm.CheckOut = Visitordetails.CheckOut;
                            vmvm.IdCard = Visitordetails.IdCard;
                            vmvm.VisitorCheckIn = DB.VisitorInviteHistories.Where(x => x.CheckInCode == OTP).Select(x => x.CheckIn).FirstOrDefault();
                            vmvm.VisitorCheckOut = DB.VisitorInviteHistories.Where(x => x.CheckInCode == OTP).Select(x => x.CheckOut).FirstOrDefault();
                            vmvm.CreatedBy = Visitordetails.CreatedBy;
                            vmvm.CreatedDate = Visitordetails.CreatedDate;
                            vmvm.LastUpdatedBy = Visitordetails.LastUpdatedBy;
                            vmvm.LastUpdatedDate = Visitordetails.LastUpdatedDate;
                            vmvm.IsActive = Visitordetails.IsActive;
                            vmvm.IsUpdated = Visitordetails.IsUpdated;
                            vmvm.IsDeleted = Visitordetails.IsDeleted;
                            vmvm.InviteCode = OTP;
                            vmvm.msg = "OTP Verified Successfully";

                            return vmvm;
                        }
                        else
                        {
                            DateTime cidate = Convert.ToDateTime(Visitordetails.Date);
                            string scheduledate = cidate.ToString("dd-MM-yyyy");
                            throw new CustomApiException(HttpStatusCode.NotFound, "CheckIn date(" + scheduledate + ") Mismatched");
                        }
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "OTP Invalid");
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
        public VisitorManagementViewModel VisitorCheckIn(VisitorManagementViewModel model)
        {
            try
            {
                string msg = "";
                int? VisitId = (model.VisitId != 0) ? model.VisitId : 0;
                string EmpMail = "", EmpName = "", VisitorMail = "", VisitorName = "", date = "", time = "", locationMap = "";
                int ecount = 0, vcount = 0, flag = 0;

                var Visitordetails = (from vm in DB.VisitorManagements
                                      where vm.VisitId == VisitId && vm.IsDeleted == false && vm.Invited == true && vm.Accept == true && vm.Expired == false
                                      select vm).FirstOrDefault();

                if (VisitId != 0)
                {
                    if (Visitordetails != null)
                    {
                        Visitordetails.IdCard = model.IdCard;
                        Visitordetails.Accessories = model.Accessories;
                        Visitordetails.Approved = true;
                        Visitordetails.CheckIn = DateTime.Now;
                        Visitordetails.IsUpdated = true;
                        Visitordetails.LastUpdatedBy = VisitId;
                        Visitordetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        var VHistorydetails = (from vh in DB.VisitorInviteHistories
                                               where vh.VisitorId == VisitId && vh.IsActive == true && vh.IsDeleted == false && vh.CheckInCode != "" && vh.CheckIn == false && vh.CheckOut == false
                                               select vh).FirstOrDefault();

                        if (VHistorydetails != null)
                        {
                            VHistorydetails.CheckIn = true;
                            VHistorydetails.CheckOut = false;
                            VHistorydetails.IsUpdated = true;
                            VHistorydetails.LastUpdatedBy = VisitId;
                            VHistorydetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();
                        }

                        model.Date = Visitordetails.Date;
                        model.Time = Visitordetails.Time;
                        model.CompId = Visitordetails.CompId;
                        int? wtm = Visitordetails.WhomtoMeet;
                        EmpMail = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.EmailId).FirstOrDefault();
                        EmpName = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.FirstName).FirstOrDefault();
                        VisitorMail = Visitordetails.OMail;
                        VisitorName = Visitordetails.Name;
                        DateTime date1 = Convert.ToDateTime(model.Date);
                        date = date1.ToString("dd-MM-yyyy");
                        time = Convert.ToString(model.Time);
                        string input = model.CompId;
                        string[] parts = input.Split(new string[] { " - " }, StringSplitOptions.None);
                        string loc = parts[1];

                        locationMap = DB.LocationMasters.Where(x => x.Location.ToUpper() == loc.ToUpper()).Select(x => x.LocationMap).FirstOrDefault();
                        if (locationMap == null)
                        {
                            locationMap = "https://www.google.com/maps/place/RIM+INDIA+PVT+LTD/@12.9765095,77.5295906,15z/data=!4m2!3m1!1s0x0:0x255305137c820daa?sa=X&ved=1t:2428&ictx=111";
                        }

                        ////model.Date = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.Date).FirstOrDefault();
                        ////model.Time = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.Time).FirstOrDefault();
                        ////model.CompId = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.CompId).FirstOrDefault();
                        ////int? wtm = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.WhomtoMeet).FirstOrDefault();
                        ////EmpMail = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.EmailId).FirstOrDefault();
                        ////EmpName = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.FirstName).FirstOrDefault();
                        ////VisitorMail = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.OMail).FirstOrDefault();
                        ////VisitorName = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.Name).FirstOrDefault();
                        ////date = Convert.ToString(model.Date);
                        ////time = Convert.ToString(model.Time);
                        ////locationMap = DB.CompanyMasters.Where(x => x.CompId == model.CompId).Select(x => x.LocationMap).FirstOrDefault();
                        string invitecode = VHistorydetails.CheckInCode;

                        if (EmpMail != "")
                        {
                            // Fire-and-forget email sending task
                            Task.Run(() => SendEmailEmpCheckIn(EmpMail, EmpName, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn));
                            //SendEmailEmpCheckIn(EmpMail, EmpName, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn);
                            ecount = 1;
                        }
                        if (VisitorMail != "")
                        {
                            // Fire-and-forget email sending task
                            Task.Run(() => SendEmailVisitorCheckIn(VisitorMail, VisitorName, date, time, invitecode, locationMap, flag, Visitordetails.CheckIn));
                            //SendEmailVisitorCheckIn(VisitorMail, VisitorName, date, time, invitecode, locationMap, flag, Visitordetails.CheckIn);
                            vcount = 1;
                        }

                        VisitorManagementViewModel ivmvm = new VisitorManagementViewModel();
                        if (ecount == 1 && vcount == 1)
                        {
                            ivmvm.msg = "Visitor Self Checked In Successfully";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }
                        else if (ecount == 0 && vcount == 1)
                        {
                            ivmvm.msg = "Employee Check in mail is not send";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }
                        else if (ecount == 0 && vcount == 1)
                        {
                            ivmvm.msg = "Visitor self Check in mail is not send";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }
                        else
                        {
                            ivmvm.msg = "Check in mail is not send";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }

                        return ivmvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Visitor Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Visitor Details are Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public VisitorManagementViewModel VisitorCheckOut(VisitorManagementViewModel model)
        {
            try
            {
                string msg = "";
                int? VisitId = (model.VisitId != 0) ? model.VisitId : 0;
                string EmpMail = "", EmpName = "", VisitorMail = "", VisitorName = "", date = "", time = "", locationMap = "";
                int ecount = 0, vcount = 0, flag = 0;

                var Visitordetails = (from vm in DB.VisitorManagements
                                      where vm.VisitId == VisitId && vm.IsDeleted == false && vm.Invited == true && vm.Accept == true 
                                      && vm.CheckIn != null && vm.Expired == false
                                      select vm).FirstOrDefault();

                if (VisitId != 0)
                {
                    if (Visitordetails != null)
                    {
                        Visitordetails.IdCard = model.IdCard;
                        Visitordetails.Accessories = model.Accessories;
                        Visitordetails.CheckOut = DateTime.Now;
                        Visitordetails.IsUpdated = true;
                        Visitordetails.LastUpdatedBy = VisitId;
                        Visitordetails.LastUpdatedDate = DateTime.Now;
                        DB.SaveChanges();

                        var VHistorydetails = (from vh in DB.VisitorInviteHistories
                                               where vh.VisitorId == VisitId && vh.IsActive == true && vh.IsDeleted == false && vh.CheckInCode != "" && vh.CheckIn == true && vh.CheckOut == false
                                               select vh).FirstOrDefault();

                        if (VHistorydetails != null)
                        {
                            VHistorydetails.CheckIn = true;
                            VHistorydetails.CheckOut = true;
                            VHistorydetails.IsUpdated = true;
                            VHistorydetails.LastUpdatedBy = VisitId;
                            VHistorydetails.LastUpdatedDate = DateTime.Now;
                            DB.SaveChanges();
                        }

                        model.Date = Visitordetails.Date;
                        model.Time = Visitordetails.Time;
                        model.CompId = Visitordetails.CompId;
                        int? wtm = Visitordetails.WhomtoMeet;
                        EmpMail = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.EmailId).FirstOrDefault();
                        EmpName = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.FirstName).FirstOrDefault();
                        VisitorMail = Visitordetails.OMail;
                        VisitorName = Visitordetails.Name;
                        DateTime date1 = Convert.ToDateTime(model.Date);
                        date = date1.ToString("dd-MM-yyyy");
                        time = Convert.ToString(model.Time);
                        string input = model.CompId;
                        string[] parts = input.Split(new string[] { " - " }, StringSplitOptions.None);
                        string loc = parts[1];

                        locationMap = DB.LocationMasters.Where(x => x.Location.ToUpper() == loc.ToUpper()).Select(x => x.LocationMap).FirstOrDefault();
                        if (locationMap == null)
                        {
                            locationMap = "https://www.google.com/maps/place/RIM+INDIA+PVT+LTD/@12.9765095,77.5295906,15z/data=!4m2!3m1!1s0x0:0x255305137c820daa?sa=X&ved=1t:2428&ictx=111";
                        }

                        ////model.Date = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.Date).FirstOrDefault();
                        ////model.Time = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.Time).FirstOrDefault();
                        ////model.CompId = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.CompId).FirstOrDefault();
                        ////int? wtm = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.WhomtoMeet).FirstOrDefault();
                        ////EmpMail = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.EmailId).FirstOrDefault();
                        ////EmpName = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.FirstName).FirstOrDefault();
                        ////VisitorMail = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.OMail).FirstOrDefault();
                        ////VisitorName = DB.VisitorManagements.Where(x => x.VisitId == model.VisitId).Select(x => x.Name).FirstOrDefault();
                        ////date = Convert.ToString(model.Date);
                        ////time = Convert.ToString(model.Time);
                        ////locationMap = DB.CompanyMasters.Where(x => x.CompId == model.CompId).Select(x => x.LocationMap).FirstOrDefault();

                        if (EmpMail != "")
                        {
                            // Fire-and-forget email sending task
                            Task.Run(() => SendEmailEmpCheckOut(EmpMail, EmpName, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn, Visitordetails.CheckOut));
                            //SendEmailEmpCheckOut(EmpMail, EmpName, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn, Visitordetails.CheckOut);
                            ecount = 1;
                        }
                        if (VisitorMail != "")
                        {
                            // Fire-and-forget email sending task
                            Task.Run(() => SendEmailVisitorCheckOut(VisitorMail, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn, Visitordetails.CheckOut));
                            //SendEmailVisitorCheckOut(VisitorMail, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn, Visitordetails.CheckOut);
                            vcount = 1;
                        }

                        VisitorManagementViewModel ivmvm = new VisitorManagementViewModel();
                        if (ecount == 1 && vcount == 1)
                        {
                            ivmvm.msg = "Visitor Checked out Successfully";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }
                        else if (ecount == 0 && vcount == 1)
                        {
                            ivmvm.msg = "Employee Check out mail is not send";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }
                        else if (ecount == 0 && vcount == 1)
                        {
                            ivmvm.msg = "Visitor self Check out mail is not send";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }
                        else
                        {
                            ivmvm.msg = "Check out mail is not send";
                            ivmvm.Name = model.Name;
                            ivmvm.InviteCode = "0";
                        }

                        return ivmvm;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Visitor Details Not Found");
                    }
                }
                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Visitor Details are Missing");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public List<VisitorManagementViewModel> VisitFilter(FilterViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                bool invite = false, Accept = false, Approved = false, expried = false;
                DateTime? checkin = null, checkout = null;
                DateTime? fdate = Convert.ToDateTime(model.FromDate);
                DateTime? tdate = Convert.ToDateTime(model.ToDate);

                if(model.FromDate == null && model.ToDate == null)
                {
                    model.FromDate = "";
                    model.ToDate = "";
                }

                if (model.Status != null)
                {
                    if (model.Status.ToUpper() == "INVITED")
                    {
                        invite = true;
                    }
                    else if (model.Status.ToUpper() == "INVITE ACCEPTED")
                    {
                        invite = true;
                        Accept = true;

                    }
                    else if (model.Status.ToUpper() == "CHECKED IN")
                    {
                        invite = true;
                        Accept = true;
                        checkin = DateTime.Now;
                    }
                    else if (model.Status.ToUpper() == "CHECKED OUT")
                    {
                        invite = true;
                        Accept = true;
                        checkin = DateTime.Now;
                        checkout = DateTime.Now;
                    }
                    else if (model.Status.ToUpper() == "EXPIRED")
                    {
                        expried = true;
                    }
                }
                else
                {
                    model.Status = "";
                }
                
                var Visitordetails = (from vm in DB.VisitorManagements
                                      where vm.IsDeleted == false
                                      select vm).OrderByDescending(x => x.VisitId).ToList();

                if (model.FromDate != ""  && model.ToDate != "")
                {
                    if (model.Status == "")
                    {
                        if (checkin != null && checkout == null)
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Date >= fdate && vm.Date <= tdate
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                        if (checkin != null && checkout != null)
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Date >= fdate && vm.Date <= tdate
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                        else
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Date >= fdate && vm.Date <= tdate
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                    }
                    else
                    {
                        if (checkin != null && checkout == null)
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Date >= fdate && vm.Date <= tdate
                                              && vm.Invited == invite && vm.Accept == Accept && vm.CheckIn != null && vm.CheckOut == checkout
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                        else if (checkin != null && checkout != null)
                        {
                            if (model.Status == "EXPIRED")
                            {
                                Visitordetails = (from vm in DB.VisitorManagements
                                                  where vm.IsDeleted == false
                                                  && vm.Date >= fdate && vm.Date <= tdate
                                                  && vm.Expired == true
                                                  select vm).OrderByDescending(x => x.VisitId).ToList();
                            }
                            else
                            {
                                Visitordetails = (from vm in DB.VisitorManagements
                                                  where vm.IsDeleted == false
                                                  && vm.Date >= fdate && vm.Date <= tdate
                                                  && vm.Invited == invite && vm.Accept == Accept && vm.CheckIn != null && vm.CheckOut != null
                                                  select vm).OrderByDescending(x => x.VisitId).ToList();
                            }
                                
                        }
                        else
                        {
                            if (model.Status == "EXPIRED")
                            {
                                Visitordetails = (from vm in DB.VisitorManagements
                                                  where vm.IsDeleted == false
                                                  && vm.Date >= fdate && vm.Date <= tdate
                                                  && vm.Expired == true
                                                  select vm).OrderByDescending(x => x.VisitId).ToList();
                            }
                            else
                            {
                                Visitordetails = (from vm in DB.VisitorManagements
                                                  where vm.IsDeleted == false
                                                  && vm.Date >= fdate && vm.Date <= tdate
                                                  && vm.Invited == invite && vm.Accept == Accept && vm.CheckIn == null && vm.CheckOut == null
                                                  select vm).OrderByDescending(x => x.VisitId).ToList();
                            }
                            
                        }
                    }
                    
                }
                else
                {
                    if (checkin != null && checkout == null)
                    {
                        Visitordetails = (from vm in DB.VisitorManagements
                                          where vm.IsDeleted == false
                                          && vm.Invited == invite && vm.Accept == Accept && vm.CheckIn != null && vm.CheckOut == checkout
                                          select vm).OrderByDescending(x => x.VisitId).ToList();
                    }
                    else if (checkin != null && checkout != null)
                    {
                        if (model.Status == "EXPIRED")
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Expired == true
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                        else
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Invited == invite && vm.Accept == Accept && vm.CheckIn != null && vm.CheckOut != null
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                    }
                    else
                    {
                        if (model.Status == "EXPIRED") 
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Expired == true
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                        else
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Invited == invite && vm.Accept == Accept && vm.CheckIn == null && vm.CheckOut == null
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                        
                    }
                }

                if (EmpId != 0)
                {
                    if (Visitordetails != null)
                    {
                        List<VisitorManagementViewModel> lstofVisitors = new List<VisitorManagementViewModel>();

                        for (int i = 0; i < Visitordetails.Count(); i++)
                        {
                            VisitorManagementViewModel vmvm = new VisitorManagementViewModel();
                            vmvm.RegNo = Visitordetails[i].RegNo;
                            vmvm.QR = Visitordetails[i].QR;
                            vmvm.VisitId = Visitordetails[i].VisitId;
                            vmvm.Name = Visitordetails[i].Name;
                            vmvm.Designation = Visitordetails[i].Designation;
                            vmvm.Company = Visitordetails[i].Company;
                            vmvm.Purpose = Visitordetails[i].Purpose;
                            vmvm.PMail = Visitordetails[i].PMail;
                            vmvm.OMail = Visitordetails[i].OMail;
                            vmvm.Mobile = Visitordetails[i].Mobile;
                            vmvm.AMobile = Visitordetails[i].AMobile;
                            vmvm.Photo = Visitordetails[i].Photo;

                            if (vmvm.Photo != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = vmvm.Photo.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                vmvm.Photo = "Uploads" + lnkval;
                            }

                            vmvm.CompId = Visitordetails[i].CompId;
                            vmvm.CompName = Visitordetails[i].CompId;
                            vmvm.Accessories = Visitordetails[i].Accessories;
                            vmvm.WhomtoMeet = Visitordetails[i].WhomtoMeet;
                            vmvm.WName = DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.FirstName).FirstOrDefault() +
                                DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.MiddleName).FirstOrDefault() +
                                DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.LastName).FirstOrDefault();
                            vmvm.WEmpCode = DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.UserName).FirstOrDefault();
                            vmvm.Date = Visitordetails[i].Date;
                            vmvm.Time = Visitordetails[i].Time;
                            vmvm.Invited = Visitordetails[i].Invited;
                            vmvm.Accept = Visitordetails[i].Accept;
                            vmvm.Approved = Visitordetails[i].Approved;
                            vmvm.DirectCheckIn = Visitordetails[i].DirectCheckIn;
                            vmvm.CheckIn = Visitordetails[i].CheckIn;
                            vmvm.CheckOut = Visitordetails[i].CheckOut;
                            vmvm.Expired = Visitordetails[i].Expired;
                            vmvm.IdCard = Visitordetails[i].IdCard;
                            vmvm.VisitorCheckIn = DB.VisitorInviteHistories.Where(x => x.VisitorId == vmvm.VisitId).Select(x => x.CheckIn).FirstOrDefault();
                            vmvm.VisitorCheckOut = DB.VisitorInviteHistories.Where(x => x.VisitorId == vmvm.VisitId).Select(x => x.CheckOut).FirstOrDefault();
                            vmvm.CreatedBy = Visitordetails[i].CreatedBy;
                            vmvm.CreatedDate = Visitordetails[i].CreatedDate;
                            vmvm.LastUpdatedBy = Visitordetails[i].LastUpdatedBy;
                            vmvm.LastUpdatedDate = Visitordetails[i].LastUpdatedDate;
                            vmvm.IsActive = Visitordetails[i].IsActive;
                            vmvm.IsUpdated = Visitordetails[i].IsUpdated;
                            vmvm.IsDeleted = Visitordetails[i].IsDeleted;
                            lstofVisitors.Add(vmvm);

                        }
                        return lstofVisitors;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Visit Details Not Found");
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
        public List<VisitorManagementViewModel> VisitExport(FilterViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = (model.EmpId != 0) ? model.EmpId : 0;
                bool invite = false, Accept = false, Approved = false, expried = false;
                DateTime? checkin = null, checkout = null;
                DateTime? fdate = Convert.ToDateTime(model.FromDate);
                DateTime? tdate = Convert.ToDateTime(model.ToDate);

                if (model.FromDate == null && model.ToDate == null)
                {
                    model.FromDate = "";
                    model.ToDate = "";
                }

                if (model.Status != null)
                {
                    if (model.Status.ToUpper() == "INVITED")
                    {
                        invite = true;
                    }
                    else if (model.Status.ToUpper() == "INVITE ACCEPTED")
                    {
                        invite = true;
                        Accept = true;

                    }
                    else if (model.Status.ToUpper() == "CHECKED IN")
                    {
                        invite = true;
                        Accept = true;
                        checkin = DateTime.Now;
                    }
                    else if (model.Status.ToUpper() == "CHECKED OUT")
                    {
                        invite = true;
                        Accept = true;
                        checkin = DateTime.Now;
                        checkout = DateTime.Now;
                    }
                    else if (model.Status.ToUpper() == "EXPIRED")
                    {
                        expried = true;
                    }
                }
                else
                {
                    model.Status = "";
                }

                var Visitordetails = (from vm in DB.VisitorManagements
                                      where vm.IsDeleted == false
                                      select vm).OrderByDescending(x => x.VisitId).ToList();

                if (model.FromDate != "" && model.ToDate != "")
                {
                    if (model.Status == "")
                    {
                        if (checkin != null && checkout == null)
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Date >= fdate && vm.Date <= tdate
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                        if (checkin != null && checkout != null)
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Date >= fdate && vm.Date <= tdate
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                        else
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Date >= fdate && vm.Date <= tdate
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                    }
                    else
                    {
                        if (checkin != null && checkout == null)
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Date >= fdate && vm.Date <= tdate
                                              && vm.Invited == invite && vm.Accept == Accept && vm.CheckIn != null && vm.CheckOut == checkout
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                        else if (checkin != null && checkout != null)
                        {
                            if (model.Status == "EXPIRED")
                            {
                                Visitordetails = (from vm in DB.VisitorManagements
                                                  where vm.IsDeleted == false
                                                  && vm.Date >= fdate && vm.Date <= tdate
                                                  && vm.Expired == true
                                                  select vm).OrderByDescending(x => x.VisitId).ToList();
                            }
                            else
                            {
                                Visitordetails = (from vm in DB.VisitorManagements
                                                  where vm.IsDeleted == false
                                                  && vm.Date >= fdate && vm.Date <= tdate
                                                  && vm.Invited == invite && vm.Accept == Accept && vm.CheckIn != null && vm.CheckOut != null
                                                  select vm).OrderByDescending(x => x.VisitId).ToList();
                            }

                        }
                        else
                        {
                            if (model.Status == "EXPIRED")
                            {
                                Visitordetails = (from vm in DB.VisitorManagements
                                                  where vm.IsDeleted == false
                                                  && vm.Date >= fdate && vm.Date <= tdate
                                                  && vm.Expired == true
                                                  select vm).OrderByDescending(x => x.VisitId).ToList();
                            }
                            else
                            {
                                Visitordetails = (from vm in DB.VisitorManagements
                                                  where vm.IsDeleted == false
                                                  && vm.Date >= fdate && vm.Date <= tdate
                                                  && vm.Invited == invite && vm.Accept == Accept && vm.CheckIn == null && vm.CheckOut == null
                                                  select vm).OrderByDescending(x => x.VisitId).ToList();
                            }

                        }
                    }

                }
                else
                {
                    if (checkin != null && checkout == null)
                    {
                        Visitordetails = (from vm in DB.VisitorManagements
                                          where vm.IsDeleted == false
                                          && vm.Invited == invite && vm.Accept == Accept && vm.CheckIn != null && vm.CheckOut == checkout
                                          select vm).OrderByDescending(x => x.VisitId).ToList();
                    }
                    else if (checkin != null && checkout != null)
                    {
                        if (model.Status == "EXPIRED")
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Expired == true
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                        else
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Invited == invite && vm.Accept == Accept && vm.CheckIn != null && vm.CheckOut != null
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                    }
                    else
                    {
                        if (model.Status == "EXPIRED")
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Expired == true
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }
                        else
                        {
                            Visitordetails = (from vm in DB.VisitorManagements
                                              where vm.IsDeleted == false
                                              && vm.Invited == invite && vm.Accept == Accept && vm.CheckIn == null && vm.CheckOut == null
                                              select vm).OrderByDescending(x => x.VisitId).ToList();
                        }

                    }
                }

                if (EmpId != 0)
                {
                    if (Visitordetails != null)
                    {
                        List<VisitorManagementViewModel> lstofVisitors = new List<VisitorManagementViewModel>();

                        for (int i = 0; i < Visitordetails.Count(); i++)
                        {
                            VisitorManagementViewModel vmvm = new VisitorManagementViewModel();
                            vmvm.RegNo = Visitordetails[i].RegNo;
                            vmvm.QR = Visitordetails[i].QR;
                            vmvm.VisitId = Visitordetails[i].VisitId;
                            vmvm.Name = Visitordetails[i].Name;
                            vmvm.Designation = Visitordetails[i].Designation;
                            vmvm.Company = Visitordetails[i].Company;
                            vmvm.Purpose = Visitordetails[i].Purpose;
                            vmvm.PMail = Visitordetails[i].PMail;
                            vmvm.OMail = Visitordetails[i].OMail;
                            vmvm.Mobile = Visitordetails[i].Mobile;
                            vmvm.AMobile = Visitordetails[i].AMobile;
                            vmvm.Photo = Visitordetails[i].Photo;

                            if (vmvm.Photo != "")
                            {
                                string[] stringSeparators = new string[] { "Uploads" };
                                string[] firstNames = vmvm.Photo.Split(stringSeparators, StringSplitOptions.None);
                                string lnkval = firstNames[1];
                                vmvm.Photo = "Uploads" + lnkval;
                            }

                            vmvm.CompId = Visitordetails[i].CompId;
                            vmvm.CompName = Visitordetails[i].CompId;
                            vmvm.Accessories = Visitordetails[i].Accessories;
                            vmvm.WhomtoMeet = Visitordetails[i].WhomtoMeet;
                            vmvm.WName = DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.FirstName).FirstOrDefault() +
                                DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.MiddleName).FirstOrDefault() +
                                DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.LastName).FirstOrDefault();
                            vmvm.WEmpCode = DB.EmployeeMasters.Where(x => x.EmpId == vmvm.WhomtoMeet).Select(x => x.UserName).FirstOrDefault();
                            vmvm.Date = Visitordetails[i].Date;
                            vmvm.Time = Visitordetails[i].Time;
                            vmvm.Invited = Visitordetails[i].Invited;
                            vmvm.Accept = Visitordetails[i].Accept;
                            vmvm.Approved = Visitordetails[i].Approved;
                            vmvm.DirectCheckIn = Visitordetails[i].DirectCheckIn;
                            vmvm.CheckIn = Visitordetails[i].CheckIn;
                            vmvm.CheckOut = Visitordetails[i].CheckOut;
                            vmvm.IdCard = Visitordetails[i].IdCard;
                            vmvm.VisitorCheckIn = DB.VisitorInviteHistories.Where(x => x.VisitorId == vmvm.VisitId).Select(x => x.CheckIn).FirstOrDefault();
                            vmvm.VisitorCheckOut = DB.VisitorInviteHistories.Where(x => x.VisitorId == vmvm.VisitId).Select(x => x.CheckOut).FirstOrDefault();
                            vmvm.CreatedBy = Visitordetails[i].CreatedBy;
                            vmvm.CreatedDate = Visitordetails[i].CreatedDate;
                            vmvm.LastUpdatedBy = Visitordetails[i].LastUpdatedBy;
                            vmvm.LastUpdatedDate = Visitordetails[i].LastUpdatedDate;
                            vmvm.IsActive = Visitordetails[i].IsActive;
                            vmvm.IsUpdated = Visitordetails[i].IsUpdated;
                            vmvm.IsDeleted = Visitordetails[i].IsDeleted;
                            lstofVisitors.Add(vmvm);

                        }
                        return lstofVisitors;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Visit Details Not Found");
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
        private byte[] GenerateQRCode(string qrText)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                using (Bitmap bitmap = qrCode.GetGraphic(20))
                {
                    // Resize the image
                    Bitmap resizedBitmap = new Bitmap(bitmap, new Size(100, 100));

                    // Save the resized image to the memory stream
                    resizedBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                    //bitmap.Save(ms, ImageFormat.Png);
                    //byte[] byteImage = ms.ToArray();
                    //return Convert.ToBase64String(byteImage);
                    return ms.ToArray();
                }
            }
        }
        private string GenerateSecureOTP()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[4];
                rng.GetBytes(byteArray);
                int randomNumber = BitConverter.ToInt32(byteArray, 0);
                randomNumber = Math.Abs(randomNumber % 1000000);
                return randomNumber.ToString("D6");
            }
        }
        public List<DDCompViewModel> DDCompany(DDCompViewModel compdd)
        {
            try
            {
                string msg = "";
                int? EmpId = 0;

                var CompLocdetails = (from Loc in DB.LocationMasters
                                      join Comp in DB.CompanyMasters on Loc.CompId equals Comp.CompId
                                      where Comp.IsActive == true && Comp.IsDeleted == false && Loc.IsActive == true && Loc.IsDeleted == false
                                      select Loc).ToList();

                //var CompLEdetails = (from LE in DB.LegalEntityMasters
                //                     join Loc in DB.LocationMasters on LE.LEId equals Loc.LEId
                //                     where LE.IsActive == true && LE.IsDeleted == false && Loc.IsActive == true && Loc.IsDeleted == false
                //                     select LE).ToList();

                var CompLEdetails = (from LE in DB.LegalEntityMasters
                                     join Loc in DB.LocationMasters on LE.LEId equals Loc.LEId into locationGroup
                                     from Loc in locationGroup.DefaultIfEmpty() // This performs a left join
                                     where LE.IsActive == true
                                        && LE.IsDeleted == false
                                        && Loc == null // This ensures only LegalEntityMasters without a matching LocationMasters record
                                     select LE).Distinct().ToList();

                List<DDCompViewModel> lstofComp = new List<DDCompViewModel>();

                for (int i = 0; i < CompLocdetails.Count(); i++)
                {
                    DDCompViewModel ddc = new DDCompViewModel();
                    ddc.CompId = Convert.ToInt32(CompLocdetails[i].CompId);
                    ddc.LEId = 0;
                    ddc.LocationId = CompLocdetails[i].LocationId;
                    string company = (ddc.CompId != 0) ? DB.CompanyMasters.Where(x => x.CompId == ddc.CompId).Select(x => x.Company).FirstOrDefault() : "";
                    string location = (ddc.LocationId != 0) ? DB.LocationMasters.Where(x => x.LocationId == ddc.LocationId).Select(x => x.Location).FirstOrDefault() : "";
                    ddc.CompName = company + " - " + location;
                    lstofComp.Add(ddc);
                }

                for (int j = 0; j < CompLEdetails.Count(); j++)
                {
                    DDCompViewModel ddc = new DDCompViewModel();
                    ddc.CompId = Convert.ToInt32(CompLEdetails[j].CompId);
                    ddc.LocationId = 0;
                    ddc.LEId = Convert.ToInt32(CompLEdetails[j].LEId);
                    string company = (ddc.CompId != 0) ? DB.CompanyMasters.Where(x => x.CompId == ddc.CompId).Select(x => x.Company).FirstOrDefault() : "";
                    string entity = (ddc.LEId != 0) ? DB.LegalEntityMasters.Where(x => x.LEId == ddc.LEId).Select(x => x.LegalEntity).FirstOrDefault() : "";
                    ddc.CompName = company + " - " + entity;
                    lstofComp.Add(ddc);
                }

                if (EmpId == 0)
                {
                    if (lstofComp != null)
                    {
                        return lstofComp;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Company Details Not Found");
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
        public List<DDEmpViewModel> DDEmployee(DDEmpViewModel empdd)
        {
            try
            {
                string msg = "";
                int? EmpId = 0;

                var Empdetails = (from Emp in DB.EmployeeMasters
                                  where Emp.IsActive == true && Emp.IsDeleted == false
                                  select new DDEmpViewModel
                                  {
                                      EmpId = Emp.EmpId,
                                      EmpName = Emp.FirstName + " " + Emp.MiddleName + " " + Emp.LastName,
                                      EmpCode = Emp.UserName,
                                  }).ToList();

                if (EmpId == 0)
                {
                    if (Empdetails != null)
                    {
                        return Empdetails;
                    }
                    else
                    {
                        throw new CustomApiException(HttpStatusCode.NotFound, "Employee Details Not Found");
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
        public VisitorManagementViewModel VisitorDirectCheckIn(VisitorManagementViewModel model)
        {
            try
            {
                string msg = "";
                int? EmpId = 0;
                string EmpMail = "", EmpName = "", VisitorMail = "", VisitorName = "", date = "", time = "", locationMap = "";
                int ecount = 0, vcount = 0, flag = 0;

                if (EmpId == 0)
                {
                    VisitorManagement ivm = new VisitorManagement();
                    ivm.RegNo = (model.RegNo != "" && model.RegNo != null) ? model.RegNo : "";
                    ivm.QR = (model.QR != "" && model.QR != null) ? model.QR : "";
                    ivm.Name = model.Name;
                    ivm.Designation = (model.Designation != "" && model.Designation != null) ? model.Designation : "";
                    ivm.Company = (model.Company != "" && model.Company != null) ? model.Company : "";
                    ivm.Purpose = (model.Purpose != "" && model.Purpose != null) ? model.Purpose : "";
                    ivm.PMail = (model.PMail != "" && model.PMail != null) ? model.PMail : "";
                    ivm.OMail = (model.OMail != "" && model.OMail != null) ? model.OMail : "";
                    ivm.Mobile = (model.Mobile != "" && model.Mobile != null) ? model.Mobile : "";
                    ivm.AMobile = (model.AMobile != "" && model.AMobile != null) ? model.AMobile : "";
                    ivm.Photo = (model.Photo != "" && model.Photo != null) ? model.Photo : "";
                    ivm.CompId = (model.CompId != "") ? model.CompId : "";
                    ivm.WhomtoMeet = (model.WhomtoMeet != 0) ? model.WhomtoMeet : 0;
                    ivm.Date = model.Date;
                    ivm.Time = model.Time;
                    ivm.Invited = true;
                    ivm.Accept = true;
                    ivm.Approved = true;
                    ivm.Expired = false;
                    ivm.Accessories = model.Accessories;
                    ivm.CheckIn = DateTime.Now;
                    //ivm.CheckOut = model.CheckOut;
                    ivm.DirectCheckIn = true;
                    ivm.IdCard = model.IdCard;
                    ivm.IsActive = true;
                    ivm.IsUpdated = false;
                    ivm.IsDeleted = false;
                    ivm.CreatedBy = EmpId;
                    ivm.CreatedDate = DateTime.Now;
                    ivm.LastUpdatedBy = EmpId;
                    ivm.LastUpdatedDate = DateTime.Now;
                    DB.VisitorManagements.Add(ivm);
                    DB.SaveChanges();
                    int visitorid = ivm.VisitId;  // Pending - Add Reg No, CompId, QR

                    //SendEmail(model.OMail, subject, body);

                    string InviteCode = GenerateSecureOTP();

                    VisitorInviteHistory vih = new VisitorInviteHistory();
                    vih.VisitorId = visitorid;
                    vih.InviteCode = "0";
                    vih.CheckInCode = InviteCode;
                    vih.Mail = true;
                    vih.Mobile = false;
                    vih.CheckIn = true;
                    vih.CheckOut = false;
                    vih.IsActive = true;
                    vih.IsUpdated = false;
                    vih.IsDeleted = false;
                    vih.CreatedBy = EmpId;
                    vih.CreatedDate = DateTime.Now;
                    vih.LastUpdatedBy = EmpId;
                    vih.LastUpdatedDate = DateTime.Now;
                    DB.VisitorInviteHistories.Add(vih);
                    DB.SaveChanges();

                    var Visitordetails = (from vm in DB.VisitorManagements
                                          where vm.VisitId == visitorid && vm.IsDeleted == false && vm.Invited == true && vm.Accept == true && vm.Expired == false
                                          select vm).FirstOrDefault();

                    model.Date = Visitordetails.Date;
                    model.Time = Visitordetails.Time;
                    model.CompId = Visitordetails.CompId;
                    int? wtm = Visitordetails.WhomtoMeet;
                    EmpMail = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.EmailId).FirstOrDefault();
                    EmpName = DB.EmployeeMasters.Where(x => x.EmpId == wtm).Select(x => x.FirstName).FirstOrDefault();
                    VisitorMail = Visitordetails.OMail;
                    VisitorName = Visitordetails.Name;
                    DateTime date1 = Convert.ToDateTime(model.Date);
                    date = date1.ToString("dd-MM-yyyy");
                    time = Convert.ToString(model.Time);
                    string input = model.CompId;
                    string[] parts = input.Split(new string[] { " - " }, StringSplitOptions.None);
                    string loc = parts[1];

                    locationMap = DB.LocationMasters.Where(x => x.Location.ToUpper() == loc.ToUpper()).Select(x => x.LocationMap).FirstOrDefault();
                    if (locationMap == null)
                    {
                        locationMap = "https://www.google.com/maps/place/RIM+INDIA+PVT+LTD/@12.9765095,77.5295906,15z/data=!4m2!3m1!1s0x0:0x255305137c820daa?sa=X&ved=1t:2428&ictx=111";
                    }


                    string invitecode = InviteCode;

                    if (EmpMail != "")
                    {
                        // Fire-and-forget email sending task
                        Task.Run(() => SendEmailEmpCheckIn(EmpMail, EmpName, VisitorName, date, time, locationMap, flag, Visitordetails.CheckIn));
                        ecount = 1;
                    }
                    if (VisitorMail != "")
                    {
                        // Fire-and-forget email sending task
                        Task.Run(() => SendEmailVisitorCheckIn(VisitorMail, VisitorName, date, time, invitecode, locationMap, flag, Visitordetails.CheckIn));
                        vcount = 1;
                    }

                    VisitorManagementViewModel ivmvm = new VisitorManagementViewModel();
                    if (ecount == 1 && vcount == 1)
                    {
                        ivmvm.msg = "Visitor Checked In Successfully";
                        ivmvm.Name = model.Name;
                        ivmvm.InviteCode = "0";
                    }
                    else if (ecount == 0 && vcount == 1)
                    {
                        ivmvm.msg = "Employee Check in mail is not send";
                        ivmvm.Name = model.Name;
                        ivmvm.InviteCode = "0";
                    }
                    else if (ecount == 0 && vcount == 1)
                    {
                        ivmvm.msg = "Visitor Check in mail is not send";
                        ivmvm.Name = model.Name;
                        ivmvm.InviteCode = "0";
                    }
                    else
                    {
                        ivmvm.msg = "Check in mail is not send";
                        ivmvm.Name = model.Name;
                        ivmvm.InviteCode = "0";
                    }

                    return ivmvm;
                }

                else
                {
                    throw new CustomApiException(HttpStatusCode.NotFound, "Visitor Only");
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public void SendEmailWithQRCode(string toEmail, string subject, string body, byte[] qrCodePng, DateTime? Date, String Time)
        {
            try
            {

                var GetSMTPData = (from ES in DB.EmailSetUps
                                   select ES).FirstOrDefault();


                if (GetSMTPData != null)
                {
                    //EmailSetUpViewModel esvm = new EmailSetUpViewModel();
                    int? CompId = GetSMTPData.CompId;
                    string SMTPServer = GetSMTPData.SMTPServer;
                    int SMTPPort = Convert.ToInt32(GetSMTPData.SMTPPort);
                    string SMTPMailId = GetSMTPData.SMTPMailId;
                    string SMTPPassword = GetSMTPData.SMTPPassword;
                    string EmailId = GetSMTPData.EmailId;

                    string teamsMeetingLink = "https://teams.microsoft.com/l/meetup-join/your-teams-link";
                    string emailBody = $@"<p>You are invited to a Teams meeting.</p>
                                          <p>Click here to join: <a href='{teamsMeetingLink}'>Join Teams Meeting</a></p>";

                    string locationMap = DB.CompanyMasters.Where(x => x.CompId == 1).Select(x => x.LocationMap).FirstOrDefault();

                    DateTime MeetDate = Convert.ToDateTime(Date);
                    DateTime meetingDateTime = DateTime.Now;

                    // Parse the time string to DateTime using ParseExact
                    DateTime parsedTime;
                    if (DateTime.TryParseExact(Time, "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedTime))
                    {
                        // Extract the TimeSpan (time part) from parsed DateTime
                        TimeSpan timePart = parsedTime.TimeOfDay;

                        // Add the TimeSpan to the Date to get the combined DateTime
                        meetingDateTime = MeetDate.Add(timePart);
                    }

                    //Calendar values
                    string description = emailBody;
                    string location = locationMap;
                    DateTime startTime = meetingDateTime.AddHours(1);  // Start time (1 hour from now)
                    DateTime endTime = meetingDateTime.AddHours(2);


                    ////str.AppendLine("BEGIN:VCALENDAR");
                    ////str.AppendLine("PRODID:-//Microsoft Corporation//Outlook 12.0 MIMEDIR//EN");
                    ////str.AppendLine("VERSION:2.0");
                    ////str.AppendLine(string.Format("METHOD:REQUEST"));
                    ////str.AppendLine("BEGIN:VEVENT");

                //PRODID: -//Your Organization//Your Product//EN


                    //Calendar
                 string calendarInvite = $@"
BEGIN:VCALENDAR
VERSION:2.0
PRODID:-//Microsoft Corporation//Outlook 12.0 MIMEDIR//EN
METHOD:REQUEST
BEGIN:VEVENT
UID:{Guid.NewGuid()}
DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}
DTSTART:{startTime:yyyyMMddTHHmmssZ}
DTEND:{endTime:yyyyMMddTHHmmssZ}
SUMMARY:{subject}
DESCRIPTION:{description}
LOCATION:{location}
ORGANIZER;CN=""3DCAD"":mailto:frontdesk@rim-global.com
ATTENDEE;CN=""{toEmail}"":mailto:{toEmail}
BEGIN:VALARM
TRIGGER:-PT15M
ACTION:DISPLAY
DESCRIPTION:Reminder
END:VALARM
END:VEVENT
END:VCALENDAR";

                    try
                    {
                        using (var message = new MailMessage())
                        {
                            message.From = new MailAddress(SMTPMailId);
                            message.To.Add(toEmail);
                            message.Subject = subject;

                            // Create a new memory stream with the QR code PNG data
                            using (var ms = new MemoryStream(qrCodePng))
                            {
                                var inlineImage = new LinkedResource(ms, "image/png")
                                {
                                    ContentId = "QRCodeImage"
                                };

                                var bodyBuilder = new AlternateView(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(body)), "text/html");
                                bodyBuilder.LinkedResources.Add(inlineImage);

                                message.Body = body;
                                message.IsBodyHtml = true;
                                message.AlternateViews.Add(bodyBuilder);

                                // Convert the iCalendar content to bytes and then to Base64 string
                                byte[] calendarBytes = Encoding.UTF8.GetBytes(calendarInvite);
                                string base64CalendarContent = Convert.ToBase64String(calendarBytes);

                                // Create the attachment
                                Attachment calendarAttachment = new Attachment(new MemoryStream(calendarBytes), "invite.ics", "text/calendar");
                                message.Attachments.Add(calendarAttachment);

                                var smtpClient = new SmtpClient(SMTPServer)
                                {
                                    Port = SMTPPort,
                                    Credentials = new System.Net.NetworkCredential(SMTPMailId, SMTPPassword),
                                    EnableSsl = true,
                                };

                                smtpClient.Send(message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("Error sending email: " + ex.Message);
                    }
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }

//        public void SendCalendarInvite(string toEmail, string subject, string description, string location, DateTime startTime, DateTime endTime)
//        {
//            string calendarInvite = $@"
//BEGIN:VCALENDAR
//VERSION:2.0
//PRODID:-//Your Organization//Your Product//EN
//METHOD:REQUEST
//BEGIN:VEVENT
//UID:{Guid.NewGuid()}
//DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}
//DTSTART:{startTime:yyyyMMddTHHmmssZ}
//DTEND:{endTime:yyyyMMddTHHmmssZ}
//SUMMARY:{subject}
//DESCRIPTION:{description}
//LOCATION:{location}
//ORGANIZER;CN=""Your Name"":mailto:your-email@example.com
//ATTENDEE;CN=""{toEmail}"":mailto:{toEmail}
//BEGIN:VALARM
//TRIGGER:-PT15M
//ACTION:DISPLAY
//DESCRIPTION:Reminder
//END:VALARM
//END:VEVENT
//END:VCALENDAR";

//            SendEmailWithCalendarInvite(toEmail, subject, calendarInvite);
//        }
//        public void SendEmailWithCalendarInvite(string toEmail, string subject, string calendarContent)
//        {
//            MailMessage mail = new MailMessage();
//            mail.From = new MailAddress("your-email@example.com");
//            mail.To.Add(new MailAddress(toEmail));
//            mail.Subject = subject;
//            mail.Body = "Please see the attached calendar invite.";

//            // Convert the iCalendar content to bytes and then to Base64 string
//            byte[] calendarBytes = Encoding.UTF8.GetBytes(calendarContent);
//            string base64CalendarContent = Convert.ToBase64String(calendarBytes);

//            // Create the attachment
//            Attachment calendarAttachment = new Attachment(new MemoryStream(calendarBytes), "invite.ics", "text/calendar");
//            mail.Attachments.Add(calendarAttachment);

//            // Send the email
//            SmtpClient smtp = new SmtpClient("your-smtp-server.com")
//            {
//                Port = 587, // or 25
//                Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-password"),
//                EnableSsl = true
//            };

//            smtp.Send(mail);
//        }
        //Calender
        //private static string MeetingRequestString(string from, List<string> toUsers, string subject, string desc, string location, DateTime startTime, DateTime endTime, int? eventID = null, bool isCancel = false)
        //{
        //    StringBuilder str = new StringBuilder();

        //    str.AppendLine("BEGIN:VCALENDAR");
        //    str.AppendLine("PRODID:-//Microsoft Corporation//Outlook 12.0 MIMEDIR//EN");
        //    str.AppendLine("VERSION:2.0");
        //    str.AppendLine(string.Format("METHOD:{0}", (isCancel ? "CANCEL" : "REQUEST")));
        //    str.AppendLine("BEGIN:VEVENT");

        //    str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", startTime.ToUniversalTime()));
        //    str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmss}", DateTime.Now));
        //    str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", endTime.ToUniversalTime()));
        //    str.AppendLine(string.Format("LOCATION: {0}", location));
        //    str.AppendLine(string.Format("UID:{0}", (eventID.HasValue ? "blablabla" + eventID : Guid.NewGuid().ToString())));
        //    str.AppendLine(string.Format("DESCRIPTION:{0}", desc.Replace("\n", "<br>")));
        //    str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", desc.Replace("\n", "<br>")));
        //    str.AppendLine(string.Format("SUMMARY:{0}", subject));

        //    str.AppendLine(string.Format("ORGANIZER;CN=\"{0}\":MAILTO:{1}", from, from));
        //    str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", string.Join(",", toUsers), string.Join(",", toUsers)));

        //    str.AppendLine("BEGIN:VALARM");
        //    str.AppendLine("TRIGGER:-PT15M");
        //    str.AppendLine("ACTION:DISPLAY");
        //    str.AppendLine("DESCRIPTION:Reminder");
        //    str.AppendLine("END:VALARM");
        //    str.AppendLine("END:VEVENT");
        //    str.AppendLine("END:VCALENDAR");

        //    return str.ToString();
        //}
        public void SendEmailEmp(string toEmail, string EmpName, string VisitorName, string InviteCode, string date, string time, string locationMap)
        {
            try
            {

                var GetSMTPData = (from ES in DB.EmailSetUps
                                   select ES).FirstOrDefault();


                if (GetSMTPData != null)
                {
                    //EmailSetUpViewModel esvm = new EmailSetUpViewModel();
                    int? CompId = GetSMTPData.CompId;
                    string SMTPServer = GetSMTPData.SMTPServer;
                    int SMTPPort = Convert.ToInt32(GetSMTPData.SMTPPort);
                    string SMTPMailId = GetSMTPData.SMTPMailId;
                    string SMTPPassword = GetSMTPData.SMTPPassword;
                    string EmailId = GetSMTPData.EmailId;

                    //string link = "http://newofficeconnect.rim-global.com/UAT_OfficeConnect/#/verify_otp";
                    string link = DB.ServiceMasters.Where(x => x.ServiceName == "VisitorLink").Select(x => x.ServiceLink).FirstOrDefault();
                    string subject = "Visitor Approval Confirmation for " + VisitorName;
                    string body = $@"
                                <p>Dear {EmpName},</p>
                                <p>I am pleased to inform you that the visit request for {VisitorName} has been approved.</p>
                                <p><strong>Visit Details:</strong></p>
                                <p><strong>Visitor's Name: </strong>{VisitorName}</p>
                                <p><strong>Date of Visit: </strong>{date}</p>
                                <p><strong>Time: </strong>{time}</p>
                                <p><strong>Location: </strong>{locationMap}</p>
                                <p></p>
                                <p>If you have any questions or require further assistance, do not hesitate to contact us.</p>
                                <p>We look forward to welcoming you to 3DCAD.</p>
                                <p>Best regards,</p>
                                <p>3DCAD</p>";

                    try
                    {
                        using (var message = new MailMessage())
                        {
                            message.From = new MailAddress(SMTPMailId);
                            message.To.Add(toEmail);
                            message.Subject = subject;

                            message.Body = body;
                            message.IsBodyHtml = true;

                            var smtpClient = new SmtpClient(SMTPServer)
                            {
                                Port = SMTPPort,
                                Credentials = new System.Net.NetworkCredential(SMTPMailId, SMTPPassword),
                                EnableSsl = true,
                            };

                            smtpClient.Send(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("Error sending email: " + ex.Message);
                    }
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public void SendEmailVisitor(string toEmail, string VisitorName, string InviteCode, string date, string time, string locationMap)
        {
            try
            {

                var GetSMTPData = (from ES in DB.EmailSetUps
                                   select ES).FirstOrDefault();


                if (GetSMTPData != null)
                {
                    //EmailSetUpViewModel esvm = new EmailSetUpViewModel();
                    int? CompId = GetSMTPData.CompId;
                    string SMTPServer = GetSMTPData.SMTPServer;
                    int SMTPPort = Convert.ToInt32(GetSMTPData.SMTPPort);
                    string SMTPMailId = GetSMTPData.SMTPMailId;
                    string SMTPPassword = GetSMTPData.SMTPPassword;
                    string EmailId = GetSMTPData.EmailId;

                    //string link = "http://newofficeconnect.rim-global.com/UAT_OfficeConnect/#/verify_otp";
                    string link = DB.ServiceMasters.Where(x => x.ServiceName == "VisitorLink").Select(x => x.ServiceLink).FirstOrDefault();
                    string subject = "Visitor Approval Confirmation";
                    string body = $@"
                                <p>Dear {VisitorName},</p>
                                <p>We are pleased to inform you that your visit request has been approved.</p>
                                <p><strong>Visit Details:</strong></p>
                                <p><strong>Date of Visit: </strong>{date}</p>
                                <p><strong>Time: </strong>{time}</p>
                                <p><strong>Location: </strong>{locationMap}</p>
                                <p><strong>CheckIn Code: </strong>{ InviteCode}</p>
                                <p></p>
                                <p>Please ensure you bring a valid photo ID for verification upon arrival. </p>

                                
                                <p>If you have any questions or require further assistance, do not hesitate to contact us.</p>
                                <p>We look forward to welcoming you to 3DCAD.</p>
                                <p>Best regards,</p>
                                <p>3DCAD</p>";
                    //< p >< strong > CheckIn Code: </ strong >{ InviteCode}</ p >
                    //< p > Self - Checkout:</ p >
     
                    //                 < p > When you are ready to leave, you can use the same link and invitation code to complete the self - checkout process.</ p >
                    //          To confirm your attendance and streamline the check -in process, please click the following link: < a href = '{link}' > CheckIn </ a >.
                    try
                    {
                        using (var message = new MailMessage())
                        {
                            message.From = new MailAddress(SMTPMailId);
                            message.To.Add(toEmail);
                            message.Subject = subject;

                            message.Body = body;
                            message.IsBodyHtml = true;

                            var smtpClient = new SmtpClient(SMTPServer)
                            {
                                Port = SMTPPort,
                                Credentials = new System.Net.NetworkCredential(SMTPMailId, SMTPPassword),
                                EnableSsl = true,
                            };

                            smtpClient.Send(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("Error sending email: " + ex.Message);
                    }
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public void SendEmailEmpCheckIn(string toEmail, string EmpName, string VisitorName, string date, string time, string locationMap, int flag, DateTime? checkin)
        {
            try
            {

                var GetSMTPData = (from ES in DB.EmailSetUps
                                   select ES).FirstOrDefault();


                if (GetSMTPData != null)
                {
                    //EmailSetUpViewModel esvm = new EmailSetUpViewModel();
                    int? CompId = GetSMTPData.CompId;
                    string SMTPServer = GetSMTPData.SMTPServer;
                    int SMTPPort = Convert.ToInt32(GetSMTPData.SMTPPort);
                    string SMTPMailId = GetSMTPData.SMTPMailId;
                    string SMTPPassword = GetSMTPData.SMTPPassword;
                    string EmailId = GetSMTPData.EmailId;

                    //string link = "http://newofficeconnect.rim-global.com/UAT_OfficeConnect/#/verify_otp";
                    string link = DB.ServiceMasters.Where(x => x.ServiceName == "VisitorLink").Select(x => x.ServiceLink).FirstOrDefault();
                    string subject = "Your Visitor(" + VisitorName + ") Has Successfully Checked In";
                    string body = $@"
                                <p>Dear {EmpName},</p>
                                <p>We are pleased to inform you that your visitor, {VisitorName}, has successfully checked in at 3DCAD.</p>
                                <p><strong>Visit Details:</strong></p>
                                <p><strong>Visitor Name: </strong>{VisitorName}</p>
                                <p><strong>Date of Visit: </strong>{date}</p>
                                <p><strong>Check-In Time: </strong>{checkin}</p>
                                <p><strong>Location: </strong>{locationMap}</p>
                                <p>Your visitor is currently waiting for you at [waiting area/location]. Please make the necessary arrangements to meet them.</p>
                                <p>If you have any questions or need assistance, feel free to contact our front desk at Preetha E.</p>
                                <p>Thank you, and we hope you have a productive meeting.</p>
                                <p>Best regards,</p>
                                <p>3DCAD</p>";

                    try
                    {
                        using (var message = new MailMessage())
                        {
                            message.From = new MailAddress(SMTPMailId);
                            message.To.Add(toEmail);
                            message.Subject = subject;

                            message.Body = body;
                            message.IsBodyHtml = true;

                            var smtpClient = new SmtpClient(SMTPServer)
                            {
                                Port = SMTPPort,
                                Credentials = new System.Net.NetworkCredential(SMTPMailId, SMTPPassword),
                                EnableSsl = true,
                            };

                            smtpClient.Send(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("Error sending email: " + ex.Message);
                    }
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public void SendEmailEmpCheckOut(string toEmail, string EmpName, string VisitorName, string date, string time, string locationMap, int flag, DateTime? checkin, DateTime? checkout)
        {
            try
            {

                var GetSMTPData = (from ES in DB.EmailSetUps
                                   select ES).FirstOrDefault();


                if (GetSMTPData != null)
                {
                    //EmailSetUpViewModel esvm = new EmailSetUpViewModel();
                    int? CompId = GetSMTPData.CompId;
                    string SMTPServer = GetSMTPData.SMTPServer;
                    int SMTPPort = Convert.ToInt32(GetSMTPData.SMTPPort);
                    string SMTPMailId = GetSMTPData.SMTPMailId;
                    string SMTPPassword = GetSMTPData.SMTPPassword;
                    string EmailId = GetSMTPData.EmailId;

                    //string link = "http://newofficeconnect.rim-global.com/UAT_OfficeConnect/#/verify_otp";
                    string link = DB.ServiceMasters.Where(x => x.ServiceName == "VisitorLink").Select(x => x.ServiceLink).FirstOrDefault();
                    string subject = "Your Visitor(" + VisitorName + ") Has Successfully Checked Out";
                    string body = $@"
                                <p>Dear {EmpName},</p>
                                <p>We would like to inform you that your visitor, {VisitorName}, has successfully checked out from 3DCAD.</p>
                                <p><strong>Visit Details:</strong></p>
                                <p><strong>Visitor Name: </strong>{VisitorName}</p>
                                <p><strong>Date of Visit: </strong>{date}</p>
                                <p><strong>Check-In Time: </strong>{checkin}</p>
                                <p><strong>Check-Out Time: </strong>{checkout}</p>
                                <p><strong>Location: </strong>{locationMap}</p>
                                <p>Thank you for hosting {VisitorName}. We hope the visit was productive and met your expectations.</p>
                                <p>If you have any feedback or need further assistance, please do not hesitate to contact us at Preetha E.</p>
                                <p>Thank you, and have a great day!</p>
                                <p>Best regards,</p>
                                <p>3DCAD</p>";

                    try
                    {
                        using (var message = new MailMessage())
                        {
                            message.From = new MailAddress(SMTPMailId);
                            message.To.Add(toEmail);
                            message.Subject = subject;

                            message.Body = body;
                            message.IsBodyHtml = true;

                            var smtpClient = new SmtpClient(SMTPServer)
                            {
                                Port = SMTPPort,
                                Credentials = new System.Net.NetworkCredential(SMTPMailId, SMTPPassword),
                                EnableSsl = true,
                            };

                            smtpClient.Send(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("Error sending email: " + ex.Message);
                    }
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public void SendEmailVisitorCheckIn(string toEmail, string VisitorName, string date, string time, string invitecode, string locationMap, int flag, DateTime? checkin)
        {
            try
            {

                var GetSMTPData = (from ES in DB.EmailSetUps
                                   select ES).FirstOrDefault();


                if (GetSMTPData != null)
                {
                    //EmailSetUpViewModel esvm = new EmailSetUpViewModel();
                    int? CompId = GetSMTPData.CompId;
                    string SMTPServer = GetSMTPData.SMTPServer;
                    int SMTPPort = Convert.ToInt32(GetSMTPData.SMTPPort);
                    string SMTPMailId = GetSMTPData.SMTPMailId;
                    string SMTPPassword = GetSMTPData.SMTPPassword;
                    string EmailId = GetSMTPData.EmailId;

                    //string link = "http://newofficeconnect.rim-global.com/UAT_OfficeConnect/#/verify_otp";
                    string link = DB.ServiceMasters.Where(x => x.ServiceName == "VisitorLink").Select(x => x.ServiceLink).FirstOrDefault();
                    string subject = "Successful Check-In Confirmation at 3DCAD";
                    string body = $@"
                                <p>Dear {VisitorName},</p>
                                <p>We are pleased to inform you that your check-in at 3DACD has been successfully completed.</p>
                                <p><strong>Visit Details:</strong></p>
                                <p><strong>Date of Visit: </strong>{date}</p>
                                <p><strong>CheckIn Time: </strong>{checkin}</p>
                                <p><strong>Host: </strong>Preetha E</p>
                                
                                <p>Feedback:</p>
                                <p>We value your feedback and would appreciate it if you could take a few minutes to share your experience with us. [Provide feedback link if applicable].</p>
                                <p>Future Visits:</p>
                                <p>Should you need to visit us again, feel free to reach out to your host or our front desk to schedule your next visit.</p>
                                <p>Contact Information:</p>
                                <p>If you have any questions or require further assistance, please do not hesitate to contact us at Preetha E.</p>
                                <p>Thank you for visiting 3DCAD, and we look forward to welcoming you again.</p>
                                <p>Best regards,</p>
                                <p>3DCAD</p>";
                    try
                    {
                        using (var message = new MailMessage())
                        {
                            message.From = new MailAddress(SMTPMailId);
                            message.To.Add(toEmail);
                            message.Subject = subject;

                            message.Body = body;
                            message.IsBodyHtml = true;

                            var smtpClient = new SmtpClient(SMTPServer)
                            {
                                Port = SMTPPort,
                                Credentials = new System.Net.NetworkCredential(SMTPMailId, SMTPPassword),
                                EnableSsl = true,
                            };

                            smtpClient.Send(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("Error sending email: " + ex.Message);
                    }
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public void SendEmailVisitorCheckOut(string toEmail, string VisitorName, string date, string time, string locationMap, int flag, DateTime? checkin, DateTime? checkout)
        {
            try
            {

                var GetSMTPData = (from ES in DB.EmailSetUps
                                   select ES).FirstOrDefault();


                if (GetSMTPData != null)
                {
                    //EmailSetUpViewModel esvm = new EmailSetUpViewModel();
                    int? CompId = GetSMTPData.CompId;
                    string SMTPServer = GetSMTPData.SMTPServer;
                    int SMTPPort = Convert.ToInt32(GetSMTPData.SMTPPort);
                    string SMTPMailId = GetSMTPData.SMTPMailId;
                    string SMTPPassword = GetSMTPData.SMTPPassword;
                    string EmailId = GetSMTPData.EmailId;

                    //string link = "http://newofficeconnect.rim-global.com/UAT_OfficeConnect/#/verify_otp";
                    string link = DB.ServiceMasters.Where(x => x.ServiceName == "VisitorLink").Select(x => x.ServiceLink).FirstOrDefault();
                    string subject = "Successful Check-Out Confirmation at 3DCAD";
                    string body = $@"
                                <p>Dear {VisitorName},</p>
                                <p>We hope you had a pleasant visit at 3DCAD. This email is to confirm that your check-out process has been successfully completed.</p>
                                <p><strong>Visit Summary:</strong></p>
                                <p><strong>Date of Visit: </strong>{date}</p>
                                <p><strong>Check-In Time: </strong>{checkin}</p>
                                <p><strong>Check-Out Time: </strong>{checkout}</p>
                                <p><strong>Location: </strong>{locationMap}</p>
                                <p><strong>Host: </strong>Preetha E</p>
                                <p>During Your Visit:</p>
                                <p>If you need any assistance or have any questions during your visit, please do not hesitate to reach out to our front desk or your host.</p>
                                <p>Contact Information:</p>
                                <p>For any additional assistance, please contact us at Preetha E.</p>
                                <p>Thank you for visiting us, and we hope you have a pleasant experience at 3DCAD!</p>
                                <p>Best regards,</p>
                                <p>3DCAD</p>";
                    try
                    {
                        using (var message = new MailMessage())
                        {
                            message.From = new MailAddress(SMTPMailId);
                            message.To.Add(toEmail);
                            message.Subject = subject;

                            message.Body = body;
                            message.IsBodyHtml = true;

                            var smtpClient = new SmtpClient(SMTPServer)
                            {
                                Port = SMTPPort,
                                Credentials = new System.Net.NetworkCredential(SMTPMailId, SMTPPassword),
                                EnableSsl = true,
                            };

                            smtpClient.Send(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("Error sending email: " + ex.Message);
                    }
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
        public void SendEmail(string tomail, string subject, string msg)
        {
            try
            {

                var GetSMTPData = (from ES in DB.EmailSetUps
                                   select ES).FirstOrDefault();


                if (GetSMTPData != null)
                {
                    //EmailSetUpViewModel esvm = new EmailSetUpViewModel();
                    int? CompId = GetSMTPData.CompId;
                    string SMTPServer = GetSMTPData.SMTPServer;
                    int SMTPPort = Convert.ToInt32(GetSMTPData.SMTPPort);
                    string SMTPMailId = GetSMTPData.SMTPMailId;
                    string SMTPPassword = GetSMTPData.SMTPPassword;
                    string EmailId = GetSMTPData.EmailId;

                    try
                    {
                        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage
                        {
                            From = new System.Net.Mail.MailAddress(EmailId),
                            Subject = subject,
                            Body = msg
                        };

                        // Add recipients
                        message.To.Add(tomail);
                        //message.CC.Add(tomail); // Make sure 'ccmail' is defined

                        SmtpClient server = new SmtpClient(SMTPServer, SMTPPort)
                        {
                            EnableSsl = false,
                            UseDefaultCredentials = false,
                            Credentials = new System.Net.NetworkCredential(SMTPMailId, SMTPPassword),
                            Timeout = 100000
                        };

                        server.Send(message);
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Console.WriteLine("Error sending email: " + ex.Message);
                    }
                }
            }
            catch (CustomApiException ex)
            {
                throw new CustomApiException(ex.StatusCode, ex.Message);
            }
        }
    }
}