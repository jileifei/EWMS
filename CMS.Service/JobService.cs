using System;
using System.Collections.Generic;
using CMS.DataAccess;
using CMS.Domain;
using CMS.DataAccess.Interface;
using System.Configuration;

namespace CMS.Service
{
    public class JobService
    {
        #region jobinfo position
        public static String FindAllUnion(Int64 pid, int rows, int currentPage,string order="id",string sort="desc")
        {
            IResumeDao resumeService = CastleContext.Instance.GetService<IResumeDao>();
            IList<Resume> resumeList;
            if (pid > 0)
            {
                resumeList = resumeService.FindAllUnionByjobID(pid);
            }
            else
            {
                resumeList = resumeService.FindAllUnion();
            }


            int rcount = resumeList.Count;
            int starrecode = (currentPage - 1) * rows;
            int endrecode = currentPage * rows;
            if (rcount < endrecode)
            {
                endrecode = rcount;
            }
            IList<Resume> resumeList2 = new List<Resume>();
            for (int i = starrecode; i < endrecode; i++)
            {
                Resume efi;
                efi = resumeList[i];
                resumeList2.Add(efi);
            }


            string strJson = "";
            string docUrl=ConfigurationSettings.AppSettings["ResumeDocUrl"];
            foreach (Resume resume in resumeList2)
            {
                strJson += "{\"ID\":\"" + resume.ID + "\",\"JobName\":\"" + resume.JobInfomation.Name + "\",\"Name\":\"" + resume.Name + "\",\"Mobile\":\"" + resume.Mobile + "\",\"CreateTime\":\"" + resume.CreateTime + "\",\"Status\":\"" + resume.Status + "\",";
                strJson += "\"Email\":\"" + resume.Email + "\",\"telphone\":\"" + resume.Telphone + "\",\"ResumeText\":\"" + resume.Text + "\",\"FileID\":\"" + resume.FileID + "\",\"employee_number\":\"" + resume.JobInfomation.EmployeeNumber + "\",\"job_description\":\"" + resume.JobInfomation.JobDescription + "\",\"responsbility\":\"" + resume.JobInfomation.Responsbility + "\",\"docUrl\":\""+docUrl+"\"},";
            }
            if (resumeList.Count > 0)
                strJson = "{\"total\":" + resumeList.Count + ",\"rows\":[" + strJson.Remove(strJson.Length - 1) + "]}";
            else
                strJson = "{\"total\":" + resumeList.Count + ",\"rows\":[]}";
            return strJson;
        }

        public static JobInfo FindByID(int ID)
        {
            IJobInfoDao jobInfoService = CastleContext.Instance.GetService<IJobInfoDao>();
            JobInfo jobInfo=jobInfoService.Find(ID);
            return jobInfo;
        }

        public static int UpdateJobInfo(JobInfo info)
        {
            IJobInfoDao jobInfoService = CastleContext.Instance.GetService<IJobInfoDao>();
            int count=jobInfoService.Update(info);
            return count;
        }

        public static void AddPosition(JobInfo info)
        {
            IJobInfoDao jobInfoService = CastleContext.Instance.GetService<IJobInfoDao>();
            jobInfoService.Insert(info);
        }

        public static IList<JobInfo> FindAllPosition()
        {
            IJobInfoDao jobInfoService = CastleContext.Instance.GetService<IJobInfoDao>();
            IList<JobInfo> jobinfoList = jobInfoService.FindAll();
            return jobinfoList;
        }

        public static int Delete(Int64 id)
        {
            IJobInfoDao jobInfoService = CastleContext.Instance.GetService<IJobInfoDao>();
            return jobInfoService.Delete(id);            
        }
        #endregion

        #region resume

        public static int GetCount(Int64 id)
        {
            IResumeDao re = CastleContext.Instance.GetService<IResumeDao>();
            return re.FindByPositionID(id).Count;
        }

        public static int UpdateResume(Resume resume)
        {
            IResumeDao resumeService = CastleContext.Instance.GetService<IResumeDao>();
            return resumeService.Update(resume);
        }

        public static int DeleteResume(Resume resume) 
        {
            IResumeDao resumeService = CastleContext.Instance.GetService<IResumeDao>();
            return resumeService.Delete(resume);
        }

        public static Resume GetResumeDetail(Int64 id)
        {
            IResumeDao resumeService = CastleContext.Instance.GetService<IResumeDao>();
            return resumeService.FindAllDetail(id);
        }

        public static void InserResume(Resume resume)
        {
            IResumeDao resumeService = CastleContext.Instance.GetService<IResumeDao>();
            resumeService.Insert(resume);
        }

        #endregion

        #region jobtype
        public static IList<JobType> FindAll()
        {
            IJobTypeDao jobTypeService = CastleContext.Instance.GetService<IJobTypeDao>();
            IList<JobType> jobTypeList = jobTypeService.FindAll();
            return jobTypeList;
        }

        public static JobType FindTypeByID(int id)
        {
            IJobTypeDao jobTypeService = CastleContext.Instance.GetService<IJobTypeDao>();
            JobType jobType = jobTypeService.Find(id);
            return jobType;
        }

        public static int UpdateTypeInfo(JobType info)
        {
            IJobTypeDao jobInfoService = CastleContext.Instance.GetService<IJobTypeDao>();
            int count = jobInfoService.Update(info);
            return count;
        }

        public static void AddType(JobType info)
        {
            IJobTypeDao jobInfoService = CastleContext.Instance.GetService<IJobTypeDao>();
            jobInfoService.Insert(info);
        }

        public static void DelType(JobType info)
        {
            IJobTypeDao jobInfoService = CastleContext.Instance.GetService<IJobTypeDao>();
            jobInfoService.Delete(info);
        }
        #endregion

    }
}
