using MongoDBToRemoteNetwork.Properties.Data;

namespace MongoDBToRemoteNetwork.Properties.subscription
{
    public class subscription
    {
        [Subscribe]
        public Users CreateUser([EventMessage] Users users)
        {
            return users;
        }
    }
}
