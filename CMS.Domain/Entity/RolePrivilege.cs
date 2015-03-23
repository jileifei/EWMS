//==============================================================================
//	CAUTION: This file is generated by IBatisNetGen.Entity.cst at 2011/9/14 21:40:03
//				Any manual editing will be lost in re-generation.
//==============================================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Domain {
	
    /// <summary><c>RolePrivilege</c> Business Object.</summary>
    [Serializable]
    public partial class RolePrivilege {
        
        #region ID

        private Int64 m_iD;
		
		/// <summary>Gets or sets ID</summary>
        public Int64 ID {
        	get { return m_iD; }
        	set { m_iD = value;}        
        }
		
	    #endregion
		
        #region RoleID

        private Int64 m_roleID;
		
		/// <summary>Gets or sets RoleID</summary>
        public Int64 RoleID {
        	get { return m_roleID; }
        	set { m_roleID = value;}        
        }
		
	    #endregion
		
        #region ModuleID

        private Int64 m_moduleID;
		
		/// <summary>Gets or sets ModuleID</summary>
        public Int64 ModuleID {
        	get { return m_moduleID; }
        	set { m_moduleID = value;}        
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
		
        #region CreateUserID

        private Int64 m_createUserID;
		
		/// <summary>Gets or sets CreateUserID</summary>
        public Int64 CreateUserID {
        	get { return m_createUserID; }
        	set { m_createUserID = value;}        
        }
		
	    #endregion
		

	}
	
}