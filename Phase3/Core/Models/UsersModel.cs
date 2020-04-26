﻿using Phase3.Core;
using Phase3.Core.Elements;
using Phase3.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phase3.Core.Models
{

    public class UsersModel : Model
    {

        private static readonly string DEFAULT_DATA_FILE = Functions.GetDataFilePath("users");

        public bool Exists(string field = "Id", object value = null)
        {
            if (typeof(User).GetProperty(field) == null) {
                Logs.Write("The parameter « " + field + " » doesn't exist in the class « User ».");
            } else {
                Dictionary<string, object> search = new Dictionary<string, object>();
                search.Add(field, value);
                try {
                    return XML.Find<User>(DEFAULT_DATA_FILE, typeof(User), search).Count() >= 1;
                } catch (Exception e) {
                    Logs.Write(e.Message);
                }
            }
            return false;
        }

        public void AddUser(User user)
        {
            try {
                XML.Add<User>(DEFAULT_DATA_FILE, user);
            } catch (Exception e) {
                Logs.Write(e.Message);
            }
        }

        public void RemakeUsersFile(List<User> users)
        {
            try {
                XML.Create<User>(DEFAULT_DATA_FILE, users);
            } catch (Exception e) {
                Logs.Write(e.Message);
            }
        }

        public List<User> GetAll()
        {
            try {
                return XML.GetAll<User>(DEFAULT_DATA_FILE);
            } catch (Exception e) {
                Logs.Write(e.Message);
            }
            return new List<User>();
        }

        public User GetUser(string field = "Id", object value = null)
        {
            User toReturn = new User();
            if (typeof(User).GetProperty(field) == null) {
                Logs.Write("The parameter « " + field + " » doesn't exist in the class « User ».");
            } else {
                Dictionary<string, object> search = new Dictionary<string, object> {
                    { field, value }
                };
                try {
                    List<User> results = XML.Find<User>(DEFAULT_DATA_FILE, typeof(User), search);
                    if (results.Count() == 1) {
                        toReturn = results[0];
                    } else if (results.Count() > 1) {
                        List<Constraint> usersConstraints = Constraints.WakeUp().GetDataFileConstraints("users");
                        List<Constraint> usersUniqueConstraints = Constraints.WakeUp().GetConstraintsOfType(ConstraintsTypes.UNIQUE, usersConstraints);
                        foreach (Constraint constraint in usersUniqueConstraints) {
                            if (constraint.Field.Equals(field)) {
                                Logs.Write("The file « users » is corrupted. Two rows have the same value on the unique field « " + field + " ».");
                                break;
                            }
                        }
                    }
                } catch (Exception e) {
                    Logs.Write(e.Message);
                }
            }
            return toReturn;
        }

        public string GetUserProfilePicture(User user)
        {
            string profilePictures = Functions.GetSolutionDirPath() + "\\Data\\ProfilePictures";
            if (File.Exists(profilePictures + "\\" + user.Id + ".png")) {
                return profilePictures + "\\" + user.Id + ".png";
            }
            return profilePictures + "\\default.png";
        }

    }
}