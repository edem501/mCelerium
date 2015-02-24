using iCelerium.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;

namespace iCelerium.Controllers
{
    [Audit]
    public class MobileController : ApiController
    {
        // GET: api/Mobile
        public class result
        {
            public int Result { get; set; }
        }

        SMSServersEntities db = new SMSServersEntities();
        public result Get(String username, String password)
        {
            result respo = new result();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var cUser = userManager.Find(username,password);

            if (cUser != null && db.Agents.Where(c=>c.UserID.Equals(cUser.Id)).First().IsEnable)
            {
                respo.Result = 1;
            }
            else
            {
                respo.Result = 0;
            }

            return respo;
        }

        public result Get(String username, String password,String newpassword)
        {
            result respo = new result();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var cUser = userManager.Find(username, password);

            if (cUser != null && db.Agents.Where(c => c.UserID.Equals(cUser.Id)).First().IsEnable)
            {

                userManager.ChangePassword(cUser.Id, password, newpassword);
                respo.Result = 1;
            }
            else
            {
                respo.Result = 0;
            }

            return respo;
        }

        public result Get(String username, String password,int partyID,int numbers)
        {
            result respo = new result();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var cUser = userManager.Find(username, password);
            CollationEntry collationEntry = new CollationEntry();
            Agent agent = new Agent();
            agent = db.Agents.Where(c => c.UserID.Equals(cUser.Id)).First();

            if (cUser != null && agent.IsEnable)
            {
                collationEntry.AgentID = agent.UserID;
                collationEntry.ConstituencyID = db.Constituencies.Find(agent.ContituencyID).ConstituencyNo;
                collationEntry.DateSend = DateTime.Now;
                collationEntry.NumberOfVote = numbers;
                collationEntry.PartyID = partyID;
                collationEntry.PollingStationID = agent.PollingStationID;
                

                if (db.CollationEntries.Where(c => c.PollingStationID.Equals(collationEntry.PollingStationID) && c.PartyID==collationEntry.PartyID).Count() == 0 )
                {
                    db.CollationEntries.Add(collationEntry);
                    db.SaveChanges();
                    respo.Result = 1;
                }
                else
                {
                    respo.Result = 0;
                }

            }
            else
            {
                respo.Result = 0;
            }

            return respo;
        }

    }
}
