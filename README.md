Dapper.Rainbow.SQLite
=======================

This project is a reimplementation of Dapper.Rainbow designed for SQLite. It is 
an addon that gives you basic crud operations while having to write even less sql.

Note that you will need to specify the correct System.Data.SQLite for your target
OS. if you are planning on doing any cross platform development you will need to 
use DLLMAP to map between the different sqlite3.dlls for each platform. 
More can be found at [Stack Overflow](http://stackoverflow.com/questions/14536843/including-custom-dll-and-dylib-in-monomac-app)

    class User {
      public int Id { get; set; }
      public String Email { get; set; }
      public String Password { get; set; }
      public String Name { get; set; }
    }
    
    class UserDB : Database<UserDB> {
      public Table<User> Users { get; set; }
    }
    
    class Demo {
      public void Setup(){        

        #if __MonoCS__
    	  var cn = new SqliteConnection("Data Source=:memory:;Version=3;");
        #else
  	    var cn = new SQLiteConnection("Data Source=:memory:;Version=3;");
        #endif
            

        cn.Open();
        db = db ?? Db.Init(cn, 30);

        db.Execute(@"CREATE TABLE Posts (
            id INTEGER PRIMARY KEY ASC, 
            name VARCHAR(16), 
            description VARCHAR(128),
            publish INTEGER,
            changed DATETIME
            );");

        db.Execute(@" INSERT INTO posts(name,description,changed,publish) VALUES('name','description',DATETIME('now'),1)");

        /*
          Do something interesting here
        */      
      }
    }


How it finds the tables
------------

Dapper.Rainbow.SQLite knows what table to query based on the name of the class. 
In this situation the table that Rainbow looks in is the User table. It is not
pluralized. 

API
----------
    
### Get All The Users
    IEnumerable<User> all = db.Users.All();
    
### Get A User
    User user = db.Users.Get(userId);
    User same_user = db.Users.Get(new {Id = userId});

### Delete a User 
    bool isTrue = db.Users.Delete(user);
    bool isAnotherTrue = db.Users.Delete(new {Id = userId});
  
### Get The First User
    User user = db.Users.First();
  
### Insert A User
    long uid = db.Users.Insert (
      new { Email="foolio@coolio.com", 
            Name="Foolio Coolio", 
            Password="AHashedPasswordOfLengthThirtyTwo"});

### Insert Or Update A User
    int uid = db.Users.InsertOrUpdate(user);
    
### Update
    user.Name = "Foolio Jr."
    int uid = db.Users.Update(uid, user);
    int uid = db.Users.Update(new {Id = uid}, user);
