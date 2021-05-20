using System;
using System.Collections.Generic;
using System.Text;
using Scala.Gebruikersbeheer.Core.Entities;
using System.Data;
using Dapper.Contrib.Extensions;
using Dapper;
using System.Linq;
using System.Data.SqlClient;

namespace Scala.Gebruikersbeheer.Core.Services
{
    public class GebruikersService
    {
        public List<Gebruiker> GetAlleGebruikers()
        {
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    List<Gebruiker> gebruikers = connection.GetAll<Gebruiker>().ToList();
                    connection.Close();
                    gebruikers = gebruikers.OrderBy(p => p.Familienaam).ThenBy(p=>p.Voornaam).ToList();
                    return gebruikers;
                }
                catch
                {
                    return null;
                }
            }
        }
        public List<Gebruiker> GetGebruikersMetToegangTotOnderdeel(Onderdeel onderdeel)
        {
            List<Gebruiker> gebruikers = new List<Gebruiker>();
            string sql = "select * from gebruikers ";
            sql += " where id in (";
            sql += "    select gebruikerId from rechten ";
            sql += "    where onderdeelId = '" + onderdeel.Id + "' )";
            sql += " order by familienaam, voornaam";
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                connection.Open();
                gebruikers = connection.Query<Gebruiker>(sql).ToList();
                connection.Close();
            }
            return gebruikers;

        }
        public List<Gebruiker> GetGebruikersZonderToegangTotOnderdeel(Onderdeel onderdeel)
        {
            List<Gebruiker> gebruikers = new List<Gebruiker>();
            string sql = "select * from gebruikers ";
            sql += " where id NOT in (";
            sql += "    select gebruikerId from rechten ";
            sql += "    where onderdeelId = '" + onderdeel.Id + "' )";
            sql += " order by familienaam, voornaam";
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                connection.Open();
                gebruikers = connection.Query<Gebruiker>(sql).ToList();
                connection.Close();
            }
            return gebruikers;
        }
        public bool IsGebruikersnaamUniek(string gebruikersnaam)
        {
            string sql;
            sql = $"select count(*) from gebruikers where gebruikersnaam = @zoeknaam ";
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                connection.Open();
                int count = connection.ExecuteScalar<int>(sql, new { zoeknaam = gebruikersnaam });
                connection.Close();
                if (count == 0)
                    return true;
                else
                    return false;
            }
        }
        public List<Onderdeel> GetOnderdelen()
        {
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    List<Onderdeel> onderdelen = connection.GetAll<Onderdeel>().ToList();
                    connection.Close();
                    onderdelen = onderdelen.OrderBy(p => p.Naam).ToList();
                    return onderdelen;
                }
                catch
                {
                    return null;
                }
            }
        }
        public List<Onderdeel> GetOnderdelenMetToegangVoorGebruiker(Gebruiker gebruiker)
        {
            List<Onderdeel> onderdelen = new List<Onderdeel>();
            string sql = "select * from onderdelen ";
            sql += " where id in (";
            sql += "   select onderdeelId from rechten ";
            sql += "   where gebruikerId = '" + gebruiker.Id + "') ";
            sql += " order by naam"; 
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                connection.Open();
                onderdelen = connection.Query<Onderdeel>(sql).ToList();
                connection.Close();
            }
            return onderdelen;

        }
        public List<Onderdeel> GetOnderdelenZonderToegangVoorGebruiker(Gebruiker gebruiker)
        {
            List<Onderdeel> onderdelen = new List<Onderdeel>();
            string sql = "select * from onderdelen ";
            sql += " where id not in (";
            sql += "   select onderdeelId from rechten ";
            sql += "   where gebruikerId = '" + gebruiker.Id + "' )";
            sql += " order by naam";
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                connection.Open();
                onderdelen = connection.Query<Onderdeel>(sql).ToList();
                connection.Close();
            }
            return onderdelen;
        }
        public bool GebruikerToevoegen(Gebruiker gebruiker)
        {
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    connection.Insert(gebruiker);
                    connection.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool GebruikerWijzigen(Gebruiker gebruiker)
        {
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    connection.Update(gebruiker);
                    connection.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool GebruikerVerwijderen(Gebruiker gebruiker)
        {
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    connection.Delete(gebruiker);
                    connection.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool KenRechtToeAanGebruiker(Gebruiker gebruiker, Onderdeel onderdeel)
        {
            Recht recht = new Recht(gebruiker.Id, onderdeel.Id);
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    connection.Insert(recht);
                    connection.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool OntneemRechtVanGebruiker(Gebruiker gebruiker, Onderdeel onderdeel)
        {
            Recht recht = new Recht(gebruiker.Id, onderdeel.Id);
            using (SqlConnection connection = new SqlConnection(Helper.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    connection.Delete(recht);
                    connection.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
