using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using SomerenModel;

namespace SomerenDAL
{
    public class DrinkDao : BaseDao
    {
        //private SqlConnection dbConnection;
        

        public List<Drink> GetAllDrinks()
        {
            string query = "SELECT Drink.drinkID, Drink.name, Drink.price, Drink.stock, Drink.type FROM Drink ORDER BY name";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Drink> ReadTables(DataTable dataTable)
        {
            List<Drink> drinks = new List<Drink>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Drink drink = new Drink()
                {
                    ID = (int)dr["drinkID"],
                    Name = (string)dr["name"],
                    Price = (decimal)dr["price"],
                    Stock = (int)dr["stock"],
                    Type = (bool)dr["type"],
                };
                drinks.Add(drink);
            }
            return drinks;
        }

        public void Add(Drink drink)
        {
            //READ: Drink ID should be set to "next one" in SQL software - I hope you know how to do it, Gnas.

            conn.Open();
            SqlCommand command = new SqlCommand(
            "INSERT INTO Drink (name, price, stock, type) " +
            "VALUES (@name, @price, @stock, @type); " +
           "SELECT SCOPE_IDENTITY();",
            conn);
            command.Parameters.AddWithValue("@name", drink.Name);
            command.Parameters.AddWithValue("@price", drink.Price);
            command.Parameters.AddWithValue("@stock", drink.Stock);
            command.Parameters.AddWithValue("@type", drink.Type);

            SqlParameter[] sqlParameters = new SqlParameter[0];
            //needs sorting out
            //response: is this to check wether it has happened or not? we can just try/catch it outside
            int nrOfRowChanged = command.ExecuteNonQuery();
            if (nrOfRowChanged == 0)
            {
                throw new Exception("No record for drink was altered");
            }
        }

        public void Update(Drink drink)
        {
            conn.Open();
            SqlCommand command = new SqlCommand(
            "UPDATE Drink SET name = @name, price = @price, type = @type, stock = @stock " +
            "WHERE drinkId = @drinkId",
            conn);
            command.Parameters.AddWithValue("@drinkId", drink.ID);
            command.Parameters.AddWithValue("@name", drink.Name);
            command.Parameters.AddWithValue("@price", drink.Price);
            command.Parameters.AddWithValue("@type", drink.Type);

            //READ: is it here we update the stock that we calculate in the cash Register maybe?
            command.Parameters.AddWithValue("@stock", drink.Stock);

            SqlParameter[] sqlParameters = new SqlParameter[0];
            //again, we can try/catch it later
            int nrOfRowChanged = command.ExecuteNonQuery();
            if (nrOfRowChanged == 0)
            {
                throw new Exception("No record for drink was altered");
            }
            conn.Close();
        }

        public void Delete(Drink drink)
        {
            conn.Open();
            SqlCommand command = new SqlCommand(
            "DELETE FROM Drink WHERE drinkId = @drinkId",
            conn);
            command.Parameters.AddWithValue("@drinkId", drink.ID);
            int nrOfRowChanged = command.ExecuteNonQuery();

            conn.Close();
            if (nrOfRowChanged == 0)
            {
                throw new Exception("No record for drink was altered");
            }
        }
        
    }
}
