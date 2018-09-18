using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string sqlConnectString = "Data Source=.;Integrated security=SSPI;Initial Catalog=Test;";

            string sqlSelect = "SELECT * FROM Table_1; SELECT * FROM Table_2";
            SqlDataAdapter da = new SqlDataAdapter(sqlSelect, sqlConnectString);
            DataSet ds = new DataSet();
            da.Fill(ds);

            using (SqlConnection connection = new SqlConnection(sqlConnectString))
            {
                SqlCommand command = new SqlCommand(sqlSelect, connection);
                connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                int rsNumber = 0;
                var table1 = new List<Table1>();
                var table2 = new List<Table2>();
                do
                {
                    ++rsNumber;
                    while (dr.Read())
                    {
                        switch (rsNumber)
                        {
                            case 1:
                                GenerateTable1(table1, dr);
                                break;
                            case 2:
                                GenerateTable2(table2, dr);
                                break;
                        }
                    }
                } while (dr.NextResult());
            }
        }
        public static void GenerateTable1(List<Table1> t1, SqlDataReader dr)
        {
            t1.Add(new Table1()
            {
                Id = (int)dr["Id"],
                Name = (string)dr["Name"]
            });
        }
        public static void GenerateTable2(List<Table2> t2, SqlDataReader dr)
        {
            t2.Add(new Table2()
            {
                Id = (int)dr["Id"],
                Text = (string)dr["Text"]
            });
        }
    }

    class Table1
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    class Table2
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
