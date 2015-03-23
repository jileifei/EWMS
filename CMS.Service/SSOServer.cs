using System.Collections.Generic;
using System.Globalization;
using CMS.CommonLib.Encrypt;
using CMS.Domain;
using CMS.DataAccess;
using CMS.DataAccess.Interface;

namespace CMS.Service
{
    public class SSOServer
    {
        /// <summary>
        /// 根据用户名密码得到用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByUserNameAndPassword(string userName, string password)
        {
            UserInfo userEntity = null;
            IUserInfoDao userDao = CastleContext.Instance.GetService<IUserInfoDao>();
            IList<UserInfo> userList = userDao.FindByUserName(userName);
            if (userList != null)
            {
                foreach (UserInfo userItem in userList)
                {
                    if (userItem.Password == DES.Encrypt(password))
                    {
                        userEntity = userItem;
                        userEntity.Token = GenToken(userEntity.ID.ToString(CultureInfo.InvariantCulture), userEntity.Password);
                        break;
                    }
                }
            }
            return userEntity;
        }

        /// <summary>
        /// 生成登录令牌
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="userPassword">用户名称</param>
        /// <returns></returns>
        public string GenToken(string userID, string userPassword)
        {
            string token = ASPMD5.MDString(userID + userPassword + "DTPWD");
            return token;
        }

        /// <summary>
        /// 防止跨站攻击
        /// </summary>
        /// <param name="curUserInfo"></param>
        /// <returns></returns>
        public bool ProtectXSS(UserInfo curUserInfo)
        {
            UserService service = new UserService();
            UserInfo lastUserInfo = service.GetUserInfoByID(curUserInfo.ID);
            string lastToken = GenToken(lastUserInfo.ID.ToString(CultureInfo.InvariantCulture), lastUserInfo.Password);
            if (curUserInfo.Token == lastToken)
            {
                return true;
            }
            return false;
        }
    }
}
