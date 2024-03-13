namespace BL.Exceptions;

public class BusinessException(string message) : Exception($"Business exception: {message}");