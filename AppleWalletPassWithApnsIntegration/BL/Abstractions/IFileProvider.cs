namespace BL.Abstractions;

public interface IFileProvider
{
    Task<byte[]> GetFileByPath(string path);
}