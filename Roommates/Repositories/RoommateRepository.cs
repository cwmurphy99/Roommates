using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roommates.Models;
using Roommates.Repositories;
using Microsoft.Data.SqlClient;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                        {
                    cmd.CommandText = "SELECT Id, firstName, lastName, roomId FROM Roommate";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> roommates = new List<Roommate>();
                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);

                            int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                            string firstNameValue = reader.GetString(firstNameColumnPosition);

                            int lastNameColumnPosition = reader.GetOrdinal("LastName");
                            string lastNameValue = reader.GetString(lastNameColumnPosition);

                            Roommate roommate = new Roommate
                            {
                                Id = idValue,
                                FirstName = firstNameValue,
                                LastName = lastNameValue,
                            };
                            roommates.Add(roommate);
                        }
                        return roommates;
                    }
                }
            }
        }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT FirstName, RentPortion, r.Name
                                        FROM Roommate rm
                                        join Room r on r.Id = rm.RoomId
                                        WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        if(reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion"))
                            };
                        }
                        return roommate;
                    }
                }
            }
        }
       
    }
}
