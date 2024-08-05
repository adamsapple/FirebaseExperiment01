using Plugin.Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseExperiment01.Models.Firestore;

public class UserDocument : IFirestoreObject
{
    [FirestoreDocumentId]
    public string Id { get; private set; }

    //[FirestoreProperty("uid")]
    //public string Uid { get; private set; }

    [FirestoreProperty("nickname")]
    public string NickName { get; set; }

    [FirestoreProperty("comment")]
    public string Comment { get; set; }

    //[FirestoreProperty("gender")]
    //public string Gender { get;  set; }

    //[FirestoreProperty("age")]
    //public int Age { get; set; }

    //[FirestoreProperty("birthday")]
    //public DateTimeOffset Birhday { get; set; }

    //[FirestoreServerTimestamp("server_timestamp")]
    //public DateTimeOffset ServerTimestamp { get; private set; }

    [FirestoreProperty("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    [FirestoreProperty("lastLoginAt")]
    public DateTimeOffset LastLoginAt { get; set; }

    [FirestoreServerTimestamp("updatedAt")]
    public DateTimeOffset UpdatedAt { get; set; }

    //[FirestoreProperty("original_reference")]
    //public IDocumentReference OriginalReference { get; private set; }
}
