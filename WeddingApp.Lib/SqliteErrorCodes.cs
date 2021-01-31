namespace WeddingApp.Lib
{
    /// <summary>
    /// A (partial) collection of SQLite error codes.
    /// (Are these availble in a library?)
    /// </summary>
    public static class SqliteErrorCodes
    {
        /// <summary>
        /// Indicates that an insert was attempted which violates
        /// a unique constraint.
        /// </summary>
        public static int UniqueConstraintViolation => 19;
    }
}
