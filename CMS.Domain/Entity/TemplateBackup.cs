//==============================================================================
//	CAUTION: This file is generated by IBatisNetGen.Entity.cst at 2011/9/14 21:40:03
//				Any manual editing will be lost in re-generation.
//==============================================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Domain {
	
    /// <summary><c>TemplateBackup</c> Business Object.</summary>
    [Serializable]
    public partial class TemplateBackup {
        
        #region ID

        private Int64 m_iD;
		
		/// <summary>Gets or sets ID</summary>
        public Int64 ID {
        	get { return m_iD; }
        	set { m_iD = value;}        
        }
		
	    #endregion
		
        #region TemplateID

        private Int64 m_templateID;
		
		/// <summary>Gets or sets TemplateID</summary>
        public Int64 TemplateID {
        	get { return m_templateID; }
        	set { m_templateID = value;}        
        }
		
	    #endregion
		
        #region TemplateCode

        private String m_templateCode;
		
		/// <summary>Gets or sets TemplateCode</summary>
        public String TemplateCode {
        	get { return m_templateCode; }
        	set { m_templateCode = value;}        
        }
		
	    #endregion
		
        #region CreateTime

        private DateTime m_createTime;
		
		/// <summary>Gets or sets CreateTime</summary>
        public DateTime CreateTime {
        	get { return m_createTime; }
        	set { m_createTime = value;}        
        }
		
	    #endregion
		

	}
	
}