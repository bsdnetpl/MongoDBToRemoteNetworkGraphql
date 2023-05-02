using HotChocolate.Subscriptions;
using MongoDBToRemoteNetwork.Properties.Data;

namespace MongoDBToRemoteNetwork.Properties.Interface
{
    public interface IMutation
    {
        Task<Book> AddBook(Book newBook);
        Task<Book> Update(string id, Book updatedBook);
        Task<string> Delete(string id);
        Task<Users> CreateUser(Users newUsers, [Service] ITopicEventSender topicEventSender);
        Task<string> DeleteUser(string id);
    }
}
