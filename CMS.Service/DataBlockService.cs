using System;
using System.Collections.Generic;
using System.Data;
using CMS.Domain;
using CMS.CommonLib.Utils;
using CMS.DataAccess;
using CMS.DataAccess.SQLHelper;
using CMS.DataAccess.Interface;

namespace CMS.Service
{
    /// <summary>
    /// 数据块
    /// </summary>
    public class DataBlockService
    {
        # region add update delete select

        /// <summary>
        /// add datablock
        /// </summary>
        /// <param name="datablockInfo"></param>
        /// <returns></returns>
        public Int64 AddDataBlock(DataBlock datablockInfo)
        {
            IDataBlockDao datablockDao = CastleContext.Instance.GetService<IDataBlockDao>();
            return datablockDao.Insert(datablockInfo);
        }

        /// <summary>
        /// update datablock
        /// </summary>
        /// <param name="datablockInfo"></param>
        /// <returns></returns>
        public bool UpdateDataBlock(DataBlock datablockInfo)
        {
            IDataBlockDao datablockDao = CastleContext.Instance.GetService<IDataBlockDao>();
            datablockDao.Update(datablockInfo);
            return true;
        }

        /// <summary>
        /// delete datablock
        /// </summary>
        /// <param name="datablockInfo"></param>
        /// <returns></returns>
        public bool DeleteDataBlock(DataBlock datablockInfo)
        {
            IDataBlockDao datablockDao = CastleContext.Instance.GetService<IDataBlockDao>();
            datablockDao.Delete(datablockInfo);
            return true;
        }

        /// <summary>
        /// select by ID
        /// </summary>
        /// <param name="dataBlockID"></param>
        /// <returns></returns>
        public DataBlock GeDataBlockInfo(long dataBlockID)
        {
            TemplateService tpService = new TemplateService();
            IDictionary<long, string> dicTemplate = tpService.GetDicTemplateList();
            IDataBlockDao datablockDao = CastleContext.Instance.GetService<IDataBlockDao>();
            DataBlock dbEntity = datablockDao.Find(dataBlockID);
            if (dbEntity != null)
            {
                if (dicTemplate.ContainsKey(dbEntity.TemplateID))
                {
                    dbEntity.TemplateName = dicTemplate[dbEntity.TemplateID];
                }
            }
            return dbEntity;
        }

        # endregion

        # region GetDataBlockList

        /// <summary>
        /// get datablock list
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <returns></returns>
        public PagerModel<DataBlock> GetPagerDataBlockList(int pageSize, int pageIndex)
        {
            PagerModel<DataBlock> datablockPagerEntity = new PagerModel<DataBlock>();
            using (SQLHelper dao = new SQLHelper())
            {
                string where = " Status=1 ";

                int totalCount;
                int totalPager;
                DataSet dsDataBlockList = dao.PageingQuery("DataBlock", "DataBlockID,BlockName,[Type],[RowCount],AddDate", "DataBlockID", where, "", 2, pageSize, pageIndex, out totalCount, out totalPager);
                datablockPagerEntity.PageSize = pageSize;
                datablockPagerEntity.CurrentPage = pageIndex;
                datablockPagerEntity.TotalRecords = totalCount;
                datablockPagerEntity.ItemList = MakeDataBlockList(dsDataBlockList);
            }
            return datablockPagerEntity;
        }

        /// <summary>
        /// Convert DataSet To IList 
        /// </summary>
        /// <param name="dsDataBlockList"></param>
        /// <returns></returns>
        private IList<DataBlock> MakeDataBlockList(DataSet dsDataBlockList)
        {
            IList<DataBlock> dataBlockList = new List<DataBlock>(10);
            
            foreach (DataRow row in dsDataBlockList.Tables[0].Rows)
            {
                DataBlock dataBlockEntity = new DataBlock
                {
                    ID = TypeParse.ToLong(row["DataBlockID"]),
                    BlockName = row["BlockName"].ToString(),
                    Type = TypeParse.ToInt16(row["Type"].ToString(), 0),
                    RowCount = TypeParse.ToInt(row["RowCount"]),
                    AddDate = TypeParse.ToDateTime(row["AddDate"])
                };
                dataBlockList.Add(dataBlockEntity);
            }
            return dataBlockList;
        }

        # endregion
    }
}
