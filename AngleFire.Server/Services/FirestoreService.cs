using Google.Cloud.Firestore;


namespace AngleFire.Server.Services
{
    public class FirestoreService
    {
        private FirestoreDb _db;

        public FirestoreService()
        {
            _db = FirestoreDb.Create("anglefire-1");
        }
    }
}
