using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;
using Dapper;
using System.Data.SQLite;

namespace Test
{
    [TestFixture]
    public class Test
    {
        Db db;       

        [Test]
        public void GetTest()
        {
            var p = db.Query<Post>("SELECT * FROM posts").FirstOrDefault();
            var o = db.Posts.Get(1);            
            Assert.AreEqual(p.Name, o.Name);
        }

        [Test]
        public void InsertTest()
        {            
            var id = db.Posts.Insert(new { name = "x", description="x", changed = DateTime.Now, publish = true });
            var x = db.Posts.Get(id);
            Assert.AreEqual(id, 2);
        }

        [Test]
        public void UpdateTest()
        {
            var x = db.Posts.Update(1, new { name = "update" });
            var y = db.Posts.Get(1);
            Assert.AreEqual(y.Name, "update");
        }

        [Test]
        public void DeleteTest()
        {
            var x = db.Posts.Delete(2);
            
            Assert.AreEqual(x, true);
        }

        [Test]
        public void FirstTest()
        {
            var c = db.Posts.Get(1);
            var f = db.Posts.First();

            Assert.AreEqual(f.Name, c.Name);
        }

        [Test]
        public void InsertOrUpdateTest()
        {            
            var f = db.Posts.InsertOrUpdate(new { id = 1, name = "insertOrUpdate", });
            var c = db.Posts.Get(1);

            Assert.AreEqual(c.Name, "insertOrUpdate");
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            var cn = new SQLiteConnection("Data Source=:memory:;Version=3;");//New=true
            cn.Open();
            db = db ?? Db.Init(cn, 30);

            var r = db.Execute(@"CREATE TABLE Posts (
                id INTEGER PRIMARY KEY ASC, 
                name VARCHAR(16), 
                description VARCHAR(128),
                publish INTEGER,
                changed DATETIME
                );INSERT INTO posts(name,description,changed,publish) VALUES('name','description',DATETIME('now'),1)");                        
        }
    }

    public class Db : Database<Db>
    {
        public Table<Post> Posts { get; set; }
    }

    public class Post
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Publish { get; private set; }
        long publish { set { Publish = Convert.ToBoolean(value); } }
        public DateTime Changed { get; set; }        
    }
}
