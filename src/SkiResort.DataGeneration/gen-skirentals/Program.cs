using PublicHoliday;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Ski.Rentals.Generator
{
    class Program
    {
        private const int RandomSeed = 12345;

        private static readonly USAPublicHoliday Holidays = new USAPublicHoliday();
        private static string ConnectionString;
        private static int[] SeasonYears = { 2013, 2014, 2015 };
        private static int[] SeasonMonths = { 12, 1, 2, 3, 4 };
        private static TimeSpan TimeOffset = TimeSpan.FromHours(-8); // PST

        static void Main(string[] args)
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            HolidaysToSql();
            WeatherToSql();
            RentalsToSql(GenerateRentals());
        }

        private static void ToFile(IEnumerable<RentalTransaction> rentals)
        {
            using (StreamWriter writer = new StreamWriter("rentals.txt", false, Encoding.UTF8))
            {
                writer.WriteLine("customerId,rentalDate");

                foreach (RentalTransaction rental in rentals)
                {
                    writer.WriteLine("{0},{1:yyyy/MM/dd HH:mm:ss}", rental.CustomerId, rental.Date);
                }
            }
        }

        private static void RentalsToSql(IEnumerable<RentalTransaction> rentals)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                if (TableRowCount(con, "Rentals") > 100)
                {
                    return;
                }

                DataTable table = new DataTable();
                table.Columns.AddRange(new [] {
                    new DataColumn("Activity", typeof(int)),
                    new DataColumn("Category", typeof(int)),
                    new DataColumn("StartDate", typeof(DateTimeOffset)),
                    new DataColumn("EndDate", typeof(DateTimeOffset)),
                    new DataColumn("Goal", typeof(int)),
                    new DataColumn("PickupHour", typeof(int)),
                    new DataColumn("PoleSize", typeof(int)),
                    new DataColumn("ShoeSize", typeof(int)),
                    new DataColumn("SkiSize", typeof(int)),
                    new DataColumn("TotalCost", typeof(int)),
                    new DataColumn("UserEmail", typeof(string))
                });

                SqlTransaction tx = con.BeginTransaction();
                int i = 0;

                foreach (RentalTransaction rental in rentals)
                {
                    i++;
                    if (i % 100000 == 0)
                    {
                        BulkCopyRentals(con, tx, table);
                        tx.Commit();
                        table.Rows.Clear();
                        Console.WriteLine("Committed - {0}", i);
                        tx = con.BeginTransaction();
                    }

                    DataRow row = table.NewRow();
                    row["Activity"] = 1;
                    row["Category"] = 3;
                    row["StartDate"] = rental.Date;
                    row["EndDate"] = rental.Date.AddHours(5);
                    row["Goal"] = 1;
                    row["PickupHour"] = 8;
                    row["PoleSize"] = 67;
                    row["ShoeSize"] = 9;
                    row["SkiSize"] = 150;
                    row["TotalCost"] = 67;
                    row["UserEmail"] = "bulkuser@awski.com";
                    table.Rows.Add(row);
                }

                BulkCopyRentals(con, tx, table);
                tx.Commit();
            }
        }

        private static void BulkCopyRentals(SqlConnection con, SqlTransaction tx, DataTable table)
        {
            SqlBulkCopy copy = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, tx);
            copy.DestinationTableName = "Rentals";
            copy.BulkCopyTimeout = 60 * 5;
            copy.ColumnMappings.Add("Activity", "Activity");
            copy.ColumnMappings.Add("Category", "Category");
            copy.ColumnMappings.Add("StartDate", "StartDate");
            copy.ColumnMappings.Add("EndDate", "EndDate");
            copy.ColumnMappings.Add("Goal", "Goal");
            copy.ColumnMappings.Add("PickupHour", "PickupHour");
            copy.ColumnMappings.Add("PoleSize", "PoleSize");
            copy.ColumnMappings.Add("ShoeSize", "ShoeSize");
            copy.ColumnMappings.Add("SkiSize", "SkiSize");
            copy.ColumnMappings.Add("TotalCost", "TotalCost");
            copy.ColumnMappings.Add("UserEmail", "UserEmail");
            copy.WriteToServer(table);
        }

        private static void HolidaysToSql()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                if (TableRowCount(con, "Holidays") > 0)
                {
                    return;
                }

                SqlTransaction tx = con.BeginTransaction();
                SqlCommand cmd = new SqlCommand("INSERT INTO Holidays (Date) VALUES (@date)", con, tx);
                SqlParameter date = cmd.Parameters.Add("@date", SqlDbType.DateTimeOffset);

                foreach (int year in SeasonYears)
                {
                    foreach (DateTime holiday in Holidays.PublicHolidays(year))
                    {
                        date.Value = new DateTimeOffset(holiday);
                        cmd.ExecuteNonQuery();
                    }
                }

                tx.Commit();
            }
        }

        private static void WeatherToSql()
        {
            Random weatherRandom = new Random(RandomSeed);

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                if (TableRowCount(con, "WeatherHistory") > 0)
                {
                    return;
                }

                SqlTransaction tx = con.BeginTransaction();
                SqlCommand cmd = new SqlCommand("INSERT INTO WeatherHistory (Date,Snow) VALUES (@date,@snow)", con, tx);
                SqlParameter date = cmd.Parameters.Add("@date", SqlDbType.DateTimeOffset);
                SqlParameter snow = cmd.Parameters.Add("@snow", SqlDbType.Bit);

                foreach (int year in SeasonYears)
                {
                    foreach (int month in SeasonMonths)
                    {
                        int monthDays = DateTime.DaysInMonth(year, month);
                        for (int day = 1; day <= monthDays; day++)
                        {
                            // 30% chance it snowed a bunch the day before
                            date.Value = new DateTimeOffset(year, month, day, 1, 0, 0, TimeOffset);
                            snow.Value = weatherRandom.Next(0, 10) < 3;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                tx.Commit();
            }
        }

        private static long TableRowCount(SqlConnection con, string table)
        {
            SqlCommand cmd = new SqlCommand("SELECT CONVERT(BIGINT, COUNT(*)) FROM " + table, con);
            return (long)cmd.ExecuteScalar();
        }

        private static IEnumerable<RentalTransaction> GenerateRentals()
        {
            int customerCount = 1000;
            Random random = new Random(RandomSeed);
            Random weatherRandom = new Random(RandomSeed);

            foreach (int year in SeasonYears)
            {
                foreach (int month in SeasonMonths)
                {
                    int monthDays = DateTime.DaysInMonth(year, month);
                    for (int day = 1; day <= monthDays; day++)
                    {
                        // 30% chance it snowed a bunch the day before
                        bool goodSnowYesterday = weatherRandom.Next(0, 10) < 3;
                        DateTimeOffset date = new DateTimeOffset(year, month, day, 1, 0, 0, TimeOffset);
                        int rentalCount = ComputeRentalCount(date.DateTime, goodSnowYesterday, random, weatherRandom);

                        for (int i = 0; i < rentalCount; i++)
                        {
                            yield return new RentalTransaction
                            {
                                CustomerId = random.Next(1, customerCount + 1),
                                Date = new DateTimeOffset(year, month, day, random.Next(8, 12), random.Next(0, 60), 0, TimeOffset)
                            };
                        }
                    }
                }
            }
        }

        private static int ComputeRentalCount(DateTime date, bool goodSnowYesterday, Random random, Random weatherRandom)
        {
            // generic weekday is baseline
            int rentalCount = random.Next(20, 40);

            // weekends are 10x the volume
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                rentalCount *= 10;
            }
            // holidays are 11x the volume, end-of-year break behaves like a holiday
            else if (Holidays.IsPublicHoliday(date))
            {
                rentalCount *= 11;
            }
            // end-of-year "work days" almost like a holiday
            else if (date.Month == 12 && date.Day > 25 && date.Day <= 31)
            {
                rentalCount *= 5;
            }

            // 20% more rental traffic when lessons are on
            if (date.Month >= 1 && date.Month <= 3)
            {
                rentalCount += (int)(rentalCount * 0.2);
            }

            // Jan/Feb 30% more popular
            if (date.Month == 1 || date.Month == 2)
            {
                rentalCount += (int)(rentalCount * 0.3);
            }
            
            if (goodSnowYesterday)
            {
                // Nice weather on non-working day, 30% boost
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || Holidays.IsPublicHoliday(date))
                {
                    rentalCount += (int)(rentalCount * 0.3);
                }
                // Nice weather on working day, 10% boost
                else
                {
                    rentalCount += (int)(rentalCount * 0.1);
                }
            }

            return rentalCount;
        }
    }
}
