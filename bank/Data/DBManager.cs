using bank.Models;  
using System.Data.SQLite;
using System;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.VisualBasic;
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Principal;

namespace bank.Data
{
    public class DBManager
    {
        private static string connectionString = "Data Source=mydatabase.db;Version=3";

        public static bool CreateTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand pragmaCmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
                    {
                        pragmaCmd.ExecuteNonQuery();
                    }



                    using (SQLiteTransaction transaction = connection.BeginTransaction()) // Start transaction
                    {                       
                        using (SQLiteCommand command = connection.CreateCommand())
                        {

                            // SQL command to create the second table "Profile"
                            command.CommandText = @"
                           CREATE TABLE UserProfile (
    userId INTEGER PRIMARY KEY AUTOINCREMENT,  -- Unique ID for each user
    name TEXT NOT NULL,
    email TEXT  NOT NULL,
    phone TEXT NOT NULL,
    password TEXT NOT NULL
)";
                            command.ExecuteNonQuery(); // Execute the command to create "Profile" table
                            Console.WriteLine("Table 'Profile' created.");

                            // SQL command to create the first table "Account"
                            command.CommandText = @"
                            CREATE TABLE Account (
accNumber INTEGER PRIMARY KEY , --Account number
    userId INTEGER, --Foreign key from UserProfile
    accType TEXT NOT NULL, --Account type(e.g., Savings, Checking)
    balance REAL , --Account balance
    accStatus TEXT,
    CONSTRAINT fk_user FOREIGN KEY(userId) REFERENCES UserProfile(userId) ON DELETE CASCADE
                               )" ;
   
                                                        command.ExecuteNonQuery(); // Execute the command to create "Account" table
                            Console.WriteLine("Table 'Account' created.");

                    
                            // SQL command to create the third table "Transaction"
                            command.CommandText = @"
                            CREATE TABLE Trans(
transId INTEGER PRIMARY KEY AUTOINCREMENT,  -- Unique transaction ID
    accNumber INTEGER,                          -- Foreign key from Accounts
    type TEXT NOT NULL,                         -- Transaction type (e.g., 'deposit', 'withdrawal')
    amount REAL NOT NULL,                       -- Transaction amount
    transDate TEXT DEFAULT CURRENT_TIMESTAMP,   -- Transaction date (default is current timestamp)
    CONSTRAINT fk_account FOREIGN KEY (accNumber) REFERENCES Account(accNumber) ON DELETE CASCADE)";

                            command.ExecuteNonQuery(); // Execute the command to create "Transaction" table
                            Console.WriteLine("Table 'Transaction' created.");
                        }

                        transaction.Commit(); // Commit the transaction
                    }
                    connection.Close(); // Close the connection after all commands have been executed
                    Console.WriteLine("Transaction committed. All tables created successfully.");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Return false if table creation fails
            }
        }

        public static bool InsertUserProfile(UserProfile userProfile)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand pragmaCmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
                    {
                        pragmaCmd.ExecuteNonQuery();
                    }


                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {                     

                        // SQL command to insert data into the "UserProfile" table
                        command.CommandText = @"
                INSERT INTO UserProfile (name, email, phone, password)
                VALUES (@name, @email, @phone, @password)";

                        // Define parameters for the query
                        command.Parameters.AddWithValue("@name", userProfile.name);
                        command.Parameters.AddWithValue("@email", userProfile.email);
                        command.Parameters.AddWithValue("@phone", userProfile.phone);
                        command.Parameters.AddWithValue("@password", userProfile.password );

                        // Execute the SQL command to insert data
                        int rowsInserted = command.ExecuteNonQuery();

                        // Check if any rows were inserted
                        connection.Close();
                        return rowsInserted > 0; // Return true if insertion was successful
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return false; // Insertion failed
        }



        public static bool InsertAccount(Account account)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand pragmaCmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
                    {
                        pragmaCmd.ExecuteNonQuery();
                    }


                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // SQL command to insert data into the "Account" table
                        command.CommandText = @"
                INSERT INTO Account (  accNumber , userId, accType, balance, accStatus)
                VALUES (@accNumber , @userId, @accType, @balance, @accStatus )";

                        // Define parameters for the query
                        command.Parameters.AddWithValue("@accNumber", account.accNumber); 
                        command.Parameters.AddWithValue("@userId", account.userId);
                        command.Parameters.AddWithValue("@accType", account.accType);
                        command.Parameters.AddWithValue("@balance", account.accBalance);
                        command.Parameters.AddWithValue("@accStatus", account.accStatus);

                        // Execute the SQL command to insert data
                        int rowsInserted = command.ExecuteNonQuery();

                        connection.Close();
                        return rowsInserted > 0; // Return true if insertion was successful
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return false; // Insertion failed
        }




        public static bool InsertTransaction(Transact transaction)
        {
            try
            {

                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();


                    using (SQLiteCommand pragmaCmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
                    {
                        pragmaCmd.ExecuteNonQuery();
                    }

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // SQL command to insert data into the "Transaction" table
                        command.CommandText = @"
                INSERT INTO Trans (accNumber, type, amount)
                VALUES (@accNumber, @type, @amount )";

                        // Define parameters for the query
                        command.Parameters.AddWithValue("@accNumber", transaction.accNumber);
                        command.Parameters.AddWithValue("@type", transaction.type);
                        command.Parameters.AddWithValue("@amount", transaction.amount);

                        // Execute the SQL command to insert data
                        int rowsInserted = command.ExecuteNonQuery();

                        connection.Close();
                        return rowsInserted > 0; // Return true if insertion was successful
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return false; // Insertion failed
        }


        public static void DBInitialize()
        {
            if (CreateTable()) // Create tables
            {
                //add user profiles 
                UserProfile prof =  new UserProfile();
                prof.phone = "7654321";
                prof.name = "John";
                prof.email = "john@gmail.com";
                prof.password = "john_321";
                InsertUserProfile(prof);

                 prof = new UserProfile();
                prof.phone = "34567654";
                prof.name = "William";
                prof.email = "William@gmail.com";
                prof.password = "william_321";
                InsertUserProfile(prof);

                 prof = new UserProfile();
                prof.phone = "87612345";
                prof.name = "Henry";
                prof.email = "Henry@gmail.com";
                prof.password = "henry_321";
                InsertUserProfile(prof);


                //add accounts
                Account acc = new Account();
                acc.userId = 1;
                acc.accNumber = 12345;
                acc.accBalance = 3000;
                acc.accStatus = "open";
                acc.accType = "savings";
                InsertAccount(acc);

                 acc = new Account();
                acc.userId = 3;
                acc.accNumber = 54321;
                acc.accBalance = 3000;
                acc.accStatus = "restricted";
                acc.accType = "current";
                InsertAccount(acc);

                acc = new Account();
                acc.userId = 1;
                acc.accNumber = 23456;
                acc.accBalance = 1200;
                acc.accStatus = "open";
                acc.accType = "current";
                InsertAccount(acc);


                //add transactions
                Transact trans = new Transact();
                //add transactions
                trans = new Transact();
                trans.accNumber = 54321;
                trans.type = "deposit";
                trans.amount = 1500;


                //add transactions
                InsertTransaction(trans);
                trans.accNumber = 12345;
                trans.type = "withdrawl";
                trans.amount = 2000;
                InsertTransaction(trans);
            }
        }




        public static List<Account> GetAllAccounts()
        {
            List<Account> list = new List<Account>();

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select all student data
                        command.CommandText = "SELECT * FROM Account";

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Account acc = new Account();
                                acc.accNumber = Convert.ToInt32(reader["accNumber"]);
                                acc.userId =  Convert.ToInt32(reader["userId"]);
                                acc.accType = reader["accType"].ToString();
                                acc.accBalance = Convert.ToDouble(reader["balance"]);
                                    acc.accStatus = reader["accStatus"].ToString();

                                // Create a Student object and add it to the list
                                list.Add(acc);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return list;
        }

        

        public static Account GetAccById(int id) //get account by account number
        {
            
            Account acc= null;

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select a student by ID
                        command.CommandText = "SELECT * FROM Account WHERE accNumber = @ID";
                        command.Parameters.AddWithValue("@ID", id);

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                acc = new Account();
                                acc.accNumber = Convert.ToInt32(reader["accNumber"]);
                                acc.userId = Convert.ToInt32(reader["userId"]);
                                acc.accStatus =reader["accStatus"].ToString() ;
                                acc.accBalance = Convert.ToDouble(reader["balance"]);
                                acc.accType = reader["accType"].ToString();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return acc;
        }

        public static List<Account> GetAccByuserId(int id) //get accounts by user ID 
        {
            List<Account> list = new List<Account>();

            Account acc = null;

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select a student by ID
                        command.CommandText = "SELECT * FROM Account WHERE userId = @ID";
                        command.Parameters.AddWithValue("@ID", id);

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                acc = new Account();
                                acc.accNumber = Convert.ToInt32(reader["accNumber"]);
                                acc.userId = Convert.ToInt32(reader["userId"]);
                                acc.accStatus = reader["accStatus"].ToString();
                                acc.accBalance = Convert.ToDouble(reader["balance"]);
                                acc.accType = reader["accType"].ToString();

                                list.Add(acc);

                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return list;
        }


        public static bool UpdateAccount(Account acc) //update acc status, type, balance 
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {

                        // Build the SQL command to update data by ID
                        command.CommandText = $"UPDATE Account SET accType = @accType, balance = @balance, accStatus = @accStatus , userId = @userId WHERE accNumber = @accNumber";
                        command.Parameters.AddWithValue("@accType", acc.accType );
                        command.Parameters.AddWithValue("@balance", acc.accBalance );
                        command.Parameters.AddWithValue("@accStatus", acc.accStatus );
                        command.Parameters.AddWithValue("@accNumber", acc.accNumber  );
                        command.Parameters.AddWithValue("@userId", acc.userId );

                        // Execute the SQL command to update data
                        int rowsUpdated = command.ExecuteNonQuery();
                        connection.Close();
                        // Check if any rows were updated
                        if (rowsUpdated > 0)
                        {
                            return true; // Update was successful
                        }
                    }
                    connection.Close();
                }

                return false; // No rows were updated
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Update failed
            }
        }
        /*{
                "userId": 3,
                "accNumber": 22222222,
                "accType": "curernt",
                "accBalance": 1000,
                "accStatus": "open"
            }*/


        public static bool DeleteAccount(int accNumber)
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();


                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to delete data by ID
                        command.CommandText = $"DELETE FROM Account WHERE accNumber = @accNumber";
                        command.Parameters.AddWithValue("@accNumber", accNumber);

                        // Execute the SQL command to delete data
                        int rowsDeleted = command.ExecuteNonQuery();

                        // Check if any rows were deleted
                        connection.Close();
                        if (rowsDeleted > 0)
                        {
                            return true; // Deletion was successful
                        }
                    }
                    connection.Close();
                }

                return false; // No rows were deleted
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Deletion failed
            }
        }


        /*************************************************  USERS  *************************************************************/

        public static List<UserProfile> GetAllUsers()
        {
            List<UserProfile> list = new List<UserProfile>();

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select all student data
                        command.CommandText = "SELECT * FROM UserProfile";

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserProfile profile = new UserProfile();
                                profile.userId = Convert.ToInt32(reader["userId"]);
                                profile.name = reader["name"].ToString();
                                profile.email = reader["email"].ToString();
                                profile.phone = reader["phone"].ToString();
                                profile.password = reader["password"].ToString();

                                // Create a Student object and add it to the list
                                list.Add(profile);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return list;
        }

        public static UserProfile GetProfileById(int id) //get account by account number
        {
            UserProfile prof = null ;

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select a student by ID
                        command.CommandText = "SELECT * FROM UserProfile WHERE userId= @ID";
                        command.Parameters.AddWithValue("@ID", id);

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                prof = new UserProfile();
                                prof.userId = Convert.ToInt32(reader["userId"]);
                                prof.name = reader["name"].ToString();  
                                prof.email = reader["email"].ToString();
                                prof.phone = reader["phone"].ToString();
                                prof.password = reader["password"].ToString();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return prof;
        }


        public static UserProfile GetProfileByName(string name)
        {
            UserProfile prof = null;

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select a user profile by name
                        command.CommandText = "SELECT * FROM UserProfile WHERE name = @Name";
                        command.Parameters.AddWithValue("@Name", name);

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                prof = new UserProfile();
                                prof.userId = Convert.ToInt32(reader["userId"]);
                                prof.name = reader["name"].ToString();
                                prof.email = reader["email"].ToString();
                                prof.phone = reader["phone"].ToString();
                                prof.password = reader["password"].ToString();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return prof;
        }

        public static UserProfile GetProfileByEmail(string email)
        {
            UserProfile prof = null;

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select a user profile by email
                        command.CommandText = "SELECT * FROM UserProfile WHERE email = @Email";
                        command.Parameters.AddWithValue("@Email", email);

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                prof = new UserProfile();
                                prof.userId = Convert.ToInt32(reader["userId"]);
                                prof.name = reader["name"].ToString();
                                prof.email = reader["email"].ToString();
                                prof.phone = reader["phone"].ToString();
                                prof.password = reader["password"].ToString();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return prof;
        }



        public static bool UpdateUserProfile(UserProfile prof) //update 
        {
            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {

                        // Build the SQL command to update data by ID
                        command.CommandText = $"UPDATE UserProfile SET name = @name, phone = @phone, email= @email, password = @password WHERE userId = @userId";
                        command.Parameters.AddWithValue("@name", prof.name);
                        command.Parameters.AddWithValue("@phone", prof.phone); // Correct the phone parameter
                        command.Parameters.AddWithValue("@email", prof.email); // Add the email parameter
                        command.Parameters.AddWithValue("@userId", prof.userId);
                        command.Parameters.AddWithValue("@password", prof.password);

                        // Execute the SQL command to update data
                        int rowsUpdated = command.ExecuteNonQuery();
                        connection.Close();
                        // Check if any rows were updated
                        if (rowsUpdated > 0)
                        {
                            return true; // Update was successful
                        }
                    }
                    connection.Close();
                }

                return false; // No rows were updated
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Update failed
            }
        }
        /*{
    "userId" : 4,
    "name": "jenny",
    "phone" : "12344",
    "email": "jenny@gmail.com"
}*/



        public static bool DeleteUser(int userId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Enable foreign key constraints
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "PRAGMA foreign_keys = ON;";
                        command.ExecuteNonQuery();
                    }


                    using (SQLiteTransaction transaction = connection.BeginTransaction()) // Start transaction
                    {
                        using (SQLiteCommand command = connection.CreateCommand())
                        {
                            // SQL query to delete a user profile by userId
                            command.CommandText = "DELETE FROM UserProfile WHERE userId = @userId";
                            command.Parameters.AddWithValue("@userId", userId);

                            // Execute the delete query
                            int rowsDeleted = command.ExecuteNonQuery();

                            // Commit the transaction
                            transaction.Commit();

                            // Return true if any rows were deleted
                            return rowsDeleted > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }





        public static UserProfile GetProfileByAccountNumber(int accNumber)
        {
            UserProfile prof = null;

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select user profile based on account number
                        command.CommandText = @"
                    SELECT up.* 
                    FROM UserProfile up
                    JOIN Account a ON up.userId = a.userId
                    WHERE a.accNumber = @AccNumber";

                        // Define the parameter for account number
                        command.Parameters.AddWithValue("@AccNumber", accNumber);

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                prof = new UserProfile();
                                prof.userId = Convert.ToInt32(reader["userId"]);
                                prof.name = reader["name"].ToString();
                                prof.email = reader["email"].ToString();
                                prof.phone = reader["phone"].ToString();
                                prof.password = reader["password"].ToString();
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return prof;
        }







        /***************************************************  Transactions  *************************************************************/
        public static List<Transact> GetAllTransactions()
        {
            List<Transact> list = new List<Transact>();

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select all student data
                        command.CommandText = "SELECT * FROM Trans";

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Transact tran = new Transact();
                                tran.transId= Convert.ToInt32(reader["transId"]);
                                tran.accNumber = Convert.ToInt32(reader["accNumber"]);
                                tran.type = reader["type"].ToString();
                                tran.transDate = reader["transDate"].ToString();
                                tran.amount = Convert.ToDouble(reader["amount"]);

                                // Create a Student object and add it to the list
                                list.Add(tran);
                               
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return list ;
        }



        //get transactions by account number
        public static List<Transact> GetTransByAccNum(int id) 
        {
            List<Transact> list = new List<Transact>();

            Transact trans = null;

            try
            {
                // Create a new SQLite connection
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Build the SQL command to select a student by ID
                        command.CommandText = "SELECT * FROM Trans WHERE accNumber = @ID";
                        command.Parameters.AddWithValue("@ID", id);

                        // Execute the SQL command and retrieve data
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                trans = new Transact();
                                trans.transId = Convert.ToInt32(reader["transId"]);
                                trans.accNumber = Convert.ToInt32(reader["accNumber"]);
                                trans.type = reader["type"].ToString();
                                trans.amount = Convert.ToDouble(reader["amount"]);
                                trans.transDate = reader["transDate"].ToString();
                                list.Add(trans);
                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return list;
        }



        /*****************************************************Deposit / Withdrawal *********************************************************/
        public static bool Deposit(int accNumber, double amount)
        {
            try
            {
                // Get the current account details (balance)
                Account account = GetAccById(accNumber);
                if (account == null)
                {
                    Console.WriteLine("Account not found.");
                    return false;
                }

                if ( account.accStatus == "restricted")
                {
                    Console.WriteLine("Account is restricted.");
                    return false;
                }
                // Update the balance by adding the deposit amount
                account.accBalance += amount;

                // Update the account in the database
                if (UpdateAccount(account))
                {
                    // Insert the transaction into the Trans table
                    Transact transaction = new Transact
                    {
                        accNumber = accNumber,
                        type = "deposit",
                        amount = amount
                    };
                    return InsertTransaction(transaction);
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }



        public static bool Withdraw(int accNumber, double amount)
        {
            try
            {
                // Get the current account details (balance)
                Account account = GetAccById(accNumber);
                if (account == null)
                {
                    Console.WriteLine("Account not found.");
                    return false;
                }

                if (account.accStatus == "restricted")
                {
                    Console.WriteLine("Account is restricted.");
                    return false;
                }

                // Check if there's enough balance for withdrawal
                if (account.accBalance < amount)
                {
                    Console.WriteLine("Insufficient funds.");
                    return false;
                }

                // Update the balance by subtracting the withdrawal amount
                account.accBalance -= amount;

                // Update the account in the database
                if (UpdateAccount(account))
                {
                    // Insert the transaction into the Trans table
                    Transact transaction = new Transact
                    {
                        accNumber = accNumber,
                        type = "withdrawal",
                        amount = amount
                    };
                    return InsertTransaction(transaction);
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }


        public static bool Transfer(int fromAccNumber, int toAccNumber, double amount)
        {
            try
            {
                // Get the accounts for the transfer
                Account fromAccount = GetAccById(fromAccNumber);
                Account toAccount = GetAccById(toAccNumber);

                // Validate accounts
                if (fromAccount == null || toAccount == null)
                {
                    Console.WriteLine("One or both accounts not found.");
                    return false;
                }

                if (fromAccount.accStatus == "restricted" || toAccount.accStatus == "restricted")
                {
                    Console.WriteLine("Account is restricted.");
                    return false;
                }

                // Check for sufficient funds in the source account
                if (fromAccount.accBalance < amount)
                 {
                    Console.WriteLine("Insufficient balance for the transfer.");
                    return false;
                 }
                 
                 // Deduct amount from the source account
                 fromAccount.accBalance -= amount;
                 UpdateAccount(fromAccount);

                 // Add amount to the destination account
                 toAccount.accBalance += amount;
                 UpdateAccount(toAccount);

                 // Record the transactions
                 Transact fromTransaction = new Transact
                 {
                     accNumber = fromAccNumber,
                     type = "transfer_out",
                     amount = amount
                 };
                InsertTransaction(fromTransaction);
                Transact toTransaction = new Transact
                        {
                            accNumber = toAccNumber,
                            type = "transfer_in",
                            amount = amount
                        };
                        InsertTransaction(toTransaction);

                        // Commit the transaction
                        return true;
                    }
            catch (Exception ex)
            {
                Console.WriteLine("Transfer failed: " + ex.Message);
                        return false;
            }            
        }
    }
}
