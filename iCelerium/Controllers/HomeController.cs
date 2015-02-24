using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using iCelerium.Models;
using iCelerium.Models.BodyClasses;

namespace iCelerium.Controllers
{
    [Authorize(Roles = "User,Admin,Super Admin")]
    [Audit]
    public class HomeController : Controller
    {
        public ActionResult calendar()
        {
            return this.View();
        }

        public ActionResult Index()
        {
            var vm = new DashboardViewModel
            {
                Chart1 = this.TransBasicCol(),
                Chart2 = this.PieData(),
                Chart3 = this.tableRegion()
            };
            ViewBag.SumCons = vm.Chart3.Sum(c => c.NoConstituency);
            ViewBag.SumNDC = vm.Chart3.Sum(c => c.NDC);
            ViewBag.SumNPP = vm.Chart3.Sum(c => c.NPP);
            ViewBag.SumCPP = vm.Chart3.Sum(c => c.CPP);
            ViewBag.SumPPP = vm.Chart3.Sum(c => c.PPP);
            ViewBag.SumOTHERS = vm.Chart3.Sum(c => c.OTHERS);
            ViewBag.REJECTED = vm.Chart3.Sum(c => c.REJECTED);
            ViewBag.SumTOTAL = vm.Chart3.Sum(c => c.TOTAL);
      
            return this.View(vm);
        }

        public ActionResult Reports(string reportModel)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "------------", Value = "" });
            items.Add(new SelectListItem { Text = "Transactions", Value = "1" });
            this.ViewBag.reportModel = items;
            return this.View();
        }

        public ActionResult Contact()
        {
            this.ViewBag.Message = "Your contact page.";

            return this.View();
        }

        public Highcharts TransBasicCol()
        {
            SMSServersEntities tp = new SMSServersEntities();
            List<Series> sr = new List<Series>();
            foreach (var sp in tp.spVotePartyTransPie().ToList())
            {
                sr.Add(new Series { Name = sp.Abrev, Data = new Data(new object[] { Math.Round(sp.Total, 2) }) });
            }

            Highcharts chart1 = new Highcharts("chart1")
               .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
               .SetTitle(new Title { Text = "Total votes per party" })
               .SetSubtitle(new Subtitle { Text = "Summary: National" })
               .SetXAxis(new XAxis { Categories = new[] { "National" } })
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
        public Highcharts TransDailyBasicBar()
        {
            SMSServersEntities db = new SMSServersEntities();
            List<string> lstRegion = new List<string>();
            lstRegion = db.spVoteAvailableRegion().ToList();
            List<Series> lstSeries = new List<Series>();

            foreach (var party in db.Parties.ToList())
            {
                List<int> lstNumbers = new List<int>();

                foreach (var reg in lstRegion)
                {

                    foreach (spVotePerRegion_Result vot in db.spVotePerRegion().OrderBy(c=>c.PARTY).Where(c => c.RegionName.Equals(reg) && c.PARTY.Equals((party.Abrev.Trim()))))
                    {
                        lstNumbers.Add(vot.TOTAL);
                    }

                }
                object[] ltl = new object[lstNumbers.Count()];
                for (int i = 0; i < lstNumbers.Count(); i++)
                {
                    ltl[i] = lstNumbers.ToArray()[i];
                }
                Series series = new Series { Name = party.Abrev.Trim(), Data = new Data(ltl) };
               lstSeries.Add(series);
            }

           
            
            
           
           
            
            

            Highcharts chart = new Highcharts("chart1")
                 .InitChart(new Chart { DefaultSeriesType = ChartTypes.Bar })
                 .SetTitle(new Title { Text = "Historic Votes Collation by Region" })
                 .SetSubtitle(new Subtitle { Text = "Available regions only" })
                 .SetXAxis(new XAxis
                 {
                     Categories = lstRegion.ToArray(),
                     Title = new XAxisTitle { Text = string.Empty }
                 })
                 .SetYAxis(new YAxis
                 {
                     Min = 0,
                     Title = new YAxisTitle
                     {
                         Text = "Votes (Thousands)",
                         Align = AxisTitleAligns.High
                     }
                 })
                 .SetTooltip(new Tooltip { Formatter = "function() { return ''+ this.series.name +': '+ this.y ; }" })
                 .SetPlotOptions(new PlotOptions
                 {
                     Bar = new PlotOptionsBar
                     {
                         DataLabels = new PlotOptionsBarDataLabels { Enabled = true }
                     }
                 })
                 .SetLegend(new Legend
                 {
                     Layout = Layouts.Vertical,
                     Align = HorizontalAligns.Right,
                     VerticalAlign = VerticalAligns.Top,
                     X = -100,
                     Y = 100,
                     Floating = true,
                     BorderWidth = 1,
                     BackgroundColor = new BackColorOrGradient(ColorTranslator.FromHtml("#FFFFFF")),
                     Shadow = true
                 })
                 .SetCredits(new Credits { Enabled = false })
                 .SetSeries(lstSeries.ToArray());

            return chart;
           
            return chart;
        }

        public Highcharts PieData()
        {
            List<ZoneData> data = new List<ZoneData>();
            SMSServersEntities tp = new SMSServersEntities();
            string tDate = DateTime.Today.ToString("MM/dd/yyyy");

            var tran = tp.spVotePartyTransPie().OrderBy(c=>c.Abrev);

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
                                                      .SetTitle(new Title { Text = String.Format("Shares of votes as at {0}",DateTime.Now) })
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

        public List<VotesPerRegionModelView> tableRegion()
        {
            SMSServersEntities db = new SMSServersEntities();
            List<VotesPerRegionModelView> voteCollationModelView = new List<VotesPerRegionModelView>();


            List<spVotePerRegion_Result> lstCol = new List<spVotePerRegion_Result>();
            foreach (iCelerium.Models.Region Reg in db.Regions.ToList())
            {
                lstCol = db.spVotePerRegion().Where(c => c.RegionName.Equals(Reg.RegionName)).ToList();
                if (lstCol.Count() > 0)
                {
                    VotesPerRegionModelView vot = new VotesPerRegionModelView();
                    foreach (spVotePerRegion_Result col in lstCol)
                    {

                        switch (col.PARTY.Trim())
                        {
                            case "NDC":
                                vot.NDC += col.TOTAL;
                                break;
                            case "NPP":
                                vot.NPP += col.TOTAL;
                                break;
                            case "CPP":
                                vot.CPP += col.TOTAL;
                                break;
                            case "PPP":
                                vot.PPP += col.TOTAL;
                                break;
                            case "OTH":
                                vot.OTHERS += col.TOTAL;
                                break;
                            case "IRB":
                                vot.REJECTED += col.TOTAL;
                                break;

                        }
                         vot.Region = col.RegionName;

                         vot.NoConstituency = db.spNumberberOfConstituency().Where(c => c.RegionName.Equals(col.RegionName)).ToList().GroupBy(o=>o.ConstituencyName).Select(group=>group.First()).Count();
                    }
                  
                    vot.TOTAL = vot.NDC + vot.NPP + vot.OTHERS + vot.PPP + vot.REJECTED + vot.CPP;
                    voteCollationModelView.Add(vot);
                }
            }



          return  voteCollationModelView.OrderByDescending(c=>c.TOTAL).ToList();
        }

        public ActionResult PieChart()
        {
            Highcharts chart = this.PieData();

            return this.View(chart);
        }

        public ActionResult TransChart()
        {
            Highcharts chart = this.TransBasicCol();

            return this.View(chart);
        }
    }
}