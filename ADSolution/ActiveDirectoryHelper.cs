using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectoryHelper
{
    class ActiveDirectoryHelper
    {
        private DirectoryEntry _directoryEntry;

        private String LDAPPath
        {
            get
            {
                return ConfigurationManager.AppSettings["LDAPPath"];
            }
        }
        private String LDAPUser
        {
            get
            {
                return ConfigurationManager.AppSettings["LDAPUser"];
            }
        }
        private String LDAPPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["LDAPPassword"];
            }
        }
        private DirectoryEntry SearchRoot
        {
            get
            {
                if (_directoryEntry == null)
                {
                    _directoryEntry = new DirectoryEntry(LDAPPath); //, LDAPUser, LDAPPassword, AuthenticationTypes.Secure);
                }
                return _directoryEntry;
            }
        }
        public ADUserDetail GetUserByLoginName(String userName)
        {
            try
            {
               
                DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
                directorySearch.Filter = "(&(objectClass=user)(SAMAccountName=" + userName + "))";
                SearchResult results = directorySearch.FindOne();
                if (results != null)
                {
                    DirectoryEntry user = new DirectoryEntry(results.Path);//, LDAPUser, LDAPPassword);
                    return ADUserDetail.GetUser(user);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ADUserDetail GetUserByFullName(String userName)
        {
            try
            {
                _directoryEntry = null;
                DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
                directorySearch.Filter = "(&(objectClass=user)(cn=" + userName + "))";
                SearchResult results = directorySearch.FindOne();
                if (results != null)
                {
                    DirectoryEntry user = new DirectoryEntry(results.Path);//, LDAPUser, LDAPPassword);
                    return ADUserDetail.GetUser(user);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ADUserDetail> GetUserFromGroup(String groupName)
        {
            List<ADUserDetail> userlist = new List<ADUserDetail>();
            try
            {

                DirectorySearcher directorySearch = new DirectorySearcher(SearchRoot);
                directorySearch.Filter = "(&(objectClass=group)(SAMAccountName=" + groupName + "))";
                SearchResult results = directorySearch.FindOne();
                if (results != null)
                {
                    DirectoryEntry deGroup = new DirectoryEntry(results.Path);
                    System.DirectoryServices.PropertyCollection pColl = deGroup.Properties;
                    int count = pColl["member"].Count;
                    for (int i = 0; i < count; i++)
                    {
                        string respath = results.Path;
                        string[] pathnavigate = respath.Split("CN".ToCharArray());
                        respath = pathnavigate[0];
                        string objpath = pColl["member"][i].ToString();
                        string path = respath + objpath;
                        DirectoryEntry user = new DirectoryEntry(path);
                        ADUserDetail userobj = ADUserDetail.GetUser(user);
                        userlist.Add(userobj);
                        user.Close();
                    }
                }
                return userlist;
            }
            catch (Exception ex)
            {
                return userlist;
            }
        }
    }
}
