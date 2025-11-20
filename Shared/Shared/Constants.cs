namespace Shared
{
    public class Constants
    {
        #region Database String Lengths
        public const int STRING_DB_MAX_LENGTH_10 = 10;
        public const int STRING_DB_MAX_LENGTH_100 = 100;
        public const int STRING_DB_MAX_LENGTH_500 = 500;
        public const int STRING_DB_MAX_LENGTH_1000 = 1000;
        #endregion

        #region Validation messages
        public const string REQUIRED_FIELD_MSG = "The field is required!";
        public const string FIELD_NO_LONGER_THAN_10 = "The field cannot be longer than 10 symbols!";
        public const string FIELD_NO_LONGER_THAN_100 = "The field cannot be longer than 100 symbols!";
        public const string FIELD_NO_LONGER_THAN_500 = "The field cannot be longer than 500 symbols!";
        public const string FIELD_NO_LONGER_THAN_1000 = "The field cannot be longer than 1000 symbols!";
        public const string FIELD_INVALID_NUMBER = "The field could not have negative value!";
        #endregion
    }
}
