using System;
using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;
using System.Linq;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> chores = new List<Chore>();

                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);
                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue
                            };

                            chores.Add(chore);
                        }
                        return chores;
                    }
                }
            }
        }
        /// Returns a single chore with the given id.
        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Chore chore = null;

                        // If we only expect a single row back from the databse, we don't need a while loop.
                        if (reader.Read())
                        {
                            chore = new Chore
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                        return chore;
                    }
                }
            }
        }
        ///add a new room to the database.  When this method is finished, we can look in the database and see the new chore.
        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name) 
                                                OUTPUT INSERTED.Id 
                                                Values (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;

                }
            }
        }
        ///updates the room
        public void Update(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                                        SET Name = @name
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.Parameters.AddWithValue("@id", chore.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// Delete the chore with the given id
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
        //WRAP THE EXECUTION IN A TRY/CATCH.  IF THERE IS ANYONE ASSIGNED TO THE CHORE, IT WILL CATCH THE EXCEPTION DUE TO NOT BEING ABLE TO DELETE WITH A FOREIGN KEY ATTACHED
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("This Room is occupied and cannot be deleted.");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                    }
                }
            }
        }
        public List<Chore> GetUnassignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM Chore 
                                        Left Join RoommateChore on RoommateChore.ChoreId = Chore.Id 
                                        where RoommateChore.RoommateId IS NULL";
                    List<Chore> unassignedChores = new List<Chore>();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Chore noAssignment = new Chore
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        unassignedChores.Add(noAssignment);
                        }
                    return unassignedChores;
                    }
                }
            }
        }
        public void AssignChore(int roommateId, int choreId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (RoommateId, ChoreId)
                                       OUTPUT INSERTED.Id
                                       VALUES (@RoommateId,@ChoreId)";
                    cmd.Parameters.AddWithValue("@RoommateId", roommateId);
                    cmd.Parameters.AddWithValue("@ChoreId", choreId);
                    int id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void ReassignChore(int roommateId, int choreId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE RoommateChore 
                                        SET RoommateId = @RoommateId,
                                        WHERE ChoreId = @ChoreId";
                    cmd.Parameters.AddWithValue("@RoommateId", roommateId);
                    cmd.Parameters.AddWithValue("@ChoreId", choreId);
                    int id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public List<Chore> GetChoreCounts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM Chore 
                                        Left Join RoommateChore on 
                                        RoommateChore.ChoreId = Chore.Id";
                    List<Chore> assignedChores = new List<Chore>();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Chore assignment = new Chore
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            assignedChores.Add(assignment);
                        }
                        return assignedChores;
                    }
                }
            }
        }
    }
}