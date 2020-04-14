using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tar1.Models;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;

namespace Tar1.Models.DAL
{
    public class DBservices
    {
        public SqlConnection connect(String conString)
        {

            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }
        private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 20;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

            return cmd;
        }
        public List<ApartmentType> GetApartmentType()
        {

            List<ApartmentType> A = new List<ApartmentType>();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM ApartmentType_2020";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    ApartmentType ApartmentType = new ApartmentType();
                    ApartmentType.Apartmenttype = (string)dr["ApartmentType"];
                    ApartmentType.Id = Convert.ToInt32(dr["ApartmentTypeId"]);
                    A.Add(ApartmentType);
                }

                return A;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }
        public void PostUnit(OrganizeUnit unit)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            String cStr = BuildInsertCommand(unit);
            cmd = CreateCommand(cStr, con);
            try
            {
                int numEffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            if (unit.Unittype == "דירה")
            {
                GetApartmentId(unit);
            }
        }
        private String BuildInsertCommand(OrganizeUnit unit)
        {
            String command;
            StringBuilder sb = new StringBuilder();

            string G = unit.Numofguides.ToString();
            string R = unit.Numofresidents.ToString();
            sb.AppendFormat("Values('{0}', '{1}','{2}', '{3}','{4}','{5}')", unit.Unitname, unit.Numofresidents, unit.Numofguides, unit.City, unit.Street_hnumber, unit.Unittype);
            String prefix = "INSERT INTO OrganizeUnit_2020" + "(UnitName,NumOfResidents,NumOfGuides,City,Street_HNumber,UnitType)";
            command = prefix + sb.ToString();
            return command;
        }
        private void GetApartmentId(OrganizeUnit unit)
        {
            int id = 0;
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "select * from Apartment_2020 where ApartmentTypeId is null";
                SqlCommand cmd1 = new SqlCommand(selectSTR, con);
                SqlDataReader dr = cmd1.ExecuteReader(CommandBehavior.CloseConnection);
                dr.Read();
                id = Convert.ToInt32(dr["UnitId"]);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            UpdateApartmentType(unit.ApartmentType1, id);
        }
        private void UpdateApartmentType(int ApartmentType, int id)
        {
            SqlConnection con;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            String cStr = "UPDATE Apartment_2020 SET ApartmentTypeId = " + ApartmentType.ToString() + " WHERE UnitId = " + id.ToString();
            SqlCommand cmd = CreateCommand(cStr, con);
            try
            {
                int numEffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }
        public void InsertTrainingLevel(TrainingLevel tl)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            String cStr = BuildInsertCommand(tl);
            cmd = CreateCommand(cStr, con);
            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {

                if (con != null)
                {
                    con.Close();
                }
            }
        }
        private String BuildInsertCommand(TrainingLevel tl)
        {
            String command;

            String prefix = "INSERT INTO TrainingLevel_2020 (TrainingLevel)";
            String str = "Values('" + tl.Traininglevel + "')";
            command = prefix + str;
            return command;
        }
        public List<TrainingLevel> GetTrainingLevel()
        {

            List<TrainingLevel> TL = new List<TrainingLevel>();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM TrainingLevel_2020";
                SqlCommand cmd = new SqlCommand(selectSTR, con);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    TrainingLevel trininglevel = new TrainingLevel();
                    trininglevel.Traininglevel = (string)dr["TrainingLevel"];
                    trininglevel.Id = Convert.ToInt32(dr["TrainingLevelId"]);
                    TL.Add(trininglevel);
                }
                return TL;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }





        }
        public List<OrganizeUnit> GetUnit()
        {

            List<OrganizeUnit> OU = new List<OrganizeUnit>();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM OrganizeUnit_2020";
                SqlCommand cmd = new SqlCommand(selectSTR, con);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    OrganizeUnit Unit = new OrganizeUnit();
                    Unit.Id = Convert.ToInt32(dr["UnitId"]);
                    Unit.Unitname = (string)dr["UnitName"];
                    OU.Add(Unit);
                }
                return OU;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }


        }
        public void InsertUser(User user)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            String cStr = BuildInsertCommand(user);
            cmd = CreateCommand(cStr, con);
            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {

                if (con != null)
                {
                    con.Close();
                }
            }

        }
        private String BuildInsertCommand(User user)
        {
            String command;
            string Uid;
            StringBuilder sb = new StringBuilder();
            int day = user.Birthdate.Day;
            int month = user.Birthdate.Month;
            int year = user.Birthdate.Year;
            //DateTime d = new DateTime(year,month,day);
            
            string bdate = day.ToString()+"/"+month.ToString()+"/"+year.ToString();
            //Insert big manager
            if (user.Unitid == 0)
            {
                string um = user.Unitmanager.ToString();
                string bm = user.Bigmanager.ToString();
                String prefix = "INSERT INTO User_2020 (UserId,UPassword,FirstName,LastName,Birthdate,Telephone,UserRole,IsUnitManager,BigManager)";
                sb.AppendFormat("Values('{0}', '{1}','{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')", user.Userid, user.Password, user.Firstname, user.Lastname, bdate, user.Telephone, user.Role, um, bm);
                command = prefix + sb;
                return command;
            }
            //Insert Guide
            else if (user.Role == "מדריך")
            {
                Uid = user.Unitid.ToString();
                String prefix = "INSERT INTO User_2020 (UserId,UPassword,FirstName,LastName,Birthdate,Telephone,UserRole,UnitId)";
                sb.AppendFormat("Values('{0}', '{1}','{2}', '{3}', '{4}', '{5}', '{6}', '{7}')", user.Userid, user.Password, user.Firstname, user.Lastname, bdate, user.Telephone, user.Role, Uid);
                command = prefix + sb;
                return command;

            }
            //Insert unit manager
            else
            {
                string um = user.Unitmanager.ToString();
                string bm = user.Bigmanager.ToString();
                Uid = user.Unitid.ToString();
                String prefix = "INSERT INTO User_2020 (UserId,UPassword,FirstName,LastName,Birthdate,Telephone,UserRole,IsUnitManager,BigManager,UnitId)";
                sb.AppendFormat("Values('{0}', '{1}','{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')", user.Userid, user.Password, user.Firstname, user.Lastname, bdate, user.Telephone, user.Role, um, bm, Uid);
                command = prefix + sb;
                return command;
            }
        }
        public void UpdateGuide(TrainingLevel tl, int id)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                throw (ex);

            }
            String cStr = BuildInsertCommand(tl, id);
            cmd = CreateCommand(cStr, con);
            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        private String BuildInsertCommand(TrainingLevel tl, int id)
        {
            String command;

            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();
            string p = id.ToString();
            string trl = tl.Id.ToString();


            command = "UPDATE Guide_2020 SET TrainingLevelId =" + trl + " WHERE UserId =" + p;


            return command;
        }
        public List<User> GetUser()
        {
            List<User> U = new List<User>();
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT * FROM User_2020";
                SqlCommand cmd = new SqlCommand(selectSTR, con);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    User us = new User();
                    us.Firstname = (string)dr["FirstName"];
                    us.Lastname = (string)dr["LastName"];
                    us.Password = (string)dr["UPassword"];
                    us.Userid = (string)dr["UserId"];
                    us.Role = (string)dr["UserRole"];
                    us.Unitid = Convert.ToInt32(dr["UnitId"]);
                    U.Add(us);
                }
                return U;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public void InsertPeriod(Period period)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            String cStr = BuildInsertCommand(period);
            cmd = CreateCommand(cStr, con);
            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {

                if (con != null)
                {
                    con.Close();
                }
            }




        }
        private String BuildInsertCommand(Period period)
        {
            String command;
            string Uid;
            StringBuilder sb = new StringBuilder();
            int day = period.Startdate.Day;
            int month = period.Startdate.Month;
            int year = period.Startdate.Year;
            DateTime sd = new DateTime(year, day, month);
            int Eday = period.Enddate.Day;
            int Emonth = period.Enddate.Month;
            int Eyear = period.Enddate.Year;
            DateTime ed = new DateTime(Eyear, Eday, Emonth);
            Uid = period.Unitid.ToString();
            string s = sd.ToString();
            string e = ed.ToString();
            String prefix = "INSERT INTO SchedulingPeriod_2020 (StartPeriod,EndPeriod,UnitId)";
            sb.AppendFormat("Values('{0}', '{1}','{2}')", s, e, Uid);
            command = prefix + sb;
            return command;

        }

        //        public int insert(Airport[] airports)
        //        {
        //            SqlConnection con = null;
        //            SqlCommand cmd;
        //            int numEffected = 0;
        //            try
        //            {
        //                con = connect("DBConnectionString");
        //                foreach (var item in airports)
        //                {
        //                    String cStr = BuildInsertCommand(item);
        //                    cmd = CreateCommand(cStr, con);
        //                    try
        //                    {
        //                        numEffected = cmd.ExecuteNonQuery(); // execute the command
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        throw (ex);
        //                    }

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw (ex);
        //            }
        //            finally
        //            {
        //                if (con != null)
        //                {
        //                    con.Close();
        //                }
        //            }

        //            return numEffected;
        //        }



        //        private String BuildInsertCommand(Airport airport)
        //        {
        //            String command;

        //            StringBuilder sb = new StringBuilder();
        //            // use a string builder to create the dynamic string
        //            sb.AppendFormat("Values('{0}', '{1}','{2}', '{3}')", airport.Code, airport.Lat, airport.Lon, airport.Name);
        //            String prefix = "INSERT INTO Airport " + "(APcode,Lat,Lon,Name) ";
        //            command = prefix + sb.ToString();

        //            return command;
        //        }
        //        private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
        //        {

        //            SqlCommand cmd = new SqlCommand(); // create the command object

        //            cmd.Connection = con;              // assign the connection to the command object

        //            cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

        //            cmd.CommandTimeout = 20;           // Time to wait for the execution' The default is 30 seconds

        //            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

        //            return cmd;
        //        }

        //        public void insert(Flight flight)
        //        {
        //            SqlConnection con = null;
        //            int counter = 0;
        //            try
        //            {
        //                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

        //                String selectSTR = "select * from Favorite_2020";
        //                SqlCommand cmd1 = new SqlCommand(selectSTR, con);

        //                // get a reader
        //                SqlDataReader dr = cmd1.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

        //                while (dr.Read())
        //                {
        //                    if (dr["FlightId"].ToString() == flight.Idflight)
        //                    {
        //                        counter = Convert.ToInt32(dr["Counterofave"]) + 1;

        //                        insertcounter(counter,flight.Idflight);                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                // write to log
        //                throw (ex);
        //            }
        //            finally
        //            {
        //                if (con != null)
        //                {
        //                    con.Close();
        //                }

        //            }
        //            if(counter==0)
        //                insertflight(flight);

        //}

        //         private int insertflight(Flight flight)
        //        {

        //            SqlConnection con;
        //            SqlCommand cmd;

        //            try
        //            {

        //                con = connect("DBConnectionString"); // create the connection

        //            }
        //            catch (Exception ex)
        //            {
        //                throw (ex);

        //            }

        //            String cStr = BuildInsertCommand(flight);

        //            cmd = CreateCommand(cStr, con);
        //            try
        //            {
        //                int numEffected = cmd.ExecuteNonQuery(); // execute the command
        //                return numEffected;
        //            }
        //            catch (Exception ex)
        //            {
        //                return 0;
        //                // write to log
        //                throw (ex);
        //            }
        //            finally
        //            {

        //                if (con != null)
        //                {
        //                    // close the db connection
        //                    con.Close();

        //                }


        //            }

        //        }
        //        private String BuildInsertCommand(Flight flight)
        //        {
        //            String command;

        //            StringBuilder sb = new StringBuilder();

        //            string p = flight.Price.ToString();
        //            sb.AppendFormat("Values('{0}', '{1}','{2}', '{3}','{4}','{5}','{6}','{7}','{8}')", flight.Idflight, flight.Origin, flight.Destination, flight.StartDate, flight.EndDate, p, flight.Route, flight.Airlinecompany, "1");
        //            String prefix = "INSERT INTO Favorite_2020" + "(FlightId,Origin,Destination,Startdate,Endate,Price,Connections,Airlinecompany,Counterofave)";
        //            command = prefix + sb.ToString();

        //            return command;
        //        }

        //        private void insertcounter(int counter, string id)
        //        {

        //            SqlConnection con;
        //            SqlCommand cmd;

        //            try
        //            {

        //                con = connect("DBConnectionString"); // create the connection

        //            }
        //            catch (Exception ex)
        //            {
        //                throw (ex);

        //            }

        //            String cStr = BuildInsertCommand(counter,id);

        //            cmd = CreateCommand(cStr, con);
        //            try
        //            {
        //                int numEffected = cmd.ExecuteNonQuery(); // execute the command

        //            }
        //            catch (Exception ex)
        //            {

        //                // write to log
        //                throw (ex);
        //            }
        //            finally
        //            {

        //                if (con != null)
        //                {
        //                    // close the db connection
        //                    con.Close();

        //                }


        //            }

        //        }
        //        private String BuildInsertCommand(int counter, string id)
        //        {
        //            String command;

        //            StringBuilder sb = new StringBuilder();
        //            StringBuilder sb1 = new StringBuilder();
        //            string p = counter.ToString();


        //            command = "UPDATE Favorite_2020 SET Counterofave =" + p + "WHERE FlightId ='" + id + "'";


        //            return command;
        //        }



        //public List<Airport> getairports()
        //{
        //    List<Airport> airportsList = new List<Airport>();
        //SqlConnection con = null;

        //    try
        //    {
        //        con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

        //String selectSTR = "SELECT * FROM Airport";
        //SqlCommand cmd = new SqlCommand(selectSTR, con);

        //// get a reader
        //SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

        //        while (dr.Read())
        //        {   // Read till the end of the data into a row
        //            Airport A = new Airport();

        //A.Code = (string) dr["APcode"];

        //airportsList.Add(A);
        //        }

        //        return airportsList;
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        if (con != null)
        //        {
        //            con.Close();
        //        }

        //    }


        //}
        //public List<Flight> getflights()
        //{
        //    List<Flight> flightsList = new List<Flight>();
        //    SqlConnection con = null;

        //    try
        //    {
        //        con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

        //        String selectSTR = "SELECT * FROM MyFlight_2020";
        //        SqlCommand cmd = new SqlCommand(selectSTR, con);

        //        // get a reader
        //        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

        //        while (dr.Read())
        //        {   // Read till the end of the data into a row
        //            Flight F = new Flight();

        //            F.Origin = (string)dr["Origin"];
        //            F.Destination = (string)dr["Destination"];
        //            F.StartDate = (string)dr["StartDate"];
        //            F.EndDate = (string)dr["EndDate"];
        //            F.Price = Convert.ToDouble(dr["Price"]);
        //            if (dr["Route"] != null)
        //            {
        //                F.Route = (string)dr["Route"];
        //            }

        //            flightsList.Add(F);
        //        }

        //        return flightsList;
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        if (con != null)
        //        {
        //            con.Close();
        //        }

        //    }


        //}
        //public bool check(UserManager userman)
        //{
        //    SqlConnection con = null;

        //    try
        //    {
        //        con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

        //        String selectSTR = "SELECT * FROM managersusers";
        //        SqlCommand cmd = new SqlCommand(selectSTR, con);


        //        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

        //        while (dr.Read())
        //        {   // Read till the end of the data into a row

        //            if (userman.Username == dr["Username"].ToString() && userman.Password == dr["passwords"].ToString())
        //                return true;
        //        }
        //        return false;


        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        if (con != null)
        //        {
        //            con.Close();
        //        }

        //    }


        //}

        //public int insertcustomer(Customer customer)
        //{
        //    SqlConnection con;
        //    SqlCommand cmd;
        //    try
        //    {
        //        con = connect("DBConnectionString"); // create the connection
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }

        //    String cStr = BuildInsertCommand(customer);

        //    cmd = CreateCommand(cStr, con);
        //    try
        //    {
        //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
        //        return numEffected;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //        // write to log
        //        throw (ex);
        //    }
        //    finally
        //    {

        //        if (con != null)
        //        {
        //            con.Close();
        //        }


        //    }


        //}

        //private String BuildInsertCommand(Customer customer)
        //{
        //    String command;

        //    StringBuilder sb = new StringBuilder();
        //    string p = customer.Agree.ToString();
        //    sb.AppendFormat("Values('{0}', '{1}','{2}')", customer.Email, customer.Fullnames, p);
        //    String prefix = "INSERT INTO Customer_2020" + "(Email,FullNames,Agree)";
        //    command = prefix + sb.ToString();

        //    return command;
        //}

    }
}