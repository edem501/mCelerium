using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using iCelerium.Models;
using iCelerium.Models.BodyClasses;
using System.Drawing;
namespace iCelerium.Controllers
{
    [Authorize(Roles = "User,Admin,Super Admin")]
    [Audit]
    public class VotesController : Controller
    {
        private readonly SMSServersEntities db = new SMSServersEntities();

        public JsonResult LoadPollingByConstituency(string ConstituencyID)
        {
            List<SelectListItem> lstPolling = new List<SelectListItem>();
            foreach (PollingStation cty in db.PollingStations.Where(c => c.ElectoralArea.Constituency1.ConstituencyNo == ConstituencyID).ToList())
            {
                lstPolling.Add(new SelectListItem { Text = cty.PollingStationName, Value = cty.ID.ToString() });
            }
            return Json(lstPolling, JsonRequestBehavior.AllowGet);
        }
        // GET: Votes
        public ActionResult Index()
        {
            List<CollationModelView> voteCollationModelView = new List<CollationModelView>();

           
            List<CollationEntry> lstCol = new List<CollationEntry>();
            foreach (Constituency cons in db.Constituencies.ToList())
            {
               lstCol = db.CollationEntries.Where(c=>c.ConstituencyID.Equals(cons.ConstituencyNo)).ToList();
                    if (lstCol.Count() > 0)
                    {
                        CollationModelView vot = new CollationModelView();
                        foreach (CollationEntry col in lstCol)
                        {
                           
                            switch (col.PartyID)
                            {
                        case 1:
                            vot.NDC += col.NumberOfVote;
                            break;
                        case 2:
                            vot.NPP += col.NumberOfVote;
                            break;
                        case 3:
                            vot.CPP += col.NumberOfVote;
                            break;
                        case 4:
                            vot.PPP += col.NumberOfVote;
                            break;
                        case 5:
                            vot.OTHERS += col.NumberOfVote;
                            break;
                        case 6:
                            vot.REJECTED += col.NumberOfVote;
                            break;

                    }
                    vot.ConstituencyID = cons.ConstituencyName;
                            
                }
                        vot.TOTAL = vot.NDC + vot.NPP + vot.OTHERS + vot.PPP + vot.REJECTED+ vot.CPP ;
               voteCollationModelView.Add(vot);
            }
            }
           


            return View(voteCollationModelView);
        }
        public ActionResult Edit(String ConstituencyID)
        {
            AspNetUser aspnetuser = db.AspNetUsers.Where(u => u.UserName.Equals(User.Identity.Name)).First();
            Agent agent = aspnetuser.Agent;

            if (agent == null)
            {
                return RedirectToAction("Edit", "Account", new { ID = aspnetuser.Id });
            }
            else
            {
                //IEnumerable<CollationEntry> exep;
                //exep = db.CollationEntries.ToList().GroupBy(c => c.ConstituencyID).Select(group => group.First());

                List<SelectListItem> itemConstituencies = new List<SelectListItem>();
                List<SelectListItem> itemPolling = new List<SelectListItem>();

                //List<vConstituency> lstConst = db.vConstituencies.Where(c => c.RegionID == agent.RegionID).ToList();
                foreach (var cst in db.Constituencies.Where(c=>c.District.RegionID==agent.RegionID))
                {
                    itemConstituencies.Add(new SelectListItem { Value = cst.ConstituencyNo, Text = cst.ConstituencyName });
                }
               
                VoteCollationModelView lstCol = new VoteCollationModelView();

                ViewBag.itemConstituencies = itemConstituencies;
                ViewBag.itemPolling = itemPolling;
                return View(lstCol);
            }
        }
//POST: Votes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VoteCollationModelView model)
        {

            if (ModelState.IsValid)
            {
                String AgentID = db.AspNetUsers.Where(u => u.UserName.Equals(User.Identity.Name)).First().Agent.UserID;
                List<CollationEntry> check = db.CollationEntries.Where(o => o.PollingStationID == model.PollingStationID).ToList();
                if (check.Count() == 0)
                {
                    CollationEntry NDCEntry = new CollationEntry();
                    NDCEntry.AgentID = AgentID;
                    NDCEntry.ConstituencyID = model.ConstituencyID;
                    NDCEntry.DateSend = DateTime.Now;
                    NDCEntry.PartyID = db.Parties.Where(c => c.Abrev.Equals("NDC")).FirstOrDefault().ID;
                    NDCEntry.NumberOfVote = model.NDC;
                    NDCEntry.PollingStationID = model.PollingStationID;



                    CollationEntry NPPEntry = new CollationEntry();
                    NPPEntry.AgentID = AgentID;
                    NPPEntry.ConstituencyID = model.ConstituencyID;
                    NPPEntry.DateSend = DateTime.Now;
                    NPPEntry.PartyID = db.Parties.Where(c => c.Abrev.Equals("NPP")).FirstOrDefault().ID;
                    NPPEntry.NumberOfVote = model.NPP;
                    NPPEntry.PollingStationID = model.PollingStationID;

                    CollationEntry CPPEntry = new CollationEntry();
                    CPPEntry.AgentID = AgentID;
                    CPPEntry.ConstituencyID = model.ConstituencyID;
                    CPPEntry.DateSend = DateTime.Now;
                    CPPEntry.PartyID = db.Parties.Where(c => c.Abrev.Equals("CPP")).FirstOrDefault().ID;
                    CPPEntry.NumberOfVote = model.CPP;
                    CPPEntry.PollingStationID = model.PollingStationID;

                    CollationEntry PPPEntry = new CollationEntry();
                    PPPEntry.AgentID = AgentID;
                    PPPEntry.ConstituencyID = model.ConstituencyID;
                    PPPEntry.DateSend = DateTime.Now;
                    PPPEntry.PartyID = db.Parties.Where(c => c.Abrev.Equals("PPP")).FirstOrDefault().ID;
                    PPPEntry.NumberOfVote = model.PPP;
                    PPPEntry.PollingStationID = model.PollingStationID;

                    CollationEntry OTHERSEntry = new CollationEntry();
                    OTHERSEntry.AgentID = AgentID;
                    OTHERSEntry.ConstituencyID = model.ConstituencyID;
                    OTHERSEntry.DateSend = DateTime.Now;
                    OTHERSEntry.PartyID = db.Parties.Where(c => c.Abrev.Equals("OTH")).FirstOrDefault().ID;
                    OTHERSEntry.NumberOfVote = model.OTHERS;
                    OTHERSEntry.PollingStationID = model.PollingStationID;

                    CollationEntry REJECTEDEntry = new CollationEntry();
                    REJECTEDEntry.AgentID = AgentID;
                    REJECTEDEntry.ConstituencyID = model.ConstituencyID;
                    REJECTEDEntry.DateSend = DateTime.Now;
                    REJECTEDEntry.PartyID = db.Parties.Where(c => c.Abrev.Equals("IRB")).FirstOrDefault().ID;
                    REJECTEDEntry.NumberOfVote = model.REJECTED;
                    REJECTEDEntry.PollingStationID = model.PollingStationID;

                    db.CollationEntries.Add(NDCEntry);
                    db.CollationEntries.Add(NPPEntry);
                    db.CollationEntries.Add(CPPEntry);
                    db.CollationEntries.Add(PPPEntry);
                    db.CollationEntries.Add(OTHERSEntry);
                    db.CollationEntries.Add(REJECTEDEntry);
                    
                    db.SaveChanges();
                    return this.RedirectToAction("Index");
                }
                else
                {
                    throw new HttpException(string.Format("The Polling Station {0} has already posted", db.PollingStations.Find(model.PollingStationID).PollingStationName));
                }
                
               
            }
            else
            {
                return View(model);
            }

        }

        public ActionResult _VotePerContituency(int regionID)
        {
            return View();
        }

        public ActionResult VoteRegional(String RegionName)
        {
            var vm = new RegionDashboardViewModel
            {
                Chart1 = this.TransDaily(RegionName),
                Chart2 = this.PieData(RegionName),
                Chart3 = this.ConstiTable(RegionName)
            };
            //ViewBag.SumCons = vm.Chart3.Sum(c => c.NoConstituency);
            ViewBag.SumNDC = vm.Chart3.Sum(c => c.NDC);
            ViewBag.SumNPP = vm.Chart3.Sum(c => c.NPP);
            ViewBag.SumCPP = vm.Chart3.Sum(c => c.CPP);
            ViewBag.SumPPP = vm.Chart3.Sum(c => c.PPP);
            ViewBag.SumOTHERS = vm.Chart3.Sum(c => c.OTHERS);
            ViewBag.REJECTED = vm.Chart3.Sum(c => c.REJECTED);
            ViewBag.SumTOTAL = vm.Chart3.Sum(c => c.TOTAL);

            return this.View(vm);
           
        }
        public List<CollationModelView> ConstiTable(String RegionName)
        {
            iCelerium.Models.Region region = new iCelerium.Models.Region();
            region = db.Regions.Where(o => o.RegionName.Equals(RegionName.Trim())).FirstOrDefault();
            if (region != null)
            {
                ViewBag.Reg = region.RegionName;
                List<CollationModelView> voteCollationModelView = new List<CollationModelView>();


                List<CollationEntry> lstCol = new List<CollationEntry>();
                foreach (Constituency cons in db.Constituencies.Where(o => o.District.RegionID == region.ID).ToList())
                {
                    lstCol = db.CollationEntries.Where(c => c.ConstituencyID.Equals(cons.ConstituencyNo)).ToList();
                    if (lstCol.Count() > 0)
                    {
                        CollationModelView vot = new CollationModelView();
                        foreach (CollationEntry col in lstCol)
                        {

                            switch (col.PartyID)
                            {
                                case 1:
                                    vot.NDC += col.NumberOfVote;
                                    break;
                                case 2:
                                    vot.NPP += col.NumberOfVote;
                                    break;
                                case 3:
                                    vot.CPP += col.NumberOfVote;
                                    break;
                                case 4:
                                    vot.PPP += col.NumberOfVote;
                                    break;
                                case 5:
                                    vot.OTHERS += col.NumberOfVote;
                                    break;
                                case 6:
                                    vot.REJECTED += col.NumberOfVote;
                                    break;

                            }
                            vot.ConstituencyID = cons.ConstituencyName;

                        }
                        vot.TOTAL = vot.NDC + vot.NPP + vot.OTHERS + vot.PPP + vot.REJECTED + vot.CPP;
                        voteCollationModelView.Add(vot);
                    }
                }



                return voteCollationModelView;
            }
            else
            {
                return new List<CollationModelView>();
            }
            
        }
        public Highcharts TransDaily(String RegionName)
        {
            SMSServersEntities tp = new SMSServersEntities();
            List<Series> sr = new List<Series>();
            foreach (var sp in db.spVotePartyRegionTransPie(RegionName).ToList())
            {
                sr.Add(new Series { Name = sp.Abrev, Data = new Data(new object[] { Math.Round(sp.Total,2) }) });
            }

            Highcharts chart1 = new Highcharts("chart1")
               .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
               .SetTitle(new Title { Text = "Total votes per party" })
               .SetSubtitle(new Subtitle { Text = String.Format("Summary for: {0}" ,RegionName)})
               .SetXAxis(new XAxis { Categories = new[] { RegionName } })
               .SetYAxis(new YAxis
               {
                   Min = 0,
                   Title = new YAxisTitle { Text = "Votes (%)" }
               })
               .SetLegend(new Legend
               {
                   Layout = Layouts.Vertical,
                   Align = HorizontalAligns.Left,
                   VerticalAlign = VerticalAligns.Top,
                   X = 100,
                   Y = 70,
                   Floating = true,
                   BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
                   Shadow = true
               })
               .SetTooltip(new Tooltip { Formatter = @"function() { return ''+ this.series.name +': '+ this.y +' %'; }" })
               .SetPlotOptions(new PlotOptions
               {
                   Column = new PlotOptionsColumn
                   {
                       PointPadding = 0.2,
                       BorderWidth = 0
                   }
               })
               .SetSeries(sr.ToArray());

            return chart1;
        }

        public Highcharts PieData(String RegionName)
        {
            List<ZoneData> data = new List<ZoneData>();
            SMSServersEntities tp = new SMSServersEntities();
            string tDate = DateTime.Today.ToString("MM/dd/yyyy");

            var tran = tp.spVotePartyRegionTransPie(RegionName).OrderBy(c => c.Abrev);

            foreach (var iTra in tran.ToList())
            {
                data.Add(new ZoneData { ZoneName = iTra.Abrev, Total = (decimal)Math.Round(iTra.Total, 2) });
            }
            object[,] chartData = new object[data.Count, 2];
            int i = 0;
            foreach (ZoneData item in data)
            {
                chartData.SetValue(item.ZoneName, i, 0);
                chartData.SetValue(item.Total, i, 1);
                i++;
            }

            Data dt = new Data(chartData);

            Highcharts chart = new Highcharts("chart")
                                                      .InitChart(new Chart { PlotShadow = false })
                                                      .SetTitle(new Title { Text = String.Format("Shares of votes as at {0}", DateTime.Now) })
                                                      .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage.toFixed(2) +' %'; }" })
                                                      .SetPlotOptions(new PlotOptions
                                                      {
                                                          Pie = new PlotOptionsPie { AllowPointSelect = true, Cursor = Cursors.Pointer, DataLabels = new PlotOptionsPieDataLabels { Color = ColorTranslator.FromHtml("#000000"), ConnectorColor = ColorTranslator.FromHtml("#000000"), Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ Math.round(this.percentage) +' %'; }" } },
                                                      })
                                                      .SetSeries(new Series
                                                      {
                                                          Type = ChartTypes.Pie,
                                                          Name = "Browser share",
                                                          Data = dt
                                                      });
            return chart;
        }
    }
}
