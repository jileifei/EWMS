using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using CMS.Domain;
using CMS.Service;
using Webdiyer.WebControls.Mvc;
using CMS.CommonLib.Utils;
using System.Configuration;

namespace CMS.AdminUI.Controllers
{
    public class JobController : BaseController
    {
        //
        // GET: /Job/

        public ActionResult Index(Int64 id=0)
        {
            ViewData["pid"] = id;
            return View();
        }

        public ActionResult Position(int? id)
        {
            SetDropDownList();
            JobInfo job;
            if (id>0)
            {
                job = JobService.FindByID(Convert.ToInt32(id));
                if (job == null)
                {
                    job = new JobInfo();
                }
            }
            else
            {
                job = new JobInfo();
            }
            ViewData["message"] = "";
            return View(job);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Position(FormCollection collection)
        {
            JobInfo jobInfo = new JobInfo();
            jobInfo.ID = Convert.ToInt32(collection["ID"]);
            jobInfo.EmployeeNumber =Convert.ToInt32(collection["Employeenumber"]);
            jobInfo.JobDescription = collection["Jobdescription"];
            jobInfo.Name = collection["Name"];
            jobInfo.Place = collection["Place"];
            jobInfo.EndDate=Convert.ToDateTime(collection["EndDate"]);
            jobInfo.Publicdate = DateTime.Now;
            jobInfo.Responsbility = collection["Responsbility"];
            jobInfo.JobTypeID =Convert.ToInt64(collection["jobType"]);
            jobInfo.Sort = Convert.ToInt32(collection["sort"]);
            jobInfo.Status = Convert.ToInt32(collection["status"]);
            jobInfo.ChiefMail = collection["chiefmail"];
            if (jobInfo.ID > 0)
            {
                int count = JobService.UpdateJobInfo(jobInfo);
                if (count > 0)
                    ViewData["message"] = "更新成功";
                else
                    ViewData["message"] = "更新失败";
            }
            else
            {                
                  JobService.AddPosition(jobInfo);
                  ViewData["message"] = "添加成功";
            }
            SetDropDownList();
            return View(jobInfo);
        }

        public ActionResult Recruitment(int? id)
        {
            IList<JobInfo> jobinfoList = JobService.FindAllPosition();
            foreach (JobInfo info in jobinfoList)
            {
                info.RCount = JobService.GetCount(info.ID);
            }
            PagedList<JobInfo> pagedList = new PagedList<JobInfo>(jobinfoList, id ?? 1, 20);
            return View(pagedList);
        }

        public void SetDropDownList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            IList<JobType> jobTypeList = JobService.FindAll();
            foreach (JobType jobtype in jobTypeList)
            {
                items.Add(new SelectListItem { Text = jobtype.Name, Value = jobtype.ID.ToString(CultureInfo.InvariantCulture) });
            }
            ViewData["list"] = items; 
        }

        public ActionResult AjaxGetResume(Int64 id, int rows, int page,string order="id",string sort="desc")
        {
            string strJson=JobService.FindAllUnion(id,rows,page,order,sort);
            return Content(strJson);
        }

        public ActionResult Delete(int id)
        {
            int count = JobService.Delete(id);
            if (count > 0)
                return Content("success");
            return Content("false");
        }

        public ActionResult Detail(Int64 id)
        {
            Resume resume=JobService.GetResumeDetail(id);
            ViewData["weburl"]= ConfigurationManager.AppSettings["ResumeDocUrl"];
            return View(resume);
        }

        #region Resume
        public ActionResult UpdateResume(int id,int status)
        {
            Resume resume = new Resume {ReviewUserID = UserID, ReviewTime = DateTime.Now, ID = id, Status = status};
            int count = JobService.UpdateResume(resume);
            if (count > 0)
                return Content("success");
            return Content("failed");
        }

        public ActionResult DeleteResume(int id)
        {
            Resume resume = new Resume {ID = id};
            int count = JobService.DeleteResume(resume);
            if (count > 0)
                return Content("success");
            return Content("failed");
        }
        #endregion

        #region jobtype
        public ActionResult JobTypeList(int? id)
        {
            IList<JobType> jobTypeList = JobService.FindAll();
            PagedList<JobType> pagedList = new PagedList<JobType>(jobTypeList, id ?? 1, 6);
            return View(pagedList);
        }

        public ActionResult GetTypeInfo(Int32 id)
        {
            JobType job;
            if (id > 0)
            {
                job = JobService.FindTypeByID(id) ?? new JobType();
            }
            else
            {
                job = new JobType();
            }
            return Json(job, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteType(Int32 id)
        {
            JobType jobInfo = new JobType();
            jobInfo.ID = id;
            JobService.DelType(jobInfo);
            return Content("success");
        }

        [HttpPost]
        public ActionResult EditType(Int32 id,string typeName,string typeDesc)
        {
            JobType jobInfo = new JobType();
            jobInfo.ID = id;
            jobInfo.Name = typeName;
            jobInfo.Description = typeDesc;
            if (jobInfo.ID > 0)
            {
                int count = JobService.UpdateTypeInfo(jobInfo);
                if (count > 0)
                    return Content("success");
                return Content("false");
            }
            JobService.AddType(jobInfo);
            return Content("success");
            //SetDropDownList();
        }
        #endregion

        #region send mail
        public ActionResult SendMail(string ty,int id, string mailTo,string title, string content)
        {
            string[] adminMail = ConfigurationManager.AppSettings["AdminMail"].Split(',');
            string smtp = ConfigurationManager.AppSettings["SmtpHost"];
            string sendMsg = Utils.SendMails(smtp, adminMail[0], adminMail[1], adminMail[0], mailTo, content, title);
            if (sendMsg == "success")
            {
                Resume resume = new Resume();
                resume.ReviewUserID = UserID;
                resume.ReviewTime = DateTime.Now;
                resume.ID = id;
                if (ty == "1")
                {
                    resume.Status = 4;
                }
                else
                {
                    resume.Status = 5;
                }
                JobService.UpdateResume(resume);
            }
            return Content(sendMsg);
        }
        #endregion
    }
}
