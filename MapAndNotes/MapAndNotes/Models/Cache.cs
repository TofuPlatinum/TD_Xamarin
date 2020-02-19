using MapAndNotes.Dtos;
using MonkeyCache.SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapAndNotes.Models
{
    public class Cache
    {
        public Cache()
        {
            Barrel.ApplicationId = "DesMapetDesNotes";
        }

        public void SauvegardeLogin(LoginResult lr)
        {
            Barrel.Current.Add(key: "login", data: lr, expireIn: TimeSpan.FromDays(1));
        }

        public LoginResult GetLogin()
        {
            LoginResult res = null;

            res = Barrel.Current.Get<LoginResult>(key: "login");

            return res;
        }
    }
}
