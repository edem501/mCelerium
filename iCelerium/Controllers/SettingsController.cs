using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Web.Mvc;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Point = DotNet.Highcharts.Options.Point;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;
using iCelerium.Models;
using DotNet.Highcharts;

namespace iCelerium.Controllers
{
    [Authorize(Users = "Admin")]
    [Audit]
    public class SettingsController : Controller
    {
        private readonly SMSServersEntities adb = new SMSServersEntities();

        //
        // GET: /Settings/
        public ActionResult Index()
        {
            return this.View();
        }
        //
        // GET: /Settings/AuditTrail
        public async Task<ActionResult> AuditTrail(string startDate, string endDate, int? page)
        {
            DateTime date1, date2;

            this.ViewBag.startDate = startDate;
            this.ViewBag.endDate = endDate;

            if (String.IsNullOrEmpty(startDate) || String.IsNullOrEmpty(endDate))
            {
                date1 = DateTime.Today;
                date2 = DateTime.Today.AddDays(1);
            }
            else
            {
                date1 = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                date2 = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            IEnumerable<AuditRecord> querry = await this.adb.AuditRecords.Where(x => x.Timestamp >= date1 && x.Timestamp < date2).OrderBy(x => x.Timestamp).ToListAsync();
            List<AuditViewModel> Vm = new List<AuditViewModel>();
            if (querry != null && querry.Count() > 0)
            {
                foreach (AuditRecord ar in querry)
                {
                    Vm.Add(new AuditViewModel
                    {
                        AreaAccessed = ar.AreaAccessed,
                        AuditID = ar.AuditID,
                        IPAddress = ar.IPAddress,
                        Timestamp = ar.Timestamp,
                        UserName = ar.UserName
                    });
                }
            }

            return this.View(Vm.ToPagedList(page ?? 1, 15));
        }

        public ActionResult DonutChart()
        {
            string[] categories = { "MSIE", "Firefox", "Chrome", "Safari", "Opera" };
            Data data = new Data(new[]
            {
                new Point
                {
                    Y = 55.11,
                    Color = Color.FromName("colors[0]"),
                    Drilldown = new Drilldown
                    {
                        Name = "MSIE versions",
                        Categories = new[] { "MSIE 6.0", "MSIE 7.0", "MSIE 8.0", "MSIE 9.0" },
                        Data = new Data(new object[] { 10.85, 7.35, 33.06, 2.81 }),
                        Color = Color.FromName("colors[0]")
                    }
                },
                new Point
                {
                    Y = 21.63,
                    Color = Color.FromName("colors[1]"),
                    Drilldown = new Drilldown
                    {
                        Name = "Firefox versions",
                        Categories = new[] { "Firefox 2.0", "Firefox 3.0", "Firefox 3.5", "Firefox 3.6", "Firefox 4.0" },
                        Data = new Data(new object[] { 0.20, 0.83, 1.58, 13.12, 5.43 }),
                        Color = Color.FromName("colors[1]")
                    }
                },
                new Point
                {
                    Y = 11.94,
                    Color = Color.FromName("colors[2]"),
                    Drilldown = new Drilldown
                    {
                        Name = "Chrome versions",
                        Categories = new[] { "Chrome 5.0", "Chrome 6.0", "Chrome 7.0", "Chrome 8.0", "Chrome 9.0", "Chrome 10.0", "Chrome 11.0", "Chrome 12.0" },
                        Data = new Data(new object[] { 0.12, 0.19, 0.12, 0.36, 0.32, 9.91, 0.50, 0.22 }),
                        Color = Color.FromName("colors[2]")
                    }
                },
                new Point
                {
                    Y = 7.15,
                    Color = Color.FromName("colors[3]"),
                    Drilldown = new Drilldown
                    {
                        Name = "Safari versions",
                        Categories = new[] { "Safari 5.0", "Safari 4.0", "Safari Win 5.0", "Safari 4.1", "Safari/Maxthon", "Safari 3.1", "Safari 4.1" },
                        Data = new Data(new object[] { 4.55, 1.42, 0.23, 0.21, 0.20, 0.19, 0.14 }),
                        Color = Color.FromName("colors[3]")
                    }
                },
                new Point
                {
                    Y = 2.14,
                    Color = Color.FromName("colors[4]"),
                    Drilldown = new Drilldown
                    {
                        Name = "Opera versions",
                        Categories = new[] { "Opera 9.x", "Opera 10.x", "Opera 11.x" },
                        Data = new Data(new object[] { 0.12, 0.37, 1.65 }),
                        Color = Color.FromName("colors[4]")
                    }
                }
            });

            List<Point> browserData = new List<Point>(categories.Length);
            List<Point> versionsData = new List<Point>();
            for (int i = 0; i < categories.Length; i++)
            {
                browserData.Add(new Point { Name = categories[i], Y = data.SeriesData[i].Y, Color = data.SeriesData[i].Color });
                for (int j = 0; j < data.SeriesData[i].Drilldown.Categories.Length; j++)
                {
                    Drilldown drilldown = data.SeriesData[i].Drilldown;
                    versionsData.Add(new Point { Name = drilldown.Categories[j], Y = Number.GetNumber(drilldown.Data.ArrayData[j]), Color = drilldown.Color });
                }
            }

            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Pie })
                .SetTitle(new Title { Text = "Browser market share, April, 2011" })
                .SetSubtitle(new Subtitle { Text = "Total percent market share" })
                .SetPlotOptions(new PlotOptions { Pie = new PlotOptionsPie { Shadow = false } })
                .SetTooltip(new Tooltip { Formatter = @"function() { return '<b>'+ this.point.name +'</b>: '+ this.y +' %';}" })
                .AddJavascripVariable("colors", "Highcharts.getOptions().colors")
                .SetSeries(new[]
                {
                    new Series
                    {
                        Name = "Browsers",
                        Data = new Data(browserData.ToArray()),
                        PlotOptionsPie = new PlotOptionsPie
                        {
                            Size = new PercentageOrPixel(60, true),
                            DataLabels = new PlotOptionsPieDataLabels
                            {
                                Formatter = "function() { return this.y > 5 ? this.point.name : null; }",
                                Color = Color.White,
                                Distance = -30
                            }
                        }
                    },
                    new Series
                    {
                        Name = "Versions",
                        Data = new Data(versionsData.ToArray()),
                        PlotOptionsPie = new PlotOptionsPie
                        {
                            InnerSize = new PercentageOrPixel(60, true),
                            DataLabels = new PlotOptionsPieDataLabels
                            {
                                Formatter = "function() { return this.y > 1 ? '<b>'+ this.point.name +':</b> '+ this.y +'%'  : null; }"
                            }
                        }
                    }
                });

            return View(chart);
        }
    }
}