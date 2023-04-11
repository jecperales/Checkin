namespace api.checkin.data
{
    public class MySQLConfiguration
    {
        public MySQLConfiguration(string conectionString)
        {
            ConectionString = conectionString;
        }
        

        public string ConectionString { get; set; }

    }
}