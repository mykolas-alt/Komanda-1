namespace Projektas.Server.Exceptions {
    public class DatabaseOperationException : Exception {
        public string ErrorCode {get;}
        public DateTime OccurredAt {get;}

        public DatabaseOperationException(string message, Exception innerException) : base(message, innerException) {
            OccurredAt = DateTime.Now;
        }

        public DatabaseOperationException(string message,string errorCode) : base(message) {
            ErrorCode=errorCode;
            OccurredAt=DateTime.Now;
        }
    }
}
