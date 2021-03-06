//==============================================================================
//	CAUTION: This file is generated by IBatisNetGen.Entity.cst at 2011/9/14 21:40:03
//				Any manual editing will be lost in re-generation.
//==============================================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Domain {
	
    /// <summary><c>SystemLog</c> Business Object.</summary>
    [Serializable]
    public partial class SystemLog {
        
        #region ID

        private Int64 m_iD;
		
		/// <summary>Gets or sets ID</summary>
        public Int64 ID {
        	get { return m_iD; }
        	set { m_iD = value;}        
        }
		
	    #endregion
		
        #region LogType

        private Int32 m_logType;
		
		/// <summary>Gets or sets LogType</summary>
        public Int32 LogType {
        	get { return m_logType; }
        	set { m_logType = value;}        
        }
		
	    #endregion
		
        #region LogLevel

        private Int32 m_logLevel;
		
		/// <summary>Gets or sets LogLevel</summary>
        public Int32 LogLevel {
        	get { return m_logLevel; }
        	set { m_logLevel = value;}        
        }
		
	    #endregion
		
        #region OperatorUserID

        private Int64? m_operatorUserID;
		
		/// <summary>Gets or sets OperatorUserID</summary>
        public Int64? OperatorUserID {
        	get { return m_operatorUserID; }
        	set { m_operatorUserID = value;}        
        }
		
	    #endregion
		
        #region URL

        private String m_uRL;
		
		/// <summary>Gets or sets URL</summary>
        public String URL {
        	get { return m_uRL; }
        	set { m_uRL = value;}        
        }
		
	    #endregion
		
        #region OperatorDesc

        private String m_operatorDesc;
		
		/// <summary>Gets or sets OperatorDesc</summary>
        public String OperatorDesc {
        	get { return m_operatorDesc; }
        	set { m_operatorDesc = value;}        
        }
		
	    #endregion
		
        #region ErrorMessage

        private String m_errorMessage;
		
		/// <summary>Gets or sets ErrorMessage</summary>
        public String ErrorMessage {
        	get { return m_errorMessage; }
        	set { m_errorMessage = value;}        
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
