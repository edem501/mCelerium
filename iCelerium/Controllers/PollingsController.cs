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
    [Audit]
    public class PollingsController : Controller
    {

        SMSServersEntities db = new SMSServersEntities();
        // GET: Pollings
        public ActionResult Index(String Polling)
        {
           
            Constituency constituency = db.Constituencies.Where(o => o.ConstituencyName.Equals(Polling)).FirstOrDefault();
            ViewBag.Constituency = constituency.ConstituencyName;
            var vm = PollingTable(Polling);
            return View(vm);
        }

        public List<CollationModelView> PollingTable(String ConstID)
        {
           
                
                List<CollationModelView> voteCollationModelView = new List<CollationModelView>();
                List<CollationEntry> lstCol = new List<CollationEntry>();
                foreach (PollingStation cons in db.PollingStations.Where(c=>c.ElectoralArea.Constituency1.ConstituencyName.Equals(ConstID)).ToList())
                {
                    lstCol = db.CollationEntries.Where(c => c.PollingStationID==cons.ID).ToList();
                    if (lstCol.Count() > 0)
                    {
                        CollationModelView vot = new CollationModelView();
                        foreach (CollationEntry col in lstCol)
                        {

                            switch (col.PartyID)
                            {
                                case 1:
                                    vot.NDC = col.NumberOfVote;
                                    break;
                                case 2:
                                    vot.NPP = col.NumberOfVote;
                                    break;
                                case 3:
                                    vot.CPP = col.NumberOfVote;
                                    break;
                                case 4:
                                    vot.PPP = col.NumberOfVote;
                                    break;
                                case 5:
                                    vot.OTHERS = col.NumberOfVote;
                                    break;
                                case 6:
                                    vot.REJECTED = col.NumberOfVote;
                                    break;

                            }
                            vot.ConstituencyID = cons.PollingStationName;

                        }
                        vot.TOTAL = vot.NDC + vot.NPP + vot.OTHERS + vot.PPP + vot.REJECTED + vot.CPP;
                        voteCollationModelView.Add(vot);
                    }
                }

                return voteCollationModelView;
        }
        private List<spVotePartyRegionTransPie_Result> Getnumbers(int PollingID)
        {
            List<spVotePartyRegionTransPie_Result> sp = new List<spVotePartyRegionTransPie_Result>();
           List<CollationEntry> lstCol = db.CollationEntries.Where(c => c.PollingStationID == PollingID).ToList();
           var TotalVotes= lstCol.Sum(c=>c.NumberOfVote);
            foreach (var col in lstCol)
           {
               sp.Add(new spVotePartyRegionTransPie_Result
               {
                   Abrev = GetParty(col.PartyID),
                   Total = col.NumberOfVote * TotalVotes / 100,
                   RegionName = "a",
                   PartyName = col.PartyID.ToString()
               });
           }
            return sp;

        }
        private string GetParty(int partyId)
        {
            return db.Parties.Find(partyId).Abrev;
        }
          
        public Highcharts TransDaily(String Polling)
        {
            SMSServersEntities tp = new SMSServersEntities();
            List<Series> sr = new List<Series>();
            PollingStation pollingstation = db.PollingStations.Where(c=> c.PollingStationName.Equals(Polling)).FirstOrDefault();

            foreach (var sp in Getnumbers(pollingstation.ID))
            {
                sr.Add(new Series { Name = sp.Abrev, Data = new Data(new object[] { Math.Round(sp.Total, 2) }) });
            }

            Highcharts chart1 = new Highcharts("chart1")
               .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
               .SetTitle(new Title { Text = "Total votes per party" })
               .SetSubtitle(new Subtitle { Text = String.Format("Summary for: {0}", pollingstation.PollingStationName) })
               .SetXAxis(new XAxis { Categories = new[] { pollingstation.PollingStationName } })
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

        public Highcharts PieData(int Polling)
        {
            List<ZoneData> data = new List<ZoneData>();
            SMSServersEntities tp = new SMSServersEntities();
            string tDate = DateTime.Today.ToString("MM/dd/yyyy");

            var tran = Getnumbers(Polling);

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