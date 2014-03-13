using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WeListenPlayer.AmazonHandler;
using WeListenPlayer.APIClasses;
using WeListenPlayer.FormHandler;

namespace WeListenPlayer.WeListenApiHandler
{
    class UserXmlParser
    {
            ///////////////////////////////////////////////////////
            // Request XML URL Handler
            // - Pulls XML page data and parses, returns objects
            //
            // - Uses       WeListenXmlParser k = new WeListenXmlParser();
            //              k.GetTrackInfo();
            // - Output     Returns object list of all songs pulled from API
            ///////////////////////////////////////////////////////
            public async Task<User> GetUserInfo(string user, string password)
            {
                // Create master / finalized list
                var userList = new List<User>();

                string baseURL;

                baseURL = "http://welistenmusic.com/api/user"; // Base default url

                string requestUrl = baseURL + "?username=" + user + "&password=" + password;

                string serviceResponse = await new XmlAccesser().GetServiceResponse(requestUrl);

                if (serviceResponse != null)
                {
                    var xDoc = XDocument.Parse(serviceResponse);
                    XNamespace ns = xDoc.Root.Name.Namespace;

                    var returnedUser = (from list in xDoc.Descendants(ns + "AuthenticatedUser")
                                    select new User
                                    {
                                        EmailAddress = (string)list.Element(ns + "Email"),
                                        FirstName = (string)list.Element(ns + "FirstName"),
                                        LastName = (string)list.Element(ns + "Album"),
                                        UserID = (int)list.Element(ns + "UserId"),
                                        Username = (string)list.Element(ns + "UserName"),
                                    });

                    if (returnedUser != null)
                    {
                        return (User)returnedUser;
                    }
                    return null;
                }
                return null;
            }
        }
    }