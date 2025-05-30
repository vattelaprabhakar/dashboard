//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Data.EntityClient;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Web;

//namespace FortuneTechPvtLtd.DBConnections
//{
//    public class SingleConnection
//    {

//        private SingleConnection() { }

//        private static SingleConnection _ConsString = null;

//        private String _String = null;



//        public static string ConString
//        {

//            get
//            {

//                if (_ConsString == null)
//                {

//                    _ConsString = new SingleConnection { _String = SingleConnection.Connect() };

//                    return _ConsString._String;

//                }

//                else

//                    return _ConsString._String;

//            }

//        }



//        public static string Connect()
//        {

//            //Build an SQL connection string

//            SqlConnectionStringBuilder sqlString = new SqlConnectionStringBuilder()

//            {

//                DataSource = "199.79.62.22", // Server name

//                InitialCatalog = "fortuff3_crm",  //Database

//                UserID = "crm_user",         //Username

//                Password = "@@hyderabad123",  //Password

//            };



//            //Build an Entity Framework connection string

//            EntityConnectionStringBuilder entityString = new EntityConnectionStringBuilder()

//            {

//                Provider = "System.Data.SqlClient",

//                Metadata = "res://*/DataModel.UserData.csdl|res://*/DataModel.UserData.ssdl|res://*/DataModel.UserData.msl",

//                ProviderConnectionString = sqlString.ToString()

//            };

//            return entityString.ConnectionString;

//        }

//    }



//    public partial class FortuneSoftEntities : DbContext
//    {

//        public FortuneSoftEntities()
//            : base(SingleConnection.ConString)
//        {

//            //"name=efDBEntities"

//        }



//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {

//            throw new UnintentionalCodeFirstException();

//        }

//    }

//}
