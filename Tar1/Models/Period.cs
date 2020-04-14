using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tar1.Models.DAL;

namespace Tar1.Models
{
    public class Period
    {
        DateTime startdate;
        DateTime enddate;
        int unitid;

        public DateTime Startdate { get => startdate; set => startdate = value; }
        public DateTime Enddate { get => enddate; set => enddate = value; }
        public int Unitid { get => unitid; set => unitid = value; }

        public Period() { }

        public Period(DateTime start, DateTime end, int id)
        {
            Startdate = start;
            Enddate = end;
            Unitid = id;

        }

        public void InsertPeriod()
        {
            DBservices dbs = new DBservices();
            dbs.InsertPeriod(this);

        }



    }
}