using CodeFirst.Models;

namespace CodeFirst.Repositories;

public interface IPost
{
    List<Post> GetPosts();

    Post GetPostByID(int postId);

    void InsertPost(Post post);

    void DeletePost(int postId);

    void UpdatePost(Post post);

    void Save();
}