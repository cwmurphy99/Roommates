using System;
using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    /// <summary>
    ///  This class is responsible for interacting with Room data.
    ///  It inherits from the BaseRepository class so that it can use the BaseRepository's Connection property
    /// </summary>
    public class RoomRepository : BaseRepository
    {
        /// <summary>
        ///  When new RoomRepository is instantiated, pass the connection string along to the BaseRepository
        /// </summary>
        public RoomRepository(string connectionString) : base(connectionString) { }

        /// <summary>
        ///  Get a list of all Rooms in the database
        /// </summary>
        public List<Room> GetAll()
        {
            
            using (SqlConnection conn = Connection)
            {
            
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, MaxOccupancy FROM Room";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Room> rooms = new List<Room>();
                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");

                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            int maxOccupancyColumPosition = reader.GetOrdinal("MaxOccupancy");
                            int maxOccupancy = reader.GetInt32(maxOccupancyColumPosition);

                            Room room = new Room
                            {
                                Id = idValue,
                                Name = nameValue,
                                MaxOccupancy = maxOccupancy,
                            };

                            rooms.Add(room);
                        }
                        return rooms;
                    }
                }
            }
        }
        ///  Returns a single room with the given id.
        public Room GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name, MaxOccupancy FROM Room WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Room room = null;

                        // If we only expect a single row back from the database, we don't need a while loop.
                        if (reader.Read())
                        {
                            room = new Room
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
                            };
                        }
                        return room;
                    }

                }
            }
        }
        ///  Add a new room to the database.  When this method is finished we can look in the database and see the new room.
        public void Insert(Room room)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Room (Name, MaxOccupancy) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@name, @maxOccupancy)";
                    cmd.Parameters.AddWithValue("@name", room.Name);
                    cmd.Parameters.AddWithValue("@maxOccupancy", room.MaxOccupancy);
                    int id = (int)cmd.ExecuteScalar();

                    room.Id = id;
                }
            }
        }
        ///  Updates the room
        public void Update(Room room)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Room
                                    SET Name = @name,
                                        MaxOccupancy = @maxOccupancy
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", room.Name);
                    cmd.Parameters.AddWithValue("@maxOccupancy", room.MaxOccupancy);
                    cmd.Parameters.AddWithValue("@id", room.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        ///  Delete the room with the given id
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Room WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    //WRAP THE EXECUTION IN A TRY/CATCH.  IF THERE IS ANYONE ASSIGNED TO THE ROOM, IT WILL CATCH THE EXCEPTION DUE TO NOT BEING ABLE TO DELETE WITH A FOREIGN KEY ATTACHED
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


    }
}
