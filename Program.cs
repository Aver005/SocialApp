using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SocialApp
{
    static class App
    {
        private static List<User> users = new List<User>();
        private static List<Post> posts = new List<Post>();

        public static void AddPost(Post post) {posts.Add(post);}
        public static void AddUser(User user) {users.Add(user);}

        public static int GetUsersCount() { return users.Count; }
        public static int GetPostsCount() { return posts.Count; }

        public static List<User> GetUsers() {return users;}
        public static List<Post> GetPosts() {return posts;}

        static void Main()
        {
            Data.Load();
            Console.WriteLine("Users count: " + GetUsersCount());
            Console.WriteLine("Please write login: ");
            string login = Console.ReadLine();
            Console.WriteLine("Please write password: ");
            string password = Console.ReadLine();
            Console.WriteLine("Please write name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Please write surname: ");
            string surname = Console.ReadLine();
            User user = new User(login, password, name, surname, true, 20);
            Data.Save();
        }
    }

    class User
    {
        private int id;
        private string login = "";
        private string password = "";
        private string name = "";
        private string surname = "";
        private string status = "";
        private bool isMan = false;
        private int age = 0;
        private Wall wall;

        public string Login { get { return login; } set { login = value; } }
        public string Password { get { return password; } set { password = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string Surname { get { return surname; } set { surname = value; } }
        public bool IsMan { get { return isMan; } set { isMan = value; } }
        public int Age { get { return age; } set { age = value; } }
        public string Status { get { return status; } set { status = value; } }


        public User(string login, string password, string name, string surname, bool isMan, int age)
        {
            this.id = App.GetUsersCount();
            this.login = login;
            this.password = password;
            this.name = name;
            this.surname = surname;
            this.isMan = isMan;
            this.age = age;
            this.wall = new Wall(this);
            App.AddUser(this);
        }

        public void NewPost(string content)
        {
            wall.NewPost(content);
        }
    }

    class Wall
    {
        private User owner;
        private List<Post> posts = new List<Post>();

        public Wall(User user)
        {
            owner = user;
        }

        public void NewPost(string content)
        {
            Post newPost = new Post(owner, content);
            posts.Add(newPost);
        }
    }

    class Post
    {
        private int id;
        private User author;
        private DateTime sendTime;
        private string content;

        public Post(User author, string content)
        {
            this.id = App.GetPostsCount();
            this.author = author;
            this.sendTime = new DateTime();
            this.content = content;
            App.AddPost(this);
        }
    }

    static class Data
    {
        private static string directoryPath = @"C:\SocialApp\";
        private static string usersPath = directoryPath + @"Users\";
        private static string postsPath = usersPath + @"Posts\";

        public static void DirectoryCheck()
        {
            string[] paths = { directoryPath, usersPath, postsPath };

            foreach(string path in paths)
            {
                if (Directory.Exists(path)) { continue; }
                Directory.CreateDirectory(path);
            }

        }

        public static void Save()
        {
            DirectoryCheck();

            foreach(User user in App.GetUsers())
            {
                string userPath = usersPath + user.Login + ".txt";
                using (StreamWriter sw = new StreamWriter(userPath, false))
                {
                    sw.WriteLine("[" + user.Login + "]");
                    sw.WriteLine(user.Password);
                    sw.WriteLine(user.Name);
                    sw.WriteLine(user.Surname);
                    sw.WriteLine(user.Status);
                    sw.WriteLine(user.Age);
                    sw.WriteLine(user.IsMan);
                }
            }
        }

        public static void Load()
        {
            DirectoryCheck();

            foreach (string userPath in Directory.EnumerateFiles(usersPath))
            {
                using (StreamReader sr = new StreamReader(userPath))
                {
                    string login = sr.ReadLine().Replace("[", "").Replace("]", "");
                    string password = sr.ReadLine();
                    string name = sr.ReadLine();
                    string surname = sr.ReadLine();
                    string status = sr.ReadLine();
                    int age = Convert.ToInt32(sr.ReadLine());
                    bool isMan = Convert.ToBoolean(sr.ReadLine());

                    User user = new User(login, password, name, surname, isMan, age);
                }
            }
        }
    }
}